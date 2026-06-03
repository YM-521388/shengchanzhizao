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
using JNPF.example.Entitys.Dto.APuOutReturnPu;
using JNPF.example.Entitys.Dto.APuOutReturnPuItem;
using JNPF.example.Entitys.Dto.APuOutReturnPuLog;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_pu_out_return_pu.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuOutReturnPu", Order = 200)]
[Route("api/example/[controller]")]
public class APuOutReturnPuService : IAPuOutReturnPuService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuOutReturnPuEntity> _repository;
    private readonly ISqlSugarRepository<APuOutReturnPuItemEntity> _repositoryItem;
    private readonly ISqlSugarRepository<APuOrderItemEntity> _repositoryder;
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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"采购退货单号\",\"field\":\"F_ReturnOutNo\"},{\"value\":\"仓库\",\"field\":\"F_WarehouseId\"},{\"value\":\"出库类型\",\"field\":\"F_ReturnOutType\"},{\"value\":\"采购单号\",\"field\":\"F_OrderId\"},{\"value\":\"退货日期\",\"field\":\"F_ReturnOutDate\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="APuOutReturnPuService"/>类型的新实例.
    /// </summary>
    public APuOutReturnPuService(
        ISqlSugarRepository<APuOutReturnPuItemEntity> repositoryItem,
        ISqlSugarRepository<AGoodsEntity> repositorygoods,
        ISqlSugarRepository<APuOutReturnPuEntity> repository,
        ISqlSugarRepository<APuOrderItemEntity> repositoryder,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryItem = repositoryItem;
        _repositorygoods = repositorygoods;
        _repository = repository;
        _repositoryder = repositoryder;
        _repositoryInventory = repositoryInventory;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_pu_out_return_pu.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.APuOutReturnPuItemList)
            .Includes(it => it.APuOutReturnPuLogList)
            .Where(it => it.DeleteMark == null)
            .Select(it => new APuOutReturnPuEntity
            {
                id = it.id,
                F_ReturnOutNo = it.F_ReturnOutNo,
                F_WarehouseId = it.F_WarehouseId,
                F_ReturnOutType = it.F_ReturnOutType,
                F_ReturnOutDate = it.F_ReturnOutDate,
                F_OrderId = it.F_OrderId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                APuOutReturnPuItemList = it.APuOutReturnPuItemList.Where(item => item.DeleteMark == null).ToList(),
                APuOutReturnPuLogList = it.APuOutReturnPuLogList.Where(log => log.DeleteMark == null).ToList(),
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuOutReturnPuInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFieldf5c8ef, async aPuOutReturnPuItem =>
        {
            // 创建人员
            aPuOutReturnPuItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOutReturnPuItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOutReturnPuItem.F_CreatorTime = aPuOutReturnPuItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            aPuOutReturnPuItem.F_WarehouseId = "";
        });
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField3ff267, async aPuOutReturnPuLog =>
        {
            // 创建人员
            aPuOutReturnPuLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOutReturnPuLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOutReturnPuLog.F_CreatorTime = aPuOutReturnPuLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

    /// <summary>
    /// 获取a_pu_out_return_pu列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuOutReturnPuListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuOutReturnPuItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuOutReturnPuLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuOutReturnPuListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_out_return_pu"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_out_return_pu_item"))) aPuOutReturnPuItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_out_return_pu_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_out_return_pu_log"))) aPuOutReturnPuLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_out_return_pu_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutReturnPuEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutReturnPuItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldf5c8ef-", entityInfo, 1);
        List<IConditionalModel> aPuOutReturnPuItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuOutReturnPuItemConditionalModel.AddRange(aPuOutReturnPuItemAuthorizeWhere);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutReturnPuLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField3ff267-", entityInfo, 1);
        List<IConditionalModel> aPuOutReturnPuLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuOutReturnPuLogConditionalModel.AddRange(aPuOutReturnPuLogAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOutReturnPuEntity>();
        var F_WarehouseId = input.F_WarehouseId?.Last();
        var F_ReturnOutTypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_ReturnOutType").DbColumnName;

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ReturnOutNo), it => it.F_ReturnOutNo.Contains(input.F_ReturnOutNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()), it => it.F_WarehouseId.Contains(F_WarehouseId))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_ReturnOutTypeDbColumnName, input.F_ReturnOutType))
            .WhereIF(input.F_ReturnOutDate?.Count() > 0, it => SqlFunc.Between(it.F_ReturnOutDate, input.F_ReturnOutDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_ReturnOutDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .WhereIF(aPuOutReturnPuItemAuthorizeWhere?.Count > 0, it => it.APuOutReturnPuItemList.Any(aPuOutReturnPuItemAuthorizeWhere))
            .WhereIF(aPuOutReturnPuLogAuthorizeWhere?.Count > 0, it => it.APuOutReturnPuLogList.Any(aPuOutReturnPuLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aPuOutReturnPuItemConditionalModel.Count > 0, it => it.APuOutReturnPuItemList.Any(aPuOutReturnPuItemConditionalModel))
            .WhereIF(aPuOutReturnPuLogConditionalModel.Count > 0, it => it.APuOutReturnPuLogList.Any(aPuOutReturnPuLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new APuOutReturnPuListOutput
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
             .Select(it => new APuOutReturnPuListOutput
             {
                 id = it.id,
                 F_ReturnOutNo = it.F_ReturnOutNo,
                 F_WarehouseId = it.F_WarehouseId,
                 F_ReturnOutType = it.F_ReturnOutType,
                 F_ReturnOutDate = it.F_ReturnOutDate.Value.ToString("yyyy-MM-dd"),
                 F_OrderId = it.F_OrderId,
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
            if (item.F_ReturnOutType != null)
            {
                item.F_ReturnOutType = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(item.F_ReturnOutType) && it.DictionaryTypeId.Equals("2013096194263355392")).Select(it => it.FullName).FirstAsync();
            }
        });

        data.pagination = idsQ.pagination;

        return PageResult<APuOutReturnPuListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_pu_out_return_pu.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] APuOutReturnPuCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuOutReturnPuEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuOutReturnPuEntity>(new APuOutReturnPuEntity()));
        ConfigModel tableFieldf5c8efConfig = new ConfigModel
        {
            tableName = "a_pu_out_return_pu_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutReturnPuItemEntity>(new APuOutReturnPuItemEntity())
        };
        FieldsModel tableFieldf5c8efModel = new FieldsModel()
        {
            __config__ = tableFieldf5c8efConfig,
            __vModel__ = "tableFieldf5c8ef"
        };
        fieldList.Add(tableFieldf5c8efModel);
        ConfigModel tableField3ff267Config = new ConfigModel
        {
            tableName = "a_pu_out_return_pu_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "设计子表",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutReturnPuLogEntity>(new APuOutReturnPuLogEntity())
        };
        FieldsModel tableField3ff267Model = new FieldsModel()
        {
            __config__ = tableField3ff267Config,
            __vModel__ = "tableField3ff267"
        };
        fieldList.Add(tableField3ff267Model);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;


        var aPuOutReturnPuItemEntityList = input.tableFieldf5c8ef.Adapt<List<APuOutReturnPuItemEntity>>();
        if(aPuOutReturnPuItemEntityList != null)
        {
            foreach (var item in aPuOutReturnPuItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuOutReturnPuItemList = aPuOutReturnPuItemEntityList;
        }

        var aPuOutReturnPuLogEntityList = input.tableField3ff267.Adapt<List<APuOutReturnPuLogEntity>>();
        if(aPuOutReturnPuLogEntityList != null)
        {
            foreach (var item in aPuOutReturnPuLogEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuOutReturnPuLogList = aPuOutReturnPuLogEntityList;
        }

        // 如果前端没有传入 F_ReturnOutNo，则自动生成，规则：CGTH + yyyyMMdd + 当天序号(3位)
        if (entity.F_ReturnOutNo.IsNullOrEmpty())
        {
            var datePrefix = DateTime.Now.ToString("yyyyMMdd");
            var prefix = $"CGTH{datePrefix}";
            // 统计当天已存在的同前缀单号数量，作为序号来源
            var todayCount = await _repository.AsQueryable()
                .Where(it => it.F_ReturnOutNo != null && it.F_ReturnOutNo.StartsWith(prefix) && it.DeleteMark == null)
                .CountAsync();
            var seq = todayCount + 1;
            entity.F_ReturnOutNo = $"{prefix}{seq.ToString("D3")}";
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.APuOutReturnPuItemList)
            .Include(it => it.APuOutReturnPuLogList)
            .ExecuteCommandAsync();

        // 减库存：遍历新增的商品列表，按 子表的仓库ID(F_WarehouseID)、库存编码(F_Encoding)、商品ID(F_CustomerId) 在库存表中匹配并减少数量
        if (entity.APuOutReturnPuItemList != null && entity.APuOutReturnPuItemList.Any())
        {
            foreach (var item in entity.APuOutReturnPuItemList)
            {
                // 子表仓库ID 在 F_WarehouseID，库存编码在 F_Encoding，商品ID在 F_CustomerId，退货数量在 F_Num
                if (string.IsNullOrEmpty(item.F_WarehouseID) || string.IsNullOrEmpty(item.F_Encoding) || string.IsNullOrEmpty(item.F_CustomerId) || item.F_Num == null) continue;

                var inv = await _repositoryInventory.AsSugarClient()
                    .Queryable<AGoodsInventoryEntity>()
                    .Where(it => it.DeleteMark == null && it.F_Code == item.F_Encoding)
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
    /// 更新a_pu_out_return_pu.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] APuOutReturnPuUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuOutReturnPuEntity>();
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_ReturnOutNo.Equals(input.F_ReturnOutNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "采购退货单号");

        if (string.IsNullOrEmpty(entity.F_ReturnOutNo))
        {
            entity.F_ReturnOutNo = oldEntity.F_ReturnOutNo;
        }
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuOutReturnPuEntity>(new APuOutReturnPuEntity()));
        ConfigModel tableFieldf5c8efConfig = new ConfigModel
        {
            tableName = "a_pu_out_return_pu_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutReturnPuItemEntity>(new APuOutReturnPuItemEntity())
        };
        FieldsModel tableFieldf5c8efModel = new FieldsModel()
        {
            __config__ = tableFieldf5c8efConfig,
            __vModel__ = "tableFieldf5c8ef"
        };
        fieldList.Add(tableFieldf5c8efModel);
        ConfigModel tableField3ff267Config = new ConfigModel
        {
            tableName = "a_pu_out_return_pu_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "系统日志表",
            children = ExportImportDataHelper.GetTemplateParsing<APuOutReturnPuLogEntity>(new APuOutReturnPuLogEntity())
        };
        FieldsModel tableField3ff267Model = new FieldsModel()
        {
            __config__ = tableField3ff267Config,
            __vModel__ = "tableField3ff267"
        };
        fieldList.Add(tableField3ff267Model);

        // 移除采购退货单商品列表可能被删除数据
        if (input.tableFieldf5c8ef !=null && input.tableFieldf5c8ef.Any())
            await _repository.AsSugarClient().Deleteable<APuOutReturnPuItemEntity>().Where(it => it.F_ReturnOutPUId == entity.id && !input.tableFieldf5c8ef.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<APuOutReturnPuItemEntity>().Where(it => it.F_ReturnOutPUId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增采购退货单商品列表新数据
        var aPuOutReturnPuItemEntityList = input.tableFieldf5c8ef.Adapt<List<APuOutReturnPuItemEntity>>();


        // 移除采购退货单日志可能被删除数据
        if (input.tableField3ff267 !=null && input.tableField3ff267.Any())
            await _repository.AsSugarClient().Deleteable<APuOutReturnPuLogEntity>().Where(it => it.F_ReturnOutPUId == entity.id && !input.tableField3ff267.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<APuOutReturnPuLogEntity>().Where(it => it.F_ReturnOutPUId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增采购退货单日志新数据
        var aPuOutReturnPuLogEntityList = input.tableField3ff267.Adapt<List<APuOutReturnPuLogEntity>>();
        
        var autoChangeList = new List<string>();
        if (oldEntity != null)
        {
            var props = typeof(APuOutReturnPuEntity).GetProperties();
            foreach (var p in props)
            {
                var propName = p.Name;
                // 过滤系统审计字段，不记录到变更日志
                var skipProps = new[] { "F_CreatorUserId", "F_CreatorTime", "F_LastModifyUserId", "F_LastModifyTime", "TenantId", "F_TenantId", "DeleteMark" };
                if (skipProps.Contains(propName)) continue;
                if (propName.Equals("id", StringComparison.OrdinalIgnoreCase)) continue;
                var oldValObj = oldEntity.GetType().GetProperty(propName)?.GetValue(oldEntity);
                var newValObj = entity.GetType().GetProperty(propName)?.GetValue(entity);
                var oldVal = oldValObj == null ? string.Empty : oldValObj.ToString();
                var newVal = newValObj == null ? string.Empty : newValObj.ToString();
                if (oldVal == newVal) continue;
                // 使用可读字段名（来自 paramList）作为标题，如果找不到则退回到属性名
                var label = paramList.Find(m => m.field == propName)?.value ?? propName;
                if (propName.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
                {
                    var displayPropName = propName.Replace("Id", "Name");
                    var displayProp = entity.GetType().GetProperty(displayPropName);
                    if (displayProp != null)
                    {
                        var oldDisplay = oldEntity.GetType().GetProperty(displayPropName)?.GetValue(oldEntity)?.ToString() ?? string.Empty;
                        var newDisplay = displayProp.GetValue(entity)?.ToString() ?? string.Empty;
                        autoChangeList.Add($"{label}：{oldDisplay} 改为 {newDisplay}");
                    }
                    else
                    {
                        // 如果没有对应的显示字段，记录旧值与新值的文本（但避免写原始 Id）
                        if (!string.IsNullOrEmpty(oldVal) || !string.IsNullOrEmpty(newVal))
                        {
                            autoChangeList.Add($"{label}：{oldVal} 改为 {newVal}");
                        }
                    }
                }
                else
                {
                    autoChangeList.Add($"{label}：{oldVal} 改为 {newVal}");
                }
            }
        }

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_ReturnOutNo,
                    it.F_WarehouseId,
                    it.F_ReturnOutType,
                    it.F_ReturnOutDate,
                    it.F_OrderId,
                    it.F_Description,
                })
                .ExecuteCommandAsync();
            if (aPuOutReturnPuItemEntityList != null)
            {
                foreach (var item in aPuOutReturnPuItemEntityList)
                {
                    if (!item.F_Id.IsNullOrEmpty())
                    {
                        var prodItem = await _repositoryItem.GetFirstAsync(it => it.DeleteMark == null && it.F_Id == item.F_Id);
                        prodItem.F_Description = item.F_Description;

                        await _repositoryItem.AsUpdateable(prodItem)
                            .UpdateColumns(it => new {
                                it.F_Description,
                            }).ExecuteCommandAsync();
                    }
                }
            }

            //if(aPuOutReturnPuItemEntityList != null)
            //{
            //    foreach (var item in aPuOutReturnPuItemEntityList)
            //    {
            //        if(item.F_Id.IsNullOrEmpty()){
            //            // 新增子表行时，扣减库存
            //            item.F_CreatorUserId = _userManager.UserId;
            //            item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            //            item.F_Id = SnowflakeIdHelper.NextId();
            //            item.F_ReturnOutPUId = entity.id;
            //            await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();

            //            // 新增商品行时扣减库存（与 Create 方法逻辑一致）
            //            if (!string.IsNullOrEmpty(item.F_WarehouseID) && !string.IsNullOrEmpty(item.F_Encoding) && !string.IsNullOrEmpty(item.F_CustomerId) && item.F_Num != null)
            //            {
            //                var inv = await _repositoryInventory.AsSugarClient()
            //                    .Queryable<AGoodsInventoryEntity>()
            //                    .Where(it => it.DeleteMark == null && it.F_Code == item.F_Encoding )
            //                    .FirstAsync();
            //                if (inv != null)
            //                {
            //                    var newNum = (inv.F_Num ?? 0) - (item.F_Num ?? 0);
            //                    if (newNum < 0) newNum = 0;
            //                    inv.F_Num = newNum;
            //                    await _repositoryInventory.AsUpdateable(inv).UpdateColumns(it => new { it.F_Num }).ExecuteCommandAsync();
            //                }
            //            }
            //        }else{
            //            // 如果是修改子表：先读取旧记录，再更新，并按数量差值调整库存（增加或减少）
            //            var oldItem = await _repository.AsSugarClient().Queryable<APuOutReturnPuItemEntity>().Where(it => it.F_Id == item.F_Id).FirstAsync();

            //            item.F_CreatorUserId = null;
            //            item.F_CreatorTime = null;
            //            item.F_ReturnOutPUId = entity.id;
            //            await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

            //            try
            //            {
            //                if (!string.IsNullOrEmpty(item.F_WarehouseID) && !string.IsNullOrEmpty(item.F_Encoding) && !string.IsNullOrEmpty(item.F_CustomerId))
            //                {
            //                    var oldQty = oldItem?.F_Num ?? 0;
            //                    var newQty = item.F_Num ?? 0;
            //                    var diff = newQty - oldQty; // 正数 => 减库存；负数 => 增库存
            //                    if (diff != 0)
            //                    {
            //                        var inv = await _repositoryInventory.AsSugarClient()
            //                            .Queryable<AGoodsInventoryEntity>()
            //                            .Where(it => it.DeleteMark == null && it.F_Code == item.F_Encoding)
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
            //                                    F_Code = item.F_Encoding,
            //                                    F_WarehouseId = item.F_WarehouseID,
            //                                    F_GoodsId = item.F_CustomerId,
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
            //                // 忽略库存更新错误，避免影响主流程；可记录或告警以便排查
            //            }
            //        }
            //    }
            //}

            if (aPuOutReturnPuLogEntityList != null)
            {
                foreach (var item in aPuOutReturnPuLogEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_ReturnOutPUId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_ReturnOutPUId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }

            // 如果存在自动变更内容，插入一条日志记录（标题记录为“修改”，内容记录字段变更）
            if (autoChangeList != null && autoChangeList.Count > 0)
            {
                var autoLog = new APuOutReturnPuLogEntity();
                autoLog.F_Id = SnowflakeIdHelper.NextId();
                autoLog.F_ReturnOutPUId = entity.id;
                autoLog.F_CreatorUserId = _userManager.UserId;
                autoLog.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                autoLog.F_Title = "修改";
                autoLog.F_Content = string.Join("；", autoChangeList);
                await _repository.AsSugarClient().Insertable(autoLog).ExecuteCommandAsync();
            }

        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 删除a_pu_out_return_pu.
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
    /// 批量删除a_pu_out_return_pu.
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
            // 在逻辑删除采购退货单之前，恢复对应库存（遍历每张单的商品，按 子表的仓库ID(F_WarehouseID)、库存编码(F_Encoding)、商品ID(F_CustomerId) 增加数量）
            try
            {
                var deletedIds = entitys.Select(e => e.id).ToList();
                var items = await _repository.AsSugarClient().Queryable<APuOutReturnPuItemEntity>()
                    .Where(it => deletedIds.Contains(it.F_ReturnOutPUId) && it.DeleteMark == null)
                    .ToListAsync();
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        // 条件检查：仓库ID、库存编码、商品ID、退货数量都必须有值
                        if (string.IsNullOrEmpty(item.F_WarehouseID) || string.IsNullOrEmpty(item.F_Encoding) || string.IsNullOrEmpty(item.F_CustomerId) || item.F_Num == null) continue;
                        var inv = await _repositoryInventory.AsSugarClient()
                            .Queryable<AGoodsInventoryEntity>()
                            .Where(it => it.DeleteMark == null && it.F_Code == item.F_Encoding )
                            .FirstAsync();
                        if (inv != null)
                        {
                            // 找到库存记录，增加数量（与 Create 方法的扣减逻辑对应）
                            inv.F_Num = (inv.F_Num ?? 0) + (item.F_Num ?? 0);
                            await _repositoryInventory.AsUpdateable(inv).UpdateColumns(it => new { it.F_Num }).ExecuteCommandAsync();
                        }
                        // 找不到库存记录时跳过，不新增（与 Create 方法逻辑一致）
                    }
                }
            }
            catch
            {
                // 恢复库存失败时不阻塞后续删除操作，必要时可记录日志
            }

            // 批量删除a_pu_out_return_pu
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_pu_out_return_pu详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.APuOutReturnPuItemList) // 在这里过滤
            .Includes(it => it.APuOutReturnPuLogList)   // 在这里过滤
            .Where(it => it.DeleteMark == null)
            .Where(it => it.id == id)
            .Select(it => new APuOutReturnPuDetailOutput
            {
                id = it.id,
                F_ReturnOutNo = it.F_ReturnOutNo,
                F_WarehouseId = it.F_WarehouseId,
                F_ReturnOutType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_ReturnOutType) && dic.DictionaryTypeId.Equals("2013096194263355392")).Select(dic => dic.FullName),
                F_ReturnOutDate = it.F_ReturnOutDate.Value.ToString("yyyy-MM-dd"),
                F_OrderId = it.F_OrderId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),

                // 直接赋值，不再写 Where + Adapt
                tableFieldf5c8ef = it.APuOutReturnPuItemList.Adapt<List<APuOutReturnPuItemDetailOutput>>(),
                tableField3ff267 = it.APuOutReturnPuLogList.Adapt<List<APuOutReturnPuLogDetailOutput>>(),
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

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFieldf5c8ef), async aPuOutReturnPuItem =>
        {
            var aPuOutReturnPu = data.ToList().Find(it => it.id.Equals(aPuOutReturnPuItem.F_ReturnOutPUId));
            var linkageParameters = new List<DataInterfaceParameter>();

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            var goodsUnit = _repositorygoods.AsQueryable().Where(it => it.id == aPuOutReturnPuItem.F_CustomerId).Select(it => it.F_Unit).First();
            if (!string.IsNullOrEmpty(goodsUnit))
            {
                var F_UnitIdCascader = goodsUnit.ToObject<List<string>>();
                if (F_UnitIdCascader.Count > 1)
                {
                    aPuOutReturnPuItem.F_NumInfo = "1" + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName + Math.Floor(aPuOutReturnPuItem.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    aPuOutReturnPuItem.F_NumInfo = Math.Floor(aPuOutReturnPuItem.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }
        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField3ff267), async aPuOutReturnPuLog =>
        {
            var aPuOutReturnPu = data.ToList().Find(it => it.id.Equals(aPuOutReturnPuLog.F_ReturnOutPUId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuOutReturnPuLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOutReturnPuLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOutReturnPuLog.F_CreatorTime = aPuOutReturnPuLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<APuOutReturnPuEntity>(new APuOutReturnPuEntity()));
        ConfigModel tableFieldf5c8efConfig = new ConfigModel
        {
            tableName = "a_pu_out_return_pu_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<APuOutReturnPuItemEntity>(new APuOutReturnPuItemEntity())
        };
        FieldsModel tableFieldf5c8ef = new FieldsModel()
        {
            __config__ = tableFieldf5c8efConfig,
            __vModel__ = "tableFieldf5c8ef"
        };
        fieldList.Add(tableFieldf5c8ef);

        resData = await _controlParsing.GetParsDataList(resData,"F_OrderId,tableFieldf5c8ef-F_CustomerId","popupSelect",_userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }




    /// <summary>
    /// 根据采购单 Id 获取对应的订单子项，返回 F_CustomerId 和 F_Num（排除软删除）
    /// </summary>
    /// <param name="orderId">采购单 Id</param>
    /// <returns></returns>
    [HttpGet("OrderItems/{orderId}")]
    public async Task<dynamic> GetOrderItemsByOrderId(string orderId)
    {
        if (string.IsNullOrEmpty(orderId)) return new List<object>();
        var items = await _repositoryder.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_OrderId == orderId)
            .Select(it => new
            {
                it.F_CustomerId,
                it.F_Num
            })
            .ToListAsync();
        return items;
    }



}
