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
using JNPF.example.Entitys.Dto.AProdProject;
using JNPF.example.Entitys.Dto.AProdProjectItem;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
using JNPF.example.Entitys.Dto.AProdProjectStep;
using JNPF.example.Entitys.Dto.AProdProjectStepItem;
using System.Linq;
using JNPF.example.Entitys.Dto.AProdProjectGood;
using Microsoft.CodeAnalysis;
using JNPF.Message.Entitys;

namespace JNPF.example;

/// <summary>
/// 业务实现：a_prod_project.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdProject", Order = 200)]
[Route("api/example/[controller]")]
public class AProdProjectService : IAProdProjectService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdProjectEntity> _repository;
    private readonly ISqlSugarRepository<AProdProjectItemEntity> _repositoryItem;
    private readonly ISqlSugarRepository<AProdProjectStepEntity> _repositoryStep;
    private readonly ISqlSugarRepository<AProdProjectStepItemEntity> _repositoryStepItem;
    private readonly ISqlSugarRepository<AProdProjectGoodEntity> _repositoryGood;
    private readonly ISqlSugarRepository<AProdProcessingEntity> _repositoryProcessing;
    private readonly ISqlSugarRepository<AProdBomItemEntity> _repositoryBomItem;
    private readonly ISqlSugarRepository<AProdTaskEntity> _repositoryTask;
    private readonly ISqlSugarRepository<AProdTaskItemEntity> _repositoryTaskItem;
    private readonly ISqlSugarRepository<AProdOutsourceEntity> _repositorywxgd;
    private readonly ISqlSugarRepository<AProdOutsourceItemEntity> _repositorywxgdItem;
    private readonly ISqlSugarRepository<APuStockFgItemEntity> _repositStockFgItem;
    private readonly ISqlSugarRepository<AProdRoutingEntity> _repositoryRouting;
    private readonly ISqlSugarRepository<AProdRoutingStepEntity> _repositoryRoutingStep;
    private readonly ISqlSugarRepository<AProdRoutingStepItemEntity> _repositoryRoutingStepItem;
    private readonly ISqlSugarRepository<MessageEntity> _repositoryMessage;
    private readonly ISqlSugarRepository<UserEntity> _repositoryUser;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"项目编号\",\"field\":\"F_ProjectNo\"},{\"value\":\"项目名称\",\"field\":\"F_ProjectName\"},{\"value\":\"合同\",\"field\":\"F_ContractId\"},{\"value\":\"项目类别\",\"field\":\"F_ProjectType\"},{\"value\":\"项目状态\",\"field\":\"F_Status\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"},{\"value\":\"修改时间\",\"field\":\"F_UpdateTime\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"修改人员\",\"field\":\"F_UpdateUserId\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdProjectService"/>类型的新实例.
    /// </summary>
    public AProdProjectService(
        ISqlSugarRepository<UserEntity> repositoryUser,
        ISqlSugarRepository<MessageEntity> repositoryMessage,
        ISqlSugarRepository<AProdTaskItemEntity> repositoryTaskItem,
        ISqlSugarRepository<AProdRoutingStepItemEntity> repositoryRoutingStepItem,
        ISqlSugarRepository<AProdRoutingStepEntity> repositoryRoutingStep,
        ISqlSugarRepository<AProdRoutingEntity> repositoryRouting,
        ISqlSugarRepository<APuStockFgItemEntity> repositStockFgItem,
        ISqlSugarRepository<AProdBomItemEntity> repositoryBomItem,
        ISqlSugarRepository<AProdTaskEntity> repositoryTask,
        ISqlSugarRepository<AProdProcessingEntity> repositoryProcessing,
        ISqlSugarRepository<AProdProjectGoodEntity> repositoryGood,
        ISqlSugarRepository<AProdProjectStepItemEntity> repositoryStepItem,
        ISqlSugarRepository<AProdProjectStepEntity> repositoryStep,
        ISqlSugarRepository<AProdProjectItemEntity> repositoryItem,
        ISqlSugarRepository<AProdProjectEntity> repository,
        ISqlSugarRepository<AProdOutsourceItemEntity> repositorywxgdItem,
        ISqlSugarRepository<AProdOutsourceEntity> repositorywxgd,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryUser = repositoryUser;
        _repositoryMessage = repositoryMessage;
        _repositoryTaskItem = repositoryTaskItem;
        _repositoryRoutingStepItem = repositoryRoutingStepItem;
        _repositoryRoutingStep = repositoryRoutingStep;
        _repositoryRouting = repositoryRouting;
        _repositStockFgItem = repositStockFgItem;
        _repositorywxgdItem = repositorywxgdItem;
        _repositoryBomItem = repositoryBomItem;
        _repositoryTask = repositoryTask;
        _repositoryProcessing = repositoryProcessing;
        _repositoryGood = repositoryGood;
        _repositoryStepItem = repositoryStepItem;
        _repositoryStep = repositoryStep;
        _repositoryItem = repositoryItem;
        _repository = repository;
        _repositorywxgd = repositorywxgd;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_prod_project.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.AProdProjectItemList)
            .Select(it => new AProdProjectEntity
            {
                id = it.id,
                F_ProjectNo = it.F_ProjectNo,
                F_ProjectName = it.F_ProjectName,
                F_ContractId = it.F_ContractId,
                F_ProjectType = it.F_ProjectType,
                F_Status = it.F_Status,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                F_UpdateUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_UpdateUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_UpdateTime = it.F_UpdateTime,
                AProdProjectItemList = it.AProdProjectItemList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AProdProjectInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField751ecb, async aProdProjectItem =>
        {
            // 创建时间
            aProdProjectItem.F_CreatorTime = aProdProjectItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            aProdProjectItem.Process = await _repositoryStep.AsQueryable().Where(it => it.F_ProjectItemId == aProdProjectItem.F_Id).Select(it => new AProdProjectStepListOutput
            {
                id = it.id,
                F_ProjectItemId = it.F_ProjectItemId,
                F_ProcessId = it.F_ProcessId,
                F_StartDate = it.F_StartDate,
                F_EndDate = it.F_EndDate,
                F_SortCode = it.F_SortCode,
                F_Description = it.F_Description,
            }).ToListAsync();
            foreach(var projectItem in aProdProjectItem.Process)
            {
                projectItem.tableField3b6f08 = await _repositoryStepItem.AsQueryable().Where(it => it.F_ProjectStepId == projectItem.id).Select(it => new AProdProjectStepItemListOutput
                {
                    id = it.id,
                    F_ProjectStepId = it.F_ProjectStepId,
                    F_GoodsId = it.F_GoodsId,
                    F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                    F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                    F_Unit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit)) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                    F_InventoryNum = SqlFunc.Subqueryable<AGoodsInventoryEntity>().EnableTableFilter().Where(dic => dic.F_GoodsId.Equals(it.F_GoodsId) && dic.DeleteMark == null && dic.F_Num > 0).Sum(it => it.F_Num) ?? 0,
                    F_Num = it.F_Num,
                    F_Description = it.F_Description,
                }).ToListAsync();
            }


            aProdProjectItem.ProjectGoods = await _repositoryGood.AsQueryable().Where(it => it.F_ProjectItemId == aProdProjectItem.F_Id).Select(it => new AProdProjectGoodListOutput
            {
                id = it.id,
                F_ProjectItemId = it.F_ProjectItemId,
                F_GoodsId = it.F_GoodsId,
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_UnitTwo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit),
                F_InventoryNum = SqlFunc.Subqueryable<AGoodsInventoryEntity>().EnableTableFilter().Where(dic => dic.F_GoodsId.Equals(it.F_GoodsId) && dic.DeleteMark == null && dic.F_Num > 0).Sum(it => it.F_Num) ?? 0,
                F_Unit = it.F_Unit,
                F_ProdUnit = it.F_ProdUnit,
                F_StockInProcess = it.F_StockInProcess,
                F_FeedProcess = it.F_FeedProcess,
                F_SortCode = it.F_SortCode,
                F_Description = it.F_Description,
            }).ToListAsync();
            foreach (var aProdTaskItem in aProdProjectItem.ProjectGoods)
            {
                var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
                if (!string.IsNullOrEmpty(aProdTaskItem.F_UnitTwo))
                {
                    var F_UnitIdCascader = aProdTaskItem.F_UnitTwo.ToObject<List<string>>();
                    aProdTaskItem.F_UnitTwo = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));

                    if (F_UnitIdCascader.Count > 1)
                    {
                        aProdTaskItem.F_NumUnit = (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                    }
                    else
                    {
                        aProdTaskItem.F_NumUnit = (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                    }
                }
            }
        });
        return data;
    }

    /// <summary>
    /// 获取a_prod_project列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdProjectListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aProdProjectItemAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<AProdProjectListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_prod_project"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_prod_project_item"))) aProdProjectItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_prod_project_item"))?.conditionalModel;
        }

        var selectIds = input.selectIds?.Split(",").ToList();

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProjectNo), it => it.F_ProjectNo.Contains(input.F_ProjectNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProjectName), it => it.F_ProjectName.Contains(input.F_ProjectName))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .WhereIF(input.F_UpdateTime?.Count() > 0, it => SqlFunc.Between(it.F_UpdateTime, input.F_UpdateTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_UpdateTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .Where(authorizeWhere)
            .WhereIF(aProdProjectItemAuthorizeWhere?.Count > 0, it => it.AProdProjectItemList.Any(aProdProjectItemAuthorizeWhere))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new AProdProjectListOutput
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
            .Select(it => new AProdProjectListOutput
            {
                id = it.id,
                F_ProjectNo = it.F_ProjectNo,
                F_ProjectName = it.F_ProjectName,
                F_ContractId = SqlFunc.Subqueryable<ACtcContractEntity>().EnableTableFilter().Where(c => c.id.Equals(it.F_ContractId)).Select(c => c.F_ContractCode),
                F_ProjectType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_ProjectType) && dic.DictionaryTypeId.Equals("2013172950349516800")).Select(dic => dic.FullName),
                F_ProjectTypeCode = it.F_ProjectType,
                F_Status = it.F_Status,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_UpdateUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_UpdateUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_UpdateTime = it.F_UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            // 项目状态
            var processingList = await _repositoryProcessing.AsQueryable().Where(it => it.DeleteMark == null && it.F_ProjectId == item.id).Select(it => new { F_State = it.F_State, F_PlanQty = it.F_PlanQty }).ToListAsync();
            item.F_Status = "已完成";
            item.F_WorkOrderQty = 0;
            item.F_CompletedQtyStatus = 0;
            item.F_PlanQty = 0;
            foreach (var proItem in processingList)
            {
                item.F_PlanQty += proItem.F_PlanQty;
                item.F_WorkOrderQty += 1;
                if (proItem.F_State != "2")
                {
                    item.F_Status = "进行中";
                }
                else
                {
                    item.F_CompletedQtyStatus += 1;
                }
            }

            item.F_ProjectProgress = item.F_CompletedQtyStatus.ToString() + "/" + item.F_WorkOrderQty.ToString();
        });

        data.pagination = idsQ.pagination;

        return PageResult<AProdProjectListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建（普通项目、合同项目）.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("CreateTwo")]
    [UnitOfWork]
    public async Task CreateTwo([FromBody] AProdProjectCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdProjectEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdProjectEntity>(new AProdProjectEntity()));
        ConfigModel tableField751ecbConfig = new ConfigModel
        {
            tableName = "a_prod_project_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetTemplateParsing<AProdProjectItemEntity>(new AProdProjectItemEntity())
        };
        FieldsModel tableField751ecbModel = new FieldsModel()
        {
            __config__ = tableField751ecbConfig,
            __vModel__ = "tableField751ecb"
        };
        fieldList.Add(tableField751ecbModel);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        if (await _repository.IsAnyAsync(it => it.F_ProjectNo.Equals(input.F_ProjectNo) && it.FlowId == null && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "项目编号");

        // 自动生成编号逻辑：前缀 yyyyMMdd，后缀 3 位序号
        if (string.IsNullOrEmpty(entity.F_ProjectNo))
        {
            var prefix = DateTime.Now.ToString("yyyyMMdd");

            // 查询数据库中已有相同前缀的编号
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_ProjectNo != null && it.F_ProjectNo.StartsWith(prefix))
                .Select(it => it.F_ProjectNo)
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
            entity.F_ProjectNo = prefix + nextSeq.ToString("D3");
        }

        var aProdProjectItemEntityList = input.tableField751ecb.Adapt<List<AProdProjectItemEntity>>();
        if(aProdProjectItemEntityList != null)
        {
            // 工单编号不能重复
            var repeatNo = aProdProjectItemEntityList
                           .Where(x => !string.IsNullOrEmpty(x.F_WorkOrderNo))
                           .GroupBy(x => x.F_WorkOrderNo)
                           .Where(g => g.Count() > 1)
                           .Select(g => g.Key)
                           .ToList();

            if (repeatNo.Any())
            {
                throw Oops.Bah(ErrorCode.COM1023, "工单编号");
            }

            foreach (var item in aProdProjectItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();

                // 自动生成编号逻辑：前缀 项目编号，后缀 1 位序号
                if (string.IsNullOrEmpty(item.F_WorkOrderNo))
                {
                    // 0. 统一前缀
                    string prefix = entity.F_ProjectNo + "-";

                    // 1. 先在内存里找出已经用过的序号（只认当前这批）
                    int maxSeq = 0;
                    foreach (var it in aProdProjectItemEntityList)
                    {
                        if (!string.IsNullOrEmpty(it.F_WorkOrderNo) &&
                            it.F_WorkOrderNo.StartsWith(prefix) &&
                            it.F_WorkOrderNo.Length > prefix.Length)
                        {
                            var suffix = it.F_WorkOrderNo.Substring(prefix.Length);
                            if (int.TryParse(suffix, out int seq) && seq > maxSeq)
                                maxSeq = seq;
                        }
                    }

                    // 2. 给没有编号的行依次赋值
                    foreach (var itemTwo in aProdProjectItemEntityList)
                    {
                        itemTwo.F_Id = SnowflakeIdHelper.NextId();
                        itemTwo.F_CreatorTime = DateTime.Now;

                        if (string.IsNullOrEmpty(itemTwo.F_WorkOrderNo))
                        {
                            maxSeq++;                       // 继续累加
                            itemTwo.F_WorkOrderNo = prefix + maxSeq.ToString("D1");
                        }
                    }
                }

                if (item.ProjectGoods != null)
                {
                    var aProdProjectGoodEntityList = item.ProjectGoods.Adapt<List<AProdProjectGoodEntity>>();

                    foreach (var itemGood in aProdProjectGoodEntityList)
                    {
                        itemGood.id = SnowflakeIdHelper.NextId();
                        itemGood.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        itemGood.F_ProjectItemId = item.F_Id;
                    }
                    // 把步骤集合挂到项目 item（导航属性）
                    item.AProdProjectGoodList = aProdProjectGoodEntityList;
                }
            }

            entity.AProdProjectItemList = aProdProjectItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
        .Include(it => it.AProdProjectItemList)        // 商品（同一条链继续）
            .ThenInclude(it => it.AProdProjectGoodList)     // 商品本身物料
        .ExecuteCommandAsync();

        if (entity.AProdProjectItemList != null)
        {
            foreach (var item in entity.AProdProjectItemList)
            {
                //生成加工单
                var processingEntity = new AProdProcessingEntity();
                processingEntity.id = SnowflakeIdHelper.NextId();
                processingEntity.F_CreatorUserId = _userManager.UserId;
                processingEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                processingEntity.F_State = "1";
                processingEntity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

                if (entity.F_ProjectType == "1")
                {
                    processingEntity.F_ContractId = entity.F_ContractId;
                }

                var processNo = await _repositoryProcessing.AsQueryable()
                    .Where(it => it.F_SequenceNo != null && it.DeleteMark == null)
                    .MaxAsync(it => it.F_SequenceNo);

                processingEntity.F_SequenceNo = (processNo ?? 0) + 1;

                // 自动生成编号逻辑：前缀 JG + yyyyMMdd，后缀 3 位序号
                if (string.IsNullOrEmpty(processingEntity.F_ProcessNo))
                {
                    var prefix = "JG" + DateTime.Now.ToString("yyyyMMdd");

                    // 查询数据库中已有相同前缀的编号
                    var existingNos = await _repositoryProcessing.AsQueryable()
                        .Where(it => it.F_ProcessNo != null && it.F_ProcessNo.StartsWith(prefix))
                        .Select(it => it.F_ProcessNo)
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
                    processingEntity.F_ProcessNo = prefix + nextSeq.ToString("D3");
                }

                processingEntity.F_ProjectId = entity.id;
                processingEntity.F_GoodsId = item.F_GoodsId;
                processingEntity.F_BOMId = item.F_BomId;
                processingEntity.F_RoutingId = item.F_RoutingId;
                processingEntity.F_PlanQty = item.F_PlanNum;
                processingEntity.F_DeliveryDate = item.F_DeliveryDate;
                processingEntity.F_Priority = item.F_Priority;
                processingEntity.F_DrawingFile = item.F_DrawingFiles;
                processingEntity.F_DoorPlateThickness = item.F_DoorPlateThickness;
                processingEntity.F_BoxPlateThickness = item.F_BoxPlateThickness;
                processingEntity.F_InstallOrBeam = item.F_InstallPlateOrBeam;
                processingEntity.F_CustomerName = item.F_CustomerName;
                processingEntity.F_LockSet = item.F_LockSet;
                processingEntity.F_Hinge = item.F_Hinge;
                processingEntity.F_BOMImages = item.F_BomImages;
                processingEntity.F_Color = item.F_Color;
                processingEntity.F_Type = item.F_Category;

                //赋值 工艺路线的XML
                var routingEntity = await _repositoryRouting.GetFirstAsync(it => it.DeleteMark == null && it.id == processingEntity.F_RoutingId);
                processingEntity.F_XML = routingEntity.F_XML;

                if (item.ProjectGoods != null)
                {
                    var aProdProjectGoodEntityList = item.ProjectGoods.Adapt<List<AProdProjectGoodEntity>>();

                    var aProdBomItemEntityList = new List<AProdBomItemEntity>();

                    foreach (var itemGood in aProdProjectGoodEntityList)
                    {
                        var bomItemEntity = new AProdBomItemEntity();
                        bomItemEntity.F_Id = SnowflakeIdHelper.NextId();
                        bomItemEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        bomItemEntity.F_ProcessingId = processingEntity.id;
                        bomItemEntity.F_GoodsId = itemGood.F_GoodsId;
                        bomItemEntity.F_Unit = itemGood.F_Unit;
                        bomItemEntity.F_StockInProcess = itemGood.F_StockInProcess;
                        bomItemEntity.F_SortCode = itemGood.F_SortCode;
                        bomItemEntity.F_Description = itemGood.F_Description;

                        aProdBomItemEntityList.Add(bomItemEntity);
                    }
                    // 把步骤集合挂到项目 item（导航属性）
                    processingEntity.AProdBomItemList = aProdBomItemEntityList;
                }
                await _repositoryProcessing.AsSugarClient().InsertNav(processingEntity)
                    .Include(it => it.AProdBomItemList)
                    .ExecuteCommandAsync();


                // 生成生产任务（从工艺路线获取）
                // 1. 查询工艺路线的所有节点
                var routingStepList = await _repositoryRoutingStep.AsQueryable()
                    .Where(it => it.DeleteMark == null && it.F_RoutingId == processingEntity.F_RoutingId)
                    .Select(it => new {
                        F_Id = it.F_Id,
                        F_ProcessId = it.F_ProcessId,
                        F_ParentId = it.F_ParentId,
                        F_NodeId = it.F_NodeId,
                        F_NodeName = it.F_NodeName,
                        F_Type = it.F_Type,
                        F_ResponsibleUserId = it.F_ResponsibleUserId,
                        F_SortCode = it.F_SortCode,
                    })
                    .ToListAsync();

                if (routingStepList.Count == 0) continue;

                // 2. 批量查询所有工序节点的物料信息
                var stepIds = routingStepList.Select(s => s.F_Id).ToList();
                var allStepItems = await _repositoryRoutingStepItem.AsQueryable()
                    .Where(it => stepIds.Contains(it.F_StepId))
                    .Select(it => new {
                        F_StepId = it.F_StepId,
                        F_GoodsId = it.F_GoodsId,
                        F_Num = it.F_Num,
                    })
                    .ToListAsync();

                // 3. 按工序ID分组物料
                var stepItemsDict = allStepItems.GroupBy(it => it.F_StepId).ToDictionary(g => g.Key, g => g.ToList());

                // 4. 统一生成时间
                var now = DateTime.Now;

                // 5. 生成生产任务及物料
                foreach (var routingStep in routingStepList)
                {
                    var taskEntity = new AProdTaskEntity
                    {
                        id = SnowflakeIdHelper.NextId(),
                        F_ProcessingId = processingEntity.id,  // 关联加工单
                        F_ProcessId = routingStep.F_ProcessId,
                        F_ParentId = routingStep.F_ParentId,
                        F_NodeId = routingStep.F_NodeId,
                        F_NodeName = routingStep.F_NodeName,
                        F_Type = routingStep.F_Type,
                        F_ResponsibleUserId = routingStep.F_ResponsibleUserId,
                        F_SortCode = routingStep.F_SortCode,
                        F_CreatorTime = now,
                        F_TaskType = "0",
                        F_TaskStatus = "1",
                        F_CreatorOrganizeId = _userManager.User.OrganizeId,
                        F_CreatorUserId = _userManager.UserId,
                        F_PlanStartDate = processingEntity.F_PlanStartDate,
                        F_PlanEndDate = processingEntity.F_PlanEndDate,
                        F_ProdQty = processingEntity.F_PlanQty,
                    };
                    await _repositoryTask.AsInsertable(taskEntity).ExecuteCommandAsync();

                    // 从字典获取该工序的物料列表
                    if (stepItemsDict.TryGetValue(routingStep.F_Id, out var stepItems))
                    {
                        var taskItemList = stepItems.Select(item => new AProdTaskItemEntity
                        {
                            F_Id = SnowflakeIdHelper.NextId(),
                            F_TaskId = taskEntity.id,
                            F_GoodsId = item.F_GoodsId,
                            F_Num = item.F_Num,
                            F_CreatorTime = now,
                        }).ToList();

                        if (taskItemList.Count > 0)
                        {
                            await _repositoryTaskItem.AsInsertable(taskItemList).ExecuteCommandAsync();
                        }
                    }
                }


                //给生产部发站内信-加工单
                var userEntity = (await _repositoryUser.AsQueryable().Where(it => it.Id == _userManager.UserId && it.DeleteMark == null).Select(it => it.RealName).ToListAsync()).FirstOrDefault();
                var userList = await _repositoryUser.AsQueryable().Where(it => it.OrganizeId.Contains("2020771359889690624") && it.DeleteMark == null).Select(it => it.Id).ToListAsync();
                foreach (var itemProcissing in userList)
                {
                    var messageSystemEntity = new MessageEntity();
                    messageSystemEntity.Id = SnowflakeIdHelper.NextId();
                    messageSystemEntity.Title = "新增加工单-" + item.F_WorkOrderNo;
                    messageSystemEntity.UserId = itemProcissing;
                    messageSystemEntity.Type = 3;
                    messageSystemEntity.FlowType = 1;
                    messageSystemEntity.IsRead = 0;
                    messageSystemEntity.SortCode = 0;
                    messageSystemEntity.CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                    messageSystemEntity.TenantId = "0";
                    messageSystemEntity.BodyText = userEntity + "-新增加工单-" + item.F_WorkOrderNo + ",请前往生产管理-加工单查看！";
                    await _repositoryMessage.AsInsertable(messageSystemEntity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
                }
            }
        }

    }

    /// <summary>
    /// 新建(外协项目）.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] AProdProjectCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdProjectEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdProjectEntity>(new AProdProjectEntity()));
        ConfigModel tableField751ecbConfig = new ConfigModel
        {
            tableName = "a_prod_project_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetTemplateParsing<AProdProjectItemEntity>(new AProdProjectItemEntity())
        };
        FieldsModel tableField751ecbModel = new FieldsModel()
        {
            __config__ = tableField751ecbConfig,
            __vModel__ = "tableField751ecb"
        };
        fieldList.Add(tableField751ecbModel);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        if (await _repository.IsAnyAsync(it => it.F_ProjectNo.Equals(input.F_ProjectNo) && it.FlowId == null && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "项目编号");

        // 自动生成编号逻辑：前缀 yyyyMMdd，后缀 3 位序号
        if (string.IsNullOrEmpty(entity.F_ProjectNo))
        {
            var prefix = DateTime.Now.ToString("yyyyMMdd");

            // 查询数据库中已有相同前缀的编号
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_ProjectNo != null && it.F_ProjectNo.StartsWith(prefix))
                .Select(it => it.F_ProjectNo)
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
            entity.F_ProjectNo = prefix + nextSeq.ToString("D3");
        }

        var aProdProjectItemEntityList = input.tableField751ecb.Adapt<List<AProdProjectItemEntity>>();
        if (aProdProjectItemEntityList != null)
        {
            // 工单编号不能重复
            var repeatNo = aProdProjectItemEntityList
                           .Where(x => !string.IsNullOrEmpty(x.F_WorkOrderNo))
                           .GroupBy(x => x.F_WorkOrderNo)
                           .Where(g => g.Count() > 1)
                           .Select(g => g.Key)
                           .ToList();

            if (repeatNo.Any())
            {
                throw Oops.Bah(ErrorCode.COM1023, "工单编号");
            }

            foreach (var item in aProdProjectItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();

                // 自动生成编号逻辑：前缀 项目编号，后缀 1 位序号
                if (string.IsNullOrEmpty(item.F_WorkOrderNo))
                {
                    // 0. 统一前缀
                    string prefix = entity.F_ProjectNo + "-";

                    // 1. 先在内存里找出已经用过的序号（只认当前这批）
                    int maxSeq = 0;
                    foreach (var it in aProdProjectItemEntityList)
                    {
                        if (!string.IsNullOrEmpty(it.F_WorkOrderNo) &&
                            it.F_WorkOrderNo.StartsWith(prefix) &&
                            it.F_WorkOrderNo.Length > prefix.Length)
                        {
                            var suffix = it.F_WorkOrderNo.Substring(prefix.Length);
                            if (int.TryParse(suffix, out int seq) && seq > maxSeq)
                                maxSeq = seq;
                        }
                    }

                    // 2. 给没有编号的行依次赋值
                    foreach (var itemTwo in aProdProjectItemEntityList)
                    {
                        itemTwo.F_Id = SnowflakeIdHelper.NextId();
                        itemTwo.F_CreatorTime = DateTime.Now;

                        if (string.IsNullOrEmpty(itemTwo.F_WorkOrderNo))
                        {
                            maxSeq++;                       // 继续累加
                            itemTwo.F_WorkOrderNo = prefix + maxSeq.ToString("D1");
                        }
                    }
                }

                if (item.Process != null)
                {
                    var aProdProjectStepList = item.Process.Adapt<List<AProdProjectStepEntity>>();

                    foreach (var itemStep in aProdProjectStepList)
                    {
                        itemStep.id = SnowflakeIdHelper.NextId();
                        itemStep.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        itemStep.F_ProjectItemId = item.F_Id;

                        if (itemStep.tableField3b6f08 != null)
                        {
                            var aProdProjectStepItemList = itemStep.tableField3b6f08.Adapt<List<AProdProjectStepItemEntity>>();

                            foreach (var itemStepItem in aProdProjectStepItemList)
                            {
                                itemStepItem.id = SnowflakeIdHelper.NextId();
                                itemStepItem.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                                itemStepItem.F_ProjectStepId = itemStep.id;
                                itemStepItem.F_ProjectItemId = item.F_Id;
                            }
                            // 1. 把子集合挂到步骤实体（导航属性）
                            itemStep.AProdProjectStepItemList = aProdProjectStepItemList;
                        }
                    }
                    // 2. 把步骤集合挂到项目 item（导航属性）
                    item.AProdProjectStepList = aProdProjectStepList;
                }

                if (item.ProjectGoods != null)
                {
                    var aProdProjectGoodEntityList = item.ProjectGoods.Adapt<List<AProdProjectGoodEntity>>();

                    foreach (var itemGood in aProdProjectGoodEntityList)
                    {
                        itemGood.id = SnowflakeIdHelper.NextId();
                        itemGood.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        itemGood.F_ProjectItemId = item.F_Id;
                    }
                    // 把步骤集合挂到项目 item（导航属性）
                    item.AProdProjectGoodList = aProdProjectGoodEntityList;
                }
            }

            entity.AProdProjectItemList = aProdProjectItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
        .Include(it => it.AProdProjectItemList)        // 商品（同一条链继续）
            .ThenInclude(it => it.AProdProjectGoodList)     // 商品本身物料
        .ExecuteCommandAsync();

        if (entity.AProdProjectItemList != null)
        {
            foreach (var item in entity.AProdProjectItemList)
            {
                // 当主表项目类型为 2 时，生成外协单（F_OutsourceNo 按 AProdOutsourceService 逻辑）
                if (entity.F_ProjectType == "2")
                {
                    var outsourceEntity = new AProdOutsourceEntity();
                    outsourceEntity.id = SnowflakeIdHelper.NextId();
                    outsourceEntity.F_CreatorUserId = _userManager.UserId;
                    outsourceEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                    outsourceEntity.F_GoodsId = item.F_GoodsId;
                    outsourceEntity.F_State = "1";
                    outsourceEntity.F_PlanNum = item.F_PlanNum;

                    // 生成编号：WX + yyyyMMdd + - + 当天序号（参考 AProdOutsourceService）
                    var today = DateTime.Now.Date;
                    var tomorrow = today.AddDays(1);
                    var todayCount = await _repositorywxgd.AsQueryable()
                        .Where(it => it.F_CreatorTime >= today && it.F_CreatorTime < tomorrow && it.F_OutsourceNo.Contains("WX" + DateTime.Now.ToString("yyyyMMdd") + "-"))
                        .CountAsync();
                    outsourceEntity.F_OutsourceNo = "WX" + DateTime.Now.ToString("yyyyMMdd") + "-" + (todayCount + 1);

                    await _repositorywxgd.AsSugarClient().Insertable(outsourceEntity).ExecuteCommandAsync();

                    if (item.AProdProjectGoodList != null)
                    {
                        var aProdProjectGoodEntityList = item.AProdProjectGoodList.Adapt<List<AProdProjectGoodEntity>>();

                        var aProdOutsourceItemEntityList = new List<AProdOutsourceItemEntity>();

                        foreach (var itemGood in aProdProjectGoodEntityList)
                        {
                            var bomItemEntity = new AProdOutsourceItemEntity();
                            bomItemEntity.F_Id = SnowflakeIdHelper.NextId();
                            bomItemEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                            bomItemEntity.F_OutsourceId = outsourceEntity.id;
                            bomItemEntity.F_GoodsId = itemGood.F_GoodsId;
                            bomItemEntity.F_Unit = itemGood.F_Unit;
                            bomItemEntity.F_Description = itemGood.F_Description;

                            aProdOutsourceItemEntityList.Add(bomItemEntity);
                        }
                        // 把步骤集合挂到项目 item（导航属性）
                        outsourceEntity.AProdOutsourceItemList = aProdOutsourceItemEntityList;
                    }
                    await _repositorywxgd.AsSugarClient().InsertNav(outsourceEntity)
                        .Include(it => it.AProdOutsourceItemList)
                        .ExecuteCommandAsync();
                }
            }
        }

    }

    /// <summary>
    /// 更新a_prod_project.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] AProdProjectUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdProjectEntity>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdProjectEntity>(new AProdProjectEntity()));
        ConfigModel tableField751ecbConfig = new ConfigModel
        {
            tableName = "a_prod_project_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetTemplateParsing<AProdProjectItemEntity>(new AProdProjectItemEntity())
        };
        FieldsModel tableField751ecbModel = new FieldsModel()
        {
            __config__ = tableField751ecbConfig,
            __vModel__ = "tableField751ecb"
        };
        fieldList.Add(tableField751ecbModel);
        entity.F_UpdateUserId = _userManager.UserId;
        entity.F_UpdateTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();

        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_ProjectNo.Equals(input.F_ProjectNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "项目编号");

        if (string.IsNullOrEmpty(entity.F_ProjectNo))
        {
            entity.F_ProjectNo = oldEntity.F_ProjectNo;
        }


        if (input.tableField751ecb != null && input.tableField751ecb.Any())
            await _repository.AsSugarClient().Deleteable<AProdRoutingStepEntity>().Where(it => it.F_RoutingId == entity.id && !input.tableField751ecb.Select(it => it.F_Id).ToList().Contains(it.F_Id)).ExecuteCommandAsync();
        else
            await _repository.AsSugarClient().Deleteable<AProdRoutingStepEntity>().Where(it => it.F_RoutingId == entity.id).ExecuteCommandAsync();


        // 移除项目商品列表可能被删除数据
        var db = _repository.AsSugarClient();

        /* 1. 库内现有三级主键 */
        var existItems = await db.Queryable<AProdProjectItemEntity>()
                                 .Where(it => it.F_ProjectId == entity.id)
                                 .ToListAsync();

        var existItemIds = existItems.Select(it => it.F_Id).ToList();

        var existSteps = await db.Queryable<AProdProjectStepEntity>()
                                 .Where(it => existItemIds.Contains(it.F_ProjectItemId))
                                 .ToListAsync();

        // 处理用料清单
        var existGoods = await db.Queryable<AProdProjectGoodEntity>()
                                 .Where(it => existItemIds.Contains(it.F_ProjectItemId))
                                 .ToListAsync();
        var existGoodIds = existGoods.Select(s => s.id).ToList();

        var existStepIds = existSteps.Select(s => s.id).ToList();

        var existStepItems = await db.Queryable<AProdProjectStepItemEntity>()
                                     .Where(it => existStepIds.Contains(it.F_ProjectStepId))
                                     .ToListAsync();

        /* 2. 前端传过来的主键（可能为空字符串，先过滤） */
        var newItemIds = input.tableField751ecb?.Select(i => i.F_Id).Where(id => !string.IsNullOrEmpty(id)).ToList()
                                   ?? new List<string>();

        var newStepIds = input.tableField751ecb?.SelectMany(i => i.Process ?? new())
                                              .Select(s => s.id).Where(id => !string.IsNullOrEmpty(id)).ToList()
                                   ?? new List<string>();

        var newStepItemIds = input.tableField751ecb?.SelectMany(i => i.Process ?? new())
                                                .SelectMany(p => p.tableField3b6f08 ?? new())
                                                .Select(si => si.id).Where(id => !string.IsNullOrEmpty(id)).ToList()
                                     ?? new List<string>();

        // 处理用料清单
        var newGoods = input.tableField751ecb?.SelectMany(i => i.ProjectGoods ?? new())
                                              .Select(s => s.id).Where(id => !string.IsNullOrEmpty(id)).ToList()
                                   ?? new List<string>();

        /* 3. 差集 = 要删的主键 */
        var toDelItemIds = existItemIds.Except(newItemIds).ToList();
        var toDelStepIds = existStepIds.Except(newStepIds).ToList();
        var toDelStepItemIds = existStepItems.Select(si => si.id).Except(newStepItemIds).ToList();

        // 处理用料清单
        var toDelGoods = existGoodIds.Except(newGoods).ToList();

        /* 4. 级联删除（深→浅） */
        if (toDelStepItemIds.Any())
            await db.Deleteable<AProdProjectStepItemEntity>().Where(it => toDelStepItemIds.Contains(it.id)).ExecuteCommandAsync();

        if (toDelStepIds.Any())
            await db.Deleteable<AProdProjectStepEntity>().Where(it => toDelStepIds.Contains(it.id)).ExecuteCommandAsync();

        if (toDelItemIds.Any())
            await db.Deleteable<AProdProjectItemEntity>().Where(it => toDelItemIds.Contains(it.F_Id)).ExecuteCommandAsync();

        // 处理用料清单
        if (toDelGoods.Any())
            await db.Deleteable<AProdProjectGoodEntity>().Where(it => toDelGoods.Contains(it.id)).ExecuteCommandAsync();


        // 新增项目商品列表新数据
        var aProdProjectItemEntityList = input.tableField751ecb.Adapt<List<AProdProjectItemEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_ProjectNo,
                    it.F_ProjectName,
                    it.F_ContractId,
                    it.F_ProjectType,
                    it.F_Status,
                    it.F_UpdateUserId,
                    it.F_UpdateTime,
                })
                .ExecuteCommandAsync();
        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }

        if (aProdProjectItemEntityList != null)
        {
            // 工单编号不能重复
            var repeatNo = aProdProjectItemEntityList
                           .Where(x => !string.IsNullOrEmpty(x.F_WorkOrderNo))
                           .GroupBy(x => x.F_WorkOrderNo)
                           .Where(g => g.Count() > 1)
                           .Select(g => g.Key)
                           .ToList();

            if (repeatNo.Any())
            {
                throw Oops.Bah(ErrorCode.COM1023, "工单编号");
            }

            foreach (var item in aProdProjectItemEntityList)
            {
                if (item.F_Id.IsNullOrEmpty())
                {
                    item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                    item.F_Id = SnowflakeIdHelper.NextId();
                    item.F_ProjectId = entity.id;

                    // 自动生成编号逻辑：前缀 项目编号，后缀 1 位序号
                    if (string.IsNullOrEmpty(item.F_WorkOrderNo))
                    {
                        var prefix = entity.F_ProjectNo + "-";

                        // 查询数据库中已有相同前缀的编号
                        var existingNos = await _repositoryItem.AsQueryable()
                            .Where(it => it.F_WorkOrderNo != null && it.F_WorkOrderNo.StartsWith(prefix))
                            .Select(it => it.F_WorkOrderNo)
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
                        item.F_WorkOrderNo = prefix + nextSeq.ToString("D1");
                    }

                    await _repositoryItem.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                }
                else
                {
                    item.F_CreatorTime = null;
                    item.F_ProjectId = entity.id;
                    await _repositoryItem.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
                if (item.ProjectGoods != null)
                {
                    var aProdProjectGoodEntityList = item.ProjectGoods.Adapt<List<AProdProjectGoodEntity>>();

                    foreach (var itemGood in aProdProjectGoodEntityList)
                    {
                        if (itemGood.id.IsNullOrEmpty())
                        {
                            itemGood.id = SnowflakeIdHelper.NextId();
                            itemGood.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                            itemGood.F_ProjectItemId = item.F_Id;
                            await _repositoryGood.AsSugarClient().Insertable(itemGood).ExecuteCommandAsync();
                        }
                        else
                        {
                            itemGood.F_CreatorTime = null;
                            itemGood.F_ProjectItemId = item.F_Id;
                            await _repositoryGood.AsSugarClient().Updateable(itemGood).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 删除a_prod_project.
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
    /// 批量删除a_prod_project.
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
            // 批量删除a_prod_project
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_prod_project详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.AProdProjectItemList)
            .Where(it => it.id == id)
            .Select(it => new AProdProjectDetailOutput
            {
                id = it.id,
                F_ProjectNo = it.F_ProjectNo,
                F_ProjectName = it.F_ProjectName,
                F_ContractId = SqlFunc.Subqueryable<ACtcContractEntity>().EnableTableFilter().Where(c => c.id.Equals(it.F_ContractId)).Select(c => c.F_ContractCode),
                F_ProjectType = it.F_ProjectType,
                F_ProjectTypeName = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_ProjectType) && dic.DictionaryTypeId.Equals("2013172950349516800")).Select(dic => dic.FullName),
                F_Status = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Status) && dic.DictionaryTypeId.Equals("2013173013452820480")).Select(dic => dic.FullName),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_UpdateUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_UpdateUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_UpdateTime = it.F_UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).ToListAsync();


        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();

        return resData.FirstOrDefault();
    }


    /// <summary>
    /// 弹窗获取项目列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("PopupList")]
    public async Task<dynamic> GetPopupList()
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
             .Select(it => new AProdProjectListOutput
             {
                 id = it.id,
                 F_ProjectNo = it.F_ProjectNo,
                 F_ProjectName = it.F_ProjectName,
                 F_ContractId = it.F_ContractId,
                 F_ProjectType = it.F_ProjectType,
                 F_Status = it.F_Status,
                 F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                 F_UpdateTime = it.F_UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
             }).MergeTable().OrderBy(it => it.F_CreatorTime, OrderByType.Desc).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 项目类别
            if (item.F_ProjectType != null)
            {
                item.F_ProjectType = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(item.F_ProjectType) && it.DictionaryTypeId.Equals("2013172950349516800")).Select(it => it.FullName).FirstAsync();
            }

            // 项目状态
            if (item.F_Status != null)
            {
                item.F_Status = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(item.F_Status) && it.DictionaryTypeId.Equals("2013173013452820480")).Select(it => it.FullName).FirstAsync();
            }

        });

        return new {
            data = data
        };
    }


    #region 大屏

    /// <summary>
    /// 生产-项目进度.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("SC_XMJD")]
    public async Task<dynamic> GetSC_XMJD()
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
             .Select(it => new SC_XMJDOutput
             {
                 id = it.id,
                 F_ProjectNo = it.F_ProjectNo,
                 F_ProjectName = it.F_ProjectName,
                 F_ProjectType = it.F_ProjectType,
                 F_Status = it.F_Status,
                 F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
             }).MergeTable().OrderBy(it => it.F_CreatorTime, OrderByType.Desc).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 项目类别
            if (item.F_ProjectType != null)
            {
                item.F_ProjectType = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(item.F_ProjectType) && it.DictionaryTypeId.Equals("2013172950349516800")).Select(it => it.FullName).FirstAsync();
            }

            // 项目状态  项目进度
            item.F_Status = "已完成";
            item.F_WorkOrderQty = 0;
            item.F_CompletedQtyStatus = 0;
            item.F_PlanQty = 0;

            //总工序数/总计划数
            decimal allNum = 0;
            //完成工序数/已验收数
            decimal yesNum = 0;

            //已入库数量
            decimal stockFgNum = 0;

            if(item.F_ProjectType == "2")
            {
                //外协工单
                var orywxgdList = await _repositorywxgd.AsQueryable().Where(it => it.DeleteMark == null && it.F_ProjectId == item.id).Select(it => new { id = it.id, F_State = it.F_State, F_PlanQty = it.F_PlanNum }).ToListAsync();
                foreach (var proItem in orywxgdList)
                {
                    item.F_PlanQty += proItem.F_PlanQty;
                    item.F_WorkOrderQty += 1;
                    if (proItem.F_State != "2")
                    {
                        item.F_Status = "进行中";
                    }
                    else
                    {
                        item.F_CompletedQtyStatus += 1;
                    }
                    allNum = allNum + await _repositoryTask.AsQueryable().Where(it => it.F_ProcessingId == proItem.id && it.F_Type == "approver").CountAsync();
                    yesNum = allNum + await _repositoryTask.AsQueryable().Where(it => it.F_ProcessingId == proItem.id && it.F_TaskStatus == "2" && it.F_Type == "approver").CountAsync();

                    stockFgNum = stockFgNum + await _repositStockFgItem.AsQueryable().Where(it => it.F_WorkOrderId == proItem.id && it.DeleteMark == null).SumAsync(it => it.F_Num.Value);

                }
            }
            else
            {
                //加工单
                var processingList = await _repositoryProcessing.AsQueryable().Where(it => it.DeleteMark == null && it.F_ProjectId == item.id).Select(it => new { id = it.id, F_State = it.F_State, F_PlanQty = it.F_PlanQty }).ToListAsync();
                foreach (var proItem in processingList)
                {
                    item.F_PlanQty += proItem.F_PlanQty;
                    item.F_WorkOrderQty += 1;
                    if (proItem.F_State != "2")
                    {
                        item.F_Status = "进行中";
                    }
                    else
                    {
                        item.F_CompletedQtyStatus += 1;
                    }

                    allNum = allNum + await _repositoryTask.AsQueryable().Where(it => it.F_ProcessingId == proItem.id && it.F_Type == "approver").CountAsync();
                    yesNum = allNum + await _repositoryTask.AsQueryable().Where(it => it.F_ProcessingId == proItem.id && it.F_TaskStatus == "2" && it.F_Type == "approver").CountAsync();

                    stockFgNum = stockFgNum + await _repositStockFgItem.AsQueryable().Where(it => it.F_WorkOrderId == proItem.id && it.DeleteMark == null).SumAsync(it => it.F_Num.Value);

                }
            }
            item.F_ProjectProgress = item.F_WorkOrderQty == 0 ? "0%": (Math.Round(item.F_CompletedQtyStatus.Value * 100m / item.F_WorkOrderQty.Value, 2)).ToString() + "%";
            item.F_ProjectTask = allNum == 0 ? "0%" : (Math.Round((yesNum * 100m / allNum), 2)).ToString() + "%";
            item.F_ProjectStock = item.F_PlanQty == 0 ? "0%" : (Math.Round((stockFgNum * 100m / item.F_PlanQty.Value), 2)).ToString() + "%";
        });

        return new {
            data = data
        };
    }

    #endregion
}
//生产-项目进度
public class SC_XMJDOutput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 项目编号.
    /// </summary>
    public string? F_ProjectNo { get; set; }

    /// <summary>
    /// 项目名称.
    /// </summary>
    public string? F_ProjectName { get; set; }

    /// <summary>
    /// 项目类别.
    /// </summary>
    public string? F_ProjectType { get; set; }

    /// <summary>
    /// 项目状态.
    /// </summary>
    public string? F_Status { get; set; }

    /// <summary>
    /// 项目进度.
    /// </summary>
    public string? F_ProjectProgress { get; set; }

    /// <summary>
    /// 生产进度.
    /// </summary>
    public string? F_ProjectTask { get; set; }

    /// <summary>
    /// 入库进度.
    /// </summary>
    public string? F_ProjectStock { get; set; }

    /// <summary>
    /// 工单数.
    /// </summary>
    public int? F_WorkOrderQty { get; set; }

    /// <summary>
    /// 已完成工单.
    /// </summary>
    public int? F_CompletedQtyStatus { get; set; }

    /// <summary>
    /// 计划数.
    /// </summary>
    public int? F_PlanQty { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string F_CreatorTime { get; set; }

}