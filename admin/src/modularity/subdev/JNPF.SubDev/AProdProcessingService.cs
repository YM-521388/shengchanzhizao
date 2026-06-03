using JNPF.Common.Core.Manager;
using JNPF.Common.Models;
using JNPF.Engine.Entity.Model;
using JNPF.ClayObject;
using JNPF.Common.Models.NPOI;
using JNPF.Common.CodeGen.ExportImport;
using JNPF.Common.Core.Manager.Files;
using JNPF.Common.Dtos;
using JNPF.Common.CodeGen.DataParsing;
using JNPF.Common.Manager;
using JNPF.Common.Const;
using JNPF.Common.Enums;
using JNPF.Common.Extension;
using JNPF.Common.Filter;
using JNPF.Common.Security;
using JNPF.DatabaseAccessor;
using JNPF.DependencyInjection;
using JNPF.DynamicApiController;
using JNPF.FriendlyException;
using JNPF.Systems.Entitys.System;
using JNPF.Systems.Entitys.Permission;
using JNPF.Systems.Interfaces.System;
using JNPF.Common.Dtos.Datainterface;
using JNPF.VisualDev.Engine;
using JNPF.example.Entitys.Dto.AProdProcessing;
using JNPF.example.Entitys.Dto.AProdBomItem;
using JNPF.example.Entitys.Dto.AProdTask;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
using JNPF.example.Entitys.Dto.AProdTaskItem;
using Minio.DataModel;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Reactive;

namespace JNPF.example;

