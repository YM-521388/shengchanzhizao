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
using JNPF.example.Entitys.Dto.APuStockPu;
using JNPF.example.Entitys.Dto.APuStockPuItem;
using JNPF.example.Entitys.Dto.APuStockPuLog;
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
/// 业务实现：a_pu_stock_pu.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuStockPu", Order = 200)]
[Route("api/example/[controller]")]
public class APuStockPuService : IAPuStockPuService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuStockPuEntity> _repository;
    private readonly ISqlSugarRepository<APuOrderItemEntity> _repositoryorder;
    private readonly ISqlSugarRepository<APuStockPuItemEntity> _repositorystockpuitem;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<APuWarehouseEntity> _repositoryWarehouse;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositorysp;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"采购入库单号\",\"field\":\"F_StockInNo\"},{\"value\":\"入库仓库\",\"field\":\"F_WarehouseId\"},{\"value\":\"入库类型\",\"field\":\"F_StockInType\"},{\"value\":\"入库日期\",\"field\":\"F_StockInDate\"},{\"value\":\"采购单\",\"field\":\"F_pu_Orderld\"},{\"value\":\"收货人\",\"field\":\"F_StockInUserId\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"采购入库商品-商品\",\"field\":\"tableField4bd139-F_CustomerId\"},{\"value\":\"采购入库商品-数量\",\"field\":\"tableField4bd139-F_Num\"},{\"value\":\"采购入库商品-成本单价(元)\",\"field\":\"tableField4bd139-F_Price\"},{\"value\":\"采购入库商品-备注\",\"field\":\"tableField4bd139-F_Description\"},{\"value\":\"采购入库商品-创建人员\",\"field\":\"tableField4bd139-F_CreatorUserId\"},{\"value\":\"采购入库商品-创建时间\",\"field\":\"tableField4bd139-F_CreatorTime\"},{\"value\":\"采购入库日志-类型\",\"field\":\"tableFieldca527d-F_Type\"},{\"value\":\"采购入库日志-标题\",\"field\":\"tableFieldca527d-F_Title\"},{\"value\":\"采购入库日志-内容\",\"field\":\"tableFieldca527d-F_Content\"},{\"value\":\"采购入库日志-修改原因\",\"field\":\"tableFieldca527d-F_Reason\"},{\"value\":\"采购入库日志-创建人员\",\"field\":\"tableFieldca527d-F_CreatorUserId\"},{\"value\":\"采购入库日志-创建时间\",\"field\":\"tableFieldca527d-F_CreatorTime\"}]".ToList<ParamsModel>();
    /// <summary>
    /// 初始化一个<see cref="APuStockPuService"/>类型的新实例.
    /// </summary>
    public APuStockPuService(
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<APuStockPuEntity> repository,
         ISqlSugarRepository<AGoodsEntity> repositorysp,
        ISqlSugarRepository<APuOrderItemEntity> repositoryorder,
        ISqlSugarRepository<APuStockPuItemEntity> repositorystockpuitem,
        ISqlSugarRepository<APuWarehouseEntity> repositoryWarehouse,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryInventory = repositoryInventory;
        _repository = repository;
        _repositorysp = repositorysp;
        _repositoryorder = repositoryorder;
        _repositorystockpuitem = repositorystockpuitem;
        _repositoryWarehouse = repositoryWarehouse;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_pu_stock_pu.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.APuStockPuItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuStockPuLogList)
            .Select(it => new APuStockPuEntity
            {
                id = it.id,
                F_StockInNo = it.F_StockInNo,
                F_WarehouseId = it.F_WarehouseId,
                F_StockInType = it.F_StockInType,
                F_StockInDate = it.F_StockInDate,
                F_pu_Orderld = it.F_pu_Orderld,
                F_StockInUserId = it.F_StockInUserId,
                F_Description = it.F_Description,
                APuStockPuItemList = it.APuStockPuItemList.Where(item => item.DeleteMark == null).ToList(),
                APuStockPuLogList = it.APuStockPuLogList.Where(log => log.DeleteMark == null).ToList(),
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuStockPuInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField4bd139, async aPuStockPuItem =>
        {
            // 创建人员
            aPuStockPuItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuStockPuItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuStockPuItem.F_CreatorTime = aPuStockPuItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFieldca527d, async aPuStockPuLog =>
        {
            // 创建人员
            aPuStockPuLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuStockPuLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuStockPuLog.F_CreatorTime = aPuStockPuLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

    /// <summary>
    /// 获取a_pu_stock_pu列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuStockPuListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuStockPuItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuStockPuLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuStockPuListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_stock_pu"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuStockPuEntity>();

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuStockPuEntity>();
        //var F_WarehouseIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_WarehouseId").DbColumnName;
        var F_WarehouseId = input.F_WarehouseId?.Last();
        var F_pu_OrderldDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_pu_Orderld").DbColumnName;
        var F_StockInTypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_StockInType").DbColumnName;
        var F_StockInUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_StockInUserId").DbColumnName;

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_StockInNo), it => it.F_StockInNo.Contains(input.F_StockInNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()), it => it.F_WarehouseId.Contains(F_WarehouseId))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_StockInTypeDbColumnName, input.F_StockInType))
            .WhereIF(input.F_StockInDate?.Count() > 0, it => SqlFunc.Between(it.F_StockInDate, input.F_StockInDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_StockInDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_StockInUserIdDbColumnName, input.F_StockInUserId))
             .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_pu_OrderldDbColumnName, input.F_pu_Orderld))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new APuStockPuListOutput
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
           .Select(it => new APuStockPuListOutput
           {
               id = it.id,
               F_StockInNo = it.F_StockInNo,
               F_WarehouseId = it.F_WarehouseId,
               F_StockInType = it.F_StockInType,
               F_StockInDate = it.F_StockInDate.Value.ToString("yyyy-MM-dd"),
               F_pu_Orderld = it.F_pu_Orderld,
               F_StockInUserId = it.F_StockInUserId,
               F_Description = it.F_Description,
               F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
           }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 入库类型
            if (item.F_StockInType != null)
            {
                item.F_StockInType = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(item.F_StockInType) && it.DictionaryTypeId.Equals("2012074650393251840")).Select(it => it.FullName).FirstAsync();
            }

            // 采购单id
            var F_pu_OrderldData = await _dataInterfaceService.GetDynamicList("F_pu_Orderld", "2011984543933927424", "F_Id", "F_OrderNo", "");
            if (item.F_pu_Orderld != null)
            {
                item.F_pu_Orderld = F_pu_OrderldData.Find(it => it.id.Equals(item.F_pu_Orderld))?.fullName;
            }

            // 收货人
            if (item.F_StockInUserId != null)
            {
                item.F_StockInUserId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_StockInUserId)))?.RealName;
            }

        });

        data.pagination = idsQ.pagination;

        return PageResult<APuStockPuListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_pu_stock_pu.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] APuStockPuCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuStockPuEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = DateTime.Now;

        // 自动生成采购入库单号
        if (string.IsNullOrEmpty(entity.F_StockInNo))
        {
            var today = DateTime.Now.ToString("yyyyMMdd");
            var prefix = $"CGRK{today}";
            var count = await _repository.AsQueryable()
                .Where(it => it.F_StockInNo.StartsWith(prefix) && it.DeleteMark == null)
                .CountAsync();
            entity.F_StockInNo = $"{prefix}{(count + 1).ToString("D3")}";
        }

        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuStockPuEntity>(new APuStockPuEntity()));
        ConfigModel tableField4bd139Config = new ConfigModel
        {
            tableName = "a_pu_stock_pu_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "采购入库商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuStockPuItemEntity>(new APuStockPuItemEntity())
        };
        FieldsModel tableField4bd139Model = new FieldsModel()
        {
            __config__ = tableField4bd139Config,
            __vModel__ = "tableField4bd139"
        };
        fieldList.Add(tableField4bd139Model);
        ConfigModel tableFieldca527dConfig = new ConfigModel
        {
            tableName = "a_pu_stock_pu_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "采购入库日志",
            children = ExportImportDataHelper.GetTemplateParsing<APuStockPuLogEntity>(new APuStockPuLogEntity())
        };
        FieldsModel tableFieldca527dModel = new FieldsModel()
        {
            __config__ = tableFieldca527dConfig,
            __vModel__ = "tableFieldca527d"
        };
        fieldList.Add(tableFieldca527dModel);


        var aPuStockPuItemEntityList = input.tableField4bd139.Adapt<List<APuStockPuItemEntity>>();
        if(aPuStockPuItemEntityList != null)
        {
            foreach (var item in aPuStockPuItemEntityList)
            {
                var inventoryEntity = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_Code == item.F_Encoding);
                if (inventoryEntity != null) throw Oops.Bah(ErrorCode.COM1018, "库存编码：" + item.F_Encoding + " 已存在该编码，请重新填写该库存编码！");

                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuStockPuItemList = aPuStockPuItemEntityList;
        }

        var aPuStockPuLogEntityList = input.tableFieldca527d.Adapt<List<APuStockPuLogEntity>>();
        if(aPuStockPuLogEntityList != null)
        {
            foreach (var item in aPuStockPuLogEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuStockPuLogList = aPuStockPuLogEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.APuStockPuItemList)
            .Include(it => it.APuStockPuLogList)
            .ExecuteCommandAsync();

        if (aPuStockPuItemEntityList != null)
        {
            foreach (var item in aPuStockPuItemEntityList)
            {
                // 使用子表商品自带的仓库ID
                var itemWarehouseId = item.F_WarehouseID;

                if (string.IsNullOrEmpty(itemWarehouseId))
                {
                    continue;
                }

                // 按商品 + 仓库 + 库存编码(F_Encoding 对应总库存 F_Code) 查询库存记录
                var inventory = await GetPuInventoryAsync(item.F_Encoding);
                if (inventory != null)
                {
                    inventory.F_Num += item.F_Num;
                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
                else
                {
                    var inventoryEntity = new AGoodsInventoryEntity();
                    inventoryEntity.id = SnowflakeIdHelper.NextId();
                    inventoryEntity.F_CreatorUserId = _userManager.UserId;
                    inventoryEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                    inventoryEntity.F_Num = item.F_Num;
                    inventoryEntity.F_Code = item.F_Encoding;   // 子表库存编码存入总库存编码
                    inventoryEntity.F_GoodsId = item.F_CustomerId;
                    inventoryEntity.F_WarehouseId = itemWarehouseId;
                    await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
                }
            }
        }


        // 如果子表中存在商品 ID，更新对应采购单商品的引用状态 F_quote = F_WarehouseId
        if (aPuStockPuItemEntityList != null && aPuStockPuItemEntityList.Any())
        {
            var customerIds = aPuStockPuItemEntityList
                .Where(it => !string.IsNullOrEmpty(it.F_CustomerId))
                .Select(it => it.F_CustomerId)
                .Distinct()
                .ToList();

            if (customerIds.Any())
            {
                await _repository.AsSugarClient()
                    .Updateable<APuOrderItemEntity>()
                    .SetColumns(it => new APuOrderItemEntity { F_quote = entity.F_WarehouseId })
                    .Where(it => customerIds.Contains(it.F_Id) && it.DeleteMark == null)
                    .ExecuteCommandAsync();
            }
        }
    }

    /// <summary>
    /// 获取a_pu_stock_pu无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(APuStockPuListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuStockPuItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuStockPuLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuStockPuListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_stock_pu"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuStockPuEntity>();

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuStockPuEntity>();
        var F_WarehouseId = input.F_WarehouseId?.Last();
        var F_pu_OrderldDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_pu_Orderld").DbColumnName;
        var F_StockInTypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_StockInType").DbColumnName;
        var F_StockInUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_StockInUserId").DbColumnName;

        var list = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_StockInNo), it => it.F_StockInNo.Contains(input.F_StockInNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()), it => it.F_WarehouseId.Contains(F_WarehouseId))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_StockInTypeDbColumnName, input.F_StockInType))
            .WhereIF(input.F_StockInDate?.Count() > 0, it => SqlFunc.Between(it.F_StockInDate, input.F_StockInDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_StockInDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_StockInUserIdDbColumnName, input.F_StockInUserId))
             .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_pu_OrderldDbColumnName, input.F_pu_Orderld))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), "F_CreatorTime DESC, id ASC")
            .OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new APuStockPuListOutput
            {
                id = it.id,
                F_StockInNo = it.F_StockInNo,
                F_WarehouseId = it.F_WarehouseId,
                F_StockInType = it.F_StockInType,
                F_StockInDate = it.F_StockInDate.Value.ToString("yyyy-MM-dd"),
                F_pu_Orderld = it.F_pu_Orderld,
                F_StockInUserId = it.F_StockInUserId,
                F_Description = it.F_Description,
            })
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            var F_WarehouseIdData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (item.F_WarehouseId != null)
            {
                var F_WarehouseIdCascader = item.F_WarehouseId.ToObject<List<string>>();
                // 获取所有匹配的仓库名称，用逗号连接
                var warehouseNames = F_WarehouseIdData.FindAll(it => F_WarehouseIdCascader.Contains(it.id)).Select(s => s.fullName);
                item.F_WarehouseId = string.Join(",", warehouseNames);
            }
            // 采购单id
            var F_pu_OrderldData = await _dataInterfaceService.GetDynamicList("F_pu_Orderld", "2011984543933927424", "F_Id", "F_OrderNo", "");
            if (item.F_pu_Orderld != null)
            {
                item.F_pu_Orderld = F_pu_OrderldData.Find(it => it.id.Equals(item.F_pu_Orderld))?.fullName;
            }


            // 入库类型
            if (item.F_StockInType != null)
            {
                item.F_StockInType = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(item.F_StockInType) && it.DictionaryTypeId.Equals("2012074650393251840")).Select(it => it.FullName).FirstAsync();
            }

            // 收货人
            if(item.F_StockInUserId != null)
            {
                item.F_StockInUserId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_StockInUserId)))?.RealName;
            }

        });

       
        var resData = list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);

        list = resData.ToObject<List<APuStockPuListOutput>>(CommonConst.options);
        return list;
    }

    /// <summary>
    /// 导出a_pu_stock_pu.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] APuStockPuListQueryInput input)
    {
        var exportData = new List<APuStockPuListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<APuStockPuListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName, input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新a_pu_stock_pu.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] APuStockPuUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuStockPuEntity>();

        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_StockInNo.Equals(input.F_StockInNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "采购入库单号");

        if (string.IsNullOrEmpty(entity.F_StockInNo))
        {
            entity.F_StockInNo = oldEntity.F_StockInNo;
        }

        // 获取原有数据用于生成修改日志
        var originalEntity = await _repository.AsSugarClient().Queryable<APuStockPuEntity>()
            .Includes(it => it.APuStockPuItemList)
            .Where(it => it.id == id)
            .FirstAsync();

        var originalItems = await _repository.AsSugarClient().Queryable<APuStockPuItemEntity>()
            .Where(it => it.F_StockInPUId == id && it.DeleteMark != 1)
            .ToListAsync();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuStockPuEntity>(new APuStockPuEntity()));
        ConfigModel tableField4bd139Config = new ConfigModel
        {
            tableName = "a_pu_stock_pu_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "采购入库商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuStockPuItemEntity>(new APuStockPuItemEntity())
        };
        FieldsModel tableField4bd139Model = new FieldsModel()
        {
            __config__ = tableField4bd139Config,
            __vModel__ = "tableField4bd139"
        };
        fieldList.Add(tableField4bd139Model);
        ConfigModel tableFieldca527dConfig = new ConfigModel
        {
            tableName = "a_pu_stock_pu_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "采购入库日志",
            children = ExportImportDataHelper.GetTemplateParsing<APuStockPuLogEntity>(new APuStockPuLogEntity())
        };
        FieldsModel tableFieldca527dModel = new FieldsModel()
        {
            __config__ = tableFieldca527dConfig,
            __vModel__ = "tableFieldca527d"
        };
        fieldList.Add(tableFieldca527dModel);

        var oldItemList = await _repository.AsSugarClient().Queryable<APuStockPuItemEntity>().Where(it => it.F_StockInPUId == entity.id).Select(it => new { F_Id = it.F_Id, F_CustomerId = it.F_CustomerId, F_Num = it.F_Num, F_WarehouseID = it.F_WarehouseID, F_Encoding = it.F_Encoding }).ToListAsync();

        // 移除采购入库单商品列表可能被删除数据
        if (input.tableField4bd139 !=null && input.tableField4bd139.Any())
            await _repository.AsSugarClient().Deleteable<APuStockPuItemEntity>().Where(it => it.F_StockInPUId == entity.id && !input.tableField4bd139.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<APuStockPuItemEntity>().Where(it => it.F_StockInPUId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增采购入库单商品列表新数据
        var aPuStockPuItemEntityList = input.tableField4bd139.Adapt<List<APuStockPuItemEntity>>();


        // 移除采购入库单日志可能被删除数据
        if (input.tableFieldca527d !=null && input.tableFieldca527d.Any())
            await _repository.AsSugarClient().Deleteable<APuStockPuLogEntity>().Where(it => it.F_StockInPUId == entity.id && !input.tableFieldca527d.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<APuStockPuLogEntity>().Where(it => it.F_StockInPUId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增采购入库单日志新数据
        var aPuStockPuLogEntityList = input.tableFieldca527d.Adapt<List<APuStockPuLogEntity>>();

        // 记录修改前数据，准备生成操作日志
        var logList = new List<APuStockPuLogEntity>();

        // 字段中文映射
        var fieldDisplay = new Dictionary<string, string>
        {
            { "F_StockInNo", "采购入库单号" },
            { "F_WarehouseId", "入库仓库" },
            { "F_StockInType", "入库类型" },
            { "F_StockInDate", "入库日期" },
            { "F_pu_Orderld", "采购单" },
            { "F_StockInUserId", "收货人" },
            { "F_Description", "备注" }
        };

        // 如果存在要删除的子表数据，先查询它们以便记录日志
        List<APuStockPuItemEntity> willDeletedItems = new List<APuStockPuItemEntity>();
        if (input.tableField4bd139 != null && input.tableField4bd139.Any())
        {
            var keepIds = input.tableField4bd139.Select(it => it.F_Id).ToList();
            willDeletedItems = await _repository.AsSugarClient().Queryable<APuStockPuItemEntity>()
                .Where(it => it.F_StockInPUId == entity.id && !keepIds.Contains(it.F_Id) && it.DeleteMark == null)
                .ToListAsync();
        }

        // 获取仓库、类型等转换数据（用于日志显示）
        dynamic warehouseData = null;
        dynamic orderData = null;
        var stockInTypeData = new List<DictionaryDataEntity>();
        try { warehouseData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children"); } catch { }
        try { stockInTypeData = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.DictionaryTypeId.Equals("2012074650393251840")).ToListAsync(); } catch { }
        try { orderData = await _dataInterfaceService.GetDynamicList("F_pu_Orderld", "2011984543933927424", "F_Id", "F_OrderNo", ""); } catch { }

        try
        {
            // 比较主表字段变更，生成日志
            if (oldEntity != null)
            {
                // 辅助方法：转换字段值为可读文字
                async Task<string> ConvertFieldValue(string fieldKey, string val)
                {
                    if (string.IsNullOrEmpty(val)) return val;

                    try
                    {
                        switch (fieldKey)
                        {
                           
                            case "F_StockInType":
                                // 入库类型：编码转换为字典名称
                                var type = stockInTypeData.FirstOrDefault(it => it.EnCode == val);
                                return type?.FullName ?? val;
                            case "F_StockInUserId":
                                // 收货人：ID转换为用户姓名
                                var user = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id == val);
                                return user?.RealName ?? val;
                            default:
                                return val;
                        }
                    }
                    catch { return val; }
                }

                async Task AddFieldChangeLog(string fieldKey, string oldVal, string newVal)
                {
                    var convertedOldVal = await ConvertFieldValue(fieldKey, oldVal);
                    var convertedNewVal = await ConvertFieldValue(fieldKey, newVal);
                    if ((convertedOldVal ?? string.Empty) == (convertedNewVal ?? string.Empty)) return;
                    logList.Add(new APuStockPuLogEntity
                    {
                        F_Id = SnowflakeIdHelper.NextId(),
                        F_StockInPUId = entity.id,
                        F_Title = "修改数据",
                        F_Content = $"{fieldDisplay.GetValueOrDefault(fieldKey, fieldKey)}: {convertedOldVal} -> {convertedNewVal}",
                        F_CreatorUserId = _userManager.UserId,
                        F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                    });
                }

                await AddFieldChangeLog("F_StockInNo", oldEntity.F_StockInNo?.ToString(), entity.F_StockInNo?.ToString());
                await AddFieldChangeLog("F_WarehouseId", oldEntity.F_WarehouseId?.ToString(), entity.F_WarehouseId?.ToString());
                await AddFieldChangeLog("F_StockInType", oldEntity.F_StockInType?.ToString(), entity.F_StockInType?.ToString());
                await AddFieldChangeLog("F_StockInDate", oldEntity.F_StockInDate?.ToString("yyyy-MM-dd"), entity.F_StockInDate?.ToString("yyyy-MM-dd"));
                await AddFieldChangeLog("F_pu_Orderld", oldEntity.F_pu_Orderld?.ToString(), entity.F_pu_Orderld?.ToString());
                await AddFieldChangeLog("F_StockInUserId", oldEntity.F_StockInUserId?.ToString(), entity.F_StockInUserId?.ToString());
                await AddFieldChangeLog("F_Description", oldEntity.F_Description?.ToString(), entity.F_Description?.ToString());
            }

            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_StockInNo,
                    it.F_WarehouseId,
                    it.F_StockInType,
                    it.F_StockInDate,
                    it.F_pu_Orderld,
                    it.F_StockInUserId,
                    it.F_Description,
                })
                .ExecuteCommandAsync();

            if(aPuStockPuItemEntityList != null)
            {
                foreach (var item in aPuStockPuItemEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_StockInPUId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();

                        // 新增子表记录日志，使用商品名称替代商品 id
                        string addGoodsName = null;
                        try
                        {
                            addGoodsName = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(g => g.id == item.F_CustomerId).Select(g => g.F_GoodsName).FirstAsync();
                        }
                        catch { }
                        logList.Add(new APuStockPuLogEntity
                        {
                            F_Id = SnowflakeIdHelper.NextId(),
                            F_StockInPUId = entity.id,
                            F_Title = "添加数据",
                            F_Content = $"采购入库商品 新增: 商品={(string.IsNullOrEmpty(addGoodsName) ? item.F_CustomerId : addGoodsName)} 数量={item.F_Num} 单价={item.F_Price}",
                            F_CreatorUserId = _userManager.UserId,
                            F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                        });
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_StockInPUId = entity.id;

                        // 对比子表修改前后的值，记录变更
                        var oldItem = await _repository.AsSugarClient().Queryable<APuStockPuItemEntity>().FirstAsync(it => it.F_Id == item.F_Id);

                        var itemChanges = new List<string>();
                        if (oldItem != null)
                        {
                            if ((oldItem.F_CustomerId ?? string.Empty) != (item.F_CustomerId ?? string.Empty))
                            {
                                string oldGoodsName = null;
                                string newGoodsName = null;
                                try { oldGoodsName = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(g => g.id == oldItem.F_CustomerId).Select(g => g.F_GoodsName).FirstAsync(); } catch { }
                                try { newGoodsName = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(g => g.id == item.F_CustomerId).Select(g => g.F_GoodsName).FirstAsync(); } catch { }
                                itemChanges.Add($"商品: {(string.IsNullOrEmpty(oldGoodsName) ? oldItem.F_CustomerId : oldGoodsName)} -> {(string.IsNullOrEmpty(newGoodsName) ? item.F_CustomerId : newGoodsName)}");
                            }
                            if ((oldItem.F_Num ?? 0) != (item.F_Num ?? 0))
                                itemChanges.Add($"数量: {oldItem.F_Num} -> {item.F_Num}");
                            if ((oldItem.F_Price ?? 0) != (item.F_Price ?? 0))
                                itemChanges.Add($"单价: {oldItem.F_Price} -> {item.F_Price}");
                            if ((oldItem.F_Description ?? string.Empty) != (item.F_Description ?? string.Empty))
                                itemChanges.Add($"备注: {oldItem.F_Description} -> {item.F_Description}");
                        }

                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

                        if (itemChanges.Any())
                        {
                            logList.Add(new APuStockPuLogEntity
                            {
                                F_Id = SnowflakeIdHelper.NextId(),
                                F_StockInPUId = entity.id,
                                F_Title = "修改数据",
                                F_Content = "采购入库商品 修改: " + string.Join("; ", itemChanges),
                                F_CreatorUserId = _userManager.UserId,
                                F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                            });
                        }
                    }
                }
            }

            if(aPuStockPuLogEntityList != null)
            {
                foreach (var item in aPuStockPuLogEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_StockInPUId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_StockInPUId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }

            // 为被删除的子项生成日志
            if (willDeletedItems != null && willDeletedItems.Any())
            {
                foreach (var del in willDeletedItems)
                {
                    string delGoodsName = null;
                    try
                    {
                        delGoodsName = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(g => g.id == del.F_CustomerId).Select(g => g.F_GoodsName).FirstAsync();
                    }
                    catch { }
                    logList.Add(new APuStockPuLogEntity
                    {
                        F_Id = SnowflakeIdHelper.NextId(),
                        F_StockInPUId = entity.id,
                        F_Title = "删除数据",
                        F_Content = $"采购入库商品 删除: 商品={(string.IsNullOrEmpty(delGoodsName) ? del.F_CustomerId : delGoodsName)} 数量={del.F_Num} 单价={del.F_Price}",
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

            // 生成自动修改日志（已迁移到上方统一处理）
            // await GenerateUpdateLogs(originalEntity, entity, originalItems, aPuStockPuItemEntityList, input);

            //if (aPuStockPuItemEntityList != null)
            //{
            //    //删除的商品减掉库存（使用子表商品自己的仓库ID）
            //    // 1. 先拿到"存在"的 ID 集合
            //    var existIds = aPuStockPuItemEntityList.Select(x => x.F_Id).ToHashSet();
            //    // 2. 找出"缺失"的行并统一扣库存
            //    var missingList = oldItemList.Where(it => !existIds.Contains(it.F_Id)).ToList();
            //    foreach (var missing in missingList)
            //    {
            //        var missingWarehouseId = missing.F_WarehouseID;
            //        if (string.IsNullOrEmpty(missingWarehouseId)) continue;

            //        var inventory = await GetPuInventoryAsync(missing.F_Encoding);
            //        if (inventory != null)
            //        {
            //            inventory.F_Num -= missing.F_Num ?? 0;
            //            await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            //        }
            //    }

                //新增、修改商品库存（使用子表商品自己的仓库ID + F_Encoding 对应总库存 F_Code）
                //foreach (var item in aPuStockPuItemEntityList)
                //{
                //    var itemWarehouseId = item.F_WarehouseID;

                //    if (string.IsNullOrEmpty(itemWarehouseId))
                //    {
                //        continue;
                //    }

                //    var inventory = await GetPuInventoryAsync(item.F_Encoding);
                //    if (item.F_Id.IsNullOrEmpty())
                //    {
                //        item.F_CreatorUserId = _userManager.UserId;
                //        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                //        item.F_Id = SnowflakeIdHelper.NextId();
                //        item.F_StockInPUId = entity.id;
                //        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();

                //        //新增库存
                //        if (inventory != null)
                //        {
                //            inventory.F_Num += item.F_Num ?? 0;
                //            await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                //        }
                //        else
                //        {
                //            var inventoryEntity = new AGoodsInventoryEntity();
                //            inventoryEntity.id = SnowflakeIdHelper.NextId();
                //            inventoryEntity.F_CreatorUserId = _userManager.UserId;
                //            inventoryEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                //            inventoryEntity.F_Num = item.F_Num ?? 0;
                //            inventoryEntity.F_GoodsId = item.F_CustomerId;
                //            inventoryEntity.F_WarehouseId = itemWarehouseId;
                //            inventoryEntity.F_Code = item.F_Encoding;
                //            await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
                //        }
                //    }
                //    else
                //    {
                //        // 从已获取的oldItemList中查找旧值，而不是再次查询数据库
                //        var oldItem = oldItemList.FirstOrDefault(it => it.F_Id == item.F_Id);

                //        item.F_CreatorUserId = null;
                //        item.F_CreatorTime = null;
                //        item.F_StockInPUId = entity.id;
                //        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();


                //        //修改库存（商品/仓库/库存编码 任一变化则按旧键扣减、新键增加；否则同键差量）
                //        if (oldItem != null)
                //        {
                //            var oldWarehouseId = oldItem.F_WarehouseID;
                //            var newWarehouseId = itemWarehouseId;
                //            var keyChanged =
                //                !string.Equals(oldItem.F_CustomerId, item.F_CustomerId, StringComparison.Ordinal)
                //                || !string.Equals(oldWarehouseId ?? "", newWarehouseId ?? "", StringComparison.Ordinal)
                //                || !string.Equals(oldItem.F_Encoding ?? "", item.F_Encoding ?? "", StringComparison.Ordinal);

                //            if (keyChanged)
                //            {
                //                // 原键扣减
                //                if (!string.IsNullOrEmpty(oldWarehouseId))
                //                {
                //                    var oldInventory = await GetPuInventoryAsync(oldItem.F_Encoding);
                //                    if (oldInventory != null)
                //                    {
                //                        oldInventory.F_Num -= oldItem.F_Num ?? 0;
                //                        await _repositoryInventory.AsSugarClient().Updateable(oldInventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                //                    }
                //                }
                //                // 新键增加
                //                if (inventory != null)
                //                {
                //                    inventory.F_Num += item.F_Num ?? 0;
                //                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                //                }
                //                else
                //                {
                //                    var inventoryEntity = new AGoodsInventoryEntity();
                //                    inventoryEntity.id = SnowflakeIdHelper.NextId();
                //                    inventoryEntity.F_CreatorUserId = _userManager.UserId;
                //                    inventoryEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                //                    inventoryEntity.F_Num = item.F_Num ?? 0;
                //                    inventoryEntity.F_GoodsId = item.F_CustomerId;
                //                    inventoryEntity.F_WarehouseId = newWarehouseId;
                //                    inventoryEntity.F_Code = item.F_Encoding;
                //                    await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
                //                }
                //            }
                //            else
                //            {
                //                // 同一商品+仓库+编码，差量更新
                //                if (inventory != null)
                //                {
                //                    var newNum = item.F_Num ?? 0;
                //                    var oldNum = oldItem.F_Num ?? 0;
                //                    var num = newNum - oldNum;
                //                    inventory.F_Num = (inventory.F_Num + num) < 0 ? 0 : (inventory.F_Num + num);
                //                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                //                }
                //            }
                //        }
                //    }
                //}
            //}
        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 删除a_pu_stock_pu.
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
    /// 批量删除a_pu_stock_pu.
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
            // 获取需要清空的F_quote字段的相关数据
            var stockPuItemEntities = await _repositorystockpuitem.AsQueryable()
                .In(it => it.F_StockInPUId, input.ids)
                .Where(it => it.DeleteMark == null)
                .ToListAsync();

            if (stockPuItemEntities.Count > 0)
            {
                var customerIds = stockPuItemEntities.Select(it => it.F_CustomerId).Distinct().ToList();

                // 清空APuOrderItemEntity中对应的F_quote字段
                await _repositoryorder.AsUpdateable()
                    .SetColumns(it => it.F_quote == null)
                    .Where(it => customerIds.Contains(it.F_Id) && it.DeleteMark == null)
                    .ExecuteCommandAsync();
            }

            // 在执行逻辑删除之前，按每张单据的子表明细扣减库存（使用子表商品自己的仓库ID）
            foreach (var entity in entitys)
            {
                var items = await _repositorystockpuitem.AsSugarClient().Queryable<APuStockPuItemEntity>()
                    .Where(it => it.F_StockInPUId == entity.id && it.DeleteMark == null)
                    .Select(it => new { it.F_CustomerId, it.F_Num, it.F_WarehouseID, it.F_Encoding })
                    .ToListAsync();

                if (items != null && items.Any())
                {
                    foreach (var it in items)
                    {
                        if (it == null || string.IsNullOrEmpty(it.F_CustomerId)) continue;

                        var itemWarehouseId = it.F_WarehouseID;
                        if (string.IsNullOrEmpty(itemWarehouseId)) continue;

                        var inventory = await GetPuInventoryAsync(it.F_Encoding);
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

            // 批量删除a_pu_stock_pu（在扣减完库存后执行）
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_pu_stock_pu详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.APuStockPuItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuStockPuLogList)
            .Where(it => it.id == id)
            .Select(it => new APuStockPuDetailOutput
            {
                id = it.id,
                F_StockInNo = it.F_StockInNo,
                F_WarehouseId = it.F_WarehouseId,
                F_StockInType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_StockInType) && dic.DictionaryTypeId.Equals("2012074650393251840")).Select(dic => dic.FullName),
                F_StockInDate = it.F_StockInDate.Value.ToString("yyyy-MM-dd"),
                F_pu_Orderld = it.F_pu_Orderld,
                F_StockInUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_StockInUserId)).Select(u => u.RealName),
                F_Description = it.F_Description,
                tableField4bd139 = it.APuStockPuItemList.Where(item => item.DeleteMark == null).Adapt<List<APuStockPuItemDetailOutput>>(),
                tableFieldca527d = it.APuStockPuLogList.Where(log => log.DeleteMark == null).Adapt<List<APuStockPuLogDetailOutput>>(),
            }).ToListAsync();

        // 收集子表所有仓库 ID，一次查询后建字典
        var allWarehouseIds = new HashSet<string>();
        var allGoodsIds = new HashSet<string>();
        foreach (var row in data)
        {
            if (row.tableField4bd139 == null) continue;
            foreach (var line in row.tableField4bd139)
            {
                foreach (var wid in ParseWarehouseIdList(line.F_WarehouseID))
                    allWarehouseIds.Add(wid);
                if (!string.IsNullOrEmpty(line.F_CustomerId))
                    allGoodsIds.Add(line.F_CustomerId);
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

        // 收集商品ID，一次查询后建字典获取 F_Unit_Ratio
        IReadOnlyDictionary<string, string> goodsIdToUnitRatio = new Dictionary<string, string>();
        if (allGoodsIds.Count > 0)
        {
            var goodsRows = await _repository.AsSugarClient().Queryable<AGoodsEntity>()
                .Where(g => g.DeleteMark == null && allGoodsIds.Contains(g.id))
                .Select(g => new { g.id, g.F_Unit_Ratio })
                .ToListAsync();
            goodsIdToUnitRatio = goodsRows.ToDictionary(r => r.id, r => r.F_Unit_Ratio ?? string.Empty, StringComparer.Ordinal);
        }

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            // 入库仓库ID
            //if(item.F_WarehouseId != null)
            //{
            //    var F_WarehouseIdData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2012073572784279552", "F_Id", "F_Name", "");
            //    item.F_WarehouseId = F_WarehouseIdData.Find(it => it.id.Equals(item.F_WarehouseId))?.fullName;
            //}

            // 采购单id
            if (item.F_pu_Orderld != null)
            {
                var F_pu_OrderldData = await _dataInterfaceService.GetDynamicList("F_pu_Orderld", "2011984543933927424", "F_Id", "F_OrderNo", "");
                item.F_pu_Orderld = F_pu_OrderldData.Find(it => it.id.Equals(item.F_pu_Orderld))?.fullName;
            }

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField4bd139), async aPuStockPuItem =>
        {
            var aPuStockPu = data.ToList().Find(it => it.id.Equals(aPuStockPuItem.F_StockInPUId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuStockPuItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuStockPuItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuStockPuItem.F_CreatorTime = aPuStockPuItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            var goodsUnit = _repositorysp.AsQueryable().Where(it => it.id == aPuStockPuItem.F_CustomerId).Select(g => g.F_Unit).First();
            if (!string.IsNullOrEmpty(goodsUnit))
            {
                var F_UnitIdCascader = goodsUnit.ToObject<List<string>>();
                if (F_UnitIdCascader.Count > 1)
                {
                    aPuStockPuItem.F_NumInfo = "1" + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName + Math.Floor(aPuStockPuItem.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    aPuStockPuItem.F_NumInfo = Math.Floor(aPuStockPuItem.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }

            // 仓库：
            var F_WarehouseIdData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (aPuStockPuItem.F_WarehouseID != null)
            {
                var F_WarehouseIdCascader = aPuStockPuItem.F_WarehouseID.ToObject<List<string>>();
                aPuStockPuItem.F_WarehouseID = string.Join("/", F_WarehouseIdData.FindAll(it => F_WarehouseIdCascader.Contains(it.id)).Select(s => s.fullName));
            }
        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFieldca527d), async aPuStockPuLog =>
        {
            var aPuStockPu = data.ToList().Find(it => it.id.Equals(aPuStockPuLog.F_StockInPUId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuStockPuLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuStockPuLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuStockPuLog.F_CreatorTime = aPuStockPuLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<APuStockPuEntity>(new APuStockPuEntity()));
        ConfigModel tableField4bd139Config = new ConfigModel
        {
            tableName = "a_pu_stock_pu_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "采购入库商品",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<APuStockPuItemEntity>(new APuStockPuItemEntity())
        };
        FieldsModel tableField4bd139 = new FieldsModel()
        {
            __config__ = tableField4bd139Config,
            __vModel__ = "tableField4bd139"
        };
        fieldList.Add(tableField4bd139);

        resData = await _controlParsing.GetParsDataList(resData,"tableField4bd139-F_CustomerId","popupSelect",_userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }

    /// <summary>
    /// 生成更新日志
    /// </summary>
    private async Task GenerateUpdateLogs(APuStockPuEntity originalEntity, APuStockPuEntity newEntity,
        List<APuStockPuItemEntity> originalItems, List<APuStockPuItemEntity> newItems, APuStockPuUpInput input)
    {
        var logs = new List<APuStockPuLogEntity>();
        var currentTime = DateTime.Now;

        // 比较主表字段变化
        if (originalEntity.F_StockInNo != newEntity.F_StockInNo)
        {
            logs.Add(new APuStockPuLogEntity
            {
                F_Id = SnowflakeIdHelper.NextId(),
                F_StockInPUId = newEntity.id,
                F_Type = "修改",
                F_Title = "修改采购入库单",
                F_Content = $"修改采购入库单号: 从 '{originalEntity.F_StockInNo}' 改为 '{newEntity.F_StockInNo}'",
                F_CreatorUserId = _userManager.UserId,
                F_CreatorTime = currentTime,
                TenantId = _userManager.TenantId
            });
        }

        //if (originalEntity.F_WarehouseId != newEntity.F_WarehouseId)
        //{
        //    logs.Add(new APuStockPuLogEntity
        //    {
        //        F_Id = SnowflakeIdHelper.NextId(),
        //        F_StockInPUId = newEntity.id,
        //        F_Type = "修改",
        //        F_Title = "修改采购入库单",
        //        F_Content = $"修改入库仓库: 从 '{originalEntity.F_WarehouseId}' 改为 '{newEntity.F_WarehouseId}'",
        //        F_CreatorUserId = _userManager.UserId,
        //        F_CreatorTime = currentTime,
        //        TenantId = _userManager.TenantId
        //    });
        //}

        //if (originalEntity.F_StockInType != newEntity.F_StockInType)
        //{
        //    logs.Add(new APuStockPuLogEntity
        //    {
        //        F_Id = SnowflakeIdHelper.NextId(),
        //        F_StockInPUId = newEntity.id,
        //        F_Type = "修改",
        //        F_Title = "修改采购入库单",
        //        F_Content = $"修改入库类型: 从 '{originalEntity.F_StockInType}' 改为 '{newEntity.F_StockInType}'",
        //        F_CreatorUserId = _userManager.UserId,
        //        F_CreatorTime = currentTime,
        //        TenantId = _userManager.TenantId
        //    });
        //}

        if (originalEntity.F_StockInDate != newEntity.F_StockInDate)
        {
            logs.Add(new APuStockPuLogEntity
            {
                F_Id = SnowflakeIdHelper.NextId(),
                F_StockInPUId = newEntity.id,
                F_Type = "修改",
                F_Title = "修改采购入库单",
                F_Content = $"修改入库日期: 从 '{originalEntity.F_StockInDate}' 改为 '{newEntity.F_StockInDate}'",
                F_CreatorUserId = _userManager.UserId,
                F_CreatorTime = currentTime,
                TenantId = _userManager.TenantId
            });
        }

        //if (originalEntity.F_StockInUserId != newEntity.F_StockInUserId)
        //{
        //    logs.Add(new APuStockPuLogEntity
        //    {
        //        F_Id = SnowflakeIdHelper.NextId(),
        //        F_StockInPUId = newEntity.id,
        //        F_Type = "修改",
        //        F_Title = "修改采购入库单",
        //        F_Content = $"修改收货人: 从 '{originalEntity.F_StockInUserId}' 改为 '{newEntity.F_StockInUserId}'",
        //        F_CreatorUserId = _userManager.UserId,
        //        F_CreatorTime = currentTime,
        //        TenantId = _userManager.TenantId
        //    });
        //}

        if (originalEntity.F_Description != newEntity.F_Description)
        {
            logs.Add(new APuStockPuLogEntity
            {
                F_Id = SnowflakeIdHelper.NextId(),
                F_StockInPUId = newEntity.id,
                F_Type = "修改",
                F_Title = "修改采购入库单",
                F_Content = $"修改备注: 从 '{originalEntity.F_Description}' 改为 '{newEntity.F_Description}'",
                F_CreatorUserId = _userManager.UserId,
                F_CreatorTime = currentTime,
                TenantId = _userManager.TenantId
            });
        }

        // 检查子表数据的变化
        var originalItemIds = originalItems.Select(it => it.F_Id).ToList();
        var newItemIds = newItems.Where(it => !string.IsNullOrEmpty(it.F_Id)).Select(it => it.F_Id).ToList();

        // 新增的子表数据
        var addedItems = newItems.Where(it => string.IsNullOrEmpty(it.F_Id)).ToList();
        foreach (var item in addedItems)
        {
            logs.Add(new APuStockPuLogEntity
            {
                F_Id = SnowflakeIdHelper.NextId(),
                F_StockInPUId = newEntity.id,
                F_Type = "增加",
                F_Title = "增加采购入库商品",
                F_Content = $"新增商品: 商品ID={item.F_CustomerId}, 数量={item.F_Num}, 单价={item.F_Price}, 备注={item.F_Description}",
                F_CreatorUserId = _userManager.UserId,
                F_CreatorTime = currentTime,
                TenantId = _userManager.TenantId
            });
        }

        // 删除的子表数据
        var deletedItemIds = originalItemIds.Except(newItemIds).ToList();
        foreach (var deletedId in deletedItemIds)
        {
            var deletedItem = originalItems.First(it => it.F_Id == deletedId);
            logs.Add(new APuStockPuLogEntity
            {
                F_Id = SnowflakeIdHelper.NextId(),
                F_StockInPUId = newEntity.id,
                F_Type = "删除",
                F_Title = "删除采购入库商品",
                F_Content = $"删除商品: 商品ID={deletedItem.F_CustomerId}, 数量={deletedItem.F_Num}, 单价={deletedItem.F_Price}, 备注={deletedItem.F_Description}",
                F_CreatorUserId = _userManager.UserId,
                F_CreatorTime = currentTime,
                TenantId = _userManager.TenantId
            });
        }

        // 修改的子表数据
        var modifiedItems = newItems.Where(it => !string.IsNullOrEmpty(it.F_Id) && originalItemIds.Contains(it.F_Id)).ToList();
        foreach (var newItem in modifiedItems)
        {
            var originalItem = originalItems.First(it => it.F_Id == newItem.F_Id);
            var changes = new List<string>();

            if (originalItem.F_CustomerId != newItem.F_CustomerId)
                changes.Add($"商品ID: 从 '{originalItem.F_CustomerId}' 改为 '{newItem.F_CustomerId}'");

            if (originalItem.F_Num != newItem.F_Num)
                changes.Add($"数量: 从 '{originalItem.F_Num}' 改为 '{newItem.F_Num}'");

            if (originalItem.F_Price != newItem.F_Price)
                changes.Add($"单价: 从 '{originalItem.F_Price}' 改为 '{newItem.F_Price}'");

            if (originalItem.F_Description != newItem.F_Description)
                changes.Add($"备注: 从 '{originalItem.F_Description}' 改为 '{newItem.F_Description}'");

            if (changes.Any())
            {
                logs.Add(new APuStockPuLogEntity
                {
                    F_Id = SnowflakeIdHelper.NextId(),
                    F_StockInPUId = newEntity.id,
                    F_Type = "修改",
                    F_Title = "修改采购入库商品",
                    F_Content = $"修改商品信息: {string.Join(", ", changes)}",
                    F_CreatorUserId = _userManager.UserId,
                    F_CreatorTime = currentTime,
                    TenantId = _userManager.TenantId
                });
            }
        }

        // 批量插入日志
        if (logs.Any())
        {
            await _repository.AsSugarClient().Insertable(logs).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 根据采购单ID获取采购单商品列表
    /// </summary>
    /// <param name="orderId">采购单ID</param>
    /// <returns>采购单商品列表</returns>
    [HttpGet("Actions/GetOrderItems")]
    public async Task<dynamic> GetOrderItems(string orderId)
    {
        if (string.IsNullOrEmpty(orderId))
        {
            return new List<dynamic>();
        }

        var data = await _repositoryorder.AsQueryable()
            .Where(it => it.F_OrderId == orderId && it.DeleteMark == null)
            .Select(it => new
            {
                it.F_Id,
                it.F_OrderId,
                it.F_CustomerId,
                it.F_SupplierId,
                it.F_Num,
                it.F_Price,
                it.F_Discount,
                it.F_DiscountMoney,
                it.F_Description,
                it.F_quote,
            })
            .ToListAsync();

        // 获取商品ID列表
        var customerIds = data.Where(it => !string.IsNullOrEmpty(it.F_CustomerId))
            .Select(it => it.F_CustomerId)
            .Distinct()
            .ToList();

        // 批量查询商品规格和编号信息
        var goodsDict = new Dictionary<string, (string Specification, string GoodsNo)>();
        if (customerIds.Any())
        {
            var goodsList = await _repository.AsSugarClient().Queryable<AGoodsEntity>()
                .In(it => it.id, customerIds)
                .Select(it => new { it.id, it.F_Specification, it.F_GoodsNo })
                .ToListAsync();

            goodsDict = goodsList.ToDictionary(
                it => it.id, 
                it => (it.F_Specification ?? "", it.F_GoodsNo ?? "")
            );
        }

        // 将商品信息填充到返回数据中
        var result = data.Select(it => new
        {
            it.F_Id,
            it.F_OrderId,
            it.F_CustomerId,
            it.F_SupplierId,
            it.F_Num,
            it.F_Price,
            it.F_Discount,
            it.F_DiscountMoney,
            it.F_Description,
            it.F_quote,
            //F_Specification = !string.IsNullOrEmpty(it.F_CustomerId) && goodsDict.ContainsKey(it.F_CustomerId) 
            //    ? goodsDict[it.F_CustomerId].Specification 
            //    : "",
            //F_GoodsNo = !string.IsNullOrEmpty(it.F_CustomerId) && goodsDict.ContainsKey(it.F_CustomerId) 
            //    ? goodsDict[it.F_CustomerId].GoodsNo 
            //    : ""
        }).ToList();

        return result;
    }

    /// <summary>
    /// 按商品ID、仓库ID、库存编码定位总库存（子表明细 F_Encoding 对应 a_goods_inventory.F_Code）.
    /// </summary>
    private async Task<AGoodsInventoryEntity?> GetPuInventoryAsync(string encoding)
    {
        if (string.IsNullOrEmpty(encoding)) return null;
        return await _repositoryInventory.GetFirstAsync(it =>
            it.DeleteMark == null
            && it.F_Code == encoding);
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
        return string.Join("-", names);
    }
}
