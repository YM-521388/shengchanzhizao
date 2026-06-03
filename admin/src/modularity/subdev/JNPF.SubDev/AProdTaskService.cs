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
using JNPF.example.Entitys.Dto.AProdTask;
using JNPF.example.Entitys.Dto.AProdTaskItem;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
using System.Text.Json;
using System.Threading.Tasks;
using static COSXML.Model.Object.SelectObjectResult;

namespace JNPF.example;

/// <summary>
/// 业务实现：a_prod_task.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdTask", Order = 200)]
[Route("api/example/[controller]")]
public class AProdTaskService : IAProdTaskService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdTaskEntity> _repository;
    private readonly ISqlSugarRepository<AProdTaskItemEntity> _repositoryItem;
    private readonly ISqlSugarRepository<AProdProcessEntity> _repositoryProcess;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositoryGoods;
    private readonly ISqlSugarRepository<UserEntity> _repositoryUser;
    private readonly ISqlSugarRepository<AProdProcessingEntity> _repositoryProcessing;
    private readonly ISqlSugarRepository<AProdReportEntity> _repositoryReport;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"工序\",\"field\":\"F_ProcessId\"},{\"value\":\"加工单\",\"field\":\"F_ProcessingId\"},{\"value\":\"任务类型\",\"field\":\"F_TaskType\"},{\"value\":\"生产数量\",\"field\":\"F_ProdQty\"},{\"value\":\"任务状态\",\"field\":\"F_TaskStatus\"},{\"value\":\"生产派工\",\"field\":\"F_ProdDispatch\"},{\"value\":\"质检派工\",\"field\":\"F_QcDispatch\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdTaskService"/>类型的新实例.
    /// </summary>
    public AProdTaskService(
        ISqlSugarRepository<AProdReportEntity> repositoryReport,
        ISqlSugarRepository<AProdProcessingEntity> repositoryProcessing,
        ISqlSugarRepository<UserEntity> repositoryUser,
        ISqlSugarRepository<AGoodsEntity> repositoryGoods,
        ISqlSugarRepository<AProdProcessEntity> repositoryProcess,
        ISqlSugarRepository<AProdTaskItemEntity> repositoryItem,
        ISqlSugarRepository<AProdTaskEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryReport = repositoryReport;
        _repositoryProcessing = repositoryProcessing;
        _repositoryGoods = repositoryGoods;
        _repositoryUser = repositoryUser;
        _repositoryProcess = repositoryProcess;
        _repositoryItem = repositoryItem;
        _repository = repository;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_prod_task.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.AProdTaskItemList)
            .Select(it => new AProdTaskEntity
            {
                id = it.id,
                F_ProcessingId = it.F_ProcessingId,
                F_ProcessId = it.F_ProcessId,
                F_PlanStartDate = it.F_PlanStartDate,
                F_PlanEndDate = it.F_PlanEndDate,
                F_ProdDispatch = it.F_ProdDispatch,
                F_QcDispatch = it.F_QcDispatch,
                F_ProdQty = it.F_ProdQty,
                F_TaskStatus = it.F_TaskStatus,
                F_TaskType = it.F_TaskType,
                F_SortCode = it.F_SortCode,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime,
                F_Reason = it.F_Reason,
                AProdTaskItemList = it.AProdTaskItemList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AProdTaskInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField0bb944, async aProdTaskItem =>
        {
            // 创建时间
            aProdTaskItem.F_CreatorTime = aProdTaskItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

    /// <summary>
    /// 获取a_prod_task列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("TaskList")]
    public async Task<dynamic> GetTaskList([FromBody] AProdTaskListQueryInput input)
    {
        var data = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_ProcessingId), it => it.F_ProcessingId.Equals(input.F_ProcessingId))
            .Where(it => it.FlowId==null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime,OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new AProdTaskListOutput
            {
                id = it.id,
                F_ProcessingId = it.F_ProcessingId,
                F_ProcessId = it.F_ProcessId,
                F_PlanStartDate = it.F_PlanStartDate.Value.ToString("yyyy-MM-dd"),
                F_PlanEndDate = it.F_PlanEndDate.Value.ToString("yyyy-MM-dd"),
                F_SortCode = it.F_SortCode,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 工序ID
            var F_ProcessIdData = await _dataInterfaceService.GetDynamicList("F_ProcessId", "2012092092830060544", "id", "F_ProcessName", "");
            if(item.F_ProcessId != null)
            {
                item.F_ProcessId = F_ProcessIdData.Find(it => it.id.Equals(item.F_ProcessId))?.fullName;
            }

        });
        return PageResult<AProdTaskListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取a_prod_task列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdTaskListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aProdTaskItemAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<AProdTaskListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_prod_task"))?.conditionalModel;
            if (allAuthorizeWhere.Any(it => it.TableName.Equals("a_prod_task_item"))) aProdTaskItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_prod_task_item"))?.conditionalModel;
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProcessingId), it => it.F_ProcessingId.Equals(input.F_ProcessingId))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProcessId), it => it.F_ProcessId.Equals(input.F_ProcessId))
            .WhereIF(input.F_PlanStartDate?.Count() > 0, it => SqlFunc.Between(it.F_PlanStartDate, input.F_PlanStartDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_PlanStartDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(input.F_PlanEndDate?.Count() > 0, it => SqlFunc.Between(it.F_PlanEndDate, input.F_PlanEndDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_PlanEndDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProdDispatch), it => it.F_ProdDispatch.Contains(input.F_ProdDispatch))
            .WhereIF(!string.IsNullOrEmpty(input.F_QcDispatch), it => it.F_QcDispatch.Contains(input.F_QcDispatch))
            .Where(authorizeWhere)
            .WhereIF(aProdTaskItemAuthorizeWhere?.Count > 0, it => it.AProdTaskItemList.Any(aProdTaskItemAuthorizeWhere))
            .Where(it => it.F_Type == "approver")
            .RightJoin<AProdProcessingEntity>((it, ps) => ps.id.Equals(it.F_ProcessingId) && ps.DeleteMark == null)
            .LeftJoin<AProdProcessEntity>((it, ps, pr) => pr.id.Equals(it.F_ProcessId) && pr.DeleteMark == null)
            .Select((it, ps, pr) => new AProdTaskListOutput
            {
                id = it.id,
                F_ProcessingId = ps.F_ProcessNo,
                F_ProcessId = it.F_ProcessId,
                F_ProcessName = pr.F_ProcessName,
                F_ProcessCode = pr.F_ProcessCode,
                F_ProdUserIds = pr.F_ProdUserIds,
                F_ProdUserByIds = pr.F_ProdUserIds,
                F_DefectIds = pr.F_DefectIds,
                F_QcUserIds = pr.F_QcUserIds,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == ps.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == ps.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == ps.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_ProdDispatch = it.F_ProdDispatch,
                F_QcDispatch = it.F_QcDispatch,
                F_PlanStartDate = it.F_PlanStartDate.Value.ToString("yyyy-MM-dd"),
                F_PlanEndDate = it.F_PlanEndDate.Value.ToString("yyyy-MM-dd"),
                F_StartDate = it.F_StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_EndDate = it.F_EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_ProdQty = it.F_ProdQty,
                F_TaskStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_TaskStatus) && dic.DictionaryTypeId.Equals("2013859706900189184")).Select(dic => dic.FullName),
                F_TaskType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_TaskType) && dic.DictionaryTypeId.Equals("2013859641523572736")).Select(dic => dic.FullName),
                F_TaskStatusCode = it.F_TaskStatus,
                F_TaskTypeCode = it.F_TaskType,
                F_Description = it.F_Description,
                F_Reason = it.F_Reason,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable()
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 生产人员
            if (item.F_ProdUserIds != null)
            {
                var F_ProdUserIdsUserSelect = item.F_ProdUserIds.ToObject<List<string>>();
                item.F_ProdUserIds = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_ProdUserIdsUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 不良品项ID
            if (item.F_DefectIds != null)
            {
                var F_DefectIdsData = await _dataInterfaceService.GetDynamicList("F_DefectIds", "2011648481097289728", "F_Id", "F_Name", "");
                var F_DefectIdsSelect = item.F_DefectIds.ToObject<List<string>>();
                item.F_DefectIds = string.Join(",", F_DefectIdsData.FindAll(it => F_DefectIdsSelect.Contains(it.id)).Select(s => s.fullName));
            }

            // 质检人员
            if (item.F_QcUserIds != null)
            {
                var F_QcUserIdsUserSelect = item.F_QcUserIds.ToObject<List<string>>();
                item.F_QcUserIds = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_QcUserIdsUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 生产派工
            if (item.F_ProdDispatch != null)
            {
                var F_ProdDispatchSelect = item.F_ProdDispatch.ToObject<List<string>>();
                item.F_ProdDispatch = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_ProdDispatchSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 质检派工
            if (item.F_QcDispatch != null)
            {
                var F_QcDispatchSelect = item.F_QcDispatch.ToObject<List<string>>();
                item.F_QcDispatch = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_QcDispatchSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

        });
        return PageResult<AProdTaskListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取任务物料列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("ItemList")]
    public async Task<dynamic> GetItemList([FromBody] AProdTaskListQueryInput input)
    {
        var data = await _repositoryItem.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_TaskId), it => it.F_TaskId.Equals(input.F_TaskId))
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .Select(it => new AProdTaskItemListOutput
            {
                F_Id = it.F_Id,
                F_TaskId = it.F_TaskId,
                F_GoodsId = it.F_GoodsId,
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => SqlFunc.MergeString(g.F_GoodsName, "-", g.F_GoodsNo)),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_Unit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit)) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                F_Num = it.F_Num,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
        });
        return PageResult<AProdTaskItemListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_prod_task.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] AProdTaskCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdTaskEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdTaskEntity>(new AProdTaskEntity()));
        ConfigModel tableField0bb944Config = new ConfigModel
        {
            tableName = "a_prod_task_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "物料清单",
            children = ExportImportDataHelper.GetTemplateParsing<AProdTaskItemEntity>(new AProdTaskItemEntity())
        };
        FieldsModel tableField0bb944Model = new FieldsModel()
        {
            __config__ = tableField0bb944Config,
            __vModel__ = "tableField0bb944"
        };
        fieldList.Add(tableField0bb944Model);
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;


        var aProdTaskItemEntityList = input.tableField0bb944.Adapt<List<AProdTaskItemEntity>>();
        if(aProdTaskItemEntityList != null)
        {
            foreach (var item in aProdTaskItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.AProdTaskItemList = aProdTaskItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.AProdTaskItemList)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新a_prod_task.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] AProdTaskUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdTaskEntity>();
        entity.id = id;
        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_ProcessingId,
                    it.F_ProcessId,
                    it.F_PlanStartDate,
                    it.F_PlanEndDate,
                    it.F_ProdDispatch,
                    it.F_QcDispatch,
                    it.F_ProdQty,
                    it.F_TaskStatus,
                    it.F_TaskType,
                    it.F_SortCode,
                    it.F_Description,
                })
                .ExecuteCommandAsync();
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
        AProdTaskEntity? entity = await _repository.GetFirstAsync(it => it.id == id);
        if (state == "4")
        {
            int isOk = await _repository.AsUpdateable().SetColumns(it => new AProdTaskEntity()
            {
                F_TaskStatus = "1",
                F_Reason = "",
            }).Where(it => it.id == id).ExecuteCommandAsync();

            await _repositoryProcessing.AsUpdateable().SetColumns(it => new AProdProcessingEntity()
            {
                F_State = "1"
            }).Where(it => it.id == entity.F_ProcessingId).ExecuteCommandAsync();
        }
        else
        {

            var processingEntity = await _repositoryProcessing.GetFirstAsync(it => it.id == entity.F_ProcessingId);
            if (processingEntity.F_State != "1" && processingEntity.F_State != "2")
            {
                throw Oops.Oh(ErrorCode.COM1018, "编号为【" + processingEntity.F_ProcessNo + "】的加工单不是生产中、已完成状态，不可修改加工单工序状态");
            }

            int isOk = await _repository.AsUpdateable().SetColumns(it => new AProdTaskEntity()
            {
                F_TaskStatus = state
            }).Where(it => it.id == id).ExecuteCommandAsync();

            if (state == "0" || state == "1") {
                await _repositoryProcessing.AsUpdateable().SetColumns(it => new AProdProcessingEntity()
                {
                    F_State = "1"
                }).Where(it => it.id == entity.F_ProcessId).ExecuteCommandAsync();
            }
            if (state == "2")
            {
                // 检查是否所有任务都已完成（状态为"2"）
                var allTasksCompleted = !await _repository.AsQueryable()
                    .Where(it => it.id == id)
                    .Where(it => it.F_TaskStatus != "2")
                    .AnyAsync();

                if (allTasksCompleted)
                {
                    await _repositoryProcessing.AsUpdateable()
                        .SetColumns(it => new AProdProcessingEntity { F_State = "2" })
                        .Where(it => it.id == entity.F_ProcessId)
                        .ExecuteCommandAsync();
                }
            }
        }
    }

    /// <summary>
    /// 暂停原因.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("ReasonUpdate/{id}")]
    [UnitOfWork]
    public async Task ReasonUpdate(string id, [FromBody] AProdTaskUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (oldEntity != null && oldEntity.F_TaskStatus != "1") {
            throw Oops.Oh(ErrorCode.COM1018, "只有进行中的工序可以暂停");
        }


        var entity = input.Adapt<AProdTaskEntity>();
        entity.id = id;
        entity.F_TaskStatus = "3";
        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_TaskStatus,
                    it.F_Reason,
                })
                .ExecuteCommandAsync();
        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 删除a_prod_task.
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
        await _repository.AsDeleteable().Where(it => it.id.Equals(id)).ExecuteCommandAsync();

        // 清空生产任务物料清单表数据
        await _repository.AsSugarClient().Deleteable<AProdTaskItemEntity>().Where(it => it.F_TaskId.Equals(entity.id)).ExecuteCommandAsync();
    }

    /// <summary>
    /// 批量删除a_prod_task.
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
            // 清空生产任务物料清单表数据
            await _repository.AsSugarClient().Deleteable<AProdTaskItemEntity>().Where(u => entitys.Select(s => s.id).ToList().Contains(u.F_TaskId)).ExecuteCommandAsync();
            // 批量删除a_prod_task
            await _repository.AsDeleteable().In(it => it.id,input.ids).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// a_prod_task详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.AProdTaskItemList)
            .Where(it => it.id == id)
            .LeftJoin<AProdProcessingEntity>((it, ps) => ps.id.Equals(it.F_ProcessingId) && ps.DeleteMark == null)
            .LeftJoin<AProdProcessEntity>((it, ps, pr) => pr.id.Equals(it.F_ProcessId) && pr.DeleteMark == null)
            .Select((it, ps, pr) => new AProdTaskDetailOutput
            {
                id = it.id,
                F_ProcessingId = it.F_ProcessingId,
                F_ProcessId = it.F_ProcessId,
                F_ProcessName = pr.F_ProcessName,
                F_ProcessCode = pr.F_ProcessCode,
                F_IsQc = SqlFunc.IIF(SqlFunc.ToInt32(pr.F_IsQc) == 1, "是", "否"),
                F_WorkshopId = pr.F_WorkshopId,
                F_Line = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(pr.F_Line) && dic.DictionaryTypeId.Equals("2011623836151320576")).Select(dic => dic.FullName),
                F_ReportUnit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(pr.F_ReportUnit) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                F_UnitRatio = "生产1个计划数， 需要" + (pr.F_UnitRatio.ToString() ?? "0"),
                F_PriceType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(pr.F_PriceType) && dic.DictionaryTypeId.Equals("2011642610875240448")).Select(dic => dic.FullName),
                F_WagePrice = pr.F_WagePrice,
                F_StdHour = pr.F_StdHour + "小时" + pr.F_StdMinute + "分钟" + pr.F_StdSecond + "秒",
                F_MachineId = pr.F_MachineId,
                F_ProdUserIds = pr.F_ProdUserIds,
                F_DefectIds = pr.F_DefectIds,
                F_QcUserIds = pr.F_QcUserIds,
                F_PlanStartDate = it.F_PlanStartDate.Value.ToString("yyyy-MM-dd"),
                F_PlanEndDate = it.F_PlanEndDate.Value.ToString("yyyy-MM-dd"),
                F_ProdDispatch = it.F_ProdDispatch,
                F_QcDispatch = it.F_QcDispatch,
                F_ProdQty = it.F_ProdQty.ToString(),
                F_TaskStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_TaskStatus) && dic.DictionaryTypeId.Equals("2013859706900189184")).Select(dic => dic.FullName),
                F_TaskType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_TaskType) && dic.DictionaryTypeId.Equals("2013859641523572736")).Select(dic => dic.FullName),
                F_SortCode = it.F_SortCode.ToString(),
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableField0bb944 = it.AProdTaskItemList.Adapt<List<AProdTaskItemDetailOutput>>(),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 加工单ID
            if(item.F_ProcessingId != null)
            {
                var F_ProcessingIdData = await _dataInterfaceService.GetDynamicList("F_ProcessingId", "2013860549426810880", "F_Id", "F_ProcessNo", "");
                item.F_ProcessingId = F_ProcessingIdData.Find(it => it.id.Equals(item.F_ProcessingId))?.fullName;
            }

            // 工序ID
            if(item.F_ProcessId != null)
            {
                var F_ProcessIdData = await _dataInterfaceService.GetDynamicList("F_ProcessId", "2012092092830060544", "id", "F_ProcessName", "");
                item.F_ProcessId = F_ProcessIdData.Find(it => it.id.Equals(item.F_ProcessId))?.fullName;
            }

            // 生产人员
            if (item.F_ProdUserIds != null)
            {
                var F_ProdUserIdsUserSelect = item.F_ProdUserIds.ToObject<List<string>>();
                item.F_ProdUserIds = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_ProdUserIdsUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 不良品项ID
            if (item.F_DefectIds != null)
            {
                var F_DefectIdsData = await _dataInterfaceService.GetDynamicList("F_DefectIds", "2011648481097289728", "F_Id", "F_Name", "");
                var F_DefectIdsSelect = item.F_DefectIds.ToObject<List<string>>();
                item.F_DefectIds = string.Join(",", F_DefectIdsData.FindAll(it => F_DefectIdsSelect.Contains(it.id)).Select(s => s.fullName));
            }

            // 质检人员
            if (item.F_QcUserIds != null)
            {
                var F_QcUserIdsUserSelect = item.F_QcUserIds.ToObject<List<string>>();
                item.F_QcUserIds = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_QcUserIdsUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 生产派工
            if (item.F_ProdDispatch != null)
            {
                var F_ProdDispatchSelect = item.F_ProdDispatch.ToObject<List<string>>();
                item.F_ProdDispatch = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_ProdDispatchSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 质检派工
            if (item.F_QcDispatch != null)
            {
                var F_QcDispatchSelect = item.F_QcDispatch.ToObject<List<string>>();
                item.F_QcDispatch = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_QcDispatchSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 机台
            if (item.F_MachineId != null)
            {
                var F_MachineIdData = await _dataInterfaceService.GetDynamicList("F_MachineId", "2011729284707782656", "id", "F_MachineName", "");
                var F_MachineIdSelect = item.F_MachineId.ToObject<string[]>();
                item.F_MachineId = string.Join(",", F_MachineIdData.FindAll(it => F_MachineIdSelect.Contains(it.id)).Select(s => s.fullName));
            }

            var F_WorkshopIdData = await _dataInterfaceService.GetDynamicList("F_WorkshopId", "2011621894347952128", "F_Id", "F_Name", "children");
            if (item.F_WorkshopId != null)
            {
                var F_WorkshopIdCascader = item.F_WorkshopId.ToObject<List<string>>();
                item.F_WorkshopId = string.Join("/", F_WorkshopIdData.FindAll(it => F_WorkshopIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField0bb944), async aProdTaskItem =>
        {
            var aProdTask = data.ToList().Find(it => it.id.Equals(aProdTaskItem.F_TaskId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 商品
            if(aProdTaskItem.F_GoodsId != null)
            {
                var goodEntity = await _repositoryGoods.GetFirstAsync(it => it.id == aProdTaskItem.F_GoodsId);
                aProdTaskItem.F_GoodsId = goodEntity?.F_GoodsName;
                aProdTaskItem.F_GoodsNo = goodEntity?.F_GoodsNo;
                aProdTaskItem.F_Specification = goodEntity?.F_Specification;
                aProdTaskItem.F_UnitTwo = goodEntity?.F_Unit;
                aProdTaskItem.F_UnitTwo = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(aProdTaskItem.F_UnitTwo) && it.DictionaryTypeId.Equals("2008448689420505088")).Select(it => it.FullName).FirstAsync();
            }

            // 创建时间
            aProdTaskItem.F_CreatorTime = aProdTaskItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        return data.FirstOrDefault();
    }


    /// <summary>
    /// 获取生产人员、质检人员列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("UserCon/{id}")]
    public async Task<dynamic> GetUserCon(string id)
    {
        var taskEntity = await _repository.GetFirstAsync(it => it.id == id);
        var dataEntity = await _repositoryProcess.GetFirstAsync(it => it.DeleteMark == null && it.id == taskEntity.F_ProcessId);
        var prodUserList = new List<object>();
        var qcUserList = new List<object>();
        if (dataEntity != null) {
            if (!string.IsNullOrEmpty(dataEntity.F_ProdUserIds))
            {
                var prodList = JsonSerializer.Deserialize<List<string>>(dataEntity.F_ProdUserIds);
                foreach (var prodItem in prodList)
                {
                    var userEntity = _repositoryUser.GetFirst(it => it.DeleteMark == null && it.Id == prodItem);
                    if (userEntity != null)
                    {
                        prodUserList.Add(new { id = userEntity.Id, fullName = userEntity.RealName });
                    }
                }
            }
            if (!string.IsNullOrEmpty(dataEntity.F_QcUserIds))
            {
                var qcList = JsonSerializer.Deserialize<List<string>>(dataEntity.F_QcUserIds);
                foreach (var qcItem in qcList)
                {
                    var userEntity = _repositoryUser.GetFirst(it => it.DeleteMark == null && it.Id == qcItem);
                    if (userEntity != null)
                    {
                        qcUserList.Add(new { id = userEntity.Id, fullName = userEntity.RealName });
                    }
                }
            }
        }

        return new {
            prodUserList = prodUserList,
            qcUserList = qcUserList,
        };
    }


    /// <summary>
    /// 大屏加工单-生产任务信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("JGD-SCRWXX")]
    public async Task<dynamic> GetJGD_SCRWXX(string id)
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.F_Type == "approver" && it.F_ProcessingId == id)
            .RightJoin<AProdProcessingEntity>((it, ps) => ps.id.Equals(it.F_ProcessingId) && ps.DeleteMark == null)
            .LeftJoin<AProdProcessEntity>((it, ps, pr) => pr.id.Equals(it.F_ProcessId) && pr.DeleteMark == null)
            .Select((it, ps, pr) => new AProdTaskAPPListOutput
            {
                id = it.id,
                F_ProcessingId = ps.F_ProcessNo,
                F_ProcessId = it.F_ProcessId,
                F_ProcessName = pr.F_ProcessName,
                F_ProdQty = it.F_ProdQty,
                F_TaskStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_TaskStatus) && dic.DictionaryTypeId.Equals("2013859706900189184")).Select(dic => dic.FullName),
                F_TaskStatusCode = it.F_TaskStatus,
                F_SortCode = it.F_SortCode,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable()
            .OrderBy(it => it.F_SortCode).OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            item.F_CompletedQty = await _repositoryReport.AsQueryable()
                .Where(r => r.F_TaskId == item.id)
                .SumAsync(r => SqlFunc.IsNull(r.F_GoodQty, 0)) ?? 0;

        });
        return data;
    }

    #region APP

    /// <summary>
    /// 获取a_prod_task列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("APPYGList")]
    public async Task<dynamic> GetAPPYGList([FromBody] AProdTaskListQueryInput input)
    {
        var selectIds = input.selectIds?.Split(",").ToList();
        var data = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_ProcessId), it => it.F_ProcessId.Equals(input.F_ProcessId))
            .WhereIF(!string.IsNullOrEmpty(input.UserId) && string.IsNullOrEmpty(input.F_ProcessingId) || (string.IsNullOrEmpty(input.UserId) && !string.IsNullOrEmpty(input.F_ProcessingId)), it => it.F_ProdDispatch.Contains(input.UserId) || it.F_ProcessingId.Equals(input.F_ProcessingId))
            .WhereIF(string.IsNullOrEmpty(input.UserId) && string.IsNullOrEmpty(input.F_ProcessingId),it => 1 == 2)
            .WhereIF(!string.IsNullOrEmpty(input.UserId) && !string.IsNullOrEmpty(input.F_ProcessingId), it => it.F_ProdDispatch.Contains(input.UserId) && it.F_ProcessingId.Equals(input.F_ProcessingId))
            //.Where(it => it.F_Type == "approver" && it.F_ProdDispatch.Contains(input.F_ProdDispatch) && !string.IsNullOrEmpty(input.F_ProdDispatch) && (it.F_TaskStatus == "1" || it.F_TaskStatus == "2"))
            .Where(it => it.F_Type == "approver" && (it.F_TaskStatus == "1" || it.F_TaskStatus == "2"))
            .RightJoin<AProdProcessingEntity>((it, ps) => ps.id.Equals(it.F_ProcessingId) && ps.DeleteMark == null)
            .LeftJoin<AProdProcessEntity>((it, ps, pr) => pr.id.Equals(it.F_ProcessId) && pr.DeleteMark == null)
            .Select((it, ps, pr) => new AProdTaskAPPListOutput
            {
                id = it.id,
                F_ProcessingId = ps.F_ProcessNo,
                F_ProcessId = it.F_ProcessId,
                F_ProcessName = pr.F_ProcessName,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == ps.F_GoodsId && g.DeleteMark == null).Select(g => SqlFunc.MergeString(g.F_GoodsName, "/", g.F_GoodsNo)),
                F_PlanStartDate = it.F_PlanStartDate.Value.ToString("yyyy-MM-dd"),
                F_PlanEndDate = it.F_PlanEndDate.Value.ToString("yyyy-MM-dd"),
                F_ProdQty = it.F_ProdQty,
                F_TaskStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_TaskStatus) && dic.DictionaryTypeId.Equals("2013859706900189184")).Select(dic => dic.FullName),
                F_TaskStatusCode = it.F_TaskStatus,
                F_SortCode = it.F_SortCode,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable()
            .OrderBy(it => it.F_ProcessingId).OrderBy(it => it.F_SortCode).OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            item.F_CompletedQty = await _repositoryReport.AsQueryable()
                .Where(r => r.F_TaskId == item.id)
                .SumAsync(r => SqlFunc.IsNull(r.F_GoodQty, 0)) ?? 0;

        });
        return PageResult<AProdTaskAPPListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 完成.
    /// </summary>
    [HttpPost("Completed")]
    public async Task<dynamic> Completed(string id)
    {
        AProdTaskEntity? entity = await _repository.GetFirstAsync(it => it.id == id);
        var processingEntity = await _repositoryProcessing.GetFirstAsync(it => it.id == entity.F_ProcessingId);

        int isOk = await _repository.AsUpdateable().SetColumns(it => new AProdTaskEntity()
        {
            F_TaskStatus = "2"
        }).Where(it => it.id == id).ExecuteCommandAsync();

        var allTasksCompleted = !await _repository.AsQueryable()
            .Where(it => it.id == id)
            .Where(it => it.F_TaskStatus != "2")
            .AnyAsync();

        if (allTasksCompleted)
        {
            await _repositoryProcessing.AsUpdateable()
                .SetColumns(it => new AProdProcessingEntity { F_State = "2" })
                .Where(it => it.id == entity.F_ProcessId)
                .ExecuteCommandAsync();
        }

        if (!(isOk > 0))
        {
            return new {
                code = 201,
                info = "修改失败"
            };
        }
        else
        {
            return new {
                code = 200,
                info = "工序已完成"
            };
        }
    }


    /// <summary>
    /// 用户统计信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Statistics")]
    public async Task<dynamic> GetStatistics(string userId)
    {
        var now = DateTime.Now;
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var monthEnd = monthStart.AddMonths(1);

        // 串行执行：先查报工数据（避免连接冲突）
        var reportData = await _repositoryReport.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_ProdUserId.Contains(userId))
            .Where(it => it.F_CreatorTime >= monthStart && it.F_CreatorTime < monthEnd)
            .Select(it => new {
                AllQty = SqlFunc.IsNull(it.F_GoodQty, 0) + SqlFunc.IsNull(it.F_GoodNotQty, 0),
                NotQty = SqlFunc.IsNull(it.F_GoodNotQty, 0),
                DateKey = it.F_CreatorTime.Value.Date
            })
            .ToListAsync();

        // 再查任务数量（串行，使用新连接）
        var taskNum = await _repository.AsQueryable()
            .Where(it => it.F_ProdDispatch.Contains(userId))
            .Where(it => it.F_CreatorTime >= monthStart && it.F_CreatorTime < monthEnd)
            .CountAsync();

        // 内存中聚合计算（避免多次循环）
        var allQty = reportData.Sum(x => x.AllQty.Value);
        var notQty = reportData.Sum(x => x.NotQty.Value);
        var badRate = allQty == 0 ? 0 : Math.Round((decimal)notQty / allQty * 100, 2);

        // 统计工作天数（按日期去重）
        var workDays = reportData.Select(x => x.DateKey).Distinct().Count();

        // 可选：按天统计明细（如需展示每日数据）
        //var dailyStats = reportData
        //    .GroupBy(x => x.DateKey)
        //    .Select(g => new {
        //        Date = g.Key,
        //        DayAllQty = g.Sum(x => x.AllQty),
        //        DayNotQty = g.Sum(x => x.NotQty),
        //        DayBadRate = g.Sum(x => x.AllQty) == 0 ? 0 : Math.Round((decimal)g.Sum(x => x.NotQty.Value) / g.Sum(x => x.AllQty.Value) * 100, 2)
        //    })
        //    .OrderBy(x => x.Date)
        //    .ToList();

        return new {
            AllQty = allQty,
            NotQty = notQty,
            BadRate = badRate,
            TaskNum = taskNum,
            WorkDays = workDays,           // 新增：工作天数
            //DailyStats = dailyStats        // 可选：每日明细
        };
    }

    #endregion
}