/// <summary>
/// 业务实现：a_prod_processing.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdProcessing", Order = 200)]
[Route("api/example/[controller]")]
public class AProdProcessingService : IAProdProcessingService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdProcessingEntity> _repository;
    private readonly ISqlSugarRepository<AProdTaskEntity> _repositoryTask;
    private readonly ISqlSugarRepository<AProdTaskItemEntity> _repositoryTaskItem;
    private readonly ISqlSugarRepository<AProdBomItemEntity> _repositoryBomItem;
    private readonly ISqlSugarRepository<AProdProcessingLogEntity> _repositoryLog;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<APuStockFgItemEntity> _repositoryStockFgItem;
    private readonly ISqlSugarRepository<AProdRoutingEntity> _repositoryRouting;
    private readonly ISqlSugarRepository<AProdRoutingStepEntity> _repositoryRoutingStep;
    private readonly ISqlSugarRepository<AProdRoutingStepItemEntity> _repositoryRoutingStepItem;
    private readonly ISqlSugarRepository<AProdProcessEntity> _repositoryProcess;
    private readonly ISqlSugarRepository<AProdReportEntity> _repositoryReport;
    private readonly ISqlSugarRepository<AGoodsCategoryEntity> _repositorysp;
    private readonly ISqlSugarRepository<DictionaryDataEntity> _repositoryspDictionaryData;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"排单序号\",\"field\":\"F_SequenceNo\"},{\"value\":\"加工单号\",\"field\":\"F_ProcessNo\"},{\"value\":\"商品\",\"field\":\"F_GoodsId\"},{\"value\":\"计划数\",\"field\":\"F_PlanQty\"},{\"value\":\"状态\",\"field\":\"F_State\"},{\"value\":\"交货日期\",\"field\":\"F_DeliveryDate\"},{\"value\":\"优先级\",\"field\":\"F_Priority\"},{\"value\":\"计划开始\",\"field\":\"F_PlanStartDate\"},{\"value\":\"计划结束\",\"field\":\"F_PlanEndDate\"},{\"value\":\"图纸\",\"field\":\"F_DrawingFile\"},{\"value\":\"客户\",\"field\":\"F_CustomerName\"},{\"value\":\"锁具\",\"field\":\"F_LockSet\"},{\"value\":\"铰链\",\"field\":\"F_Hinge\"},{\"value\":\"颜色\",\"field\":\"F_Color\"},{\"value\":\"类别\",\"field\":\"F_Type\"},{\"value\":\"BOM图片\",\"field\":\"F_BOMImages\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"},{\"value\":\"合同\",\"field\":\"F_ContractId\"},{\"value\":\"门板厚度\",\"field\":\"F_DoorPlateThickness\"},{\"value\":\"箱体板厚\",\"field\":\"F_BoxPlateThickness\"},{\"value\":\"安装板或安装梁\",\"field\":\"F_InstallOrBeam\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdProcessingService"/>类型的新实例.
    /// </summary>
    public AProdProcessingService(
        ISqlSugarRepository<DictionaryDataEntity> repositoryspDictionaryData,
        ISqlSugarRepository<AProdReportEntity> repositoryReport,
        ISqlSugarRepository<AProdProcessEntity> repositoryProcess,
        ISqlSugarRepository<AProdRoutingStepItemEntity> repositoryRoutingStepItem,
        ISqlSugarRepository<AProdRoutingStepEntity> repositoryRoutingStep,
        ISqlSugarRepository<AProdRoutingEntity> repositoryRouting,
        ISqlSugarRepository<APuStockFgItemEntity> repositoryStockFgItem,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<AProdProcessingLogEntity> repositoryLog,
        ISqlSugarRepository<AProdBomItemEntity> repositoryBomItem,
        ISqlSugarRepository<AProdTaskEntity> repositoryTask,
        ISqlSugarRepository<AProdTaskItemEntity> repositoryTaskItem,
        ISqlSugarRepository<AProdProcessingEntity> repository,
         ISqlSugarRepository<AGoodsCategoryEntity> repositorysp,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryspDictionaryData = repositoryspDictionaryData;
        _repositoryReport = repositoryReport;
        _repositoryProcess = repositoryProcess;
        _repositoryRoutingStepItem = repositoryRoutingStepItem;
        _repositoryRoutingStep = repositoryRoutingStep;
        _repositoryRouting = repositoryRouting;
        _repositoryStockFgItem = repositoryStockFgItem;
        _repositoryInventory = repositoryInventory;
        _repositoryLog = repositoryLog;
        _repositoryBomItem = repositoryBomItem;
        _repositoryTask = repositoryTask;
        _repositoryTaskItem = repositoryTaskItem;
        _repository = repository;
        _repositorysp = repositorysp;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_prod_processing.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.AProdBomItemList)
            .Includes(it => it.AProdTaskList)
            .Select(it => new AProdProcessingEntity
            {
                id = it.id,
                F_ContractId = it.F_ContractId,
                F_ProjectId = it.F_ProjectId,
                F_GoodsId = it.F_GoodsId,
                F_BOMId = it.F_BOMId,
                F_RoutingId = it.F_RoutingId,
                F_ProdPlanId = it.F_ProdPlanId,
                F_ProcessNo = it.F_ProcessNo,
                F_PlanQty = it.F_PlanQty,
                F_DeliveryDate = it.F_DeliveryDate,
                F_Priority = it.F_Priority,
                F_PlanStartDate = it.F_PlanStartDate,
                F_PlanEndDate = it.F_PlanEndDate,
                F_DrawingFile = it.F_DrawingFile,
                F_CustomerName = it.F_CustomerName,
                F_DoorPlateThickness = it.F_DoorPlateThickness,
                F_BoxPlateThickness = it.F_BoxPlateThickness,
                F_InstallOrBeam = it.F_InstallOrBeam,
                F_LockSet = it.F_LockSet,
                F_Hinge = it.F_Hinge,
                F_Color = it.F_Color,
                F_Type = it.F_Type,
                F_BOMImages = it.F_BOMImages,
                F_State = it.F_State,
                F_SequenceNo = it.F_SequenceNo,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                AProdBomItemList = it.AProdBomItemList,
                AProdTaskList = it.AProdTaskList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AProdProcessingInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField449e6c, async aProdBomItem =>
        {
            // 创建时间
            aProdBomItem.F_CreatorTime = aProdBomItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            var unit = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(it => it.id.Equals(aProdBomItem.F_GoodsId)).Select(it => it.F_Unit).FirstAsync();

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(unit))
            {
                var F_UnitIdCascader = unit.ToObject<List<string>>();
                aProdBomItem.F_UnitTwo = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));


                if (F_UnitIdCascader.Count > 1)
                {
                    aProdBomItem.F_NumUnit = (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    aProdBomItem.F_NumUnit = (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }

            aProdBomItem.F_InventoryNum = (await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == aProdBomItem.F_GoodsId && it.F_Num > 0).SumAsync(it => it.F_Num)) ?? 0;
        });
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField0c720e, async aProdTask =>
        {
            // 创建时间
            aProdTask.F_CreatorTime = aProdTask.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            aProdTask.tableField0bb944 = await _repositoryTaskItem.AsQueryable().Where(it => it.F_TaskId == aProdTask.id).Select(it => new AProdTaskItemInfoOutput
            {
                F_Id = it.F_Id,
                F_GoodsId = it.F_GoodsId,
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_Num = it.F_Num,
                F_Unit = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit),
                F_InventoryNum = SqlFunc.Subqueryable<AGoodsInventoryEntity>().EnableTableFilter().Where(dic => dic.F_GoodsId.Equals(it.F_GoodsId) && dic.DeleteMark == null && dic.F_Num > 0).Sum(it => it.F_Num) ?? 0,
                F_Description = it.F_Description,
            }).ToListAsync();
            foreach(var aProdTaskItem in aProdTask.tableField0bb944)
            {
                var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
                if (!string.IsNullOrEmpty(aProdTaskItem.F_Unit))
                {
                    var F_UnitIdCascader = aProdTaskItem.F_Unit.ToObject<List<string>>();
                    aProdTaskItem.F_Unit = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));
                }
            }
            
        });
        return data;
    }

    /// <summary>
    /// 获取a_prod_processing列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdProcessingListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aProdBomItemAuthorizeWhere = new List<IConditionalModel>();
        var aProdTaskAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<AProdProcessingListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_prod_processing"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_prod_bom_item"))) aProdBomItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_prod_bom_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_prod_task"))) aProdTaskAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_prod_task"))?.conditionalModel;
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var startF_PlanQty = input.F_PlanQty?.FirstOrDefault()?.ParseToDecimal() == null ? decimal.MinValue : input.F_PlanQty?.First();
        var endF_PlanQty = input.F_PlanQty?.LastOrDefault()?.ParseToDecimal() == null ? decimal.MaxValue : input.F_PlanQty?.Last();
        var F_Type = input.F_Type?.Last();
        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AProdProcessingEntity>();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AProdProcessingEntity>();
        var F_CustomerNameDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_CustomerName").DbColumnName;

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContractId), it => it.F_ContractId.Equals(input.F_ContractId))
            .WhereIF(!string.IsNullOrEmpty(input.F_GoodstId), it => it.F_GoodsId.Equals(input.F_GoodstId))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProjectId), it => it.F_ProjectId.Equals(input.F_ProjectId))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProdPlanId), it => it.F_ProdPlanId.Equals(input.F_ProdPlanId))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProcessNo), it => it.F_ProcessNo.Contains(input.F_ProcessNo))
            .WhereIF(input.F_PlanQty?.Count() > 0, it => SqlFunc.Between(it.F_PlanQty, startF_PlanQty, endF_PlanQty))
            .WhereIF(input.F_DeliveryDate?.Count() > 0, it => SqlFunc.Between(it.F_DeliveryDate, input.F_DeliveryDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_DeliveryDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_Priority), it => it.F_Priority.Equals(input.F_Priority))
            .WhereIF(input.F_PlanStartDate?.Count() > 0, it => SqlFunc.Between(it.F_PlanStartDate, input.F_PlanStartDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_PlanStartDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(input.F_PlanEndDate?.Count() > 0, it => SqlFunc.Between(it.F_PlanEndDate, input.F_PlanEndDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_PlanEndDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_CustomerNameDbColumnName, input.F_CustomerName))
            .WhereIF(!string.IsNullOrEmpty(input.F_DoorPlateThickness), it => it.F_DoorPlateThickness.Equals(input.F_DoorPlateThickness))
            .WhereIF(!string.IsNullOrEmpty(input.F_BoxPlateThickness), it => it.F_BoxPlateThickness.Equals(input.F_BoxPlateThickness))
            .WhereIF(!string.IsNullOrEmpty(input.F_InstallOrBeam), it => it.F_InstallOrBeam.Equals(input.F_InstallOrBeam))
            .WhereIF(!string.IsNullOrEmpty(input.F_LockSet), it => it.F_LockSet.Contains(input.F_LockSet))
            .WhereIF(!string.IsNullOrEmpty(input.F_Hinge), it => it.F_Hinge.Contains(input.F_Hinge))
            .WhereIF(!string.IsNullOrEmpty(input.F_Color), it => it.F_Color.Contains(input.F_Color))
            .WhereIF(!string.IsNullOrEmpty(input.F_Type?.ToString()), it => it.F_Type.Contains(F_Type))
            .WhereIF(!string.IsNullOrEmpty(input.F_State), it => it.F_State.Equals(input.F_State))
            .WhereIF(!string.IsNullOrEmpty(input.F_Description), it => it.F_Description.Contains(input.F_Description))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .Where(authorizeWhere)
            .WhereIF(aProdBomItemAuthorizeWhere?.Count > 0, it => it.AProdBomItemList.Any(aProdBomItemAuthorizeWhere))
            .WhereIF(aProdTaskAuthorizeWhere?.Count > 0, it => it.AProdTaskList.Any(aProdTaskAuthorizeWhere))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new AProdProcessingListOutput
            {
                id = it.id,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_SequenceNo = it.F_SequenceNo.ToString(),
            })
            //把多表结果变成“单表”再排序：
            .MergeTable()
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .OrderBy(it => it.F_SequenceNo, OrderByType.Desc)
            .ToPagedListAsync(input.currentPage, input.pageSize);

        var ids = idsQ.list.Select(x => x.id).ToList();   // 20 个 long

        // 2. 再用 ids 一次性补全字段（此时只有 20 行，随便 join 18 张表都毫秒级）
        var data = await _repository.AsQueryable()
            .Where(it => ids.Contains(it.id))
            .Select(it => new AProdProcessingListOutput
            {
                id = it.id,
                F_ContractId = SqlFunc.Subqueryable<ACtcContractEntity>().EnableTableFilter().Where(c => c.id == it.F_ContractId && c.DeleteMark == null).Select(c => c.F_ContractCode),
                F_Source = !string.IsNullOrEmpty(it.F_ContractId) ? "合同" : "其他",
                F_GoodsId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => SqlFunc.MergeString(g.F_GoodsName, "-", g.F_GoodsNo)),
                F_ProcessNo = it.F_ProcessNo,
                F_PlanQty = it.F_PlanQty.ToString(),
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_Priority = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Priority) && dic.DictionaryTypeId.Equals("2013182853290004480")).Select(dic => dic.FullName),
                F_PriorityCode = it.F_Priority,
                F_PlanStartDate = it.F_PlanStartDate.Value.ToString("yyyy-MM-dd"),
                F_PlanEndDate = it.F_PlanEndDate.Value.ToString("yyyy-MM-dd"),
                F_DrawingFile = it.F_DrawingFile,
                F_CustomerName = SqlFunc.Subqueryable<ACustomerEntity>().EnableTableFilter().Where(c => c.id == it.F_CustomerName && c.DeleteMark == null).Select(c => c.F_CustomerName),
                F_DoorPlateThickness = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_DoorPlateThickness) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_BoxPlateThickness = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_BoxPlateThickness) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_InstallOrBeam = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_InstallOrBeam) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_LockSet = it.F_LockSet,
                F_Hinge = it.F_Hinge,
                F_Color = it.F_Color,
                F_Type = it.F_Type,
                F_BOMImages = it.F_BOMImages,
                F_State = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_State) && dic.DictionaryTypeId.Equals("2013819135649255424")).Select(dic => dic.FullName),
                F_StateId = it.F_State,
                F_SequenceNo = it.F_SequenceNo.ToString(),
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable()
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc).OrderBy(it => it.F_SequenceNo, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            if (item.F_DrawingFile != null)
            {
                item.F_DrawingFile = item.F_DrawingFile.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_DrawingFile = new List<FileControlsModel>();
            }

            // 类别
            var F_TypeData = await _dataInterfaceService.GetDynamicList("F_Type", "2008414575283802112", "F_Id", "F_Name", "children");
            if (item.F_Type != null)
            {
                var F_TypeCascader = item.F_Type.ToObject<List<string>>();
                item.F_Type = string.Join("/", F_TypeData.FindAll(it => F_TypeCascader.Contains(it.id)).Select(s => s.fullName));
            }

            if (item.F_BOMImages != null)
            {
                item.F_BOMImages = item.F_BOMImages.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_BOMImages = new List<FileControlsModel>();
            }
        });

        data.pagination = idsQ.pagination;

        return PageResult<AProdProcessingListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建（包含生成 生产任务）.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] AProdProcessingCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdProcessingEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdProcessingEntity>(new AProdProcessingEntity()));
        ConfigModel tableField449e6cConfig = new ConfigModel
        {
            tableName = "a_prod_bom_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "用料清单",
            children = ExportImportDataHelper.GetTemplateParsing<AProdBomItemEntity>(new AProdBomItemEntity())
        };
        FieldsModel tableField449e6cModel = new FieldsModel()
        {
            __config__ = tableField449e6cConfig,
            __vModel__ = "tableField449e6c"
        };
        fieldList.Add(tableField449e6cModel);
        
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_State = "1";
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        var processNo = await _repository.AsQueryable()
            .Where(it => it.F_SequenceNo != null && it.DeleteMark == null)
            .MaxAsync(it => it.F_SequenceNo);

        entity.F_SequenceNo = (processNo ?? 0) + 1;

        if (await _repository.IsAnyAsync(it => it.F_ProcessNo.Equals(input.F_ProcessNo) && it.FlowId == null && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "加工单号");

        // 自动生成编号逻辑：前缀 JG + yyyyMMdd，后缀 3 位序号
        if (string.IsNullOrEmpty(entity.F_ProcessNo))
        {
            var prefix = "JG" + DateTime.Now.ToString("yyyyMMdd");

            // 查询数据库中已有相同前缀的编号
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_ProcessNo != null && it.F_ProcessNo.StartsWith(prefix))
                .Select(it => it.F_ProcessNo)
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
            entity.F_ProcessNo = prefix + nextSeq.ToString("D3");
        }

        //赋值 工艺路线的XML
        var routingEntity = await _repositoryRouting.GetFirstAsync(it => it.DeleteMark == null && it.id == entity.F_RoutingId);
        entity.F_XML = routingEntity.F_XML;

        // 插入主表（确保 F_SequenceNo 被保存）
        await _repository.AsSugarClient().Insertable(entity).ExecuteCommandAsync();

        // 插入用料清单
        var aProdBomItemEntityList = input.tableField449e6c.Adapt<List<AProdBomItemEntity>>();
        if (aProdBomItemEntityList != null && aProdBomItemEntityList.Count > 0)
        {
            foreach (var item in aProdBomItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_ProcessingId = entity.id;
                item.F_CreatorTime = DateTime.Now;
            }
            await _repository.AsSugarClient().Insertable(aProdBomItemEntityList).ExecuteCommandAsync();
        }

        // 记录日志
        var logEntity = new AProdProcessingLogEntity
        {
            F_CreatorTime = DateTime.Now,
            F_CreatorUserId = _userManager.UserId,
            id = SnowflakeIdHelper.NextId(),
            F_ProcessingId = entity.id,
            F_Title = "新增加工单",
            F_Content = "加工单编号：" + entity.F_ProcessNo
        };
        await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();

        // 重新压号（连续 1,2,3...）- 确保新数据有序号
        await ReorderWholeTableAsync();

        // 生成生产任务（从工艺路线获取）
        // 1. 查询工艺路线的所有节点
        var routingStepList = await _repositoryRoutingStep.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_RoutingId == entity.F_RoutingId)
            .Select(it => new
            {
                F_Id = it.F_Id,
                F_ProcessId = it.F_ProcessId,
                F_ParentId = it.F_ParentId,
                F_NodeId = it.F_NodeId,
                F_NodeName = it.F_NodeName,
                F_Type = it.F_Type,
                F_ResponsibleUserId = it.F_ResponsibleUserId,
                F_SortCode = it.F_SortCode,
            })
            .ToListAsync();

        if (routingStepList.Count == 0) return;

        // 2. 批量查询所有工序节点的物料信息
        var stepIds = routingStepList.Select(s => s.F_Id).ToList();
        var allStepItems = await _repositoryRoutingStepItem.AsQueryable()
            .Where(it => stepIds.Contains(it.F_StepId))
            .Select(it => new
            {
                F_StepId = it.F_StepId,
                F_GoodsId = it.F_GoodsId,
                F_Num = it.F_Num,
            })
            .ToListAsync();

        // 3. 按工序ID分组物料
        var stepItemsDict = allStepItems.GroupBy(it => it.F_StepId).ToDictionary(g => g.Key, g => g.ToList());

        // 4. 统一生成时间
        var now = DateTime.Now;

        // 5. 生成生产任务及物料
        foreach (var routingStep in routingStepList)
        {
            var taskEntity = new AProdTaskEntity
            {
                id = SnowflakeIdHelper.NextId(),
                F_ProcessingId = entity.id,  // 关联加工单
                F_ParentId = routingStep.F_ParentId,
                F_ProcessId = routingStep.F_ProcessId,
                F_NodeId = routingStep.F_NodeId,
                F_NodeName = routingStep.F_NodeName,
                F_Type = routingStep.F_Type,
                F_ResponsibleUserId = routingStep.F_ResponsibleUserId,
                F_SortCode = routingStep.F_SortCode,
                F_CreatorTime = now,
                F_TaskType = "0",
                F_TaskStatus = "1",
                F_CreatorOrganizeId = _userManager.User.OrganizeId,
                F_CreatorUserId = _userManager.UserId,
            };
            taskEntity.F_PlanStartDate = entity.F_PlanStartDate;
            taskEntity.F_PlanEndDate = entity.F_PlanEndDate;
            taskEntity.F_ProdQty = entity.F_PlanQty;
            await _repositoryTask.AsInsertable(taskEntity).ExecuteCommandAsync();

            // 从字典获取该工序的物料列表
            if (stepItemsDict.TryGetValue(routingStep.F_Id, out var stepItems))
            {
                var taskItemList = stepItems.Select(item => new AProdTaskItemEntity
                {
                    F_Id = SnowflakeIdHelper.NextId(),
                    F_TaskId = taskEntity.id,
                    F_GoodsId = item.F_GoodsId,
                    F_Num = item.F_Num,
                    F_CreatorTime = now,
                }).ToList();

                if (taskItemList.Count > 0)
                {
                    await _repositoryTaskItem.AsInsertable(taskItemList).ExecuteCommandAsync();
                }
            }
        }
    }

    /// <summary>
    /// 更新a_prod_processing.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] AProdProcessingUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdProcessingEntity>();
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdProcessingEntity>(new AProdProcessingEntity()));
        ConfigModel tableField449e6cConfig = new ConfigModel
        {
            tableName = "a_prod_bom_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "用料清单",
            children = ExportImportDataHelper.GetTemplateParsing<AProdBomItemEntity>(new AProdBomItemEntity())
        };
        FieldsModel tableField449e6cModel = new FieldsModel()
        {
            __config__ = tableField449e6cConfig,
            __vModel__ = "tableField449e6c"
        };
        fieldList.Add(tableField449e6cModel);
        if (await _repository.IsAnyAsync(it => it.F_ProcessNo.Equals(input.F_ProcessNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "加工单号");

        if (string.IsNullOrEmpty(entity.F_ProcessNo))
        {
            entity.F_ProcessNo = oldEntity.F_ProcessNo;
        }


        // 移除加工单用料清单可能被删除数据
        if (input.tableField449e6c != null && input.tableField449e6c.Any())
            await _repository.AsSugarClient().Deleteable<AProdBomItemEntity>().Where(it => it.F_ProcessingId == entity.id && !input.tableField449e6c.Select(it => it.F_Id).ToList().Contains(it.F_Id)).ExecuteCommandAsync();
        else
            await _repository.AsSugarClient().Deleteable<AProdBomItemEntity>().Where(it => it.F_ProcessingId == entity.id).ExecuteCommandAsync();


        // 新增加工单用料清单新数据
        var aProdBomItemEntityList = input.tableField449e6c.Adapt<List<AProdBomItemEntity>>();


        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_ContractId,
                    it.F_ProjectId,
                    it.F_GoodsId,
                    it.F_BOMId,
                    it.F_ProdPlanId,
                    it.F_ProcessNo,
                    it.F_PlanQty,
                    it.F_DeliveryDate,
                    it.F_Priority,
                    it.F_PlanStartDate,
                    it.F_PlanEndDate,
                    it.F_DrawingFile,
                    it.F_CustomerName,
                    it.F_DoorPlateThickness,
                    it.F_BoxPlateThickness,
                    it.F_InstallOrBeam,
                    it.F_LockSet,
                    it.F_Hinge,
                    it.F_Color,
                    it.F_Type,
                    it.F_BOMImages,
                    it.F_State,
                    it.F_SequenceNo,
                    it.F_Description,
                    it.F_RoutingId,
                })
                .ExecuteCommandAsync();


        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }

        //增加日志
        var changeList = new List<string>();
        string safeToStr(object v) => v == null ? string.Empty : v.ToString();

        void AddChange(string label, object oldVal, object newVal)
        {
            var o = safeToStr(oldVal);
            var n = safeToStr(newVal);
            if (o != n)
            {
                changeList.Add($"{label} 值由 {o} 改为 {n}");
            }
        }

        AddChange("【加工单号】", oldEntity.F_ProcessNo, entity.F_ProcessNo);
        if(oldEntity.F_BOMId != entity.F_BOMId)
        {
            string oldName = "";
            string newName = "";
            if (oldEntity.F_BOMId != null)
            {
                oldName = await _repository.AsSugarClient().Queryable<ABomEntity>().Where(it => it.id.Equals(oldEntity.F_BOMId)).Select(it => it.F_BomName).FirstAsync();
            }
            if (entity.F_BOMId != null)
            {
                newName = await _repository.AsSugarClient().Queryable<ABomEntity>().Where(it => it.id.Equals(entity.F_BOMId)).Select(it => it.F_BomName).FirstAsync();
            }
            changeList.Add($"【BOM】 值由 {oldName} 改为 {newName}");
        }
        AddChange("【计划数】", oldEntity.F_PlanQty, entity.F_PlanQty);
        AddChange("【交货日期】", oldEntity.F_DeliveryDate, entity.F_DeliveryDate);

        if (oldEntity.F_Priority != entity.F_Priority)
        {
            string oldName = "";
            string newName = "";
            if (oldEntity.F_Priority != null)
            {
                oldName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(oldEntity.F_Priority) && it.DictionaryTypeId.Equals("2013182853290004480")).Select(it => it.FullName).FirstAsync();
            }
            if (entity.F_Priority != null)
            {
                newName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(entity.F_Priority) && it.DictionaryTypeId.Equals("2013182853290004480")).Select(it => it.FullName).FirstAsync();
            }
            changeList.Add($"【优先级】 值由 {oldName} 改为 {newName}");
        }
        AddChange("【计划开始】", oldEntity.F_PlanStartDate, entity.F_PlanStartDate);
        AddChange("【计划结束】", oldEntity.F_PlanEndDate, entity.F_PlanEndDate);
        AddChange("【客户】", oldEntity.F_CustomerName, entity.F_CustomerName);
        if (oldEntity.F_DoorPlateThickness != entity.F_DoorPlateThickness)
        {
            string oldName = "";
            string newName = "";
            if (oldEntity.F_DoorPlateThickness != null)
            {
                oldName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(oldEntity.F_DoorPlateThickness) && it.DictionaryTypeId.Equals("2013183327061807104")).Select(it => it.FullName).FirstAsync();
            }
            if (entity.F_DoorPlateThickness != null)
            {
                newName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(entity.F_DoorPlateThickness) && it.DictionaryTypeId.Equals("2013183327061807104")).Select(it => it.FullName).FirstAsync();
            }
            changeList.Add($"【门板厚度】 值由 {oldName} 改为 {newName}");
        }
        if (oldEntity.F_BoxPlateThickness != entity.F_BoxPlateThickness)
        {
            string oldName = "";
            string newName = "";
            if (oldEntity.F_BoxPlateThickness != null)
            {
                oldName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(oldEntity.F_BoxPlateThickness) && it.DictionaryTypeId.Equals("2013183327061807104")).Select(it => it.FullName).FirstAsync();
            }
            if (entity.F_BoxPlateThickness != null)
            {
                newName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(entity.F_BoxPlateThickness) && it.DictionaryTypeId.Equals("2013183327061807104")).Select(it => it.FullName).FirstAsync();
            }
            changeList.Add($"【箱体板厚】 值由 {oldName} 改为 {newName}");
        }
        if (oldEntity.F_InstallOrBeam != entity.F_InstallOrBeam)
        {
            string oldName = "";
            string newName = "";
            if (oldEntity.F_InstallOrBeam != null)
            {
                oldName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(oldEntity.F_InstallOrBeam) && it.DictionaryTypeId.Equals("2013183327061807104")).Select(it => it.FullName).FirstAsync();
            }
            if (entity.F_InstallOrBeam != null)
            {
                newName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(entity.F_InstallOrBeam) && it.DictionaryTypeId.Equals("2013183327061807104")).Select(it => it.FullName).FirstAsync();
            }
            changeList.Add($"【安装板或安装梁】 值由 {oldName} 改为 {newName}");
        }
        AddChange("【锁具】", oldEntity.F_LockSet, entity.F_LockSet);
        AddChange("【铰链】", oldEntity.F_Hinge, entity.F_Hinge);
        AddChange("【颜色】", oldEntity.F_Color, entity.F_Color);
        if (oldEntity.F_Type != entity.F_Type)
        {
            string oldName = "";
            string newName = "";
            var F_TypeData = await _dataInterfaceService.GetDynamicList("F_Type", "2008414575283802112", "F_Id", "F_Name", "children");
            if (oldEntity.F_Type != null)
            {
                var F_TypeCascader = oldEntity.F_Type.ToObject<List<string>>();
                oldName = string.Join("/", F_TypeData.FindAll(it => F_TypeCascader.Contains(it.id)).Select(s => s.fullName));
            }
            if (entity.F_Type != null)
            {
                var F_TypeCascader = entity.F_Type.ToObject<List<string>>();
                newName = string.Join("/", F_TypeData.FindAll(it => F_TypeCascader.Contains(it.id)).Select(s => s.fullName));
            }
            changeList.Add($"【类别】 值由 {oldName} 改为 {newName}");
        }
        AddChange("【备注】", oldEntity.F_Description, entity.F_Description);

        if (changeList.Any())
        {

            var logEntity = new AProdProcessingLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_ProcessingId = entity.id;
            logEntity.F_Title = "编辑加工单";
            logEntity.F_Content = "加工单编号：" + entity.F_ProcessNo + "；修改内容："+ string.Join("；", changeList);
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }


        if (aProdBomItemEntityList != null)
        {
            foreach (var item in aProdBomItemEntityList)
            {
                if (item.F_Id.IsNullOrEmpty())
                {
                    item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                    item.F_Id = SnowflakeIdHelper.NextId();
                    item.F_ProcessingId = entity.id;
                    await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                }
                else
                {
                    item.F_CreatorTime = null;
                    item.F_ProcessingId = entity.id;
                    await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
            }
        }

        
    }

    /// <summary>
    /// 修改状态.
    /// </summary>
    [HttpGet("Check/{id}/{state}")]
    public async Task Check(string id, string state)
    {
        AProdProcessingEntity? entity = await _repository.GetFirstAsync(it => it.id == id);

        if (state == "5")
        {
            var processNo = await _repository.AsQueryable()
                .Where(it => it.F_SequenceNo != null && it.DeleteMark == null)
                .MaxAsync(it => it.F_SequenceNo);

            int isOk = await _repository.AsUpdateable().SetColumns(it => new AProdProcessingEntity()
            {
                F_State = "1",
                F_SequenceNo = processNo + 1,
            }).Where(it => it.id == id).ExecuteCommandAsync();

            //  重新压号（连续 1,2,3...）
            await ReorderWholeTableAsync();

            var logEntity = new AProdProcessingLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_ProcessingId = entity.id;
            logEntity.F_Title = "取消暂停";
            logEntity.F_Content = "取消暂停：" + entity.F_ProcessNo;
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }
        else
        {
            var oldEntity = await _repository.GetFirstAsync(it => it.id == entity.id);

            if (state != "1")
            {
                if (state == "6")
                {
                    //生产中
                    if (oldEntity.F_SequenceNo == null)
                    {
                        var processNo = await _repository.AsQueryable()
                            .Where(it => it.F_SequenceNo != null && it.DeleteMark == null)
                            .MaxAsync(it => it.F_SequenceNo);

                        int isOk = await _repository.AsUpdateable().SetColumns(it => new AProdProcessingEntity()
                        {
                            F_State = "1",
                            F_SequenceNo = (processNo ?? 0) + 1,
                        }).Where(it => it.id == id).ExecuteCommandAsync();
                    }
                    else
                    {
                        int isOk = await _repository.AsUpdateable().SetColumns(it => new AProdProcessingEntity()
                        {
                            F_State = "1",
                        }).Where(it => it.id == id).ExecuteCommandAsync();
                    }

                    await _repositoryTask.AsUpdateable().SetColumns(it => new AProdTaskEntity()
                    {
                        F_TaskStatus = "1"
                    }).Where(it => it.F_ProcessingId == id && it.F_TaskStatus == "2").ExecuteCommandAsync();
                }
                else
                {
                    int isOk = await _repository.AsUpdateable().SetColumns(it => new AProdProcessingEntity()
                    {
                        F_State = state,
                        F_SequenceNo = null
                    }).Where(it => it.id == id).ExecuteCommandAsync();

                    if (state == "2")
                    {
                        await _repositoryTask.AsUpdateable().SetColumns(it => new AProdTaskEntity()
                        {
                            F_TaskStatus = "2"
                        }).Where(it => it.F_ProcessingId == id).ExecuteCommandAsync();
                    }
                }

            }
            else
            {
                if(oldEntity.F_SequenceNo == null)
                {
                    var processNo = await _repository.AsQueryable()
                        .Where(it => it.F_SequenceNo != null && it.DeleteMark == null)
                        .MaxAsync(it => it.F_SequenceNo);

                    int isOk = await _repository.AsUpdateable().SetColumns(it => new AProdProcessingEntity()
                    {
                        F_State = state,
                        F_SequenceNo = (processNo ?? 0) + 1,
                    }).Where(it => it.id == id).ExecuteCommandAsync();
                }
                else
                {
                    int isOk = await _repository.AsUpdateable().SetColumns(it => new AProdProcessingEntity()
                    {
                        F_State = state,
                    }).Where(it => it.id == id).ExecuteCommandAsync();
                }
            }

            //  重新压号（连续 1,2,3...）
            await ReorderWholeTableAsync();


            var stateName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(state) && it.DictionaryTypeId.Equals("2013819135649255424")).Select(it => it.FullName).FirstAsync();

            var logEntity = new AProdProcessingLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_ProcessingId = entity.id;
            logEntity.F_Title = "修改加工单状态";
            logEntity.F_Content = "修改状态为：" + stateName;
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 暂停、取消原因.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("ReasonUpdate/{id}/{type}")]
    [UnitOfWork]
    public async Task ReasonUpdate(string id,string type, [FromBody] AProdProcessingUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdProcessingEntity>();
        entity.id = id;
        entity.F_State = type;
        entity.F_SequenceNo = null;
        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_State,
                    it.F_SequenceNo
                })
                .ExecuteCommandAsync();
        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }

        //  重新压号（连续 1,2,3...）
        await ReorderWholeTableAsync();

        if (type == "3")
        {
            await _repositoryTask.AsUpdateable().SetColumns(it => new AProdTaskEntity()
            {
                F_TaskStatus = "3"
            }).Where(it => it.F_ProcessingId == id && it.F_TaskStatus == "1").ExecuteCommandAsync();

            var logEntity = new AProdProcessingLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_ProcessingId = entity.id;
            logEntity.F_Title = "暂停加工单";
            logEntity.F_Content = "暂停：" + input.F_Reason;
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }
        else if (type == "4")
        {
            var logEntity = new AProdProcessingLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_ProcessingId = entity.id;
            logEntity.F_Title = "取消加工单";
            logEntity.F_Content = "取消：" + input.F_Reason; ;
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 删除a_prod_processing.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [UnitOfWork]
    public async Task Delete(string id)
    {
        if(!await _repository.IsAnyAsync(it => it.id == id))
        {
            throw Oops.Oh(ErrorCode.COM1005);
        }

        var entity = await _repository.GetFirstAsync(it => it.id.Equals(id));
        await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
    }

    /// <summary>
    /// 批量删除a_prod_processing.
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
            // 批量删除a_prod_processing
            /* await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);*/

            // 删除
            await DeleteWithReorderAsync(input.ids[0].ToString());
        }
    }

    /// <summary>
    /// a_prod_processing详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.id == id)
            .Select(it => new AProdProcessingDetailOutput
            {
                id = it.id,
                F_ContractId = SqlFunc.Subqueryable<ACtcContractEntity>().EnableTableFilter().Where(c => c.id == it.F_ContractId && c.DeleteMark == null).Select(c => c.F_ContractCode),
                F_ProjectId = it.F_ProjectId,
                F_GoodsId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_BOMId = it.F_BOMId,
                F_ProdPlanId = it.F_ProdPlanId,
                F_ProcessNo = it.F_ProcessNo,
                F_PlanQty = it.F_PlanQty.ToString(),
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_Priority = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Priority) && dic.DictionaryTypeId.Equals("2013182853290004480")).Select(dic => dic.FullName),
                F_PlanStartDate = it.F_PlanStartDate.Value.ToString("yyyy-MM-dd"),
                F_PlanEndDate = it.F_PlanEndDate.Value.ToString("yyyy-MM-dd"),
                F_DrawingFile = it.F_DrawingFile,
                F_CustomerName = SqlFunc.Subqueryable<ACustomerEntity>().EnableTableFilter().Where(c => c.id == it.F_CustomerName && c.DeleteMark == null).Select(c => c.F_CustomerName),
                F_DoorPlateThickness = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_DoorPlateThickness) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_BoxPlateThickness = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_BoxPlateThickness) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_InstallOrBeam = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_InstallOrBeam) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_LockSet = it.F_LockSet,
                F_Hinge = it.F_Hinge,
                F_Color = it.F_Color,
                F_Type = it.F_Type,
                F_BOMImages = it.F_BOMImages,
                F_State = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_State) && dic.DictionaryTypeId.Equals("2013819135649255424")).Select(dic => dic.FullName),
                F_SequenceNo = it.F_SequenceNo.ToString(),
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // BOMID
            if (item.F_BOMId != null)
            {
                linkageParameters = new List<DataInterfaceParameter>();
                linkageParameters.Add(new DataInterfaceParameter
                {
                    field = "goodsId",
                    relationField = item.F_GoodsId,
                    isSubTable = false,
                    dataType = "varchar",
                    defaultValue = item.F_GoodsId,
                    fieldName = "合同ID",
                    sourceType = 1
                });
                var F_BOMIdData = await _dataInterfaceService.GetDynamicList("F_BOMId", "2013188646957617152", "id", "F_BomName", "", linkageParameters);
                item.F_BOMId = F_BOMIdData.Find(it => it.id.Equals(item.F_BOMId))?.fullName;
            }

            if(item.F_DrawingFile != null)
            {
                item.F_DrawingFile = item.F_DrawingFile.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_DrawingFile = new List<FileControlsModel>();
            }

            var F_TypeData = await _dataInterfaceService.GetDynamicList("F_Type", "2008414575283802112", "F_Id", "F_Name", "children");
            if(item.F_Type != null)
            {
                var F_TypeCascader = item.F_Type.ToObject<List<string>>();
                item.F_Type = string.Join("/", F_TypeData.FindAll(it => F_TypeCascader.Contains(it.id)).Select(s => s.fullName));
            }

            if(item.F_BOMImages != null)
            {
                item.F_BOMImages = item.F_BOMImages.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_BOMImages = new List<FileControlsModel>();
            }

        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        return resData.FirstOrDefault();
    }



    /// <summary>
    /// 获取加工单 生产计划信息.
    /// </summary>
    /// <param name="id">路线ID.</param>
    /// <returns></returns>
    [HttpGet("FlowInfo/{id}")]
    public async Task<dynamic> GetFlowInfo(string id)
    {
        // 1. 查询加工单信息
        var entity = await _repository.GetFirstAsync(it => it.id.Equals(id) && it.DeleteMark == null);
        if (entity == null)
            throw Oops.Oh(ErrorCode.COM1005);

        // 2. 查询工序节点列表
        var stepList = await _repositoryTask.AsQueryable()
            .Where(it => it.F_ProcessingId.Equals(id))
            .OrderBy(it => it.F_SortCode)
            .ToListAsync();

        // 3. 查询所有工序节点的物料信息
        var stepIds = stepList.Select(it => it.id).ToList();
        var stepItemList = await _repositoryTaskItem.AsQueryable()
            .Where(it => stepIds.Contains(it.F_TaskId))
            .ToListAsync();
        var stepItemDict = stepItemList.GroupBy(it => it.F_TaskId).ToDictionary(g => g.Key, g => g.ToList());

        // 4. 构建流程节点字典
        var flowNodes = new Dictionary<string, FlowNode>();

        foreach (var step in stepList)
        {
            var node = new FlowNode
            {
                id = step.id,
                nodeId = step.F_NodeId,
                type = step.F_Type,
                nodeName = step.F_NodeName,
                F_TaskStatusCode = step.F_TaskStatus,
            };
            var routingStepEntity = await _repositoryRoutingStep.GetFirstAsync(it => it.F_ProcessId == step.F_ProcessId && it.F_RoutingId == entity.F_RoutingId && it.F_NodeId == step.F_NodeId);

            // 构建formData内容（工序详细字段）
            var content = new FlowNodeContent
            {
                F_BOMId = routingStepEntity.F_BOMId,
                F_WagePrice = routingStepEntity.F_WagePrice,
                F_StdHour = routingStepEntity.F_StdHour,
                F_StdMinute = routingStepEntity.F_StdMinute,
                F_StdSecond = routingStepEntity.F_StdSecond,
                F_PriceType = routingStepEntity.F_PriceType,
                F_UnitRatio = routingStepEntity.F_UnitRatioProd,
                F_ReportUnit = routingStepEntity.F_UnitRatioBase,
                F_IsOutsource = routingStepEntity.F_IsOutsource,
                F_IsQc = routingStepEntity.F_IsQc,
                F_DefectHandle = routingStepEntity.F_DefectHandle,
                F_Files = routingStepEntity.F_Files != null ? routingStepEntity.F_Files.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>(),

                F_ProcessId = step.F_ProcessId,
                F_ResponsibleUserId = step.F_ResponsibleUserId,
                F_ProdDispatch = step.F_ProdDispatch,
                F_QcDispatch = step.F_QcDispatch,
                F_PlanStartDate = step.F_PlanStartDate,
                F_PlanEndDate = step.F_PlanEndDate,
                F_ProdQty = step.F_ProdQty,
                F_TaskStatus = step.F_TaskStatus,
                F_Description = step.F_Description,
            };

            // 填充工序物料信息
            if (stepItemDict.TryGetValue(step.id, out var items))
            {
                content.tableField0bb944 = items.Adapt<List<AProdTaskItemCrInput>>();
            }
            else
            {
                content.tableField0bb944 = new List<AProdTaskItemCrInput>();
            }

            // 如果是 approver 节点，填充路线编号和名称
            if (step.F_Type == "approver")
            {
                content.F_ProcessNo = entity.F_ProcessNo;

                node.content = await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => a.Id == step.F_ResponsibleUserId).Select(it => SqlFunc.MergeString(it.RealName, "/", it.Account)).FirstAsync();
            }

            // formData直接返回对象
            node.formData = content;

            // 根据节点类型解析特定数据
            if (!string.IsNullOrEmpty(step.F_Description))
            {
                try
                {
                    switch (step.F_Type)
                    {
                        case "global":
                            // 解析全局节点数据
                            node.formId = "";
                            node.formName = "";
                            break;
                        case "approver":
                            // 解析审批节点数据
                            break;
                    }
                }
                catch
                {
                    // 解析失败时忽略
                }
            }

            flowNodes[step.F_NodeId] = node;
        }

        // 5. 构建返回结果
        var result = new {
            id = entity.id,
            F_ProcessNo = entity.F_ProcessNo,
            flowId = entity.FlowId,
            flowXml = entity.F_XML,
            flowNodes = flowNodes,
            flowConfig = (string)null // 如果有流程配置，可以从数据库读取
        };

        return result;
    }

    /// <summary>
    /// 更新加工单流程(暂不使用).
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("FlowUpdate/{id}")]
    [UnitOfWork]
    public async Task FlowUpdate(string id, [FromBody] AProdProcessingFlowInput input)
    {
        if (input == null)
            throw Oops.Oh(ErrorCode.COM1000);

        // 1. 查询原有路线信息
        var entity = await _repository.GetFirstAsync(it => it.id.Equals(id) && it.DeleteMark == null);
        if (entity == null)
            throw Oops.Oh(ErrorCode.COM1005);

        // 2. 解析版本ID
        var flowId = input.flowId;

        // 3. 解析XML字符串(需要URL解码)
        var flowXml = input.flowXml;
        if (!string.IsNullOrEmpty(flowXml))
        {
            flowXml = Uri.UnescapeDataString(flowXml);
        }

        // 4. 从第一个 approver 节点获取路线编号和名称
        string? routingNo = null;
        if (input.flowNodes != null && input.flowNodes.Count > 0)
        {
            var firstApproverNode = input.flowNodes.Values.FirstOrDefault(n => n.type == "approver");
            if (firstApproverNode?.formData != null)
            {
                routingNo = firstApproverNode.formData.F_ProcessNo;
            }
        }

        // 5. 如果未提供编号，保留原值
        if (string.IsNullOrEmpty(routingNo))
        {
            routingNo = entity.F_ProcessNo;
        }

        // 6. 检查重复（排除当前记录）
        if (await _repository.IsAnyAsync(it => it.F_ProcessNo.Equals(routingNo) && !it.id.Equals(id) && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "加工单号");

        // 7. 更新路线信息
        entity.FlowId = flowId;
        entity.F_XML = flowXml;
        entity.F_ProcessNo = routingNo;

        // 8. 查询原有的工序节点
        var existingSteps = await _repositoryTask.AsQueryable()
            .Where(it => it.F_ProcessingId.Equals(id))
            .ToListAsync();
        var existingStepDict = existingSteps.ToDictionary(it => it.F_NodeId, it => it);

        // 6. 解析流程节点并处理增删改
        var stepsToInsert = new List<AProdTaskEntity>();
        var stepsToUpdate = new List<AProdTaskEntity>();
        var processedNodeIds = new HashSet<string>();
        var flowNodes = input.flowNodes;

        if (flowNodes != null && flowNodes.Count > 0)
        {
            int sortCode = 0;
            foreach (var node in flowNodes)
            {
                var nodeId = node.Key;
                var nodeValue = node.Value;

                // 跳过空节点
                if (nodeValue == null) continue;

                processedNodeIds.Add(nodeId);

                // 从formData中获取节点内容字段
                var content = nodeValue.formData;

                // 根据nodeId判断是新增还是修改
                if (existingStepDict.TryGetValue(nodeId, out var existingStep))
                {
                    // 存在相同nodeId，更新原有记录
                    existingStep.F_NodeName = nodeValue.nodeName;
                    existingStep.F_Type = nodeValue.type;
                    existingStep.F_SortCode = sortCode++;

                    // 从formData中更新字段
                    if (content != null)
                    {
                        if (string.IsNullOrEmpty(content.F_ProcessId))
                        {
                            throw Oops.Bah(ErrorCode.COM1018, "请填写工序");
                        }
                        if (string.IsNullOrEmpty(content.F_ResponsibleUserId))
                        {
                            throw Oops.Bah(ErrorCode.COM1018, "请填写负责人");
                        }

                        existingStep.F_ProcessId = content.F_ProcessId;
                        existingStep.F_ResponsibleUserId = content.F_ResponsibleUserId;
                        existingStep.F_ProdDispatch = content.F_ProdDispatch;
                        existingStep.F_QcDispatch = content.F_QcDispatch;
                        existingStep.F_PlanStartDate = content.F_PlanStartDate;
                        existingStep.F_PlanEndDate = content.F_PlanEndDate;
                        existingStep.F_ProdQty = content.F_ProdQty;
                        existingStep.F_Description = content.F_Description;


                        // 处理生产任务物料信息(tableField0bb944)
                        //if (content.tableField0bb944 != null)
                        //{
                        //    existingStep.AProdTaskItemList = content.tableField0bb944.Adapt<List<AProdTaskItemEntity>>();
                        //    foreach (var item in existingStep.AProdTaskItemList)
                        //    {
                        //        item.F_TaskId = existingStep.id;
                        //        if (string.IsNullOrEmpty(item.F_Id))
                        //        {
                        //            item.F_Id = SnowflakeIdHelper.NextId();
                        //            item.F_CreatorTime = DateTime.Now;
                        //        }
                        //    }
                        //}
                    }

                    // 根据节点类型处理特定数据
                    switch (nodeValue.type)
                    {
                        case "global":
                            // 全局节点：保存表单信息到备注
                            break;
                        case "approver":
                            // 审批节点/工序节点：保存审批人列表
                            break;
                        case "start":
                        case "end":
                        case "connect":
                        default:
                            break;
                    }

                    stepsToUpdate.Add(existingStep);
                }
            }
        }

        // 更新加工单信息
        await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new {
                it.FlowId,
                it.F_XML,
                it.F_ProcessNo,
            })
            .ExecuteCommandAsync();

        // 批量更新已有的工序节点及其物料
        foreach (var step in stepsToUpdate)
        {
            // 更新工序节点基本信息
            await _repositoryTask.AsUpdateable(step)
                .UpdateColumns(it => new {
                    it.F_NodeName,
                    it.F_Type,
                    it.F_Description,
                    it.F_ProcessId,
                    it.F_ResponsibleUserId,
                    it.F_ProdDispatch,
                    it.F_QcDispatch,
                    it.F_PlanStartDate,
                    it.F_PlanEndDate,
                    it.F_ProdQty,
                })
                .ExecuteCommandAsync();

            // 处理生产任务物料：先删除旧的，再插入新的
            //if (step.AProdTaskItemList != null)
            //{
            //    // 删除该工序下所有旧的物料
            //    await _repositoryTask.AsSugarClient().Deleteable<AProdTaskItemEntity>()
            //        .Where(it => it.F_TaskId == step.id)
            //        .ExecuteCommandAsync();

            //    // 插入新的物料
            //    if (step.AProdTaskItemList.Count > 0)
            //    {
            //        await _repositoryTask.AsSugarClient().Insertable(step.AProdTaskItemList)
            //            .ExecuteCommandAsync();
            //    }
            //}
        }
    }



    /// <summary>
    /// 获取加工单用料清单.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("BomItemList")]
    public async Task<dynamic> GetBomItemList([FromBody] AProdProcessingListQueryInput input)
    {
        var idsQ = await _repositoryBomItem.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_ProcessingId), it => it.F_ProcessingId.Equals(input.F_ProcessingId))
            .Select(it => new AProdBomItemListOutput
            {
                F_Id = it.F_Id,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            //把多表结果变成“单表”再排序：
            .MergeTable()
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .ToPagedListAsync(input.currentPage, input.pageSize);

        var ids = idsQ.list.Select(x => x.F_Id).ToList();   // 20 个 long

        // 2. 再用 ids 一次性补全字段（此时只有 20 行，随便 join 18 张表都毫秒级）
        var data = await _repositoryBomItem.AsQueryable()
            .Where(it => ids.Contains(it.F_Id))
            .Select(it => new AProdBomItemListOutput
            {
                F_Id = it.F_Id,
                F_ProcessingId = it.F_ProcessingId,
                F_GoodsId = it.F_GoodsId,
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => SqlFunc.MergeString(g.F_GoodsName, "-", g.F_GoodsNo)),
                F_UnitTwo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit),
                F_Unit = it.F_Unit,
                F_StockInProcess = it.F_StockInProcess,
                F_SortCode = it.F_SortCode,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);


        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            if (item.F_StockInProcess != null)
            {
                var F_StockInProcessData = await _dataInterfaceService.GetDynamicList("F_StockInProcess", "2012092092830060544", "id", "F_ProcessName", "");
                item.F_StockInProcess = F_StockInProcessData.Find(it => it.id.Equals(item.F_StockInProcess))?.fullName;
            }
            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(item.F_UnitTwo))
            {
                var F_UnitIdCascader = item.F_UnitTwo.ToObject<List<string>>();
                item.F_UnitTwo = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));

                if (F_UnitIdCascader.Count > 1)
                {
                    item.F_NumUnit = item.F_Unit.ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    item.F_NumUnit = item.F_Unit.ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }
        });

        data.pagination = idsQ.pagination;

        return PageResult<AProdBomItemListOutput>.SqlSugarPageResult(data);
    }


    /// <summary>
    /// 弹窗获取加工单及商品信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("PopupList")]
    public async Task<dynamic> GetPopupList()
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .LeftJoin<AGoodsEntity>((it, g) => g.id.Equals(it.F_GoodsId) && g.DeleteMark == null)
            .Select((it, g) => new AProdProcessingListOutput
            {
                id = it.id,
                F_ProcessNo = it.F_ProcessNo,
                F_GoodsId = it.F_GoodsId,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_CategoryId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_CategoryId),
                F_PlanQty = it.F_PlanQty.ToString(),
                F_InventoryNum = 0,
                F_YseNum = 0,
                F_CostPrice = g.F_CostPrice,
                F_Unit_Ratio = g.F_Unit_Ratio,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            //库存
            var inventory = await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_GoodsId).SumAsync(it => it.F_Num);
            if (inventory != null)
            {
                item.F_InventoryNum += inventory;
            }
            //已入库数量
            var stockFg = await _repositoryStockFgItem.AsQueryable().Where(it => it.DeleteMark == null && it.F_WorkOrderId == item.id).SumAsync(it => it.F_Num);
            if (stockFg != null)
            {
                item.F_YseNum += stockFg;
            }
            //转换CategoryId为F_Code
            if (!string.IsNullOrEmpty(item.F_CategoryId))
            {
                try
                {
                    var categoryIds = System.Text.Json.JsonSerializer.Deserialize<List<string>>(item.F_CategoryId);
                    if (categoryIds != null && categoryIds.Any())
                    {
                        var categoryCodes = await _repositorysp.AsQueryable()
                            .Where(it => categoryIds.Contains(it.id))
                            .Select(it => it.F_Code)
                            .ToListAsync();
                        item.F_CategoryId = System.Text.Json.JsonSerializer.Serialize(categoryCodes);
                    }
                }
                catch
                {
                    // 如果解析失败，保持原值
                }
            }
        });
        return new {
            data = data
        };
    }

    /// <summary>
    /// 获取某个 加工单及商品信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("ProcessingIdList/{id}")]
    public async Task<dynamic> GetProcessingIdList(string id)
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null && it.id == id)
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .LeftJoin<AGoodsEntity>((it, g) => g.id.Equals(it.F_GoodsId) && g.DeleteMark == null)
            .Select((it, g) => new AProdProcessingListOutput
            {
                id = it.id,
                F_ProcessNo = it.F_ProcessNo,
                F_GoodsId = it.F_GoodsId,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_PlanQty = it.F_PlanQty.ToString(),
                F_InventoryNum = 0,
                F_YseNum = 0,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            //库存
            var inventory = await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_GoodsId).SumAsync(it => it.F_Num);
            if (inventory != null)
            {
                item.F_InventoryNum += inventory;
            }
            //已入库数量
            var stockFg = await _repositoryStockFgItem.AsQueryable().Where(it => it.DeleteMark == null && it.F_WorkOrderId == item.id).SumAsync(it => it.F_Num);
            if (stockFg != null)
            {
                item.F_YseNum += stockFg;
            }
        });
        return data;
    }


    /// <summary>
    /// 删除一条记录，并保证整表 F_SequenceNo 连续
    /// </summary>
    public async Task DeleteWithReorderAsync(string id)
    {
        var db = _repository.AsSugarClient();

        // 1. 拿到被删行的序号
        var deletedSeq = await db.Queryable<AProdProcessingEntity>()
                                 .Where(it => it.id == id)
                                 .Select(it => it.F_SequenceNo)
                                 .FirstAsync();

        if (!deletedSeq.HasValue) return;   // 本来就没序号，直接退

        // 2. 物理删除（或软删自己调）
        await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 3. 把 > deletedSeq 的所有行整体 -1
        await db.Updateable<AProdProcessingEntity>()
                .SetColumns(it => it.F_SequenceNo == it.F_SequenceNo - 1)
                .Where(it => it.F_SequenceNo > deletedSeq && it.DeleteMark == null)
                .ExecuteCommandAsync();
    }

    /// <summary>
    /// 把整表 F_SequenceNo 重新压成 1,2,3... 连续
    /// </summary>
    private async Task ReorderWholeTableAsync()
    {
        var db = _repository.AsSugarClient();

        // 按旧序号正序取全部
        var all = await db.Queryable<AProdProcessingEntity>()
                          .Where(it => it.F_SequenceNo != null && it.DeleteMark == null)
                          .OrderBy(it => it.F_SequenceNo)
                          .ToListAsync();

        if (!all.Any()) return;

        // 生成新序号（1 起）
        for (int i = 0; i < all.Count; i++)
            all[i].F_SequenceNo = i + 1;

        // 批量更新
        await db.Updateable(all)
                .UpdateColumns(it => new { it.F_SequenceNo })
                .ExecuteCommandAsync();
    }



    #region 大屏
    /// <summary>
    /// 生产-工单执行进度.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("SC_GDZXJD_JGD")]
    public async Task<dynamic> GetSC_GDZXJD_JGD()
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_State != "2")
            .LeftJoin<AGoodsEntity>((it, g) => g.id.Equals(it.F_GoodsId) && g.DeleteMark == null)
            .Select((it, g) => new SC_GDZXJD_JGDOutput
            {
                id = it.id,
                F_ProcessNo = it.F_ProcessNo,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_PlanQty = it.F_PlanQty.ToString(),
                F_Priority = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Priority) && dic.DictionaryTypeId.Equals("2013182853290004480")).Select(dic => dic.FullName),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc).OrderBy(it => it.F_ProcessNo, OrderByType.Desc)
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            //工序总数
            item.F_TaskAllNum = await _repositoryTask.AsQueryable().Where(it => it.F_ProcessingId == item.id && it.F_Type == "approver").CountAsync();

            //已完成工序数
            item.F_TaskYseNum = await _repositoryTask.AsQueryable().Where(it => it.F_ProcessingId == item.id && it.F_TaskStatus == "2" && it.F_Type == "approver").CountAsync();

            item.F_OrderTask = item.F_TaskAllNum == 0 ? "0%" : (Math.Round((item.F_TaskYseNum.Value * 100.0 / item.F_TaskAllNum.Value), 2)).ToString() + "%";

            item.ProdTaskList = await GetJGD_SCRWXX(item.id);
        });


        return new {
            data = data
        };
    }
    /// <summary>
    /// 大屏加工单-生产任务信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("JGD-SCRWXX")]
    public async Task<dynamic> GetJGD_SCRWXX(string id)
    {
        // 先获取所有数据
        var allData = await _repositoryTask.AsQueryable()
            .Where(it => it.F_Type == "approver" && it.F_ProcessingId == id)
            .RightJoin<AProdProcessingEntity>((it, ps) => ps.id.Equals(it.F_ProcessingId) && ps.DeleteMark == null)
            .LeftJoin<AProdProcessEntity>((it, ps, pr) => pr.id.Equals(it.F_ProcessId) && pr.DeleteMark == null)
            .Select((it, ps, pr) => new AProdTaskAPPListOutput
            {
                id = it.id,
                F_ParentId = it.F_ParentId,
                F_ProcessingId = ps.F_ProcessNo,
                F_ProcessId = it.F_ProcessId,
                F_NodeId = it.F_NodeId,
                F_ProcessName = pr.F_ProcessName,
                F_ProdQty = it.F_ProdQty,
                F_TaskStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_TaskStatus) && dic.DictionaryTypeId.Equals("2013859706900189184")).Select(dic => dic.FullName),
                F_TaskStatusCode = it.F_TaskStatus,
                F_SortCode = it.F_SortCode,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable()
            .ToListAsync();

        // 构建层级关系并排序
        var sortedData = BuildHierarchicalOrder(allData);

        await _repositoryTask.AsSugarClient().CopyNew().ThenMapperAsync(sortedData, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            item.F_CompletedQty = await _repositoryReport.AsQueryable()
                .Where(r => r.F_TaskId == item.id)
                .SumAsync(r => SqlFunc.IsNull(r.F_GoodQty, 0)) ?? 0;

        });
        return sortedData;
    }

    /// <summary>
    /// 构建层级排序：以F_ParentId为'-1'开始，子节点紧随父节点
    /// 根据F_NodeId而不是id进行层级匹配
    /// </summary>
    private List<AProdTaskAPPListOutput> BuildHierarchicalOrder(List<AProdTaskAPPListOutput> allData)
    {
        var result = new List<AProdTaskAPPListOutput>();
        
        // 1. 先找出所有根节点（F_ParentId 为 null、空字符串 或 "-1"）
        var rootNodes = allData.Where(item => 
            string.IsNullOrEmpty(item.F_ParentId) || item.F_ParentId == "-1").ToList();

        // 2. 按 F_SortCode 对根节点排序，并且只保留第一个F_ParentId为"-1"的节点
        var sortedRootNodes = rootNodes.OrderBy(item => item.F_SortCode).ToList();

        // 3. 只处理第一个根节点（根据用户要求只保留第一级）
        if (sortedRootNodes.Any())
        {
            var firstRootNode = sortedRootNodes.First();
            result.Add(firstRootNode);
            AddChildrenRecursive(firstRootNode.F_NodeId, allData, ref result);
        }

        return result;
    }

    /// <summary>
    /// 递归添加子节点（根据F_NodeId匹配）
    /// </summary>
    private void AddChildrenRecursive(string parentNodeId, List<AProdTaskAPPListOutput> allData, ref List<AProdTaskAPPListOutput> result)
    {
        // 找出所有直接子节点（根据F_NodeId匹配）
        var children = allData.Where(item => item.F_ParentId == parentNodeId)
                            .OrderBy(item => item.F_SortCode)
                            .ToList();

        foreach (var child in children)
        {
            result.Add(child);
            // 递归添加子节点的子节点
            AddChildrenRecursive(child.F_NodeId, allData, ref result);
        }
    }
    #endregion

    #region 门户
    /// <summary>
    /// 门户-加工单优先级信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("MH-JGDYXJXX")]
    public async Task<dynamic> GetMH_JGDYXJXX()
    {
        var dataList = new List<JGDYXJXXOutput>();
        var priorityDictionary = await _repositoryspDictionaryData.AsQueryable().Where(it => it.DeleteMark == null && it.DictionaryTypeId == "2013182853290004480").ToListAsync();
        foreach (var item in priorityDictionary)
        {
            var processingData = await _repository.AsQueryable().Where(it => it.F_Priority == item.EnCode && it.DeleteMark == null).CountAsync();
            dataList.Add(new JGDYXJXXOutput { F_Name = item.FullName, F_Num = processingData });
        }

        return new {
            data = dataList
        };
    }
    /// <summary>
    /// 门户-数量信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("MH-SLXX")]
    public async Task<dynamic> GetMH_SLXX()
    {
        var DaiShengChanDan = await _repository.AsQueryable().Where(it => it.F_State == "0" && it.DeleteMark == null).CountAsync();
        var JinXingZhongGongDan = await _repository.AsQueryable().Where(it => it.F_State == "1" && it.DeleteMark == null).CountAsync();
        var WanChengGongDan = await _repository.AsQueryable().Where(it => it.F_State == "2" && it.DeleteMark == null).CountAsync();
        var BaoGong = (await _repository.AsQueryable().Where(it => it.DeleteMark == null && it.F_State == "2").SumAsync(it => it.F_PlanQty)) ?? 0;
        return new {
            data = new { DaiShengChanDan = DaiShengChanDan,JinXingZhongGongDan = JinXingZhongGongDan,WanChengGongDan = WanChengGongDan,BaoGong = BaoGong }
        };
    }
    #endregion
}
public class JGDYXJXXOutput
{
    public string F_Name { get; set; }

    public int F_Num { get; set; }

}
//生产-工单执行进度
public class SC_GDZXJD_JGDOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 商品ID.
    /// </summary>
    public string? F_GoodsName { get; set; }
    public string? F_GoodsNo { get; set; }
    public string? F_Specification { get; set; }

    /// <summary>
    /// 工序总数.
    /// </summary>
    public int? F_TaskAllNum { get; set; }

    /// <summary>
    /// 已完成工序数.
    /// </summary>
    public int? F_TaskYseNum { get; set; }

    /// <summary>
    /// 工序进度.
    /// </summary>
    public string? F_OrderTask { get; set; }

    /// <summary>
    /// 加工单号.
    /// </summary>
    public string? F_ProcessNo { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    public string F_PlanQty { get; set; }

    /// <summary>
    /// 优先级.
    /// </summary>
    public string? F_Priority { get; set; }

    /// <summary>
    /// 状态.
    /// </summary>
    public string? F_State { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

    /// <summary>
    /// 生产任务列表.
    /// </summary>
    public List<AProdTaskAPPListOutput> ProdTaskList { get; set; }

}