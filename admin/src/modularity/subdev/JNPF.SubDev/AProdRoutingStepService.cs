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
using JNPF.example.Entitys.Dto.AProdRoutingStep;
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
/// 业务实现：工艺路线工序管理.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdRoutingStep", Order = 200)]
[Route("api/example/[controller]")]
public class AProdRoutingStepService : IAProdRoutingStepService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdRoutingStepEntity> _repository;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"工序\",\"field\":\"F_ProcessId\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"计价方式\",\"field\":\"F_PriceType\"},{\"value\":\"工资单价\",\"field\":\"F_WagePrice\"},{\"value\":\"标准工时\",\"field\":\"F_StdHour\"},{\"value\":\"标准工时(分)\",\"field\":\"F_StdMinute\"},{\"value\":\"标准工时(秒)\",\"field\":\"F_StdSecond\"},{\"value\":\"附件\",\"field\":\"F_Files\"},{\"value\":\"工序需外协\",\"field\":\"F_IsOutsource\"},{\"value\":\"工序需质检\",\"field\":\"F_IsQc\"},{\"value\":\"不良品需报废、返工\",\"field\":\"F_DefectHandle\"},{\"value\":\"生产任务计时\",\"field\":\"F_TaskTimer\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdRoutingStepService"/>类型的新实例.
    /// </summary>
    public AProdRoutingStepService(
        ISqlSugarRepository<AProdRoutingStepEntity> repository,
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
    /// 获取工艺路线工序管理.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .FirstAsync(it => it.F_Id.Equals(id))).Adapt<AProdRoutingStepInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取工艺路线工序管理列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdRoutingStepListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AProdRoutingStepListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.F_Id))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Select(it => new AProdRoutingStepListOutput
            {
                id = it.F_Id,
                F_ProcessId = it.F_ProcessId,
                F_WagePrice = it.F_WagePrice,
                F_StdHour = it.F_StdHour.ToString(),
                F_StdMinute = it.F_StdMinute.ToString(),
                F_StdSecond = it.F_StdSecond.ToString(),
                F_PriceType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_PriceType) && dic.DictionaryTypeId.Equals("2011642610875240448")).Select(dic => dic.FullName),
                F_IsOutsource = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsOutsource) == 1, "启用", "禁用"),
                F_IsQc = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsQc) == 1, "启用", "禁用"),
                F_DefectHandle = SqlFunc.IIF(SqlFunc.ToInt32(it.F_DefectHandle) == 1, "启用", "禁用"),
                F_TaskTimer = SqlFunc.IIF(SqlFunc.ToInt32(it.F_TaskTimer) == 1, "启用", "禁用"),
                F_Files = it.F_Files,
                F_Description = it.F_Description,
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            if(item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }

        });

        return PageResult<AProdRoutingStepListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建工艺路线工序管理.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AProdRoutingStepCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdRoutingStepEntity>();
        entity.F_Id = SnowflakeIdHelper.NextId();
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        if (await _repository.IsAnyAsync(it => it.F_ProcessId.Equals(input.F_ProcessId) && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "工序");
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 更新工艺路线工序管理.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AProdRoutingStepUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdRoutingStepEntity>();
        if (await _repository.IsAnyAsync(it => it.F_ProcessId.Equals(input.F_ProcessId)  && it.DeleteMark == null && !it.F_Id.Equals(id)))
            throw Oops.Bah(ErrorCode.COM1023, "工序");
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_ProcessId,
                it.F_WagePrice,
                it.F_StdHour,
                it.F_StdMinute,
                it.F_StdSecond,
                it.F_PriceType,
                it.F_UnitRatioProd,
                it.F_IsOutsource,
                it.F_IsQc,
                it.F_DefectHandle,
                it.F_TaskTimer,
                it.F_Files,
                it.F_Description,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除工艺路线工序管理.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.F_Id.Equals(id)).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除工艺路线工序管理.
    /// </summary>
    /// <param name="input">主键数组.</param>
    /// <returns></returns>
    [HttpPost("batchRemove")]
    [UnitOfWork]
    public async Task BatchRemove([FromBody] BatchRemoveInput input)
    {
        var entitys = await _repository.AsQueryable().In(it => it.F_Id, input.ids).Where(it => it.DeleteMark == null).ToListAsync();
        if (entitys.Count > 0)
        {
            // 批量删除工艺路线工序管理
            await _repository.AsDeleteable().In(it => it.F_Id,input.ids).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 工艺路线工序管理详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AProdRoutingStepDetailOutput
            {
                F_Id = it.F_Id,
                F_ProcessId = it.F_ProcessId,
                F_WagePrice = it.F_WagePrice,
                F_StdHour = it.F_StdHour.ToString(),
                F_StdMinute = it.F_StdMinute.ToString(),
                F_StdSecond = it.F_StdSecond.ToString(),
                F_PriceType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_PriceType) && dic.DictionaryTypeId.Equals("2011642610875240448")).Select(dic => dic.FullName),
                F_UnitRatioProd = it.F_UnitRatioProd.ToString(),
                F_IsOutsource = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsOutsource) == 1, "启用", "禁用"),
                F_IsQc = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsQc) == 1, "启用", "禁用"),
                F_DefectHandle = SqlFunc.IIF(SqlFunc.ToInt32(it.F_DefectHandle) == 1, "启用", "禁用"),
                F_TaskTimer = SqlFunc.IIF(SqlFunc.ToInt32(it.F_TaskTimer) == 1, "启用", "禁用"),
                F_Files = it.F_Files,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.F_Id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            if(item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }

        });
        return data.FirstOrDefault();
    }



    /// <summary>
    /// 根据工艺路线获取工序列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("RoutingStepList/{routingId}")]
    public async Task<dynamic> GetRoutingStepList(string routingId)
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.F_RoutingId == routingId && it.DeleteMark == null)
             .Select(it => new AProdRoutingStepListOutput
             {
                 id = it.F_Id,
                 F_ProcessId = it.F_ProcessId,
                 F_WagePrice = it.F_WagePrice,
                 F_StdHour = it.F_StdHour.ToString(),
                 F_StdMinute = it.F_StdMinute.ToString(),
                 F_StdSecond = it.F_StdSecond.ToString(),
                 F_PriceType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_PriceType) && dic.DictionaryTypeId.Equals("2011642610875240448")).Select(dic => dic.FullName),
                 F_IsOutsource = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsOutsource) == 1, "是", "否"),
                 F_IsQc = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsQc) == 1, "是", "否"),
                 F_DefectHandle = SqlFunc.IIF(SqlFunc.ToInt32(it.F_DefectHandle) == 1, "是", "否"),
                 F_TaskTimer = SqlFunc.IIF(SqlFunc.ToInt32(it.F_TaskTimer) == 1, "是", "否"),
                 F_Description = it.F_Description,
             }).MergeTable().ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

        });

        return data;
    }
}
