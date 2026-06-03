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
using JNPF.example.Entitys.Dto.APuOutStockOther;
using JNPF.example.Entitys.Dto.APuOutStockOtherItem;
using JNPF.example.Entitys.Dto.APuOutStockOtherLog;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_pu_out_stock_other.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuOutStockOther", Order = 200)]
[Route("api/example/[controller]")]
public class APuOutStockOtherService : IAPuOutStockOtherService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuOutStockOtherEntity> _repository;
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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"其他出库单号\",\"field\":\"F_StockOutNo\"},{\"value\":\"仓库\",\"field\":\"F_WarehouseId\"},{\"value\":\"出库类型\",\"field\":\"F_StockOutType\"},{\"value\":\"出库日期\",\"field\":\"F_StockOutDate\"},{\"value\":\"发货人\",\"field\":\"F_StockOutUserId\"},{\"value\":\"加工单号\",\"field\":\"F_ProcessNo\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="APuOutStockOtherService"/>类型的新实例.
    /// </summary>
    public APuOutStockOtherService(
        ISqlSugarRepository<APuOutStockOtherEntity> repository,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repository = repository;
        _repositoryInventory = repositoryInventory;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_pu_out_stock_other.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.APuOutStockOtherItemList)
            .Includes(it => it.APuOutStockOtherLogList)
            .Select(it => new APuOutStockOtherEntity
            {
                id = it.id,
                F_StockOutNo = it.F_StockOutNo,
                F_WarehouseId = it.F_WarehouseId,
                F_StockOutType = it.F_StockOutType,
                F_StockOutDate = it.F_StockOutDate,
                F_StockOutUserId = it.F_StockOutUserId,
                F_ProcessNo = it.F_ProcessNo,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                APuOutStockOtherItemList = it.APuOutStockOtherItemList.Where(item => item.DeleteMark == null).ToList(),
                APuOutStockOtherLogList = it.APuOutStockOtherLogList.Where(log => log.DeleteMark == null).ToList(),
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuOutStockOtherInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField44e07d, async aPuOutStockOtherItem =>
        {
            // 创建人员
            aPuOutStockOtherItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOutStockOtherItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOutStockOtherItem.F_CreatorTime = aPuOutStockOtherItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField211772, async aPuOutStockOtherLog =>
        {
            // 创建人员
            aPuOutStockOtherLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOutStockOtherLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOutStockOtherLog.F_CreatorTime = aPuOutStockOtherLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

    /// <summary>
    /// 获取a_pu_out_stock_other列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuOutStockOtherListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuOutStockOtherItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuOutStockOtherLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuOutStockOtherListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_out_stock_other"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_out_stock_other_item"))) aPuOutStockOtherItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_out_stock_other_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_out_stock_other_log"))) aPuOutStockOtherLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_out_stock_other_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutStockOtherEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutStockOtherItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField44e07d-", entityInfo, 1);
        List<IConditionalModel> aPuOutStockOtherItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuOutStockOtherItemConditionalModel.AddRange(aPuOutStockOtherItemAuthorizeWhere);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutStockOtherLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField211772-", entityInfo, 1);
        List<IConditionalModel> aPuOutStockOtherLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuOutStockOtherLogConditionalModel.AddRange(aPuOutStockOtherLogAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutStockOtherEntity>();
        var F_WarehouseId = input.F_WarehouseId?.Last();
        var F_StockOutTypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_StockOutType").DbColumnName;
        var F_StockOutUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_StockOutUserId").DbColumnName;
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_StockOutNo), it => it.F_StockOutNo.Contains(input.F_StockOutNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()), it => it.F_WarehouseId.Contains(F_WarehouseId))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_StockOutTypeDbColumnName, input.F_StockOutType))
            .WhereIF(input.F_StockOutDate?.Count() > 0, it => SqlFunc.Between(it.F_StockOutDate, input.F_StockOutDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_StockOutDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_StockOutUserIdDbColumnName, input.F_StockOutUserId))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .Where(authorizeWhere)
            .WhereIF(aPuOutStockOtherItemAuthorizeWhere?.Count > 0, it => it.APuOutStockOtherItemList.Any(aPuOutStockOtherItemAuthorizeWhere))
            .WhereIF(aPuOutStockOtherLogAuthorizeWhere?.Count > 0, it => it.APuOutStockOtherLogList.Any(aPuOutStockOtherLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aPuOutStockOtherItemConditionalModel.Count > 0, it => it.APuOutStockOtherItemList.Any(aPuOutStockOtherItemConditionalModel))
            .WhereIF(aPuOutStockOtherLogConditionalModel.Count > 0, it => it.APuOutStockOtherLogList.Any(aPuOutStockOtherLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
             //.OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
             .OrderByIF(string.IsNullOrEmpty(input.sidx), "F_CreatorTime DESC, id ASC").OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new APuOutStockOtherListOutput
            {
                id = it.id,
                F_StockOutNo = it.F_StockOutNo,
                F_WarehouseId = it.F_WarehouseId,
                F_StockOutType = it.F_StockOutType,
                F_StockOutDate = it.F_StockOutDate.Value.ToString("yyyy-MM-dd"),
                F_StockOutUserId = it.F_StockOutUserId,
                F_ProcessNo = it.F_ProcessNo,
                F_CreatorUserId = it.F_CreatorUserId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 仓库ID
            var F_WarehouseIdData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (item.F_WarehouseId != null)
            {
                var F_WarehouseIdCascader = item.F_WarehouseId.ToObject<List<string>>();
                // 获取所有匹配的仓库名称，用逗号连接
                var warehouseNames = F_WarehouseIdData.FindAll(it => F_WarehouseIdCascader.Contains(it.id)).Select(s => s.fullName);
                item.F_WarehouseId = string.Join(",", warehouseNames);
            }

            // 出库类型
            if (item.F_StockOutType != null)
            {
                item.F_StockOutType = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(item.F_StockOutType) && it.DictionaryTypeId.Equals("2013096194263355392")).Select(it => it.FullName).FirstAsync();
            }

            // 发货人
            if(item.F_StockOutUserId != null)
            {
                item.F_StockOutUserId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_StockOutUserId)))?.RealName;
            }

            // 创建人员
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;

        });

        var resData = data.list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<APuOutStockOtherEntity>(new APuOutStockOtherEntity()));
        ConfigModel tableField44e07dConfig = new ConfigModel
        {
            tableName = "a_pu_out_stock_other_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品管理",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<APuOutStockOtherItemEntity>(new APuOutStockOtherItemEntity())
        };
        FieldsModel tableField44e07d = new FieldsModel()
        {
            __config__ = tableField44e07dConfig,
            __vModel__ = "tableField44e07d"
        };
        fieldList.Add(tableField44e07d);

        resData = await _controlParsing.GetParsDataList(resData, "tableField44e07d-F_CustomerId", "popupSelect", _userManager.TenantId, fieldList);

        data.list = resData.ToObject<List<APuOutStockOtherListOutput>>(CommonConst.options);
        return PageResult<APuOutStockOtherListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_pu_out_stock_other.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] APuOutStockOtherCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuOutStockOtherEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuOutStockOtherEntity>(new APuOutStockOtherEntity()));
        ConfigModel tableField44e07dConfig = new ConfigModel
        {
            tableName = "a_pu_out_stock_other_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品管理",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutStockOtherItemEntity>(new APuOutStockOtherItemEntity())
        };
        FieldsModel tableField44e07dModel = new FieldsModel()
        {
            __config__ = tableField44e07dConfig,
            __vModel__ = "tableField44e07d"
        };
        fieldList.Add(tableField44e07dModel);
        ConfigModel tableField211772Config = new ConfigModel
        {
            tableName = "a_pu_out_stock_other_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "设计子表",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutStockOtherLogEntity>(new APuOutStockOtherLogEntity())
        };
        FieldsModel tableField211772Model = new FieldsModel()
        {
            __config__ = tableField211772Config,
            __vModel__ = "tableField211772"
        };
        fieldList.Add(tableField211772Model);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        // 如果前端没有传F_StockOutNo，则自动生成
        if (string.IsNullOrEmpty(entity.F_StockOutNo))
        {
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);

            // 查询今天已有的记录数量
            var todayCount = await _repository.AsSugarClient()
                .Queryable<APuOutStockOtherEntity>()
                .Where(x => x.F_CreatorTime >= today && x.F_CreatorTime < tomorrow)
                .CountAsync();

            // 生成编号：QTCK + 当前日期 + 序号（3位，不足补0）
            var dateStr = DateTime.Now.ToString("yyyyMMdd");
            var sequence = (todayCount + 1).ToString("D3");
            entity.F_StockOutNo = $"QTCK{dateStr}{sequence}";
        }

        var aPuOutStockOtherItemEntityList = input.tableField44e07d.Adapt<List<APuOutStockOtherItemEntity>>();
        if(aPuOutStockOtherItemEntityList != null)
        {
            foreach (var item in aPuOutStockOtherItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuOutStockOtherItemList = aPuOutStockOtherItemEntityList;
        }

        var aPuOutStockOtherLogEntityList = input.tableField211772.Adapt<List<APuOutStockOtherLogEntity>>();
        if(aPuOutStockOtherLogEntityList != null)
        {
            foreach (var item in aPuOutStockOtherLogEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuOutStockOtherLogList = aPuOutStockOtherLogEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.APuOutStockOtherItemList)
            .Include(it => it.APuOutStockOtherLogList)
            .ExecuteCommandAsync();

        // 减库存：遍历新增的商品列表，按 商品ID(F_CustomerId) 与 出库单的 仓库ID(F_WarehouseId) 在库存表中匹配并减少数量
        if (entity.APuOutStockOtherItemList != null && entity.APuOutStockOtherItemList.Any() && !string.IsNullOrEmpty(entity.F_WarehouseId))
        {
            foreach (var item in entity.APuOutStockOtherItemList)
            {
                // 商品ID 保存在 F_CustomerId，出库数量在 F_Num
                if (string.IsNullOrEmpty(item.F_CustomerId) || item.F_Num == null) continue;

                var inv = await _repositoryInventory.AsSugarClient()
                    .Queryable<AGoodsInventoryEntity>()
                    .Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == entity.F_WarehouseId)
                    .FirstAsync();

                if (inv != null)
                {
                    var newNum = (inv.F_Num ?? 0) - (item.F_Num ?? 0);
                    // 避免出现负库存，这里设为最小值 0（若您希望抛错或记录日志，可调整此行为）
                    if (newNum < 0) newNum = 0;
                    inv.F_Num = newNum;
                    await _repositoryInventory.AsUpdateable(inv).UpdateColumns(it => new { it.F_Num }).ExecuteCommandAsync();
                }
            }
        }
    }

    /// <summary>
    /// 更新a_pu_out_stock_other.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] APuOutStockOtherUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuOutStockOtherEntity>();
        entity.id = id; // 确保ID正确设置
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_StockOutNo.Equals(input.F_StockOutNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "其他出货单号");

        if (string.IsNullOrEmpty(entity.F_StockOutNo))
        {
            entity.F_StockOutNo = oldEntity.F_StockOutNo;
        }

        // 获取原有数据，用于比较字段变化
        var originalEntity = await _repository.AsSugarClient()
            .Queryable<APuOutStockOtherEntity>()
            .Where(x => x.id == id)
            .FirstAsync();

        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuOutStockOtherEntity>(new APuOutStockOtherEntity()));
        ConfigModel tableField44e07dConfig = new ConfigModel
        {
            tableName = "a_pu_out_stock_other_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品管理",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutStockOtherItemEntity>(new APuOutStockOtherItemEntity())
        };
        FieldsModel tableField44e07dModel = new FieldsModel()
        {
            __config__ = tableField44e07dConfig,
            __vModel__ = "tableField44e07d"
        };
        fieldList.Add(tableField44e07dModel);
        ConfigModel tableField211772Config = new ConfigModel
        {
            tableName = "a_pu_out_stock_other_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "操作日志",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutStockOtherLogEntity>(new APuOutStockOtherLogEntity())
        };
        FieldsModel tableField211772Model = new FieldsModel()
        {
            __config__ = tableField211772Config,
            __vModel__ = "tableField211772"
        };
        fieldList.Add(tableField211772Model);

        // 移除其他出库单商品列表可能被删除数据
        var deletedItemIds = new List<string>();
        List<APuOutStockOtherItemEntity> deletedItems = new List<APuOutStockOtherItemEntity>();
        if (input.tableField44e07d != null && input.tableField44e07d.Any())
        {
            // 获取将被删除的商品ID
            var existingItemIds = input.tableField44e07d.Where(it => !string.IsNullOrEmpty(it.F_Id)).Select(it => it.F_Id).ToList();
            deletedItems = await _repository.AsSugarClient()
                .Queryable<APuOutStockOtherItemEntity>()
                .Where(it => it.F_StockOutOTId == entity.id && !existingItemIds.Contains(it.F_Id))
                .ToListAsync();

            deletedItemIds = deletedItems.Select(it => it.F_Id).ToList();

            await _repository.AsSugarClient().Deleteable<APuOutStockOtherItemEntity>()
                .Where(it => it.F_StockOutOTId == entity.id && !existingItemIds.Contains(it.F_Id))
                .IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
        else
        {
            // 获取所有将被删除的商品
            deletedItems = await _repository.AsSugarClient()
                .Queryable<APuOutStockOtherItemEntity>()
                .Where(it => it.F_StockOutOTId == entity.id)
                .ToListAsync();

            deletedItemIds = deletedItems.Select(it => it.F_Id).ToList();

            await _repository.AsSugarClient().Deleteable<APuOutStockOtherItemEntity>()
                .Where(it => it.F_StockOutOTId == entity.id)
                .IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }

        // 记录删除商品的日志
        if (deletedItemIds.Any())
        {
                try
                {
                    // 将被删除商品的商品名称收集（避免记录 id），优先使用已查询到的 deletedItems
                    var deletedProducts = new List<string>();
                    var productList = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2015036768293883904", "F_Id", "F_GoodsName", "");
                    if (deletedItems != null && deletedItems.Any())
                    {
                        foreach (var di in deletedItems)
                        {
                            var name = FindDisplayFromDynamicList(productList, di.F_CustomerId) ?? di.F_CustomerId;
                            deletedProducts.Add(name);
                        }
                    }

                    var productSummary = deletedProducts.Any() ? $"商品：{string.Join(",", deletedProducts)}" : $"商品名称无法解析";
                    await LogSubTableOperation(entity.id, "删除", $"删除了 {deletedItemIds.Count} 条商品数据，{productSummary}");
                }
                catch
                {
                    // 回退：尽量不记录商品 id，尝试解析名称列表，若失败则记录通用提示
                    var fallbackNames = new List<string>();
                    try
                    {
                        var productListFallback = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2015036768293883904", "F_Id", "F_GoodsName", "");
                        if (deletedItems != null && deletedItems.Any())
                        {
                            foreach (var di in deletedItems)
                            {
                                var name = FindDisplayFromDynamicList(productListFallback, di.F_CustomerId) ?? "未知商品";
                                fallbackNames.Add(name);
                            }
                        }
                    }
                    catch { }
                    var fallbackSummary = fallbackNames.Any() ? $"商品：{string.Join(",", fallbackNames)}" : "商品名称无法解析";
                    await LogSubTableOperation(entity.id, "删除", $"删除了 {deletedItemIds.Count} 条商品数据，{fallbackSummary}");
                }
        }

        // 新增其他出库单商品列表新数据
        var aPuOutStockOtherItemEntityList = input.tableField44e07d.Adapt<List<APuOutStockOtherItemEntity>>();


        // 移除其他出库单号日志可能被删除数据
        var deletedLogIds = new List<string>();
        if (input.tableField211772 !=null && input.tableField211772.Any())
        {
            // 获取将被删除的日志ID
            var existingLogIds = input.tableField211772.Where(it => !string.IsNullOrEmpty(it.F_Id)).Select(it => it.F_Id).ToList();
            var deletedLogs = await _repository.AsSugarClient()
                .Queryable<APuOutStockOtherLogEntity>()
                .Where(it => it.F_StockOutOTId == entity.id && !existingLogIds.Contains(it.F_Id))
                .ToListAsync();

            deletedLogIds = deletedLogs.Select(it => it.F_Id).ToList();

            await _repository.AsSugarClient().Deleteable<APuOutStockOtherLogEntity>()
                .Where(it => it.F_StockOutOTId == entity.id && !existingLogIds.Contains(it.F_Id))
                .IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
        else
        {
            // 获取所有将被删除的日志
            var allLogs = await _repository.AsSugarClient()
                .Queryable<APuOutStockOtherLogEntity>()
                .Where(it => it.F_StockOutOTId == entity.id)
                .ToListAsync();

            deletedLogIds = allLogs.Select(it => it.F_Id).ToList();

            await _repository.AsSugarClient().Deleteable<APuOutStockOtherLogEntity>()
                .Where(it => it.F_StockOutOTId == entity.id)
                .IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }

        // 新增其他出库单号日志新数据
        var aPuOutStockOtherLogEntityList = input.tableField211772.Adapt<List<APuOutStockOtherLogEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_StockOutNo,
                    it.F_WarehouseId,
                    it.F_StockOutType,
                    it.F_StockOutDate,
                    it.F_StockOutUserId,
                    it.F_ProcessNo,
                })
                .ExecuteCommandAsync();

            // 比较字段变化并记录日志
            await LogFieldChanges(originalEntity, entity);

            if (aPuOutStockOtherItemEntityList != null)
            {
                // 读取一次商品接口数据以用于转换
                var productList = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2015036768293883904", "F_Id", "F_GoodsName", "");
                foreach (var item in aPuOutStockOtherItemEntityList)
                {
                    if (item.F_Id.IsNullOrEmpty())
                    {
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_StockOutOTId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();

                        // 记录新增商品日志：只记录关键信息且把商品 id 转为名称
                        try
                        {
                            var productName = FindDisplayFromDynamicList(productList, item.F_CustomerId) ?? "未知商品";
                            var summary = $"商品：{productName}";
                            if (item.F_Num != null) summary += $", 数量：{item.F_Num}";
                            if (item.F_Price != null) summary += $", 价格：{item.F_Price}";
                            if (!string.IsNullOrEmpty(item.F_Description)) summary += $", 备注：{item.F_Description}";
                            await LogSubTableOperation(entity.id, "新增", $"新增商品：{summary}");
                        }
                        catch
                        {
                            // 回退：尽量不写入商品 id，使用商品名称占位
                            var fallbackName = "未知商品";
                            try { fallbackName = FindDisplayFromDynamicList(productList, item.F_CustomerId) ?? "未知商品"; } catch { }
                            await LogSubTableOperation(entity.id, "新增", $"新增商品：{fallbackName}");
                        }
                    }
                    else
                    {
                        // 获取修改前的数据以比较字段变化
                        var oldItem = await _repository.AsSugarClient()
                            .Queryable<APuOutStockOtherItemEntity>()
                            .Where(it => it.F_Id == item.F_Id)
                            .FirstAsync();

                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_StockOutOTId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

                        // 记录修改商品日志：仅记录发生变化的字段并转换 id 为可读值
                        try
                        {
                            var changes = new List<string>();
                            if (oldItem.F_CustomerId != item.F_CustomerId)
                            {
                                var oldName = FindDisplayFromDynamicList(productList, oldItem.F_CustomerId) ?? "未知商品";
                                var newName = FindDisplayFromDynamicList(productList, item.F_CustomerId) ?? "未知商品";
                                changes.Add($"商品：从 '{oldName}' 改为 '{newName}'");
                            }
                            if (oldItem.F_Num != item.F_Num)
                            {
                                changes.Add($"数量：从 '{oldItem.F_Num}' 改为 '{item.F_Num}'");
                            }
                            if (oldItem.F_Price != item.F_Price)
                            {
                                changes.Add($"价格：从 '{oldItem.F_Price}' 改为 '{item.F_Price}'");
                            }
                            if (oldItem.F_Description != item.F_Description)
                            {
                                changes.Add($"备注：从 '{oldItem.F_Description}' 改为 '{item.F_Description}'");
                            }
                            if (changes.Any())
                            {
                                var itemProductName = FindDisplayFromDynamicList(productList, item.F_CustomerId) ?? "子表商品";
                                await LogSubTableOperation(entity.id, "修改", $"修改商品 {itemProductName}：{string.Join("；", changes)}");
                            }
                        }
                        catch
                        {
                            // 回退：尽量不写入商品 id，使用商品名称占位
                            var fallbackName = "子表商品";
                            try { fallbackName = FindDisplayFromDynamicList(productList, item.F_CustomerId) ?? "商品"; } catch { }
                            await LogSubTableOperation(entity.id, "修改", $"修改商品：{fallbackName}");
                        }
                        
                        // 按数量差值同步库存（增加或减少）
                        try
                        {
                            if (!string.IsNullOrEmpty(entity.F_WarehouseId) && !string.IsNullOrEmpty(item.F_CustomerId))
                            {
                                var oldQty = oldItem?.F_Num ?? 0;
                                var newQty = item.F_Num ?? 0;
                                var diff = newQty - oldQty; // 正数 => 减库存；负数 => 增库存
                                if (diff != 0)
                                {
                                    var inv = await _repositoryInventory.AsSugarClient()
                                        .Queryable<AGoodsInventoryEntity>()
                                        .Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == entity.F_WarehouseId)
                                        .FirstAsync();
                                    if (inv != null)
                                    {
                                        var updated = (inv.F_Num ?? 0) - diff;
                                        if (updated < 0) updated = 0;
                                        inv.F_Num = updated;
                                        await _repositoryInventory.AsUpdateable(inv).UpdateColumns(it => new { it.F_Num }).ExecuteCommandAsync();
                                    }
                                    else
                                    {
                                        // 若库存记录不存在且需要增加库存（diff < 0），则插入一条记录
                                        if (diff < 0)
                                        {
                                            var newInv = new AGoodsInventoryEntity
                                            {
                                                id = SnowflakeIdHelper.NextId(),
                                                F_GoodsId = item.F_CustomerId,
                                                F_WarehouseId = entity.F_WarehouseId,
                                                F_Num = -diff
                                            };
                                            await _repositoryInventory.AsInsertable(newInv).ExecuteCommandAsync();
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // 忽略库存更新错误，避免影响主流程；可记录或告警以便排查
                        }
                    }
                }
            }

            if(aPuOutStockOtherLogEntityList != null)
            {
                foreach (var item in aPuOutStockOtherLogEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_StockOutOTId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_StockOutOTId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }

        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 删除a_pu_out_stock_other.
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
    /// 批量删除a_pu_out_stock_other.
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
            // 在逻辑删除其他出库单之前，恢复对应库存（遍历每张单的商品，按 商品ID 与 仓库ID 增加库存数量）
            try
            {
                var deletedIds = entitys.Select(e => e.id).ToList();
                var items = await _repository.AsSugarClient().Queryable<APuOutStockOtherItemEntity>()
                    .Where(it => deletedIds.Contains(it.F_StockOutOTId) && it.DeleteMark == null)
                    .ToListAsync();
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        if (string.IsNullOrEmpty(item.F_CustomerId) || item.F_Num == null) continue;
                        var parent = entitys.Find(e => e.id.Equals(item.F_StockOutOTId));
                        if (parent == null || string.IsNullOrEmpty(parent.F_WarehouseId)) continue;
                        var inv = await _repositoryInventory.AsSugarClient()
                            .Queryable<AGoodsInventoryEntity>()
                            .Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == parent.F_WarehouseId)
                            .FirstAsync();
                        if (inv != null)
                        {
                            inv.F_Num = (inv.F_Num ?? 0) + (item.F_Num ?? 0);
                            await _repositoryInventory.AsUpdateable(inv).UpdateColumns(it => new { it.F_Num }).ExecuteCommandAsync();
                        }
                        else
                        {
                            var newInv = new AGoodsInventoryEntity
                            {
                                id = SnowflakeIdHelper.NextId(),
                                F_GoodsId = item.F_CustomerId,
                                F_WarehouseId = parent.F_WarehouseId,
                                F_Num = item.F_Num
                            };
                            await _repositoryInventory.AsInsertable(newInv).ExecuteCommandAsync();
                        }
                    }
                }
            }
            catch
            {
              
            }

            // 批量删除a_pu_out_stock_other
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_pu_out_stock_other详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.APuOutStockOtherItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuOutStockOtherLogList)
            .Where(it => it.id == id)
            .Select(it => new APuOutStockOtherDetailOutput
            {
                id = it.id,
                F_StockOutNo = it.F_StockOutNo,
                F_WarehouseId = it.F_WarehouseId,
                F_StockOutType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_StockOutType) && dic.DictionaryTypeId.Equals("2013096194263355392")).Select(dic => dic.FullName),
                F_StockOutDate = it.F_StockOutDate.Value.ToString("yyyy-MM-dd"),
                F_StockOutUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_StockOutUserId)).Select(u => u.RealName),
                F_ProcessNo = it.F_ProcessNo,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableField44e07d = it.APuOutStockOtherItemList.Where(item => item.DeleteMark == null).Adapt<List<APuOutStockOtherItemDetailOutput>>(),
                tableField211772 = it.APuOutStockOtherLogList.Where(log => log.DeleteMark == null).Adapt<List<APuOutStockOtherLogDetailOutput>>(),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 仓库ID
            var F_WarehouseIdData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (item.F_WarehouseId != null)
            {
                var F_WarehouseIdCascader = item.F_WarehouseId.ToObject<List<string>>();
                // 获取所有匹配的仓库名称，用逗号连接
                var warehouseNames = F_WarehouseIdData.FindAll(it => F_WarehouseIdCascader.Contains(it.id)).Select(s => s.fullName);
                item.F_WarehouseId = string.Join(",", warehouseNames);
            }

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField44e07d), async aPuOutStockOtherItem =>
        {
            var aPuOutStockOther = data.ToList().Find(it => it.id.Equals(aPuOutStockOtherItem.F_StockOutOTId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuOutStockOtherItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOutStockOtherItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOutStockOtherItem.F_CreatorTime = aPuOutStockOtherItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField211772), async aPuOutStockOtherLog =>
        {
            var aPuOutStockOther = data.ToList().Find(it => it.id.Equals(aPuOutStockOtherLog.F_StockOutOTId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuOutStockOtherLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOutStockOtherLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOutStockOtherLog.F_CreatorTime = aPuOutStockOtherLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<APuOutStockOtherEntity>(new APuOutStockOtherEntity()));
        ConfigModel tableField44e07dConfig = new ConfigModel
        {
            tableName = "a_pu_out_stock_other_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品管理",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<APuOutStockOtherItemEntity>(new APuOutStockOtherItemEntity())
        };
        FieldsModel tableField44e07d = new FieldsModel()
        {
            __config__ = tableField44e07dConfig,
            __vModel__ = "tableField44e07d"
        };
        fieldList.Add(tableField44e07d);

        resData = await _controlParsing.GetParsDataList(resData,"tableField44e07d-F_CustomerId","popupSelect",_userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }

    /// <summary>
    /// 比较字段变化并记录日志
    /// </summary>
    private async Task LogFieldChanges(APuOutStockOtherEntity originalEntity, APuOutStockOtherEntity newEntity)
    {
        var logEntries = new List<APuOutStockOtherLogEntity>();

        // 局部异步函数：检查字段是否变化并添加日志（把 id 转为可读值）
        async Task AddIfChanged<T>(string propName, T origVal, T newVal, string displayLabel)
        {
            if (!Equals(origVal, newVal))
            {
                var origDisplay = await ResolveFieldDisplayValue(propName, origVal);
                var newDisplay = await ResolveFieldDisplayValue(propName, newVal);
                logEntries.Add(CreateLogEntry(newEntity.id, "修改", $"{displayLabel}：从 '{origDisplay}' 改为 '{newDisplay}'"));
            }
        }

        await AddIfChanged("F_StockOutNo", originalEntity.F_StockOutNo, newEntity.F_StockOutNo, "修改出库单号");
        await AddIfChanged("F_WarehouseId", originalEntity.F_WarehouseId, newEntity.F_WarehouseId, "修改仓库");
        await AddIfChanged("F_StockOutType", originalEntity.F_StockOutType, newEntity.F_StockOutType, "修改出库类型");
        await AddIfChanged("F_StockOutDate", originalEntity.F_StockOutDate, newEntity.F_StockOutDate, "修改出库日期");
        await AddIfChanged("F_StockOutUserId", originalEntity.F_StockOutUserId, newEntity.F_StockOutUserId, "修改发货人");
        await AddIfChanged("F_ProcessNo", originalEntity.F_ProcessNo, newEntity.F_ProcessNo, "修改加工单号");

        // 批量插入日志记录
        if (logEntries.Any())
        {
            await _repository.AsSugarClient().Insertable(logEntries).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 将字段的存储值（通常为 id 或 code）解析为可读显示值，用于日志记录中避免记录原始 id
    /// </summary>
    private async Task<string> ResolveFieldDisplayValue(string fieldName, object value)
    {
        if (value == null) return null;
        var valStr = value.ToString();
        if (string.IsNullOrEmpty(valStr)) return null;

        try
        {
            switch (fieldName)
            {
                case "F_WarehouseId":
                    // 使用已有的数据接口把仓库 id 转为名称（与列表展示保持一致）
                    var warehouseList = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2012073572784279552", "F_Id", "F_Name", "");
                    return warehouseList?.Find(it => it.id.Equals(valStr))?.fullName ?? valStr;
                case "F_StockOutType":
                    // 字典类型转为 FullName
                    var dictName = await _repository.AsSugarClient()
                        .Queryable<DictionaryDataEntity>()
                        .Where(it => it.EnCode.Equals(valStr) && it.DictionaryTypeId.Equals("2013096194263355392"))
                        .Select(it => it.FullName)
                        .FirstAsync();
                    return string.IsNullOrEmpty(dictName) ? valStr : dictName;
                case "F_StockOutUserId":
                case "F_CreatorUserId":
                    // 用户 id -> RealName/Account（与其它处展示保持一致）
                    var userDisplay = await _repository.AsSugarClient()
                        .Queryable<UserEntity>()
                        .Where(u => u.Id.Equals(valStr))
                        .Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account))
                        .FirstAsync();
                    return string.IsNullOrEmpty(userDisplay) ? valStr : userDisplay;
                case "F_StockOutDate":
                    if (DateTime.TryParse(valStr, out var dt)) return dt.ToString("yyyy-MM-dd");
                    return valStr;
                case "F_CustomerId":
                    // 子表商品 id -> 商品名称（使用商品数据接口）
                    var goodsList = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2015036768293883904", "F_Id", "F_GoodsName", "");
                    try
                    {
                        var found = FindDisplayFromDynamicList(goodsList, valStr);
                        return string.IsNullOrEmpty(found) ? valStr : found;
                    }
                    catch
                    {
                        return valStr;
                    }
                default:
                    return valStr;
            }
        }
        catch
        {
            // 解析失败回退为原始值，避免抛出影响主流程
            return valStr;
        }
    }

    /// <summary>
    /// 记录子表操作日志
    /// </summary>
    private async Task LogSubTableOperation(string mainId, string operationType, string content)
    {
        var logEntry = CreateLogEntry(mainId, operationType, content);
        await _repository.AsSugarClient().Insertable(logEntry).ExecuteCommandAsync();
    }

    /// <summary>
    /// 创建日志条目
    /// </summary>
    private APuOutStockOtherLogEntity CreateLogEntry(string mainId, string title, string content)
    {
        return new APuOutStockOtherLogEntity
        {
            F_Id = SnowflakeIdHelper.NextId(),
            F_StockOutOTId = mainId,
            F_Type = "操作日志",
            F_Title = title,
            F_Content = content,
            F_CreatorUserId = _userManager.UserId,
            F_CreatorTime = DateTime.Now,
            TenantId = _userManager.TenantId
        };
    }

    /// <summary>
    /// 从动态数据接口返回列表中，通过多字段匹配 id 并返回可读名称（尝试多个显示字段）
    /// </summary>
    private string FindDisplayFromDynamicList(IEnumerable<object> list, string val)
    {
        if (list == null || string.IsNullOrEmpty(val)) return null;
        foreach (var item in list)
        {
            if (item == null) continue;
            var t = item.GetType();
            // 值字段候选
            var valueCandidates = new[] { "id", "value", "F_Id", "FId", "F_GoodsId", "key" };
            bool isMatch = false;
            foreach (var vc in valueCandidates)
            {
                var prop = t.GetProperty(vc);
                if (prop == null) continue;
                var v = prop.GetValue(item)?.ToString();
                if (!string.IsNullOrEmpty(v) && v.Equals(val, StringComparison.OrdinalIgnoreCase))
                {
                    isMatch = true;
                    break;
                }
            }
            if (!isMatch) continue;

            // 显示字段候选
            var displayCandidates = new[] { "fullName", "F_GoodsName", "name", "label", "text" };
            foreach (var dc in displayCandidates)
            {
                var pd = t.GetProperty(dc);
                if (pd == null) continue;
                var disp = pd.GetValue(item)?.ToString();
                if (!string.IsNullOrEmpty(disp)) return disp;
            }

            // 如果没有找到显示字段，尝试返回 id/value 本身
            foreach (var vc in valueCandidates)
            {
                var prop = t.GetProperty(vc);
                if (prop == null) continue;
                var v = prop.GetValue(item)?.ToString();
                if (!string.IsNullOrEmpty(v)) return v;
            }
        }
        return null;
    }
}
