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
using JNPF.example.Entitys.Dto.ACtcContact;
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
/// 业务实现：客户联系人.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "ACtcContact", Order = 200)]
[Route("api/example/[controller]")]
public class ACtcContactService : IACtcContactService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<ACtcContactEntity> _repository;
    private readonly ISqlSugarRepository<ACustomerEntity> _repositoryyh;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"联系人\",\"field\":\"F_ContactName\"},{\"value\":\"联系人电话\",\"field\":\"F_ContactPhone\"},{\"value\":\"是否默认\",\"field\":\"F_IsDefault\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="ACtcContactService"/>类型的新实例.
    /// </summary>
    public ACtcContactService(
        ISqlSugarRepository<ACtcContactEntity> repository,
        ISqlSugarRepository<ACustomerEntity> repositoryyh,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repository = repository;
        _repositoryyh = repositoryyh;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取客户联系人.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .FirstAsync(it => it.F_Id.Equals(id))).Adapt<ACtcContactInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取客户联系人列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] ACtcContactListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<ACtcContactListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_ctc_contact"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcContactEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();

        // 构建数据权限条件：非管理员需要满足权限条件或被共享
        var query = _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.F_Id))
            .WhereIF(!string.IsNullOrEmpty(input.F_CustomerId), it => it.F_CustomerId.Equals(input.F_CustomerId))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContactName), it => it.F_ContactName.Contains(input.F_ContactName))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContactPhone), it => it.F_ContactPhone.Contains(input.F_ContactPhone))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .LeftJoin<ACustomerEntity>((it, ac) => ac.id == it.F_CustomerId);

        // 非管理员：应用数据权限过滤（满足权限条件 或 被共享给当前用户）
        if (_userManager.User.IsAdministrator == 0)
        {
            // 获取当前用户ID
            var currentUserId = _userManager.UserId;

            if (authorizeWhere != null && authorizeWhere.Any())
            {
                // 有权限条件时：满足权限条件 或 被共享给当前用户
                // 使用子查询方式实现 OR 逻辑
                query = query.Where((it, ac) =>
                    SqlFunc.Subqueryable<ACtcContactEntity>()
                        .Where(a => a.F_Id == it.F_Id)
                        .Where(authorizeWhere)
                        .Any()
                    || ac.F_ShareUsers.Contains(currentUserId));
            }
            else
            {
                // 没有权限条件时：只检查是否被共享
                query = query.Where((it, ac) => ac.F_ShareUsers.Contains(currentUserId));
            }
        }

        var data = await query
            .Select(it => new ACtcContactListOutput
            {
                id = it.F_Id,
                F_CustomerId = SqlFunc.Subqueryable<ACustomerEntity>().Where(c => c.id == it.F_CustomerId).Select(c => c.F_CustomerName),
                F_IsDefault = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsDefault) == 1, "开", "关"),
                F_ContactName = it.F_ContactName,
                F_ContactPhone = it.F_ContactPhone,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable()
           .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc)
            .OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        return PageResult<ACtcContactListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建客户联系人.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] ACtcContactCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACtcContactEntity>();
        entity.F_Id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ACtcContactEntity>(new ACtcContactEntity()));
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 获取客户联系人无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(ACtcContactListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<ACtcContactListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcContactEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();

        // 构建数据权限条件：非管理员需要满足权限条件或被共享
        var query = _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.F_Id))
            .WhereIF(!string.IsNullOrEmpty(input.F_CustomerId), it => it.F_CustomerId.Equals(input.F_CustomerId))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContactName), it => it.F_ContactName.Contains(input.F_ContactName))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContactPhone), it => it.F_ContactPhone.Contains(input.F_ContactPhone))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .LeftJoin<ACustomerEntity>((it, ac) => ac.id == it.F_CustomerId);

        // 非管理员：应用数据权限过滤（满足权限条件 或 被共享给当前用户）
        if (_userManager.User.IsAdministrator == 0)
        {
            // 获取当前用户ID
            var currentUserId = _userManager.UserId;

            if (authorizeWhere != null && authorizeWhere.Any())
            {
                // 有权限条件时：满足权限条件 或 被共享给当前用户
                // 使用子查询方式实现 OR 逻辑
                query = query.Where((it, ac) =>
                    SqlFunc.Subqueryable<ACtcContactEntity>()
                        .Where(a => a.F_Id == it.F_Id)
                        .Where(authorizeWhere)
                        .Any()
                    || ac.F_ShareUsers.Contains(currentUserId));
            }
            else
            {
                // 没有权限条件时：只检查是否被共享
                query = query.Where((it, ac) => ac.F_ShareUsers.Contains(currentUserId));
            }
        }

        var list = await query
            .Select(it => new ACtcContactListOutput
            {
                id = it.F_Id,
                F_CustomerId = SqlFunc.Subqueryable<ACustomerEntity>().Where(c => c.id == it.F_CustomerId).Select(c => c.F_CustomerName),
                F_IsDefault = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsDefault) == 1, "开", "关"),
                F_ContactName = it.F_ContactName,
                F_ContactPhone = it.F_ContactPhone,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToListAsync();

        return list;
    }

    /// <summary>
    /// 导出客户联系人.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] ACtcContactListQueryInput input)
    {
        var exportData = new List<ACtcContactListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<ACtcContactListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName,input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新客户联系人.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] ACtcContactUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACtcContactEntity>();
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_CustomerId,
                it.F_IsDefault,
                it.F_ContactName,
                it.F_ContactPhone,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除客户联系人.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.F_Id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除客户联系人.
    /// </summary>
    /// <param name="input">主键数组.</param>
    /// <returns></returns>
    [HttpPost("batchRemove")]
    [UnitOfWork]
    public async Task BatchRemove([FromBody] BatchRemoveInput input)
    {
        var entitys = await _repository.AsQueryable().In(it => it.F_Id, input.ids).Where(it => it.DeleteMark == null).ToListAsync();
        if (entitys.Count > 0)
        {
            // 批量删除客户联系人
            await _repository.AsDeleteable().In(it => it.F_Id, input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 客户联系人详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new ACtcContactDetailOutput
            {
                id = it.F_Id,
                F_CustomerId = it.F_CustomerId,
                F_IsDefault = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsDefault) == 1, "开", "关"),
                F_ContactName = it.F_ContactName,
                F_ContactPhone = it.F_ContactPhone,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();
        return data.FirstOrDefault();
    }
}
