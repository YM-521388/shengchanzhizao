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
using JNPF.example.Entitys.Dto.ACtcQuote;
using JNPF.example.Entitys.Dto.ACtcQuoteItem;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_ctc_quote.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "ACtcQuote", Order = 200)]
[Route("api/example/[controller]")]
public class ACtcQuoteService : IACtcQuoteService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<ACtcQuoteEntity> _repository;
    private readonly ISqlSugarRepository<ACtcAddressEntity> _repositorydz;
    private readonly ISqlSugarRepository<ACtcContactEntity> _repositorylxr;
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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"报价单号\",\"field\":\"F_QuoteCode\"},{\"value\":\"客户\",\"field\":\"F_CustomerId\"},{\"value\":\"联系人\",\"field\":\"F_ContactId\"},{\"value\":\"客户地址ID\",\"field\":\"F_AddressId\"},{\"value\":\"报价金额\",\"field\":\"F_QuoteAmount\"},{\"value\":\"商品总数\",\"field\":\"F_GoodsTotal\"},{\"value\":\"交货日期\",\"field\":\"F_DeliveryDate\"},{\"value\":\"报价日期\",\"field\":\"F_QuoteDate\"},{\"value\":\"业务员\",\"field\":\"F_SalesmanId\"},{\"value\":\"报价状态\",\"field\":\"F_QuoteStatus\"},{\"value\":\"审核状态\",\"field\":\"F_AuditStatus\"},{\"value\":\"选择报价单商品-商品ID\",\"field\":\"tableFielddc29f7-F_CustomerId\"},{\"value\":\"选择报价单商品-单价\",\"field\":\"tableFielddc29f7-F_Price\"},{\"value\":\"选择报价单商品-销售数量\",\"field\":\"tableFielddc29f7-F_SaleQty\"},{\"value\":\"选择报价单商品-折扣\",\"field\":\"tableFielddc29f7-F_Discount\"},{\"value\":\"选择报价单商品-优惠金额\",\"field\":\"tableFielddc29f7-F_DiscountAmount\"},{\"value\":\"选择报价单商品-商品备注\",\"field\":\"tableFielddc29f7-F_Description\"},{\"value\":\"选择报价单商品-附件\",\"field\":\"tableFielddc29f7-F_Files\"},{\"value\":\"选择报价单商品-创建人员\",\"field\":\"tableFielddc29f7-F_CreatorUserId\"},{\"value\":\"选择报价单商品-创建时间\",\"field\":\"tableFielddc29f7-F_CreatorTime\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="ACtcQuoteService"/>类型的新实例.
    /// </summary>
    public ACtcQuoteService(
        ISqlSugarRepository<ACtcQuoteEntity> repository,
        ISqlSugarRepository<ACtcAddressEntity> repositorydz,
        ISqlSugarRepository<ACtcContactEntity> repositorylxr,
         ISqlSugarRepository<AGoodsEntity> repositorysp,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repository = repository;
        _repositorydz = repositorydz;
        _repositorysp = repositorysp;
        _repositorylxr = repositorylxr;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_ctc_quote.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.ACtcQuoteItemList)
            .Where(it => it.DeleteMark == null)
            .Select(it => new ACtcQuoteEntity
            {
                id = it.id,
                F_QuoteCode = it.F_QuoteCode,
                F_CustomerId = it.F_CustomerId,
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
                F_QuoteAmount = it.F_QuoteAmount,
                F_GoodsTotal = it.F_GoodsTotal,
                F_DeliveryDate = it.F_DeliveryDate,
                F_QuoteDate = it.F_QuoteDate,
                F_SalesmanId = it.F_SalesmanId,
                F_QuoteStatus = it.F_QuoteStatus,
                F_AuditStatus = it.F_AuditStatus,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                ACtcQuoteItemList = it.ACtcQuoteItemList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<ACtcQuoteInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFielddc29f7, async aCtcQuoteItem =>
        {
            // 创建人员
            aCtcQuoteItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aCtcQuoteItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aCtcQuoteItem.F_CreatorTime = aCtcQuoteItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            var goodEntity = await _repositorysp.GetFirstAsync(it => it.id.Equals(aCtcQuoteItem.F_CustomerId));

            var unit = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(it => it.id.Equals(aCtcQuoteItem.F_CustomerId)).Select(it => it.F_Unit).FirstAsync();

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(unit))
            {
                var F_UnitIdCascader = unit.ToObject<List<string>>();
                aCtcQuoteItem.F_UnitTwo = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));

            }

            aCtcQuoteItem.F_GoodsNo = goodEntity.F_GoodsNo;

            aCtcQuoteItem.F_Specification = goodEntity.F_Specification;
        });
        return data;
    }

    /// <summary>
    /// 获取a_ctc_quote列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] ACtcQuoteListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<ACtcQuoteListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_ctc_quote"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcQuoteEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcQuoteEntity>();
        var F_CustomerIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_CustomerId").DbColumnName;
        var F_SalesmanIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_SalesmanId").DbColumnName;
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_QuoteCode), it => it.F_QuoteCode.Contains(input.F_QuoteCode))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_CustomerIdDbColumnName, input.F_CustomerId))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_SalesmanIdDbColumnName, input.F_SalesmanId))
            .WhereIF(!string.IsNullOrEmpty(input.F_QuoteStatus), it => it.F_QuoteStatus.Equals(input.F_QuoteStatus))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new ACtcQuoteListOutput
            {
                id = it.id,
                F_QuoteCode = it.F_QuoteCode,
                F_CustomerId = it.F_CustomerId,
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
                F_QuoteAmount = it.F_QuoteAmount,
                F_GoodsTotal = it.F_GoodsTotal.ToString(),
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_QuoteDate = it.F_QuoteDate.Value.ToString("yyyy-MM-dd"),
                F_SalesmanId = it.F_SalesmanId,
                F_QuoteStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_QuoteStatus) && dic.DictionaryTypeId.Equals("2028350323948654592")).Select(dic => dic.FullName),
                F_QuoteStatusNo = it.F_QuoteStatus,
                F_AuditStatus = it.F_AuditStatus,
                F_CreatorUserId = it.F_CreatorUserId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),

            })
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 客户ID
            var F_CustomerIdData = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2009458830353764352", "F_Id", "F_CustomerName", "");
            if(item.F_CustomerId != null)
            {
                item.F_CustomerId = F_CustomerIdData.Find(it => it.id.Equals(item.F_CustomerId))?.fullName;
            }

            // 联系人（把 id 转为联系人名）
            if (item.F_ContactId != null)
            {
                var contactData = await _repositorylxr.AsQueryable().FirstAsync(c => c.F_Id.Equals(item.F_ContactId));
                item.F_ContactId = contactData?.F_ContactName;
            }

            // 客户地址ID
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
            if(item.F_AddressId != null)
            {
                item.F_AddressId = F_AddressIdData.Find(it => it.id.Equals(item.F_AddressId))?.fullName;
            }

            // 业务员
            if(item.F_SalesmanId != null)
            {
                item.F_SalesmanId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_SalesmanId)))?.RealName;
            }
            // 创建人员
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;

        });

        var resData = data.list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcQuoteEntity>(new ACtcQuoteEntity()));

        resData = await _controlParsing.GetParsDataList(resData, "F_ContactId", "popupSelect", _userManager.TenantId, fieldList);

        data.list = resData.ToObject<List<ACtcQuoteListOutput>>(CommonConst.options);
        // 按创建时间降序返回给前端（从字符串转换为 DateTime，格式为 "yyyy-MM-dd HH:mm:ss"）
        data.list = data.list.OrderByDescending(it =>
        {
            if (DateTime.TryParse(it.F_CreatorTime, out var dt)) return dt;
            // 若解析失败则放到最末尾
            return DateTime.MinValue;
        }).ToList();
        return PageResult<ACtcQuoteListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_ctc_quote.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] ACtcQuoteCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        // 若前端未传入 F_QuoteCode，则按规则自动生成：前缀 BJ + 当日日期(yyyyMMdd) + 当日序号(3位，左补0)
        if (string.IsNullOrWhiteSpace(input.F_QuoteCode))
        {
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var prefix = $"BJ{datePart}";
            // 统计数据库中已有相同前缀的编码数量，序号从 1 开始
            var existCount = await _repository.AsQueryable()
                .Where(it => it.F_QuoteCode != null && it.F_QuoteCode.StartsWith(prefix))
                .CountAsync();
            var seq = existCount + 1;
            input.F_QuoteCode = $"{prefix}{seq.ToString("D3")}";
        }
        var entity = input.Adapt<ACtcQuoteEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ACtcQuoteEntity>(new ACtcQuoteEntity()));
        ConfigModel tableFielddc29f7Config = new ConfigModel
        {
            tableName = "a_ctc_quote_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择报价单商品",
            children = ExportImportDataHelper.GetTemplateParsing<ACtcQuoteItemEntity>(new ACtcQuoteItemEntity())
        };
        FieldsModel tableFielddc29f7Model = new FieldsModel()
        {
            __config__ = tableFielddc29f7Config,
            __vModel__ = "tableFielddc29f7"
        };
        fieldList.Add(tableFielddc29f7Model);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;


        var aCtcQuoteItemEntityList = input.tableFielddc29f7.Adapt<List<ACtcQuoteItemEntity>>();
        if(aCtcQuoteItemEntityList != null)
        {
            foreach (var item in aCtcQuoteItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.ACtcQuoteItemList = aCtcQuoteItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.ACtcQuoteItemList)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取a_ctc_quote无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(ACtcQuoteListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aCtcQuoteItemAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<ACtcQuoteListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_ctc_quote"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_ctc_quote_item"))) aCtcQuoteItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_ctc_quote_item"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcQuoteEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcQuoteItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFielddc29f7-", entityInfo, 1);
        List<IConditionalModel> aCtcQuoteItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);


        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcQuoteEntity>();
        var F_CustomerIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_CustomerId").DbColumnName;
        var F_SalesmanIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_SalesmanId").DbColumnName;

        var list = await _repository.AsQueryable()
            .Includes(x => x.ACtcQuoteItemList
            .Where(aCtcQuoteItemConditionalModel)
            .Where(it => it.DeleteMark  == null)
            .Select(it => new ACtcQuoteItemEntity()
            {
                F_CustomerId = it.F_CustomerId,
                F_Price = it.F_Price,
                F_SaleQty = it.F_SaleQty,
                F_Discount = it.F_Discount,
                F_DiscountAmount = it.F_DiscountAmount,
                F_Description = it.F_Description,
                F_Files = it.F_Files,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .ToList())
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_QuoteCode), it => it.F_QuoteCode.Contains(input.F_QuoteCode))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_CustomerIdDbColumnName, input.F_CustomerId))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_SalesmanIdDbColumnName, input.F_SalesmanId))
            .WhereIF(!string.IsNullOrEmpty(input.F_QuoteStatus), it => it.F_QuoteStatus.Equals(input.F_QuoteStatus))
            .Where(authorizeWhere)
            .WhereIF(aCtcQuoteItemAuthorizeWhere?.Count > 0, it => it.ACtcQuoteItemList.Any(aCtcQuoteItemAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aCtcQuoteItemConditionalModel.Count > 0, it => it.ACtcQuoteItemList.Any(aCtcQuoteItemConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new ACtcQuoteListOutput
            {
                id = it.id,
                F_QuoteCode = it.F_QuoteCode,
                F_CustomerId = it.F_CustomerId,
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
                F_QuoteAmount = it.F_QuoteAmount,
                F_GoodsTotal = it.F_GoodsTotal.ToString(),
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_QuoteDate = it.F_QuoteDate.Value.ToString("yyyy-MM-dd"),
                F_SalesmanId = it.F_SalesmanId,
                F_QuoteStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_QuoteStatus) && dic.DictionaryTypeId.Equals("2028350323948654592")).Select(dic => dic.FullName),
                F_QuoteStatusNo = it.F_QuoteStatus,
                F_AuditStatus = it.F_AuditStatus,
                F_CreatorUserId = it.F_CreatorUserId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableFielddc29f7 = it.ACtcQuoteItemList.Adapt<List<ACtcQuoteItemListOutput>>(),
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

            // 客户地址ID
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
            if(item.F_AddressId != null)
            {
                item.F_AddressId = F_AddressIdData.Find(it => it.id.Equals(item.F_AddressId))?.fullName;
            }

            // 业务员
            if(item.F_SalesmanId != null)
            {
                item.F_SalesmanId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_SalesmanId)))?.RealName;
            }

            // 创建人员
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;

        });

        await _repository.AsSugarClient().ThenMapperAsync(list.SelectMany(it => it.tableFielddc29f7), async aCtcQuoteItem =>
        {
            var aCtcQuote = list.ToList().Find(it => it.id.Equals(aCtcQuoteItem.F_QuoteId));
            var linkageParameters = new List<DataInterfaceParameter>();

            if(aCtcQuoteItem.F_Files != null)
            {
                aCtcQuoteItem.F_Files = aCtcQuoteItem.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                aCtcQuoteItem.F_Files = new List<FileControlsModel>();
            }

            // 创建时间
            aCtcQuoteItem.F_CreatorTime = aCtcQuoteItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcQuoteEntity>(new ACtcQuoteEntity()));
        ConfigModel tableFielddc29f7Config = new ConfigModel
        {
            tableName = "a_ctc_quote_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择报价单商品",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcQuoteItemEntity>(new ACtcQuoteItemEntity())
        };
        FieldsModel tableFielddc29f7 = new FieldsModel()
        {
            __config__ = tableFielddc29f7Config,
            __vModel__ = "tableFielddc29f7"
        };
        fieldList.Add(tableFielddc29f7);

        resData = await _controlParsing.GetParsDataList(resData, "F_ContactId,tableFielddc29f7-F_CustomerId", "popupSelect", _userManager.TenantId, fieldList);

        list = resData.ToObject<List<ACtcQuoteListOutput>>(CommonConst.options);
        return list;
    }

    /// <summary>
    /// 导出a_ctc_quote.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] ACtcQuoteListQueryInput input)
    {
        var exportData = new List<ACtcQuoteListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<ACtcQuoteListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName, input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新a_ctc_quote.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] ACtcQuoteUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACtcQuoteEntity>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ACtcQuoteEntity>(new ACtcQuoteEntity()));
        ConfigModel tableFielddc29f7Config = new ConfigModel
        {
            tableName = "a_ctc_quote_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择报价单商品",
            children = ExportImportDataHelper.GetTemplateParsing<ACtcQuoteItemEntity>(new ACtcQuoteItemEntity())
        };
        FieldsModel tableFielddc29f7Model = new FieldsModel()
        {
            __config__ = tableFielddc29f7Config,
            __vModel__ = "tableFielddc29f7"
        };
        fieldList.Add(tableFielddc29f7Model);

        // 移除合同报价单商品可能被删除数据
        if (input.tableFielddc29f7 !=null && input.tableFielddc29f7.Any())
            await _repository.AsSugarClient().Deleteable<ACtcQuoteItemEntity>().Where(it => it.F_QuoteId == entity.id && !input.tableFielddc29f7.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<ACtcQuoteItemEntity>().Where(it => it.F_QuoteId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增合同报价单商品新数据
        var aCtcQuoteItemEntityList = input.tableFielddc29f7.Adapt<List<ACtcQuoteItemEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_QuoteCode,
                    it.F_CustomerId,
                    it.F_ContactId,
                    it.F_AddressId,
                    it.F_QuoteAmount,
                    it.F_GoodsTotal,
                    it.F_DeliveryDate,
                    it.F_QuoteDate,
                    it.F_SalesmanId,
                    it.F_QuoteStatus,
                    it.F_AuditStatus,
                    it.F_Description,
                })
                .ExecuteCommandAsync();

            if(aCtcQuoteItemEntityList != null)
            {
                foreach (var item in aCtcQuoteItemEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_QuoteId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_QuoteId = entity.id;
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
    /// 修改报价状态.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("State/{id}")]
    [UnitOfWork]
    public async Task UpdateState(string id)
    {
        var entity = await _repository.GetFirstAsync(it => it.id == id);
        entity.F_QuoteStatus = "1";

        await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new {
                it.F_QuoteStatus,
            })
            .ExecuteCommandAsync();

    }


    /// <summary>
    /// 删除a_ctc_quote.
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
    /// 批量删除a_ctc_quote.
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
            // 批量删除a_ctc_quote
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_ctc_quote详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.ACtcQuoteItemList)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.id == id)
            .Select(it => new ACtcQuoteDetailOutput
            {
                id = it.id,
                F_QuoteCode = it.F_QuoteCode,
                F_CustomerId = it.F_CustomerId,
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
                F_QuoteAmount = it.F_QuoteAmount,
                F_GoodsTotal = it.F_GoodsTotal.ToString(),
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_QuoteDate = it.F_QuoteDate.Value.ToString("yyyy-MM-dd"),
                F_SalesmanId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_SalesmanId)).Select(u => u.RealName),
                F_QuoteStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_QuoteStatus) && dic.DictionaryTypeId.Equals("2028350323948654592")).Select(dic => dic.FullName),
                F_AuditStatus = it.F_AuditStatus,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableFielddc29f7 = it.ACtcQuoteItemList.Adapt<List<ACtcQuoteItemDetailOutput>>(),
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

            // 联系人（把 id 转为联系人名）
            if (item.F_ContactId != null)
            {
                var contactData = await _repositorylxr.AsQueryable().FirstAsync(c => c.F_Id.Equals(item.F_ContactId));
                item.F_ContactId = contactData?.F_ContactName;
            }

            // 客户地址ID
            if (item.F_AddressId != null)
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

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFielddc29f7), async aCtcQuoteItem =>
        {
            var aCtcQuote = data.ToList().Find(it => it.id.Equals(aCtcQuoteItem.F_QuoteId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aCtcQuoteItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aCtcQuoteItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aCtcQuoteItem.F_CreatorTime = aCtcQuoteItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            var unit = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(it => it.id.Equals(aCtcQuoteItem.F_CustomerId)).Select(it => it.F_Unit).FirstAsync();

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(unit))
            {
                var F_UnitIdCascader = unit.ToObject<List<string>>();
                aCtcQuoteItem.F_UnitTwo = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));

            }
            if(aCtcQuoteItem.F_Discount == null)
            {
                aCtcQuoteItem.F_DiscountAmount = 0;
            }
            else
            {
                if(aCtcQuoteItem.F_SaleQty != null && aCtcQuoteItem.F_Price != null){
                    aCtcQuoteItem.F_DiscountAmount = (aCtcQuoteItem.F_SaleQty * aCtcQuoteItem.F_Price) * (aCtcQuoteItem.F_Discount / 100);
                }
            }
            // 商品编号（根据 F_CustomerId 从 AGoodsEntity 表获取 F_GoodsNo）
            if (aCtcQuoteItem.F_CustomerId != null)
            {
                var goodsData = await _repositorysp.AsQueryable().FirstAsync(g => g.id.Equals(aCtcQuoteItem.F_CustomerId));
                aCtcQuoteItem.F_CustomerId = goodsData?.F_GoodsName + "-" + goodsData?.F_GoodsNo;
            }
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcQuoteEntity>(new ACtcQuoteEntity()));
        ConfigModel tableFielddc29f7Config = new ConfigModel
        {
            tableName = "a_ctc_quote_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择报价单商品",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcQuoteItemEntity>(new ACtcQuoteItemEntity())
        };
        FieldsModel tableFielddc29f7 = new FieldsModel()
        {
            __config__ = tableFielddc29f7Config,
            __vModel__ = "tableFielddc29f7"
        };
        fieldList.Add(tableFielddc29f7);

        resData = await _controlParsing.GetParsDataList(resData,"F_ContactId,tableFielddc29f7-F_CustomerId","popupSelect",_userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
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
            .Select(it => new { it.F_Id, it.F_Region, F_Address = it.F_Address ?? "" })
            .ToListAsync();

        return new
        {
            contacts = contacts,
            addresses = addresses
        };
    }
}
