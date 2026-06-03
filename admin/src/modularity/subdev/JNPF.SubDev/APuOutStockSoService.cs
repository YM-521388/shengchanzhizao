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
using JNPF.example.Entitys.Dto.APuOutStockSo;
using JNPF.example.Entitys.Dto.APuOutStockSoItem;
using JNPF.example.Entitys.Dto.APuOutStockSoLog;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_pu_out_stock_so.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuOutStockSo", Order = 200)]
[Route("api/example/[controller]")]
public class APuOutStockSoService : IAPuOutStockSoService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuOutStockSoEntity> _repository;
    private readonly ISqlSugarRepository<APuOutStockSoItemEntity> _repositoryItem;
    private readonly ISqlSugarRepository<ACtcContactEntity> _repositorylxr;
    private readonly ISqlSugarRepository<ACtcAddressEntity> _repositoryldz;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositorygoods;
    

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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"销售出库单号\",\"field\":\"F_StockOutNo\"},{\"value\":\"仓库\",\"field\":\"F_WarehouseId\"},{\"value\":\"出库类型\",\"field\":\"F_StockOutType\"},{\"value\":\"出库日期\",\"field\":\"F_StockOutDate\"},{\"value\":\"发货人\",\"field\":\"F_StockOutUserId\"},{\"value\":\"客户\",\"field\":\"F_CustomerId\"},{\"value\":\"联系人\",\"field\":\"F_ContactId\"},{\"value\":\"地址\",\"field\":\"F_AddressId\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="APuOutStockSoService"/>类型的新实例.
    /// </summary>
    public APuOutStockSoService(
        ISqlSugarRepository<APuOutStockSoItemEntity> repositoryItem,
        ISqlSugarRepository<APuOutStockSoEntity> repository,
        ISqlSugarRepository<ACtcContactEntity> repositorylxr,
        ISqlSugarRepository<ACtcAddressEntity> repositoryldz,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<AGoodsEntity> repositorygoods,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)  
    {
        _repositoryItem = repositoryItem;
        _repository = repository;
        _repositorylxr = repositorylxr;
        _repositoryldz = repositoryldz;
        _repositoryInventory = repositoryInventory;
        _repositorygoods = repositorygoods;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_pu_out_stock_so.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.APuOutStockSoItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuOutStockSoLogList)
            .Select(it => new APuOutStockSoEntity
            {
                id = it.id,
                F_StockOutNo = it.F_StockOutNo,
                F_WarehouseId = it.F_WarehouseId,
                F_StockOutType = it.F_StockOutType,
                F_StockOutDate = it.F_StockOutDate,
                F_StockOutUserId = it.F_StockOutUserId,
                F_CustomerId = it.F_CustomerId,
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                APuOutStockSoItemList = it.APuOutStockSoItemList.Where(item => item.DeleteMark == null).ToList(),
                APuOutStockSoLogList = it.APuOutStockSoLogList.Where(log => log.DeleteMark == null).ToList(),
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuOutStockSoInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFielde48def, async aPuOutStockSoItem =>
        {
            // 创建人员
            aPuOutStockSoItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOutStockSoItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOutStockSoItem.F_CreatorTime = aPuOutStockSoItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField67a4d7, async aPuOutStockSoLog =>
        {
            // 创建人员
            aPuOutStockSoLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOutStockSoLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOutStockSoLog.F_CreatorTime = aPuOutStockSoLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }
   
    /// <summary>
    /// 获取a_pu_out_stock_so列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuOutStockSoListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuOutStockSoItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuOutStockSoLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuOutStockSoListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_out_stock_so"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_out_stock_so_item"))) aPuOutStockSoItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_out_stock_so_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_out_stock_so_log"))) aPuOutStockSoLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_out_stock_so_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutStockSoEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutStockSoItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFielde48def-", entityInfo, 1);
        List<IConditionalModel> aPuOutStockSoItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuOutStockSoItemConditionalModel.AddRange(aPuOutStockSoItemAuthorizeWhere);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutStockSoLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField67a4d7-", entityInfo, 1);
        List<IConditionalModel> aPuOutStockSoLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuOutStockSoLogConditionalModel.AddRange(aPuOutStockSoLogAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutStockSoEntity>();
        var F_WarehouseId = input.F_WarehouseId?.Last();
        var F_StockOutTypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_StockOutType").DbColumnName;
        var F_StockOutUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_StockOutUserId").DbColumnName;
        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_StockOutNo), it => it.F_StockOutNo.Contains(input.F_StockOutNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()), it => it.F_WarehouseId.Contains(F_WarehouseId))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_StockOutTypeDbColumnName, input.F_StockOutType))
            .WhereIF(input.F_StockOutDate?.Count() > 0, it => SqlFunc.Between(it.F_StockOutDate, input.F_StockOutDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_StockOutDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_StockOutUserIdDbColumnName, input.F_StockOutUserId))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .Where(authorizeWhere)
            .WhereIF(aPuOutStockSoItemAuthorizeWhere?.Count > 0, it => it.APuOutStockSoItemList.Any(aPuOutStockSoItemAuthorizeWhere))
            .WhereIF(aPuOutStockSoLogAuthorizeWhere?.Count > 0, it => it.APuOutStockSoLogList.Any(aPuOutStockSoLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aPuOutStockSoItemConditionalModel.Count > 0, it => it.APuOutStockSoItemList.Any(aPuOutStockSoItemConditionalModel))
            .WhereIF(aPuOutStockSoLogConditionalModel.Count > 0, it => it.APuOutStockSoLogList.Any(aPuOutStockSoLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new APuOutStockSoListOutput
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
            .Select(it => new APuOutStockSoListOutput
            {
                id = it.id,
                F_StockOutNo = it.F_StockOutNo,
                F_WarehouseId = it.F_WarehouseId,
                F_StockOutType = it.F_StockOutType,
                F_StockOutDate = it.F_StockOutDate.Value.ToString("yyyy-MM-dd"),
                F_StockOutUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_StockOutUserId)).Select(u => u.RealName),
                F_CustomerId = SqlFunc.Subqueryable<ACustomerEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_CustomerId)).Select(u => u.F_CustomerName),
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
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
                // 获取所有匹配的仓库名称，用逗号连接
                var warehouseNames = F_WarehouseIdData.FindAll(it => F_WarehouseIdCascader.Contains(it.id)).Select(s => s.fullName);
                item.F_WarehouseId = string.Join(",", warehouseNames);
            }

            // 出库类型
            if (item.F_StockOutType != null)
            {
                item.F_StockOutType = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(item.F_StockOutType) && it.DictionaryTypeId.Equals("2013096194263355392")).Select(it => it.FullName).FirstAsync();
            }

            // 联系人
            linkageParameters = new List<DataInterfaceParameter>();
            linkageParameters.Add(new DataInterfaceParameter
            {
                field = "Id",
                relationField = "2009181616060108800",
                isSubTable = false,
                dataType = "varchar",
                defaultValue = "",
                fieldName = "",
                sourceType = 2
            });
            var F_ContactIdData = await _dataInterfaceService.GetDynamicList("F_ContactId", "2009459664370143232", "F_Id", "F_ContactName", "", linkageParameters);
            if (item.F_ContactId != null)
            {
                item.F_ContactId = F_ContactIdData.Find(it => it.id.Equals(item.F_ContactId))?.fullName;
            }

            // 地址
            linkageParameters = new List<DataInterfaceParameter>();
            linkageParameters.Add(new DataInterfaceParameter
            {
                field = "id",
                relationField = "2009181616060108800",
                isSubTable = false,
                dataType = "varchar",
                defaultValue = "",
                fieldName = "",
                sourceType = 2
            });
            var F_AddressIdData = await _dataInterfaceService.GetDynamicList("F_AddressId", "2009527238009163776", "F_Id", "F_Address", "", linkageParameters);
            if (item.F_AddressId != null)
            {
                item.F_AddressId = F_AddressIdData.Find(it => it.id.Equals(item.F_AddressId))?.fullName;
            }

        });

        data.pagination = idsQ.pagination;

        return PageResult<APuOutStockSoListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_pu_out_stock_so.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] APuOutStockSoCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuOutStockSoEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuOutStockSoEntity>(new APuOutStockSoEntity()));
        ConfigModel tableFielde48defConfig = new ConfigModel
        {
            tableName = "a_pu_out_stock_so_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品列表",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutStockSoItemEntity>(new APuOutStockSoItemEntity())
        };
        FieldsModel tableFielde48defModel = new FieldsModel()
        {
            __config__ = tableFielde48defConfig,
            __vModel__ = "tableFielde48def"
        };
        fieldList.Add(tableFielde48defModel);
        ConfigModel tableField67a4d7Config = new ConfigModel
        {
            tableName = "a_pu_out_stock_so_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "操作日志",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutStockSoLogEntity>(new APuOutStockSoLogEntity())
        };
        FieldsModel tableField67a4d7Model = new FieldsModel()
        {
            __config__ = tableField67a4d7Config,
            __vModel__ = "tableField67a4d7"
        };
        fieldList.Add(tableField67a4d7Model);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        // 自动生成出库单号（HTCK + yyyyMMdd + 当日序号）
        if (string.IsNullOrEmpty(entity.F_StockOutNo))
        {
            var today = DateTime.Now.ToString("yyyyMMdd");
            var prefix = $"HTCK{today}";
            var count = await _repository.AsQueryable()
                .Where(it => it.F_StockOutNo.StartsWith(prefix) && it.DeleteMark == null)
                .CountAsync();
            entity.F_StockOutNo = $"{prefix}{(count + 1).ToString("D3")}";
        }

        var aPuOutStockSoItemEntityList = input.tableFielde48def.Adapt<List<APuOutStockSoItemEntity>>();
        if(aPuOutStockSoItemEntityList != null)
        {
            foreach (var item in aPuOutStockSoItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuOutStockSoItemList = aPuOutStockSoItemEntityList;
        }

        var aPuOutStockSoLogEntityList = input.tableField67a4d7.Adapt<List<APuOutStockSoLogEntity>>();
        if(aPuOutStockSoLogEntityList != null)
        {
            foreach (var item in aPuOutStockSoLogEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuOutStockSoLogList = aPuOutStockSoLogEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.APuOutStockSoItemList)
            .Include(it => it.APuOutStockSoLogList)
            .ExecuteCommandAsync();

        // 减库存：遍历新增的商品列表，按 商品ID(F_CustomerId) 与 出库单的 仓库ID(F_WarehouseId) 在库存表中匹配并减少数量
        if (entity.APuOutStockSoItemList != null && entity.APuOutStockSoItemList.Any() && !string.IsNullOrEmpty(entity.F_WarehouseId))
        {
            foreach (var item in entity.APuOutStockSoItemList)
            {
                // 商品ID 保存在 F_CustomerId，出库数量在 F_Num
                if (string.IsNullOrEmpty(item.F_CustomerId) || item.F_Num == null) continue;

                var inv = await _repositoryInventory.AsSugarClient()
                    .Queryable<AGoodsInventoryEntity>()
                    .Where(it => it.DeleteMark == null  && it.F_Code == item.F_Code)
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
    /// 更新a_pu_out_stock_so.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] APuOutStockSoUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuOutStockSoEntity>();
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_StockOutNo.Equals(input.F_StockOutNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "销售出库单号");

        if (string.IsNullOrEmpty(entity.F_StockOutNo))
        {
            entity.F_StockOutNo = oldEntity.F_StockOutNo;
        }
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuOutStockSoEntity>(new APuOutStockSoEntity()));
        ConfigModel tableFielde48defConfig = new ConfigModel
        {
            tableName = "a_pu_out_stock_so_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品列表",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutStockSoItemEntity>(new APuOutStockSoItemEntity())
        };
        FieldsModel tableFielde48defModel = new FieldsModel()
        {
            __config__ = tableFielde48defConfig,
            __vModel__ = "tableFielde48def"
        };
        fieldList.Add(tableFielde48defModel);
        ConfigModel tableField67a4d7Config = new ConfigModel
        {
            tableName = "a_pu_out_stock_so_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "操作日志",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutStockSoLogEntity>(new APuOutStockSoLogEntity())
        };
        FieldsModel tableField67a4d7Model = new FieldsModel()
        {
            __config__ = tableField67a4d7Config,
            __vModel__ = "tableField67a4d7"
        };
        fieldList.Add(tableField67a4d7Model);

        // 移除销售出库商品列表可能被删除数据
        List<APuOutStockSoItemEntity> deletedItems = new List<APuOutStockSoItemEntity>();
        if (input.tableFielde48def != null && input.tableFielde48def.Any())
        {
            var existingItemIds = input.tableFielde48def.Where(it => !string.IsNullOrEmpty(it.F_Id)).Select(it => it.F_Id).ToList();
            // 先查询将要被删除的行，用于回滚库存
            deletedItems = await _repository.AsSugarClient().Queryable<APuOutStockSoItemEntity>()
                .Where(it => it.F_StockOutSOId == entity.id && !existingItemIds.Contains(it.F_Id) && it.DeleteMark == null)
                .ToListAsync();
            await _repository.AsSugarClient().Deleteable<APuOutStockSoItemEntity>()
                .Where(it => it.F_StockOutSOId == entity.id && !existingItemIds.Contains(it.F_Id))
                .IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
        else
        {
            // 全部删除的场景，先读取再逻辑删除
            deletedItems = await _repository.AsSugarClient().Queryable<APuOutStockSoItemEntity>()
                .Where(it => it.F_StockOutSOId == entity.id && it.DeleteMark == null)
                .ToListAsync();
            await _repository.AsSugarClient().Deleteable<APuOutStockSoItemEntity>()
                .Where(it => it.F_StockOutSOId == entity.id)
                .IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }

        // 对被删除的子表行回滚库存（将删除的出库数量加回对应仓库库存）
        if (deletedItems != null && deletedItems.Any() && !string.IsNullOrEmpty(entity.F_WarehouseId))
        {
            foreach (var del in deletedItems)
            {
                try
                {
                    if (string.IsNullOrEmpty(del.F_CustomerId) || del.F_Num == null) continue;
                    var inv = await _repositoryInventory.AsSugarClient()
                        .Queryable<AGoodsInventoryEntity>()
                        .Where(it => it.DeleteMark == null  && it.F_Code == del.F_Code)
                        .FirstAsync();
                    if (inv != null)
                    {
                        inv.F_Num = (inv.F_Num ?? 0) + (del.F_Num ?? 0);
                        await _repositoryInventory.AsUpdateable(inv).UpdateColumns(it => new { it.F_Num }).ExecuteCommandAsync();
                    }
                    else
                    {
                        var newInv = new AGoodsInventoryEntity
                        {
                            id = SnowflakeIdHelper.NextId(),
                            F_GoodsId = del.F_CustomerId,
                            F_WarehouseId = entity.F_WarehouseId,
                            F_Code = del.F_Code,
                            F_Num = del.F_Num
                        };
                        await _repositoryInventory.AsInsertable(newInv).ExecuteCommandAsync();
                    }
                }
                catch
                {
                    // 避免影响主流程：库存回滚失败时忽略（可后续记录或告警）
                }
            }
        }

        // 新增销售出库商品列表新数据
        var aPuOutStockSoItemEntityList = input.tableFielde48def.Adapt<List<APuOutStockSoItemEntity>>();


        // 移除销售出库单日志可能被删除数据
        if (input.tableField67a4d7 !=null && input.tableField67a4d7.Any())
            await _repository.AsSugarClient().Deleteable<APuOutStockSoLogEntity>().Where(it => it.F_StockOutSOId == entity.id && !input.tableField67a4d7.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<APuOutStockSoLogEntity>().Where(it => it.F_StockOutSOId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增销售出库单日志新数据
        var aPuOutStockSoLogEntityList = input.tableField67a4d7.Adapt<List<APuOutStockSoLogEntity>>();

        try
        {
            // 获取更新前的数据，用于记录变更日志
            // 本方法内的辅助函数：把字段值（id/code）转换为展示文字
            async Task<string> GetDisplayAsync(string field, string val)
            {
                if (string.IsNullOrEmpty(val)) return null;
                try
                {
                    switch (field)
                    {
                        //case "F_WarehouseId":
                        //    var whList = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2012073572784279552", "F_Id", "F_Name", "");
                        //    return whList.Find(it => it.id.Equals(val))?.fullName;
                        case "F_StockOutType":
                            return await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(d => d.EnCode.Equals(val) && d.DictionaryTypeId.Equals("2013096194263355392")).Select(d => d.FullName).FirstAsync();
                        case "F_StockOutUserId":
                            return (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(val)))?.RealName;
                        case "F_ContactId":
                            {
                                var linkageParameters = new List<DataInterfaceParameter>();
                                linkageParameters.Add(new DataInterfaceParameter
                                {
                                    field = "Id",
                                    relationField = "2009181616060108800",
                                    isSubTable = false,
                                    dataType = "varchar",
                                    defaultValue = "",
                                    fieldName = "",
                                    sourceType = 2
                                });
                                var contactList = await _dataInterfaceService.GetDynamicList("F_ContactId", "2009459664370143232", "F_Id", "F_ContactName", "", linkageParameters);
                                return contactList.Find(it => it.id.Equals(val))?.fullName;
                            }
                        case "F_AddressId":
                            {
                                var linkageParameters = new List<DataInterfaceParameter>();
                                linkageParameters.Add(new DataInterfaceParameter
                                {
                                    field = "id",
                                    relationField = "2009181616060108800",
                                    isSubTable = false,
                                    dataType = "varchar",
                                    defaultValue = "",
                                    fieldName = "",
                                    sourceType = 2
                                });
                                var addrList = await _dataInterfaceService.GetDynamicList("F_AddressId", "2009527238009163776", "F_Id", "F_Address", "", linkageParameters);
                                return addrList.Find(it => it.id.Equals(val))?.fullName;
                            }
                        case "F_CustomerId":
                            var custList = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2009458830353764352", "F_Id", "F_CustomerName", "");
                            return custList.Find(it => it.id.Equals(val))?.fullName;
                        default:
                            return val;
                    }
                }
                catch
                {
                    return val;
                }
            }
            // 更新主表字段
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_StockOutNo,
                    it.F_WarehouseId,
                    it.F_StockOutType,
                    it.F_StockOutDate,
                    it.F_StockOutUserId,
                    it.F_CustomerId,
                    it.F_ContactId,
                    it.F_AddressId,
                    it.F_Description,
                })
                .ExecuteCommandAsync();

            // 比较字段，如果有变化则写入系统变更日志（记录字段名：原值 -> 新值，且对下拉/关联字段记录展示文字）
            if (oldEntity != null)
            {

                var compareFields = new List<(string field, string oldVal, string newVal)>
                {
                    ("F_StockOutNo", oldEntity.F_StockOutNo, entity.F_StockOutNo),
                    ("F_WarehouseId", oldEntity.F_WarehouseId, entity.F_WarehouseId),
                    ("F_StockOutType", oldEntity.F_StockOutType, entity.F_StockOutType),
                    ("F_StockOutDate", oldEntity.F_StockOutDate?.ToString("yyyy-MM-dd HH:mm:ss"), entity.F_StockOutDate?.ToString("yyyy-MM-dd HH:mm:ss")),
                    ("F_StockOutUserId", oldEntity.F_StockOutUserId, entity.F_StockOutUserId),
                    ("F_CustomerId", oldEntity.F_CustomerId, entity.F_CustomerId),
                    ("F_ContactId", oldEntity.F_ContactId, entity.F_ContactId),
                    ("F_AddressId", oldEntity.F_AddressId, entity.F_AddressId),
                    ("F_Description", oldEntity.F_Description, entity.F_Description),
                };

                foreach (var (field, oldVal, newVal) in compareFields)
                {
                    if ((oldVal ?? "") != (newVal ?? ""))
                    {
                        var label = paramList.Find(p => p.field == field)?.value ?? field;
                        var oldDisp = await GetDisplayAsync(field, oldVal);
                        var newDisp = await GetDisplayAsync(field, newVal);
                        var reason = $"{label}：{(string.IsNullOrEmpty(oldDisp) ? "" : oldDisp)} -> {(string.IsNullOrEmpty(newDisp) ? "" : newDisp)}";

                        var sysLog = new APuOutStockSoLogEntity
                        {
                            F_Id = SnowflakeIdHelper.NextId(),
                            F_StockOutSOId = entity.id,
                            F_Type = "系统",
                            F_Title = "更改数据",
                            F_Content = null,
                            F_Reason = reason,
                            F_CreatorUserId = _userManager.UserId,
                            F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                        };
                        await _repository.AsSugarClient().Insertable(sysLog).ExecuteCommandAsync();
                    }
                }
            }

            // 处理子表（商品列表）并同步库存变化
            if (aPuOutStockSoItemEntityList != null)
            {
                foreach (var item in aPuOutStockSoItemEntityList)
                {
                    if (!item.F_Id.IsNullOrEmpty())
                    {
                        var prodItem = await _repositoryItem.GetFirstAsync(it => it.DeleteMark == null && it.F_Id == item.F_Id);
                        prodItem.F_SalesPrice = item.F_SalesPrice;
                        prodItem.F_Type = item.F_Type;
                        prodItem.F_Price = item.F_Price;
                        prodItem.F_Description = item.F_Description;

                        await _repositoryItem.AsUpdateable(prodItem)
                            .UpdateColumns(it => new {
                                it.F_SalesPrice,
                                it.F_Type,
                                it.F_Price,
                                it.F_Description,
                            }).ExecuteCommandAsync();
                    }
                    
                }
            }
            //if (aPuOutStockSoItemEntityList != null)
            //{
            //    foreach (var item in aPuOutStockSoItemEntityList)
            //    {
            //        if (item.F_Id.IsNullOrEmpty())
            //        {
            //            // 新增子表：插入数据并从库存中扣减相应数量
            //            item.F_CreatorUserId = _userManager.UserId;
            //            item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            //            item.F_Id = SnowflakeIdHelper.NextId();
            //            item.F_StockOutSOId = entity.id;
            //            await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();

            //            // 扣减库存
            //            try
            //            {
            //                if (!string.IsNullOrEmpty(entity.F_WarehouseId) && !string.IsNullOrEmpty(item.F_CustomerId) && item.F_Num != null)
            //                {
            //                    var inv = await _repositoryInventory.AsSugarClient()
            //                        .Queryable<AGoodsInventoryEntity>()
            //                        .Where(it => it.DeleteMark == null  && it.F_Code == item.F_Code)
            //                        .FirstAsync();
            //                    if (inv != null)
            //                    {
            //                        var newNum = (inv.F_Num ?? 0) - (item.F_Num ?? 0);
            //                        if (newNum < 0) newNum = 0;
            //                        inv.F_Num = newNum;
            //                        await _repositoryInventory.AsUpdateable(inv).UpdateColumns(it => new { it.F_Num }).ExecuteCommandAsync();
            //                    }
            //                    // 若库存记录不存在，忽略（或可插入一条库存为0的记录），此处不创建负库存记录
            //                }
            //            }
            //            catch
            //            {
            //                // 忽略库存更新错误，避免影响主流程；可考虑记录日志以便排查
            //            }
            //        }
            //        else
            //        {
            //            // 修改子表：比较旧数量与新数量，按差值调整库存（增加或减少）
            //            var oldItem = await _repository.AsSugarClient().Queryable<APuOutStockSoItemEntity>().Where(it => it.F_Id == item.F_Id).FirstAsync();

            //            item.F_CreatorUserId = null;
            //            item.F_CreatorTime = null;
            //            item.F_StockOutSOId = entity.id;
            //            await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

            //            try
            //            {
            //                if (!string.IsNullOrEmpty(entity.F_WarehouseId) && !string.IsNullOrEmpty(item.F_CustomerId))
            //                {
            //                    var oldQty = oldItem?.F_Num ?? 0;
            //                    var newQty = item.F_Num ?? 0;
            //                    var diff = newQty - oldQty; // 正数 => 减库存；负数 => 增库存
            //                    if (diff != 0)
            //                    {
            //                        var inv = await _repositoryInventory.AsSugarClient()
            //                            .Queryable<AGoodsInventoryEntity>()
            //                            .Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId.Contains(entity.F_WarehouseId) && it.F_Code == item.F_Code)
            //                            .FirstAsync();
            //                        if (inv != null)
            //                        {
            //                            var updated = (inv.F_Num ?? 0) - diff;
            //                            if (updated < 0) updated = 0;
            //                            inv.F_Num = updated;
            //                            await _repositoryInventory.AsUpdateable(inv).UpdateColumns(it => new { it.F_Num }).ExecuteCommandAsync();
            //                        }
            //                        else
            //                        {
            //                            // 若库存记录不存在且需要增加库存（diff < 0），则插入一条记录
            //                            if (diff < 0)
            //                            {
            //                                var newInv = new AGoodsInventoryEntity
            //                                {
            //                                    id = SnowflakeIdHelper.NextId(),
            //                                    F_GoodsId = item.F_CustomerId,
            //                                    F_WarehouseId = entity.F_WarehouseId,
            //                                    F_Code = item.F_Code,
            //                                    F_Num = -diff
            //                                };
            //                                await _repositoryInventory.AsInsertable(newInv).ExecuteCommandAsync();
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            catch
            //            {
            //                // 忽略库存更新错误
            //            }
            //        }
            //    }
            //}

            // 处理前端传来的日志记录：确保新增的记录有创建信息，且标题/原因必要时补全或转换下拉为文本
            if (aPuOutStockSoLogEntityList != null)
            {
                foreach (var item in aPuOutStockSoLogEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_StockOutSOId = entity.id;
                        // 如果前端没有设置标题，则视为新增日志
                        if (string.IsNullOrEmpty(item.F_Title)) item.F_Title = "新增数据";
                        // 如果有原因并包含字段名或id，尽可能把常见字段的id换成展示文本
                        if (!string.IsNullOrEmpty(item.F_Reason))
                        {
                            // 简单替换常见 id -> text（尝试解析可能的字段:value 形式）
                            foreach (var fld in new[] { "F_WarehouseId", "F_StockOutType", "F_StockOutUserId", "F_ContactId", "F_AddressId", "F_CustomerId" })
                            {
                                if (item.F_Reason.Contains(fld) && item.F_Reason.Contains("->"))
                                {
                                    try
                                    {
                                        var parts = item.F_Reason.Split("->");
                                        if (parts.Length == 2)
                                        {
                                            var left = parts[0];
                                            var right = parts[1];
                                            // attempt to extract id from right side
                                            var idCandidate = right.Trim();
                                            var disp = await (new Func<Task<string>>(async () => await GetDisplayAsync(fld, idCandidate)))();
                                            if (!string.IsNullOrEmpty(disp)) item.F_Reason = $"{left} -> {disp}";
                                        }
                                    }
                                    catch { /* ignore parse errors */ }
                                }
                            }
                        }
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_StockOutSOId = entity.id;
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
    /// 删除a_pu_out_stock_so.
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
    /// 批量删除a_pu_out_stock_so.
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
            // 在逻辑删除销售出库单之前，恢复对应库存（遍历每张单的商品，按 商品ID 与 仓库ID 增加库存数量）
            try
            {
                var deletedIds = entitys.Select(e => e.id).ToList();
                var items = await _repository.AsSugarClient().Queryable<APuOutStockSoItemEntity>()
                    .Where(it => deletedIds.Contains(it.F_StockOutSOId) && it.DeleteMark == null)
                    .ToListAsync();
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        if (string.IsNullOrEmpty(item.F_CustomerId) || item.F_Num == null) continue;
                        var parent = entitys.Find(e => e.id.Equals(item.F_StockOutSOId));
                        if (parent == null || string.IsNullOrEmpty(parent.F_WarehouseId)) continue;
                        var inv = await _repositoryInventory.AsSugarClient()
                            .Queryable<AGoodsInventoryEntity>()
                            .Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId.Contains(parent.F_WarehouseId) && it.F_Code == item.F_Code)
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
                                F_Code = item.F_Code,
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

            // 批量删除a_pu_out_stock_so
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_pu_out_stock_so详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.APuOutStockSoItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuOutStockSoLogList)
            .Where(it => it.id == id)
            .Select(it => new APuOutStockSoDetailOutput
            {
                id = it.id,
                F_StockOutNo = it.F_StockOutNo,
                F_WarehouseId = it.F_WarehouseId,
                F_StockOutType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_StockOutType) && dic.DictionaryTypeId.Equals("2013096194263355392")).Select(dic => dic.FullName),
                F_StockOutDate = it.F_StockOutDate.Value.ToString("yyyy-MM-dd"),
                F_StockOutUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_StockOutUserId)).Select(u => u.RealName),
                F_CustomerId = it.F_CustomerId,
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableFielde48def = it.APuOutStockSoItemList.Where(item => item.DeleteMark == null).Adapt<List<APuOutStockSoItemDetailOutput>>(),
                tableField67a4d7 = it.APuOutStockSoLogList.Where(log => log.DeleteMark == null).Adapt<List<APuOutStockSoLogDetailOutput>>(),
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

            // 联系人
            if (item.F_ContactId != null)
            {
                linkageParameters = new List<DataInterfaceParameter>();
                linkageParameters.Add(new DataInterfaceParameter
                {
                    field = "Id",
                    relationField = "2009181616060108800",
                    isSubTable = false,
                    dataType = "varchar",
                    defaultValue = "",
                    fieldName = "",
                    sourceType = 2
                });
                var F_ContactIdData = await _dataInterfaceService.GetDynamicList("F_ContactId", "2009459664370143232", "F_Id", "F_ContactName", "", linkageParameters);
                item.F_ContactId = F_ContactIdData.Find(it => it.id.Equals(item.F_ContactId))?.fullName;
            }

            // 地址
            if(item.F_AddressId != null)
            {
                linkageParameters = new List<DataInterfaceParameter>();
                linkageParameters.Add(new DataInterfaceParameter
                {
                    field = "id",
                    relationField = "2009181616060108800",
                    isSubTable = false,
                    dataType = "varchar",
                    defaultValue = "",
                    fieldName = "",
                    sourceType = 2
                });
                var F_AddressIdData = await _dataInterfaceService.GetDynamicList("F_AddressId", "2009527238009163776", "F_Id", "F_Address", "", linkageParameters);
                item.F_AddressId = F_AddressIdData.Find(it => it.id.Equals(item.F_AddressId))?.fullName;
            }

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFielde48def), async aPuOutStockSoItem =>
        {
            var aPuOutStockSo = data.ToList().Find(it => it.id.Equals(aPuOutStockSoItem.F_StockOutSOId));
            var linkageParameters = new List<DataInterfaceParameter>();

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            var goodsUnit =  _repositorygoods.AsQueryable().Where(it => it.id == aPuOutStockSoItem.F_CustomerId).Select(it => it.F_Unit).First();
            if (!string.IsNullOrEmpty(goodsUnit))
            {
                var F_UnitIdCascader = goodsUnit.ToObject<List<string>>();
                if (F_UnitIdCascader.Count > 1)
                {
                    aPuOutStockSoItem.F_NumInfo = "1" + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName + Math.Floor(aPuOutStockSoItem.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    aPuOutStockSoItem.F_NumInfo = Math.Floor(aPuOutStockSoItem.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }
        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField67a4d7), async aPuOutStockSoLog =>
        {
            var aPuOutStockSo = data.ToList().Find(it => it.id.Equals(aPuOutStockSoLog.F_StockOutSOId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuOutStockSoLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOutStockSoLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOutStockSoLog.F_CreatorTime = aPuOutStockSoLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");


        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<APuOutStockSoEntity>(new APuOutStockSoEntity()));
        ConfigModel tableFielde48defConfig = new ConfigModel
        {
            tableName = "a_pu_out_stock_so_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品列表",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<APuOutStockSoItemEntity>(new APuOutStockSoItemEntity())
        };
        FieldsModel tableFielde48def = new FieldsModel()
        {
            __config__ = tableFielde48defConfig,
            __vModel__ = "tableFielde48def"
        };
        fieldList.Add(tableFielde48def);

        resData = await _controlParsing.GetParsDataList(resData,"F_CustomerId,tableFielde48def-F_CustomerId","popupSelect",_userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }


    /// <summary>
    /// 根据客户 id 获取默认联系人与默认地址（返回 id 与展示字段）
    /// </summary>
    /// <param name="customerId">客户主键 F_CustomerId</param>
    /// <returns></returns>
    [HttpGet("LinkData/{customerId}")]
    public async Task<dynamic> GetLinkData(string customerId)
    {
        if (string.IsNullOrEmpty(customerId)) return new { contactId = (string)null, contactName = (string)null, addressId = (string)null, address = (string)null };

        // 查询默认联系人（F_IsDefault == 1）
        var contact = await _repositorylxr.AsSugarClient()
            .Queryable<ACtcContactEntity>()
            .Where(it => it.DeleteMark == null && it.F_CustomerId.Equals(customerId) && it.F_IsDefault == "1")
            .Select(it => new ACtcContactEntity { F_Id = it.F_Id, F_ContactName = it.F_ContactName })
            .FirstAsync();

        // 查询默认地址（F_IsDefault == 1）
        var address = await _repositoryldz.AsSugarClient()
            .Queryable<ACtcAddressEntity>()
            .Where(it => it.DeleteMark == null && it.F_CustomerId.Equals(customerId) && it.F_IsDefault == "1")
            .Select(it => new ACtcAddressEntity { F_Id = it.F_Id, F_Address = it.F_Address })
            .FirstAsync();

        return new {
            contactId = contact?.F_Id,
            contactName = contact?.F_ContactName,
            addressId = address?.F_Id,
            address = address?.F_Address
        };
    }

    /// <summary>
    /// 根据仓库 id 获取库存商品的商品 id 与数量。
    /// </summary>
    /// <param name="warehouseId">仓库主键 F_WarehouseId</param>
    [HttpGet("InventoryByWarehouse/{warehouseId}")]
    public async Task<dynamic> GetInventoryByWarehouse(string warehouseId)
    {
        if (string.IsNullOrEmpty(warehouseId)) return new List<object>();

        // 关联商品表，返回库存以及商品展示字段（只返回能在商品表中匹配到的记录）
        var data = await _repositoryInventory.AsSugarClient()
            .Queryable<AGoodsInventoryEntity, AGoodsEntity>((inv, g) => new object[] { JoinType.Inner, inv.F_GoodsId == g.id })
            .Where((inv, g) => inv.DeleteMark == null && inv.F_WarehouseId.Equals(warehouseId) && g.DeleteMark == null)
            .Select((inv, g) => new
            {
                F_GoodsId = inv.F_GoodsId,
                F_Num = inv.F_Num,
                F_GoodsNo = g.F_GoodsNo,
                F_GoodsName = g.F_GoodsName,
                F_Unit = g.F_Unit,
                F_SalePrice = g.F_SalePrice,
                F_Source = g.F_Source,
                F_Specification = g.F_Specification
            })
            .ToListAsync();

        return data;
    }



}
