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
using JNPF.example.Entitys.Dto.AProdPlanItem;
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
/// 业务实现：生产计划商品列表.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdPlanItem", Order = 200)]
[Route("api/example/[controller]")]
public class AProdPlanItemService : IAProdPlanItemService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdPlanItemEntity> _repository;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"生产计划ID\",\"field\":\"F_ProdPlanId\"},{\"value\":\"商品\",\"field\":\"F_GoodsId\"},{\"value\":\"计划数量\",\"field\":\"F_Num\"},{\"value\":\"商品BOM\",\"field\":\"F_BOMId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdPlanItemService"/>类型的新实例.
    /// </summary>
    public AProdPlanItemService(
        ISqlSugarRepository<AProdPlanItemEntity> repository,
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
    /// 获取生产计划商品列表.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .FirstAsync(it => it.F_Id.Equals(id))).Adapt<AProdPlanItemInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取生产计划商品列表列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdPlanItemListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AProdPlanItemListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.F_Id))
            .WhereIF(!string.IsNullOrEmpty(input.F_GoodsId), it => it.F_GoodsId.Equals(input.F_GoodsId))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProdPlanId), it => it.F_ProdPlanId.Equals(input.F_ProdPlanId))
            .Where(authorizeWhere)
            .Select(it => new AProdPlanItemListOutput
            {
                id = it.F_Id,
                F_ProdPlanId = it.F_ProdPlanId,
                F_GoodsId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsById = it.F_GoodsId,
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_Unit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit)) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                F_Num = it.F_Num.ToString(),
                F_BOMId = SqlFunc.Subqueryable<ABomEntity>().EnableTableFilter().Where(g => g.id == it.F_BOMId && g.DeleteMark == null).Select(g => g.F_BomName),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
        });

        return PageResult<AProdPlanItemListOutput>.SqlSugarPageResult(data);
    }

    [HttpPost("NotList")]
    public async Task<dynamic> GetNotList([FromBody] AProdPlanItemListQueryInput input)
    {
        var data = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_GoodsId), it => it.F_GoodsId.Equals(input.F_GoodsId))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProdPlanId), it => it.F_ProdPlanId.Equals(input.F_ProdPlanId))
            .Select(it => new AProdPlanItemListOutput
            {
                id = it.F_Id,
                F_ProdPlanId = it.F_ProdPlanId,
                F_GoodsId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsById = it.F_GoodsId,
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_Unit = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit),
                F_Num = it.F_Num.ToString(),
                F_BOMId = SqlFunc.Subqueryable<ABomEntity>().EnableTableFilter().Where(g => g.id == it.F_BOMId && g.DeleteMark == null).Select(g => g.F_BomName),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(item.F_Unit))
            {
                var F_UnitIdCascader = item.F_Unit.ToObject<List<string>>();
                item.F_Unit = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));

                if (F_UnitIdCascader.Count > 1)
                {
                    item.F_NumUnit = item.F_Num.ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    item.F_NumUnit = item.F_Num.ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }
        });

        return PageResult<AProdPlanItemListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建生产计划商品列表.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AProdPlanItemCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdPlanItemEntity>();
        entity.F_Id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdPlanItemEntity>(new AProdPlanItemEntity()));
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 更新生产计划商品列表.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AProdPlanItemUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdPlanItemEntity>();
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_ProdPlanId,
                it.F_GoodsId,
                it.F_Num,
                it.F_BOMId,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除生产计划商品列表.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.F_Id.Equals(id)).ExecuteCommandAsync();   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除生产计划商品列表.
    /// </summary>
    /// <param name="input">主键数组.</param>
    /// <returns></returns>
    [HttpPost("batchRemove")]
    [UnitOfWork]
    public async Task BatchRemove([FromBody] BatchRemoveInput input)
    {
        var entitys = await _repository.AsQueryable().In(it => it.F_Id, input.ids).ToListAsync();
        if (entitys.Count > 0)
        {
            // 批量删除生产计划商品列表
            await _repository.AsDeleteable().In(it => it.F_Id, input.ids).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 生产计划商品列表详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AProdPlanItemDetailOutput
            {
                id = it.F_Id,
                F_ProdPlanId = it.F_ProdPlanId,
                F_GoodsId = it.F_GoodsId,
                F_Num = it.F_Num,
                F_BOMId = it.F_BOMId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 商品ID
            if(item.F_GoodsId != null)
            {
                linkageParameters = new List<DataInterfaceParameter>();
                linkageParameters.Add(new DataInterfaceParameter
                {
                    field = "contractId",
                    relationField = string.Empty,
                    isSubTable = false,
                    dataType = "varchar",
                    defaultValue = "",
                    fieldName = "合同ID",
                    sourceType = 3
                });
                var F_GoodsIdData = await _dataInterfaceService.GetDynamicList("F_GoodsId", "2012085692393459712", "id", "F_GoodsName", "", linkageParameters);
                item.F_GoodsId = F_GoodsIdData.Find(it => it.id.Equals(item.F_GoodsId))?.fullName;
            }

            // 商品BOM
            if(item.F_BOMId != null)
            {
                linkageParameters = new List<DataInterfaceParameter>();
                linkageParameters.Add(new DataInterfaceParameter
                {
                    field = "goodsId",
                    relationField = string.Empty,
                    isSubTable = false,
                    dataType = "varchar",
                    defaultValue = "",
                    fieldName = "合同ID",
                    sourceType = 3
                });
                var F_BOMIdData = await _dataInterfaceService.GetDynamicList("F_BOMId", "2013188646957617152", "id", "F_BomName", "", linkageParameters);
                item.F_BOMId = F_BOMIdData.Find(it => it.id.Equals(item.F_BOMId))?.fullName;
            }

        });
        return data.FirstOrDefault();
    }
}
