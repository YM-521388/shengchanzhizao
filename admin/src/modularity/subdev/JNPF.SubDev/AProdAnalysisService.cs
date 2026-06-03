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
using JNPF.example.Entitys.Dto.AProdAnalysis;
using JNPF.example.Entitys.Dto.AProdAnalysisItem;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_prod_analysis.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdAnalysis", Order = 200)]
[Route("api/example/[controller]")]
public class AProdAnalysisService : IAProdAnalysisService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdAnalysisEntity> _repository;
    private readonly ISqlSugarRepository<AProdAnalysisItemEntity> _repositoryAnalysisItem;
    private readonly ISqlSugarRepository<AProdPlanItemEntity> _repositoryPlanItem;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositoryGoods;
    private readonly ISqlSugarRepository<ABomGoodsEntity> _repositoryBom;
     private readonly ISqlSugarRepository<ABomEntity> _repositoryBomEntity;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"生产计划\",\"field\":\"F_ProdPlanId\"},{\"value\":\"是否考虑主商品库存\",\"field\":\"F_ConsiderMainStock\"},{\"value\":\"是否考虑物料库存\",\"field\":\"F_ConsiderMaterialStock\"},{\"value\":\"是否考虑物料占用\",\"field\":\"F_ConsiderMaterialOccupy\"},{\"value\":\"是否考虑数量向上取整\",\"field\":\"F_RoundUpQty\"},{\"value\":\"是否考虑在制商品\",\"field\":\"F_ConsiderWipGoods\"},{\"value\":\"是否考虑在途商品\",\"field\":\"F_ConsiderTransitGoods\"},{\"value\":\"占用物料能否被其他单据出库\",\"field\":\"F_OccupyAllowOtherOut\"},{\"value\":\"分析人\",\"field\":\"F_AnalysisUserId\"},{\"value\":\"分析时间\",\"field\":\"F_AnalysisTime\"},{\"value\":\"状态\",\"field\":\"F_State\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建用户\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdAnalysisService"/>类型的新实例.
    /// </summary>
    public AProdAnalysisService(
        ISqlSugarRepository<AProdAnalysisEntity> repository,
        ISqlSugarRepository<AProdAnalysisItemEntity> repositoryAnalysisItem,
        ISqlSugarRepository<AProdPlanItemEntity> repositoryPlanItem,
        ISqlSugarRepository<AGoodsEntity> repositoryGoods,
        ISqlSugarRepository<ABomGoodsEntity> repositoryBom,
        ISqlSugarRepository<ABomEntity> repositoryBomEntity,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repository = repository;
        _repositoryAnalysisItem = repositoryAnalysisItem;
        _repositoryPlanItem = repositoryPlanItem;
        _repositoryGoods = repositoryGoods;
        _repositoryBom = repositoryBom;
        _repositoryBomEntity = repositoryBomEntity;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_prod_analysis.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.AProdAnalysisItemList)
            .Where(it => it.DeleteMark == null)
            .Select(it => new AProdAnalysisEntity
            {
                id = it.id,
                F_ProdPlanId = it.F_ProdPlanId,
                F_ConsiderMainStock = it.F_ConsiderMainStock,
                F_ConsiderMaterialOccupy = it.F_ConsiderMaterialOccupy,
                F_ConsiderWipGoods = it.F_ConsiderWipGoods,
                F_OccupyAllowOtherOut = it.F_OccupyAllowOtherOut,
                F_ConsiderMaterialStock = it.F_ConsiderMaterialStock,
                F_RoundUpQty = it.F_RoundUpQty,
                F_ConsiderTransitGoods = it.F_ConsiderTransitGoods,
                F_AnalysisUserId = it.F_AnalysisUserId,
                F_AnalysisTime = it.F_AnalysisTime,
                F_State = it.F_State,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                AProdAnalysisItemList = it.AProdAnalysisItemList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AProdAnalysisInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField4d57ab, async aProdAnalysisItem =>
        {
            // 创建用户
            aProdAnalysisItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aProdAnalysisItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aProdAnalysisItem.F_CreatorTime = aProdAnalysisItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

    /// <summary>
    /// 获取a_prod_analysis列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdAnalysisListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aProdAnalysisItemAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<AProdAnalysisListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_prod_analysis"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_prod_analysis_item"))) aProdAnalysisItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_prod_analysis_item"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AProdAnalysisEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AProdAnalysisItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField4d57ab-", entityInfo, 1);
        List<IConditionalModel> aProdAnalysisItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aProdAnalysisItemConditionalModel.AddRange(aProdAnalysisItemAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AProdAnalysisEntity>();
        var F_AnalysisUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_AnalysisUserId").DbColumnName;
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_AnalysisUserIdDbColumnName, input.F_AnalysisUserId))
            .WhereIF(input.F_AnalysisTime?.Count() > 0, it => SqlFunc.Between(it.F_AnalysisTime, input.F_AnalysisTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_AnalysisTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_State), it => it.F_State.Contains(input.F_State))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .Where(authorizeWhere)
            .WhereIF(aProdAnalysisItemAuthorizeWhere?.Count > 0, it => it.AProdAnalysisItemList.Any(aProdAnalysisItemAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aProdAnalysisItemConditionalModel.Count > 0, it => it.AProdAnalysisItemList.Any(aProdAnalysisItemConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new AProdAnalysisListOutput
            {
                id = it.id,
                F_ProdPlanId = it.F_ProdPlanId,
                F_ConsiderMainStock = it.F_ConsiderMainStock,
                F_ConsiderMaterialOccupy = it.F_ConsiderMaterialOccupy,
                F_ConsiderWipGoods = it.F_ConsiderWipGoods,
                F_OccupyAllowOtherOut = it.F_OccupyAllowOtherOut,
                F_ConsiderMaterialStock = it.F_ConsiderMaterialStock,
                F_RoundUpQty = it.F_RoundUpQty,
                F_ConsiderTransitGoods = it.F_ConsiderTransitGoods,
                F_AnalysisUserId = it.F_AnalysisUserId,
                F_AnalysisTime = it.F_AnalysisTime.Value.ToString("yyyy-MM-dd"),
                F_State = it.F_State,
                F_Description = it.F_Description,
                F_CreatorUserId = it.F_CreatorUserId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 是否考虑主商品库存
            var F_ConsiderMainStockData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_ConsiderMainStock != null)
            {
                item.F_ConsiderMainStock = F_ConsiderMainStockData.Find(it => item.F_ConsiderMainStock.Equals(it.id))?.fullName;
            }

            // 是否考虑物料占用
            var F_ConsiderMaterialOccupyData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_ConsiderMaterialOccupy != null)
            {
                item.F_ConsiderMaterialOccupy = F_ConsiderMaterialOccupyData.Find(it => item.F_ConsiderMaterialOccupy.Equals(it.id))?.fullName;
            }

            // 是否考虑在制商品
            var F_ConsiderWipGoodsData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_ConsiderWipGoods != null)
            {
                item.F_ConsiderWipGoods = F_ConsiderWipGoodsData.Find(it => item.F_ConsiderWipGoods.Equals(it.id))?.fullName;
            }

            // 占用物料能否被其他单据出库
            var F_OccupyAllowOtherOutData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_OccupyAllowOtherOut != null)
            {
                item.F_OccupyAllowOtherOut = F_OccupyAllowOtherOutData.Find(it => item.F_OccupyAllowOtherOut.Equals(it.id))?.fullName;
            }

            // 是否考虑物料库存
            var F_ConsiderMaterialStockData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_ConsiderMaterialStock != null)
            {
                item.F_ConsiderMaterialStock = F_ConsiderMaterialStockData.Find(it => item.F_ConsiderMaterialStock.Equals(it.id))?.fullName;
            }

            // 是否考虑数量向上取整
            var F_RoundUpQtyData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_RoundUpQty != null)
            {
                item.F_RoundUpQty = F_RoundUpQtyData.Find(it => item.F_RoundUpQty.Equals(it.id))?.fullName;
            }

            // 是否考虑在途商品
            var F_ConsiderTransitGoodsData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_ConsiderTransitGoods != null)
            {
                item.F_ConsiderTransitGoods = F_ConsiderTransitGoodsData.Find(it => item.F_ConsiderTransitGoods.Equals(it.id))?.fullName;
            }

            // 分析人
            if(item.F_AnalysisUserId != null)
            {
                item.F_AnalysisUserId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_AnalysisUserId)))?.RealName;
            }

            // 创建用户
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;

        });

        var resData = data.list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<AProdAnalysisEntity>(new AProdAnalysisEntity()));
        ConfigModel tableField4d57abConfig = new ConfigModel
        {
            tableName = "a_prod_analysis_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "物料分析商品列表信息",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<AProdAnalysisItemEntity>(new AProdAnalysisItemEntity())
        };
        FieldsModel tableField4d57ab = new FieldsModel()
        {
            __config__ = tableField4d57abConfig,
            __vModel__ = "tableField4d57ab"
        };
        fieldList.Add(tableField4d57ab);

        resData = await _controlParsing.GetParsDataList(resData, "F_ProdPlanId,tableField4d57ab-F_GoodsId", "popupSelect", _userManager.TenantId, fieldList);

        data.list = resData.ToObject<List<AProdAnalysisListOutput>>(CommonConst.options);
        return PageResult<AProdAnalysisListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_prod_analysis.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] AProdAnalysisCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdAnalysisEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdAnalysisEntity>(new AProdAnalysisEntity()));
        ConfigModel tableField4d57abConfig = new ConfigModel
        {
            tableName = "a_prod_analysis_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "物料分析商品列表信息",
            children = ExportImportDataHelper.GetTemplateParsing<AProdAnalysisItemEntity>(new AProdAnalysisItemEntity())
        };
        FieldsModel tableField4d57abModel = new FieldsModel()
        {
            __config__ = tableField4d57abConfig,
            __vModel__ = "tableField4d57ab"
        };
        fieldList.Add(tableField4d57abModel);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();


        var aProdAnalysisItemEntityList = input.tableField4d57ab.Adapt<List<AProdAnalysisItemEntity>>();
        if(aProdAnalysisItemEntityList != null)
        {
            foreach (var item in aProdAnalysisItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.AProdAnalysisItemList = aProdAnalysisItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.AProdAnalysisItemList)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新a_prod_analysis.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] AProdAnalysisUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdAnalysisEntity>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdAnalysisEntity>(new AProdAnalysisEntity()));
        ConfigModel tableField4d57abConfig = new ConfigModel
        {
            tableName = "a_prod_analysis_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "物料分析商品列表信息",
            children = ExportImportDataHelper.GetTemplateParsing<AProdAnalysisItemEntity>(new AProdAnalysisItemEntity())
        };
        FieldsModel tableField4d57abModel = new FieldsModel()
        {
            __config__ = tableField4d57abConfig,
            __vModel__ = "tableField4d57ab"
        };
        fieldList.Add(tableField4d57abModel);

        // 移除物料分析商品列表信息可能被删除数据
        if (input.tableField4d57ab !=null && input.tableField4d57ab.Any())
            await _repository.AsSugarClient().Deleteable<AProdAnalysisItemEntity>().Where(it => it.F_AnalysisId == entity.id && !input.tableField4d57ab.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<AProdAnalysisItemEntity>().Where(it => it.F_AnalysisId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增物料分析商品列表信息新数据
        var aProdAnalysisItemEntityList = input.tableField4d57ab.Adapt<List<AProdAnalysisItemEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_ProdPlanId,
                    it.F_ConsiderMainStock,
                    it.F_ConsiderMaterialOccupy,
                    it.F_ConsiderWipGoods,
                    it.F_OccupyAllowOtherOut,
                    it.F_ConsiderMaterialStock,
                    it.F_RoundUpQty,
                    it.F_ConsiderTransitGoods,
                    it.F_AnalysisUserId,
                    it.F_AnalysisTime,
                    it.F_State,
                    it.F_Description,
                })
                .ExecuteCommandAsync();

            if(aProdAnalysisItemEntityList != null)
            {
                foreach (var item in aProdAnalysisItemEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_AnalysisId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_AnalysisId = entity.id;
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
    /// 删除a_prod_analysis.
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
    /// 批量删除a_prod_analysis.
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
            // 批量删除a_prod_analysis
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }


    /// <summary>
    /// a_prod_analysis详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.AProdAnalysisItemList)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.id == id)
            .Select(it => new AProdAnalysisDetailOutput
            {
                id = it.id,
                F_ProdPlanId = it.F_ProdPlanId,
                F_ConsiderMainStock = it.F_ConsiderMainStock,
                F_ConsiderMaterialOccupy = it.F_ConsiderMaterialOccupy,
                F_ConsiderWipGoods = it.F_ConsiderWipGoods,
                F_OccupyAllowOtherOut = it.F_OccupyAllowOtherOut,
                F_ConsiderMaterialStock = it.F_ConsiderMaterialStock,
                F_RoundUpQty = it.F_RoundUpQty,
                F_ConsiderTransitGoods = it.F_ConsiderTransitGoods,
                F_AnalysisUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_AnalysisUserId)).Select(u => u.RealName),
                F_AnalysisTime = it.F_AnalysisTime.Value.ToString("yyyy-MM-dd"),
                F_State = it.F_State,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableField4d57ab = it.AProdAnalysisItemList.Adapt<List<AProdAnalysisItemDetailOutput>>(),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 是否考虑主商品库存
            var F_ConsiderMainStockData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_ConsiderMainStock != null)
            {
                item.F_ConsiderMainStock = F_ConsiderMainStockData.Find(it => item.F_ConsiderMainStock.Equals(it.id))?.fullName;
            }

            // 是否考虑物料占用
            var F_ConsiderMaterialOccupyData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_ConsiderMaterialOccupy != null)
            {
                item.F_ConsiderMaterialOccupy = F_ConsiderMaterialOccupyData.Find(it => item.F_ConsiderMaterialOccupy.Equals(it.id))?.fullName;
            }

            // 是否考虑在制商品
            var F_ConsiderWipGoodsData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_ConsiderWipGoods != null)
            {
                item.F_ConsiderWipGoods = F_ConsiderWipGoodsData.Find(it => item.F_ConsiderWipGoods.Equals(it.id))?.fullName;
            }

            // 占用物料能否被其他单据出库
            var F_OccupyAllowOtherOutData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_OccupyAllowOtherOut != null)
            {
                item.F_OccupyAllowOtherOut = F_OccupyAllowOtherOutData.Find(it => item.F_OccupyAllowOtherOut.Equals(it.id))?.fullName;
            }

            // 是否考虑物料库存
            var F_ConsiderMaterialStockData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_ConsiderMaterialStock != null)
            {
                item.F_ConsiderMaterialStock = F_ConsiderMaterialStockData.Find(it => item.F_ConsiderMaterialStock.Equals(it.id))?.fullName;
            }

            // 是否考虑数量向上取整
            var F_RoundUpQtyData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_RoundUpQty != null)
            {
                item.F_RoundUpQty = F_RoundUpQtyData.Find(it => item.F_RoundUpQty.Equals(it.id))?.fullName;
            }

            // 是否考虑在途商品
            var F_ConsiderTransitGoodsData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"2\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_ConsiderTransitGoods != null)
            {
                item.F_ConsiderTransitGoods = F_ConsiderTransitGoodsData.Find(it => item.F_ConsiderTransitGoods.Equals(it.id))?.fullName;
            }

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField4d57ab), async aProdAnalysisItem =>
        {
            var aProdAnalysis = data.ToList().Find(it => it.id.Equals(aProdAnalysisItem.F_AnalysisId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建用户
            aProdAnalysisItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aProdAnalysisItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aProdAnalysisItem.F_CreatorTime = aProdAnalysisItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<AProdAnalysisEntity>(new AProdAnalysisEntity()));
        ConfigModel tableField4d57abConfig = new ConfigModel
        {
            tableName = "a_prod_analysis_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "物料分析商品列表信息",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<AProdAnalysisItemEntity>(new AProdAnalysisItemEntity())
        };
        FieldsModel tableField4d57ab = new FieldsModel()
        {
            __config__ = tableField4d57abConfig,
            __vModel__ = "tableField4d57ab"
        };
        fieldList.Add(tableField4d57ab);

        resData = await _controlParsing.GetParsDataList(resData,"F_ProdPlanId,tableField4d57ab-F_GoodsId","popupSelect",_userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }




    /// <summary>
    /// 根据生产计划ID获取BOM物料信息.
    /// </summary>
    /// <param name="prodPlanId">生产计划ID.</param>
    /// <returns>BOM物料信息列表，包含BOM头商品信息和子物料信息.</returns>
    [HttpGet("GetBomMaterials/{prodPlanId}")]
    public async Task<dynamic> GetBomMaterials(string prodPlanId)
    {
        if (string.IsNullOrEmpty(prodPlanId))
        {
            return new List<object>();
        }

        // 1. 根据生产计划ID获取对应的BOM IDs（从生产计划项表中获取）
        var bomIds = await _repositoryPlanItem.AsQueryable()
            .Where(it => it.F_ProdPlanId == prodPlanId && it.DeleteMark == null)
            .Where(it => !string.IsNullOrEmpty(it.F_BOMId)) // 过滤掉空的BOM ID
            .Select(it => it.F_BOMId)
            .Distinct()
            .ToListAsync();

        if (!bomIds.Any())
        {
            return new List<object>();
        }

        var result = new List<object>();

        // 2. 获取BOM头信息（ABomEntity中的商品信息）
        var bomHeaders = await _repositoryBomEntity.AsQueryable()
            .Where(it => bomIds.Contains(it.id) && it.DeleteMark == null)
            .Select(it => new {
                F_Id = it.id,
                F_BomId = (string?)null, // BOM头没有F_BomId，设为null
                F_GoodsId = it.F_GoodsId,
                F_Unit = (int?)null // BOM头没有单位用量，设为null
            })
            .ToListAsync();

        result.AddRange(bomHeaders);

        // 3. 获取子物料信息（ABomGoodsEntity中的物料信息）
        var materials = await _repositoryBom.AsQueryable()
            .Where(it => bomIds.Contains(it.F_ParentId) && it.DeleteMark == null)
            .Select(it => new {
                F_Id = it.F_Id,
                F_BomId = it.F_BomId,
                F_GoodsId = it.F_GoodsId,
                F_Unit = it.F_Unit
            })
            .ToListAsync();

        result.AddRange(materials);

        return result;
    }

    /// <summary>
    /// 根据分析ID获取物料分析商品信息并按商品来源分类.
    /// </summary>
    /// <param name="analysisId">分析ID.</param>
    /// <returns>按商品来源分类的物料分析商品信息.</returns>
    [HttpGet("GetAnalysisItemsBySource/{analysisId}")]
    public async Task<dynamic> GetAnalysisItemsBySource(string analysisId)
    {
        if (string.IsNullOrEmpty(analysisId))
        {
            return new
            {
                source0 = new List<object>(),
                source1 = new List<object>(),
                source2 = new List<object>()
            };
        }

        // 获取匹配的分析项数据
        var analysisItems = await _repositoryAnalysisItem.AsQueryable()
            .Where(it => it.F_AnalysisId == analysisId && it.DeleteMark == null)
            .Select(it => new {
                F_GoodsId = it.F_GoodsId,
                F_ParentId = it.F_ParentId,
                F_Unit = it.F_Unit
            })
            .ToListAsync();

        if (!analysisItems.Any())
        {
            return new
            {
                source0 = new List<object>(),
                source1 = new List<object>(),
                source2 = new List<object>()
            };
        }

        // 获取商品ID列表
        var goodsIds = analysisItems.Where(it => !string.IsNullOrEmpty(it.F_GoodsId)).Select(it => it.F_GoodsId).Distinct().ToList();

        // 获取商品来源信息
        var goodsSources = await _repositoryGoods.AsQueryable()
            .Where(it => goodsIds.Contains(it.id) && it.DeleteMark == null)
            .Select(it => new {
                F_Id = it.id,
                F_Source = it.F_Source
            })
            .ToListAsync();

        // 创建分类结果
        var source0Items = new List<object>();
        var source1Items = new List<object>();
        var source2Items = new List<object>();

        // 根据商品来源分类
        foreach (var item in analysisItems)
        {
            if (string.IsNullOrEmpty(item.F_GoodsId))
                continue;

            var goodsSource = goodsSources.FirstOrDefault(g => g.F_Id == item.F_GoodsId);
            if (goodsSource == null)
                continue;

            var itemData = new
            {
                F_GoodsId = item.F_GoodsId,
                F_ParentId = item.F_ParentId,
                F_Unit = item.F_Unit
            };

            // 根据F_Source值分类
            if (goodsSource.F_Source == "0")
            {
                source0Items.Add(itemData);
            }
            else if (goodsSource.F_Source == "1")
            {
                source1Items.Add(itemData);
            }
            else if (goodsSource.F_Source == "2")
            {
                source2Items.Add(itemData);
            }
        }

        return new
        {
            source0 = source0Items,
            source1 = source1Items,
            source2 = source2Items
        };
    }







}
