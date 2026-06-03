using JNPF.Common.Core.Manager;
using JNPF.Engine.Entity.Model;
using JNPF.ClayObject;
using JNPF.Common.Models.NPOI;
using JNPF.Common.CodeGen.ExportImport;
using JNPF.Common.Core.Manager.Files;
using JNPF.Common.Dtos;
using JNPF.Common.CodeGen.DataParsing;
using JNPF.Common.Const;
using JNPF.Common.Manager;
using JNPF.Common.Enums;
using JNPF.Common.Extension;
using JNPF.Common.Filter;
using JNPF.Common.Models;
using JNPF.Common.Security;
using JNPF.DependencyInjection;
using JNPF.DynamicApiController;
using JNPF.FriendlyException;
using JNPF.Systems.Entitys.Permission;
using JNPF.Systems.Entitys.System;
using JNPF.Systems.Interfaces.System;
using JNPF.Common.Dtos.Datainterface;
using JNPF.example.Entitys.Dto.AMaintDeviceQr;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.DatabaseAccessor;
using JNPF.Common.Dtos;

namespace JNPF.example;

/// <summary>
/// 业务实现：设备二维码.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AMaintDeviceQr", Order = 200)]
[Route("api/example/[controller]")]
public class AMaintDeviceQrService : IAMaintDeviceQrService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AMaintDeviceQrEntity> _repository;


    /// <summary>
    /// 数据接口服务.
    /// </summary>
    private readonly IDataInterfaceService _dataInterfaceService;

    /// <summary>
    /// 缓存管理.
    /// </summary>
    private readonly ICacheManager _cacheManager;

    /// <summary>
    /// 通用数据解析.
    /// </summary>
    private readonly ControlParsing _controlParsing;

    /// <summary>
    /// 用户管理.
    /// </summary>
    private readonly IUserManager _userManager;

    /// <summary>
    /// 代码生成导出数据帮助类.
    /// </summary>
    private readonly ExportImportDataHelper _exportImportDataHelper;

    /// <summary>
    /// 文件服务.
    /// </summary>
    private readonly IFileManager _fileManager;


    /// <summary>
    /// 导出字段.
    /// </summary>
    private readonly List<ParamsModel> paramList = "[{\"value\":\"二维码编号\",\"field\":\"F_Code\"},{\"value\":\"生成用户\",\"field\":\"F_CreatorUserId\"},{\"value\":\"生成时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AMaintDeviceQrService"/>类型的新实例.
    /// </summary>
    public AMaintDeviceQrService(
        ISqlSugarRepository<AMaintDeviceQrEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repository = repository;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取设备二维码.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new AMaintDeviceQrEntity
            {
                id = it.id,
                F_Code = it.F_Code,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AMaintDeviceQrInfoOutput>();

        return data;
    }

    /// <summary>
    /// 获取设备二维码列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AMaintDeviceQrListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AMaintDeviceQrListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_Code), it => it.F_Code.Contains(input.F_Code))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .LeftJoin<AMaintDeviceEntity>((it, d) => d.F_DeviceQrCode.Equals(it.F_Code) && d.DeleteMark == null)
            .Select((it, d) => new AMaintDeviceQrListOutput
            {
                id = it.id,
                F_Code = it.F_Code,
                F_DeviceNo = SqlFunc.IsNull(d.F_DeviceNo, "未绑定"),
                F_DeviceName = SqlFunc.IsNull(d.F_DeviceName, "未绑定"),
                F_DeviceSpec = SqlFunc.IF(!string.IsNullOrEmpty(d.F_DeviceName)).Return(d.F_DeviceSpec).End("未绑定"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_Code, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        return PageResult<AMaintDeviceQrListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建设备二维码.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AMaintDeviceQrCrInput input)
    {
        // 1. 先拿起始编号
        long? maxVal = await _repository.AsQueryable()
                                .Where(it => it.F_Code != null && it.F_Code != "" && it.DeleteMark == null)
                                .MaxAsync(it => SqlFunc.ToInt64(it.F_Code));
        long? start = maxVal > 0 ? maxVal + 1 : 1000000L;

        // 2. 构造带连续编号的实体列表
        var list = Enumerable.Range(0, input.F_Num.Value)
            .Select(offset =>
            {
                var entity = input.Adapt<AMaintDeviceQrEntity>();
                entity.id = SnowflakeIdHelper.NextId();
                entity.F_CreatorUserId = _userManager.UserId;
                entity.F_CreatorTime = DateTime.Now;
                entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
                entity.F_Code = (start + offset).ToString();
                return entity;
            }).ToList();

        // 3. 一次性插入
        var rows = await _repository.AsInsertable(list).ExecuteCommandAsync();
        if (rows != input.F_Num) throw Oops.Oh(ErrorCode.COM1000);
    }


    /// <summary>
    /// 获取设备二维码无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(AMaintDeviceQrListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AMaintDeviceQrListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var list = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_Code), it => it.F_Code.Contains(input.F_Code))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new AMaintDeviceQrListOutput
            {
                id = it.id,
                F_Code = it.F_Code,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToListAsync();

        return list;
    }

    /// <summary>
    /// 导出设备二维码.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] AMaintDeviceQrListQueryInput input)
    {
        var exportData = new List<AMaintDeviceQrListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<AMaintDeviceQrListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName, input.selectKey, _userManager.UserId, exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新设备二维码.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AMaintDeviceQrUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AMaintDeviceQrEntity>();
        var isOk = 0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new {
                it.F_Code,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除设备二维码.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除设备二维码.
    /// </summary>
    /// <param name="input">主键数组.</param>
    /// <returns></returns>
    [HttpPost("batchRemove")]
    [UnitOfWork]
    public async Task BatchRemove([FromBody] BatchRemoveInput input)
    {
        var entitys = await _repository.AsQueryable().In(it => it.id, input.ids).Where(it => it.DeleteMark == null).ToListAsync();
        if (entitys.Count > 0)
        {
            // 批量删除设备二维码
            await _repository.AsDeleteable().In(it => it.id, input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 设备二维码详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AMaintDeviceQrDetailOutput
            {
                id = it.id,
                F_Code = it.F_Code,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();
        return data.FirstOrDefault();
    }
}
