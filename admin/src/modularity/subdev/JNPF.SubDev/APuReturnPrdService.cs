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
using JNPF.example.Entitys.Dto.APuReturnPrd;
using JNPF.example.Entitys.Dto.APuReturnPrdItem;
using JNPF.example.Entitys.Dto.APuReturnPrdLog;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
using System.Text.Json;
namespace JNPF.example;

/// <summary>
/// 业务实现：a_pu_return_prd.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuReturnPrd", Order = 200)]
[Route("api/example/[controller]")]
public class APuReturnPrdService : IAPuReturnPrdService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuReturnPrdEntity> _repository;
    private readonly ISqlSugarRepository<APuReturnPrdItemEntity> _repositoryItem;
    private readonly ISqlSugarRepository<APuReturnPrdLogEntity> _repositoryLog;
    private readonly ISqlSugarRepository<AProdBomItemEntity> _repositoryBomItem;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositorygoods;
      private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"生产退料单号\",\"field\":\"F_ReturnInNo\"},{\"value\":\"入库类型\",\"field\":\"F_ReturnInType\"},{\"value\":\"加工单号\",\"field\":\"F_WorkOrderId\"},{\"value\":\"工单类型\",\"field\":\"F_Type\"},{\"value\":\"退料日期\",\"field\":\"F_ReturnInDate\"},{\"value\":\"操作方式\",\"field\":\"F_Method\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="APuReturnPrdService"/>类型的新实例.
    /// </summary>
    public APuReturnPrdService(
        ISqlSugarRepository<APuReturnPrdItemEntity> repositoryItem,
        ISqlSugarRepository<APuReturnPrdLogEntity> repositoryLog,
        ISqlSugarRepository<APuReturnPrdEntity> repository,
        ISqlSugarRepository<AProdBomItemEntity> repositoryBomItem,
        ISqlSugarRepository<AGoodsEntity> repositorygoods,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryItem = repositoryItem;
        _repositoryLog = repositoryLog;
        _repository = repository;
        _repositoryBomItem = repositoryBomItem;
        _repositorygoods = repositorygoods;
        _repositoryInventory = repositoryInventory;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_pu_return_prd.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.APuReturnPrdItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuReturnPrdLogList)
            .Select(it => new APuReturnPrdEntity
            {
                id = it.id,
                F_ReturnInNo = it.F_ReturnInNo,
                //F_WarehouseId = it.F_WarehouseId,
                F_ReturnInType = it.F_ReturnInType,
                F_ReturnInDate = it.F_ReturnInDate,
                F_Type = it.F_Type,
                F_WorkOrderId = it.F_WorkOrderId,
                F_Method = it.F_Method,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                APuReturnPrdItemList = it.APuReturnPrdItemList.Where(it => it.DeleteMark == null).ToList(),
                APuReturnPrdLogList = it.APuReturnPrdLogList.Where(it => it.DeleteMark == null).ToList()    ,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuReturnPrdInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFieldbb1084, async aPuReturnPrdItem =>
        {
            // 创建人员
            aPuReturnPrdItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuReturnPrdItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuReturnPrdItem.F_CreatorTime = aPuReturnPrdItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            aPuReturnPrdItem.F_WarehouseId = "";
        });
        return data;
    }

    /// <summary>
    /// 获取a_pu_return_prd列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuReturnPrdListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuReturnPrdItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuReturnPrdLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuReturnPrdListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_return_prd"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_return_prd_item"))) aPuReturnPrdItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_return_prd_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_return_prd_log"))) aPuReturnPrdLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_return_prd_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuReturnPrdEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuReturnPrdItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldbb1084-", entityInfo, 1);
        List<IConditionalModel> aPuReturnPrdItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuReturnPrdItemConditionalModel.AddRange(aPuReturnPrdItemAuthorizeWhere);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuReturnPrdLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldceb7bd-", entityInfo, 1);
        List<IConditionalModel> aPuReturnPrdLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuReturnPrdLogConditionalModel.AddRange(aPuReturnPrdLogAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuReturnPrdEntity>();
      
        var F_ReturnInTypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_ReturnInType").DbColumnName;
        var F_WorkOrderIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_WorkOrderId").DbColumnName;

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ReturnInNo), it => it.F_ReturnInNo.Contains(input.F_ReturnInNo))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_ReturnInTypeDbColumnName, input.F_ReturnInType))
            .WhereIF(input.F_ReturnInDate?.Count() > 0, it => SqlFunc.Between(it.F_ReturnInDate, input.F_ReturnInDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_ReturnInDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_Type), it => it.F_Type.Contains(input.F_Type))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_WorkOrderIdDbColumnName, input.F_WorkOrderId))
            .Where(authorizeWhere)
            .WhereIF(aPuReturnPrdItemAuthorizeWhere?.Count > 0, it => it.APuReturnPrdItemList.Any(aPuReturnPrdItemAuthorizeWhere))
            .WhereIF(aPuReturnPrdLogAuthorizeWhere?.Count > 0, it => it.APuReturnPrdLogList.Any(aPuReturnPrdLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aPuReturnPrdItemConditionalModel.Count > 0, it => it.APuReturnPrdItemList.Any(aPuReturnPrdItemConditionalModel))
            .WhereIF(aPuReturnPrdLogConditionalModel.Count > 0, it => it.APuReturnPrdLogList.Any(aPuReturnPrdLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new APuReturnPrdListOutput
            {
                id = it.id,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            //把多表结果变成“单表”再排序：
            .MergeTable()
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .ToPagedListAsync(input.currentPage, input.pageSize);

        var ids = idsQ.list.Select(x => x.id).ToList();   // 20 个 long

        // 2. 再用 ids 一次性补全字段（此时只有 20 行，随便 join 18 张表都毫秒级）
        var data = await _repository.AsQueryable()
            .Where(it => ids.Contains(it.id))
            .Select(it => new APuReturnPrdListOutput
            {
                id = it.id,
                F_ReturnInNo = it.F_ReturnInNo,
                F_ReturnInType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_ReturnInType) && dic.DictionaryTypeId.Equals("2012074650393251840")).Select(dic => dic.FullName),
                F_ReturnInDate = it.F_ReturnInDate.Value.ToString("yyyy-MM-dd"),
                F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2014894783159472128")).Select(dic => dic.FullName),
                F_TypeCode = it.F_Type,
                F_WorkOrderId = it.F_WorkOrderId,
                F_Method = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Method) && dic.DictionaryTypeId.Equals("2014907575941861376")).Select(dic => dic.FullName),
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();


            // 工单ID
            if (item.F_TypeCode == "0")
            {
                var F_WorkOrderIdData = await _dataInterfaceService.GetDynamicList("F_WorkOrderId", "2013860549426810880", "F_Id", "F_ProcessNo", "");
                if (item.F_WorkOrderId != null)
                {
                    item.F_WorkOrderId = F_WorkOrderIdData.Find(it => it.id.Equals(item.F_WorkOrderId))?.fullName;
                }
            }
            else if (item.F_TypeCode == "1")
            {
                var F_WorkOrderIdData = await _dataInterfaceService.GetDynamicList("F_WorkOrderId", "2014969808717746176", "id", "F_ProcessNo", "");
                if (item.F_WorkOrderId != null)
                {
                    item.F_WorkOrderId = F_WorkOrderIdData.Find(it => it.id.Equals(item.F_WorkOrderId))?.fullName;
                }
            }

        });

        data.pagination = idsQ.pagination;

        return PageResult<APuReturnPrdListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取商品列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("GoodsList")]
    public async Task<dynamic> GetGoodsList([FromBody] APuReturnPrdListQueryInput input)
    {
        var idsQ = await _repositoryItem.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_ReturnInPRDId), it => it.F_ReturnInPRDId.Contains(input.F_ReturnInPRDId))
            .Select(it => new APuReturnPrdItemInfoOutput
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
        var data = await _repositoryItem.AsQueryable()
            .Where(it => ids.Contains(it.F_Id))
            .Select(it => new APuReturnPrdItemInfoOutput
            {
                F_Id = it.F_Id,
                F_CustomerId = it.F_CustomerId,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_UnitTwo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_Unit),
                F_Num = it.F_Num,
                F_Price = it.F_Price,
                F_Description = it.F_Description,
                F_WarehouseId = it.F_WarehouseID,
                F_Encoding = it.F_Encoding,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        // 子表仓库 JSON Id 数组 -> 与详情页一致的名称展示（供 BasicTable 使用）
        var allWarehouseIds = new HashSet<string>();
        foreach (var row in data.list)
        {
            foreach (var wid in ParseWarehouseIdList(row.F_WarehouseId))
                allWarehouseIds.Add(wid);
        }

        IReadOnlyDictionary<string, string> warehouseIdToName = new Dictionary<string, string>();
        if (allWarehouseIds.Count > 0)
        {
            var idList = allWarehouseIds.ToList();
            var whRows = await _repository.AsSugarClient().Queryable<APuWarehouseEntity>()
                .Where(w => w.DeleteMark == null && idList.Contains(w.id))
                .Select(w => new { w.id, w.F_Name })
                .ToListAsync();
            warehouseIdToName = whRows.ToDictionary(r => r.id, r => r.F_Name ?? string.Empty, StringComparer.Ordinal);
        }

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            var whIds = ParseWarehouseIdList(item.F_WarehouseId);
            if (whIds.Count > 0)
            {
                var label = FormatWarehouseNames(whIds, warehouseIdToName);
                if (!string.IsNullOrEmpty(label))
                    item.F_WarehouseId = label;
            }

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(item.F_UnitTwo))
            {
                var F_UnitIdCascader = item.F_UnitTwo.ToObject<List<string>>();
                if (F_UnitIdCascader.Count > 1)
                {
                    item.F_NumInfo = "1" + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName + item.F_Num.ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    item.F_NumInfo = item.F_Num.ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }
        });

        data.pagination = idsQ.pagination;

        return PageResult<APuReturnPrdItemInfoOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取日志列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("LogList")]
    public async Task<dynamic> GetLogList([FromBody] APuReturnPrdListQueryInput input)
    {
        var idsQ = await _repositoryLog.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_ReturnInPRDId), it => it.F_ReturnInPRDId.Contains(input.F_ReturnInPRDId))
            .Select(it => new APuReturnPrdLogInfoOutput
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
        var data = await _repositoryLog.AsQueryable()
            .Where(it => ids.Contains(it.F_Id))
            .Select(it => new APuReturnPrdLogInfoOutput
            {
                F_Id = it.F_Id,
                F_Title = it.F_Title,
                F_Content = it.F_Content,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        data.pagination = idsQ.pagination;

        return PageResult<APuReturnPrdLogInfoOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_pu_return_prd.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] APuReturnPrdCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuReturnPrdEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        // 如果前端未传入单号，则自动生成：SCTL + yyyyMMdd + 当天序号(3位)
        if (string.IsNullOrEmpty(entity.F_ReturnInNo))
        {
            var prefix = $"SCTL{DateTime.Now:yyyyMMdd}";
            // 统计当天已存在的记录数（按前缀匹配），再 +1 作为序号
            var todayCount = await _repository.AsQueryable()
                .Where(it => it.F_ReturnInNo != null && it.F_ReturnInNo.StartsWith(prefix) && it.DeleteMark == null)
                .CountAsync();
            var seq = todayCount + 1;
            entity.F_ReturnInNo = prefix + seq.ToString("D3");
        }
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuReturnPrdEntity>(new APuReturnPrdEntity()));
        ConfigModel tableFieldbb1084Config = new ConfigModel
        {
            tableName = "a_pu_return_prd_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuReturnPrdItemEntity>(new APuReturnPrdItemEntity())
        };
        FieldsModel tableFieldbb1084Model = new FieldsModel()
        {
            __config__ = tableFieldbb1084Config,
            __vModel__ = "tableFieldbb1084"
        };
        fieldList.Add(tableFieldbb1084Model);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;


        var aPuReturnPrdItemEntityList = input.tableFieldbb1084.Adapt<List<APuReturnPrdItemEntity>>();
        if(aPuReturnPrdItemEntityList != null)
        {
            foreach (var item in aPuReturnPrdItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuReturnPrdItemList = aPuReturnPrdItemEntityList;
        }


        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.APuReturnPrdItemList)
            .Include(it => it.APuReturnPrdLogList)
            .ExecuteCommandAsync();

        //日志
        var logEntity = new APuReturnPrdLogEntity();
        logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        logEntity.F_CreatorUserId = _userManager.UserId;
        logEntity.F_Id = SnowflakeIdHelper.NextId();
        logEntity.F_ReturnInPRDId = entity.id;
        logEntity.F_Title = "新增生产退料单";
        logEntity.F_Content = "生产退料单号：" + entity.F_ReturnInNo;
        await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();


        if (aPuReturnPrdItemEntityList != null && aPuReturnPrdItemEntityList.Any())
        {
                // 逐行处理库存，使用子表的 F_WarehouseID 字段（完整数组形式）
            foreach (var item in aPuReturnPrdItemEntityList)
            {
                if (string.IsNullOrEmpty(item.F_CustomerId) || item.F_Num == null || item.F_Num <= 0)
                    continue;

                // 直接使用子表行的仓库ID（完整数组），如果为空则使用主表仓库作为兜底
                var warehouseIds = !string.IsNullOrEmpty(item.F_WarehouseID)
                    ? item.F_WarehouseID
                    : entity.F_WarehouseId;

                if (string.IsNullOrEmpty(warehouseIds))
                    continue;

                // 按库存编码 F_Encoding 查询库存记录，每个编码对应一条独立库存
                var inventory = await _repositoryInventory.AsSugarClient()
                    .Queryable<AGoodsInventoryEntity>()
                    .Where(it => it.DeleteMark == null && it.F_Code == item.F_Encoding)
                    .FirstAsync();

                if (inventory != null)
                {
                    // 累加库存，并填充商品编码
                    inventory.F_Num = (inventory.F_Num ?? 0) + item.F_Num;
                    inventory.F_Code = item.F_Encoding;
                    inventory.F_WarehouseId = warehouseIds; // 存储完整的仓库数组
                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
                else
                {
                    // 新增库存记录
                    var inventoryEntity = new AGoodsInventoryEntity
                    {
                        id = SnowflakeIdHelper.NextId(),
                        F_CreatorUserId = _userManager.UserId,
                        F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime(),
                        F_Num = item.F_Num,
                        F_GoodsId = item.F_CustomerId,
                        F_WarehouseId = warehouseIds, // 存储完整的仓库数组
                        F_Code = item.F_Encoding
                    };
                    await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
                }
            }
        }
    }

    /// <summary>
    /// 更新a_pu_return_prd.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] APuReturnPrdUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        // 读取原数据用于比对变更
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        var entity = input.Adapt<APuReturnPrdEntity>();
        if (await _repository.IsAnyAsync(it => it.F_ReturnInNo.Equals(input.F_ReturnInNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "生成退料单号");

        if (string.IsNullOrEmpty(entity.F_ReturnInNo))
        {
            entity.F_ReturnInNo = oldEntity.F_ReturnInNo;
        }
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuReturnPrdEntity>(new APuReturnPrdEntity()));
        ConfigModel tableFieldbb1084Config = new ConfigModel
        {
            tableName = "a_pu_return_prd_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuReturnPrdItemEntity>(new APuReturnPrdItemEntity())
        };
        FieldsModel tableFieldbb1084Model = new FieldsModel()
        {
            __config__ = tableFieldbb1084Config,
            __vModel__ = "tableFieldbb1084"
        };
        fieldList.Add(tableFieldbb1084Model);

        // 移除生产退料单商品列表可能被删除数据
        if (input.tableFieldbb1084 !=null && input.tableFieldbb1084.Any())
            await _repository.AsSugarClient().Deleteable<APuReturnPrdItemEntity>().Where(it => it.F_ReturnInPRDId == entity.id && !input.tableFieldbb1084.Select(it => it.F_Id).ToList().Contains(it.F_Id)).ExecuteCommandAsync();
        else
            await _repository.AsSugarClient().Deleteable<APuReturnPrdItemEntity>().Where(it => it.F_ReturnInPRDId == entity.id).ExecuteCommandAsync();

        // 新增生产退料单商品列表新数据
        var aPuReturnPrdItemEntityList = input.tableFieldbb1084.Adapt<List<APuReturnPrdItemEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_ReturnInNo,
                    it.F_WarehouseId,
                    it.F_ReturnInType,
                    it.F_ReturnInDate,
                    it.F_Type,
                    it.F_WorkOrderId,
                    it.F_Method,
                    it.F_Description,
                })
                .ExecuteCommandAsync();

           
            //日志
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

            AddChange("【生产退料单号】", oldEntity.F_ReturnInNo, entity.F_ReturnInNo);
            if (oldEntity.F_WarehouseId != entity.F_WarehouseId)
            {
                string oldName = "";
                string newName = "";
                // 获取第一个仓库ID用于比较和查询
                var oldFirstWarehouseId = GetFirstWarehouseId(oldEntity.F_WarehouseId);
                var newFirstWarehouseId = GetFirstWarehouseId(entity.F_WarehouseId);
                if (!string.IsNullOrEmpty(oldFirstWarehouseId))
                {
                    oldName = await _repository.AsSugarClient().Queryable<APuWarehouseEntity>().Where(it => it.id.Equals(oldFirstWarehouseId)).Select(it => it.F_Name).FirstAsync();
                }
                if (!string.IsNullOrEmpty(newFirstWarehouseId))
                {
                    newName = await _repository.AsSugarClient().Queryable<APuWarehouseEntity>().Where(it => it.id.Equals(newFirstWarehouseId)).Select(it => it.F_Name).FirstAsync();
                }
                changeList.Add($"【仓库】 值由 {oldName} 改为 {newName}");
            }
            if (oldEntity.F_ReturnInType != entity.F_ReturnInType)
            {
                string oldName = "";
                string newName = "";
                if (oldEntity.F_ReturnInType != null)
                {
                    oldName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(oldEntity.F_ReturnInType) && it.DictionaryTypeId.Equals("2012074650393251840")).Select(it => it.FullName).FirstAsync();
                }
                if (entity.F_ReturnInType != null)
                {
                    newName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(entity.F_ReturnInType) && it.DictionaryTypeId.Equals("2012074650393251840")).Select(it => it.FullName).FirstAsync();
                }
                changeList.Add($"【入库类型】 值由 {oldName} 改为 {newName}");
            }
            if (oldEntity.F_Type != entity.F_Type)
            {
                string oldName = "";
                string newName = "";
                if (oldEntity.F_Type != null)
                {
                    oldName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(oldEntity.F_Type) && it.DictionaryTypeId.Equals("2014894783159472128")).Select(it => it.FullName).FirstAsync();
                }
                if (entity.F_Type != null)
                {
                    newName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(entity.F_Type) && it.DictionaryTypeId.Equals("2014894783159472128")).Select(it => it.FullName).FirstAsync();
                }
                changeList.Add($"【工单类型】 值由 {oldName} 改为 {newName}");
            }
            if (oldEntity.F_WorkOrderId != entity.F_WorkOrderId)
            {
                string oldName = "";
                string newName = "";
                if (oldEntity.F_WarehouseId != null)
                {
                    if (oldEntity.F_Type == "0")
                    {
                        oldName = await _repository.AsSugarClient().Queryable<AProdProcessingEntity>().Where(it => it.id.Equals(oldEntity.F_WorkOrderId)).Select(it => it.F_ProcessNo).FirstAsync();
                    }
                    else if (oldEntity.F_Type == "1")
                    {
                        oldName = await _repository.AsSugarClient().Queryable<AProdOutsourceEntity>().Where(it => it.id.Equals(oldEntity.F_WorkOrderId)).Select(it => it.F_OutsourceNo).FirstAsync();
                    }
                }
                if (entity.F_WarehouseId != null)
                {
                    if (oldEntity.F_Type == "0")
                    {
                        newName = await _repository.AsSugarClient().Queryable<AProdProcessingEntity>().Where(it => it.id.Equals(entity.F_WorkOrderId)).Select(it => it.F_ProcessNo).FirstAsync();
                    }
                    else if (oldEntity.F_Type == "1")
                    {
                        newName = await _repository.AsSugarClient().Queryable<AProdOutsourceEntity>().Where(it => it.id.Equals(entity.F_WorkOrderId)).Select(it => it.F_OutsourceNo).FirstAsync();
                    }
                }
                changeList.Add($"【工单号】 值由 {oldName} 改为 {newName}");
            }
            AddChange("【退料日期】", oldEntity.F_ReturnInDate, entity.F_ReturnInDate);
            AddChange("【备注】", oldEntity.F_Description, entity.F_Description);
            if (changeList.Any())
            {
                var logEntity = new APuReturnPrdLogEntity();
                logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                logEntity.F_CreatorUserId = _userManager.UserId;
                logEntity.F_Id = SnowflakeIdHelper.NextId();
                logEntity.F_ReturnInPRDId = entity.id;
                logEntity.F_Title = "编辑生产退料单";
                logEntity.F_Content = "生产退料单号：" + entity.F_ReturnInNo + "；修改内容：" + string.Join("；", changeList);
                await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
            }


            //库存：使用子表的仓库字段
            if (aPuReturnPrdItemEntityList != null)
            {
                // 修改商品信息
                foreach (var item in aPuReturnPrdItemEntityList)
                {
                    if (!item.F_Id.IsNullOrEmpty())
                    {
                        var prdItem = await _repositoryItem.GetFirstAsync(it => it.DeleteMark == null && it.F_Id == item.F_Id);
                        prdItem.F_Price = item.F_Price;
                        prdItem.F_Description = item.F_Description;
                        await _repositoryItem.AsUpdateable(prdItem)
                            .UpdateColumns(it => new {
                                it.F_Price,
                                it.F_Description,
                            }).ExecuteCommandAsync();
                    }
                   
                }
            }
            //if (aPuReturnPrdItemEntityList != null)
            //{
            //    //获取旧子表数据用于对比仓库变更
            //    var oldItemListFull = await _repository.AsSugarClient()
            //        .Queryable<APuReturnPrdItemEntity>()
            //        .Where(it => it.F_ReturnInPRDId == entity.id)
            //        .Select(it => new { it.F_Id, it.F_CustomerId, it.F_Num, it.F_WarehouseID })
            //        .ToListAsync();

            //    // 删除的商品需要扣减旧仓库的库存
            //    var existIds = aPuReturnPrdItemEntityList.Select(x => x.F_Id).ToHashSet();
            //    var deletedItems = oldItemListFull.Where(it => !existIds.Contains(it.F_Id)).ToList();

            //    foreach (var deleted in deletedItems)
            //    {
            //        if (string.IsNullOrEmpty(deleted.F_CustomerId) || deleted.F_Num == null)
            //            continue;

            //        // 使用旧子表的仓库ID，如果为空则用主表仓库兜底
            //        var warehouseId = !string.IsNullOrEmpty(deleted.F_WarehouseID)
            //            ? GetFirstWarehouseId(deleted.F_WarehouseID)
            //            : GetFirstWarehouseId(oldEntity.F_WarehouseId);

            //        if (string.IsNullOrEmpty(warehouseId))
            //            continue;

            //        var inventory = await _repositoryInventory.AsSugarClient()
            //            .Queryable<AGoodsInventoryEntity>()
            //            .Where(it => it.DeleteMark == null && it.F_GoodsId == deleted.F_CustomerId && it.F_WarehouseId == warehouseId)
            //            .FirstAsync();

            //        if (inventory != null)
            //        {
            //            inventory.F_Num = (inventory.F_Num - deleted.F_Num) < 0 ? 0 : (inventory.F_Num - deleted.F_Num);
            //            await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //        }
            //    }

            //    // 新增、修改商品库存
            //    foreach (var item in aPuReturnPrdItemEntityList)
            //    {
            //        // 获取子表行的仓库ID，如果为空则用主表仓库兜底
            //        var warehouseId = !string.IsNullOrEmpty(item.F_WarehouseID)
            //            ? GetFirstWarehouseId(item.F_WarehouseID)
            //            : GetFirstWarehouseId(entity.F_WarehouseId);

            //        if (item.F_Id.IsNullOrEmpty())
            //        {
            //            // 新增行
            //            item.F_CreatorUserId = _userManager.UserId;
            //            item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            //            item.F_Id = SnowflakeIdHelper.NextId();
            //            item.F_ReturnInPRDId = entity.id;
            //            await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();

            //            if (!string.IsNullOrEmpty(item.F_CustomerId) && item.F_Num != null && item.F_Num > 0 && !string.IsNullOrEmpty(warehouseId))
            //            {
            //                var inventory = await _repositoryInventory.AsSugarClient()
            //                    .Queryable<AGoodsInventoryEntity>()
            //                    .Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == warehouseId)
            //                    .FirstAsync();

            //                if (inventory != null)
            //                {
            //                    inventory.F_Num += item.F_Num;
            //                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //                }
            //                else
            //                {
            //                    var inventoryEntity = new AGoodsInventoryEntity
            //                    {
            //                        id = SnowflakeIdHelper.NextId(),
            //                        F_CreatorUserId = _userManager.UserId,
            //                        F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime(),
            //                        F_Num = item.F_Num,
            //                        F_GoodsId = item.F_CustomerId,
            //                        F_WarehouseId = warehouseId
            //                    };
            //                    await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
            //                }
            //            }
            //        }
            //        else
            //        {
            //            // 修改行：对比旧数据
            //            var oldItem = oldItemListFull.FirstOrDefault(it => it.F_Id == item.F_Id);
            //            if (oldItem == null)
            //            {
            //                // 兜底：从DB重新查
            //                var dbOldItem = await _repository.AsSugarClient()
            //                    .Queryable<APuReturnPrdItemEntity>()
            //                    .FirstAsync(it => it.F_Id == item.F_Id);
            //                if (dbOldItem != null)
            //                {
            //                    oldItem = new { dbOldItem.F_Id, dbOldItem.F_CustomerId, dbOldItem.F_Num, dbOldItem.F_WarehouseID };
            //                }
            //            }

            //            item.F_CreatorUserId = null;
            //            item.F_CreatorTime = null;
            //            item.F_ReturnInPRDId = entity.id;
            //            await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

            //            // 库存变更处理
            //            if (!string.IsNullOrEmpty(item.F_CustomerId) && item.F_Num != null)
            //            {
            //                // 判断仓库是否变更
            //                var oldWarehouseId = oldItem != null && !string.IsNullOrEmpty(oldItem.F_WarehouseID)
            //                    ? GetFirstWarehouseId(oldItem.F_WarehouseID)
            //                    : GetFirstWarehouseId(oldEntity.F_WarehouseId);

            //                // 仓库变更：旧仓库扣减，新仓库增加
            //                if (!string.IsNullOrEmpty(warehouseId) && warehouseId != oldWarehouseId)
            //                {
            //                    // 旧仓库扣减
            //                    if (!string.IsNullOrEmpty(oldWarehouseId))
            //                    {
            //                        var oldInventory = await _repositoryInventory.AsSugarClient()
            //                            .Queryable<AGoodsInventoryEntity>()
            //                            .Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == oldWarehouseId)
            //                            .FirstAsync();

            //                        if (oldInventory != null)
            //                        {
            //                            oldInventory.F_Num = (oldInventory.F_Num - (oldItem?.F_Num ?? 0)) < 0 ? 0 : (oldInventory.F_Num - (oldItem?.F_Num ?? 0));
            //                            await _repositoryInventory.AsSugarClient().Updateable(oldInventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //                        }
            //                    }

            //                    // 新仓库增加
            //                    var newInventory = await _repositoryInventory.AsSugarClient()
            //                        .Queryable<AGoodsInventoryEntity>()
            //                        .Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == warehouseId)
            //                        .FirstAsync();

            //                    if (newInventory != null)
            //                    {
            //                        newInventory.F_Num += item.F_Num;
            //                        await _repositoryInventory.AsSugarClient().Updateable(newInventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //                    }
            //                    else
            //                    {
            //                        var inventoryEntity = new AGoodsInventoryEntity
            //                        {
            //                            id = SnowflakeIdHelper.NextId(),
            //                            F_CreatorUserId = _userManager.UserId,
            //                            F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime(),
            //                            F_Num = item.F_Num,
            //                            F_GoodsId = item.F_CustomerId,
            //                            F_WarehouseId = warehouseId
            //                        };
            //                        await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
            //                    }
            //                }
            //                else
            //                {
            //                    // 仓库不变：直接用差值
            //                    if (!string.IsNullOrEmpty(warehouseId))
            //                    {
            //                        var inventory = await _repositoryInventory.AsSugarClient()
            //                            .Queryable<AGoodsInventoryEntity>()
            //                            .Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == warehouseId)
            //                            .FirstAsync();

            //                        if (inventory != null)
            //                        {
            //                            var numChange = item.F_Num - (oldItem?.F_Num ?? 0);
            //                            inventory.F_Num = (inventory.F_Num + numChange) < 0 ? 0 : (inventory.F_Num + numChange);
            //                            await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 删除a_pu_return_prd.
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

        // 获取子表数据用于扣减库存
        var itemList = await _repository.AsSugarClient()
            .Queryable<APuReturnPrdItemEntity>()
            .Where(it => it.F_ReturnInPRDId == id)
            .Select(it => new { it.F_CustomerId, it.F_Num, it.F_WarehouseID })
            .ToListAsync();

        // 扣减库存：使用子表的仓库字段
        foreach (var item in itemList)
        {
            if (string.IsNullOrEmpty(item.F_CustomerId) || item.F_Num == null)
                continue;

            var warehouseId = !string.IsNullOrEmpty(item.F_WarehouseID)
                ? GetFirstWarehouseId(item.F_WarehouseID)
                : GetFirstWarehouseId(entity.F_WarehouseId);

            if (string.IsNullOrEmpty(warehouseId))
                continue;

            var inventory = await _repositoryInventory.AsSugarClient()
                .Queryable<AGoodsInventoryEntity>()
                .Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == warehouseId)
                .FirstAsync();

            if (inventory != null)
            {
                inventory.F_Num = (inventory.F_Num - item.F_Num) < 0 ? 0 : (inventory.F_Num - item.F_Num);
                await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            }
        }

        await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
    }

    /// <summary>
    /// 批量删除a_pu_return_prd.
    /// </summary>
    /// <param name="input">主键数组.</param>
    /// <returns></returns>
    [HttpPost("batchRemove")]
    [UnitOfWork]
    public async Task BatchRemove([FromBody] BatchRemoveInput input)
    {
        var oldEntity = await _repository.GetFirstAsync(it => it.DeleteMark == null && it.id == input.ids[0]);

        var entitys = await _repository.AsQueryable().In(it => it.id, input.ids).Where(it => it.DeleteMark == null).ToListAsync();
        if (entitys.Count > 0)
        {
            // 批量删除a_pu_return_prd
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

            // 扣减库存：使用子表的仓库字段
            foreach (var entityItem in entitys)
            {
                var itemList = await _repository.AsSugarClient()
                    .Queryable<APuReturnPrdItemEntity>()
                    .Where(it => it.F_ReturnInPRDId == entityItem.id)
                    .Select(it => new { it.F_CustomerId, it.F_Num, it.F_WarehouseID })
                    .ToListAsync();

                foreach (var item in itemList)
                {
                    if (string.IsNullOrEmpty(item.F_CustomerId) || item.F_Num == null)
                        continue;

                    var warehouseId = !string.IsNullOrEmpty(item.F_WarehouseID)
                        ? GetFirstWarehouseId(item.F_WarehouseID)
                        : GetFirstWarehouseId(entityItem.F_WarehouseId);

                    if (string.IsNullOrEmpty(warehouseId))
                        continue;

                    var inventory = await _repositoryInventory.AsSugarClient()
                        .Queryable<AGoodsInventoryEntity>()
                        .Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == warehouseId)
                        .FirstAsync();

                    if (inventory != null)
                    {
                        inventory.F_Num = (inventory.F_Num - item.F_Num) < 0 ? 0 : (inventory.F_Num - item.F_Num);
                        await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }
        }
    }

    /// <summary>
    /// a_pu_return_prd详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.APuReturnPrdItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuReturnPrdLogList)
            .Where(it => it.id == id)
            .Select(it => new APuReturnPrdDetailOutput
            {
                id = it.id,
                F_ReturnInNo = it.F_ReturnInNo,
                //F_WarehouseId = it.F_WarehouseId,
                F_ReturnInType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_ReturnInType) && dic.DictionaryTypeId.Equals("2012074650393251840")).Select(dic => dic.FullName),
                F_ReturnInDate = it.F_ReturnInDate.Value.ToString("yyyy-MM-dd"),
                F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2014894783159472128")).Select(dic => dic.FullName),
                F_TypeCode = it.F_Type,
                F_WorkOrderId = it.F_WorkOrderId,
                F_Method = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Method) && dic.DictionaryTypeId.Equals("2014907575941861376")).Select(dic => dic.FullName),
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableFieldbb1084 = it.APuReturnPrdItemList.Where(it => it.DeleteMark == null).Adapt<List<APuReturnPrdItemDetailOutput>>(),
                tableFieldceb7bd = it.APuReturnPrdLogList.Where(it => it.DeleteMark == null).Adapt<List<APuReturnPrdLogDetailOutput>>(),
            }).ToListAsync();

        // 收集所有仓库ID并查询仓库名称字典
        var allWarehouseIds = new HashSet<string>();
        foreach (var row in data)
        {
            if (row.tableFieldbb1084 == null) continue;
            foreach (var line in row.tableFieldbb1084)
            {
                foreach (var wid in ParseWarehouseIdList(line.F_WarehouseID))
                    allWarehouseIds.Add(wid);
            }
        }

        IReadOnlyDictionary<string, string> warehouseIdToName = new Dictionary<string, string>();
        if (allWarehouseIds.Count > 0)
        {
            var idList = allWarehouseIds.ToList();
            var whRows = await _repository.AsSugarClient().Queryable<APuWarehouseEntity>()
                .Where(w => w.DeleteMark == null && idList.Contains(w.id))
                .Select(w => new { w.id, w.F_Name })
                .ToListAsync();
            warehouseIdToName = whRows.ToDictionary(r => r.id, r => r.F_Name ?? string.Empty, StringComparer.Ordinal);
        }

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();


            // 工单ID
            if (item.F_TypeCode == "0")
            {
                var F_WorkOrderIdData = await _dataInterfaceService.GetDynamicList("F_WorkOrderId", "2013860549426810880", "F_Id", "F_ProcessNo", "");
                if (item.F_WorkOrderId != null)
                {
                    item.F_WorkOrderId = F_WorkOrderIdData.Find(it => it.id.Equals(item.F_WorkOrderId))?.fullName;
                }
            }
            else if (item.F_TypeCode == "1")
            {
                var F_WorkOrderIdData = await _dataInterfaceService.GetDynamicList("F_WorkOrderId", "2014969808717746176", "id", "F_ProcessNo", "");
                if (item.F_WorkOrderId != null)
                {
                    item.F_WorkOrderId = F_WorkOrderIdData.Find(it => it.id.Equals(item.F_WorkOrderId))?.fullName;
                }
            }
        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFieldbb1084), async aPuReturnPrdItem =>
        {
            var aPuReturnPrd = data.ToList().Find(it => it.id.Equals(aPuReturnPrdItem.F_ReturnInPRDId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuReturnPrdItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuReturnPrdItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuReturnPrdItem.F_CreatorTime = aPuReturnPrdItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            // 如果 F_CustomerId 存在且为商品表的 id，则将其替换为对应的商品名称 F_GoodsName
            if (!string.IsNullOrEmpty(aPuReturnPrdItem.F_CustomerId))
            {
                var goodsName = _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(g => g.id.Equals(aPuReturnPrdItem.F_CustomerId)).Select(g => g.F_GoodsName).First();
                if (!string.IsNullOrEmpty(goodsName))
                {
                    aPuReturnPrdItem.F_CustomerId = goodsName;
                }
            }
            // 仓库：JSON Id 数组 -> 按级联顺序用 "-" 拼接仓库名称
            var whIds = ParseWarehouseIdList(aPuReturnPrdItem.F_WarehouseID);
            if (whIds.Count > 0)
            {
                var label = FormatWarehouseNames(whIds, warehouseIdToName);
                if (!string.IsNullOrEmpty(label))
                    aPuReturnPrdItem.F_WarehouseID = label;
            }
        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFieldceb7bd), async aPuReturnPrdLog =>
        {
            var aPuReturnPrd = data.ToList().Find(it => it.id.Equals(aPuReturnPrdLog.F_ReturnInPRDId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuReturnPrdLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuReturnPrdLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuReturnPrdLog.F_CreatorTime = aPuReturnPrdLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<APuReturnPrdEntity>(new APuReturnPrdEntity()));
        ConfigModel tableFieldbb1084Config = new ConfigModel
        {
            tableName = "a_pu_return_prd_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<APuReturnPrdItemEntity>(new APuReturnPrdItemEntity())
        };
        FieldsModel tableFieldbb1084 = new FieldsModel()
        {
            __config__ = tableFieldbb1084Config,
            __vModel__ = "tableFieldbb1084"
        };
        fieldList.Add(tableFieldbb1084);

        resData = await _controlParsing.GetParsDataList(resData,"tableFieldbb1084-F_CustomerId","popupSelect",_userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }
    
    /// <summary>
    /// 根据加工ID获取商品ID及对应的成本价
    /// 前端传入加工数据 id（processingId），返回匹配到的商品 id 及 AGoodsEntity 中的 F_CostPrice
    /// </summary>
    /// <param name="processingId">加工数据 id</param>
    /// <returns></returns>
    [HttpGet("CostsByProcessing/{processingId}")]
    public async Task<dynamic> GetCostsByProcessing(string processingId)
    {
        if (string.IsNullOrEmpty(processingId)) return new List<object>();

        // 从 APuReturnPrdItemEntity 表中找到所有匹配的 F_GoodsId
        var goodsIds = await _repositoryBomItem.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_ProcessingId == processingId)
            .Select(it => it.F_GoodsId)
            .ToListAsync();

        if (goodsIds == null || !goodsIds.Any()) return new List<object>();

        // 根据 AGoodsEntity.id 列表查询对应的成本价 F_CostPrice
        var goodsCostList = await _repositorygoods.AsQueryable()
            .Where(it => it.DeleteMark == null && goodsIds.Contains(it.id))
            .Select(it => new { it.id, it.F_CostPrice })
            .ToListAsync();

        return goodsCostList;
    }

    /// <summary>
    /// 获取仓库ID数组中的第一个值（用于库存操作）
    /// </summary>
    /// <param name="warehouseIdJson">仓库ID的JSON数组字符串，如 ["2014233789731049472","2026473367191818240"]</param>
    /// <returns>第一个仓库ID，如果解析失败则返回原值</returns>
    private string GetFirstWarehouseId(string warehouseIdJson)
    {
        if (string.IsNullOrEmpty(warehouseIdJson))
            return warehouseIdJson;

        try
        {
            // 尝试解析JSON数组
            var warehouseIds = JsonSerializer.Deserialize<List<string>>(warehouseIdJson);
            if (warehouseIds != null && warehouseIds.Count > 0)
                return warehouseIds[0];
        }
        catch
        {
            // 如果解析失败，可能是旧格式，直接返回原值
        }
        return warehouseIdJson;
    }

    /// <summary>
    /// 解析明细行中的仓库字段（JSON 数组字符串）为 Id 列表.
    /// </summary>
    private static List<string> ParseWarehouseIdList(string warehouseIdJson)
    {
        if (string.IsNullOrWhiteSpace(warehouseIdJson)) return new List<string>();
        try
        {
            var arr = warehouseIdJson.ToObject<List<string>>();
            return arr?.Where(s => !string.IsNullOrEmpty(s)).ToList() ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }

    /// <summary>
    /// 按 Id 顺序取仓库名称并用 "-" 拼接；查不到名称时保留原 Id.
    /// </summary>
    private static string FormatWarehouseNames(IReadOnlyList<string> ids, IReadOnlyDictionary<string, string> idToName)
    {
        if (ids == null || ids.Count == 0) return null;
        var names = new List<string>(ids.Count);
        foreach (var wid in ids)
        {
            if (idToName != null && idToName.TryGetValue(wid, out var n) && !string.IsNullOrEmpty(n))
                names.Add(n);
            else
                names.Add(wid);
        }
        return string.Join(" / ", names);
    }
}
