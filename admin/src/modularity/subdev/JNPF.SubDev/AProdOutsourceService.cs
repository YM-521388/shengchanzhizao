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
using JNPF.example.Entitys.Dto.AProdOutsource;
using JNPF.example.Entitys.Dto.AProdOutsourceItem;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_prod_outsource.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdOutsource", Order = 200)]
[Route("api/example/[controller]")]
public class AProdOutsourceService : IAProdOutsourceService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdOutsourceEntity> _repository;
    private readonly ISqlSugarRepository<AProdOutsourceItemEntity> _repositoryItem;
    private readonly ISqlSugarRepository<ASupplierEntity> _repositorysupplier;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<APuStockFgItemEntity> _repositoryStockFgItem;
    private readonly ISqlSugarRepository<AGoodsCategoryEntity> _repositorysp;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"外协工单号\",\"field\":\"F_OutsourceNo\"},{\"value\":\"商品\",\"field\":\"F_GoodsId\"},{\"value\":\"计划数\",\"field\":\"F_PlanNum\"},{\"value\":\"BOMID\",\"field\":\"F_BOMId\"},{\"value\":\"供应商联系人\",\"field\":\"F_ContactPerson\"},{\"value\":\"供应商\",\"field\":\"F_SupplierId\"},{\"value\":\"供应商联系人电话\",\"field\":\"F_ContactPhone\"},{\"value\":\"供应商地区\",\"field\":\"F_Region\"},{\"value\":\"外协单价\",\"field\":\"F_Price\"},{\"value\":\"供应商详细地址\",\"field\":\"F_DetailAddress\"},{\"value\":\"交货日期\",\"field\":\"F_DeliveryDate\"},{\"value\":\"优先级\",\"field\":\"F_Priority\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"},{\"value\":\"创建用户\",\"field\":\"F_CreatorUserId\"},{\"value\":\"描述\",\"field\":\"F_Description\"},{\"value\":\"状态\",\"field\":\"F_State\"},{\"value\":\"图纸\",\"field\":\"F_Files\"},{\"value\":\"计划结束\",\"field\":\"F_PlanEndDate\"},{\"value\":\"计划开始\",\"field\":\"F_PlanStartDate\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdOutsourceService"/>类型的新实例.
    /// </summary>
    public AProdOutsourceService(
        ISqlSugarRepository<AProdOutsourceItemEntity> repositoryItem,
        ISqlSugarRepository<APuStockFgItemEntity> repositoryStockFgItem,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<AProdOutsourceEntity> repository,
        ISqlSugarRepository<ASupplierEntity> repositorysupplier,
        ISqlSugarRepository<AGoodsCategoryEntity> repositorysp,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryItem = repositoryItem;
        _repositoryStockFgItem = repositoryStockFgItem;
        _repositoryInventory = repositoryInventory;
        _repository = repository;

        _repositorysupplier = repositorysupplier;
        _repositorysp = repositorysp;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_prod_outsource.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.AProdOutsourceItemList)
            .Where(it => it.DeleteMark == null)
            .Select(it => new AProdOutsourceEntity
            {
                id = it.id,
                F_GoodsId = it.F_GoodsId,
                F_BOMId = it.F_BOMId,
                F_OutsourceNo = it.F_OutsourceNo,
                F_PlanNum = it.F_PlanNum,
                F_SupplierId = it.F_SupplierId,
                F_ContactPerson = it.F_ContactPerson,
                F_ContactPhone = it.F_ContactPhone,
                F_Region = it.F_Region,
                F_DetailAddress = it.F_DetailAddress,
                F_Price = it.F_Price,
                F_DeliveryDate = it.F_DeliveryDate,
                F_Priority = it.F_Priority,
                F_PlanStartDate = it.F_PlanStartDate,
                F_PlanEndDate = it.F_PlanEndDate,
                F_Files = it.F_Files,
                F_State = it.F_State,
                F_Description = it.F_Description,
                F_PauseReason = it.F_PauseReason,
                F_CancelReason = it.F_CancelReason,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                AProdOutsourceItemList = it.AProdOutsourceItemList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AProdOutsourceInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField7a8044, async aProdOutsourceItem =>
        {
            // 创建用户
            aProdOutsourceItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aProdOutsourceItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aProdOutsourceItem.F_CreatorTime = aProdOutsourceItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            aProdOutsourceItem.F_GoodsNo = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(it => it.id.Equals(aProdOutsourceItem.F_GoodsId)).Select(it => it.F_GoodsNo).FirstAsync();
            aProdOutsourceItem.F_Specification = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(it => it.id.Equals(aProdOutsourceItem.F_GoodsId)).Select(it => it.F_Specification).FirstAsync();

            var unit = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(it => it.id.Equals(aProdOutsourceItem.F_GoodsId)).Select(it => it.F_Unit).FirstAsync();

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(unit))
            {
                var F_UnitIdCascader = unit.ToObject<List<string>>();
                aProdOutsourceItem.F_UnitTwo = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));

                if (F_UnitIdCascader.Count > 1)
                {
                    aProdOutsourceItem.F_NumUnit = (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    aProdOutsourceItem.F_NumUnit =  (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }

            aProdOutsourceItem.F_InventoryNum = (await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == aProdOutsourceItem.F_GoodsId && it.F_Num > 0).SumAsync(it => it.F_Num)) ?? 0;




        });
        return data;
    }

    /// <summary>
    /// 获取a_prod_outsource列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdOutsourceListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aProdOutsourceItemAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<AProdOutsourceListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_prod_outsource"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_prod_outsource_item"))) aProdOutsourceItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_prod_outsource_item"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AProdOutsourceEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AProdOutsourceItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField7a8044-", entityInfo, 1);
        List<IConditionalModel> aProdOutsourceItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aProdOutsourceItemConditionalModel.AddRange(aProdOutsourceItemAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AProdOutsourceEntity>();
        var startF_PlanNum = input.F_PlanNum?.FirstOrDefault()?.ParseToDecimal() == null ? decimal.MinValue : input.F_PlanNum?.First();
        var endF_PlanNum = input.F_PlanNum?.LastOrDefault()?.ParseToDecimal() == null ? decimal.MaxValue : input.F_PlanNum?.Last();
        var F_SupplierIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_SupplierId").DbColumnName;
        var F_PriorityDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_Priority").DbColumnName;
        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_OutsourceNo), it => it.F_OutsourceNo.Contains(input.F_OutsourceNo))
            .WhereIF(input.F_PlanNum?.Count() > 0, it => SqlFunc.Between(it.F_PlanNum, startF_PlanNum, endF_PlanNum))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_SupplierIdDbColumnName, input.F_SupplierId))
            .WhereIF(!string.IsNullOrEmpty(input.F_ContactPhone), it => it.F_ContactPhone.Contains(input.F_ContactPhone))
            .WhereIF(!string.IsNullOrEmpty(input.F_DetailAddress), it => it.F_DetailAddress.Contains(input.F_DetailAddress))
            .WhereIF(input.F_DeliveryDate?.Count() > 0, it => SqlFunc.Between(it.F_DeliveryDate, input.F_DeliveryDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_DeliveryDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_PriorityDbColumnName, input.F_Priority))
            .WhereIF(input.F_PlanStartDate?.Count() > 0, it => SqlFunc.Between(it.F_PlanStartDate, input.F_PlanStartDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_PlanStartDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(input.F_PlanEndDate?.Count() > 0, it => SqlFunc.Between(it.F_PlanEndDate, input.F_PlanEndDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_PlanEndDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .WhereIF(aProdOutsourceItemAuthorizeWhere?.Count > 0, it => it.AProdOutsourceItemList.Any(aProdOutsourceItemAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aProdOutsourceItemConditionalModel.Count > 0, it => it.AProdOutsourceItemList.Any(aProdOutsourceItemConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new AProdOutsourceListOutput
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
            .Select(it => new AProdOutsourceListOutput
            {
                id = it.id,
                F_GoodsId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_GoodsId)).Select(u => SqlFunc.MergeString(u.F_GoodsName, "-", u.F_GoodsNo)),
                F_BOMId = it.F_BOMId,
                F_OutsourceNo = it.F_OutsourceNo,
                F_PlanNum = it.F_PlanNum.ToString(),
                F_SupplierId = SqlFunc.Subqueryable<ASupplierEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_SupplierId)).Select(u => u.F_SupplierName),
                F_ContactPerson = it.F_ContactPerson,
                F_ContactPhone = it.F_ContactPhone,
                F_Region = it.F_Region,
                F_DetailAddress = it.F_DetailAddress,
                F_Price = it.F_Price,
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_Priority = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Priority) && dic.DictionaryTypeId.Equals("2013182853290004480")).Select(dic => dic.FullName),
                F_PriorityCode = it.F_Priority,
                F_PlanStartDate = it.F_PlanStartDate.Value.ToString("yyyy-MM-dd"),
                F_PlanEndDate = it.F_PlanEndDate.Value.ToString("yyyy-MM-dd"),
                F_Files = it.F_Files,
                F_State = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_State) && dic.DictionaryTypeId.Equals("2013819135649255424")).Select(dic => dic.FullName),
                F_StateId = it.F_State,
                F_Description = it.F_Description,
                F_PauseReason = it.F_PauseReason,
                F_CancelReason = it.F_CancelReason,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 供应商地区
            if (item.F_Region != null)
            {
                var F_RegionAreaSelect = item.F_Region.ToObject<List<string>>();
                item.F_Region = string.Join("/", await _repository.AsSugarClient().Queryable<ProvinceEntity>().Where(it => F_RegionAreaSelect.Contains(it.Id)).Select(it => it.FullName).ToListAsync());
            }

            if (item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }

        });

        data.pagination = idsQ.pagination;

        return PageResult<AProdOutsourceListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取商品列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("GoodsList")]
    public async Task<dynamic> GetGoodsList([FromBody] AProdOutsourceListQueryInput input)
    {
        var idsQ = await _repositoryItem.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_OutsourceId), it => it.F_OutsourceId.Contains(input.F_OutsourceId))
            .Select(it => new AProdOutsourceItemInfoOutput
            {
                F_Id = it.F_Id,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            //把多表结果变成“单表”再排序：
            .MergeTable()
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .ToPagedListAsync(input.currentPage, input.pageSize);

        var ids = idsQ.list.Select(x => x.F_Id).ToList();   // 20 个 long

        // 2. 再用 ids 一次性补全字段（此时只有 20 行，随便 join 18 张表都毫秒级）
        var data = await _repositoryItem.AsQueryable()
            .Where(it => ids.Contains(it.F_Id))
            .Select(it => new AProdOutsourceItemInfoOutput
            {
                F_Id = it.F_Id,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_UnitTwo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit),
                F_Unit = it.F_Unit,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(item.F_UnitTwo))
            {
                var F_UnitIdCascader = item.F_UnitTwo.ToObject<List<string>>();
                item.F_UnitTwo = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));


                if (F_UnitIdCascader.Count > 1)
                {
                    item.F_NumUnit = item.F_Unit.ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    item.F_NumUnit = item.F_Unit.ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }
        });

        data.pagination = idsQ.pagination;

        return PageResult<AProdOutsourceItemInfoOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_prod_outsource.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] AProdOutsourceCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdOutsourceEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdOutsourceEntity>(new AProdOutsourceEntity()));
        ConfigModel tableField7a8044Config = new ConfigModel
        {
            tableName = "a_prod_outsource_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "用料清单",
            children = ExportImportDataHelper.GetTemplateParsing<AProdOutsourceItemEntity>(new AProdOutsourceItemEntity())
        };
        FieldsModel tableField7a8044Model = new FieldsModel()
        {
            __config__ = tableField7a8044Config,
            __vModel__ = "tableField7a8044"
        };
        fieldList.Add(tableField7a8044Model);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        // 如果前端没有传入 F_OutsourceNo，自动生成编号
        if (string.IsNullOrEmpty(entity.F_OutsourceNo))
        {
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);

            // 查询今天已创建的记录数量
            var todayCount = await _repository.AsQueryable()
                .Where(it => it.F_CreatorTime >= today && it.F_CreatorTime < tomorrow && it.F_OutsourceNo.Contains($"WX{DateTime.Now.ToString("yyyyMMdd")}-"))
                .CountAsync();

            // 生成编号：WX + 日期 + - + 序号
            entity.F_OutsourceNo = $"WX{DateTime.Now.ToString("yyyyMMdd")}-{(todayCount + 1)}";
        }

        var aProdOutsourceItemEntityList = input.tableField7a8044.Adapt<List<AProdOutsourceItemEntity>>();
        if(aProdOutsourceItemEntityList != null)
        {
            foreach (var item in aProdOutsourceItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.AProdOutsourceItemList = aProdOutsourceItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.AProdOutsourceItemList)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新a_prod_outsource.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] AProdOutsourceUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdOutsourceEntity>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdOutsourceEntity>(new AProdOutsourceEntity()));
        ConfigModel tableField7a8044Config = new ConfigModel
        {
            tableName = "a_prod_outsource_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "用料清单",
            children = ExportImportDataHelper.GetTemplateParsing<AProdOutsourceItemEntity>(new AProdOutsourceItemEntity())
        };
        FieldsModel tableField7a8044Model = new FieldsModel()
        {
            __config__ = tableField7a8044Config,
            __vModel__ = "tableField7a8044"
        };
        fieldList.Add(tableField7a8044Model);

        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_OutsourceNo.Equals(input.F_OutsourceNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "外协工单号");

        if (string.IsNullOrEmpty(entity.F_OutsourceNo))
        {
            entity.F_OutsourceNo = oldEntity.F_OutsourceNo;
        }

        // 移除外协工单用料清单可能被删除数据
        if (input.tableField7a8044 !=null && input.tableField7a8044.Any())
            await _repository.AsSugarClient().Deleteable<AProdOutsourceItemEntity>().Where(it => it.F_OutsourceId == entity.id && !input.tableField7a8044.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<AProdOutsourceItemEntity>().Where(it => it.F_OutsourceId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增外协工单用料清单新数据
        var aProdOutsourceItemEntityList = input.tableField7a8044.Adapt<List<AProdOutsourceItemEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_GoodsId,
                    it.F_BOMId,
                    it.F_OutsourceNo,
                    it.F_PlanNum,
                    it.F_SupplierId,
                    it.F_ContactPerson,
                    it.F_ContactPhone,
                    it.F_Region,
                    it.F_DetailAddress,
                    it.F_Price,
                    it.F_DeliveryDate,
                    it.F_Priority,
                    it.F_PlanStartDate,
                    it.F_PlanEndDate,
                    it.F_Files,
                    it.F_State,
                    it.F_Description,
                })
                .ExecuteCommandAsync();

            if(aProdOutsourceItemEntityList != null)
            {
                foreach (var item in aProdOutsourceItemEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_OutsourceId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_OutsourceId = entity.id;
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
    /// 修改状态.
    /// </summary>
    [HttpGet("Check/{id}/{state}")]
    public async Task Check(string id, string state)
    {
        AProdOutsourceEntity? entity = await _repository.GetFirstAsync(it => it.id == id);

        if (state == "5")
        {
            int isOk = await _repository.AsUpdateable().SetColumns(it => new AProdOutsourceEntity()
            {
                F_State = "1",
                F_PauseReason = null,
            }).Where(it => it.id == id).ExecuteCommandAsync();

        }
        else if (state == "6")
        {
            var oldEntity = await _repository.GetFirstAsync(it => it.id == entity.id);
            if (oldEntity.F_State == "1")
            {
                int isOk = await _repository.AsUpdateable().SetColumns(it => new AProdOutsourceEntity()
                {
                    F_State = "0",
                }).Where(it => it.id == id).ExecuteCommandAsync();
            }
            else if(oldEntity.F_State == "2") {
                int isOk = await _repository.AsUpdateable().SetColumns(it => new AProdOutsourceEntity()
                {
                    F_State = "1",
                }).Where(it => it.id == id).ExecuteCommandAsync();
            }
        }
        else
        {
            int isOk = await _repository.AsUpdateable().SetColumns(it => new AProdOutsourceEntity()
            {
                F_State = state,
            }).Where(it => it.id == id).ExecuteCommandAsync();

        }
    }

    /// <summary>
    /// 暂停、取消原因.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("ReasonUpdate/{id}/{type}")]
    [UnitOfWork]
    public async Task ReasonUpdate(string id, string type, [FromBody] AProdOutsourceUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdOutsourceEntity>();
        entity.id = id;
        entity.F_State = type;
        if (type == "3")
        {
            entity.F_PauseReason = input.F_Reason;
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_State,
                    it.F_PauseReason
                })
                .ExecuteCommandAsync();

        }
        else if (type == "4")
        {
            entity.F_CancelReason = input.F_Reason;
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_State,
                    it.F_CancelReason
                })
                .ExecuteCommandAsync();
        }
    }


    /// <summary>
    /// 删除a_prod_outsource.
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
    /// 批量删除a_prod_outsource.
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
            // 批量删除a_prod_outsource
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_prod_outsource详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.AProdOutsourceItemList)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.id == id)
            .Select(it => new AProdOutsourceDetailOutput
            {
                id = it.id,
                F_GoodsId = it.F_GoodsId,
                F_BOMId = it.F_BOMId,
                F_OutsourceNo = it.F_OutsourceNo,
                F_PlanNum = it.F_PlanNum.ToString(),
                F_SupplierId = SqlFunc.Subqueryable<ASupplierEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_SupplierId)).Select(u => u.F_SupplierName),
                F_ContactPerson = it.F_ContactPerson,
                F_ContactPhone = it.F_ContactPhone,
                F_Region = it.F_Region,
                F_DetailAddress = it.F_DetailAddress,
                F_Price = it.F_Price,
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_Priority = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Priority) && dic.DictionaryTypeId.Equals("2013182853290004480")).Select(dic => dic.FullName),
                F_PlanStartDate = it.F_PlanStartDate.Value.ToString("yyyy-MM-dd"),
                F_PlanEndDate = it.F_PlanEndDate.Value.ToString("yyyy-MM-dd"),
                F_Files = it.F_Files,
                F_State = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_State) && dic.DictionaryTypeId.Equals("2013819135649255424")).Select(dic => dic.FullName),
                F_Description = it.F_Description,
                F_PauseReason = it.F_PauseReason,
                F_CancelReason = it.F_CancelReason,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableField7a8044 = it.AProdOutsourceItemList.Adapt<List<AProdOutsourceItemDetailOutput>>(),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            // 供应商地区
            if(item.F_Region != null)
            {
                var F_RegionAreaSelect = item.F_Region.ToObject<List<string>>();
                item.F_Region = string.Join("/", await _repository.AsSugarClient().Queryable<ProvinceEntity>().Where(it => F_RegionAreaSelect.Contains(it.Id)).Select(it => it.FullName).ToListAsync());
            }

            if(item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField7a8044), async aProdOutsourceItem =>
        {
            var aProdOutsource = data.ToList().Find(it => it.id.Equals(aProdOutsourceItem.F_OutsourceId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建用户
            aProdOutsourceItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aProdOutsourceItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aProdOutsourceItem.F_CreatorTime = aProdOutsourceItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<AProdOutsourceEntity>(new AProdOutsourceEntity()));
        ConfigModel tableField7a8044Config = new ConfigModel
        {
            tableName = "a_prod_outsource_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "用料清单",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<AProdOutsourceItemEntity>(new AProdOutsourceItemEntity())
        };
        FieldsModel tableField7a8044 = new FieldsModel()
        {
            __config__ = tableField7a8044Config,
            __vModel__ = "tableField7a8044"
        };
        fieldList.Add(tableField7a8044);

        resData = await _controlParsing.GetParsDataList(resData,"F_GoodsId,F_BOMId,tableField7a8044-F_GoodsId","popupSelect",_userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }

    /// <summary>
    /// 根据供应商ID获取供应商联系信息.
    /// </summary>
    /// <param name="supplierId">供应商ID.</param>
    /// <returns></returns>
    [HttpGet("GetSupplierContactInfo/{supplierId}")]
    public async Task<dynamic> GetSupplierContactInfo(string supplierId)
    {
        var supplierData = await _repositorysupplier.AsQueryable()
            .Where(it => it.id == supplierId && it.DeleteMark == null)
            .Select(it => new
            {
                F_ContactPerson = it.F_ContactPerson,
                F_ContactPhone = it.F_ContactPhone,
                F_Region = it.F_Region,
                F_DetailAddress = it.F_DetailAddress
            })
            .FirstAsync();

        return supplierData;
    }


    /// <summary>
    /// 切换外协单状态（生产中↔已完成）.
    /// </summary>
    /// <param name="id">外协单主键.</param>
    /// <returns></returns>
    [HttpPut("Complete/{id}")]
    [UnitOfWork]
    public async Task Complete(string id)
    {
        // 先查询数据库获取当前状态
        var entity = await _repository.GetFirstAsync(it => it.id == id);
        if (entity == null)
        {
            return;
        }

        // 根据当前状态切换状态
        string newState = entity.F_State == "1" ? "2" : "1";

        await _repository.AsUpdateable()
            .SetColumns(it => new AProdOutsourceEntity
            {
                F_State = newState
            })
            .Where(it => it.id == id)
            .ExecuteCommandAsync();
    }



    /// <summary>
    /// 弹窗获取外协工单及商品信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("PopupList")]
    public async Task<dynamic> GetPopupList()
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .LeftJoin<AGoodsEntity>((it, g) => g.id.Equals(it.F_GoodsId) && g.DeleteMark == null)
            .Select((it, g) => new AProdOutsourceListOutput
            {
                id = it.id,
                F_OutsourceNo = it.F_OutsourceNo,
                F_GoodsId = it.F_GoodsId,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_CategoryId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_CategoryId),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_PlanNum = it.F_PlanNum.ToString(),
                F_InventoryNum = 0,
                F_YseNum = 0,
                F_CostPrice = g.F_CostPrice,
                F_Unit_Ratio = g.F_Unit_Ratio,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            //库存
            var inventory = await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_GoodsId).SumAsync(it => it.F_Num);
            if (inventory != null)
            {
                item.F_InventoryNum += inventory;
            }
            //已入库数量
            var stockFg = await _repositoryStockFgItem.AsQueryable().Where(it => it.DeleteMark == null && it.F_WorkOrderId == item.id).SumAsync(it => it.F_Num);
            if (stockFg != null)
            {
                item.F_YseNum += stockFg;
            }
            //转换CategoryId为F_Code（与加工单一致）
            if (!string.IsNullOrEmpty(item.F_CategoryId))
            {
                try
                {
                    var categoryIds = System.Text.Json.JsonSerializer.Deserialize<List<string>>(item.F_CategoryId);
                    if (categoryIds != null && categoryIds.Any())
                    {
                        var categoryCodes = await _repositorysp.AsQueryable()
                            .Where(it => categoryIds.Contains(it.id))
                            .Select(it => it.F_Code)
                            .ToListAsync();
                        item.F_CategoryId = System.Text.Json.JsonSerializer.Serialize(categoryCodes);
                    }
                }
                catch
                {
                    // 如果解析失败，保持原值
                }
            }
        });
        return new {
            data = data
        };
    }

    /// <summary>
    /// 获取某个 外协工单及商品信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("OutsourceIdList/{id}")]
    public async Task<dynamic> GetOutsourceIdList(string id)
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null && it.id == id)
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .LeftJoin<AGoodsEntity>((it, g) => g.id.Equals(it.F_GoodsId) && g.DeleteMark == null)
            .Select((it, g) => new AProdOutsourceListOutput
            {
                id = it.id,
                F_OutsourceNo = it.F_OutsourceNo,
                F_GoodsId = it.F_GoodsId,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_CategoryId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_CategoryId),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_PlanNum = it.F_PlanNum.ToString(),
                F_InventoryNum = 0,
                F_YseNum = 0,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            //库存
            var inventory = await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_GoodsId).SumAsync(it => it.F_Num);
            if (inventory != null)
            {
                item.F_InventoryNum += inventory;
            }
            //已入库数量
            var stockFg = await _repositoryStockFgItem.AsQueryable().Where(it => it.DeleteMark == null && it.F_WorkOrderId == item.id).SumAsync(it => it.F_Num);
            if (stockFg != null)
            {
                item.F_YseNum += stockFg;
            }
            //转换CategoryId为F_Code（与加工单一致）
            if (!string.IsNullOrEmpty(item.F_CategoryId))
            {
                try
                {
                    var categoryIds = System.Text.Json.JsonSerializer.Deserialize<List<string>>(item.F_CategoryId);
                    if (categoryIds != null && categoryIds.Any())
                    {
                        var categoryCodes = await _repositorysp.AsQueryable()
                            .Where(it => categoryIds.Contains(it.id))
                            .Select(it => it.F_Code)
                            .ToListAsync();
                        item.F_CategoryId = System.Text.Json.JsonSerializer.Serialize(categoryCodes);
                    }
                }
                catch
                {
                    // 如果解析失败，保持原值
                }
            }
        });
        return data;
    }


    /// <summary>
    /// 根据外协工单和仓库获取用料清单.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("OutsourceGoodsList/{id}/{warehouseId}")]
    public async Task<dynamic> GetOutsourceGoodsList(string id, string warehouseId)
    {
        var data = await _repositoryItem.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_OutsourceId == id)
            .OrderBy("F_CreatorTime desc")
            .LeftJoin<AGoodsEntity>((it, g) => g.id.Equals(it.F_GoodsId) && g.DeleteMark == null)
            .Select((it, g) => new AProdOutsourceItemInfoOutput
            {
                F_Id = it.F_Id,
                F_GoodsId = it.F_GoodsId,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_Unit = it.F_Unit,
                F_UnitTwo = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit)) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                F_CostPrice = g.F_CostPrice,
                F_InventoryNum = 0,
            })
            .ToListAsync();
        foreach (var item in data)
        {
            var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == item.F_GoodsId && it.F_WarehouseId == warehouseId);
            if (inventory != null)
            {
                item.F_InventoryNum += inventory.F_Num;
            }
        }

        return data;
    }
}
