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
using JNPF.example.Entitys.Dto.APuOrder;
using JNPF.example.Entitys.Dto.APuOrderItem;
using JNPF.example.Entitys.Dto.APuOrderLog;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
using System.Text.Json;
using JNPF.Message.Entitys;

namespace JNPF.example;

/// <summary>
/// 业务实现：a_pu_order.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuOrder", Order = 200)]
[Route("api/example/[controller]")]
public class APuOrderService : IAPuOrderService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuOrderEntity> _repository;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositorysp;
    private readonly ISqlSugarRepository<AProdPlanItemEntity> _repositorycgjh;
    private readonly ISqlSugarRepository<APuPlanItemEntity> _repositoryPuPlanItem;
    private readonly ISqlSugarRepository<MessageEntity> _repositoryMessage;
    private readonly ISqlSugarRepository<UserEntity> _repositoryUser;

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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"采购单号\",\"field\":\"F_OrderNo\"},{\"value\":\"供应商\",\"field\":\"F_SupplierId\"},{\"value\":\"商品金额\",\"field\":\"F_Money\"},{\"value\":\"采购数量\",\"field\":\"F_PurchaseNum\"},{\"value\":\"生产计划\",\"field\":\"F_ProdPlanId\"},{\"value\":\"采购人\",\"field\":\"F_UserId\"},{\"value\":\"交货日期\",\"field\":\"F_DeliveryDate\"},{\"value\":\"审核状态\",\"field\":\"F_CheckState\"},{\"value\":\"审核人员\",\"field\":\"F_CheckUserId\"},{\"value\":\"审核时间\",\"field\":\"F_CheckTime\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="APuOrderService"/>类型的新实例.
    /// </summary>
    public APuOrderService(
        ISqlSugarRepository<UserEntity> repositoryUser,
        ISqlSugarRepository<MessageEntity> repositoryMessage,
        ISqlSugarRepository<APuPlanItemEntity> repositoryPuPlanItem,
        ISqlSugarRepository<AGoodsEntity> repositorysp,
        ISqlSugarRepository<APuOrderEntity> repository,
        ISqlSugarRepository<AProdPlanItemEntity> repositorycgjh,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryUser = repositoryUser;
        _repositoryMessage = repositoryMessage;
        _repositoryPuPlanItem = repositoryPuPlanItem;
        _repositorysp = repositorysp;
        _repository = repository;
        _repositorycgjh = repositorycgjh;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_pu_order.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.APuOrderItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuOrderLogList)
            .Select(it => new APuOrderEntity
            {
                id = it.id,
                F_OrderNo = it.F_OrderNo,
                F_SupplierId = it.F_SupplierId,
                F_ProdPlanId = it.F_ProdPlanId,
                F_Money = it.F_Money,
                F_UserId = it.F_UserId,
                F_DeliveryDate = it.F_DeliveryDate,
                F_PurchaseNum = it.F_PurchaseNum,
                F_Description = it.F_Description,
                F_CheckState = it.F_CheckState,
                F_CheckUserId = it.F_CheckUserId,
                F_CheckTime = it.F_CheckTime,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                APuOrderItemList = it.APuOrderItemList,
                APuOrderLogList = it.APuOrderLogList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuOrderInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFieldf01abd, async aPuOrderItem =>
        {
            // 创建人员
            aPuOrderItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOrderItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOrderItem.F_CreatorTime = aPuOrderItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField8b2a57, async aPuOrderLog =>
        {
            // 创建人员
            aPuOrderLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOrderLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOrderLog.F_CreatorTime = aPuOrderLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

  
    /// <summary>
    /// 获取a_pu_order列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuOrderListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuOrderItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuOrderLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuOrderListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_order"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_order_item"))) aPuOrderItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_order_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_order_log"))) aPuOrderLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_order_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOrderEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOrderItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldf01abd-", entityInfo, 1);
        List<IConditionalModel> aPuOrderItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuOrderItemConditionalModel.AddRange(aPuOrderItemAuthorizeWhere);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOrderLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField8b2a57-", entityInfo, 1);
        List<IConditionalModel> aPuOrderLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuOrderLogConditionalModel.AddRange(aPuOrderLogAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOrderEntity>();
        var F_SupplierIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_SupplierId").DbColumnName;
        var F_UserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_UserId").DbColumnName;

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_OrderNo), it => it.F_OrderNo.Contains(input.F_OrderNo))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_SupplierIdDbColumnName, input.F_SupplierId))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_UserIdDbColumnName, input.F_UserId))
            .WhereIF(input.F_DeliveryDate?.Count() > 0, it => SqlFunc.Between(it.F_DeliveryDate, input.F_DeliveryDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_DeliveryDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .WhereIF(!string.IsNullOrEmpty(input.F_CheckState), it => it.F_CheckState.Equals(input.F_CheckState))
            .Where(authorizeWhere)
            .WhereIF(aPuOrderItemAuthorizeWhere?.Count > 0, it => it.APuOrderItemList.Any(aPuOrderItemAuthorizeWhere))
            .WhereIF(aPuOrderLogAuthorizeWhere?.Count > 0, it => it.APuOrderLogList.Any(aPuOrderLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aPuOrderItemConditionalModel.Count > 0, it => it.APuOrderItemList.Any(aPuOrderItemConditionalModel))
            .WhereIF(aPuOrderLogConditionalModel.Count > 0, it => it.APuOrderLogList.Any(aPuOrderLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new APuOrderListOutput
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
            .Select(it => new APuOrderListOutput
            {
                id = it.id,
                F_OrderNo = it.F_OrderNo,
                F_SupplierId = SqlFunc.Subqueryable<ASupplierEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_SupplierId)).Select(u => u.F_SupplierName),
                F_ProdPlanId = SqlFunc.Subqueryable<APuPlanEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_ProdPlanId)).Select(u => u.F_PlanName),
                F_Money = it.F_Money,
                F_CheckState = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_CheckState) && dic.DictionaryTypeId.Equals("2043513064111869952")).Select(dic => dic.FullName),
                F_CheckStateInfo = it.F_CheckState,
                F_CheckUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CheckUserId)).Select(u => u.RealName),
                F_CheckTime = it.F_CheckTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_UserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_UserId)).Select(u => u.RealName),
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_PurchaseNum = it.F_PurchaseNum.ToString(),
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
        });

        data.pagination = idsQ.pagination;

        return PageResult<APuOrderListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_pu_order.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] APuOrderCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuOrderEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuOrderEntity>(new APuOrderEntity()));
        ConfigModel tableFieldf01abdConfig = new ConfigModel
        {
            tableName = "a_pu_order_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "采购单商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuOrderItemEntity>(new APuOrderItemEntity())
        };
        FieldsModel tableFieldf01abdModel = new FieldsModel()
        {
            __config__ = tableFieldf01abdConfig,
            __vModel__ = "tableFieldf01abd"
        };
        fieldList.Add(tableFieldf01abdModel);

        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        entity.F_CheckState = "0";

        // 如果前端未传入订单号，则自动生成
        if (string.IsNullOrEmpty(entity.F_OrderNo))
        {
            var today = DateTime.Now.ToString("yyyyMMdd");
            var todayPrefix = $"CG{today}";

            // 查询今天已存在的订单数量
            var todayCount = await _repository.AsQueryable()
                .Where(it => it.F_OrderNo.StartsWith(todayPrefix) && it.DeleteMark == null)
                .CountAsync();

            // 生成新的订单号：CG + 日期 + 序号（3位，不足补0）
            var sequenceNumber = (todayCount + 1).ToString("D3");
            entity.F_OrderNo = $"{todayPrefix}{sequenceNumber}";
        }

        var aPuOrderItemEntityList = input.tableFieldf01abd.Adapt<List<APuOrderItemEntity>>();
        if(aPuOrderItemEntityList != null)
        {
            foreach (var item in aPuOrderItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuOrderItemList = aPuOrderItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.APuOrderItemList)
            .ExecuteCommandAsync();

        var userEntity = (await _repositoryUser.AsQueryable().Where(it => it.Id == _userManager.UserId && it.DeleteMark == null).Select(it => it.RealName).ToListAsync()).FirstOrDefault();

        var userList = await _repositoryUser.AsQueryable().Where(it => it.OrganizeId.Contains("2020769494137442304") && it.DeleteMark == null).Select(it => it.Id).ToListAsync();
        //给总经办发送站内信
        foreach (var item in userList)
        {
            var messageSystemEntity = new MessageEntity();
            messageSystemEntity.Id = SnowflakeIdHelper.NextId();
            messageSystemEntity.Title = "新增采购单-" + entity.F_OrderNo;
            messageSystemEntity.UserId = item;
            messageSystemEntity.Type = 3;
            messageSystemEntity.FlowType = 1;
            messageSystemEntity.IsRead = 0;
            messageSystemEntity.SortCode = 0;
            messageSystemEntity.CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            messageSystemEntity.TenantId = "0";
            messageSystemEntity.BodyText = userEntity + "-新增采购单" + entity.F_OrderNo + ",请前往仓储/采购-采购单审批！";
            await _repositoryMessage.AsInsertable(messageSystemEntity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 获取a_pu_order无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(APuOrderListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuOrderItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuOrderLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuOrderListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_order"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_order_item"))) aPuOrderItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_order_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_order_log"))) aPuOrderLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_order_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOrderEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOrderItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldf01abd-", entityInfo, 1);
        List<IConditionalModel> aPuOrderItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOrderLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField8b2a57-", entityInfo, 1);
        List<IConditionalModel> aPuOrderLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);


        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOrderEntity>();
        var F_SupplierIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_SupplierId").DbColumnName;
        var F_UserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_UserId").DbColumnName;

        var list = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_OrderNo), it => it.F_OrderNo.Contains(input.F_OrderNo))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_SupplierIdDbColumnName, input.F_SupplierId))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_UserIdDbColumnName, input.F_UserId))
            .WhereIF(input.F_DeliveryDate?.Count() > 0, it => SqlFunc.Between(it.F_DeliveryDate, input.F_DeliveryDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_DeliveryDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .WhereIF(!string.IsNullOrEmpty(input.F_CheckState), it => it.F_CheckState.Equals(input.F_CheckState))
            .Where(authorizeWhere)
            .WhereIF(aPuOrderItemAuthorizeWhere?.Count > 0, it => it.APuOrderItemList.Any(aPuOrderItemAuthorizeWhere))
            .WhereIF(aPuOrderLogAuthorizeWhere?.Count > 0, it => it.APuOrderLogList.Any(aPuOrderLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aPuOrderItemConditionalModel.Count > 0, it => it.APuOrderItemList.Any(aPuOrderItemConditionalModel))
            .WhereIF(aPuOrderLogConditionalModel.Count > 0, it => it.APuOrderLogList.Any(aPuOrderLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new APuOrderListOutput
            {
                id = it.id,
                F_OrderNo = it.F_OrderNo,
                F_SupplierId = SqlFunc.Subqueryable<ASupplierEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_SupplierId)).Select(u => u.F_SupplierName),
                F_ProdPlanId = SqlFunc.Subqueryable<APuPlanEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_ProdPlanId)).Select(u => u.F_PlanName),
                F_Money = it.F_Money,
                F_UserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_UserId)).Select(u => u.RealName),
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_CheckState = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_CheckState) && dic.DictionaryTypeId.Equals("2043513064111869952")).Select(dic => dic.FullName),
                F_CheckUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CheckUserId)).Select(u => u.RealName),
                F_CheckTime = it.F_CheckTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_PurchaseNum = it.F_PurchaseNum.ToString(),
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

        });

        var resData = list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        list = resData.ToObject<List<APuOrderListOutput>>(CommonConst.options);
        return list;
    }

    /// <summary>
    /// 导出a_pu_order.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] APuOrderListQueryInput input)
    {
        var exportData = new List<APuOrderListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<APuOrderListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName, input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新a_pu_order.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] APuOrderUpInput input)
    {
        // 获取原有数据用于日志记录
        var originalEntity = await _repository.AsQueryable()
            .Includes(it => it.APuOrderItemList)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.id == id && it.DeleteMark == null)
            .FirstAsync();

        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuOrderEntity>();
        entity.id = id;
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuOrderEntity>(new APuOrderEntity()));
        ConfigModel tableFieldf01abdConfig = new ConfigModel
        {
            tableName = "a_pu_order_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "采购单商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuOrderItemEntity>(new APuOrderItemEntity())
        };
        FieldsModel tableFieldf01abdModel = new FieldsModel()
        {
            __config__ = tableFieldf01abdConfig,
            __vModel__ = "tableFieldf01abd"
        };
        fieldList.Add(tableFieldf01abdModel);
        ConfigModel tableField8b2a57Config = new ConfigModel
        {
            tableName = "a_pu_order_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "采购单日志",
            children = ExportImportDataHelper.GetTemplateParsing<APuOrderLogEntity>(new APuOrderLogEntity())
        };
        FieldsModel tableField8b2a57Model = new FieldsModel()
        {
            __config__ = tableField8b2a57Config,
            __vModel__ = "tableField8b2a57"
        };
        fieldList.Add(tableField8b2a57Model);

        // 移除采购单商品信息可能被删除数据
        if (input.tableFieldf01abd !=null && input.tableFieldf01abd.Any())
            await _repository.AsSugarClient().Deleteable<APuOrderItemEntity>().Where(it => it.F_OrderId == entity.id && !input.tableFieldf01abd.Select(it => it.F_Id).ToList().Contains(it.F_Id)).ExecuteCommandAsync();
        else
            await _repository.AsSugarClient().Deleteable<APuOrderItemEntity>().Where(it => it.F_OrderId == entity.id).ExecuteCommandAsync();

        // 新增采购单商品信息新数据
        var aPuOrderItemEntityList = input.tableFieldf01abd.Adapt<List<APuOrderItemEntity>>();


        // 如果前端未传入订单号，则在更新时生成一个新的订单号（与创建逻辑一致）
        if (string.IsNullOrEmpty(entity.F_OrderNo))
        {
            var today = DateTime.Now.ToString("yyyyMMdd");
            var todayPrefix = $"CG{today}";

            // 查询今天已存在的订单数量
            var todayCount = await _repository.AsQueryable()
                .Where(it => it.F_OrderNo.StartsWith(todayPrefix) && it.DeleteMark == null)
                .CountAsync();

            // 生成新的订单号：CG + 日期 + 序号（3位，不足补0）
            var sequenceNumber = (todayCount + 1).ToString("D3");
            entity.F_OrderNo = $"{todayPrefix}{sequenceNumber}";
        }

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_OrderNo,
                    it.F_SupplierId,
                    it.F_ProdPlanId,
                    it.F_Money,
                    it.F_UserId,
                    it.F_DeliveryDate,
                    it.F_PurchaseNum,
                    it.F_Description,
                })
                .ExecuteCommandAsync();

            if(aPuOrderItemEntityList != null)
            {
                foreach (var item in aPuOrderItemEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_OrderId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_OrderId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }

        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }

        // 生成操作日志
        await GenerateOperationLog(originalEntity, entity, input);
    }

    /// <summary>
    /// 修改状态.
    /// </summary>
    [HttpGet("Check/{id}/{state}")]
    public async Task Check(string id, string state)
    {
        APuOrderEntity? entity = await _repository.GetFirstAsync(it => it.id == id);

        int isOk = await _repository.AsUpdateable().SetColumns(it => new APuOrderEntity()
        {
            F_CheckState = state,
            F_CheckTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime(),
            F_CheckUserId = _userManager.UserId,
        }).Where(it => it.id == id).ExecuteCommandAsync();


        var userEntity = (await _repositoryUser.AsQueryable().Where(it => it.Id == entity.F_CreatorUserId && it.DeleteMark == null).Select(it => it.RealName).ToListAsync()).FirstOrDefault();
        if (state == "1")
        {
            //给采购人发送站内信--通过
            if (!string.IsNullOrEmpty(entity.F_UserId))
            {
                var messageEntity = new MessageEntity();
                messageEntity.Id = SnowflakeIdHelper.NextId();
                messageEntity.Title = "新增采购单已通过-" + entity.F_OrderNo;
                messageEntity.UserId = entity.F_UserId;
                messageEntity.Type = 3;
                messageEntity.FlowType = 1;
                messageEntity.IsRead = 0;
                messageEntity.SortCode = 0;
                messageEntity.CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                messageEntity.TenantId = "0";
                messageEntity.BodyText = userEntity + "-新增采购单已通过" + entity.F_OrderNo + ",请前往仓储/采购-采购单查看！";

                await _repositoryMessage.AsInsertable(messageEntity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
            }
        }
        else if (state == "2")
        {
            //给创建采购单人员发送站内信--驳回
            var messageEntity = new MessageEntity();
            messageEntity.Id = SnowflakeIdHelper.NextId();
            messageEntity.Title = "新增采购单被驳回-" + entity.F_OrderNo;
            messageEntity.UserId = entity.F_CreatorUserId;
            messageEntity.Type = 3;
            messageEntity.FlowType = 1;
            messageEntity.IsRead = 0;
            messageEntity.SortCode = 0;
            messageEntity.CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            messageEntity.TenantId = "0";
            messageEntity.BodyText = userEntity + "-新增采购单被驳回" + entity.F_OrderNo + ",请前往仓储/采购-采购单查看！";

            await _repositoryMessage.AsInsertable(messageEntity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        }
    }
    
    /// <summary>
     /// 删除a_pu_order.
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
    /// 批量删除a_pu_order.
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
            // 批量删除a_pu_order
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_pu_order详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuOrderItemList)
            .Includes(it => it.APuOrderLogList)
            .Where(it => it.id == id)
            .Select(it => new APuOrderDetailOutput
            {
                id = it.id,
                F_OrderNo = it.F_OrderNo,
                F_SupplierId = SqlFunc.Subqueryable<ASupplierEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_SupplierId)).Select(u => u.F_SupplierName),
                F_ProdPlanId = SqlFunc.Subqueryable<APuPlanEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_ProdPlanId)).Select(u => u.F_PlanName),
                F_Money = it.F_Money,
                F_UserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_UserId)).Select(u => u.RealName),
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_PurchaseNum = it.F_PurchaseNum.ToString(),
                F_Description = it.F_Description,
                F_CheckState = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_CheckState) && dic.DictionaryTypeId.Equals("2043513064111869952")).Select(dic => dic.FullName),
                F_CheckUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CheckUserId)).Select(u => u.RealName),
                F_CheckTime = it.F_CheckTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableFieldf01abd = it.APuOrderItemList.Adapt<List<APuOrderItemDetailOutput>>(),
                tableField8b2a57 = it.APuOrderLogList.Adapt<List<APuOrderLogDetailOutput>>(),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFieldf01abd), async aPuOrderItem =>
        {
            var aPuOrder = data.ToList().Find(it => it.id.Equals(aPuOrderItem.F_OrderId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 供应商ID
            var F_SupplierIdData = await _dataInterfaceService.GetDynamicList("aPuOrderItemF_SupplierId", "2008457397298925568", "F_Id", "F_SupplierName", "");
            if(aPuOrderItem.F_SupplierId != null)
            {
                aPuOrderItem.F_SupplierId = F_SupplierIdData.Find(it => it.id.Equals(aPuOrderItem.F_SupplierId))?.fullName;
            }

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            var goodsUnit = _repositorysp.AsQueryable().Where(it => it.id == aPuOrderItem.F_CustomerId).Select(g => g.F_Unit).First();
            if (!string.IsNullOrEmpty(goodsUnit))
            {
                var F_UnitIdCascader = goodsUnit.ToObject<List<string>>();
                if (F_UnitIdCascader.Count > 1)
                {
                    var goodsUnitData =  _repositorysp.AsQueryable().Where(it => it.id == aPuOrderItem.F_CustomerId).Select(g => g.F_Unit_Ratio).First();
                    List<UnitItem> goodsUnitList = JsonSerializer.Deserialize<List<UnitItem>>(goodsUnitData);

                    aPuOrderItem.F_NumInfo = Math.Floor(aPuOrderItem.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName + "/" + Math.Floor(goodsUnitList[1].qty * aPuOrderItem.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    aPuOrderItem.F_NumInfo = Math.Floor(aPuOrderItem.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }
            aPuOrderItem.F_CustomerId = _repositorysp.AsQueryable().Where(it => it.id == aPuOrderItem.F_CustomerId).Select(g => SqlFunc.MergeString(g.F_GoodsName, "/", g.F_GoodsNo)).First();
        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField8b2a57), async aPuOrderLog =>
        {
            var aPuOrder = data.ToList().Find(it => it.id.Equals(aPuOrderLog.F_OrderId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuOrderLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuOrderLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuOrderLog.F_CreatorTime = aPuOrderLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();

        return resData.FirstOrDefault();
    }

    /// <summary>
    /// 生成操作日志
    /// </summary>
    /// <param name="originalEntity">原始数据</param>
    /// <param name="newEntity">新数据</param>
    /// <param name="input">输入参数</param>
    /// <returns></returns>
    private async Task GenerateOperationLog(APuOrderEntity originalEntity, APuOrderEntity newEntity, APuOrderUpInput input)
    {
        var logContents = new List<string>();

        // 比较主表字段变化
        var mainTableChanges = CompareMainTableFields(originalEntity, newEntity);
        if (mainTableChanges.Any())
        {
            logContents.AddRange(mainTableChanges);
        }

        // 比较子表变化
        var itemChanges = await CompareItemTableChanges(originalEntity?.APuOrderItemList ?? new List<APuOrderItemEntity>(),
                                                        input.tableFieldf01abd ?? new List<APuOrderItemCrInput>());
        if (itemChanges != null && itemChanges.Any())
        {
            logContents.AddRange(itemChanges);
        }

        // 如果有任何变化，创建日志记录
        if (logContents.Any())
        {
            var logEntity = new APuOrderLogEntity
            {
                F_Id = SnowflakeIdHelper.NextId(),
                F_OrderId = newEntity.id,
                F_Title = "修改数据",
                F_Content = string.Join("；", logContents),
                F_CreatorUserId = _userManager.UserId,
                F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
            };

            await _repository.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 比较主表字段变化
    /// </summary>
    private List<string> CompareMainTableFields(APuOrderEntity original, APuOrderEntity updated)
    {
        var changes = new List<string>();

        // 采购单号
        if (!string.Equals(original?.F_OrderNo, updated.F_OrderNo))
        {
            changes.Add($"采购单号：\"{original?.F_OrderNo ?? ""}\" → \"{updated.F_OrderNo ?? ""}\"");
        }

        // 供应商ID
        if (!string.Equals(original?.F_SupplierId, updated.F_SupplierId))
        {
            changes.Add($"供应商：\"{original?.F_SupplierId ?? ""}\" → \"{updated.F_SupplierId ?? ""}\"");
        }

        // 生产计划ID
        if (!string.Equals(original?.F_ProdPlanId, updated.F_ProdPlanId))
        {
            changes.Add($"生产计划：\"{original?.F_ProdPlanId ?? ""}\" → \"{updated.F_ProdPlanId ?? ""}\"");
        }

        // 金额
        if (original?.F_Money != updated.F_Money)
        {
            changes.Add($"金额：\"{original?.F_Money ?? 0}\" → \"{updated.F_Money ?? 0}\"");
        }

        // 采购人
        if (!string.Equals(original?.F_UserId, updated.F_UserId))
        {
            changes.Add($"采购人：\"{original?.F_UserId ?? ""}\" → \"{updated.F_UserId ?? ""}\"");
        }

        // 交货日期
        if (original?.F_DeliveryDate?.ToString("yyyy-MM-dd") != updated.F_DeliveryDate?.ToString("yyyy-MM-dd"))
        {
            changes.Add($"交货日期：\"{original?.F_DeliveryDate?.ToString("yyyy-MM-dd") ?? ""}\" → \"{updated.F_DeliveryDate?.ToString("yyyy-MM-dd") ?? ""}\"");
        }

        // 采购数量
        if (original?.F_PurchaseNum != updated.F_PurchaseNum)
        {
            changes.Add($"采购数量：\"{original?.F_PurchaseNum ?? 0}\" → \"{updated.F_PurchaseNum ?? 0}\"");
        }

        // 备注
        if (!string.Equals(original?.F_Description, updated.F_Description))
        {
            changes.Add($"备注：\"{original?.F_Description ?? ""}\" → \"{updated.F_Description ?? ""}\"");
        }

        return changes;
    }

    /// <summary>
    /// 比较子表变化
    /// </summary>
    private async Task<List<string>> CompareItemTableChanges(List<APuOrderItemEntity> originalItems, List<APuOrderItemCrInput> newItems)
    {
        var changes = new List<string>();

        // 转换为实体列表进行比较
        var newItemEntities = newItems.Adapt<List<APuOrderItemEntity>>();

        // 找出新增的商品
        var addedItems = newItemEntities.Where(newItem =>
            string.IsNullOrEmpty(newItem.F_Id) ||
            !originalItems.Any(orig => orig.F_Id == newItem.F_Id)).ToList();

        foreach (var addedItem in addedItems)
        {
            // 获取商品名称（如果有的话）
            var goodsName = await GetGoodsNameById(addedItem.F_CustomerId);
            changes.Add($"新增商品：{goodsName}，数量：{addedItem.F_Num ?? 0}，单价：{addedItem.F_Price ?? 0}");
        }

        // 找出删除的商品（在原数据中存在但在新数据中不存在的）
        var deletedItems = originalItems.Where(orig =>
            !newItemEntities.Any(newItem => newItem.F_Id == orig.F_Id)).ToList();

        foreach (var deletedItem in deletedItems)
        {
            var goodsName = await GetGoodsNameById(deletedItem.F_CustomerId);
            changes.Add($"删除商品：{goodsName}");
        }

        // 找出修改的商品
        var modifiedItems = newItemEntities.Where(newItem =>
            !string.IsNullOrEmpty(newItem.F_Id) &&
            originalItems.Any(orig => orig.F_Id == newItem.F_Id)).ToList();

        foreach (var newItem in modifiedItems)
        {
            var originalItem = originalItems.First(orig => orig.F_Id == newItem.F_Id);

            var itemChanges = new List<string>();

            // 比较商品ID
            if (!string.Equals(originalItem.F_CustomerId, newItem.F_CustomerId))
            {
                var oldGoodsName = await GetGoodsNameById(originalItem.F_CustomerId);
                var newGoodsName = await GetGoodsNameById(newItem.F_CustomerId);
                itemChanges.Add($"商品：\"{oldGoodsName}\" → \"{newGoodsName}\"");
            }

            // 比较供应商ID
            if (!string.Equals(originalItem.F_SupplierId, newItem.F_SupplierId))
            {
                itemChanges.Add($"供应商：\"{originalItem.F_SupplierId ?? ""}\" → \"{newItem.F_SupplierId ?? ""}\"");
            }

            // 比较数量
            if (originalItem.F_Num != newItem.F_Num)
            {
                itemChanges.Add($"数量：\"{originalItem.F_Num ?? 0}\" → \"{newItem.F_Num ?? 0}\"");
            }

            // 比较单价
            if (originalItem.F_Price != newItem.F_Price)
            {
                itemChanges.Add($"单价：\"{originalItem.F_Price ?? 0}\" → \"{newItem.F_Price ?? 0}\"");
            }

            // 比较备注
            if (!string.Equals(originalItem.F_Description, newItem.F_Description))
            {
                itemChanges.Add($"备注：\"{originalItem.F_Description ?? ""}\" → \"{newItem.F_Description ?? ""}\"");
            }

            if (itemChanges.Any())
            {
                var goodsName = await GetGoodsNameById(newItem.F_CustomerId);
                changes.Add($"修改商品 {goodsName}：{string.Join("，", itemChanges)}");
            }
        }

        return changes;
    }

    /// <summary>
    /// 根据商品ID获取商品名称
    /// </summary>
    private async Task<string> GetGoodsNameById(string goodsId)
    {
        if (string.IsNullOrEmpty(goodsId))
            return "未知商品";

        try
        {
            // 这里需要根据实际的数据接口获取商品名称
            // 假设有一个数据接口可以获取商品信息
            var goodsData = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2008721559174385664", "F_Id", "F_GoodsName", "");
            var goods = goodsData.FirstOrDefault(g => g.id == goodsId);
            return goods?.fullName ?? "未知商品";
        }
        catch
        {
            return "未知商品";
        }
    }


    /// <summary>
    /// 根据生产计划ID获取对应的物料与数量（F_GoodsId, F_Num）
    /// </summary>
    /// <param name="prodPlanId">生产计划 ID（F_ProdPlanId）</param>
    /// <returns>匹配记录的 F_GoodsId 与 F_Num 列表</returns>
    [HttpGet("ProdPlanItems/{prodPlanId}")]
    public async Task<dynamic> GetProdPlanItemsByProdPlanId(string prodPlanId)
    {
        if (string.IsNullOrEmpty(prodPlanId)) return new List<object>();

        var items = await _repositoryPuPlanItem.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_PlanId == prodPlanId)
            .Select(it => new
            {
                F_GoodsId = it.F_CustomerId,
                F_Num = it.F_Num,
                F_SupplierId = it.F_SupplierId,
                F_Price = it.F_Price,
            })
            .ToListAsync();

        return items;
    }





}
public class UnitItem
{
    public string id { get; set; }
    public string name { get; set; }
    public int rate { get; set; }
    public int qty { get; set; }
}