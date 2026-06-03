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
using JNPF.example.Entitys.Dto.AProdPlan;
using JNPF.example.Entitys.Dto.AProdPlanItem;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_prod_plan.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdPlan", Order = 200)]
[Route("api/example/[controller]")]
public class AProdPlanService : IAProdPlanService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdPlanEntity> _repository; 

    private readonly ISqlSugarRepository<AGoodsEntity> _repositoryGoods;

    private readonly ISqlSugarRepository<DictionaryDataEntity> _repositoryDicData;

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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"计划编号\",\"field\":\"F_PlanNo\"},{\"value\":\"计划名称\",\"field\":\"F_PlanName\"},{\"value\":\"物料分析状态\",\"field\":\"F_State\"},{\"value\":\"交货日期\",\"field\":\"F_DeliveryDate\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdPlanService"/>类型的新实例.
    /// </summary>
    public AProdPlanService(
        ISqlSugarRepository<DictionaryDataEntity> repositoryDicData,
        ISqlSugarRepository<AGoodsEntity> repositoryGoods,
        ISqlSugarRepository<AProdPlanEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryDicData = repositoryDicData;
        _repositoryGoods = repositoryGoods;
        _repository = repository;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_prod_plan.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.AProdPlanItemList)
            .Select(it => new AProdPlanEntity
            {
                id = it.id,
                F_PlanNo = it.F_PlanNo,
                F_PlanName = it.F_PlanName,
                F_DeliveryDate = it.F_DeliveryDate,
                F_State = it.F_State,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                AProdPlanItemList = it.AProdPlanItemList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AProdPlanInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField2152f8, async aProdPlanItem =>
        {
            // 创建时间
            aProdPlanItem.F_CreatorTime = aProdPlanItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            aProdPlanItem.F_GoodsNo = (await _repositoryGoods.GetFirstAsync(it => it.id == aProdPlanItem.F_GoodsId && it.DeleteMark == null)).F_GoodsNo;
            aProdPlanItem.F_Specification = (await _repositoryGoods.GetFirstAsync(it => it.id == aProdPlanItem.F_GoodsId && it.DeleteMark == null)).F_Specification;
            var goodsEntity = await _repositoryGoods.GetFirstAsync(it => it.id == aProdPlanItem.F_GoodsId && it.DeleteMark == null);
            //aProdPlanItem.F_Unit = (await _repositoryDicData.GetFirstAsync(dic => dic.EnCode == goodsEntity.F_Unit && dic.DeleteMark == null && dic.DictionaryTypeId.Equals("2008448689420505088"))).FullName;

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(goodsEntity.F_Unit))
            {
                var F_UnitIdCascader = goodsEntity.F_Unit.ToObject<List<string>>();
                aProdPlanItem.F_Unit = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));

                if (F_UnitIdCascader.Count > 1)
                {
                    aProdPlanItem.F_NumUnit = (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    aProdPlanItem.F_NumUnit = (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }

        });
        return data;
    }

    /// <summary>
    /// 获取a_prod_plan列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdPlanListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aProdPlanItemAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<AProdPlanListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_prod_plan"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_prod_plan_item"))) aProdPlanItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_prod_plan_item"))?.conditionalModel;
        }

        var selectIds = input.selectIds?.Split(",").ToList();

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_PlanNo), it => it.F_PlanNo.Contains(input.F_PlanNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_PlanName), it => it.F_PlanName.Contains(input.F_PlanName))
            .WhereIF(input.F_DeliveryDate?.Count() > 0, it => SqlFunc.Between(it.F_DeliveryDate, input.F_DeliveryDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_DeliveryDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_State), it => it.F_State.Equals(input.F_State))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .Where(authorizeWhere)
            .WhereIF(aProdPlanItemAuthorizeWhere?.Count > 0, it => it.AProdPlanItemList.Any(aProdPlanItemAuthorizeWhere))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new AProdPlanListOutput
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
            .Select(it => new AProdPlanListOutput
            {
                id = it.id,
                F_PlanNo = it.F_PlanNo,
                F_PlanName = it.F_PlanName,
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_State = it.F_State,
                F_CreatorUserId = it.F_CreatorUserId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 物料分析状态
            var F_StateData = "[{\"id\":\"1\",\"fullName\":\"已分析\"},{\"id\":\"0\",\"fullName\":\"未分析\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_State != null)
            {
                item.F_State = F_StateData.Find(it => item.F_State.Equals(it.id))?.fullName;
            }

            // 创建人员
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;

        });

        data.pagination = idsQ.pagination;

        return PageResult<AProdPlanListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_prod_plan.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] AProdPlanCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdPlanEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdPlanEntity>(new AProdPlanEntity()));
        ConfigModel tableField2152f8Config = new ConfigModel
        {
            tableName = "a_prod_plan_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetTemplateParsing<AProdPlanItemEntity>(new AProdPlanItemEntity())
        };
        FieldsModel tableField2152f8Model = new FieldsModel()
        {
            __config__ = tableField2152f8Config,
            __vModel__ = "tableField2152f8"
        };
        fieldList.Add(tableField2152f8Model);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_State = "0";
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        if (await _repository.IsAnyAsync(it => it.F_PlanNo.Equals(input.F_PlanNo) && it.FlowId == null && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "计划编号");

        // 自动生成编号逻辑：前缀 JH + yyyyMMdd，后缀 3 位序号
        if (string.IsNullOrEmpty(entity.F_PlanNo))
        {
            var prefix = "JH" + DateTime.Now.ToString("yyyyMMdd");

            // 查询数据库中已有相同前缀的编号
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_PlanNo != null && it.F_PlanNo.StartsWith(prefix))
                .Select(it => it.F_PlanNo)
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
            entity.F_PlanNo = prefix + nextSeq.ToString("D3");
        }

        var aProdPlanItemEntityList = input.tableField2152f8.Adapt<List<AProdPlanItemEntity>>();
        if(aProdPlanItemEntityList != null)
        {
            foreach (var item in aProdPlanItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.AProdPlanItemList = aProdPlanItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.AProdPlanItemList)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新a_prod_plan.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] AProdPlanUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdPlanEntity>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdPlanEntity>(new AProdPlanEntity()));
        ConfigModel tableField2152f8Config = new ConfigModel
        {
            tableName = "a_prod_plan_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetTemplateParsing<AProdPlanItemEntity>(new AProdPlanItemEntity())
        };
        FieldsModel tableField2152f8Model = new FieldsModel()
        {
            __config__ = tableField2152f8Config,
            __vModel__ = "tableField2152f8"
        };
        fieldList.Add(tableField2152f8Model);

        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_PlanNo.Equals(input.F_PlanNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "计划编号");

        if (string.IsNullOrEmpty(entity.F_PlanNo))
        {
            entity.F_PlanNo = oldEntity.F_PlanNo;
        }


        // 移除生产计划商品列表可能被删除数据
        if (input.tableField2152f8 != null && input.tableField2152f8.Any())
            await _repository.AsSugarClient().Deleteable<AProdPlanItemEntity>().Where(it => it.F_ProdPlanId == entity.id && !input.tableField2152f8.Select(it => it.F_Id).ToList().Contains(it.F_Id)).ExecuteCommandAsync();
        else
            await _repository.AsSugarClient().Deleteable<AProdPlanItemEntity>().Where(it => it.F_ProdPlanId == entity.id).ExecuteCommandAsync();

        // 新增生产计划商品列表新数据
        var aProdPlanItemEntityList = input.tableField2152f8.Adapt<List<AProdPlanItemEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_PlanNo,
                    it.F_PlanName,
                    it.F_DeliveryDate,
                    it.F_Description,
                })
                .ExecuteCommandAsync();

            if(aProdPlanItemEntityList != null)
            {
                foreach (var item in aProdPlanItemEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_ProdPlanId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorTime = null;
                        item.F_ProdPlanId = entity.id;
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
    /// 删除a_prod_plan.
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
    /// 批量删除a_prod_plan.
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
            // 批量删除a_prod_plan
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_prod_plan详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.id == id)
            .Select(it => new AProdPlanDetailOutput
            {
                id = it.id,
                F_PlanNo = it.F_PlanNo,
                F_PlanName = it.F_PlanName,
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_State = it.F_State,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 物料分析状态
            var F_StateData = "[{\"id\":\"1\",\"fullName\":\"已分析\"},{\"id\":\"0\",\"fullName\":\"未分析\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_State != null)
            {
                item.F_State = F_StateData.Find(it => item.F_State.Equals(it.id))?.fullName;
            }

        });

        return data.FirstOrDefault();
    }


    /// <summary>
    /// 弹窗获取生产计划列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("PopupList")]
    public async Task<dynamic> GetPopupList()
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
             .Select(it => new AProdPlanListOutput
             {
                 id = it.id,
                 F_PlanNo = it.F_PlanNo,
                 F_PlanName = it.F_PlanName,
                 F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                 F_State = it.F_State,
                 F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
             }).MergeTable().OrderBy(it => it.F_CreatorTime, OrderByType.Desc).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 物料分析状态
            var F_StateData = "[{\"id\":\"1\",\"fullName\":\"已分析\"},{\"id\":\"0\",\"fullName\":\"未分析\"}]".ToObject<List<StaticDataModel>>();
            if (item.F_State != null)
            {
                item.F_State = F_StateData.Find(it => item.F_State.Equals(it.id))?.fullName;
            }

        });

        return new {
            data = data
        };
    }
}
