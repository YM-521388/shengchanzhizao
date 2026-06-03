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
using JNPF.example.Entitys.Dto.ASupplier;
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
/// 业务实现：供应商管理2.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "ASupplier", Order = 200)]
[Route("api/example/[controller]")]
public class ASupplierService : IASupplierService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<ASupplierEntity> _repository;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"供应商编号\",\"field\":\"F_SupplierNo\"},{\"value\":\"供应商名称\",\"field\":\"F_SupplierName\"},{\"value\":\"联系人\",\"field\":\"F_ContactPerson\"},{\"value\":\"联系人电话\",\"field\":\"F_ContactPhone\"},{\"value\":\"地区\",\"field\":\"F_Region\"},{\"value\":\"详细地址\",\"field\":\"F_DetailAddress\"},{\"value\":\"供应商标签\",\"field\":\"F_SupplierTags\"},{\"value\":\"附件\",\"field\":\"F_AttachmentUrls\"},{\"value\":\"是否启用\",\"field\":\"F_StartUsing\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="ASupplierService"/>类型的新实例.
    /// </summary>
    public ASupplierService(
        ISqlSugarRepository<ASupplierEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repository = repository;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取供应商管理2.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new ASupplierEntity
            {
                id = it.id,
                F_SupplierName = it.F_SupplierName,
                F_SupplierNo = it.F_SupplierNo,
                F_ContactPerson = it.F_ContactPerson,
                F_ContactPhone = it.F_ContactPhone,
                F_Region = it.F_Region,
                F_DetailAddress = it.F_DetailAddress,
                F_SupplierTags = it.F_SupplierTags,
                F_AttachmentUrls = it.F_AttachmentUrls,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                F_StartUsing = it.F_StartUsing,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<ASupplierInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取供应商管理2列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] ASupplierListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<ASupplierListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ASupplierEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();
        var F_Region = input.F_Region?.Last();
        var F_SupplierTagsDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_SupplierTags").DbColumnName;

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_SupplierName), it => it.F_SupplierName.Contains(input.F_SupplierName))
            .WhereIF(!string.IsNullOrEmpty(input.F_SupplierNo), it => it.F_SupplierNo.Contains(input.F_SupplierNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContactPerson), it => it.F_ContactPerson.Contains(input.F_ContactPerson))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContactPhone), it => it.F_ContactPhone.Contains(input.F_ContactPhone))
            .WhereIF(!string.IsNullOrEmpty(input.F_Region?.ToString()), it => it.F_Region.Contains(F_Region))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_SupplierTagsDbColumnName, input.F_SupplierTags))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new ASupplierListOutput
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
           .Select(it => new ASupplierListOutput
           {
               id = it.id,
               F_SupplierName = it.F_SupplierName,
               F_SupplierNo = it.F_SupplierNo,
               F_ContactPerson = it.F_ContactPerson,
               F_ContactPhone = it.F_ContactPhone,
               F_Region = it.F_Region,
               F_DetailAddress = it.F_DetailAddress,
               F_SupplierTags = it.F_SupplierTags,
               F_AttachmentUrls = it.F_AttachmentUrls,
               F_Description = it.F_Description,
               F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
               F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
               F_StartUsing = SqlFunc.IIF(SqlFunc.ToInt32(it.F_StartUsing) == 1, "开", "关"),
           }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 地区
            if (item.F_Region != null)
            {
                var F_RegionAreaSelect = item.F_Region.ToObject<List<string>>();
                item.F_Region = string.Join("/", await _repository.AsSugarClient().Queryable<ProvinceEntity>().Where(it => F_RegionAreaSelect.Contains(it.Id)).Select(it => it.FullName).ToListAsync());
            }

            // 供应商标签
            if (item.F_SupplierTags != null)
            {
                var F_SupplierTagsSelect = item.F_SupplierTags.ToObject<List<string>>();
                item.F_SupplierTags = string.Join(",", await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => F_SupplierTagsSelect.Contains(it.EnCode) && it.DictionaryTypeId.Equals("2008453808677588992")).Select(it => it.FullName).ToListAsync());
            }

            if (item.F_AttachmentUrls != null)
            {
                item.F_AttachmentUrls = item.F_AttachmentUrls.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_AttachmentUrls = new List<FileControlsModel>();
            }

        });

        data.pagination = idsQ.pagination;

        return PageResult<ASupplierListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建供应商管理2.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] ASupplierCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ASupplierEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ASupplierEntity>(new ASupplierEntity()));

        // 自动生成供应商编号
        if (string.IsNullOrEmpty(entity.F_SupplierNo))
        {
            var maxSupplierNo = await _repository.AsQueryable()
                .Where(it => it.F_SupplierNo.StartsWith("GYS") && it.DeleteMark == null && it.FlowId == null)
                .Select(it => it.F_SupplierNo)
                .ToListAsync();

            int nextNumber = 1;
            if (maxSupplierNo.Any())
            {
                var maxNumber = maxSupplierNo
                    .Where(no => no.Length >= 6 && no.StartsWith("GYS"))
                    .Select(no => int.TryParse(no.Substring(3), out int num) ? num : 0)
                    .Max();
                nextNumber = maxNumber + 1;
            }

            entity.F_SupplierNo = $"GYS{nextNumber:D6}";
        }

        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        if (await _repository.IsAnyAsync(it => it.F_ContactPhone.Equals(input.F_ContactPhone) && it.FlowId==null   && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "联系人电话");
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 获取供应商管理2无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(ASupplierListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<ASupplierListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ASupplierEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();
        var F_Region = input.F_Region?.Last();
        var F_SupplierTagsDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_SupplierTags").DbColumnName;
        var list = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_SupplierName), it => it.F_SupplierName.Contains(input.F_SupplierName))
            .WhereIF(!string.IsNullOrEmpty(input.F_SupplierNo), it => it.F_SupplierNo.Contains(input.F_SupplierNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContactPerson), it => it.F_ContactPerson.Contains(input.F_ContactPerson))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContactPhone), it => it.F_ContactPhone.Contains(input.F_ContactPhone))
            .WhereIF(!string.IsNullOrEmpty(input.F_Region?.ToString()), it => it.F_Region.Contains(F_Region))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_SupplierTagsDbColumnName, input.F_SupplierTags))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new ASupplierListOutput
            {
                id = it.id,
                F_SupplierName = it.F_SupplierName,
                F_SupplierNo = it.F_SupplierNo,
                F_ContactPerson = it.F_ContactPerson,
                F_ContactPhone = it.F_ContactPhone,
                F_Region = it.F_Region,
                F_DetailAddress = it.F_DetailAddress,
                F_SupplierTags = it.F_SupplierTags,
                F_AttachmentUrls = it.F_AttachmentUrls,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_StartUsing = SqlFunc.IIF(SqlFunc.ToInt32(it.F_StartUsing) == 1, "开", "关"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 地区
            if(item.F_Region != null)
            {
                var F_RegionAreaSelect = item.F_Region.ToObject<List<string>>();
                item.F_Region = string.Join("/", await _repository.AsSugarClient().Queryable<ProvinceEntity>().Where(it => F_RegionAreaSelect.Contains(it.Id)).Select(it => it.FullName).ToListAsync());
            }

            // 供应商标签
            if(item.F_SupplierTags != null)
            {
                var F_SupplierTagsSelect = item.F_SupplierTags.ToObject<List<string>>();
                item.F_SupplierTags = string.Join(",", await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => F_SupplierTagsSelect.Contains(it.EnCode) && it.DictionaryTypeId.Equals("2008453808677588992")).Select(it => it.FullName).ToListAsync());
            }

            if(item.F_AttachmentUrls != null)
            {
                item.F_AttachmentUrls = item.F_AttachmentUrls.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_AttachmentUrls = new List<FileControlsModel>();
            }

        });

        return list;
    }

    /// <summary>
    /// 导出供应商管理2.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] ASupplierListQueryInput input)
    {
        var exportData = new List<ASupplierListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<ASupplierListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName,input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新供应商管理2.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] ASupplierUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ASupplierEntity>();
        if (await _repository.IsAnyAsync(it => it.F_ContactPhone.Equals(input.F_ContactPhone) && it.FlowId==null   && it.DeleteMark == null && !it.id.Equals(id)))
            throw Oops.Bah(ErrorCode.COM1023, "联系人电话");
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_SupplierNo.Equals(input.F_SupplierNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "供应商编号");

        if (string.IsNullOrEmpty(entity.F_SupplierNo))
        {
            entity.F_SupplierNo = oldEntity.F_SupplierNo;
        }

        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_SupplierName,
                it.F_SupplierNo,
                it.F_ContactPerson,
                it.F_ContactPhone,
                it.F_Region,
                it.F_DetailAddress,
                it.F_SupplierTags,
                it.F_AttachmentUrls,
                it.F_Description,
                it.F_StartUsing,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除供应商管理2.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除供应商管理2.
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
            // 批量删除供应商管理2
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 供应商管理2详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new ASupplierDetailOutput
            {
                id = it.id,
                F_SupplierName = it.F_SupplierName,
                F_SupplierNo = it.F_SupplierNo,
                F_ContactPerson = it.F_ContactPerson,
                F_ContactPhone = it.F_ContactPhone,
                F_Region = it.F_Region,
                F_DetailAddress = it.F_DetailAddress,
                F_SupplierTags = it.F_SupplierTags,
                F_AttachmentUrls = it.F_AttachmentUrls,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_StartUsing = SqlFunc.IIF(SqlFunc.ToInt32(it.F_StartUsing) == 1, "开", "关"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 地区
            if(item.F_Region != null)
            {
                var F_RegionAreaSelect = item.F_Region.ToObject<List<string>>();
                item.F_Region = string.Join("/", await _repository.AsSugarClient().Queryable<ProvinceEntity>().Where(it => F_RegionAreaSelect.Contains(it.Id)).Select(it => it.FullName).ToListAsync());
            }

            // 供应商标签
            if(item.F_SupplierTags != null)
            {
                var F_SupplierTagsSelect = item.F_SupplierTags.ToObject<List<string>>();
                item.F_SupplierTags = string.Join(",", await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => F_SupplierTagsSelect.Contains(it.EnCode) && it.DictionaryTypeId.Equals("2008453808677588992")).Select(it => it.FullName).ToListAsync());
            }

            if(item.F_AttachmentUrls != null)
            {
                item.F_AttachmentUrls = item.F_AttachmentUrls.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_AttachmentUrls = new List<FileControlsModel>();
            }

        });
        return data.FirstOrDefault();
    }
}
