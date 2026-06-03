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
using JNPF.example.Entitys.Dto.ACtcContract;
using JNPF.example.Entitys.Dto.ACtcContractItem;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
using JNPF.Message.Entitys;
using Aop.Api.Domain;

namespace JNPF.example;

/// <summary>
/// 业务实现：a_ctc_contract.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "ACtcContract", Order = 200)]
[Route("api/example/[controller]")]
public class ACtcContractService : IACtcContractService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<ACtcContractEntity> _repository;
    private readonly ISqlSugarRepository<ACtcAddressEntity> _repositorydz;
    private readonly ISqlSugarRepository<ACtcContactEntity> _repositorylxr;
    private readonly ISqlSugarRepository<ACtcContractlogEntity> _repositorylog;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositorysp;
    private readonly ISqlSugarRepository<AProdProcessingEntity> _repositoryProcessing;
    private readonly ISqlSugarRepository<AProdRoutingEntity> _repositoryRouting;
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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"合同编号\",\"field\":\"F_ContractCode\"},{\"value\":\"客户\",\"field\":\"F_CustomerId\"},{\"value\":\"联系人ID\",\"field\":\"F_ContactId\"},{\"value\":\"客户地址ID\",\"field\":\"F_AddressId\"},{\"value\":\"客户合同号\",\"field\":\"F_CustomerCode\"},{\"value\":\"预付款\",\"field\":\"F_PrepayAmount\"},{\"value\":\"付款周期（天）\",\"field\":\"F_PaymentCycle\"},{\"value\":\"销售总数\",\"field\":\"F_SaleTotal\"},{\"value\":\"合同金额\",\"field\":\"F_ContractAmount\"},{\"value\":\"交货日期\",\"field\":\"F_DeliveryDate\"},{\"value\":\"业务员\",\"field\":\"F_SalesmanId\"},{\"value\":\"是否含税\",\"field\":\"F_IsTaxable\"},{\"value\":\"审核状态\",\"field\":\"F_AuditStatus\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"},{\"value\":\"选择合同商品-商品\",\"field\":\"tableFieldf4dfaf-F_CustomerId\"},{\"value\":\"选择合同商品-单价\",\"field\":\"tableFieldf4dfaf-F_Price\"},{\"value\":\"选择合同商品-销售数量\",\"field\":\"tableFieldf4dfaf-F_SaleQty\"},{\"value\":\"选择合同商品-折扣\",\"field\":\"tableFieldf4dfaf-F_Discount\"},{\"value\":\"选择合同商品-商品备注\",\"field\":\"tableFieldf4dfaf-F_Description\"},{\"value\":\"选择合同商品-门板厚度\",\"field\":\"tableFieldf4dfaf-F_DoorPlateThickness\"},{\"value\":\"选择合同商品-箱体板厚\",\"field\":\"tableFieldf4dfaf-F_BoxPlateThickness\"},{\"value\":\"选择合同商品-安装或安装梁\",\"field\":\"tableFieldf4dfaf-F_InstallOrBeam\"},{\"value\":\"选择合同商品-锁具\",\"field\":\"tableFieldf4dfaf-F_LockSet\"},{\"value\":\"选择合同商品-铰链\",\"field\":\"tableFieldf4dfaf-F_Hinge\"},{\"value\":\"选择合同商品-颜色\",\"field\":\"tableFieldf4dfaf-F_Color\"},{\"value\":\"选择合同商品-材质\",\"field\":\"tableFieldf4dfaf-F_Material\"},{\"value\":\"选择合同商品-审核状态\",\"field\":\"tableFieldf4dfaf-F_F_AuditStatus\"},{\"value\":\"选择合同商品-附件\",\"field\":\"tableFieldf4dfaf-F_Files\"},{\"value\":\"选择合同商品-创建人员\",\"field\":\"tableFieldf4dfaf-F_CreatorUserId\"},{\"value\":\"选择合同商品-创建时间\",\"field\":\"tableFieldf4dfaf-F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="ACtcContractService"/>类型的新实例.
    /// </summary>
    public ACtcContractService(
        ISqlSugarRepository<UserEntity> repositoryUser,
        ISqlSugarRepository<MessageEntity> repositoryMessage,
        ISqlSugarRepository<AGoodsEntity> repositorysp,
        ISqlSugarRepository<AProdProcessingEntity> repositoryProcessing,
        ISqlSugarRepository<AProdRoutingEntity> repositoryRouting,
        ISqlSugarRepository<ACtcContractEntity> repository,
        ISqlSugarRepository<ACtcAddressEntity> repositorydz,
        ISqlSugarRepository<ACtcContactEntity> repositorylxr,
        ISqlSugarRepository<ACtcContractlogEntity> repositorylog,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryUser = repositoryUser;
        _repositoryMessage = repositoryMessage;
        _repositorysp = repositorysp;
        _repositoryProcessing = repositoryProcessing;
        _repositoryRouting = repositoryRouting;
        _repository = repository;
        _repositorydz = repositorydz;
        _repositorylxr = repositorylxr;
        _repositorylog = repositorylog;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_ctc_contract.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.ACtcContractItemList)
            .Where(it => it.DeleteMark == null)
            .Select(it => new ACtcContractEntity
            {
                id = it.id,
                F_ContractCode = it.F_ContractCode,
                F_CustomerCode = it.F_CustomerCode,
                F_CustomerId = it.F_CustomerId,
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
                F_PrepayAmount = it.F_PrepayAmount,
                F_PaymentCycle = it.F_PaymentCycle,
                F_SaleTotal = it.F_SaleTotal,
                F_ContractAmount = it.F_ContractAmount,
                F_DeliveryDate = it.F_DeliveryDate,
                F_IsTaxable = it.F_IsTaxable,
                F_SalesmanId = it.F_SalesmanId,
                F_AuditStatus = it.F_AuditStatus,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                ACtcContractItemList = it.ACtcContractItemList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<ACtcContractInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFieldf4dfaf, async aCtcContractItem =>
        {
            // 创建人员
            aCtcContractItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aCtcContractItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aCtcContractItem.F_CreatorTime = aCtcContractItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            var goodEntity = await _repositorysp.GetFirstAsync(it => it.id.Equals(aCtcContractItem.F_CustomerId));

            var unit = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(it => it.id.Equals(aCtcContractItem.F_CustomerId)).Select(it => it.F_Unit).FirstAsync();

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(unit))
            {
                var F_UnitIdCascader = unit.ToObject<List<string>>();
                aCtcContractItem.F_UnitTwo = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));

            }

            aCtcContractItem.F_GoodsNo = goodEntity.F_GoodsNo;

            aCtcContractItem.F_Specification = goodEntity.F_Specification;
        });
        return data;
    }

    /// <summary>
    /// 获取a_ctc_contract列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] ACtcContractListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<ACtcContractListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_ctc_contract"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcContractEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        // 子表查询条件已移除（前端不需要子表数据）

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcContractEntity>();
        var F_CustomerIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_CustomerId").DbColumnName;
        var startF_PrepayAmount = input.F_PrepayAmount?.FirstOrDefault()?.ParseToDecimal() == null ? decimal.MinValue : input.F_PrepayAmount?.First();
        var endF_PrepayAmount = input.F_PrepayAmount?.LastOrDefault()?.ParseToDecimal() == null ? decimal.MaxValue : input.F_PrepayAmount?.Last();
        var F_SalesmanIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_SalesmanId").DbColumnName;


        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContractCode), it => it.F_ContractCode.Contains(input.F_ContractCode))
            .WhereIF(!string.IsNullOrEmpty(input.F_CustomerCode), it => it.F_CustomerCode.Contains(input.F_CustomerCode))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_CustomerIdDbColumnName, input.F_CustomerId))
            .WhereIF(input.F_PrepayAmount?.Count() > 0, it => SqlFunc.Between(it.F_PrepayAmount, startF_PrepayAmount, endF_PrepayAmount))
            .WhereIF(input.F_DeliveryDate?.Count() > 0, it => SqlFunc.Between(it.F_DeliveryDate, input.F_DeliveryDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_DeliveryDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_SalesmanIdDbColumnName, input.F_SalesmanId))
            .WhereIF(!string.IsNullOrEmpty(input.F_AuditStatus), it => it.F_AuditStatus.Contains(input.F_AuditStatus))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new ACtcContractListOutput
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
            .Select(it => new ACtcContractListOutput
            {
                id = it.id,
                F_ContractCode = it.F_ContractCode,
                F_CustomerCode = it.F_CustomerCode,
                F_CustomerId = it.F_CustomerId,
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
                F_PrepayAmount = it.F_PrepayAmount,
                F_PaymentCycle = it.F_PaymentCycle,
                F_SaleTotal = it.F_SaleTotal.ToString(),
                F_ContractAmount = it.F_ContractAmount,
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_IsTaxable = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsTaxable) == 1, "是", "否"),
                F_SalesmanId = it.F_SalesmanId,
                F_AuditStatus = it.F_AuditStatus,
                F_Description = it.F_Description,
                F_CreatorUserId = it.F_CreatorUserId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 客户ID
            var F_CustomerIdData = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2009458830353764352", "F_Id", "F_CustomerName", "");
            if (item.F_CustomerId != null)
            {
                item.F_CustomerId = F_CustomerIdData.Find(it => it.id.Equals(item.F_CustomerId))?.fullName;
            }
            if (item.F_ContactId != null)
            {
                var contactData = await _repositorylxr.AsQueryable().FirstAsync(c => c.F_Id.Equals(item.F_ContactId));
                item.F_ContactId = contactData?.F_ContactName;
            }

            // 业务员
            if (item.F_SalesmanId != null)
            {
                item.F_SalesmanId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_SalesmanId)))?.RealName;
            }

            // 创建人员
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;

        });

        data.pagination = idsQ.pagination;

        return PageResult<ACtcContractListOutput>.SqlSugarPageResult(data);

    }

    /// <summary>
    /// 新建a_ctc_contract.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] ACtcContractCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACtcContractEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ACtcContractEntity>(new ACtcContractEntity()));
        ConfigModel tableFieldf4dfafConfig = new ConfigModel
        {
            tableName = "a_ctc_contract_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择合同商品",
            children = ExportImportDataHelper.GetTemplateParsing<ACtcContractItemEntity>(new ACtcContractItemEntity())
        };
        FieldsModel tableFieldf4dfafModel = new FieldsModel()
        {
            __config__ = tableFieldf4dfafConfig,
            __vModel__ = "tableFieldf4dfaf"
        };
        fieldList.Add(tableFieldf4dfafModel);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        // 如果前端未传F_ContractCode，则自动生成编码
        if (string.IsNullOrEmpty(entity.F_ContractCode))
        {
            var prefix = "HT" + DateTime.Now.ToString("yyyyMMdd");

            // 查询数据库中已有相同前缀的编号
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_ContractCode != null && it.F_ContractCode.StartsWith(prefix))
                .Select(it => it.F_ContractCode)
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
            entity.F_ContractCode = prefix + nextSeq.ToString("D3");

        }


        var aCtcContractItemEntityList = input.tableFieldf4dfaf.Adapt<List<ACtcContractItemEntity>>();
        if(aCtcContractItemEntityList != null)
        {
            foreach (var item in aCtcContractItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();

                // 处理用料清单
                var inputItem = input.tableFieldf4dfaf.FirstOrDefault(it => it.F_Id == item.F_Id || it.F_CustomerId == item.F_CustomerId);
                if (inputItem?.ProjectGoods != null)
                {
                    var aCtcContractItemGoodEntityList = inputItem.ProjectGoods.Adapt<List<ACtcContractItemGoodEntity>>();
                    foreach (var itemGood in aCtcContractItemGoodEntityList)
                    {
                        itemGood.id = SnowflakeIdHelper.NextId();
                        itemGood.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        itemGood.F_ProjectItemId = item.F_Id;
                    }
                    item.ACtcContractItemGoodList = aCtcContractItemGoodEntityList;
                }
            }

            entity.ACtcContractItemList = aCtcContractItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.ACtcContractItemList)
                .ThenInclude(it => it.ACtcContractItemGoodList)
            .ExecuteCommandAsync();

        //给研发部发站内信
        var userEntity = (await _repositoryUser.AsQueryable().Where(it => it.Id == _userManager.UserId && it.DeleteMark == null).Select(it => it.RealName).ToListAsync()).FirstOrDefault();
        var userList = await _repositoryUser.AsQueryable().Where(it => it.OrganizeId.Contains("2020769791316463616") && it.DeleteMark == null).Select(it => it.Id).ToListAsync();
        foreach (var item in userList)
        {
            var messageSystemEntity = new MessageEntity();
            messageSystemEntity.Id = SnowflakeIdHelper.NextId();
            messageSystemEntity.Title = "新增合同订单-" + entity.F_ContractCode;
            messageSystemEntity.UserId = item;
            messageSystemEntity.Type = 3;
            messageSystemEntity.FlowType = 1;
            messageSystemEntity.IsRead = 0;
            messageSystemEntity.SortCode = 0;
            messageSystemEntity.CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            messageSystemEntity.TenantId = "0";
            messageSystemEntity.BodyText = userEntity + "-新增合同订单-" + entity.F_ContractCode + ",请前往客户管理-合同订单查看！";
            await _repositoryMessage.AsInsertable(messageSystemEntity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        }

        // 为每个合同商品子项生成加工单
        //if (aCtcContractItemEntityList != null)
        //{
        //    foreach (var item in aCtcContractItemEntityList)
        //    {
        //        var processingEntity = new AProdProcessingEntity();
        //        processingEntity.id = SnowflakeIdHelper.NextId();
        //        processingEntity.F_CreatorUserId = _userManager.UserId;
        //        processingEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        //        processingEntity.F_State = "1";
        //        processingEntity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        //        processingEntity.F_CustomerName = entity.F_CustomerId;

        //        // 关联合同ID
        //        processingEntity.F_ContractId = entity.id;

        //        // 排单序号
        //        var processNo = await _repositoryProcessing.AsQueryable()
        //            .Where(it => it.F_SequenceNo != null && it.DeleteMark == null)
        //            .MaxAsync(it => it.F_SequenceNo);
        //        processingEntity.F_SequenceNo = (processNo ?? 0) + 1;

        //        // 自动生成加工单编号：前缀 JG + yyyyMMdd，后缀 3 位序号
        //        var prefix = "JG" + DateTime.Now.ToString("yyyyMMdd");
        //        var existingNos = await _repositoryProcessing.AsQueryable()
        //            .Where(it => it.F_ProcessNo != null && it.F_ProcessNo.StartsWith(prefix))
        //            .Select(it => it.F_ProcessNo)
        //            .ToListAsync();

        //        int maxSeq = 0;
        //        foreach (var no in existingNos)
        //        {
        //            if (no.Length > prefix.Length)
        //            {
        //                var suffix = no.Substring(prefix.Length);
        //                if (int.TryParse(suffix, out int seq))
        //                {
        //                    if (seq > maxSeq) maxSeq = seq;
        //                }
        //            }
        //        }
        //        var nextSeq = maxSeq + 1;
        //        processingEntity.F_ProcessNo = prefix + nextSeq.ToString("D3");

        //        // 赋值：商品、规格、计划数、类别、工艺路线、交货日期、优先级、BOM、图纸
        //        processingEntity.F_GoodsId = item.F_CustomerId;         // 商品ID（合同子项的F_CustomerId即商品ID）
        //        processingEntity.F_PlanQty = item.F_PlanNum;             // 计划数
        //        processingEntity.F_Type = item.F_Category;               // 类别
        //        processingEntity.F_RoutingId = item.F_RoutingId;         // 工艺路线
        //        processingEntity.F_DeliveryDate = item.F_DeliveryDate;   // 交货日期
        //        processingEntity.F_Priority = item.F_Priority;           // 优先级
        //        processingEntity.F_BOMId = item.F_BomId;                 // BOM
        //        processingEntity.F_DrawingFile = item.F_DrawingFiles;    // 图纸

        //        // 其他属性
        //        processingEntity.F_DoorPlateThickness = item.F_DoorPlateThickness;
        //        processingEntity.F_BoxPlateThickness = item.F_BoxPlateThickness;
        //        processingEntity.F_InstallOrBeam = item.F_InstallOrBeam;
        //        processingEntity.F_LockSet = item.F_LockSet;
        //        processingEntity.F_Hinge = item.F_Hinge;
        //        processingEntity.F_Color = item.F_Color;

        //        // 赋值工艺路线的XML
        //        if (!string.IsNullOrEmpty(processingEntity.F_RoutingId))
        //        {
        //            var routingEntity = await _repositoryRouting.GetFirstAsync(it => it.DeleteMark == null && it.id == processingEntity.F_RoutingId);
        //            if (routingEntity != null)
        //            {
        //                processingEntity.F_XML = routingEntity.F_XML;
        //            }
        //        }

        //        // 将用料清单转为加工单用料清单（AProdBomItemEntity）
        //        if (item.ACtcContractItemGoodList != null)
        //        {
        //            var aProdBomItemEntityList = new List<AProdBomItemEntity>();
        //            foreach (var itemGood in item.ACtcContractItemGoodList)
        //            {
        //                var bomItemEntity = new AProdBomItemEntity();
        //                bomItemEntity.F_Id = SnowflakeIdHelper.NextId();
        //                bomItemEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        //                bomItemEntity.F_ProcessingId = processingEntity.id;
        //                bomItemEntity.F_GoodsId = itemGood.F_GoodsId;
        //                bomItemEntity.F_Unit = itemGood.F_Unit;
        //                bomItemEntity.F_StockInProcess = itemGood.F_StockInProcess;
        //                bomItemEntity.F_SortCode = itemGood.F_SortCode;
        //                bomItemEntity.F_Description = itemGood.F_Description;

        //                aProdBomItemEntityList.Add(bomItemEntity);
        //            }
        //            processingEntity.AProdBomItemList = aProdBomItemEntityList;
        //        }

        //        await _repositoryProcessing.AsSugarClient().InsertNav(processingEntity)
        //            .Include(it => it.AProdBomItemList)
        //            .ExecuteCommandAsync();
        //    }
        //}
    }

    /// <summary>
    /// 获取a_ctc_contract无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(ACtcContractListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aCtcContractItemAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<ACtcContractListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_ctc_contract"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_ctc_contract_item"))) aCtcContractItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_ctc_contract_item"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcContractEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcContractItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldf4dfaf-", entityInfo, 1);
        List<IConditionalModel> aCtcContractItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);


        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcContractEntity>();
        var F_CustomerIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_CustomerId").DbColumnName;
        var startF_PrepayAmount = input.F_PrepayAmount?.FirstOrDefault()?.ParseToDecimal() == null ? decimal.MinValue : input.F_PrepayAmount?.First();
        var endF_PrepayAmount = input.F_PrepayAmount?.LastOrDefault()?.ParseToDecimal() == null ? decimal.MaxValue : input.F_PrepayAmount?.Last();
        var F_SalesmanIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_SalesmanId").DbColumnName;

        var list = await _repository.AsQueryable()
            .Includes(x => x.ACtcContractItemList
            .Where(aCtcContractItemConditionalModel)
            .Where(it => it.DeleteMark  == null)
            .Select(it => new ACtcContractItemEntity()
            {
                F_CustomerId = it.F_CustomerId,
                F_Price = it.F_Price,
                F_SaleQty = it.F_SaleQty,
                F_Discount = it.F_Discount,
                F_Description = it.F_Description,
                F_DoorPlateThickness = it.F_DoorPlateThickness,
                F_BoxPlateThickness = it.F_BoxPlateThickness,
                F_InstallOrBeam = it.F_InstallOrBeam,
                F_LockSet = it.F_LockSet,
                F_Hinge = it.F_Hinge,
                F_Color = it.F_Color,
                F_Material = it.F_Material,
                F_F_AuditStatus = it.F_F_AuditStatus,
                F_Files = it.F_Files,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .ToList())
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContractCode), it => it.F_ContractCode.Contains(input.F_ContractCode))
            .WhereIF(!string.IsNullOrEmpty(input.F_CustomerCode), it => it.F_CustomerCode.Contains(input.F_CustomerCode))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_CustomerIdDbColumnName, input.F_CustomerId))
            .WhereIF(input.F_PrepayAmount?.Count() > 0, it => SqlFunc.Between(it.F_PrepayAmount, startF_PrepayAmount, endF_PrepayAmount))
            .WhereIF(input.F_DeliveryDate?.Count() > 0, it => SqlFunc.Between(it.F_DeliveryDate, input.F_DeliveryDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_DeliveryDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_SalesmanIdDbColumnName, input.F_SalesmanId))
            .WhereIF(!string.IsNullOrEmpty(input.F_AuditStatus), it => it.F_AuditStatus.Contains(input.F_AuditStatus))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .Where(authorizeWhere)
            .WhereIF(aCtcContractItemAuthorizeWhere?.Count > 0, it => it.ACtcContractItemList.Any(aCtcContractItemAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aCtcContractItemConditionalModel.Count > 0, it => it.ACtcContractItemList.Any(aCtcContractItemConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new ACtcContractListOutput
            {
                id = it.id,
                F_ContractCode = it.F_ContractCode,
                F_CustomerCode = it.F_CustomerCode,
                F_CustomerId = it.F_CustomerId,
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
                F_PrepayAmount = it.F_PrepayAmount,
                F_PaymentCycle = it.F_PaymentCycle,
                F_SaleTotal = it.F_SaleTotal.ToString(),
                F_ContractAmount = it.F_ContractAmount,
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_IsTaxable = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsTaxable) == 1, "是", "否"),
                F_SalesmanId = it.F_SalesmanId,
                F_AuditStatus = it.F_AuditStatus,
                F_Description = it.F_Description,
                F_CreatorUserId = it.F_CreatorUserId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableFieldf4dfaf = it.ACtcContractItemList.Adapt<List<ACtcContractItemListOutput>>(),
            })
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 客户ID
            var F_CustomerIdData = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2009458830353764352", "F_Id", "F_CustomerName", "");
            if(item.F_CustomerId != null)
            {
                item.F_CustomerId = F_CustomerIdData.Find(it => it.id.Equals(item.F_CustomerId))?.fullName;
            }

            if (item.F_ContactId != null)
            {
                var contactData = await _repositorylxr.AsQueryable().FirstAsync(c => c.F_Id.Equals(item.F_ContactId));
                item.F_ContactId = contactData?.F_ContactName;
            }

            // 业务员
            if (item.F_SalesmanId != null)
            {
                item.F_SalesmanId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_SalesmanId)))?.RealName;
            }

            // 创建人员
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;

        });

        await _repository.AsSugarClient().ThenMapperAsync(list.SelectMany(it => it.tableFieldf4dfaf), async aCtcContractItem =>
        {
            var aCtcContract = list.ToList().Find(it => it.id.Equals(aCtcContractItem.F_ContractId));
            var linkageParameters = new List<DataInterfaceParameter>();

            if(aCtcContractItem.F_Files != null)
            {
                aCtcContractItem.F_Files = aCtcContractItem.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                aCtcContractItem.F_Files = new List<FileControlsModel>();
            }

            // 创建时间
            aCtcContractItem.F_CreatorTime = aCtcContractItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcContractEntity>(new ACtcContractEntity()));
        ConfigModel tableFieldf4dfafConfig = new ConfigModel
        {
            tableName = "a_ctc_contract_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择合同商品",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcContractItemEntity>(new ACtcContractItemEntity())
        };
        FieldsModel tableFieldf4dfaf = new FieldsModel()
        {
            __config__ = tableFieldf4dfafConfig,
            __vModel__ = "tableFieldf4dfaf"
        };
        fieldList.Add(tableFieldf4dfaf);

        resData = await _controlParsing.GetParsDataList(resData, "F_ContactId,tableFieldf4dfaf-F_CustomerId", "popupSelect", _userManager.TenantId, fieldList);

        list = resData.ToObject<List<ACtcContractListOutput>>(CommonConst.options);
        return list;
    }

    /// <summary>
    /// 导出a_ctc_contract.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] ACtcContractListQueryInput input)
    {
        var exportData = new List<ACtcContractListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<ACtcContractListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName, input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新a_ctc_contract.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] ACtcContractUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACtcContractEntity>();
        // 确保使用正确的主键
        entity.id = id;

        // 读取更新前的主表数据，用于比较并记录变更
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);

        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ACtcContractEntity>(new ACtcContractEntity()));
        ConfigModel tableFieldf4dfafConfig = new ConfigModel
        {
            tableName = "a_ctc_contract_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择合同商品",
            children = ExportImportDataHelper.GetTemplateParsing<ACtcContractItemEntity>(new ACtcContractItemEntity())
        };
        FieldsModel tableFieldf4dfafModel = new FieldsModel()
        {
            __config__ = tableFieldf4dfafConfig,
            __vModel__ = "tableFieldf4dfaf"
        };
        fieldList.Add(tableFieldf4dfafModel);

        // 先计算将被删除的子项（在执行逻辑删除前获取原有子项ID）
        var existingChildIds = await _repository.AsSugarClient().Queryable<ACtcContractItemEntity>()
            .Where(it => it.F_ContractId == entity.id && it.DeleteMark == null).Select(it => it.F_Id).ToListAsync();
        var incomingChildIds = input.tableFieldf4dfaf?.Select(it => it.F_Id).ToList() ?? new List<string>();
        var deletedChildIds = existingChildIds.Where(e => !incomingChildIds.Contains(e)).ToList();

        // 移除合同订单商品可能被删除数据（逻辑删除）
        if (input.tableFieldf4dfaf != null && input.tableFieldf4dfaf.Any())
            await _repository.AsSugarClient().Deleteable<ACtcContractItemEntity>().Where(it => it.F_ContractId == entity.id && !input.tableFieldf4dfaf.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<ACtcContractItemEntity>().Where(it => it.F_ContractId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增/更新合同订单商品数据
        var aCtcContractItemEntityList = input.tableFieldf4dfaf.Adapt<List<ACtcContractItemEntity>>();

        try
        {
            // 更新主表指定字段
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_ContractCode,
                    it.F_CustomerCode,
                    it.F_CustomerId,
                    it.F_ContactId,
                    it.F_AddressId,
                    it.F_PrepayAmount,
                    it.F_PaymentCycle,
                    it.F_SaleTotal,
                    it.F_ContractAmount,
                    it.F_DeliveryDate,
                    it.F_IsTaxable,
                    it.F_SalesmanId,
                    it.F_AuditStatus,
                    it.F_Description,
                })
                .ExecuteCommandAsync();

            // 记录主表变更日志（如果有差异）
            if (oldEntity != null)
            {
                var mainChanges = new List<string>();
                if (!string.Equals(oldEntity.F_ContractCode, entity.F_ContractCode)) mainChanges.Add($"合同编号: '{oldEntity.F_ContractCode}' 修改为 '{entity.F_ContractCode}'");
                if (!string.Equals(oldEntity.F_CustomerCode, entity.F_CustomerCode)) mainChanges.Add($"客户合同号: '{oldEntity.F_CustomerCode}' 修改为 '{entity.F_CustomerCode}'");
                if (!string.Equals(oldEntity.F_CustomerId, entity.F_CustomerId)) mainChanges.Add($"客户: '{oldEntity.F_CustomerId}' 修改为 '{entity.F_CustomerId}'");
                if (!string.Equals(oldEntity.F_ContactId, entity.F_ContactId)) mainChanges.Add($"联系人: '{oldEntity.F_ContactId}' 修改为 '{entity.F_ContactId}'");
                if (!string.Equals(oldEntity.F_AddressId, entity.F_AddressId)) mainChanges.Add($"地址: '{oldEntity.F_AddressId}' 修改为 '{entity.F_AddressId}'");
                if (oldEntity.F_PrepayAmount != entity.F_PrepayAmount) mainChanges.Add($"预付款: '{oldEntity.F_PrepayAmount}' 修改为 '{entity.F_PrepayAmount}'");
                if (oldEntity.F_PaymentCycle != entity.F_PaymentCycle) mainChanges.Add($"付款周期（天）: '{oldEntity.F_PaymentCycle}' 修改为 '{entity.F_PaymentCycle}'");
                if (oldEntity.F_SaleTotal != entity.F_SaleTotal) mainChanges.Add($"销售总数: '{oldEntity.F_SaleTotal}' 修改为 '{entity.F_SaleTotal}'");
                if (oldEntity.F_ContractAmount != entity.F_ContractAmount) mainChanges.Add($"合同金额: '{oldEntity.F_ContractAmount}' 修改为 '{entity.F_ContractAmount}'");
                if (oldEntity.F_DeliveryDate != entity.F_DeliveryDate) mainChanges.Add($"交货日期: '{oldEntity.F_DeliveryDate}' 修改为 '{entity.F_DeliveryDate}'");
                if (!string.Equals(oldEntity.F_IsTaxable, entity.F_IsTaxable)) mainChanges.Add($"是否含税: '{oldEntity.F_IsTaxable}' 修改为 '{entity.F_IsTaxable}'");
                if (!string.Equals(oldEntity.F_SalesmanId, entity.F_SalesmanId)) mainChanges.Add($"业务员: '{oldEntity.F_SalesmanId}' 修改为 '{entity.F_SalesmanId}'");
                if (!string.Equals(oldEntity.F_AuditStatus, entity.F_AuditStatus)) mainChanges.Add($"审核状态: '{oldEntity.F_AuditStatus}' 修改为 '{entity.F_AuditStatus}'");
                if (!string.Equals(oldEntity.F_Description, entity.F_Description)) mainChanges.Add($"备注: '{oldEntity.F_Description}' 修改为 '{entity.F_Description}'");

                if (mainChanges.Any())
                {
                    var mainLog = new ACtcContractlogEntity
                    {
                        id = SnowflakeIdHelper.NextId(),
                        F_ContractId = entity.id,
                        F_Title = "更新合同主表",
                        F_Content = string.Join("；", mainChanges),
                        F_CreatorUserId = _userManager.UserId,
                        F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                    };
                    await _repositorylog.AsSugarClient().Insertable(mainLog).ExecuteCommandAsync();
                }
            }

            // 记录被删除的子项（逻辑删除）
            if (deletedChildIds != null && deletedChildIds.Any())
            {
                var delLog = new ACtcContractlogEntity
                {
                    id = SnowflakeIdHelper.NextId(),
                    F_ContractId = entity.id,
                    F_Title = "删除合同商品",
                    F_Content = "删除子项ID: " + string.Join(",", deletedChildIds),
                    F_CreatorUserId = _userManager.UserId,
                    F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                };
                await _repositorylog.AsSugarClient().Insertable(delLog).ExecuteCommandAsync();
            }

            // 新增或更新子项，同时记录相应日志
            if (aCtcContractItemEntityList != null)
            {
                foreach (var item in aCtcContractItemEntityList)
                {
                    if (item.F_Id.IsNullOrEmpty())
                    {
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_ContractId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();

                        var insertLog = new ACtcContractlogEntity
                        {
                            id = SnowflakeIdHelper.NextId(),
                            F_ContractId = entity.id,
                            F_Title = "新增合同商品",
                            F_Content = $"子项ID:{item.F_Id}，商品备注:{item.F_Description}，单价:{item.F_Price}，数量:{item.F_SaleQty}",
                            F_CreatorUserId = _userManager.UserId,
                            F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                        };
                        await _repositorylog.AsSugarClient().Insertable(insertLog).ExecuteCommandAsync();
                    }
                    else
                    {
                        // 记录子项修改前后差异
                        var oldChild = await _repository.AsSugarClient().Queryable<ACtcContractItemEntity>().FirstAsync(it => it.F_Id.Equals(item.F_Id));
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_ContractId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

                        var childChanges = new List<string>();
                        if (oldChild != null)
                        {
                            if (oldChild.F_Price != item.F_Price) childChanges.Add($"单价: '{oldChild.F_Price}' 修改为 '{item.F_Price}'");
                            if (oldChild.F_SaleQty != item.F_SaleQty) childChanges.Add($"数量: '{oldChild.F_SaleQty}' 修改为 '{item.F_SaleQty}'");
                            if (oldChild.F_Discount != item.F_Discount) childChanges.Add($"折扣: '{oldChild.F_Discount}' 修改为 '{item.F_Discount}'");
                            if (!string.Equals(oldChild.F_Description, item.F_Description)) childChanges.Add($"备注: '{oldChild.F_Description}' 修改为 '{item.F_Description}'");
                        }

                        if (childChanges.Any())
                        {
                            var updateLog = new ACtcContractlogEntity
                            {
                                id = SnowflakeIdHelper.NextId(),
                                F_ContractId = entity.id,
                                F_Title = "修改合同商品",
                                F_Content = $"子项ID:{item.F_Id}，" + string.Join("；", childChanges),
                                F_CreatorUserId = _userManager.UserId,
                                F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                            };
                            await _repositorylog.AsSugarClient().Insertable(updateLog).ExecuteCommandAsync();
                        }
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
    /// 删除a_ctc_contract.
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
    /// 批量删除a_ctc_contract.
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
            // 批量删除a_ctc_contract
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_ctc_contract详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.ACtcContractItemList)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.id == id)
            .Select(it => new ACtcContractDetailOutput
            {
                id = it.id,
                F_ContractCode = it.F_ContractCode,
                F_CustomerCode = it.F_CustomerCode,
                F_CustomerId = it.F_CustomerId,
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
                F_PaymentCycle = it.F_PaymentCycle,
                F_PrepayAmount = it.F_PrepayAmount,
                F_SaleTotal = it.F_SaleTotal.ToString(),
                F_ContractAmount = it.F_ContractAmount,
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_IsTaxable = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsTaxable) == 1, "是", "否"),
                F_SalesmanId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_SalesmanId)).Select(u => u.RealName),
                F_AuditStatus = it.F_AuditStatus,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableFieldf4dfaf = it.ACtcContractItemList.Adapt<List<ACtcContractItemDetailOutput>>(),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 客户ID
            if(item.F_CustomerId != null)
            {
                var F_CustomerIdData = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2009458830353764352", "F_Id", "F_CustomerName", "");
                item.F_CustomerId = F_CustomerIdData.Find(it => it.id.Equals(item.F_CustomerId))?.fullName;
            }

            if (item.F_ContactId != null)
            {
                var contactData = await _repositorylxr.AsQueryable().FirstAsync(c => c.F_Id.Equals(item.F_ContactId));
                item.F_ContactId = contactData?.F_ContactName;
            }

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFieldf4dfaf), async aCtcContractItem =>
        {
            var aCtcContract = data.ToList().Find(it => it.id.Equals(aCtcContractItem.F_ContractId));
            var linkageParameters = new List<DataInterfaceParameter>();

            // 材质
            if (aCtcContractItem.F_Material != null)
            {
                aCtcContractItem.F_Material = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(aCtcContractItem.F_Material) && it.DictionaryTypeId.Equals("2016346179754921984")).Select(it => it.FullName).FirstAsync();
            }
            var unit = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(it => it.id.Equals(aCtcContractItem.F_CustomerId)).Select(it => it.F_Unit).FirstAsync();

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(unit))
            {
                var F_UnitIdCascader = unit.ToObject<List<string>>();
                aCtcContractItem.F_UnitTwo = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));

            }
            // 创建人员
            aCtcContractItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aCtcContractItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aCtcContractItem.F_CreatorTime = aCtcContractItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcContractEntity>(new ACtcContractEntity()));
        ConfigModel tableFieldf4dfafConfig = new ConfigModel
        {
            tableName = "a_ctc_contract_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择合同商品",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcContractItemEntity>(new ACtcContractItemEntity())
        };
        FieldsModel tableFieldf4dfaf = new FieldsModel()
        {
            __config__ = tableFieldf4dfafConfig,
            __vModel__ = "tableFieldf4dfaf"
        };
        fieldList.Add(tableFieldf4dfaf);

        resData = await _controlParsing.GetParsDataList(resData,"F_ContactId,tableFieldf4dfaf-F_CustomerId","popupSelect",_userManager.TenantId, fieldList);

        // 获取操作日志并加入返回结果（按创建时间降序）
        var logs = await _repositorylog.AsQueryable()
            .Where(it => it.F_ContractId == id)
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .Select(it => new ACtcContractlogEntity
            {
                id = it.id,
                F_ContractId = it.F_ContractId,
                F_Title = it.F_Title,
                F_Content = it.F_Content,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .ToListAsync();

        var first = resData.FirstOrDefault();
        if (first != null)
        {
            first["logs"] = logs;
        }

        return first;
    }


    /// <summary>
    /// 根据客户 ID 获取默认联系人和默认地址（F_IsDefault == 1）
    /// </summary>
    /// <param name="customerId">F_CustomerId</param>
    /// <returns>返回包含 contacts 与 addresses 的对象</returns>
    [HttpGet("Defaults/Customer/{customerId}")]
    public async Task<dynamic> GetDefaultsByCustomer(string customerId)
    {
        if (string.IsNullOrEmpty(customerId)) return null;

        // 默认联系人：返回 F_Id 和 F_ContactName
        var contacts = await _repositorylxr.AsQueryable()
            .Where(it => it.F_CustomerId == customerId && it.F_IsDefault == "1" && it.DeleteMark == null)
            .Select(it => new { it.F_Id, it.F_ContactName })
            .ToListAsync();

        // 默认地址：返回 F_Id、F_Region、F_Address
        var addresses = await _repositorydz.AsQueryable()
            .Where(it => it.F_CustomerId == customerId && it.F_IsDefault == "1" && it.DeleteMark == null)
            .Select(it => new { it.F_Id, it.F_Region, it.F_Address })
            .ToListAsync();

        return new {
            contacts = contacts,
            addresses = addresses
        };
    }


}
