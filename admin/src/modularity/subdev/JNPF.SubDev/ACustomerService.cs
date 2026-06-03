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
using JNPF.example.Entitys.Dto.ACustomer;
using JNPF.example.Entitys.Dto.ACtcAddress;
using JNPF.example.Entitys.Dto.ACtcContact;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_customer.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "ACustomer", Order = 200)]
[Route("api/example/[controller]")]
public class ACustomerService : IACustomerService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<ACustomerEntity> _repository;
    private readonly ISqlSugarRepository<ACtcContactEntity> _repositorylxr;
    private readonly ISqlSugarRepository<ACtclongEntity> _repositorylog;
    private readonly ISqlSugarRepository<AConfigEntity> _repositorypz;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"名称\",\"field\":\"F_CustomerName\"},{\"value\":\"编号\",\"field\":\"F_CustomerCode\"},{\"value\":\"二维码\",\"field\":\"F_QRCode\"},{\"value\":\"公海客户\",\"field\":\"F_IsPublic\"},{\"value\":\"客户标签\",\"field\":\"F_CustomerTags\"},{\"value\":\"客户共享\",\"field\":\"F_ShareUsers\"},{\"value\":\"关注\",\"field\":\"F_IsFollow\"},{\"value\":\"状态\",\"field\":\"F_State\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"附件\",\"field\":\"F_Files\"},{\"value\":\"领用人员\",\"field\":\"F_RequisitionUserId\"},{\"value\":\"领用时间\",\"field\":\"F_RequisitionTime\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"},{\"value\":\"客户地址-地区\",\"field\":\"tableField6eed25-F_Region\"},{\"value\":\"客户地址-地址\",\"field\":\"tableField6eed25-F_Address\"},{\"value\":\"客户地址-是否默认\",\"field\":\"tableField6eed25-F_IsDefault\"},{\"value\":\"客户地址-创建时间\",\"field\":\"tableField6eed25-F_CreatorTime\"},{\"value\":\"客户联系人-联系人\",\"field\":\"tableFieldddabab-F_ContactName\"},{\"value\":\"客户联系人-联系人电话\",\"field\":\"tableFieldddabab-F_ContactPhone\"},{\"value\":\"客户联系人-是否默认\",\"field\":\"tableFieldddabab-F_IsDefault\"},{\"value\":\"客户联系人-创建时间\",\"field\":\"tableFieldddabab-F_CreatorTime\"}]".ToList<ParamsModel>();


    /// <summary>
    /// 初始化一个<see cref="ACustomerService"/>类型的新实例.
    /// </summary>
    public ACustomerService(
        ISqlSugarRepository<ACustomerEntity> repository,
        ISqlSugarRepository<ACtcContactEntity> repositorylxr,
        ISqlSugarRepository<ACtclongEntity> repositorylog,
        ISqlSugarRepository<AConfigEntity> repositorypz,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repository = repository;
        _repositorylxr = repositorylxr;
        _repositorylog = repositorylog;
        _repositorypz = repositorypz;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }





    /// <summary>
    /// 获取a_customer.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.ACtcAddressList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.ACtcContactList)
            //.Where(it => it.DeleteMark == null)
            .Select(it => new ACustomerEntity
            {
                id = it.id,
                F_CustomerName = it.F_CustomerName,
                F_CustomerCode = it.F_CustomerCode,
                F_QRCode = it.F_QRCode,
                F_IsPublic = it.F_IsPublic,
                F_CustomerTags = it.F_CustomerTags,
                F_ShareUsers = it.F_ShareUsers,
                F_IsFollow = it.F_IsFollow,
                F_State = it.F_State,
                F_Description = it.F_Description,
                F_Files = it.F_Files,
                F_RequisitionTime = it.F_RequisitionTime,
                F_RequisitionUserId = it.F_RequisitionUserId,
                F_CreatorTime = it.F_CreatorTime,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                ACtcAddressList = it.ACtcAddressList,
                ACtcContactList = it.ACtcContactList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<ACustomerInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField6eed25, async aCtcAddress =>
        {
            // 创建时间
            aCtcAddress.F_CreatorTime = aCtcAddress.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFieldddabab, async aCtcContact =>
        {
            // 创建时间
            aCtcContact.F_CreatorTime = aCtcContact.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

    /// <summary>
    /// 获取a_customer列表（仅返回主表数据，移除子表处理）.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] ACustomerListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<ACustomerListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_customer"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACustomerEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACustomerEntity>();
        var F_RequisitionUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_RequisitionUserId").DbColumnName;

        // 构建数据权限条件：非管理员需要满足权限条件或被共享
        var query = _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_CustomerName), it => it.F_CustomerName.Contains(input.F_CustomerName))
            .WhereIF(!string.IsNullOrEmpty(input.F_CustomerCode), it => it.F_CustomerCode.Contains(input.F_CustomerCode))
            .WhereIF(!string.IsNullOrEmpty(input.F_CustomerTags), it => it.F_CustomerTags.Equals(input.F_CustomerTags))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_RequisitionUserIdDbColumnName, input.F_RequisitionUserId))
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.F_IsPublic == null)
            .Where(it => it.FlowId == null);

        // 非管理员：应用数据权限过滤（满足权限条件 或 被共享给当前用户）
        if (_userManager.User.IsAdministrator == 0)
        {
            // 获取当前用户ID
            var currentUserId = _userManager.UserId;

            if (authorizeWhere != null && authorizeWhere.Any())
            {
                // 有权限条件时：满足权限条件 或 被共享给当前用户
                // 使用子查询方式实现 OR 逻辑
                query = query.Where(it =>
                    SqlFunc.Subqueryable<ACustomerEntity>()
                        .Where(a => a.id == it.id)
                        .Where(authorizeWhere)
                        .Any()
                    || it.F_ShareUsers.Contains(currentUserId));
            }
            else
            {
                // 没有权限条件时：只检查是否被共享
                query = query.Where(it => it.F_ShareUsers.Contains(currentUserId) || it.F_RequisitionUserId.Contains(currentUserId));
            }
        }

        var data = await query
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc)
            .OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new ACustomerListOutput
            {
                id = it.id,
                F_ContactName = SqlFunc.Subqueryable<ACtcContactEntity>().EnableTableFilter().Where(g => g.F_CustomerId == it.id && g.F_IsDefault == "1" && g.DeleteMark == null).Select(g => g.F_ContactName),
                F_ContactPhone = SqlFunc.Subqueryable<ACtcContactEntity>().EnableTableFilter().Where(g => g.F_CustomerId == it.id && g.F_IsDefault == "1" && g.DeleteMark == null).Select(g => g.F_ContactPhone),
                F_CustomerName = it.F_CustomerName,
                F_CustomerCode = it.F_CustomerCode,
                F_QRCode = it.F_QRCode,
                F_IsPublic = it.F_IsPublic,
                F_CustomerTags = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_CustomerTags) && dic.DictionaryTypeId.Equals("2009094067723571200")).Select(dic => dic.FullName),
                F_ShareUsers = it.F_ShareUsers,
                F_IsFollow = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsFollow) == 1, "开", "关"),
                F_State = SqlFunc.IIF(SqlFunc.ToInt32(it.F_State) == 1, "开", "关"),
                F_Description = it.F_Description,
                F_Files = it.F_Files,
                F_RequisitionTime = it.F_RequisitionTime.Value.ToString("yyyy-MM-dd"),
                F_RequisitionUserId = it.F_RequisitionUserId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_CreatorUserId = it.F_CreatorUserId,
            })
            .ToPagedListAsync(input.currentPage, input.pageSize);

        // 只处理主表结果（保留原来对主表字段的映射与数据调整）
        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            // 客户共享
            if (item.F_ShareUsers != null)
            {
                var F_ShareUsersUserSelect = item.F_ShareUsers.ToObject<List<string>>();
                item.F_ShareUsers = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => F_ShareUsersUserSelect.Contains(it.Id)).Select(it => it.RealName).ToListAsync());
            }

            if (item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }

            // 领用人员
            if (item.F_RequisitionUserId != null)
            {
                item.F_RequisitionUserId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_RequisitionUserId)))?.RealName;
            }

            // 创建人员
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;
        });

        return PageResult<ACustomerListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取a_customer列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    //[HttpPost("List")]
    //public async Task<dynamic> GetList([FromBody] ACustomerListQueryInput input)
    //{
    //    var authorizeWhere = new List<IConditionalModel>();
    //    var aCtcAddressAuthorizeWhere = new List<IConditionalModel>();
    //    var aCtcContactAuthorizeWhere = new List<IConditionalModel>();

    //    // 数据权限过滤
    //    if (_userManager.User.IsAdministrator == 0)
    //    {
    //        var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<ACustomerListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
    //        authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_customer"))?.conditionalModel;
    //        if (allAuthorizeWhere.Any(it => it.TableName.Equals("a_ctc_address"))) aCtcAddressAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_ctc_address"))?.conditionalModel;
    //        if (allAuthorizeWhere.Any(it => it.TableName.Equals("a_ctc_contact"))) aCtcContactAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_ctc_contact"))?.conditionalModel;
    //    }

    //    var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACustomerEntity>();
    //    var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
    //    superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
    //    List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

    //    entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcAddressEntity>();
    //    superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField6eed25-", entityInfo, 1);
    //    List<IConditionalModel> aCtcAddressConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
    //    aCtcAddressConditionalModel.AddRange(aCtcAddressAuthorizeWhere);

    //    entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcContactEntity>();
    //    superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldddabab-", entityInfo, 1);
    //    List<IConditionalModel> aCtcContactConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
    //    aCtcContactConditionalModel.AddRange(aCtcContactAuthorizeWhere);

    //    var selectIds = input.selectIds?.Split(",").ToList();
    //    entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACustomerEntity>();
    //    var F_CustomerTagsDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_CustomerTags").DbColumnName;
    //    var F_RequisitionUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_RequisitionUserId").DbColumnName;
    //    var data = await _repository.AsQueryable()
    //        .Includes(x => x.ACtcAddressList
    //        .Where(aCtcAddressConditionalModel)
    //        .Where(it => it.DeleteMark == null)
    //        .Select(it => new ACtcAddressEntity()
    //        {
    //            F_IsDefault = it.F_IsDefault,
    //            F_Region = it.F_Region,
    //            F_Address = it.F_Address,
    //            F_CreatorTime = it.F_CreatorTime,
    //        })
    //        .ToList())
    //        .Includes(x => x.ACtcContactList
    //        .Where(aCtcContactConditionalModel)
    //        .Where(it => it.DeleteMark == null)
    //        .Select(it => new ACtcContactEntity()
    //        {
    //            F_IsDefault = it.F_IsDefault,
    //            F_ContactName = it.F_ContactName,
    //            F_ContactPhone = it.F_ContactPhone,
    //            F_CreatorTime = it.F_CreatorTime,
    //        })
    //        .ToList())
    //        .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
    //        .WhereIF(!string.IsNullOrEmpty(input.F_CustomerName), it => it.F_CustomerName.Contains(input.F_CustomerName))
    //        .WhereIF(!string.IsNullOrEmpty(input.F_CustomerCode), it => it.F_CustomerCode.Contains(input.F_CustomerCode))
    //        .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_CustomerTagsDbColumnName, input.F_CustomerTags))
    //        .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_RequisitionUserIdDbColumnName, input.F_RequisitionUserId))
    //        .Where(authorizeWhere)
    //        .WhereIF(aCtcAddressAuthorizeWhere?.Count > 0, it => it.ACtcAddressList.Any(aCtcAddressAuthorizeWhere))
    //        .WhereIF(aCtcContactAuthorizeWhere?.Count > 0, it => it.ACtcContactList.Any(aCtcContactAuthorizeWhere))
    //        .Where(mainConditionalModel)
    //        .WhereIF(aCtcAddressConditionalModel.Count > 0, it => it.ACtcAddressList.Any(aCtcAddressConditionalModel))
    //        .WhereIF(aCtcContactConditionalModel.Count > 0, it => it.ACtcContactList.Any(aCtcContactConditionalModel))
    //        .Where(it => it.DeleteMark == null)
    //         .Where(it => it.F_IsPublic == null)
    //        .Where(it => it.FlowId == null)
    //        .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
    //        .Select(it => new ACustomerListOutput
    //        {
    //            id = it.id,
    //            F_CustomerName = it.F_CustomerName,
    //            F_CustomerCode = it.F_CustomerCode,
    //            F_QRCode = it.F_QRCode,
    //            F_IsPublic = it.F_IsPublic,
    //            F_CustomerTags = it.F_CustomerTags,
    //            F_ShareUsers = it.F_ShareUsers,
    //            F_IsFollow = it.F_IsFollow,
    //            F_State = SqlFunc.IIF(SqlFunc.ToInt32(it.F_State) == 1, "开", "关"),
    //            F_Description = it.F_Description,
    //            F_Files = it.F_Files,
    //            F_RequisitionTime = it.F_RequisitionTime.Value.ToString("yyyy-MM-dd"),
    //            F_RequisitionUserId = it.F_RequisitionUserId,
    //            F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
    //            F_CreatorUserId = it.F_CreatorUserId,
    //            tableField6eed25 = it.ACtcAddressList.Adapt<List<ACtcAddressListOutput>>(),
    //            tableFieldddabab = it.ACtcContactList.Adapt<List<ACtcContactListOutput>>(),
    //        })
    //        .ToPagedListAsync(input.currentPage, input.pageSize);

    //    await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
    //    {
    //        var linkageParameters = new List<DataInterfaceParameter>();
    //        // 客户标签
    //        if (item.F_CustomerTags != null)
    //        {
    //            item.F_CustomerTags = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.Id.Equals(item.F_CustomerTags) && it.DictionaryTypeId.Equals("2009094067723571200")).Select(it => it.FullName).FirstAsync();
    //        }

    //        // 客户共享
    //        if (item.F_ShareUsers != null)
    //        {
    //            item.F_ShareUsers = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_ShareUsers)))?.RealName;
    //        }

    //        if (item.F_Files != null)
    //        {
    //            item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
    //        }
    //        else
    //        {
    //            item.F_Files = new List<FileControlsModel>();
    //        }

    //        // 领用人员
    //        if (item.F_RequisitionUserId != null)
    //        {
    //            item.F_RequisitionUserId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_RequisitionUserId)))?.RealName;
    //        }

    //        // 创建人员
    //        var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
    //        item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;

    //    });

    //    await _repository.AsSugarClient().ThenMapperAsync(data.list.SelectMany(it => it.tableField6eed25), async aCtcAddress =>
    //    {
    //        var aCustomer = data.list.ToList().Find(it => it.id.Equals(aCtcAddress.F_CustomerId));
    //        var linkageParameters = new List<DataInterfaceParameter>();

    //        // 是否默认
    //        aCtcAddress.F_IsDefault = aCtcAddress.F_IsDefault == "1" ? "开" : "关";

    //        // 地区
    //        if (aCtcAddress.F_Region != null)
    //        {
    //            var F_RegionAreaSelect = aCtcAddress.F_Region.ToObject<List<string>>();
    //            aCtcAddress.F_Region = string.Join("/", await _repository.AsSugarClient().Queryable<ProvinceEntity>().Where(it => F_RegionAreaSelect.Contains(it.Id)).Select(it => it.FullName).ToListAsync());
    //        }

    //        // 创建时间
    //        aCtcAddress.F_CreatorTime = aCtcAddress.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
    //    });
    //    await _repository.AsSugarClient().ThenMapperAsync(data.list.SelectMany(it => it.tableFieldddabab), async aCtcContact =>
    //    {
    //        var aCustomer = data.list.ToList().Find(it => it.id.Equals(aCtcContact.F_CustomerId));
    //        var linkageParameters = new List<DataInterfaceParameter>();

    //        // 是否默认
    //        aCtcContact.F_IsDefault = aCtcContact.F_IsDefault == "1" ? "开" : "关";

    //        // 创建时间
    //        aCtcContact.F_CreatorTime = aCtcContact.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
    //    });
    //    return PageResult<ACustomerListOutput>.SqlSugarPageResult(data);
    //}


    /// <summary>
    /// 获取公海客户a_customer列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Listgh")]
    public async Task<dynamic> GetListgh([FromBody] ACustomerListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aCtcAddressAuthorizeWhere = new List<IConditionalModel>();
        var aCtcContactAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        //if (_userManager.User.IsAdministrator == 0)
        //{
        //    var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<ACustomerListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
        //    authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_customer"))?.conditionalModel;
        //    if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_ctc_address"))) aCtcAddressAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_ctc_address"))?.conditionalModel;
        //    if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_ctc_contact"))) aCtcContactAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_ctc_contact"))?.conditionalModel;
        //}

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACustomerEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcAddressEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField6eed25-", entityInfo, 1);
        List<IConditionalModel> aCtcAddressConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aCtcAddressConditionalModel.AddRange(aCtcAddressAuthorizeWhere);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcContactEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldddabab-", entityInfo, 1);
        List<IConditionalModel> aCtcContactConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aCtcContactConditionalModel.AddRange(aCtcContactAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACustomerEntity>();
        var F_RequisitionUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_RequisitionUserId").DbColumnName;
        var data = await _repository.AsQueryable()
            .Includes(x => x.ACtcAddressList
            .Where(aCtcAddressConditionalModel)
            .Where(it => it.DeleteMark  == null)
               .Where(it => it.DeleteMark  == null)
            .Select(it => new ACtcAddressEntity()
            {
                F_IsDefault = it.F_IsDefault,
                F_Region = it.F_Region,
                F_Address = it.F_Address,
                F_CreatorTime = it.F_CreatorTime,
            })
            .ToList())
            .Includes(x => x.ACtcContactList
            .Where(aCtcContactConditionalModel)
            .Where(it => it.DeleteMark  == null)
            .Select(it => new ACtcContactEntity()
            {
                F_IsDefault = it.F_IsDefault,
                F_ContactName = it.F_ContactName,
                F_ContactPhone = it.F_ContactPhone,
                F_CreatorTime = it.F_CreatorTime,
            })
            .ToList())
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_CustomerName), it => it.F_CustomerName.Contains(input.F_CustomerName))
            .WhereIF(!string.IsNullOrEmpty(input.F_CustomerCode), it => it.F_CustomerCode.Contains(input.F_CustomerCode))
            .WhereIF(!string.IsNullOrEmpty(input.F_CustomerTags), it => it.F_CustomerTags.Equals(input.F_CustomerTags))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_RequisitionUserIdDbColumnName, input.F_RequisitionUserId))
            .Where(authorizeWhere)
            .WhereIF(aCtcAddressAuthorizeWhere?.Count > 0, it => it.ACtcAddressList.Any(aCtcAddressAuthorizeWhere))
            .WhereIF(aCtcContactAuthorizeWhere?.Count > 0, it => it.ACtcContactList.Any(aCtcContactAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aCtcAddressConditionalModel.Count > 0, it => it.ACtcAddressList.Any(aCtcAddressConditionalModel))
            .WhereIF(aCtcContactConditionalModel.Count > 0, it => it.ACtcContactList.Any(aCtcContactConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.F_IsPublic == "1")
            .Where(it => it.FlowId==null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new ACustomerListOutput
            {
                id = it.id,
                F_CustomerName = it.F_CustomerName,
                F_CustomerCode = it.F_CustomerCode,
                F_QRCode = it.F_QRCode,
                F_IsPublic = it.F_IsPublic,
                F_CustomerTags = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_CustomerTags) && dic.DictionaryTypeId.Equals("2009094067723571200")).Select(dic => dic.FullName),
                F_ShareUsers = it.F_ShareUsers,
                F_IsFollow = it.F_IsFollow,
                F_State = SqlFunc.IIF(SqlFunc.ToInt32(it.F_State) == 1, "开", "关"),
                F_Description = it.F_Description,
                F_Files = it.F_Files,
                F_RequisitionTime = it.F_RequisitionTime.Value.ToString("yyyy-MM-dd"),
                F_RequisitionUserId = it.F_RequisitionUserId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_CreatorUserId = it.F_CreatorUserId,
                tableField6eed25 = it.ACtcAddressList.Adapt<List<ACtcAddressListOutput>>(),
                tableFieldddabab = it.ACtcContactList.Adapt<List<ACtcContactListOutput>>(),
            })
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            // 客户共享
            if (item.F_ShareUsers != null)
            {
                var F_ShareUsersUserSelect = item.F_ShareUsers.ToObject<List<string>>();
                item.F_ShareUsers = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => F_ShareUsersUserSelect.Contains(it.Id)).Select(it => it.RealName).ToListAsync());
            }
            if (item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }

            // 领用人员
            if(item.F_RequisitionUserId != null)
            {
                item.F_RequisitionUserId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_RequisitionUserId)))?.RealName;
            }

            // 创建人员
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.list.SelectMany(it => it.tableField6eed25), async aCtcAddress =>
        {
            var aCustomer = data.list.ToList().Find(it => it.id.Equals(aCtcAddress.F_CustomerId));
            var linkageParameters = new List<DataInterfaceParameter>();

            // 是否默认
            aCtcAddress.F_IsDefault = aCtcAddress.F_IsDefault=="1" ? "开" : "关";

            // 地区
            if(aCtcAddress.F_Region != null)
            {
                var F_RegionAreaSelect = aCtcAddress.F_Region.ToObject<List<string>>();
                aCtcAddress.F_Region = string.Join("/", await _repository.AsSugarClient().Queryable<ProvinceEntity>().Where(it => F_RegionAreaSelect.Contains(it.Id)).Select(it => it.FullName).ToListAsync());
            }

            // 创建时间
            aCtcAddress.F_CreatorTime = aCtcAddress.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        await _repository.AsSugarClient().ThenMapperAsync(data.list.SelectMany(it => it.tableFieldddabab), async aCtcContact =>
        {
            var aCustomer = data.list.ToList().Find(it => it.id.Equals(aCtcContact.F_CustomerId));
            var linkageParameters = new List<DataInterfaceParameter>();

            // 是否默认
            aCtcContact.F_IsDefault = aCtcContact.F_IsDefault=="1" ? "开" : "关";

            // 创建时间
            aCtcContact.F_CreatorTime = aCtcContact.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return PageResult<ACustomerListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取客户联系人列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Listlxr")]
    public async Task<dynamic> GetListlxr([FromBody] ACtcContactListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<ACtcContactListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var entityInfo = _repositorylxr.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcContactEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();
        var data = await _repositorylxr.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.F_Id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContactName), it => it.F_ContactName.Contains(input.F_ContactName))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContactPhone), it => it.F_ContactPhone.Contains(input.F_ContactPhone))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Select(it => new ACtcContactListOutput
            {
                F_CustomerId = it.F_Id,
                F_IsDefault = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsDefault) == 1, "开", "关"),
                F_ContactName = it.F_ContactName,
                F_ContactPhone = it.F_ContactPhone,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CustomerId).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        return PageResult<ACtcContactListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_customer.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] ACustomerCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACustomerEntity>();
        entity.id = SnowflakeIdHelper.NextId();

        // 自动生成客户编号
        if (string.IsNullOrEmpty(entity.F_CustomerCode))
        {
            var maxCode = await _repository.AsQueryable()
                .Where(it => !string.IsNullOrEmpty(it.F_CustomerCode) && it.F_CustomerCode.StartsWith("KH"))
                .Select(it => it.F_CustomerCode)
                .ToListAsync();

            int nextNumber = 1;
            if (maxCode.Any())
            {
                // 找到最大的编号
                var maxNumber = maxCode
                    .Where(code => code.Length > 2 && int.TryParse(code.Substring(2), out _))
                    .Select(code => int.Parse(code.Substring(2)))
                    .DefaultIfEmpty(0)
                    .Max();
                nextNumber = maxNumber + 1;
            }

            entity.F_CustomerCode = $"KH{nextNumber:D6}";
        }

        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ACustomerEntity>(new ACustomerEntity()));
        ConfigModel tableField6eed25Config = new ConfigModel
        {
            tableName = "a_ctc_address",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "客户地址",
            children = ExportImportDataHelper.GetTemplateParsing<ACtcAddressEntity>(new ACtcAddressEntity())
        };
        FieldsModel tableField6eed25Model = new FieldsModel()
        {
            __config__ = tableField6eed25Config,
            __vModel__ = "tableField6eed25"
        };
        fieldList.Add(tableField6eed25Model);
        ConfigModel tableFieldddababConfig = new ConfigModel
        {
            tableName = "a_ctc_contact",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "客户联系人",
            children = ExportImportDataHelper.GetTemplateParsing<ACtcContactEntity>(new ACtcContactEntity())
        };
        FieldsModel tableFieldddababModel = new FieldsModel()
        {
            __config__ = tableFieldddababConfig,
            __vModel__ = "tableFieldddabab"
        };
        fieldList.Add(tableFieldddababModel);
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;


        var aCtcAddressEntityList = input.tableField6eed25.Adapt<List<ACtcAddressEntity>>();
        if(aCtcAddressEntityList != null)
        {
            foreach (var item in aCtcAddressEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.ACtcAddressList = aCtcAddressEntityList;
        }

        var aCtcContactEntityList = input.tableFieldddabab.Adapt<List<ACtcContactEntity>>();
        if(aCtcContactEntityList != null)
        {
            foreach (var item in aCtcContactEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorOrganizeId = _userManager.User.OrganizeId;
            }

            entity.ACtcContactList = aCtcContactEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.ACtcAddressList)
            .Include(it => it.ACtcContactList)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取a_customer无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(ACustomerListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aCtcAddressAuthorizeWhere = new List<IConditionalModel>();
        var aCtcContactAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<ACustomerListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_customer"))?.conditionalModel;
            if (allAuthorizeWhere.Any(it => it.TableName.Equals("a_ctc_address"))) aCtcAddressAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_ctc_address"))?.conditionalModel;
            if (allAuthorizeWhere.Any(it => it.TableName.Equals("a_ctc_contact"))) aCtcContactAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_ctc_contact"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACustomerEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcAddressEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField6eed25-", entityInfo, 1);
        List<IConditionalModel> aCtcAddressConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcContactEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldddabab-", entityInfo, 1);
        List<IConditionalModel> aCtcContactConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);


        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACustomerEntity>();
        var F_RequisitionUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_RequisitionUserId").DbColumnName;

        // 构建数据权限条件：非管理员需要满足权限条件或被共享
        var query = _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_CustomerName), it => it.F_CustomerName.Contains(input.F_CustomerName))
            .WhereIF(!string.IsNullOrEmpty(input.F_CustomerCode), it => it.F_CustomerCode.Contains(input.F_CustomerCode))
            .WhereIF(!string.IsNullOrEmpty(input.F_CustomerTags), it => it.F_CustomerTags.Equals(input.F_CustomerTags))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_RequisitionUserIdDbColumnName, input.F_RequisitionUserId))
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.F_IsPublic == null)
            .Where(it => it.FlowId == null);

        // 非管理员：应用数据权限过滤（满足权限条件 或 被共享给当前用户）
        if (_userManager.User.IsAdministrator == 0)
        {
            // 获取当前用户ID
            var currentUserId = _userManager.UserId;

            if (authorizeWhere != null && authorizeWhere.Any())
            {
                // 有权限条件时：满足权限条件 或 被共享给当前用户
                // 使用子查询方式实现 OR 逻辑
                query = query.Where(it =>
                    SqlFunc.Subqueryable<ACustomerEntity>()
                        .Where(a => a.id == it.id)
                        .Where(authorizeWhere)
                        .Any()
                    || it.F_ShareUsers.Contains(currentUserId));
            }
            else
            {
                // 没有权限条件时：只检查是否被共享
                query = query.Where(it => it.F_ShareUsers.Contains(currentUserId) || it.F_RequisitionUserId.Contains(currentUserId));
            }
        }

        var list = await query
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new ACustomerListOutput
            {
                id = it.id,
                F_CustomerName = it.F_CustomerName,
                F_CustomerCode = it.F_CustomerCode,
                F_QRCode = it.F_QRCode,
                F_IsPublic = it.F_IsPublic,
                F_CustomerTags = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_CustomerTags) && dic.DictionaryTypeId.Equals("2009094067723571200")).Select(dic => dic.FullName),
                F_ShareUsers = it.F_ShareUsers,
                F_IsFollow = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsFollow) == 1, "是", "否"),
                F_State = SqlFunc.IIF(SqlFunc.ToInt32(it.F_State) == 1, "启用", "禁用"),
                F_Description = it.F_Description,
                F_Files = it.F_Files,
                F_RequisitionTime = it.F_RequisitionTime.Value.ToString("yyyy-MM-dd"),
                F_RequisitionUserId = it.F_RequisitionUserId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_CreatorUserId = it.F_CreatorUserId,
            })
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 客户共享
            if (item.F_ShareUsers != null)
            {
                var F_ShareUsersUserSelect = item.F_ShareUsers.ToObject<List<string>>();
                item.F_ShareUsers = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => F_ShareUsersUserSelect.Contains(it.Id)).Select(it => it.RealName).ToListAsync());
            }

            if (item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }

            // 领用人员
            if (item.F_RequisitionUserId != null)
            {
                item.F_RequisitionUserId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_RequisitionUserId)))?.RealName;
            }

            // 创建人员
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;

        });

        return list;
    }

    /// <summary>
    /// 导出a_customer.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] ACustomerListQueryInput input)
    {
        var exportData = new List<ACustomerListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<ACustomerListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName, input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新a_customer.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] ACustomerUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACustomerEntity>();
        // Ensure entity.id is set from route if not provided in input
        if (string.IsNullOrEmpty(entity.id))
        {
            entity.id = id;
        }
        // 读取原始实体用于比对并记录变更日志
        var oldEntity = await _repository.GetFirstAsync(it => it.id == entity.id);
        if (oldEntity == null) throw Oops.Oh(ErrorCode.COM1002);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ACustomerEntity>(new ACustomerEntity()));
        ConfigModel tableField6eed25Config = new ConfigModel
        {
            tableName = "a_ctc_address",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "客户地址",
            children = ExportImportDataHelper.GetTemplateParsing<ACtcAddressEntity>(new ACtcAddressEntity())
        };
        FieldsModel tableField6eed25Model = new FieldsModel()
        {
            __config__ = tableField6eed25Config,
            __vModel__ = "tableField6eed25"
        };
        fieldList.Add(tableField6eed25Model);
        ConfigModel tableFieldddababConfig = new ConfigModel
        {
            tableName = "a_ctc_contact",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "客户联系人",
            children = ExportImportDataHelper.GetTemplateParsing<ACtcContactEntity>(new ACtcContactEntity())
        };
        FieldsModel tableFieldddababModel = new FieldsModel()
        {
            __config__ = tableFieldddababConfig,
            __vModel__ = "tableFieldddabab"
        };
        fieldList.Add(tableFieldddababModel);

        // 移除客户地址可能被删除数据
        if (input.tableField6eed25 !=null && input.tableField6eed25.Any())
            await _repository.AsSugarClient().Deleteable<ACtcAddressEntity>().Where(it => it.F_CustomerId == entity.id && !input.tableField6eed25.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<ACtcAddressEntity>().Where(it => it.F_CustomerId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增客户地址新数据
        var aCtcAddressEntityList = input.tableField6eed25.Adapt<List<ACtcAddressEntity>>();


        // 移除客户联系人可能被删除数据
        if (input.tableFieldddabab !=null && input.tableFieldddabab.Any())
            await _repository.AsSugarClient().Deleteable<ACtcContactEntity>().Where(it => it.F_CustomerId == entity.id && !input.tableFieldddabab.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<ACtcContactEntity>().Where(it => it.F_CustomerId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增客户联系人新数据
        var aCtcContactEntityList = input.tableFieldddabab.Adapt<List<ACtcContactEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_CustomerName,
                    it.F_CustomerCode,
                    it.F_QRCode,
                    it.F_IsPublic,
                    it.F_CustomerTags,
                    it.F_ShareUsers,
                    it.F_IsFollow,
                    it.F_State,
                    it.F_Description,
                    it.F_Files,
                    it.F_RequisitionTime,
                    it.F_RequisitionUserId,
                })
                .ExecuteCommandAsync();

            if(aCtcAddressEntityList != null)
            {
                foreach (var item in aCtcAddressEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_CustomerId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorTime = null;
                        item.F_CustomerId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }

            if(aCtcContactEntityList != null)
            {
                foreach (var item in aCtcContactEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_CustomerId = entity.id;
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorOrganizeId = _userManager.User.OrganizeId;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorTime = null;
                        item.F_CustomerId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }
            // 记录变更日志到 a_ctclong 表（仅当有变更时）
            try
            {
                var changeList = new List<string>();
                string safeToStr(object v) => v == null ? string.Empty : v.ToString();

                void AddChange(string label, object oldVal, object newVal)
                {
                    var o = safeToStr(oldVal);
                    var n = safeToStr(newVal);
                    if (o != n)
                    {
                        changeList.Add($"{label} 字段 值由 {o} 改为 {n}");
                    }
                }

                AddChange("名称", oldEntity.F_CustomerName, entity.F_CustomerName);
                AddChange("编号", oldEntity.F_CustomerCode, entity.F_CustomerCode);
                AddChange("二维码", oldEntity.F_QRCode, entity.F_QRCode);
                AddChange("公海客户", oldEntity.F_IsPublic, entity.F_IsPublic);
                // 将客户标签的 id 转换为字典文本（如果存在）
                string oldCustomerTagName = null;
                string newCustomerTagName = null;
                if (oldEntity.F_CustomerTags != null)
                {
                    oldCustomerTagName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(oldEntity.F_CustomerTags) && it.DictionaryTypeId.Equals("2009094067723571200")).Select(it => it.FullName).FirstAsync();
                }
                if (entity.F_CustomerTags != null)
                {
                    newCustomerTagName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(entity.F_CustomerTags) && it.DictionaryTypeId.Equals("2009094067723571200")).Select(it => it.FullName).FirstAsync();
                }
                AddChange("客户标签", oldCustomerTagName, newCustomerTagName);
                AddChange("客户共享", oldEntity.F_ShareUsers, entity.F_ShareUsers);
                AddChange("关注", oldEntity.F_IsFollow, entity.F_IsFollow);
                AddChange("状态", oldEntity.F_State, entity.F_State);
                AddChange("备注", oldEntity.F_Description, entity.F_Description);
                AddChange("附件", oldEntity.F_Files, entity.F_Files);
                AddChange("领用时间", oldEntity.F_RequisitionTime, entity.F_RequisitionTime);
                AddChange("领用人员", oldEntity.F_RequisitionUserId, entity.F_RequisitionUserId);

                if (changeList.Any())
                {
                    var log = new ACtclongEntity
                    {
                        F_Id = SnowflakeIdHelper.NextId(),
                        F_Ctcid = entity.id,
                        F_CreatorUserId = string.IsNullOrEmpty(entity.F_CreatorUserId) ? _userManager.UserId : entity.F_CreatorUserId,
                        F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime(),
                        F_Title = "更新数据",
                        F_Content = string.Join("；", changeList)
                    };
                    await _repositorylog.AsInsertable(log).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
                }
            }
            catch
            {
                // 记录日志部分为非关键操作，避免影响主流程，故忽略日志写入失败
            }
        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 删除a_customer.
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
    /// 批量删除a_customer.
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
            // 批量删除a_customer
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_customer详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.ACtcAddressList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.ACtcContactList)
            //.Where(it => it.DeleteMark == null)
            .Where(it => it.id == id)
            .Select(it => new ACustomerDetailOutput
            {
                id = it.id,
                F_CustomerName = it.F_CustomerName,
                F_CustomerCode = it.F_CustomerCode,
                F_QRCode = it.F_QRCode,
                F_IsPublic = it.F_IsPublic,
                F_CustomerTags = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_CustomerTags) && dic.DictionaryTypeId.Equals("2009094067723571200")).Select(dic => dic.FullName),
                F_ShareUsers = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_ShareUsers)).Select(u => u.RealName),
                F_IsFollow = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsFollow) == 1, "是", "否"),
                F_State = SqlFunc.IIF(SqlFunc.ToInt32(it.F_State) == 1, "启用", "禁用"),
                F_Description = it.F_Description,
                F_Files = it.F_Files,
                F_RequisitionTime = it.F_RequisitionTime.Value.ToString("yyyy-MM-dd"),
                F_RequisitionUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_RequisitionUserId)).Select(u => u.RealName),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                tableField6eed25 = it.ACtcAddressList.Adapt<List<ACtcAddressDetailOutput>>(),
                tableFieldddabab = it.ACtcContactList.Adapt<List<ACtcContactDetailOutput>>(),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            if(item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField6eed25), async aCtcAddress =>
        {
            var aCustomer = data.ToList().Find(it => it.id.Equals(aCtcAddress.F_CustomerId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 是否默认
            aCtcAddress.F_IsDefault = aCtcAddress.F_IsDefault=="1" ? "开" : "关";

            // 地区
            if(aCtcAddress.F_Region != null)
            {
                var F_RegionAreaSelect = aCtcAddress.F_Region.ToObject<List<string>>();
                aCtcAddress.F_Region = string.Join("/", await _repository.AsSugarClient().Queryable<ProvinceEntity>().Where(it => F_RegionAreaSelect.Contains(it.Id)).Select(it => it.FullName).ToListAsync());
            }

            // 创建时间
            aCtcAddress.F_CreatorTime = aCtcAddress.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFieldddabab), async aCtcContact =>
        {
            var aCustomer = data.ToList().Find(it => it.id.Equals(aCtcContact.F_CustomerId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 是否默认
            aCtcContact.F_IsDefault = aCtcContact.F_IsDefault=="1" ? "开" : "关";

            // 创建时间
            aCtcContact.F_CreatorTime = aCtcContact.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        // 查询并返回关联操作日志（按时间倒序）
        var detail = data.FirstOrDefault();
        var logs = await _repositorylog.AsQueryable()
            .Where(it => it.F_Ctcid == id)
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .Select(it => new ACtclongEntity
            {
                F_Id = it.F_Id,
                F_Type = it.F_Type,
                F_Title = it.F_Title,
                F_Content = it.F_Content,
                F_CreatorUserId = it.F_CreatorUserId,
                F_CreatorTime = it.F_CreatorTime
            }).ToListAsync();

        var logsOutput = new List<object>();
        foreach (var l in logs)
        {
            string creator = null;
            if (!string.IsNullOrEmpty(l.F_CreatorUserId))
            {
                var u = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(l.F_CreatorUserId));
                creator = u != null ? string.Format("{0}/{1}", u.RealName, u.Account) : l.F_CreatorUserId;
            }
            logsOutput.Add(new
            {
                id = l.F_Id,
                F_Type = l.F_Type,
                F_Title = l.F_Title,
                F_Content = l.F_Content,
                F_CreatorUserId = creator,
                F_CreatorTime = l.F_CreatorTime?.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        return new { detail, logs = logsOutput };
    }


        /// <summary>
    /// DTO: 检查重名接口输入
    /// </summary>
    public class CheckDuplicateNameInput
    {
        public string F_CustomerName { get; set; }
    }


    /// <summary>
    /// 检查客户名称是否重复（受系统配置控制）
    /// 前端传入 JSON：{ "F_CustomerName": "张三" }
    /// 当 a_config 中 id = 2009104993759662080 的 F_Value 为 "1" 时启用重名检查，若存在重名则返回 400 验证异常。
    /// </summary>
    /// <param name="input">包含 F_CustomerName 的对象</param>
    /// <returns></returns>
    [HttpPost("CheckDuplicateName")]
    public async Task<dynamic> CheckDuplicateName([FromBody] CheckDuplicateNameInput input)
    {
        string name = input == null ? null : input.F_CustomerName;
        if (string.IsNullOrWhiteSpace(name)) throw Oops.Bah("名称不能为空");

        // 默认不重复
        bool exists = false;
        try
        {
            var configValue = await _repositorypz.AsQueryable()
                .Where(it => it.id == "2009104993759662080" && it.DeleteMark == null)
                .Select(it => it.F_Value)
                .FirstAsync();

            if (!string.IsNullOrEmpty(configValue) && configValue.Trim() == "1")
            {
                exists = await _repository.IsAnyAsync(it => it.F_CustomerName == name && it.DeleteMark == null);
            }
        }
        catch
        {
            // 配置读取异常不阻塞主流程，视为不启用重名检查（exists 保持 false）
        }

        if (exists)
        {
            // 返回结构化信息，前端更容易消费
            return new { exists = true, message = "该姓名已重复" };
        }
        else
        {
            return new { exists = false, message = "名称可用" };
        }
    }


    
    /// <summary>
    /// 将指定客户移入公海（设置 F_IsPublic 为 '1'）
    /// </summary>
    /// <param name="id">客户主键</param>
    [HttpPost("MoveToPublicSea/{id}")]
    public async Task MoveToPublicSea(string id)
    {
        // 读取实体
        var entity = await _repository.AsQueryable().FirstAsync(it => it.id.Equals(id));
        if (entity == null) throw Oops.Oh(ErrorCode.COM1002);

        // 标记为公海客户（值使用字符串 '1'）
        entity.F_IsPublic = "1";

        // 只更新 F_IsPublic 字段
        var isOk = await _repository.AsUpdateable(entity).UpdateColumns(it => new { it.F_IsPublic }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 领用客户：清空公海标记并设置领用人和领用时间
    /// </summary>
    /// <param name="id">客户主键</param>
    /// <param name="userId">领用人用户id</param>
    [HttpPost("Requisition/{id}/{userId}")]
    public async Task Requisition(string id, string userId)
    {
        // 查询实体
        var entity = await _repository.AsQueryable().FirstAsync(it => it.id.Equals(id));
        if (entity == null) throw Oops.Oh(ErrorCode.COM1002);

        // 清空公海标识并设置领用人/时间
        entity.F_IsPublic = null;
        entity.F_RequisitionUserId = userId;
        entity.F_RequisitionTime = DateTime.Now;

        // 只更新需要的字段
        var isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new { it.F_IsPublic, it.F_RequisitionUserId, it.F_RequisitionTime })
            .ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }


}
