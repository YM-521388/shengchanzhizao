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
using JNPF.example.Entitys.Dto.APuStockOther;
using JNPF.example.Entitys.Dto.APuStockOtherItem;
using JNPF.example.Entitys.Dto.APuStockOtherLog;
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
/// 业务实现：a_pu_stock_other.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuStockOther", Order = 200)]
[Route("api/example/[controller]")]
public class APuStockOtherService : IAPuStockOtherService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuStockOtherEntity> _repository;
    private readonly ISqlSugarRepository<APuStockOtherItemEntity> _repositoryItem;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<APuWarehouseEntity> _repositoryck;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositoryGoods;



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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"其他入库单号\",\"field\":\"F_StockInNo\"},{\"value\":\"供应商\",\"field\":\"F_SupplierId\"},{\"value\":\"入库类型\",\"field\":\"F_StockInType\"},{\"value\":\"入库日期\",\"field\":\"F_StockInDate\"},{\"value\":\"收货人\",\"field\":\"F_StockInUserId\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="APuStockOtherService"/>类型的新实例.
    /// </summary>
    public APuStockOtherService(
        ISqlSugarRepository<AGoodsEntity> repositoryGoods,
        ISqlSugarRepository<APuStockOtherItemEntity> repositoryItem,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<APuStockOtherEntity> repository,
        ISqlSugarRepository<APuWarehouseEntity> repositoryck,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryGoods = repositoryGoods;
        _repositoryItem = repositoryItem;
        _repositoryInventory = repositoryInventory;
        _repository = repository;
        _repositoryck = repositoryck;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

   
    /// <summary>
    /// 获取a_pu_stock_other.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.APuStockOtherItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuStockOtherLogList)
            .Select(it => new APuStockOtherEntity
            {
                id = it.id,
                F_StockInNo = it.F_StockInNo,
                F_SupplierId = it.F_SupplierId,
                //F_WarehouseId = it.F_WarehouseId,
                F_StockInType = it.F_StockInType,
                F_StockInDate = it.F_StockInDate,
                F_StockInUserId = it.F_StockInUserId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                APuStockOtherItemList = it.APuStockOtherItemList.Where(item => item.DeleteMark == null).ToList(),
                APuStockOtherLogList = it.APuStockOtherLogList.Where(log => log.DeleteMark == null).ToList(),
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuStockOtherInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFieldecf5cb, async aPuStockOtherItem =>
        {
            // 创建人员
            aPuStockOtherItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuStockOtherItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuStockOtherItem.F_CreatorTime = aPuStockOtherItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            var goodEntity = await _repositoryGoods.GetFirstAsync(it => it.id == aPuStockOtherItem.F_CustomerId);
            aPuStockOtherItem.F_Unit_Ratio = goodEntity.F_Unit_Ratio;
        });
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFielde1e6d9, async aPuStockOtherLog =>
        {
            // 创建人员
            aPuStockOtherLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuStockOtherLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuStockOtherLog.F_CreatorTime = aPuStockOtherLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

    /// <summary>
    /// 获取a_pu_stock_other列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuStockOtherListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuStockOtherItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuStockOtherLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuStockOtherListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_stock_other"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_stock_other_item"))) aPuStockOtherItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_stock_other_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_stock_other_log"))) aPuStockOtherLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_stock_other_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuStockOtherEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuStockOtherItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldecf5cb-", entityInfo, 1);
        List<IConditionalModel> aPuStockOtherItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuStockOtherItemConditionalModel.AddRange(aPuStockOtherItemAuthorizeWhere);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuStockOtherLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFielde1e6d9-", entityInfo, 1);
        List<IConditionalModel> aPuStockOtherLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuStockOtherLogConditionalModel.AddRange(aPuStockOtherLogAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuStockOtherEntity>();
       
        var F_StockInTypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_StockInType").DbColumnName;

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_StockInNo), it => it.F_StockInNo.Contains(input.F_StockInNo))

            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_StockInTypeDbColumnName, input.F_StockInType))
            .WhereIF(input.F_StockInDate?.Count() > 0, it => SqlFunc.Between(it.F_StockInDate, input.F_StockInDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_StockInDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .WhereIF(aPuStockOtherItemAuthorizeWhere?.Count > 0, it => it.APuStockOtherItemList.Any(aPuStockOtherItemAuthorizeWhere))
            .WhereIF(aPuStockOtherLogAuthorizeWhere?.Count > 0, it => it.APuStockOtherLogList.Any(aPuStockOtherLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aPuStockOtherItemConditionalModel.Count > 0, it => it.APuStockOtherItemList.Any(aPuStockOtherItemConditionalModel))
            .WhereIF(aPuStockOtherLogConditionalModel.Count > 0, it => it.APuStockOtherLogList.Any(aPuStockOtherLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new APuStockOtherListOutput
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
            .Select(it => new APuStockOtherListOutput
            {
                id = it.id,
                F_StockInNo = it.F_StockInNo,
                F_SupplierId = it.F_SupplierId,

                F_StockInType = it.F_StockInType,
                F_StockInDate = it.F_StockInDate.Value.ToString("yyyy-MM-dd"),
                F_StockInUserId = it.F_StockInUserId,
                F_Description = it.F_Description,
                F_CreatorUserId = it.F_CreatorUserId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 供应商ID
            var F_SupplierIdData = await _dataInterfaceService.GetDynamicList("F_SupplierId", "2008457397298925568", "F_Id", "F_SupplierName", "");
            if (item.F_SupplierId != null)
            {
                item.F_SupplierId = F_SupplierIdData.Find(it => it.id.Equals(item.F_SupplierId))?.fullName;
            }


            // 入库类型
            if (item.F_StockInType != null)
            {
                item.F_StockInType = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(item.F_StockInType) && it.DictionaryTypeId.Equals("2012074650393251840")).Select(it => it.FullName).FirstAsync();
            }

            // 收货人
            if (item.F_StockInUserId != null)
            {
                item.F_StockInUserId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_StockInUserId)))?.RealName;
            }

            // 创建人员
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;

        });

        data.pagination = idsQ.pagination;

        return PageResult<APuStockOtherListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_pu_stock_other.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] APuStockOtherCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuStockOtherEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuStockOtherEntity>(new APuStockOtherEntity()));
        ConfigModel tableFieldecf5cbConfig = new ConfigModel
        {
            tableName = "a_pu_stock_other_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "入库商品列表",
            children = ExportImportDataHelper.GetTemplateParsing<APuStockOtherItemEntity>(new APuStockOtherItemEntity())
        };
        FieldsModel tableFieldecf5cbModel = new FieldsModel()
        {
            __config__ = tableFieldecf5cbConfig,
            __vModel__ = "tableFieldecf5cb"
        };
        fieldList.Add(tableFieldecf5cbModel);
        ConfigModel tableFielde1e6d9Config = new ConfigModel
        {
            tableName = "a_pu_stock_other_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "设计子表",
            children = ExportImportDataHelper.GetTemplateParsing<APuStockOtherLogEntity>(new APuStockOtherLogEntity())
        };
        FieldsModel tableFielde1e6d9Model = new FieldsModel()
        {
            __config__ = tableFielde1e6d9Config,
            __vModel__ = "tableFielde1e6d9"
        };
        fieldList.Add(tableFielde1e6d9Model);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        // 自动生成入库单号
        if (string.IsNullOrEmpty(entity.F_StockInNo))
        {
            var today = DateTime.Now.ToString("yyyyMMdd");
            var prefix = $"QTRK{today}";
            var count = await _repository.AsQueryable()
                .Where(it => it.F_StockInNo.StartsWith(prefix) && it.DeleteMark == null)
                .CountAsync();
            entity.F_StockInNo = $"{prefix}{(count + 1).ToString("D3")}";
        }

        var aPuStockOtherItemEntityList = input.tableFieldecf5cb.Adapt<List<APuStockOtherItemEntity>>();
        if(aPuStockOtherItemEntityList != null)
        {
            foreach (var item in aPuStockOtherItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuStockOtherItemList = aPuStockOtherItemEntityList;
        }

        var aPuStockOtherLogEntityList = input.tableFielde1e6d9.Adapt<List<APuStockOtherLogEntity>>();
        if(aPuStockOtherLogEntityList != null)
        {
            foreach (var item in aPuStockOtherLogEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuStockOtherLogList = aPuStockOtherLogEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.APuStockOtherItemList)
            .Include(it => it.APuStockOtherLogList)
            .ExecuteCommandAsync();

        if (aPuStockOtherItemEntityList != null)
        {
            foreach (var item in aPuStockOtherItemEntityList)
            {
                var warehouseId = !string.IsNullOrEmpty(item.F_WarehouseID)
                    ? GetWarehouseId(item.F_WarehouseID)
                    : GetWarehouseId(entity.F_WarehouseId);

                var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_Code == item.F_Encoding);
                if (inventory != null)
                {
                    inventory.F_Num += item.F_Num;
                    // 同时更新库存编码
                    if (!string.IsNullOrEmpty(item.F_Encoding))
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
                    // 保存库存编码
                    inventoryEntity.F_Code = item.F_Encoding;
                    await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
                }
            }
        }
    }

    /// <summary>
    /// 更新a_pu_stock_other.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] APuStockOtherUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuStockOtherEntity>();
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_StockInNo.Equals(input.F_StockInNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "其他入库单号");

        if (string.IsNullOrEmpty(entity.F_StockInNo))
        {
            entity.F_StockInNo = oldEntity.F_StockInNo;
        }
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuStockOtherEntity>(new APuStockOtherEntity()));
        ConfigModel tableFieldecf5cbConfig = new ConfigModel
        {
            tableName = "a_pu_stock_other_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "入库商品列表",
            children = ExportImportDataHelper.GetTemplateParsing<APuStockOtherItemEntity>(new APuStockOtherItemEntity())
        };
        FieldsModel tableFieldecf5cbModel = new FieldsModel()
        {
            __config__ = tableFieldecf5cbConfig,
            __vModel__ = "tableFieldecf5cb"
        };
        fieldList.Add(tableFieldecf5cbModel);
        ConfigModel tableFielde1e6d9Config = new ConfigModel
        {
            tableName = "a_pu_stock_other_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "设计子表",
            children = ExportImportDataHelper.GetTemplateParsing<APuStockOtherLogEntity>(new APuStockOtherLogEntity())
        };
        FieldsModel tableFielde1e6d9Model = new FieldsModel()
        {
            __config__ = tableFielde1e6d9Config,
            __vModel__ = "tableFielde1e6d9"
        };
        fieldList.Add(tableFielde1e6d9Model);

        var oldItemList = await _repository.AsSugarClient().Queryable<APuStockOtherItemEntity>().Where(it => it.F_StockInOTId == entity.id).Select(it => new { F_Id = it.F_Id, F_CustomerId = it.F_CustomerId, F_Num = it.F_Num, F_WarehouseID = it.F_WarehouseID }).ToListAsync();

        // 移除其他入库单商品列表可能被删除数据
        if (input.tableFieldecf5cb !=null && input.tableFieldecf5cb.Any())
            await _repository.AsSugarClient().Deleteable<APuStockOtherItemEntity>().Where(it => it.F_StockInOTId == entity.id && !input.tableFieldecf5cb.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<APuStockOtherItemEntity>().Where(it => it.F_StockInOTId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增其他入库单商品列表新数据
        var aPuStockOtherItemEntityList = input.tableFieldecf5cb.Adapt<List<APuStockOtherItemEntity>>();


        // 记录修改前数据，准备生成操作日志
        var logList = new List<APuStockOtherLogEntity>();

        // 字段中文映射
        var fieldDisplay = new Dictionary<string, string>
        {
            { "F_StockInNo", "其他入库单号" },
            //{ "F_SupplierId", "供应商" },
            //{ "F_WarehouseId", "入库仓库" },
            //{ "F_StockInType", "入库类型" },
            { "F_StockInDate", "入库日期" },
            //{ "F_StockInUserId", "收货人" },
            { "F_Description", "备注" }
        };

        // 如果存在要删除的子表数据，先查询它们以便记录日志
        List<APuStockOtherItemEntity> willDeletedItems = new List<APuStockOtherItemEntity>();
        if (input.tableFieldecf5cb != null && input.tableFieldecf5cb.Any())
        {
            var keepIds = input.tableFieldecf5cb.Select(it => it.F_Id).ToList();
            willDeletedItems = await _repository.AsSugarClient().Queryable<APuStockOtherItemEntity>()
                .Where(it => it.F_StockInOTId == entity.id && !keepIds.Contains(it.F_Id) && it.DeleteMark == null)
                .ToListAsync();
        }

        // 移除其他入库单日志可能被删除数据
        if (input.tableFielde1e6d9 != null && input.tableFielde1e6d9.Any())
            await _repository.AsSugarClient().Deleteable<APuStockOtherLogEntity>().Where(it => it.F_StockInOTId == entity.id && !input.tableFielde1e6d9.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<APuStockOtherLogEntity>().Where(it => it.F_StockInOTId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增其他入库单日志新数据（来自前端）
        var aPuStockOtherLogEntityList = input.tableFielde1e6d9.Adapt<List<APuStockOtherLogEntity>>();

        try
        {
            // 比较主表字段变更，生成日志
            if (oldEntity != null)
            {
                void AddFieldChangeLog(string fieldKey, string oldVal, string newVal)
                {
                    if ((oldVal ?? string.Empty) == (newVal ?? string.Empty)) return;
                    logList.Add(new APuStockOtherLogEntity
                    {
                        F_Id = SnowflakeIdHelper.NextId(),
                        F_StockInOTId = entity.id,
                        F_Title = "修改数据",
                        F_Content = $"{fieldDisplay.GetValueOrDefault(fieldKey, fieldKey)}: {oldVal} -> {newVal}",
                        F_CreatorUserId = _userManager.UserId,
                        F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                    });
                }

                AddFieldChangeLog("F_StockInNo", oldEntity.F_StockInNo?.ToString(), entity.F_StockInNo?.ToString());
                AddFieldChangeLog("F_SupplierId", oldEntity.F_SupplierId?.ToString(), entity.F_SupplierId?.ToString());
                AddFieldChangeLog("F_WarehouseId", oldEntity.F_WarehouseId?.ToString(), entity.F_WarehouseId?.ToString());
                AddFieldChangeLog("F_StockInType", oldEntity.F_StockInType?.ToString(), entity.F_StockInType?.ToString());
                AddFieldChangeLog("F_StockInDate", oldEntity.F_StockInDate?.ToString("yyyy-MM-dd"), entity.F_StockInDate?.ToString("yyyy-MM-dd"));
                AddFieldChangeLog("F_StockInUserId", oldEntity.F_StockInUserId?.ToString(), entity.F_StockInUserId?.ToString());
                AddFieldChangeLog("F_Description", oldEntity.F_Description?.ToString(), entity.F_Description?.ToString());
            }

            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_StockInNo,
                    it.F_SupplierId,
                    it.F_WarehouseId,
                    it.F_StockInType,
                    it.F_StockInDate,
                    it.F_StockInUserId,
                    it.F_Description,
                })
                .ExecuteCommandAsync();

            if (aPuStockOtherItemEntityList != null)
            {
                //修改商品信息
                foreach (var item in aPuStockOtherItemEntityList)
                {
                    if (!item.F_Id.IsNullOrEmpty())
                    {
                        var otherItem = await _repositoryItem.GetFirstAsync(it => it.DeleteMark == null && it.F_Id == item.F_Id);
                        otherItem.F_Sales_Price = item.F_Sales_Price;
                        otherItem.F_Price = item.F_Price;
                        otherItem.F_Description = item.F_Description;

                        // 对比子表修改前后的值，记录变更
                        var oldItem = await _repository.AsSugarClient().Queryable<APuStockOtherItemEntity>().FirstAsync(it => it.F_Id == item.F_Id);

                        await _repositoryItem.AsUpdateable(otherItem)
                            .UpdateColumns(it => new {
                                it.F_Sales_Price,
                                it.F_Price,
                                it.F_Description,
                            }).ExecuteCommandAsync();
                        var itemChanges = new List<string>();
                        if (oldItem != null)
                        {
                            if ((oldItem.F_Sales_Price ?? 0) != (item.F_Sales_Price ?? 0))
                                itemChanges.Add($"销售单价: {oldItem.F_Price} -> {item.F_Price}");
                            if ((oldItem.F_Price ?? 0) != (item.F_Price ?? 0))
                                itemChanges.Add($"成本单价: {oldItem.F_Price} -> {item.F_Price}");
                            if ((oldItem.F_Description ?? string.Empty) != (item.F_Description ?? string.Empty))
                                itemChanges.Add($"备注: {oldItem.F_Description} -> {item.F_Description}");
                        }

                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

                        if (itemChanges.Any())
                        {
                            logList.Add(new APuStockOtherLogEntity
                            {
                                F_Id = SnowflakeIdHelper.NextId(),
                                F_StockInOTId = entity.id,
                                F_Title = "修改数据",
                                F_Content = "入库商品列表 修改: " + string.Join("; ", itemChanges),
                                F_CreatorUserId = _userManager.UserId,
                                F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                            });
                        }
                    }

                }
            }
            //    if(aPuStockOtherItemEntityList != null)
            //{
            //    // 1. 先拿到"存在"的 ID 集合
            //        var existIds = aPuStockOtherItemEntityList.Select(x => x.F_Id).ToHashSet();

            //        // 2. 找出"缺失"的行并统一扣库存
            //        var missingList = oldItemList.Where(it => !existIds.Contains(it.F_Id)).ToList();

            //        foreach (var missing in missingList)
            //        {
            //            var warehouseId = !string.IsNullOrEmpty(missing.F_WarehouseID)
            //                ? GetWarehouseId(missing.F_WarehouseID)
            //                : GetWarehouseId(entity.F_WarehouseId);

            //            var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == missing.F_CustomerId && it.F_WarehouseId.Contains(warehouseId));
            //            if (inventory != null)
            //            {
            //                inventory.F_Num -= missing.F_Num;
            //                inventory.F_Num = inventory.F_Num < 0 ? 0 : inventory.F_Num;
            //                await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //            }
            //        }

            //        //新增、修改商品库存
            //        foreach (var item in aPuStockOtherItemEntityList)
            //        {
            //            var warehouseId = !string.IsNullOrEmpty(item.F_WarehouseID)
            //                ? GetWarehouseId(item.F_WarehouseID)
            //                : GetWarehouseId(entity.F_WarehouseId);

            //            var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId.Contains(warehouseId));
            //            if (item.F_Id.IsNullOrEmpty()){
            //                item.F_CreatorUserId = _userManager.UserId;
            //                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            //                item.F_Id = SnowflakeIdHelper.NextId();
            //                item.F_StockInOTId = entity.id;
            //                await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();

            //                // 新增子表记录日志，使用商品名称替代商品 id（找不到名称则回退显示 id）
            //                string addGoodsName = null;
            //                try
            //                {
            //                    addGoodsName = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(g => g.id == item.F_CustomerId).Select(g => g.F_GoodsName).FirstAsync();
            //                }
            //                catch
            //                {
            //                    // ignore
            //                }
            //                logList.Add(new APuStockOtherLogEntity
            //                {
            //                    F_Id = SnowflakeIdHelper.NextId(),
            //                    F_StockInOTId = entity.id,
            //                    F_Title = "添加数据",
            //                    F_Content = $"入库商品列表 新增: 商品={(string.IsNullOrEmpty(addGoodsName) ? item.F_CustomerId : addGoodsName)} 数量={item.F_Num} 单价={item.F_Price}",
            //                    F_CreatorUserId = _userManager.UserId,
            //                    F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
            //                });

            //                //新增库存
            //                if (inventory != null)
            //                {
            //                    inventory.F_Num += item.F_Num;
            //                    // 同时更新库存编码
            //                    if (!string.IsNullOrEmpty(item.F_Encoding))
            //                    {
            //                        inventory.F_Code = item.F_Encoding;
            //                    }
            //                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //                }
            //                else
            //                {
            //                    var inventoryEntity = new AGoodsInventoryEntity();
            //                    inventoryEntity.id = SnowflakeIdHelper.NextId();
            //                    inventoryEntity.F_CreatorUserId = _userManager.UserId;
            //                    inventoryEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            //                    inventoryEntity.F_Num = item.F_Num;
            //                    inventoryEntity.F_GoodsId = item.F_CustomerId;
            //                    inventoryEntity.F_WarehouseId = warehouseId;
            //                    // 保存库存编码
            //                    inventoryEntity.F_Code = item.F_Encoding;
            //                    await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
            //                }
            //            }
            //            else{
            //                item.F_CreatorUserId = null;
            //                item.F_CreatorTime = null;
            //                item.F_StockInOTId = entity.id;

            //                // 对比子表修改前后的值，记录变更
            //                var oldItem = await _repository.AsSugarClient().Queryable<APuStockOtherItemEntity>().FirstAsync(it => it.F_Id == item.F_Id);

            //                var itemChanges = new List<string>();
            //                if (oldItem != null)
            //                {
            //                    if ((oldItem.F_CustomerId ?? string.Empty) != (item.F_CustomerId ?? string.Empty))
            //                    {
            //                        string oldGoodsName = null;
            //                        string newGoodsName = null;
            //                        try
            //                        {
            //                            oldGoodsName = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(g => g.id == oldItem.F_CustomerId).Select(g => g.F_GoodsName).FirstAsync();
            //                        }
            //                        catch
            //                        {
            //                            // ignore
            //                        }
            //                        try
            //                        {
            //                            newGoodsName = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(g => g.id == item.F_CustomerId).Select(g => g.F_GoodsName).FirstAsync();
            //                        }
            //                        catch
            //                        {
            //                            // ignore
            //                        }
            //                        itemChanges.Add($"商品: {(string.IsNullOrEmpty(oldGoodsName) ? oldItem.F_CustomerId : oldGoodsName)} -> {(string.IsNullOrEmpty(newGoodsName) ? item.F_CustomerId : newGoodsName)}");
            //                    }
            //                    if ((oldItem.F_Num ?? 0) != (item.F_Num ?? 0))
            //                        itemChanges.Add($"数量: {oldItem.F_Num} -> {item.F_Num}");
            //                    if ((oldItem.F_Price ?? 0) != (item.F_Price ?? 0))
            //                        itemChanges.Add($"单价: {oldItem.F_Price} -> {item.F_Price}");
            //                    if ((oldItem.F_Description ?? string.Empty) != (item.F_Description ?? string.Empty))
            //                        itemChanges.Add($"备注: {oldItem.F_Description} -> {item.F_Description}");
            //                }

            //                await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

            //                if (itemChanges.Any())
            //                {
            //                    logList.Add(new APuStockOtherLogEntity
            //                    {
            //                        F_Id = SnowflakeIdHelper.NextId(),
            //                        F_StockInOTId = entity.id,
            //                        F_Title = "修改数据",
            //                        F_Content = "入库商品列表 修改: " + string.Join("; ", itemChanges),
            //                        F_CreatorUserId = _userManager.UserId,
            //                        F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
            //                    });
            //                }

            //                //修改库存：考虑仓库变更的情况
            //                var oldWarehouseId = oldItem != null && !string.IsNullOrEmpty(oldItem.F_WarehouseID)
            //                    ? GetWarehouseId(oldItem.F_WarehouseID)
            //                    : GetWarehouseId(entity.F_WarehouseId);

            //                if (warehouseId != oldWarehouseId)
            //                {
            //                    // 仓库变更：从原仓库扣减
            //                    if (oldWarehouseId != null)
            //                    {
            //                        var oldInventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId.Contains(oldWarehouseId));
            //                        if (oldInventory != null)
            //                        {
            //                            oldInventory.F_Num -= (oldItem?.F_Num ?? 0);
            //                            oldInventory.F_Num = oldInventory.F_Num < 0 ? 0 : oldInventory.F_Num;
            //                            await _repositoryInventory.AsSugarClient().Updateable(oldInventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //                        }
            //                    }
            //                    // 新仓库增加
            //                    if (inventory != null)
            //                    {
            //                        inventory.F_Num += item.F_Num;
            //                        // 同时更新库存编码
            //                        if (!string.IsNullOrEmpty(item.F_Encoding))
            //                        {
            //                            inventory.F_Code = item.F_Encoding;
            //                        }
            //                        await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //                    }
            //                    else
            //                    {
            //                        var inventoryEntity = new AGoodsInventoryEntity();
            //                        inventoryEntity.id = SnowflakeIdHelper.NextId();
            //                        inventoryEntity.F_CreatorUserId = _userManager.UserId;
            //                        inventoryEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            //                        inventoryEntity.F_Num = item.F_Num;
            //                        inventoryEntity.F_GoodsId = item.F_CustomerId;
            //                        inventoryEntity.F_WarehouseId = warehouseId;
            //                        // 保存库存编码
            //                        inventoryEntity.F_Code = item.F_Encoding;
            //                        await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
            //                    }
            //                }
            //                else
            //                {
            //                    // 同一仓库，调整数量差异
            //                    var num = item.F_Num - (oldItem?.F_Num ?? 0);
            //                    if (inventory != null)
            //                    {
            //                        inventory.F_Num += num;
            //                        inventory.F_Num = inventory.F_Num < 0 ? 0 : inventory.F_Num;
            //                        // 同时更新库存编码
            //                        if (!string.IsNullOrEmpty(item.F_Encoding))
            //                        {
            //                            inventory.F_Code = item.F_Encoding;
            //                        }
            //                        await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //                    }
            //                    else if (num > 0)
            //                    {
            //                        var inventoryEntity = new AGoodsInventoryEntity();
            //                        inventoryEntity.id = SnowflakeIdHelper.NextId();
            //                        inventoryEntity.F_CreatorUserId = _userManager.UserId;
            //                        inventoryEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            //                        inventoryEntity.F_Num = item.F_Num;
            //                        inventoryEntity.F_GoodsId = item.F_CustomerId;
            //                        inventoryEntity.F_WarehouseId = warehouseId;
            //                        // 保存库存编码
            //                        inventoryEntity.F_Code = item.F_Encoding;
            //                        await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
            //                    }
            //                }
            //            }
            //        }
            //    }

            if (aPuStockOtherLogEntityList != null)
            {
                foreach (var item in aPuStockOtherLogEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_StockInOTId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_StockInOTId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }

            // 为被删除的子项生成日志
            if (willDeletedItems != null && willDeletedItems.Any())
            {
                foreach (var del in willDeletedItems)
                {
                                // 获取商品名称用于日志
                                string delGoodsName = null;
                                try
                                {
                                    delGoodsName = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(g => g.id == del.F_CustomerId).Select(g => g.F_GoodsName).FirstAsync();
                                }
                                catch
                                {
                                    // ignore
                                }
                                logList.Add(new APuStockOtherLogEntity
                                {
                                    F_Id = SnowflakeIdHelper.NextId(),
                                    F_StockInOTId = entity.id,
                                    F_Title = "删除数据",
                                    F_Content = $"入库商品列表 删除: 商品={(string.IsNullOrEmpty(delGoodsName) ? del.F_CustomerId : delGoodsName)} 数量={del.F_Num} 单价={del.F_Price}",
                                    F_CreatorUserId = _userManager.UserId,
                                    F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                                });
                }
            }

            // 批量插入日志
            if (logList.Any())
            {
                await _repository.AsSugarClient().Insertable(logList).ExecuteCommandAsync();
            }

        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 删除a_pu_stock_other.
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
    /// 批量删除a_pu_stock_other.
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
            // 在执行逻辑删除之前，按每张单据的子表明细扣减库存（使用子表的仓库）
            foreach (var entity in entitys)
            {
                var items = await _repository.AsSugarClient().Queryable<APuStockOtherItemEntity>()
                    .Where(it => it.F_StockInOTId == entity.id && it.DeleteMark == null)
                    .Select(it => new { it.F_CustomerId, it.F_Num, it.F_WarehouseID })
                    .ToListAsync();

                if (items != null && items.Any())
                {
                    foreach (var it in items)
                    {
                        if (it == null || string.IsNullOrEmpty(it.F_CustomerId)) continue;

                        var warehouseId = !string.IsNullOrEmpty(it.F_WarehouseID)
                            ? GetWarehouseId(it.F_WarehouseID)
                            : GetWarehouseId(entity.F_WarehouseId);

                        var inventory = await _repositoryInventory.GetFirstAsync(inv => inv.DeleteMark == null && inv.F_GoodsId == it.F_CustomerId && inv.F_WarehouseId.Contains(warehouseId));
                        if (inventory != null)
                        {
                            // 扣减库存，防止出现负数
                            var dec = it.F_Num ?? 0;
                            inventory.F_Num = (inventory.F_Num - dec) < 0 ? 0 : (inventory.F_Num - dec);
                            await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                        }
                    }
                }
            }

            // 批量逻辑删除 a_pu_stock_other（在扣减完库存后执行）
            await _repository.AsDeleteable().In(it => it.id, input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_pu_stock_other详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.APuStockOtherItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuStockOtherLogList)
            .Where(it => it.id == id)
            .Select(it => new APuStockOtherDetailOutput
            {
                id = it.id,
                F_StockInNo = it.F_StockInNo,
                F_SupplierId = it.F_SupplierId,
                F_StockInType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_StockInType) && dic.DictionaryTypeId.Equals("2012074650393251840")).Select(dic => dic.FullName),
                F_StockInDate = it.F_StockInDate.Value.ToString("yyyy-MM-dd"),
                F_StockInUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_StockInUserId)).Select(u => u.RealName),
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableFieldecf5cb = it.APuStockOtherItemList.Where(item => item.DeleteMark == null).Adapt<List<APuStockOtherItemDetailOutput>>(),
                tableFielde1e6d9 = it.APuStockOtherLogList.Where(log => log.DeleteMark == null).Adapt<List<APuStockOtherLogDetailOutput>>(),
            }).ToListAsync();

        var allWarehouseIds = new HashSet<string>();
        foreach (var row in data)
        {
            if (row.tableFieldecf5cb == null) continue;
            foreach (var line in row.tableFieldecf5cb)
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
            // 供应商ID
            if(item.F_SupplierId != null)
            {
                var F_SupplierIdData = await _dataInterfaceService.GetDynamicList("F_SupplierId", "2008457397298925568", "F_Id", "F_SupplierName", "");
                item.F_SupplierId = F_SupplierIdData.Find(it => it.id.Equals(item.F_SupplierId))?.fullName;
            }

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFieldecf5cb ?? Enumerable.Empty<APuStockOtherItemDetailOutput>()), async aPuStockOtherItem =>
        {
            var aPuStockOther = data.ToList().Find(it => it.id.Equals(aPuStockOtherItem.F_StockInOTId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuStockOtherItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuStockOtherItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuStockOtherItem.F_CreatorTime = aPuStockOtherItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            // 仓库：JSON Id 数组 -> 按级联顺序用 "-" 拼接仓库名称
            var whIds = ParseWarehouseIdList(aPuStockOtherItem.F_WarehouseID);
            if (whIds.Count > 0)
            {
                var label = FormatWarehouseNames(whIds, warehouseIdToName);
                if (!string.IsNullOrEmpty(label))
                    aPuStockOtherItem.F_WarehouseID = label;
            }

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            var goodsUnit = _repositoryGoods.AsQueryable().Where(it => it.id == aPuStockOtherItem.F_CustomerId).Select(it => it.F_Unit).First();
            if (!string.IsNullOrEmpty(goodsUnit))
            {
                var F_UnitIdCascader = goodsUnit.ToObject<List<string>>();
                if (F_UnitIdCascader.Count > 1)
                {
                    aPuStockOtherItem.F_NumInfo = "1" + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName + aPuStockOtherItem.F_Num.ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    aPuStockOtherItem.F_NumInfo = aPuStockOtherItem.F_Num.ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }
        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFielde1e6d9), async aPuStockOtherLog =>
        {
            var aPuStockOther = data.ToList().Find(it => it.id.Equals(aPuStockOtherLog.F_StockInOTId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuStockOtherLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuStockOtherLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuStockOtherLog.F_CreatorTime = aPuStockOtherLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<APuStockOtherEntity>(new APuStockOtherEntity()));
        ConfigModel tableFieldecf5cbConfig = new ConfigModel
        {
            tableName = "a_pu_stock_other_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "入库商品列表",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<APuStockOtherItemEntity>(new APuStockOtherItemEntity())
        };
        FieldsModel tableFieldecf5cb = new FieldsModel()
        {
            __config__ = tableFieldecf5cbConfig,
            __vModel__ = "tableFieldecf5cb"
        };
        fieldList.Add(tableFieldecf5cb);

        resData = await _controlParsing.GetParsDataList(resData,"tableFieldecf5cb-F_CustomerId","popupSelect",_userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }

 /// <summary>
    /// 辅助方法：获取完整的仓库ID（JSON数组字符串）
    /// </summary>
    private string GetWarehouseId(string warehouseIdJson)
    {
        if (string.IsNullOrEmpty(warehouseIdJson)) return null;
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
        return string.Join("/", names);
    }


}
