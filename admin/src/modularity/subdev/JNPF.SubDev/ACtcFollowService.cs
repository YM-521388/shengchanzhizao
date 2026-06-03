using JNPF.Common.Core.Manager;
using JNPF.Engine.Entity.Model;
using JNPF.ClayObject;
using JNPF.Common.Models.NPOI;
using JNPF.Common.CodeGen.ExportImport;
using JNPF.Common.Core.Manager.Files;
using JNPF.Common.Dtos;
using JNPF.Common.CodeGen.DataParsing;
using JNPF.Common.Const;
using JNPF.Common.Manager;
using JNPF.Common.Enums;
using JNPF.Common.Extension;
using JNPF.Common.Filter;
using JNPF.Common.Models;
using JNPF.Common.Security;
using JNPF.DependencyInjection;
using JNPF.DynamicApiController;
using JNPF.FriendlyException;
using JNPF.Systems.Entitys.Permission;
using JNPF.Systems.Entitys.System;
using JNPF.Systems.Interfaces.System;
using JNPF.Common.Dtos.Datainterface;
using JNPF.VisualDev.Engine;
using JNPF.example.Entitys.Dto.ACtcFollow;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.DatabaseAccessor;
using JNPF.Common.Dtos;

namespace JNPF.example;

/// <summary>
/// 业务实现：客户跟单.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "ACtcFollow", Order = 200)]
[Route("api/example/[controller]")]
public class ACtcFollowService : IACtcFollowService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<ACtcFollowEntity> _repository;
    private readonly ISqlSugarRepository<ACtcContactEntity> _repositorykh;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"客户\",\"field\":\"F_CustomerId\"},{\"value\":\"跟单类型\",\"field\":\"F_FollowType\"},{\"value\":\"联系人\",\"field\":\"F_ContactId\"},{\"value\":\"跟单内容\",\"field\":\"F_FollowContent\"},{\"value\":\"下次跟单时间\",\"field\":\"F_NextFollowTime\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="ACtcFollowService"/>类型的新实例.
    /// </summary>
    public ACtcFollowService(
        ISqlSugarRepository<ACtcFollowEntity> repository,
         ISqlSugarRepository<ACtcContactEntity> repositorykh,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repository = repository;
        _repositorykh = repositorykh;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取客户跟单.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new ACtcFollowEntity
            {
                id = it.id,
                F_CustomerId = it.F_CustomerId,
                F_FollowType = it.F_FollowType,
                F_ContactId = it.F_ContactId,
                F_FollowContent = it.F_FollowContent,
                F_NextFollowTime = it.F_NextFollowTime,
                F_State = it.F_State,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<ACtcFollowInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取客户跟单列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] ACtcFollowListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<ACtcFollowListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcFollowEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();
        var F_CustomerIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_CustomerId").DbColumnName;
        var F_FollowTypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_FollowType").DbColumnName;
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_CustomerIdDbColumnName, input.F_CustomerId))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_FollowTypeDbColumnName, input.F_FollowType))
            .WhereIF(!string.IsNullOrEmpty(input.F_FollowContent), it => it.F_FollowContent.Contains(input.F_FollowContent))
            .WhereIF(input.F_NextFollowTime?.Count() > 0, it => SqlFunc.Between(it.F_NextFollowTime, input.F_NextFollowTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_NextFollowTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.FlowId==null)
            .Select(it => new ACtcFollowListOutput
            {
                id = it.id,
                F_CustomerId = it.F_CustomerId,
                F_FollowType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_FollowType) && dic.DictionaryTypeId.Equals("2009462155446980608")).Select(dic => dic.FullName),
                F_ContactId = it.F_ContactId,
                F_FollowContent = it.F_FollowContent,
                F_State = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_State) && dic.DictionaryTypeId.Equals("2015701595244859392")).Select(dic => dic.FullName),
                F_StateCode = it.F_State,
                F_NextFollowTime = it.F_NextFollowTime.Value.ToString("yyyy-MM-dd"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), "F_CreatorTime desc").OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        // 预批量查询联系人名称，避免 N+1 查询
        var contactIds = data.list.Where(x => !string.IsNullOrEmpty(x.F_ContactId)).Select(x => x.F_ContactId).Distinct().ToList();
        Dictionary<string, string> contactIdToName = new Dictionary<string, string>();
        if (contactIds.Any())
        {
            var contactList = await _repositorykh.AsQueryable().Where(it => contactIds.Contains(it.F_Id)).Select(it => new { it.F_Id, it.F_ContactName }).ToListAsync();
            contactIdToName = contactList.ToDictionary(k => k.F_Id, v => v.F_ContactName ?? string.Empty);
        }

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 客户ID
            if(item.F_CustomerId != null)
            {
                var F_CustomerIdData = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2009458830353764352", "F_Id", "F_CustomerName", "");
                item.F_CustomerId = F_CustomerIdData.Find(it => it.id.Equals(item.F_CustomerId))?.fullName;
            }
            // 联系人ID：使用映射替换为联系人名称
            if (!string.IsNullOrEmpty(item.F_ContactId) && contactIdToName.TryGetValue(item.F_ContactId, out var contactName))
            {
                item.F_ContactId = contactName;
            }
        });

        var resData = data.list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcFollowEntity>(new ACtcFollowEntity()));
        resData = await _controlParsing.GetParsDataList(resData, "F_ContactId", "popupSelect", _userManager.TenantId, fieldList);

        data.list = resData.ToObject<List<ACtcFollowListOutput>>(CommonConst.options);

        return PageResult<ACtcFollowListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建客户跟单.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] ACtcFollowCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACtcFollowEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ACtcFollowEntity>(new ACtcFollowEntity()));
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        entity.F_State = "0";
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 获取客户跟单无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(ACtcFollowListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<ACtcFollowListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcFollowEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();
        var F_CustomerIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_CustomerId").DbColumnName;
        var F_FollowTypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_FollowType").DbColumnName;
        var list = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_CustomerIdDbColumnName, input.F_CustomerId))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_FollowTypeDbColumnName, input.F_FollowType))
            .WhereIF(!string.IsNullOrEmpty(input.F_FollowContent), it => it.F_FollowContent.Contains(input.F_FollowContent))
            .WhereIF(input.F_NextFollowTime?.Count() > 0, it => SqlFunc.Between(it.F_NextFollowTime, input.F_NextFollowTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_NextFollowTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.FlowId==null)
            .Select(it => new ACtcFollowListOutput
            {
                id = it.id,
                F_CustomerId = it.F_CustomerId,
                F_FollowType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_FollowType) && dic.DictionaryTypeId.Equals("2009462155446980608")).Select(dic => dic.FullName),
                F_ContactId = it.F_ContactId,
                F_FollowContent = it.F_FollowContent,
                F_State = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_State) && dic.DictionaryTypeId.Equals("2015701595244859392")).Select(dic => dic.FullName),
                F_NextFollowTime = it.F_NextFollowTime.Value.ToString("yyyy-MM-dd"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToListAsync();

        // 预批量查询联系人名称
        var contactIds2 = list.Where(x => !string.IsNullOrEmpty(x.F_ContactId)).Select(x => x.F_ContactId).Distinct().ToList();
        Dictionary<string, string> contactIdToName2 = new Dictionary<string, string>();
        if (contactIds2.Any())
        {
            var contactList2 = await _repositorykh.AsQueryable().Where(it => contactIds2.Contains(it.F_Id)).Select(it => new { it.F_Id, it.F_ContactName }).ToListAsync();
            contactIdToName2 = contactList2.ToDictionary(k => k.F_Id, v => v.F_ContactName ?? string.Empty);
        }

        await _repository.AsSugarClient().ThenMapperAsync(list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 客户ID
            if(item.F_CustomerId != null)
            {
                var F_CustomerIdData = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2009458830353764352", "F_Id", "F_CustomerName", "");
                item.F_CustomerId = F_CustomerIdData.Find(it => it.id.Equals(item.F_CustomerId))?.fullName;
            }
            // 联系人ID：使用映射替换
            if (!string.IsNullOrEmpty(item.F_ContactId) && contactIdToName2.TryGetValue(item.F_ContactId, out var contactName2))
            {
                item.F_ContactId = contactName2;
            }
        });

        var resData = list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcFollowEntity>(new ACtcFollowEntity()));
        resData = await _controlParsing.GetParsDataList(resData, "F_ContactId", "popupSelect", _userManager.TenantId, fieldList);

        list = resData.ToObject<List<ACtcFollowListOutput>>(CommonConst.options);

        return list;
    }

    /// <summary>
    /// 导出客户跟单.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] ACtcFollowListQueryInput input)
    {
        var exportData = new List<ACtcFollowListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<ACtcFollowListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName,input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新客户跟单.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] ACtcFollowUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACtcFollowEntity>();
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_CustomerId,
                it.F_FollowType,
                it.F_ContactId,
                it.F_FollowContent,
                it.F_NextFollowTime,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 修改状态.
    /// </summary>
    [HttpGet("Check/{id}/{state}")]
    public async Task Check(string id, string state)
    {
        ACtcFollowEntity? entity = await _repository.GetFirstAsync(it => it.id == id);

        int isOk = await _repository.AsUpdateable().SetColumns(it => new ACtcFollowEntity()
        {
            F_State = state,
        }).Where(it => it.id == id).ExecuteCommandAsync();
    }


    /// <summary>
    /// 删除客户跟单.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).ExecuteCommandAsync();   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除客户跟单.
    /// </summary>
    /// <param name="input">主键数组.</param>
    /// <returns></returns>
    [HttpPost("batchRemove")]
    [UnitOfWork]
    public async Task BatchRemove([FromBody] BatchRemoveInput input)
    {
        var entitys = await _repository.AsQueryable().In(it => it.id, input.ids).ToListAsync();
        if (entitys.Count > 0)
        {
            // 批量删除客户跟单
            await _repository.AsDeleteable().In(it => it.id,input.ids).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 客户跟单详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new ACtcFollowDetailOutput
            {
                id = it.id,
                F_CustomerId = it.F_CustomerId,
                F_FollowType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_FollowType) && dic.DictionaryTypeId.Equals("2009462155446980608")).Select(dic => dic.FullName),
                F_ContactId = it.F_ContactId,
                F_FollowContent = it.F_FollowContent,
                F_State = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_State) && dic.DictionaryTypeId.Equals("2015701595244859392")).Select(dic => dic.FullName),
                F_NextFollowTime = it.F_NextFollowTime.Value.ToString("yyyy-MM-dd"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        // 预批量查询联系人名称（详情通常为单条）
        var contactIds3 = data.Where(x => !string.IsNullOrEmpty(x.F_ContactId)).Select(x => x.F_ContactId).Distinct().ToList();
        Dictionary<string, string> contactIdToName3 = new Dictionary<string, string>();
        if (contactIds3.Any())
        {
            var contactList3 = await _repositorykh.AsQueryable().Where(it => contactIds3.Contains(it.F_Id)).Select(it => new { it.F_Id, it.F_ContactName }).ToListAsync();
            contactIdToName3 = contactList3.ToDictionary(k => k.F_Id, v => v.F_ContactName ?? string.Empty);
        }

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 客户ID
            if(item.F_CustomerId != null)
            {
                var F_CustomerIdData = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2009458830353764352", "F_Id", "F_CustomerName", "");
                item.F_CustomerId = F_CustomerIdData.Find(it => it.id.Equals(item.F_CustomerId))?.fullName;
            }
            // 联系人ID：使用映射替换为姓名
            if (!string.IsNullOrEmpty(item.F_ContactId) && contactIdToName3.TryGetValue(item.F_ContactId, out var contactName3))
            {
                item.F_ContactId = contactName3;
            }
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcFollowEntity>(new ACtcFollowEntity()));
        resData = await _controlParsing.GetParsDataList(resData, "F_ContactId", "popupSelect", _userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }
}
