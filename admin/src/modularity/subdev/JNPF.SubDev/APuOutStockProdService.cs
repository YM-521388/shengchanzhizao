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
using JNPF.example.Entitys.Dto.APuOutStockProd;
using JNPF.example.Entitys.Dto.APuOutStockProdItem;
using JNPF.example.Entitys.Dto.APuOutStockProdLog;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_pu_out_stock_prod.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuOutStockProd", Order = 200)]
[Route("api/example/[controller]")]
public class APuOutStockProdService : IAPuOutStockProdService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuOutStockProdEntity> _repository;
    private readonly ISqlSugarRepository<APuOutStockProdItemEntity> _repositoryItem;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<APuOutStockProdLogEntity> _repositoryLog;
    private readonly ISqlSugarRepository<APuStockFgItemEntity> _repositoryStockFgItem;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositoryGoods;
    private readonly ISqlSugarRepository<APuStockFgEntity> _repositorycprk;
   

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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"生产出库单号\",\"field\":\"F_StockOutNo\"},{\"value\":\"仓库\",\"field\":\"F_WarehouseId\"},{\"value\":\"出库类型\",\"field\":\"F_StockOutType\"},{\"value\":\"出库日期\",\"field\":\"F_StockOutDate\"},{\"value\":\"工单\",\"field\":\"F_WorkOrderId\"},{\"value\":\"出库套数\",\"field\":\"F_Num\"},{\"value\":\"工单类型\",\"field\":\"F_Type\"},{\"value\":\"操作方式\",\"field\":\"F_Method\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="APuOutStockProdService"/>类型的新实例.
    /// </summary>
    public APuOutStockProdService(
        ISqlSugarRepository<AGoodsEntity> repositoryGoods,
         ISqlSugarRepository<APuStockFgEntity> repositorycprk,
        ISqlSugarRepository<APuStockFgItemEntity> repositoryStockFgItem,
        ISqlSugarRepository<APuOutStockProdLogEntity> repositoryLog,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<APuOutStockProdItemEntity> repositoryItem,
        ISqlSugarRepository<APuOutStockProdEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryItem = repositoryItem;
        _repositoryGoods = repositoryGoods;
        _repositoryStockFgItem = repositoryStockFgItem;
        _repositoryLog = repositoryLog;
        _repositoryInventory = repositoryInventory;
        _repository = repository;
        _repositorycprk = repositorycprk;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_pu_out_stock_prod.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.APuOutStockProdItemList)
            .Where(it => it.DeleteMark == null)
            .Select(it => new APuOutStockProdEntity
            {
                id = it.id,
                F_StockOutNo = it.F_StockOutNo,
                F_WarehouseId = it.F_WarehouseId,
                F_StockOutType = it.F_StockOutType,
                F_StockOutDate = it.F_StockOutDate,
                F_Num = it.F_Num,
                F_Type = it.F_Type,
                F_Method = it.F_Method,
                F_WorkOrderId = it.F_WorkOrderId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                APuOutStockProdItemList = it.APuOutStockProdItemList.Where(it => it.DeleteMark == null).ToList(),
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuOutStockProdInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField2752cf, async aPuOutStockProdItem =>
        {
            // 创建人员
            aPuOutStockProdItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOutStockProdItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOutStockProdItem.F_CreatorTime = aPuOutStockProdItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            aPuOutStockProdItem.F_InventoryNum = 0;

            var goodEntity = await _repositoryGoods.GetFirstAsync(it => it.id == aPuOutStockProdItem.F_CustomerId);
            aPuOutStockProdItem.F_Specification = goodEntity?.F_Specification;

            //库存
            var inventory = await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == aPuOutStockProdItem.F_CustomerId).SumAsync(it => it.F_Num);
            if (inventory != null)
            {
                aPuOutStockProdItem.F_InventoryNum += inventory;
            }
            aPuOutStockProdItem.F_WarehouseId = "";

        });
        return data;
    }

    /// <summary>
    /// 获取a_pu_out_stock_prod列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuOutStockProdListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuOutStockProdItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuOutStockProdLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuOutStockProdListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_out_stock_prod"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_out_stock_prod_item"))) aPuOutStockProdItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_out_stock_prod_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_out_stock_prod_log"))) aPuOutStockProdLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_out_stock_prod_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutStockProdEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutStockProdItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField2752cf-", entityInfo, 1);
        List<IConditionalModel> aPuOutStockProdItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuOutStockProdItemConditionalModel.AddRange(aPuOutStockProdItemAuthorizeWhere);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutStockProdLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField49228c-", entityInfo, 1);
        List<IConditionalModel> aPuOutStockProdLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuOutStockProdLogConditionalModel.AddRange(aPuOutStockProdLogAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        var F_WarehouseId = input.F_WarehouseId?.Last();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutStockProdEntity>();
        var F_MethodDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_Method").DbColumnName;

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_StockOutNo), it => it.F_StockOutNo.Contains(input.F_StockOutNo))
           .WhereIF(!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()), it => it.F_WarehouseId.Contains(F_WarehouseId))
            .WhereIF(!string.IsNullOrEmpty(input.F_StockOutType), it => it.F_StockOutType.Equals(input.F_StockOutType))
            .WhereIF(input.F_StockOutDate?.Count() > 0, it => SqlFunc.Between(it.F_StockOutDate, input.F_StockOutDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_StockOutDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_MethodDbColumnName, input.F_Method))
            .WhereIF(!string.IsNullOrEmpty(input.F_WorkOrderId), it => it.F_WorkOrderId.Equals(input.F_WorkOrderId))
            .Where(authorizeWhere)
            .WhereIF(aPuOutStockProdItemAuthorizeWhere?.Count > 0, it => it.APuOutStockProdItemList.Any(aPuOutStockProdItemAuthorizeWhere))
            .WhereIF(aPuOutStockProdLogAuthorizeWhere?.Count > 0, it => it.APuOutStockProdLogList.Any(aPuOutStockProdLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aPuOutStockProdItemConditionalModel.Count > 0, it => it.APuOutStockProdItemList.Any(aPuOutStockProdItemConditionalModel))
            .WhereIF(aPuOutStockProdLogConditionalModel.Count > 0, it => it.APuOutStockProdLogList.Any(aPuOutStockProdLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new APuOutStockProdListOutput
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
            .Select(it => new APuOutStockProdListOutput
            {
                id = it.id,
                F_StockOutNo = it.F_StockOutNo,
                F_WarehouseId = it.F_WarehouseId,
                F_StockOutType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_StockOutType) && dic.DictionaryTypeId.Equals("2013096194263355392")).Select(dic => dic.FullName),
                F_StockOutTypeCode = it.F_StockOutType,
                F_StockOutDate = it.F_StockOutDate.Value.ToString("yyyy-MM-dd"),
                F_Num = it.F_Num.ToString(),
                F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2014894783159472128")).Select(dic => dic.FullName),
                F_TypeCode = it.F_Type,
                F_Method = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Method) && dic.DictionaryTypeId.Equals("2014907575941861376")).Select(dic => dic.FullName),
                F_WorkOrderId = it.F_WorkOrderId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 仓库ID
            var F_WarehouseIdData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            //if (item.F_WarehouseId != null)
            //{
            //    var F_WarehouseIdCascader = item.F_WarehouseId.ToObject<List<string>>();
            //    // 获取所有匹配的仓库名称，用逗号连接
            //    var warehouseNames = F_WarehouseIdData.FindAll(it => F_WarehouseIdCascader.Contains(it.id)).Select(s => s.fullName);
            //    item.F_WarehouseId = string.Join(",", warehouseNames);
            //}
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

        return PageResult<APuOutStockProdListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取商品列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("GoodsList")]
    public async Task<dynamic> GetGoodsList([FromBody] APuOutStockProdListQueryInput input)
    {
        var idsQ = await _repositoryItem.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_StockOutProdId), it => it.F_StockOutProdId.Contains(input.F_StockOutProdId))
            .Select(it => new APuOutStockProdItemInfoOutput
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
           .Select(it => new APuOutStockProdItemInfoOutput
           {
               F_Id = it.F_Id,
               F_CustomerId = it.F_CustomerId,
               F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_GoodsName),
               F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
               F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_Specification),
               F_UnitTwo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_Unit),
               F_InventoryNum = 0,
               F_Num = it.F_Num,
               F_Price = it.F_Price,
               F_Description = it.F_Description,
               F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
               F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
           }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            //库存
            var inventory = await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId).SumAsync(it => it.F_Num);
            if (inventory != null)
            {
                item.F_InventoryNum += inventory;
            }

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(item.F_UnitTwo))
            {
                var F_UnitIdCascader = item.F_UnitTwo.ToObject<List<string>>();
                if (F_UnitIdCascader.Count > 1)
                {
                    item.F_NumInfo = "1" + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName + Math.Floor(item.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    item.F_NumInfo = Math.Floor(item.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }
        });

        data.pagination = idsQ.pagination;

        return PageResult<APuOutStockProdItemInfoOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取日志列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("LogList")]
    public async Task<dynamic> GetLogList([FromBody] APuOutStockProdListQueryInput input)
    {
        var idsQ = await _repositoryLog.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_StockOutProdId), it => it.F_StockOutProdId.Contains(input.F_StockOutProdId))
            .Select(it => new APuOutStockProdLogInfoOutput
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
            .Select(it => new APuOutStockProdLogInfoOutput
            {
                F_Id = it.F_Id,
                F_Title = it.F_Title,
                F_Content = it.F_Content,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
        });

        data.pagination = idsQ.pagination;

        return PageResult<APuOutStockProdLogInfoOutput>.SqlSugarPageResult(data);
    }


    /// <summary>
    /// 新建a_pu_out_stock_prod.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] APuOutStockProdCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuOutStockProdEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuOutStockProdEntity>(new APuOutStockProdEntity()));
        ConfigModel tableField2752cfConfig = new ConfigModel
        {
            tableName = "a_pu_out_stock_prod_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutStockProdItemEntity>(new APuOutStockProdItemEntity())
        };
        FieldsModel tableField2752cfModel = new FieldsModel()
        {
            __config__ = tableField2752cfConfig,
            __vModel__ = "tableField2752cf"
        };
        fieldList.Add(tableField2752cfModel);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        entity.F_WarehouseId = "";

        if (await _repository.IsAnyAsync(it => it.F_StockOutNo.Equals(input.F_StockOutNo) && it.FlowId == null && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "生产出库单号");

        // 自动生成编号逻辑：前缀 yyyyMMdd，后缀 3 位序号
        if (string.IsNullOrEmpty(entity.F_StockOutNo))
        {
            var prefix = "SCCK" + DateTime.Now.ToString("yyyyMMdd");

            // 查询数据库中已有相同前缀的编号
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_StockOutNo != null && it.F_StockOutNo.StartsWith(prefix))
                .Select(it => it.F_StockOutNo)
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
            entity.F_StockOutNo = prefix + nextSeq.ToString("D3");
        }


        var aPuOutStockProdItemEntityList = input.tableField2752cf.Adapt<List<APuOutStockProdItemEntity>>();
        if(aPuOutStockProdItemEntityList != null)
        {
            foreach (var item in aPuOutStockProdItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuOutStockProdItemList = aPuOutStockProdItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.APuOutStockProdItemList)
            .Include(it => it.APuOutStockProdLogList)
            .ExecuteCommandAsync();

        // 减少库存
        if (aPuOutStockProdItemEntityList != null)
        {
            foreach (var item in aPuOutStockProdItemEntityList)
            {
                var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_Code == item.F_Encoding);
                if (inventory != null)
                {
                    inventory.F_Num -= item.F_Num;
                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
            }
        }
    }

    /// <summary>
    /// 更新a_pu_out_stock_prod.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] APuOutStockProdUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        var entity = input.Adapt<APuOutStockProdEntity>();
        if (await _repository.IsAnyAsync(it => it.F_StockOutNo.Equals(input.F_StockOutNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "生成出库单号");

        if (string.IsNullOrEmpty(entity.F_StockOutNo))
        {
            entity.F_StockOutNo = oldEntity.F_StockOutNo;
        }
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuOutStockProdEntity>(new APuOutStockProdEntity()));
        ConfigModel tableField2752cfConfig = new ConfigModel
        {
            tableName = "a_pu_out_stock_prod_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutStockProdItemEntity>(new APuOutStockProdItemEntity())
        };
        FieldsModel tableField2752cfModel = new FieldsModel()
        {
            __config__ = tableField2752cfConfig,
            __vModel__ = "tableField2752cf"
        };
        fieldList.Add(tableField2752cfModel);

        var oldItemList = await _repository.AsSugarClient().Queryable<APuOutStockProdItemEntity>().Where(it => it.F_StockOutProdId == entity.id).Select(it => new { F_Id = it.F_Id, F_CustomerId = it.F_CustomerId, F_Num = it.F_Num, F_WarehouseID = it.F_WarehouseID, F_Encoding = it.F_Encoding }).ToListAsync();

        // 移除生产出库单商品列表可能被删除数据
        if (input.tableField2752cf !=null && input.tableField2752cf.Any())
            await _repository.AsSugarClient().Deleteable<APuOutStockProdItemEntity>().Where(it => it.F_StockOutProdId == entity.id && !input.tableField2752cf.Select(it => it.F_Id).ToList().Contains(it.F_Id)).ExecuteCommandAsync();
        else
            await _repository.AsSugarClient().Deleteable<APuOutStockProdItemEntity>().Where(it => it.F_StockOutProdId == entity.id).ExecuteCommandAsync();

        // 新增生产出库单商品列表新数据
        var aPuOutStockProdItemEntityList = input.tableField2752cf.Adapt<List<APuOutStockProdItemEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_StockOutNo,
                    it.F_StockOutType,
                    it.F_StockOutDate,
                    it.F_Num,
                    it.F_Type,
                    it.F_Method,
                    it.F_WorkOrderId,
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

            AddChange("【生产出库单号】", oldEntity.F_StockOutNo, entity.F_StockOutNo);
            if (oldEntity.F_WarehouseId != entity.F_WarehouseId)
            {
                string oldName = "";
                string newName = "";
                if (oldEntity.F_WarehouseId != null)
                {
                    oldName = await _repository.AsSugarClient().Queryable<APuWarehouseEntity>().Where(it => it.id.Equals(oldEntity.F_WarehouseId)).Select(it => it.F_Name).FirstAsync();
                }
                if (entity.F_WarehouseId != null)
                {
                    newName = await _repository.AsSugarClient().Queryable<APuWarehouseEntity>().Where(it => it.id.Equals(entity.F_WarehouseId)).Select(it => it.F_Name).FirstAsync();
                }
                changeList.Add($"【仓库】 值由 {oldName} 改为 {newName}");
            }
            if (oldEntity.F_StockOutType != entity.F_StockOutType)
            {
                string oldName = "";
                string newName = "";
                if (oldEntity.F_StockOutType != null)
                {
                    oldName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(oldEntity.F_StockOutType) && it.DictionaryTypeId.Equals("2012074650393251840")).Select(it => it.FullName).FirstAsync();
                }
                if (entity.F_StockOutType != null)
                {
                    newName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(entity.F_StockOutType) && it.DictionaryTypeId.Equals("2012074650393251840")).Select(it => it.FullName).FirstAsync();
                }
                changeList.Add($"【出库类型】 值由 {oldName} 改为 {newName}");
            }
            AddChange("【出库日期】", oldEntity.F_StockOutDate, entity.F_StockOutDate);
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
                    if(oldEntity.F_Type == "0")
                    {
                        oldName = await _repository.AsSugarClient().Queryable<AProdProcessingEntity>().Where(it => it.id.Equals(oldEntity.F_WorkOrderId)).Select(it => it.F_ProcessNo).FirstAsync();
                    }else if(oldEntity.F_Type == "1")
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
            AddChange("【出库套数】", oldEntity.F_Num, entity.F_Num);
            AddChange("【备注】", oldEntity.F_Description, entity.F_Description);
            if (changeList.Any())
            {
                var logEntity = new APuOutStockProdLogEntity();
                logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                logEntity.F_CreatorUserId = _userManager.UserId;
                logEntity.F_Id = SnowflakeIdHelper.NextId();
                logEntity.F_StockOutProdId = entity.id;
                logEntity.F_Title = "编辑生产出库单";
                logEntity.F_Content = "生产出库单号：" + entity.F_StockOutNo + "；修改内容：" + string.Join("；", changeList);
                await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
            }

            //库存
            if (aPuOutStockProdItemEntityList != null)
            {
                //新增、修改商品库存
                foreach (var item in aPuOutStockProdItemEntityList)
                {
                    if (!item.F_Id.IsNullOrEmpty())
                    {
                        var prodItem = await _repositoryItem.GetFirstAsync(it => it.DeleteMark == null && it.F_Id == item.F_Id);
                        prodItem.F_Price = item.F_Price;
                        prodItem.F_Description = item.F_Description;

                        await _repositoryItem.AsUpdateable(prodItem)
                            .UpdateColumns(it => new {
                                it.F_Price,
                                it.F_Description,
                            }).ExecuteCommandAsync();
                    }
                }
            }
            //if (aPuOutStockProdItemEntityList != null)
            //{
            //    //删除的商品减掉库存

            //    // 1. 先拿到“存在”的 ID 集合
            //    var existIds = aPuOutStockProdItemEntityList.Select(x => x.F_Id).ToHashSet();

            //    // 2. 找出“缺失”的行并统一扣库存
            //    var missingList = oldItemList.Where(it => !existIds.Contains(it.F_Id)).ToList();

            //    foreach (var missing in missingList)
            //    {
            //        var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_Code == missing.F_Encoding);
            //        if (inventory != null)
            //        {
            //            inventory.F_Num = inventory.F_Num + missing.F_Num;
            //            await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //        }
            //    }

            //    //新增、修改商品库存
            //    foreach (var item in aPuOutStockProdItemEntityList)
            //    {
            //        var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_Code == item.F_Encoding);
            //        if (item.F_Id.IsNullOrEmpty()){
            //            item.F_CreatorUserId = _userManager.UserId;
            //            item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            //            item.F_Id = SnowflakeIdHelper.NextId();
            //            item.F_StockOutProdId = entity.id;
            //            await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();

            //            //修改库存
            //            if (inventory != null)
            //            {
            //                inventory.F_Num = (inventory.F_Num - item.F_Num) < 0 ? 0 : (inventory.F_Num - item.F_Num);
            //                await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //            }

            //        }
            //        else{

            //            // 对比子表修改前后的值，记录变更
            //            var oldItem = await _repository.AsSugarClient().Queryable<APuOutStockProdItemEntity>().FirstAsync(it => it.F_Id == item.F_Id);

            //            item.F_CreatorUserId = null;
            //            item.F_CreatorTime = null;
            //            item.F_StockOutProdId = entity.id;
            //            await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

            //            //修改库存
            //            if (oldItem != null && inventory != null)
            //            {
            //                var num = item.F_Num - oldItem.F_Num;
            //                inventory.F_Num = (inventory.F_Num - num) < 0 ? 0 : (inventory.F_Num - num);
            //                await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
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
    /// 删除a_pu_out_stock_prod.
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

        // 删除出库单时还原库存
        var itemList = await _repository.AsSugarClient().Queryable<APuOutStockProdItemEntity>().Where(it => it.F_StockOutProdId == id).Select(it => new { F_WarehouseID = it.F_WarehouseID, F_Encoding = it.F_Encoding, F_Num = it.F_Num }).ToListAsync();
        foreach (var item in itemList)
        {
            var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_Code == item.F_Encoding);
            if (inventory != null)
            {
                inventory.F_Num = inventory.F_Num + item.F_Num;
                await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            }
        }
    }

    /// <summary>
    /// 批量删除a_pu_out_stock_prod.
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
            // 批量删除a_pu_out_stock_prod
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);


            //删除 减掉库存
            var missingList = await _repository.AsSugarClient().Queryable<APuOutStockProdItemEntity>().Where(it => it.F_StockOutProdId == input.ids[0]).Select(it => new { F_Id = it.F_Id, F_CustomerId = it.F_CustomerId, F_Num = it.F_Num, F_WarehouseID = it.F_WarehouseID, F_Encoding = it.F_Encoding }).ToListAsync();

            foreach (var missing in missingList)
            {
                var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_Code == missing.F_Encoding);
                if (inventory != null)
                {
                    inventory.F_Num = (inventory.F_Num + missing.F_Num) < 0 ? 0 : (inventory.F_Num + missing.F_Num);
                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
            }
        }
    }

    /// <summary>
    /// a_pu_out_stock_prod详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Where(it => it.id == id)
            .Select(it => new APuOutStockProdDetailOutput
            {
                id = it.id,
                F_StockOutNo = it.F_StockOutNo,
                F_WarehouseId = it.F_WarehouseId,
                F_StockOutType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_StockOutType) && dic.DictionaryTypeId.Equals("2013096194263355392")).Select(dic => dic.FullName),
                F_StockOutDate = it.F_StockOutDate.Value.ToString("yyyy-MM-dd"),
                F_Num = it.F_Num.ToString(),
                F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2014894783159472128")).Select(dic => dic.FullName),
                F_TypeCode = it.F_Type,
                F_Method = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Method) && dic.DictionaryTypeId.Equals("2014907575941861376")).Select(dic => dic.FullName),
                F_WorkOrderId = it.F_WorkOrderId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 仓库ID
            var F_WarehouseIdData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            //if (item.F_WarehouseId != null)
            //{
            //    var F_WarehouseIdCascader = item.F_WarehouseId.ToObject<List<string>>();
            //    // 获取所有匹配的仓库名称，用逗号连接
            //    var warehouseNames = F_WarehouseIdData.FindAll(it => F_WarehouseIdCascader.Contains(it.id)).Select(s => s.fullName);
            //    item.F_WarehouseId = string.Join(",", warehouseNames);
            //}

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

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        return resData.FirstOrDefault();
    }
}
