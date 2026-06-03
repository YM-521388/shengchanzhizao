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
using JNPF.example.Entitys.Dto.AGoodsSpecification;
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
/// 业务实现：商品规格.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AGoodsSpecification", Order = 200)]
[Route("api/example/[controller]")]
public class AGoodsSpecificationService : IAGoodsSpecificationService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AGoodsSpecificationEntity> _repository;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"规格名称\",\"field\":\"F_Name\"},{\"value\":\"上级\",\"field\":\"F_ParentId\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"排序\",\"field\":\"F_SortCode\"},{\"value\":\"创建用户\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AGoodsSpecificationService"/>类型的新实例.
    /// </summary>
    public AGoodsSpecificationService(
        ISqlSugarRepository<AGoodsSpecificationEntity> repository,
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
    /// 获取商品规格.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new AGoodsSpecificationEntity
            {
                id = it.id,
                F_Name = it.F_Name,
                F_ParentId = it.F_ParentId,
                F_SortCode = it.F_SortCode,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AGoodsSpecificationInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取商品规格列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AGoodsSpecificationListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AGoodsSpecificationListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AGoodsSpecificationEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();
        var F_ParentId = input.F_ParentId?.Last();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_Name), it => it.F_Name.Contains(input.F_Name))
            .WhereIF(!string.IsNullOrEmpty(input.F_ParentId?.ToString()), it => it.F_ParentId.Contains(F_ParentId))
            .WhereIF(!string.IsNullOrEmpty(input.F_Description), it => it.F_Description.Contains(input.F_Description))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new AGoodsSpecificationListOutput
            {
                id = it.id,
                F_Name = it.F_Name,
                F_ParentId = it.F_ParentId,
                F_SortCode = it.F_SortCode.ToString(),
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 上级id
            var F_ParentIdData = await _dataInterfaceService.GetDynamicList("F_ParentId", "2034507723852353536", "F_Id", "F_Name", "children");
            if(item.F_ParentId != null)
            {
                var F_ParentIdCascader = item.F_ParentId.ToObject<List<string>>();
                item.F_ParentId = F_ParentIdData.FindAll(it => F_ParentIdCascader.Contains(it.id)).Select(s => s.fullName).FirstOrDefault();
            }

        });

        return PageResult<AGoodsSpecificationListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建商品规格.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AGoodsSpecificationCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AGoodsSpecificationEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AGoodsSpecificationEntity>(new AGoodsSpecificationEntity()));
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 更新商品规格.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AGoodsSpecificationUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AGoodsSpecificationEntity>();
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_Name,
                it.F_ParentId,
                it.F_SortCode,
                it.F_Description,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除商品规格.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除商品规格.
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
            // 批量删除商品规格
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 商品规格详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AGoodsSpecificationDetailOutput
            {
                id = it.id,
                F_Name = it.F_Name,
                F_ParentId = it.F_ParentId,
                F_SortCode = it.F_SortCode.ToString(),
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            var F_ParentIdData = await _dataInterfaceService.GetDynamicList("F_ParentId", "2034507723852353536", "F_Id", "F_Name", "children");
            if(item.F_ParentId != null)
            {
                var F_ParentIdCascader = item.F_ParentId.ToObject<List<string>>();
                item.F_ParentId = F_ParentIdData.FindAll(it => F_ParentIdCascader.Contains(it.id)).Select(s => s.fullName).FirstOrDefault();
            }

        });
        return data.FirstOrDefault();
    }
}
