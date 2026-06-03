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
using JNPF.example.Entitys.Dto.APuPlan;
using JNPF.example.Entitys.Dto.APuPlanItem;
using JNPF.example.Entitys.Dto.APuPlanLog;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
using JNPF.Message.Entitys;

namespace JNPF.example;

/// <summary>
/// 业务实现：a_pu_plan.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuPlan", Order = 200)]
[Route("api/example/[controller]")]
public class APuPlanService : IAPuPlanService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuPlanEntity> _repository;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositorysp;
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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"计划编号\",\"field\":\"F_PlanNo\"},{\"value\":\"计划名称\",\"field\":\"F_PlanName\"},{\"value\":\"供应商\",\"field\":\"F_SupplierId\"},{\"value\":\"金额\",\"field\":\"F_Money\"},{\"value\":\"交货日期\",\"field\":\"F_DeliveryDate\"},{\"value\":\"采购数量\",\"field\":\"F_PurchaseNum\"},{\"value\":\"负责人\",\"field\":\"F_ResponsibleUserId\"},{\"value\":\"审核状态\",\"field\":\"F_CheckState\"},{\"value\":\"审核人员\",\"field\":\"F_CheckUserId\"},{\"value\":\"审核时间\",\"field\":\"F_CheckTime\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="APuPlanService"/>类型的新实例.
    /// </summary>
    public APuPlanService(
        ISqlSugarRepository<UserEntity> repositoryUser,
        ISqlSugarRepository<MessageEntity> repositoryMessage,
        ISqlSugarRepository<APuPlanEntity> repository,
        ISqlSugarRepository<AGoodsEntity> repositorysp,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryUser = repositoryUser;
        _repositoryMessage = repositoryMessage;
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
    /// 获取a_pu_plan.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.APuPlanItemList)
            //.Where(it => it.DeleteMark == null)
            .Includes(it => it.APuPlanLogList)
            //.Where(it => it.DeleteMark == null)
            .Select(it => new APuPlanEntity
            {
                id = it.id,
                F_PlanNo = it.F_PlanNo,
                F_PlanName = it.F_PlanName,
                F_SupplierId = it.F_SupplierId,
                F_Money = it.F_Money,
                F_DeliveryDate = it.F_DeliveryDate,
                F_PurchaseNum = it.F_PurchaseNum,
                F_Description = it.F_Description,
                F_CheckState = it.F_CheckState,
                F_CheckUserId = it.F_CheckUserId,
                F_CheckTime = it.F_CheckTime,
                F_ResponsibleUserId = it.F_ResponsibleUserId,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                APuPlanItemList = it.APuPlanItemList.Where(item => item.DeleteMark == null).ToList(),
                APuPlanLogList = it.APuPlanLogList.Where(log => log.DeleteMark == null).ToList(),
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuPlanInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFieldc87c94, async aPuPlanItem =>
        {
            // 创建人员
            aPuPlanItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuPlanItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuPlanItem.F_CreatorTime = aPuPlanItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFielde82301, async aPuPlanLog =>
        {
            // 创建人员
            aPuPlanLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuPlanLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuPlanLog.F_CreatorTime = aPuPlanLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

    /// <summary>
    /// 获取a_pu_plan列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuPlanListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuPlanItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuPlanLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuPlanListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_plan"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_plan_item"))) aPuPlanItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_plan_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_plan_log"))) aPuPlanLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_plan_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuPlanEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuPlanItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldc87c94-", entityInfo, 1);
        List<IConditionalModel> aPuPlanItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuPlanItemConditionalModel.AddRange(aPuPlanItemAuthorizeWhere);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuPlanLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFielde82301-", entityInfo, 1);
        List<IConditionalModel> aPuPlanLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuPlanLogConditionalModel.AddRange(aPuPlanLogAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuPlanEntity>();
        var F_SupplierIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_SupplierId").DbColumnName;

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_PlanNo), it => it.F_PlanNo.Contains(input.F_PlanNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_PlanName), it => it.F_PlanName.Contains(input.F_PlanName))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_SupplierIdDbColumnName, input.F_SupplierId))
            .WhereIF(input.F_DeliveryDate?.Count() > 0, it => SqlFunc.Between(it.F_DeliveryDate, input.F_DeliveryDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_DeliveryDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_CheckState), it => it.F_CheckState.Equals(input.F_CheckState))
            .Where(authorizeWhere)
            .WhereIF(!string.IsNullOrEmpty(input.F_ResponsibleUserId), it => it.F_ResponsibleUserId.Equals(input.F_ResponsibleUserId))
            .WhereIF(aPuPlanItemAuthorizeWhere?.Count > 0, it => it.APuPlanItemList.Any(aPuPlanItemAuthorizeWhere))
            .WhereIF(aPuPlanLogAuthorizeWhere?.Count > 0, it => it.APuPlanLogList.Any(aPuPlanLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aPuPlanItemConditionalModel.Count > 0, it => it.APuPlanItemList.Any(aPuPlanItemConditionalModel))
            .WhereIF(aPuPlanLogConditionalModel.Count > 0, it => it.APuPlanLogList.Any(aPuPlanLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new APuPlanListOutput
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
            .Select(it => new APuPlanListOutput
            {
                id = it.id,
                F_PlanNo = it.F_PlanNo,
                F_PlanName = it.F_PlanName,
                F_SupplierId = it.F_SupplierId,
                F_Money = it.F_Money,
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_PurchaseNum = it.F_PurchaseNum.ToString(),
                F_Description = it.F_Description,
                F_CheckStateInfo = it.F_CheckState,
                F_CheckState = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_CheckState) && dic.DictionaryTypeId.Equals("2043513064111869952")).Select(dic => dic.FullName),
                F_CheckUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CheckUserId)).Select(u => u.RealName),
                F_CheckTime = it.F_CheckTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_ResponsibleUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_ResponsibleUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
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

        });

        data.pagination = idsQ.pagination;

        return PageResult<APuPlanListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_pu_plan.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] APuPlanCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuPlanEntity>();
        entity.id = SnowflakeIdHelper.NextId();

        // 如果前端没有传F_PlanNo，则自动生成
        if (string.IsNullOrEmpty(entity.F_PlanNo))
        {
            // 获取当前日期
            string today = DateTime.Now.ToString("yyyyMMdd");

            // 查询今天已有的最大编号
            var maxPlanNo = await _repository.AsQueryable()
                .Where(it => it.F_PlanNo.StartsWith($"CGJH{today}"))
                .Where(it => it.DeleteMark == null)
                .Select(it => it.F_PlanNo)
                .ToListAsync();

            // 提取序号并找到最大值
            int maxSequence = 0;
            foreach (var planNo in maxPlanNo)
            {
                if (planNo.Length >= $"CGJH{today}".Length + 3)
                {
                    string sequenceStr = planNo.Substring($"CGJH{today}".Length, 3);
                    if (int.TryParse(sequenceStr, out int sequence))
                    {
                        if (sequence > maxSequence)
                        {
                            maxSequence = sequence;
                        }
                    }
                }
            }

            // 生成新的编号
            string newSequence = (maxSequence + 1).ToString("D3");
            entity.F_PlanNo = $"CGJH{today}{newSequence}";
        }
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuPlanEntity>(new APuPlanEntity()));
        ConfigModel tableFieldc87c94Config = new ConfigModel
        {
            tableName = "a_pu_plan_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择采购商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuPlanItemEntity>(new APuPlanItemEntity())
        };
        FieldsModel tableFieldc87c94Model = new FieldsModel()
        {
            __config__ = tableFieldc87c94Config,
            __vModel__ = "tableFieldc87c94"
        };
        fieldList.Add(tableFieldc87c94Model);

        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        entity.F_CheckState = "0";


        var aPuPlanItemEntityList = input.tableFieldc87c94.Adapt<List<APuPlanItemEntity>>();
        if(aPuPlanItemEntityList != null)
        {
            foreach (var item in aPuPlanItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuPlanItemList = aPuPlanItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.APuPlanItemList)
            .ExecuteCommandAsync();

        var userEntity = (await _repositoryUser.AsQueryable().Where(it => it.Id == _userManager.UserId && it.DeleteMark == null).Select(it => it.RealName).ToListAsync()).FirstOrDefault();
        
        var userList = await _repositoryUser.AsQueryable().Where(it => it.OrganizeId.Contains("2020769494137442304") && it.DeleteMark == null).Select(it => it.Id).ToListAsync();
        //给总经办发送站内信
        foreach (var item in userList)
        {
            var messageSystemEntity = new MessageEntity();
            messageSystemEntity.Id = SnowflakeIdHelper.NextId();
            messageSystemEntity.Title = "新增采购计划-" + entity.F_PlanName;
            messageSystemEntity.UserId = item;
            messageSystemEntity.Type = 3;
            messageSystemEntity.FlowType = 1;
            messageSystemEntity.IsRead = 0;
            messageSystemEntity.SortCode = 0;
            messageSystemEntity.CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            messageSystemEntity.TenantId = "0";
            messageSystemEntity.BodyText = userEntity + "-新增采购计划" + entity.F_PlanName + ",请前往仓储/采购-采购计划审批！";
            await _repositoryMessage.AsInsertable(messageSystemEntity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 获取a_pu_plan无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(APuPlanListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuPlanItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuPlanLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuPlanListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_plan"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_plan_item"))) aPuPlanItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_plan_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_plan_log"))) aPuPlanLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_plan_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuPlanEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuPlanItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldc87c94-", entityInfo, 1);
        List<IConditionalModel> aPuPlanItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuPlanLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFielde82301-", entityInfo, 1);
        List<IConditionalModel> aPuPlanLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);


        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuPlanEntity>();
        var F_SupplierIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_SupplierId").DbColumnName;

        var list = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_PlanNo), it => it.F_PlanNo.Contains(input.F_PlanNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_PlanName), it => it.F_PlanName.Contains(input.F_PlanName))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_SupplierIdDbColumnName, input.F_SupplierId))
            .WhereIF(input.F_DeliveryDate?.Count() > 0, it => SqlFunc.Between(it.F_DeliveryDate, input.F_DeliveryDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_DeliveryDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_CheckState), it => it.F_CheckState.Equals(input.F_CheckState))
            .WhereIF(!string.IsNullOrEmpty(input.F_ResponsibleUserId), it => it.F_ResponsibleUserId.Equals(input.F_ResponsibleUserId))
            .Where(authorizeWhere)
            .WhereIF(aPuPlanItemAuthorizeWhere?.Count > 0, it => it.APuPlanItemList.Any(aPuPlanItemAuthorizeWhere))
            .WhereIF(aPuPlanLogAuthorizeWhere?.Count > 0, it => it.APuPlanLogList.Any(aPuPlanLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aPuPlanItemConditionalModel.Count > 0, it => it.APuPlanItemList.Any(aPuPlanItemConditionalModel))
            .WhereIF(aPuPlanLogConditionalModel.Count > 0, it => it.APuPlanLogList.Any(aPuPlanLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new APuPlanListOutput
            {
                id = it.id,
                F_PlanNo = it.F_PlanNo,
                F_PlanName = it.F_PlanName,
                F_SupplierId = it.F_SupplierId,
                F_Money = it.F_Money,
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_PurchaseNum = it.F_PurchaseNum.ToString(),
                F_Description = it.F_Description,
                F_CheckState = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_CheckState) && dic.DictionaryTypeId.Equals("2043513064111869952")).Select(dic => dic.FullName),
                F_CheckUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CheckUserId)).Select(u => u.RealName),
                F_CheckTime = it.F_CheckTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_ResponsibleUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_ResponsibleUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 供应商ID
            var F_SupplierIdData = await _dataInterfaceService.GetDynamicList("F_SupplierId", "2008457397298925568", "F_Id", "F_SupplierName", "");
            if(item.F_SupplierId != null)
            {
                item.F_SupplierId = F_SupplierIdData.Find(it => it.id.Equals(item.F_SupplierId))?.fullName;
            }


        });

        var resData = list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<APuPlanEntity>(new APuPlanEntity()));
        ConfigModel tableFieldc87c94Config = new ConfigModel
        {
            tableName = "a_pu_plan_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择采购商品",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<APuPlanItemEntity>(new APuPlanItemEntity())
        };
        FieldsModel tableFieldc87c94 = new FieldsModel()
        {
            __config__ = tableFieldc87c94Config,
            __vModel__ = "tableFieldc87c94"
        };
        fieldList.Add(tableFieldc87c94);

        resData = await _controlParsing.GetParsDataList(resData, "tableFieldc87c94-F_CustomerId", "popupSelect", _userManager.TenantId, fieldList);

        list = resData.ToObject<List<APuPlanListOutput>>(CommonConst.options);
        return list;
    }

    /// <summary>
    /// 导出a_pu_plan.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] APuPlanListQueryInput input)
    {
        var exportData = new List<APuPlanListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<APuPlanListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName, input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新a_pu_plan.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] APuPlanUpInput input)
    {
        // 获取原有数据用于日志记录
        var originalEntity = await _repository.AsQueryable()
            .Includes(it => it.APuPlanItemList)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.id == id && it.DeleteMark == null)
            .FirstAsync();

        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuPlanEntity>();
        entity.id = id;

        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_PlanNo.Equals(input.F_PlanNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "计划编号");

        if (string.IsNullOrEmpty(entity.F_PlanNo))
        {
            entity.F_PlanNo = oldEntity.F_PlanNo;
        }

        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuPlanEntity>(new APuPlanEntity()));
        ConfigModel tableFieldc87c94Config = new ConfigModel
        {
            tableName = "a_pu_plan_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择采购商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuPlanItemEntity>(new APuPlanItemEntity())
        };
        FieldsModel tableFieldc87c94Model = new FieldsModel()
        {
            __config__ = tableFieldc87c94Config,
            __vModel__ = "tableFieldc87c94"
        };
        fieldList.Add(tableFieldc87c94Model);

        // 移除采购计划商品信息可能被删除数据
        if (input.tableFieldc87c94 !=null && input.tableFieldc87c94.Any())
            await _repository.AsSugarClient().Deleteable<APuPlanItemEntity>().Where(it => it.F_PlanId == entity.id && !input.tableFieldc87c94.Select(it => it.F_Id).ToList().Contains(it.F_Id)).ExecuteCommandAsync();
        else
            await _repository.AsSugarClient().Deleteable<APuPlanItemEntity>().Where(it => it.F_PlanId == entity.id).ExecuteCommandAsync();

        // 新增采购计划商品信息新数据
        var aPuPlanItemEntityList = input.tableFieldc87c94.Adapt<List<APuPlanItemEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_PlanNo,
                    it.F_PlanName,
                    it.F_SupplierId,
                    it.F_Money,
                    it.F_DeliveryDate,
                    it.F_PurchaseNum,
                    it.F_Description,
                    it.F_ResponsibleUserId,
                })
                .ExecuteCommandAsync();

            if(aPuPlanItemEntityList != null)
            {
                foreach (var item in aPuPlanItemEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_PlanId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_PlanId = entity.id;
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
        APuPlanEntity? entity = await _repository.GetFirstAsync(it => it.id == id);

        int isOk = await _repository.AsUpdateable().SetColumns(it => new APuPlanEntity()
        {
            F_CheckState = state,
            F_CheckTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime(),
            F_CheckUserId = _userManager.UserId,
        }).Where(it => it.id == id).ExecuteCommandAsync();


        var userEntity = (await _repositoryUser.AsQueryable().Where(it => it.Id == entity.F_CreatorUserId && it.DeleteMark == null).Select(it => it.RealName).ToListAsync()).FirstOrDefault();
        if (state == "1")
        {
            //给责任人发送站内信--通过
            if (!string.IsNullOrEmpty(entity.F_ResponsibleUserId))
            {
                var messageEntity = new MessageEntity();
                messageEntity.Id = SnowflakeIdHelper.NextId();
                messageEntity.Title = "新增采购计划已通过-" + entity.F_PlanName;
                messageEntity.UserId = entity.F_ResponsibleUserId;
                messageEntity.Type = 3;
                messageEntity.FlowType = 1;
                messageEntity.IsRead = 0;
                messageEntity.SortCode = 0;
                messageEntity.CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                messageEntity.TenantId = "0";
                messageEntity.BodyText = userEntity + "-新增采购计划已通过" + entity.F_PlanName + ",请前往仓储/采购-采购计划查看！";

                await _repositoryMessage.AsInsertable(messageEntity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
            }
        }
        else if(state == "2")
        {
            //给创建采购计划人员发送站内信--驳回
            var messageEntity = new MessageEntity();
            messageEntity.Id = SnowflakeIdHelper.NextId();
            messageEntity.Title = "新增采购计划被驳回-" + entity.F_PlanName;
            messageEntity.UserId = entity.F_CreatorUserId;
            messageEntity.Type = 3;
            messageEntity.FlowType = 1;
            messageEntity.IsRead = 0;
            messageEntity.SortCode = 0;
            messageEntity.CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            messageEntity.TenantId = "0";
            messageEntity.BodyText = userEntity + "-新增采购计划被驳回" + entity.F_PlanName + ",请前往仓储/采购-采购计划查看！";

            await _repositoryMessage.AsInsertable(messageEntity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 删除a_pu_plan.
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
    /// 批量删除a_pu_plan.
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
            // 批量删除a_pu_plan
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_pu_plan详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.APuPlanItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuPlanLogList)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.id == id)
            .Select(it => new APuPlanDetailOutput
            {
                id = it.id,
                F_PlanNo = it.F_PlanNo,
                F_PlanName = it.F_PlanName,
                F_SupplierId = it.F_SupplierId,
                F_Money = it.F_Money,
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_PurchaseNum = it.F_PurchaseNum.ToString(),
                F_Description = it.F_Description,
                F_CheckState = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_CheckState) && dic.DictionaryTypeId.Equals("2043513064111869952")).Select(dic => dic.FullName),
                F_CheckUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CheckUserId)).Select(u => u.RealName),
                F_CheckTime = it.F_CheckTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_ResponsibleUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_ResponsibleUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableFieldc87c94 = it.APuPlanItemList.Where(item => item.DeleteMark == null).Adapt<List<APuPlanItemDetailOutput>>(),
                tableFielde82301 = it.APuPlanLogList.Where(log => log.DeleteMark == null).Adapt<List<APuPlanLogDetailOutput>>(),
            }).ToListAsync();

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
        // 预加载子表中关联的商品信息（根据 F_CustomerId -> a_goods.id）以便批量映射商品字段
        var goodsIds = data.SelectMany(d => d.tableFieldc87c94 ?? new List<APuPlanItemDetailOutput>())
            .Select(i => i.F_CustomerId)
            .Where(id => !string.IsNullOrEmpty(id))
            .Distinct()
            .ToList();
        Dictionary<string, AGoodsEntity> goodsDict = new Dictionary<string, AGoodsEntity>();
        if (goodsIds.Any())
        {
            var goodsList = await _repositorysp.AsSugarClient().Queryable<AGoodsEntity>()
                .Where(g => goodsIds.Contains(g.id))
                .Select(g => new AGoodsEntity
                {
                    id = g.id,
                    F_GoodsNo = g.F_GoodsNo,
                    F_Unit = g.F_Unit,
                    F_Unit_Ratio = g.F_Unit_Ratio,
                    F_Specification = g.F_Specification,
                    F_Image = g.F_Image
                }).ToListAsync();
            goodsDict = goodsList.ToDictionary(g => g.id, g => g);
        }

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFieldc87c94), async aPuPlanItem =>
        {
            var aPuPlan = data.ToList().Find(it => it.id.Equals(aPuPlanItem.F_PlanId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 供应商ID
            var F_SupplierIdData = await _dataInterfaceService.GetDynamicList("aPuPlanItemF_SupplierId", "2008457397298925568", "F_Id", "F_SupplierName", "");
            if(aPuPlanItem.F_SupplierId != null)
            {
                aPuPlanItem.F_SupplierId = F_SupplierIdData.Find(it => it.id.Equals(aPuPlanItem.F_SupplierId))?.fullName;
            }

            aPuPlanItem.F_GoodsName = (await _repositorysp.GetFirstAsync(it => it.id == aPuPlanItem.F_CustomerId)).F_GoodsName;
            // 创建人员
            aPuPlanItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuPlanItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuPlanItem.F_CreatorTime = aPuPlanItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFielde82301), async aPuPlanLog =>
        {
            var aPuPlan = data.ToList().Find(it => it.id.Equals(aPuPlanLog.F_PlanId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuPlanLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuPlanLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuPlanLog.F_CreatorTime = aPuPlanLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<APuPlanEntity>(new APuPlanEntity()));
        ConfigModel tableFieldc87c94Config = new ConfigModel
        {
            tableName = "a_pu_plan_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择采购商品",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<APuPlanItemEntity>(new APuPlanItemEntity())
        };
        FieldsModel tableFieldc87c94 = new FieldsModel()
        {
            __config__ = tableFieldc87c94Config,
            __vModel__ = "tableFieldc87c94"
        };
        fieldList.Add(tableFieldc87c94);

        resData = await _controlParsing.GetParsDataList(resData,"tableFieldc87c94-F_CustomerId","popupSelect",_userManager.TenantId, fieldList);

        // controlParsing 可能会把 F_CustomerId 替换为显示文本并生成 F_CustomerId_id 保存真实 id，
        // 因此在最终 resData 上再映射一次商品字段，确保新加的 F_GoodsNo/F_Unit/F_Specification/F_Image 保留在返回结果中。
        if (goodsDict != null && goodsDict.Any())
        {
            foreach (var row in resData)
            {
                if (!row.ContainsKey("tableFieldc87c94") || row["tableFieldc87c94"] == null) continue;
                if (row["tableFieldc87c94"] is System.Collections.IEnumerable tableEnum)
                {
                    foreach (var childObj in tableEnum)
                    {
                        if (childObj is Dictionary<string, object> childDict)
                        {
                            string custId = null;
                            if (childDict.ContainsKey("F_CustomerId_id") && childDict["F_CustomerId_id"] != null)
                                custId = childDict["F_CustomerId_id"].ToString();
                            else if (childDict.ContainsKey("F_CustomerId") && childDict["F_CustomerId"] != null)
                                custId = childDict["F_CustomerId"].ToString();

                            if (!string.IsNullOrEmpty(custId) && goodsDict.ContainsKey(custId))
                            {
                                var g = goodsDict[custId];
                                childDict["F_GoodsNo"] = g?.F_GoodsNo;
                                childDict["F_Unit"] = g?.F_Unit;
                                childDict["F_Specification"] = g?.F_Specification;
                                childDict["F_Image"] = g?.F_Image;

                                // 将子表数量转换为“主单位+次单位”展示，如 5箱子50盒
                                if (childDict.ContainsKey("F_Num") && childDict["F_Num"] != null)
                                {
                                    if (decimal.TryParse(childDict["F_Num"].ToString(), out var numValue))
                                    {
                                        var displayNum = BuildDualUnitNumDisplay(numValue, g?.F_Unit_Ratio);
                                        if (!string.IsNullOrEmpty(displayNum))
                                            childDict["F_Num"] = displayNum;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return resData.FirstOrDefault();
    }

    /// <summary>
    /// 单位比例项（用于解析 F_Unit_Ratio JSON）.
    /// </summary>
    private class UnitRatioItem
    {
        public string id { get; set; }
        public string name { get; set; }
        public decimal rate { get; set; }
        public decimal qty { get; set; }
    }

    /// <summary>
    /// 根据单位比例把数量转成单位展示（例如：5箱子50盒 或 5箱子）
    /// </summary>
    /// <param name="numValue">数量值</param>
    /// <param name="unitRatioJson">单位比例JSON</param>
    /// <param name="onlyMainUnit">是否只显示主单位</param>
    /// <returns></returns>
    private static string BuildDualUnitNumDisplay(decimal numValue, string unitRatioJson, bool onlyMainUnit = false)
    {
        if (string.IsNullOrWhiteSpace(unitRatioJson)) return null;

        try
        {
            var ratioList = unitRatioJson.ToObject<List<UnitRatioItem>>();
            if (ratioList == null || ratioList.Count == 0) return null;

            var level1 = ratioList[0];
            var qty1 = level1.qty <= 0 ? 1 : level1.qty;
            var level1Value = numValue * qty1;
            var name1 = level1.name ?? string.Empty;

            if (onlyMainUnit || ratioList.Count < 2)
            {
                return $"{FormatNum(level1Value)}{name1}";
            }

            var level2 = ratioList[1];
            var qty2 = level2.qty <= 0 ? 1 : level2.qty;
            var level2Value = numValue * qty2;
            var name2 = level2.name ?? string.Empty;

            return $"{FormatNum(level1Value)}{name1}{FormatNum(level2Value)}{name2}";
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 数量格式化：去掉无意义的尾随零.
    /// </summary>
    private static string FormatNum(decimal value)
    {
        return value % 1 == 0 ? decimal.Truncate(value).ToString() : value.ToString("0.####");
    }

    /// <summary>
    /// 生成操作日志
    /// </summary>
    /// <param name="originalEntity">原始数据</param>
    /// <param name="newEntity">新数据</param>
    /// <param name="input">输入参数</param>
    /// <returns></returns>
    private async Task GenerateOperationLog(APuPlanEntity originalEntity, APuPlanEntity newEntity, APuPlanUpInput input)
    {
        var logContents = new List<string>();

        // 比较主表字段变化
        var mainTableChanges = CompareMainTableFields(originalEntity, newEntity);
        if (mainTableChanges.Any())
        {
            logContents.AddRange(mainTableChanges);
        }

        // 比较子表变化
        var itemChanges = await CompareItemTableChanges(originalEntity?.APuPlanItemList ?? new List<APuPlanItemEntity>(),
                                                        input.tableFieldc87c94 ?? new List<APuPlanItemCrInput>());
        if (itemChanges != null && itemChanges.Any())
        {
            logContents.AddRange(itemChanges);
        }

        // 如果有任何变化，创建日志记录
        if (logContents.Any())
        {
            var logEntity = new APuPlanLogEntity
            {
                F_Id = SnowflakeIdHelper.NextId(),
                F_PlanId = newEntity.id,
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
    private List<string> CompareMainTableFields(APuPlanEntity original, APuPlanEntity updated)
    {
        var changes = new List<string>();

        // 计划名称
        if (!string.Equals(original?.F_PlanName, updated.F_PlanName))
        {
            changes.Add($"计划名称：\"{original?.F_PlanName ?? ""}\" → \"{updated.F_PlanName ?? ""}\"");
        }

        // 供应商ID
        if (!string.Equals(original?.F_SupplierId, updated.F_SupplierId))
        {
            changes.Add($"供应商：\"{original?.F_SupplierId ?? ""}\" → \"{updated.F_SupplierId ?? ""}\"");
        }

        // 金额
        if (original?.F_Money != updated.F_Money)
        {
            changes.Add($"金额：\"{original?.F_Money ?? 0}\" → \"{updated.F_Money ?? 0}\"");
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
    private async Task<List<string>> CompareItemTableChanges(List<APuPlanItemEntity> originalItems, List<APuPlanItemCrInput> newItems)
    {
        var changes = new List<string>();

        // 转换为实体列表进行比较
        var newItemEntities = newItems.Adapt<List<APuPlanItemEntity>>();

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
}
