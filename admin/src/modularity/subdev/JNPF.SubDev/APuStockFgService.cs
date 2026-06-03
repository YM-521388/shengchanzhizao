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
using JNPF.example.Entitys.Dto.APuStockFg;
using JNPF.example.Entitys.Dto.APuStockFgItem;
using JNPF.example.Entitys.Dto.APuStockFgLog;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
using Aop.Api.Domain;
using System.Reflection;
using System.Text.Json;
namespace JNPF.example;

/// <summary>
/// 业务实现：a_pu_stock_fg.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuStockFg", Order = 200)]
[Route("api/example/[controller]")]
public class APuStockFgService : IAPuStockFgService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuStockFgEntity> _repository;
    private readonly ISqlSugarRepository<APuStockFgItemEntity> _repositoryItem;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<APuStockFgLogEntity> _repositoryLog;
    private readonly ISqlSugarRepository<APuStockFgItemEntity> _repositoryStockFgItem;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositoryGoods;
    private readonly ISqlSugarRepository<AProdProcessingEntity> _repositoryProcessing;
    private readonly ISqlSugarRepository<AProdOutsourceEntity> _repositoryOutsource;
    private readonly ISqlSugarRepository<AGoodsInventoryQrEntity> _repositoryInventoryQr;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"成品入库单号\",\"field\":\"F_StockInNo\"},{\"value\":\"入库类型\",\"field\":\"F_StockInType\"},{\"value\":\"入库日期\",\"field\":\"F_StockInDate\"},{\"value\":\"工单类型\",\"field\":\"F_ProcessId\"},{\"value\":\"操作方式\",\"field\":\"F_Method\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();
    /// <summary>
    /// 初始化一个<see cref="APuStockFgService"/>类型的新实例.
    /// </summary>
    public APuStockFgService(
        ISqlSugarRepository<AGoodsInventoryQrEntity> repositoryInventoryQr,
        ISqlSugarRepository<AProdOutsourceEntity> repositoryOutsource,
        ISqlSugarRepository<AProdProcessingEntity> repositoryProcessing,
        ISqlSugarRepository<AGoodsEntity> repositoryGoods,
        ISqlSugarRepository<APuStockFgItemEntity> repositoryStockFgItem,
        ISqlSugarRepository<APuStockFgLogEntity> repositoryLog,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<APuStockFgItemEntity> repositoryItem,
        ISqlSugarRepository<APuStockFgEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryInventoryQr = repositoryInventoryQr;
        _repositoryItem = repositoryItem;
        _repositoryOutsource = repositoryOutsource;
        _repositoryProcessing = repositoryProcessing;
        _repositoryGoods = repositoryGoods;
        _repositoryStockFgItem = repositoryStockFgItem;
        _repositoryLog = repositoryLog;
        _repositoryInventory = repositoryInventory;
        _repository = repository;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_pu_stock_fg.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.APuStockFgItemList)
            .Select(it => new APuStockFgEntity
            {
                id = it.id,
                F_StockInNo = it.F_StockInNo,
                F_Type = it.F_Type,
                //F_WarehouseId = it.F_WarehouseId,
                F_StockInType = it.F_StockInType,
                F_StockInDate = it.F_StockInDate,
                F_Method = it.F_Method,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                APuStockFgItemList = it.APuStockFgItemList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuStockFgInfoOutput>();

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFieldc43f9b, async aPuStockFgItem =>
        {
            // 创建人员
            aPuStockFgItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuStockFgItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuStockFgItem.F_CreatorTime = aPuStockFgItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            aPuStockFgItem.F_InventoryNum = 0;
            aPuStockFgItem.F_YseNum = 0;

            var goodEntity = await _repositoryGoods.GetFirstAsync(it => it.id == aPuStockFgItem.F_CustomerId);
            aPuStockFgItem.F_Specification = goodEntity?.F_Specification;

            //库存
            var inventory = await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == aPuStockFgItem.F_CustomerId).SumAsync(it => it.F_Num);
            if (inventory != null)
            {
                //aPuStockFgItem.F_InventoryNum += inventory;
            }

            //已入库数量
            var stockFg = await _repositoryStockFgItem.AsQueryable().Where(it => it.DeleteMark == null && it.F_WorkOrderId == aPuStockFgItem.F_WorkOrderId).SumAsync(it => it.F_Num);
            if (stockFg != null)
            {
                //aPuStockFgItem.F_YseNum += stockFg;
            }

            aPuStockFgItem.F_PlanQty = 0;
            if (data.F_Type == "0")
            {
                aPuStockFgItem.F_PlanQty = (await _repositoryProcessing.GetFirstAsync(it => it.id == aPuStockFgItem.F_WorkOrderId))?.F_PlanQty;
            }else if(data.F_Type == "1")
            {
                aPuStockFgItem.F_PlanQty = (await _repositoryOutsource.GetFirstAsync(it => it.id == aPuStockFgItem.F_WorkOrderId))?.F_PlanNum;
            }

            aPuStockFgItem.F_Unit_Ratio = goodEntity.F_Unit_Ratio;

            aPuStockFgItem.F_WarehouseId = "";
        });
        return data;
    }

    /// <summary>
    /// 获取a_pu_stock_fg列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuStockFgListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuStockFgItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuStockFgLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuStockFgListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_stock_fg"))?.conditionalModel;
            if (allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_stock_fg_item"))) aPuStockFgItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_stock_fg_item"))?.conditionalModel;
            if (allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_stock_fg_log"))) aPuStockFgLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_stock_fg_log"))?.conditionalModel;
        }
       
        var selectIds = input.selectIds?.Split(",").ToList();

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_StockInNo), it => it.F_StockInNo.Contains(input.F_StockInNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_Type), it => it.F_Type.Equals(input.F_Type))
            .WhereIF(!string.IsNullOrEmpty(input.F_StockInType), it => it.F_StockInType.Equals(input.F_StockInType))
            .WhereIF(input.F_StockInDate?.Count() > 0, it => SqlFunc.Between(it.F_StockInDate, input.F_StockInDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_StockInDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_Method), it => it.F_Method.Equals(input.F_Method))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .Where(authorizeWhere)
            .WhereIF(aPuStockFgItemAuthorizeWhere?.Count > 0, it => it.APuStockFgItemList.Any(aPuStockFgItemAuthorizeWhere))
            .WhereIF(aPuStockFgLogAuthorizeWhere?.Count > 0, it => it.APuStockFgLogList.Any(aPuStockFgLogAuthorizeWhere))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new APuStockFgListOutput
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
           .Select(it => new APuStockFgListOutput
           {
               id = it.id,
               F_StockInNo = it.F_StockInNo,
               F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2014894783159472128")).Select(dic => dic.FullName),

               F_StockInType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_StockInType) && dic.DictionaryTypeId.Equals("2012074650393251840")).Select(dic => dic.FullName),
               F_StockInTypeCode = it.F_StockInType,
               F_StockInDate = it.F_StockInDate.Value.ToString("yyyy-MM-dd"),
               F_Method = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Method) && dic.DictionaryTypeId.Equals("2014907575941861376")).Select(dic => dic.FullName),
               F_Description = it.F_Description,
               F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
               F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
           }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

        });
        data.pagination = idsQ.pagination;

        return PageResult<APuStockFgListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取商品列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("GoodsList")]
    public async Task<dynamic> GetGoodsList([FromBody] APuStockFgListQueryInput input)
    {
        var idsQ = await _repositoryItem.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_StockFgId), it => it.F_StockInFGId.Contains(input.F_StockFgId))
            .Select(it => new APuStockFgItemInfoOutput
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
            .Select(it => new APuStockFgItemInfoOutput
            {
                F_Id = it.F_Id,
                F_CustomerId = it.F_CustomerId,
                F_WorkOrderId = it.F_WorkOrderId,
                F_WarehouseId = it.F_WarehouseID,
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

            // 仓库ID
            var F_WarehouseIdData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (item.F_WarehouseId != null)
            {
                var F_WarehouseIdCascader = item.F_WarehouseId.ToObject<List<string>>();
                item.F_WarehouseId = string.Join("/", F_WarehouseIdData.FindAll(it => F_WarehouseIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

            //库存
            //var inventory = await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId).SumAsync(it => it.F_Num);
            //if (inventory != null)
            //{
            //    item.F_InventoryNum += inventory;
            //}

            //已入库数量
            //var stockFg = await _repositoryStockFgItem.AsQueryable().Where(it => it.DeleteMark == null && it.F_WorkOrderId == item.F_WorkOrderId).SumAsync(it => it.F_Num);
            //if (stockFg != null)
            //{
            //    item.F_YseNum += stockFg;
            //}

            item.F_PlanQty = 0;
            var stockFgEntity = await _repository.GetFirstAsync(it => it.id == input.F_StockFgId);
            if (stockFgEntity.F_Type == "0")
            {
                item.F_PlanQty = (await _repositoryProcessing.GetFirstAsync(it => it.id == item.F_WorkOrderId)).F_PlanQty;
                item.F_WorkOrderId = (await _repositoryProcessing.GetFirstAsync(it => it.id == item.F_WorkOrderId)).F_ProcessNo;
            }
            else if (stockFgEntity.F_Type == "1")
            {
                item.F_PlanQty = (await _repositoryOutsource.GetFirstAsync(it => it.id == item.F_WorkOrderId)).F_PlanNum;
                item.F_WorkOrderId = (await _repositoryOutsource.GetFirstAsync(it => it.id == item.F_WorkOrderId)).F_OutsourceNo;
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

        return PageResult<APuStockFgItemInfoOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取日志列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("LogList")]
    public async Task<dynamic> GetLogList([FromBody] APuStockFgListQueryInput input)
    {
        var data = await _repositoryLog.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_StockFgId), it => it.F_StockInFGId.Contains(input.F_StockFgId))
            .Select(it => new APuStockFgLogListOutput
            {
                id = it.F_Id,
                F_StockInFGId = it.F_StockInFGId,
                F_Title = it.F_Title,
                F_Content = it.F_Content,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        return PageResult<APuStockFgLogListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_pu_stock_fg.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] APuStockFgCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuStockFgEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuStockFgEntity>(new APuStockFgEntity()));
        ConfigModel tableFieldc43f9bConfig = new ConfigModel
        {
            tableName = "a_pu_stock_fg_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuStockFgItemEntity>(new APuStockFgItemEntity())
        };
        FieldsModel tableFieldc43f9bModel = new FieldsModel()
        {
            __config__ = tableFieldc43f9bConfig,
            __vModel__ = "tableFieldc43f9b"
        };
        fieldList.Add(tableFieldc43f9bModel);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        if (await _repository.IsAnyAsync(it => it.F_StockInNo.Equals(input.F_StockInNo) && it.FlowId == null && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "生产出库单号");

        // 自动生成编号逻辑：前缀 yyyyMMdd，后缀 3 位序号
        if (string.IsNullOrEmpty(entity.F_StockInNo))
        {
            var prefix = "CPRK" + DateTime.Now.ToString("yyyyMMdd");

            // 查询数据库中已有相同前缀的编号
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_StockInNo != null && it.F_StockInNo.StartsWith(prefix))
                .Select(it => it.F_StockInNo)
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
            entity.F_StockInNo = prefix + nextSeq.ToString("D3");
        }


        var aPuStockFgItemEntityList = input.tableFieldc43f9b.Adapt<List<APuStockFgItemEntity>>();
        if (aPuStockFgItemEntityList != null)
        {
            foreach (var item in aPuStockFgItemEntityList)
            {
                var inventoryEntity = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_Code == item.F_Encoding);
                if (inventoryEntity != null) throw Oops.Bah(ErrorCode.COM1018, "库存编码："+ item.F_Encoding+" 已存在该编码，请重新填写该库存编码！");
                //var inventoryQrEntity = await _repositoryInventoryQr.GetFirstAsync(it => it.DeleteMark == null && it.F_Code == item.F_Encoding);
                //if (inventoryEntity != null) throw Oops.Bah(ErrorCode.COM1018, "库存编码：" + item.F_Encoding + " 已存在该编码，请重新填写该库存编码！");

                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuStockFgItemList = aPuStockFgItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.APuStockFgItemList)
            .Include(it => it.APuStockFgLogList)
            .ExecuteCommandAsync();

        //日志
        var logEntity = new APuStockFgLogEntity();
        logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        logEntity.F_CreatorUserId = _userManager.UserId;
        logEntity.F_Id = SnowflakeIdHelper.NextId();
        logEntity.F_StockInFGId = entity.id;
        logEntity.F_Title = "新增成品入库单";
        logEntity.F_Content = "成品入库单号：" + entity.F_StockInNo;
        await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();

        //库存
        if (aPuStockFgItemEntityList != null)
        {
            foreach (var item in aPuStockFgItemEntityList)
            {
                var warehouseId = item.F_WarehouseID;

                var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_Code == item.F_Encoding);
                if (inventory != null)
                {
                    inventory.F_Num += item.F_Num;
                    // 如果库存编码为空，则设置为子表的库存编码
                    if (string.IsNullOrEmpty(inventory.F_Code))
                    {
                        inventory.F_Code = item.F_Encoding;
                    }
                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
                else
                {
                    var inventoryEntity = new AGoodsInventoryEntity();
                    inventoryEntity.id = SnowflakeIdHelper.NextId();
                    inventoryEntity.F_CreatorUserId = _userManager.UserId;
                    inventoryEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                    inventoryEntity.F_Num = item.F_Num;
                    inventoryEntity.F_GoodsId = item.F_CustomerId;
                    inventoryEntity.F_WarehouseId = warehouseId;
                    inventoryEntity.F_Code = item.F_Encoding;
                    await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
                }
            }
        }

    }

    /// <summary>
    /// 更新a_pu_stock_fg.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] APuStockFgUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        var entity = input.Adapt<APuStockFgEntity>();
        if (await _repository.IsAnyAsync(it => it.F_StockInNo.Equals(input.F_StockInNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "成品入库单号");

        if (string.IsNullOrEmpty(entity.F_StockInNo))
        {
            entity.F_StockInNo = oldEntity.F_StockInNo;
        }
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuStockFgEntity>(new APuStockFgEntity()));
        ConfigModel tableFieldc43f9bConfig = new ConfigModel
        {
            tableName = "a_pu_stock_fg_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuStockFgItemEntity>(new APuStockFgItemEntity())
        };
        FieldsModel tableFieldc43f9bModel = new FieldsModel()
        {
            __config__ = tableFieldc43f9bConfig,
            __vModel__ = "tableFieldc43f9b"
        };
        fieldList.Add(tableFieldc43f9bModel);

        var oldItemList = await _repository.AsSugarClient().Queryable<APuStockFgItemEntity>().Where(it => it.F_StockInFGId == entity.id).Select(it => new { F_Id = it.F_Id, F_CustomerId = it.F_CustomerId, F_Num = it.F_Num, F_WarehouseID = it.F_WarehouseID, F_Encoding = it.F_Encoding }).ToListAsync();

        // 移除成品入库单商品列表可能被删除数据
        if (input.tableFieldc43f9b != null && input.tableFieldc43f9b.Any())
            await _repository.AsSugarClient().Deleteable<APuStockFgItemEntity>().Where(it => it.F_StockInFGId == entity.id && !input.tableFieldc43f9b.Select(it => it.F_Id).ToList().Contains(it.F_Id)).ExecuteCommandAsync();
        else
            await _repository.AsSugarClient().Deleteable<APuStockFgItemEntity>().Where(it => it.F_StockInFGId == entity.id).ExecuteCommandAsync();

        // 新增成品入库单商品列表新数据
        var aPuStockFgItemEntityList = input.tableFieldc43f9b.Adapt<List<APuStockFgItemEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_StockInNo,
                    it.F_Type,
                    it.F_WarehouseId,
                    it.F_StockInType,
                    it.F_StockInDate,
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

            AddChange("【成品入库单号】", oldEntity.F_StockInNo, entity.F_StockInNo);
            AddChange("【入库日期】", oldEntity.F_StockInDate, entity.F_StockInDate);
            if (oldEntity.F_WarehouseId != entity.F_WarehouseId)
            {
                string oldName = "";
                string newName = "";
                // 直接使用前端传来的仓库ID进行查询
                if (!string.IsNullOrEmpty(oldEntity.F_WarehouseId))
                {
                    oldName = await _repository.AsSugarClient().Queryable<APuWarehouseEntity>().Where(it => it.id.Equals(oldEntity.F_WarehouseId)).Select(it => it.F_Name).FirstAsync();
                }
                if (!string.IsNullOrEmpty(entity.F_WarehouseId))
                {
                    newName = await _repository.AsSugarClient().Queryable<APuWarehouseEntity>().Where(it => it.id.Equals(entity.F_WarehouseId)).Select(it => it.F_Name).FirstAsync();
                }
                changeList.Add($"【入库仓库】 值由 {oldName} 改为 {newName}");
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
            if (oldEntity.F_StockInType != entity.F_StockInType)
            {
                string oldName = "";
                string newName = "";
                if (oldEntity.F_StockInType != null)
                {
                    oldName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(oldEntity.F_StockInType) && it.DictionaryTypeId.Equals("2012074650393251840")).Select(it => it.FullName).FirstAsync();
                }
                if (entity.F_StockInType != null)
                {
                    newName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(entity.F_StockInType) && it.DictionaryTypeId.Equals("2012074650393251840")).Select(it => it.FullName).FirstAsync();
                }
                changeList.Add($"【入库类型】 值由 {oldName} 改为 {newName}");
            }
            AddChange("【备注】", oldEntity.F_Description, entity.F_Description);
            if (changeList.Any())
            {
                var logEntity = new APuStockFgLogEntity();
                logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                logEntity.F_CreatorUserId = _userManager.UserId;
                logEntity.F_Id = SnowflakeIdHelper.NextId();
                logEntity.F_StockInFGId = entity.id;
                logEntity.F_Title = "编辑成品入库单";
                logEntity.F_Content = "成品入库单号：" + entity.F_StockInNo + "；修改内容：" + string.Join("；", changeList);
                await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
            }


            //库存
            if (aPuStockFgItemEntityList != null)
            {
                foreach (var item in aPuStockFgItemEntityList)
                {
                    if (!item.F_Id.IsNullOrEmpty())
                    {
                        var fgItem = await _repositoryItem.GetFirstAsync(it => it.DeleteMark == null && it.F_Id == item.F_Id);
                        fgItem.F_Price = item.F_Price;
                        fgItem.F_Description = item.F_Description;
                        await _repositoryItem.AsUpdateable(fgItem)
                            .UpdateColumns(it => new {
                                it.F_Price,
                                it.F_Description,
                            }).ExecuteCommandAsync();

                    }
                }
            }
            //if (aPuStockFgItemEntityList != null)
            //{
            //    // 1. 先拿到"存在"的 ID 集合
            //    var existIds = aPuStockFgItemEntityList.Select(x => x.F_Id).ToHashSet();

            //    // 2. 找出"缺失"的行并统一扣库存
            //    var missingList = oldItemList.Where(it => !existIds.Contains(it.F_Id)).ToList();
            //    foreach (var missing in missingList)
            //    {
            //        var missingWarehouseId = missing.F_WarehouseID;
            //        if (string.IsNullOrEmpty(missingWarehouseId)) continue;

            //        var inventory = await GetFgInventoryAsync(missing.F_CustomerId, missingWarehouseId, missing.F_Encoding);
            //        if (inventory != null)
            //        {
            //            inventory.F_Num = (inventory.F_Num - (missing.F_Num ?? 0)) < 0 ? 0 : (inventory.F_Num - (missing.F_Num ?? 0));
            //            await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //        }
            //    }

            //    //新增、修改商品库存
            //    foreach (var item in aPuStockFgItemEntityList)
            //    {
            //        var itemWarehouseId = item.F_WarehouseID;

            //        if (string.IsNullOrEmpty(itemWarehouseId))
            //        {
            //            continue;
            //        }

            //        var inventory = await GetFgInventoryAsync(item.F_CustomerId, itemWarehouseId, item.F_Encoding);
            //        if (item.F_Id.IsNullOrEmpty())
            //        {
            //            item.F_CreatorUserId = _userManager.UserId;
            //            item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            //            item.F_Id = SnowflakeIdHelper.NextId();
            //            item.F_StockInFGId = entity.id;
            //            await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();

            //            //新增库存
            //            if (inventory != null)
            //            {
            //                inventory.F_Num += item.F_Num ?? 0;
            //                await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //            }
            //            else
            //            {
            //                var inventoryEntity = new AGoodsInventoryEntity();
            //                inventoryEntity.id = SnowflakeIdHelper.NextId();
            //                inventoryEntity.F_CreatorUserId = _userManager.UserId;
            //                inventoryEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            //                inventoryEntity.F_Num = item.F_Num ?? 0;
            //                inventoryEntity.F_GoodsId = item.F_CustomerId;
            //                inventoryEntity.F_WarehouseId = itemWarehouseId;
            //                inventoryEntity.F_Code = item.F_Encoding;
            //                await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
            //            }
            //        }
            //        else
            //        {
            //            // 从已获取的oldItemList中查找旧值
            //            var oldItem = oldItemList.FirstOrDefault(it => it.F_Id == item.F_Id);

            //            item.F_CreatorUserId = null;
            //            item.F_CreatorTime = null;
            //            item.F_StockInFGId = entity.id;
            //            await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

            //            //修改库存（商品/仓库/库存编码 任一变化则按旧键扣减、新键增加；否则同键差量）
            //            if (oldItem != null)
            //            {
            //                var oldWarehouseId = oldItem.F_WarehouseID;
            //                var newWarehouseId = itemWarehouseId;
            //                var keyChanged =
            //                    !string.Equals(oldItem.F_CustomerId, item.F_CustomerId, StringComparison.Ordinal)
            //                    || !string.Equals(oldWarehouseId ?? "", newWarehouseId ?? "", StringComparison.Ordinal)
            //                    || !string.Equals(oldItem.F_Encoding ?? "", item.F_Encoding ?? "", StringComparison.Ordinal);

            //                if (keyChanged)
            //                {
            //                    // 原键扣减
            //                    if (!string.IsNullOrEmpty(oldWarehouseId))
            //                    {
            //                        var oldInventory = await GetFgInventoryAsync(oldItem.F_CustomerId, oldWarehouseId, oldItem.F_Encoding);
            //                        if (oldInventory != null)
            //                        {
            //                            oldInventory.F_Num = (oldInventory.F_Num - (oldItem.F_Num ?? 0)) < 0 ? 0 : (oldInventory.F_Num - (oldItem.F_Num ?? 0));
            //                            await _repositoryInventory.AsSugarClient().Updateable(oldInventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //                        }
            //                    }
            //                    // 新键增加
            //                    if (inventory != null)
            //                    {
            //                        inventory.F_Num += item.F_Num ?? 0;
            //                        await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //                    }
            //                    else
            //                    {
            //                        var inventoryEntity = new AGoodsInventoryEntity();
            //                        inventoryEntity.id = SnowflakeIdHelper.NextId();
            //                        inventoryEntity.F_CreatorUserId = _userManager.UserId;
            //                        inventoryEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            //                        inventoryEntity.F_Num = item.F_Num ?? 0;
            //                        inventoryEntity.F_GoodsId = item.F_CustomerId;
            //                        inventoryEntity.F_WarehouseId = newWarehouseId;
            //                        inventoryEntity.F_Code = item.F_Encoding;
            //                        await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
            //                    }
            //                }
            //                else
            //                {
            //                    // 同一商品+仓库+编码，差量更新
            //                    if (inventory != null)
            //                    {
            //                        var newNum = item.F_Num ?? 0;
            //                        var oldNum = oldItem.F_Num ?? 0;
            //                        var num = newNum - oldNum;
            //                        inventory.F_Num = (inventory.F_Num + num) < 0 ? 0 : (inventory.F_Num + num);
            //                        await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
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
    /// 按商品ID、仓库ID、库存编码定位总库存（子表明细 F_Encoding 对应 a_goods_inventory.F_Code）.
    /// </summary>
    private async Task<AGoodsInventoryEntity?> GetFgInventoryAsync(string? encoding)
    {
        if (string.IsNullOrEmpty(encoding)) return null;
        return await _repositoryInventory.GetFirstAsync(it =>
            it.DeleteMark == null
            && it.F_Code == encoding);
    }

    /// <summary>
    /// 删除a_pu_stock_fg.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [UnitOfWork]
    public async Task Delete(string id)
    {
        if (!await _repository.IsAnyAsync(it => it.id == id))
        {
            throw Oops.Oh(ErrorCode.COM1005);
        }

        var entity = await _repository.GetFirstAsync(it => it.id.Equals(id));
        await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
    }

    /// <summary>
    /// 批量删除a_pu_stock_fg.
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
            // 批量删除a_pu_stock_fg
            await _repository.AsDeleteable().In(it => it.id, input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

            //删除 减掉库存
            var missingList = await _repository.AsSugarClient().Queryable<APuStockFgItemEntity>().Where(it => it.F_StockInFGId == input.ids[0]).Select(it => new { F_Id = it.F_Id, F_CustomerId = it.F_CustomerId, F_Num = it.F_Num, F_WarehouseID = it.F_WarehouseID }).ToListAsync();

            foreach (var missing in missingList)
            {
                var warehouseId = missing.F_WarehouseID;
                var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == missing.F_CustomerId && it.F_WarehouseId == warehouseId);
                if (inventory != null)
                {
                    inventory.F_Num = (inventory.F_Num - missing.F_Num) < 0 ? 0 : (inventory.F_Num - missing.F_Num);
                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
            }
        }
    }

    /// <summary>
    /// a_pu_stock_fg详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.APuStockFgItemList)
            .Where(it => it.id == id)
            .Select(it => new APuStockFgDetailOutput
            {
                id = it.id,
                F_StockInNo = it.F_StockInNo,
                F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2014894783159472128")).Select(dic => dic.FullName),
                F_WarehouseId = it.F_WarehouseId,
                F_StockInType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_StockInType) && dic.DictionaryTypeId.Equals("2012074650393251840")).Select(dic => dic.FullName),
                F_StockInDate = it.F_StockInDate.Value.ToString("yyyy-MM-dd"),
                F_Method = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Method) && dic.DictionaryTypeId.Equals("2014907575941861376")).Select(dic => dic.FullName),
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 入库仓库ID
            var F_WarehouseIdData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (item.F_WarehouseId != null)
            {
                var F_WarehouseIdCascader = item.F_WarehouseId.ToObject<List<string>>();
                item.F_WarehouseId = F_WarehouseIdData.FindAll(it => F_WarehouseIdCascader.Contains(it.id)).Select(s => s.fullName).FirstOrDefault();
            }


        });
        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();

        return resData.FirstOrDefault();
    }
}
