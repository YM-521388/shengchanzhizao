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
using JNPF.example.Entitys.Dto.AProdDefect;
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
/// 业务实现：不良品项.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdDefect", Order = 200)]
[Route("api/example/[controller]")]
public class AProdDefectService : IAProdDefectService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdDefectEntity> _repository;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"不良品项编号\",\"field\":\"F_DefectCode\"},{\"value\":\"不良品项名称\",\"field\":\"F_DefectName\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdDefectService"/>类型的新实例.
    /// </summary>
    public AProdDefectService(
        ISqlSugarRepository<AProdDefectEntity> repository,
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
    /// 获取不良品项.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new AProdDefectEntity
            {
                id = it.id,
                F_DefectCode = it.F_DefectCode,
                F_DefectName = it.F_DefectName,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AProdDefectInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取不良品项列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdDefectListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AProdDefectListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_DefectCode), it => it.F_DefectCode.Contains(input.F_DefectCode))
            .WhereIF(!string.IsNullOrEmpty(input.F_DefectName), it => it.F_DefectName.Contains(input.F_DefectName))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new AProdDefectListOutput
            {
                id = it.id,
                F_DefectCode = it.F_DefectCode,
                F_DefectName = it.F_DefectName,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        return PageResult<AProdDefectListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建不良品项.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AProdDefectCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdDefectEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        if (await _repository.IsAnyAsync(it => it.F_DefectCode.Equals(input.F_DefectCode) && it.FlowId == null && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "不良品项编号");
        if (await _repository.IsAnyAsync(it => it.F_DefectName.Equals(input.F_DefectName) && it.FlowId==null   && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "不良品项名称");

        // 自动生成编号逻辑：前缀 BLPX + yyyyMMdd，后缀 3 位序号
        if (string.IsNullOrEmpty(entity.F_DefectCode))
        {
            var prefix = "BLPX" + DateTime.Now.ToString("yyyyMMdd");

            // 查询数据库中已有相同前缀的编号
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_DefectCode != null && it.F_DefectCode.StartsWith(prefix))
                .Select(it => it.F_DefectCode)
                .ToListAsync();

            int maxSeq = 0;
            foreach (var no in existingNos)
            {
                if (no.Length > prefix.Length)
                {
                    var suffix = no.Substring(prefix.Length);
                    if (int.TryParse(suffix, out int seq))
                    {
                        if (seq > maxSeq) maxSeq = seq;
                    }
                }
            }

            var nextSeq = maxSeq + 1;
            entity.F_DefectCode = prefix + nextSeq.ToString("D3");
        }

        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 更新不良品项.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AProdDefectUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdDefectEntity>();
        if (await _repository.IsAnyAsync(it => it.F_DefectCode.Equals(input.F_DefectCode) && it.FlowId == null && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "不良品项编号");
        if (await _repository.IsAnyAsync(it => it.F_DefectName.Equals(input.F_DefectName) && it.FlowId==null   && it.DeleteMark == null && !it.id.Equals(id)))
            throw Oops.Bah(ErrorCode.COM1023, "不良品项名称");


        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);

        if (string.IsNullOrEmpty(entity.F_DefectCode))
        {
            entity.F_DefectCode = oldEntity.F_DefectCode;
        }

        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_DefectCode,
                it.F_DefectName,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除不良品项.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除不良品项.
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
            // 批量删除不良品项
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 不良品项详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AProdDefectDetailOutput
            {
                id = it.id,
                F_DefectCode = it.F_DefectCode,
                F_DefectName = it.F_DefectName,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();
        return data.FirstOrDefault();
    }
}
