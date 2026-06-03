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
using JNPF.VisualDev.Engine;
using JNPF.example.Entitys.Dto.AProdProcess;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.DatabaseAccessor;
using JNPF.Common.Dtos;
using System.Reflection.PortableExecutable;
using Aop.Api.Domain;
using System.Text.Json;
using JNPF.example.Entitys.Dto.AProdBomItem;

namespace JNPF.example;

/// <summary>
/// 业务实现：工序管理.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdProcess", Order = 200)]
[Route("api/example/[controller]")]
public class AProdProcessService : IAProdProcessService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdProcessEntity> _repository;
    private readonly ISqlSugarRepository<AProdDefectEntity> _repositoryDefect;
    private readonly ISqlSugarRepository<AProdBomItemEntity> _repositoryBomItem;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<APuStockFgItemEntity> _repositorycprkzb;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositorysp;
    private readonly ISqlSugarRepository<APuWarehouseEntity> _repositorykc;
     private readonly ISqlSugarRepository<AProdOutsourceItemEntity> _repositorywx;
    private readonly ISqlSugarRepository<APuOrderItemEntity> _repositorycgd;

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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"工序名\",\"field\":\"F_ProcessName\"},{\"value\":\"工序编号\",\"field\":\"F_ProcessCode\"},{\"value\":\"生产人员\",\"field\":\"F_ProdUserIds\"},{\"value\":\"质检人员\",\"field\":\"F_QcUserIds\"},{\"value\":\"不良品项\",\"field\":\"F_DefectIds\"},{\"value\":\"计价方式\",\"field\":\"F_PriceType\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"},{\"value\":\"启用状态\",\"field\":\"F_State\"},{\"value\":\"备注\",\"field\":\"F_Description\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdProcessService"/>类型的新实例.
    /// </summary>
    public AProdProcessService(
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<AProdBomItemEntity> repositoryBomItem,
        ISqlSugarRepository<AProdDefectEntity> repositoryDefect,
        ISqlSugarRepository<APuStockFgItemEntity> repositorycprkzb,
        ISqlSugarRepository<AProdProcessEntity> repository,
         ISqlSugarRepository<APuOrderItemEntity> repositorycgd,
         ISqlSugarRepository<APuWarehouseEntity> repositorykc,
         ISqlSugarRepository<AGoodsEntity> repositorysp,
         ISqlSugarRepository<AProdOutsourceItemEntity> repositorywx,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryInventory = repositoryInventory;
        _repositoryBomItem = repositoryBomItem;
        _repositoryDefect = repositoryDefect;
        _repository = repository;
        _repositorycgd = repositorycgd;
        _repositorykc = repositorykc;
        _repositorysp = repositorysp;
        _repositorycprkzb = repositorycprkzb;
        _repositorywx = repositorywx;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取工序管理.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new AProdProcessEntity
            {
                id = it.id,
                F_ProcessName = it.F_ProcessName,
                F_ProcessCode = it.F_ProcessCode,
                F_WorkshopId = it.F_WorkshopId,
                F_Line = it.F_Line,
                F_ReportUnit = it.F_ReportUnit,
                F_UnitRatio = it.F_UnitRatio,
                F_PriceType = it.F_PriceType,
                F_WagePrice = it.F_WagePrice,
                F_StdHour = it.F_StdHour,
                F_StdMinute = it.F_StdMinute,
                F_StdSecond = it.F_StdSecond,
                F_Files = it.F_Files,
                F_MachineId = it.F_MachineId,
                F_ProdUserIds = it.F_ProdUserIds,
                F_DefectIds = it.F_DefectIds,
                F_IsOutsource = it.F_IsOutsource,
                F_IsQc = it.F_IsQc,
                F_DefectHandle = it.F_DefectHandle,
                F_QcUserIds = it.F_QcUserIds,
                F_TaskTimer = it.F_TaskTimer,
                F_State = it.F_State,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AProdProcessInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取工序管理列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdProcessListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AProdProcessListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var F_WorkshopId = input.F_WorkshopId?.Last();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProcessName), it => it.F_ProcessName.Contains(input.F_ProcessName))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProcessCode), it => it.F_ProcessCode.Contains(input.F_ProcessCode))
            .WhereIF(!string.IsNullOrEmpty(input.F_WorkshopId?.ToString()), it => it.F_WorkshopId.Contains(F_WorkshopId))
            .WhereIF(!string.IsNullOrEmpty(input.F_Line), it => it.F_Line.Equals(input.F_Line))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProdUserIds), it => it.F_ProdUserIds.Contains(input.F_ProdUserIds))
            .WhereIF(!string.IsNullOrEmpty(input.F_DefectIds), it => it.F_DefectIds.Contains(input.F_DefectIds))
            .WhereIF(!string.IsNullOrEmpty(input.F_QcUserIds), it => it.F_QcUserIds.Contains(input.F_QcUserIds))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new AProdProcessListOutput
            {
                id = it.id,
                F_ProcessName = it.F_ProcessName,
                F_ProcessCode = it.F_ProcessCode,
                F_PriceType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_PriceType) && dic.DictionaryTypeId.Equals("2011642610875240448")).Select(dic => dic.FullName),
                F_ProdUserIds = it.F_ProdUserIds,
                F_DefectIds = it.F_DefectIds,
                F_QcUserIds = it.F_QcUserIds,
                F_State = it.F_State,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime,OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 生产人员
            if(item.F_ProdUserIds != null)
            {
                var F_ProdUserIdsUserSelect = item.F_ProdUserIds.ToObject<List<string>>();
                item.F_ProdUserIds = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_ProdUserIdsUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 不良品项ID
            if(item.F_DefectIds != null)
            {
                var F_DefectIdsData = await _dataInterfaceService.GetDynamicList("F_DefectIds", "2011648481097289728", "F_Id", "F_Name", "");
                var F_DefectIdsSelect = item.F_DefectIds.ToObject<List<string>>();
                item.F_DefectIds = string.Join(",", F_DefectIdsData.FindAll(it => F_DefectIdsSelect.Contains(it.id)).Select(s => s.fullName));
            }

            // 质检人员
            if(item.F_QcUserIds != null)
            {
                var F_QcUserIdsUserSelect = item.F_QcUserIds.ToObject<List<string>>();
                item.F_QcUserIds = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_QcUserIdsUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

        });

        var resData = data.list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<AProdProcessEntity>(new AProdProcessEntity()));
        resData = await _controlParsing.GetParsDataList(resData, "F_MachineId", "popupSelect", _userManager.TenantId, fieldList);

        data.list = resData.ToObject<List<AProdProcessListOutput>>(CommonConst.options);

        return PageResult<AProdProcessListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建工序管理.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AProdProcessCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdProcessEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdProcessEntity>(new AProdProcessEntity()));
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        if (await _repository.IsAnyAsync(it => it.F_ProcessName.Equals(input.F_ProcessName) && it.FlowId==null   && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "工序名");
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 更新工序管理.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AProdProcessUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdProcessEntity>();
        if (await _repository.IsAnyAsync(it => it.F_ProcessName.Equals(input.F_ProcessName) && it.FlowId==null   && it.DeleteMark == null && !it.id.Equals(id)))
            throw Oops.Bah(ErrorCode.COM1023, "工序名");
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_ProcessName,
                it.F_ProcessCode,
                it.F_WorkshopId,
                it.F_Line,
                it.F_ReportUnit,
                it.F_UnitRatio,
                it.F_PriceType,
                it.F_WagePrice,
                it.F_StdHour,
                it.F_StdMinute,
                it.F_StdSecond,
                it.F_Files,
                it.F_MachineId,
                it.F_ProdUserIds,
                it.F_DefectIds,
                it.F_IsOutsource,
                it.F_IsQc,
                it.F_DefectHandle,
                it.F_QcUserIds,
                it.F_TaskTimer,
                it.F_State,
                it.F_Description,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除工序管理.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除工序管理.
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
            // 批量删除工序管理
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 工序管理详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AProdProcessDetailOutput
            {
                id = it.id,
                F_ProcessName = it.F_ProcessName,
                F_ProcessCode = it.F_ProcessCode,
                F_WorkshopId = it.F_WorkshopId,
                F_Line = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Line) && dic.DictionaryTypeId.Equals("2011623836151320576")).Select(dic => dic.FullName),
                F_ReportUnit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_ReportUnit) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                F_UnitRatio = "生产1个计划数， 需要" + (it.F_UnitRatio.ToString() ?? "0"),
                F_PriceType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_PriceType) && dic.DictionaryTypeId.Equals("2011642610875240448")).Select(dic => dic.FullName),
                F_WagePrice = it.F_WagePrice,
                F_StdHour = it.F_StdHour + "小时" + it.F_StdMinute + "分钟" + it.F_StdSecond + "秒",
                F_Files = it.F_Files,
                F_MachineId = it.F_MachineId,
                F_ProdUserIds = it.F_ProdUserIds,
                F_DefectIds = it.F_DefectIds,
                F_IsOutsource = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsOutsource) == 1, "启用", "禁用"),
                F_IsQc = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsQc) == 1, "启用", "禁用"),
                F_DefectHandle = SqlFunc.IIF(SqlFunc.ToInt32(it.F_DefectHandle) == 1, "启用", "禁用"),
                F_QcUserIds = it.F_QcUserIds,
                F_TaskTimer = SqlFunc.IIF(SqlFunc.ToInt32(it.F_TaskTimer) == 1, "启用", "禁用"),
                F_State = SqlFunc.IIF(SqlFunc.ToInt32(it.F_State) == 1, "启用", "禁用"),
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            var F_WorkshopIdData = await _dataInterfaceService.GetDynamicList("F_WorkshopId", "2011621894347952128", "F_Id", "F_Name", "children");
            if(item.F_WorkshopId != null)
            {
                var F_WorkshopIdCascader = item.F_WorkshopId.ToObject<List<string>>();
                item.F_WorkshopId = string.Join("/", F_WorkshopIdData.FindAll(it => F_WorkshopIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

            if(item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }

            // 生产人员
            if(item.F_ProdUserIds != null)
            {
                var F_ProdUserIdsUserSelect = item.F_ProdUserIds.ToObject<List<string>>();
                item.F_ProdUserIds = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_ProdUserIdsUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 不良品项ID
            if(item.F_DefectIds != null)
            {
                var F_DefectIdsData = await _dataInterfaceService.GetDynamicList("F_DefectIds", "2011648481097289728", "F_Id", "F_Name", "");
                var F_DefectIdsSelect = item.F_DefectIds.ToObject<List<string>>();
                item.F_DefectIds = string.Join(",", F_DefectIdsData.FindAll(it => F_DefectIdsSelect.Contains(it.id)).Select(s => s.fullName));
            }

            // 质检人员
            if(item.F_QcUserIds != null)
            {
                var F_QcUserIdsUserSelect = item.F_QcUserIds.ToObject<List<string>>();
                item.F_QcUserIds = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_QcUserIdsUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 机台
            if (item.F_MachineId != null)
            {
                var F_MachineIdData = await _dataInterfaceService.GetDynamicList("F_MachineId", "2011729284707782656", "id", "F_MachineName", "");
                var F_MachineIdSelect = item.F_MachineId.ToObject<string[]>();
                item.F_MachineId = string.Join(",", F_MachineIdData.FindAll(it => F_MachineIdSelect.Contains(it.id)).Select(s => s.fullName));
            }

        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<AProdProcessEntity>(new AProdProcessEntity()));
        resData = await _controlParsing.GetParsDataList(resData, "F_MachineId", "popupSelect", _userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }


    /// <summary>
    /// 弹窗获取工序列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("PopupList")]
    public async Task<dynamic> GetPopupList()
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new AProdProcessListOutput
            {
                id = it.id,
                F_ProcessName = it.F_ProcessName,
                F_ProcessCode = it.F_ProcessCode,
                F_ProdUserIds = it.F_ProdUserIds,
                F_DefectIds = it.F_DefectIds,
                F_QcUserIds = it.F_QcUserIds,
                F_IsOutsource = it.F_IsOutsource,
                F_IsQc = it.F_IsQc,
                F_DefectHandle = it.F_DefectHandle,
                F_DefectHandleName = SqlFunc.IIF(SqlFunc.ToInt32(it.F_DefectHandle) == 1, "是", "否"),
                F_Description = it.F_Description,
                F_TaskTimer = SqlFunc.IIF(SqlFunc.ToInt32(it.F_TaskTimer) == 1, "是", "否"),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderBy(it => it.F_CreatorTime, OrderByType.Desc).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            // 生产人员
            if (item.F_ProdUserIds != null)
            {
                var F_ProdUserIdsUserSelect = item.F_ProdUserIds.ToObject<List<string>>();
                item.F_ProdUserIds = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_ProdUserIdsUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 不良品项ID
            if (item.F_DefectIds != null)
            {
                var F_DefectIdsData = await _dataInterfaceService.GetDynamicList("F_DefectIds", "2011648481097289728", "F_Id", "F_Name", "");
                var F_DefectIdsSelect = item.F_DefectIds.ToObject<List<string>>();
                item.F_DefectIds = string.Join(",", F_DefectIdsData.FindAll(it => F_DefectIdsSelect.Contains(it.id)).Select(s => s.fullName));
            }

            // 质检人员
            if (item.F_QcUserIds != null)
            {
                var F_QcUserIdsUserSelect = item.F_QcUserIds.ToObject<List<string>>();
                item.F_QcUserIds = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_QcUserIdsUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

        });


        return new {
            data = data
        };
    }

    /// <summary>
    /// 根据工序获取不良品项.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("DefectList")]
    public async Task<dynamic> GetDefectList(string processId)
    {
        var dataEntity = await _repository.GetFirstAsync(it => it.DeleteMark == null && it.id == processId);

        var prodList = new List<object>();
        if (dataEntity != null)
        {
            var defectList = JsonSerializer.Deserialize<List<string>>(dataEntity.F_DefectIds);
            foreach (var item in defectList)
            {
                var defectEntity = _repositoryDefect.GetFirst(it => it.DeleteMark == null && it.id == item);
                if (defectEntity != null)
                {
                    prodList.Add(new { id = defectEntity.id, fullName = defectEntity.F_DefectName });
                }
            }
        }

        return new {
            data = prodList
        };
    }

    /// <summary>
    /// 获取机台下工序信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("ProcessList")]
    public async Task<dynamic> GetProcessList([FromBody] AProdProcessListQueryInput input)
    {
        var data = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_ProcessName), it => it.F_ProcessName.Contains(input.F_ProcessName))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProcessCode), it => it.F_ProcessCode.Contains(input.F_ProcessCode))
            .WhereIF(!string.IsNullOrEmpty(input.F_Line), it => it.F_Line.Equals(input.F_Line))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProdUserIds), it => it.F_ProdUserIds.Contains(input.F_ProdUserIds))
            .WhereIF(!string.IsNullOrEmpty(input.F_DefectIds), it => it.F_DefectIds.Contains(input.F_DefectIds))
            .WhereIF(!string.IsNullOrEmpty(input.F_QcUserIds), it => it.F_QcUserIds.Contains(input.F_QcUserIds))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(it => it.DeleteMark == null && it.F_MachineId.Contains(input.machineId))
            .Where(it => it.FlowId == null)
            .Select(it => new AProdProcessListOutput
            {
                id = it.id,
                F_ProcessName = it.F_ProcessName,
                F_ProcessCode = it.F_ProcessCode,
                F_WorkshopId = it.F_WorkshopId,
                F_PriceType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_PriceType) && dic.DictionaryTypeId.Equals("2011642610875240448")).Select(dic => dic.FullName),
                F_Line = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Line) && dic.DictionaryTypeId.Equals("2011623836151320576")).Select(dic => dic.FullName),
                F_ReportUnit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_ReportUnit) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                F_UnitRatio = "生产1个计划数， 需要" + (it.F_UnitRatio.ToString() ?? "0"),
                F_ProdUserIds = it.F_ProdUserIds,
                F_WagePrice = it.F_WagePrice.ToString() + "元/小时",
                F_DefectIds = it.F_DefectIds,
                F_QcUserIds = it.F_QcUserIds,
                F_StdHour = (it.F_StdHour.ToString() ?? "0") + "小时" + (it.F_StdMinute.ToString() ?? "0") + "分钟" + (it.F_StdSecond.ToString() ?? "0") + "秒",
                F_State = it.F_State,
                F_MachineId = it.F_MachineId,
                F_IsOutsource = it.F_IsOutsource,
                F_IsQc = it.F_IsQc,
                F_DefectHandle = it.F_DefectHandle,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            var F_WorkshopIdData = await _dataInterfaceService.GetDynamicList("F_WorkshopId", "2011621894347952128", "F_Id", "F_Name", "children");
            if (item.F_WorkshopId != null)
            {
                var F_WorkshopIdCascader = item.F_WorkshopId.ToObject<List<string>>();
                item.F_WorkshopId = string.Join("/", F_WorkshopIdData.FindAll(it => F_WorkshopIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

            // 生产人员
            if (item.F_ProdUserIds != null)
            {
                var F_ProdUserIdsUserSelect = item.F_ProdUserIds.ToObject<List<string>>();
                item.F_ProdUserIds = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_ProdUserIdsUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 不良品项ID
            if (item.F_DefectIds != null)
            {
                var F_DefectIdsData = await _dataInterfaceService.GetDynamicList("F_DefectIds", "2011648481097289728", "F_Id", "F_Name", "");
                var F_DefectIdsSelect = item.F_DefectIds.ToObject<List<string>>();
                item.F_DefectIds = string.Join(",", F_DefectIdsData.FindAll(it => F_DefectIdsSelect.Contains(it.id)).Select(s => s.fullName));
            }

            // 质检人员
            if (item.F_QcUserIds != null)
            {
                var F_QcUserIdsUserSelect = item.F_QcUserIds.ToObject<List<string>>();
                item.F_QcUserIds = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_QcUserIdsUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 机台
            if (item.F_MachineId != null)
            {
                var F_MachineIdData = await _dataInterfaceService.GetDynamicList("F_MachineId", "2011729284707782656", "id", "F_MachineName", "");
                var F_MachineIdSelect = item.F_MachineId.ToObject<string[]>();
                item.F_MachineId = string.Join(",", F_MachineIdData.FindAll(it => F_MachineIdSelect.Contains(it.id)).Select(s => s.fullName));
            }
        });

        return PageResult<AProdProcessListOutput>.SqlSugarPageResult(data);
    }


    /// <summary>
    /// 根据加工单和仓库获取用料清单.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("ProcessingGoodsList/{id}/{warehouseId}")]
    public async Task<dynamic> GetProcessingGoodsList(string id,string warehouseId)
    {
        var data = await _repositoryBomItem.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_ProcessingId == id)
            .OrderBy("F_CreatorTime desc")
            .LeftJoin<AGoodsEntity>((it, g) => g.id.Equals(it.F_GoodsId) && g.DeleteMark == null)
            .Select((it, g) => new AProdBomItemListOutput
            {
                F_Id = it.F_Id,
                F_GoodsId = it.F_GoodsId,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_Unit = it.F_Unit,
                F_UnitTwo = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit)) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                F_CostPrice = g.F_CostPrice,
                F_InventoryNum = 0,
            })
            .ToListAsync();
        foreach(var item in data)
        {
            var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == item.F_GoodsId && it.F_WarehouseId == warehouseId);
            if( inventory != null)
            {
                item.F_InventoryNum += inventory.F_Num;
            }
        }

        return data;
    }

    /// <summary>
    /// 根据加工单获取物料库存列表.
    /// </summary>
    /// <param name="F_ProcessingId">加工单ID.</param>
    /// <param name="F_WarehouseId">仓库ID（可选）.</param>
    /// <returns></returns>
    [HttpPost("jiagondnsp")]
    public async Task<dynamic> GetGoodsInventoryByProcessingId(string F_ProcessingId, string F_WarehouseId)
    {
        if (string.IsNullOrWhiteSpace(F_ProcessingId))
        {
            return new { data = new List<AProdBomInventoryOutput>() };
        }

        var bomRows = await _repositoryBomItem.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_ProcessingId == F_ProcessingId)
            .OrderBy(it => it.F_SortCode)
            .Select(it => new { it.F_GoodsId, it.F_Unit })
            .ToListAsync();

        var bomGoodsIds = bomRows
            .Select(it => it.F_GoodsId)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Distinct()
            .ToList();

        var bomUnitByGoodsId = bomRows
            .Where(it => !string.IsNullOrWhiteSpace(it.F_GoodsId))
            .GroupBy(it => it.F_GoodsId!)
            .ToDictionary(g => g.Key, g => g.First().F_Unit);

        var data = await _repositoryInventory.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .WhereIF(!string.IsNullOrEmpty(F_WarehouseId), it => it.F_WarehouseId == F_WarehouseId)
            .WhereIF(bomGoodsIds.Any(), it => bomGoodsIds.Contains(it.F_GoodsId))
            .Select(it => new AProdBomInventoryOutput
            {
                F_WarehouseId = it.F_WarehouseId,
                F_WarehouseIdList = it.F_WarehouseId,
                F_Num = it.F_Num,
                F_Code = it.F_Code,
                F_GoodsId = it.F_GoodsId,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Unit_Ratio = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit_Ratio),
            })
            .ToListAsync();

        foreach (var row in data)
        {
            if (!string.IsNullOrWhiteSpace(row.F_GoodsId) && bomUnitByGoodsId.TryGetValue(row.F_GoodsId, out var unit))
                row.F_Unit = unit;
        }

        var warehouseIds = data
            .Where(it => !string.IsNullOrWhiteSpace(it.F_WarehouseId))
            .SelectMany(it =>
            {
                try
                {
                    return JsonSerializer.Deserialize<List<string>>(it.F_WarehouseId!) ?? new List<string>();
                }
                catch
                {
                    return new List<string>();
                }
            })
            .Where(it => !string.IsNullOrWhiteSpace(it))
            .Distinct()
            .ToList();

        var warehouseList = await _repositorykc.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .WhereIF(warehouseIds.Any(), it => warehouseIds.Contains(it.id))
            .Select(it => new { it.id, it.F_Name })
            .ToListAsync();

        var warehouseDict = warehouseList
            .Where(it => !string.IsNullOrWhiteSpace(it.id))
            .GroupBy(it => it.id!)
            .ToDictionary(it => it.Key, it => it.First().F_Name ?? string.Empty);

        foreach (var item in data)
        {
            if (string.IsNullOrWhiteSpace(item.F_WarehouseId)) continue;

            List<string> ids;
            try
            {
                ids = JsonSerializer.Deserialize<List<string>>(item.F_WarehouseId) ?? new List<string>();
            }
            catch
            {
                ids = new List<string>();
            }

            var names = ids
                .Where(id => !string.IsNullOrWhiteSpace(id) && warehouseDict.ContainsKey(id))
                .Select(id => warehouseDict[id])
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .ToList();

            item.F_WarehouseId = names.Any() ? string.Join("-", names) : item.F_WarehouseId;
        }

        return new {
            data = data
        };
    }

    /// <summary>
    /// 根据外协工单获取物料库存列表.
    /// </summary>
    /// <param name="F_ProcessingId">外协工单ID.</param>
    /// <param name="F_WarehouseId">仓库ID（可选）.</param>
    /// <returns></returns>
    [HttpPost("waixiednsp")]
    public async Task<dynamic> GetGoodsInventoryByOutsourceId(string F_ProcessingId, string F_WarehouseId)
    {
        if (string.IsNullOrWhiteSpace(F_ProcessingId))
        {
            return new { data = new List<AProdOutsourceInventoryOutput>() };
        }

        var outsourceRows = await _repositorywx.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_OutsourceId == F_ProcessingId)
            .Select(it => new { it.F_GoodsId, it.F_Unit })
            .ToListAsync();

        var outsourceGoodsIds = outsourceRows
            .Select(it => it.F_GoodsId)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Distinct()
            .ToList();

        var outsourceUnitByGoodsId = outsourceRows
            .Where(it => !string.IsNullOrWhiteSpace(it.F_GoodsId))
            .GroupBy(it => it.F_GoodsId!)
            .ToDictionary(g => g.Key, g => g.First().F_Unit);

        var data = await _repositoryInventory.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .WhereIF(!string.IsNullOrEmpty(F_WarehouseId), it => it.F_WarehouseId == F_WarehouseId)
            .WhereIF(outsourceGoodsIds.Any(), it => outsourceGoodsIds.Contains(it.F_GoodsId))
            .Select(it => new AProdOutsourceInventoryOutput
            {
                F_WarehouseId = it.F_WarehouseId,
                F_WarehouseIdList = it.F_WarehouseId,
                F_Num = it.F_Num,
                F_Code = it.F_Code,
                F_GoodsId = it.F_GoodsId,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Unit_Ratio = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit_Ratio),
            })
            .ToListAsync();

        foreach (var row in data)
        {
            if (!string.IsNullOrWhiteSpace(row.F_GoodsId) && outsourceUnitByGoodsId.TryGetValue(row.F_GoodsId, out var unit))
                row.F_Unit = unit;
        }

        var warehouseIds = data
            .Where(it => !string.IsNullOrWhiteSpace(it.F_WarehouseId))
            .SelectMany(it =>
            {
                var json = it.F_WarehouseId;
                return string.IsNullOrWhiteSpace(json)
                    ? new List<string>()
                    : JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            })
            .Where(it => !string.IsNullOrWhiteSpace(it))
            .Distinct()
            .ToList();

        var warehouseList = await _repositorykc.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .WhereIF(warehouseIds.Any(), it => warehouseIds.Contains(it.id))
            .Select(it => new { it.id, it.F_Name })
            .ToListAsync();

        var warehouseDict = warehouseList
            .Where(it => !string.IsNullOrWhiteSpace(it.id))
            .GroupBy(it => it.id!)
            .ToDictionary(it => it.Key, it => it.First().F_Name ?? string.Empty);

        foreach (var item in data)
        {
            if (string.IsNullOrWhiteSpace(item.F_WarehouseId)) continue;

            var json = item.F_WarehouseId;
            var ids = string.IsNullOrWhiteSpace(json)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();

            var names = ids
                .Where(id => !string.IsNullOrWhiteSpace(id) && warehouseDict.ContainsKey(id))
                .Select(id => warehouseDict[id])
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .ToList();

            item.F_WarehouseId = names.Any() ? string.Join("-", names) : item.F_WarehouseId;
        }

        return new
        {
            data = data
        };
    }

    /// <summary>
    /// 根据采购单获取物料库存列表.
    /// </summary>
    /// <param name="F_OrderId">采购单ID.</param>
    /// <param name="F_WarehouseId">仓库ID（可选）.</param>
    /// <returns></returns>
    [HttpPost("caigoudnsp")]
    public async Task<dynamic> GetGoodsInventoryByOrderId(string F_OrderId, string F_WarehouseId)
    {
        if (string.IsNullOrWhiteSpace(F_OrderId))
        {
            return new { data = new List<APuOrderInventoryOutput>() };
        }

        var orderRows = await _repositorycgd.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_OrderId == F_OrderId)
            .Select(it => new { it.F_CustomerId, it.F_Num })
            .ToListAsync();

        var orderGoodsIds = orderRows
            .Select(it => it.F_CustomerId)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Distinct()
            .ToList();

        var orderUnitByGoodsId = orderRows
            .Where(it => !string.IsNullOrWhiteSpace(it.F_CustomerId))
            .GroupBy(it => it.F_CustomerId!)
            .ToDictionary(g => g.Key, g => g.First().F_Num);

        var data = await _repositoryInventory.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .WhereIF(!string.IsNullOrEmpty(F_WarehouseId), it => it.F_WarehouseId == F_WarehouseId)
            .WhereIF(orderGoodsIds.Any(), it => orderGoodsIds.Contains(it.F_GoodsId))
            .Select(it => new APuOrderInventoryOutput
            {
                F_WarehouseId = it.F_WarehouseId,
                F_WarehouseIdList = it.F_WarehouseId,
                F_Num = it.F_Num,
                F_Code = it.F_Code,
                F_GoodsId = it.F_GoodsId,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Unit_Ratio = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit_Ratio),
            })
            .ToListAsync();

        foreach (var row in data)
        {
            if (!string.IsNullOrWhiteSpace(row.F_GoodsId) && orderUnitByGoodsId.TryGetValue(row.F_GoodsId, out var orderNum))
                row.F_OrderNum = orderNum;
        }

        var warehouseIds = data
            .Where(it => !string.IsNullOrWhiteSpace(it.F_WarehouseId))
            .SelectMany(it =>
            {
                var json = it.F_WarehouseId;
                return string.IsNullOrWhiteSpace(json)
                    ? new List<string>()
                    : JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            })
            .Where(it => !string.IsNullOrWhiteSpace(it))
            .Distinct()
            .ToList();

        var warehouseList = await _repositorykc.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .WhereIF(warehouseIds.Any(), it => warehouseIds.Contains(it.id))
            .Select(it => new { it.id, it.F_Name })
            .ToListAsync();

        var warehouseDict = warehouseList
            .Where(it => !string.IsNullOrWhiteSpace(it.id))
            .GroupBy(it => it.id!)
            .ToDictionary(it => it.Key, it => it.First().F_Name ?? string.Empty);

        foreach (var item in data)
        {
            if (string.IsNullOrWhiteSpace(item.F_WarehouseId)) continue;

            var json = item.F_WarehouseId;
            var ids = string.IsNullOrWhiteSpace(json)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();

            var names = ids
                .Where(id => !string.IsNullOrWhiteSpace(id) && warehouseDict.ContainsKey(id))
                .Select(id => warehouseDict[id])
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .ToList();

            item.F_WarehouseId = names.Any() ? string.Join("-", names) : item.F_WarehouseId;
        }

        return new
        {
            data = data
        };
    }
}
