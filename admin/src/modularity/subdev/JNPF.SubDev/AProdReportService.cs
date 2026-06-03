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
using JNPF.example.Entitys.Dto.AProdReport;
using JNPF.example.Entitys.Dto.AProdReportDefect;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
using JNPF.example.Entitys.Dto.AProdDefect;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace JNPF.example;

/// <summary>
/// 业务实现：a_prod_report.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdReport", Order = 200)]
[Route("api/example/[controller]")]
public class AProdReportService : IAProdReportService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdReportEntity> _repository;
    private readonly ISqlSugarRepository<AProdTaskEntity> _repositoryTask;
    private readonly ISqlSugarRepository<AProdProcessingEntity> _repositoryProcessing;
    private readonly ISqlSugarRepository<AProdReportDefectEntity> _repositoryDefect;
    private readonly ISqlSugarRepository<AProdReportLogEntity> _repositoryLog;
    private readonly ISqlSugarRepository<AProdDefectEntity> _repositoryProdDefect;
    private readonly ISqlSugarRepository<AProdMachineEntity> _repositoryMachine;
    private readonly ISqlSugarRepository<AProdProcessEntity> _repositoryProcess;

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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"生产人员\",\"field\":\"F_ProdUserId\"},{\"value\":\"良品数\",\"field\":\"F_GoodQty\"},{\"value\":\"不良品数\",\"field\":\"F_GoodNotQty\"},{\"value\":\"不良品项\",\"field\":\"F_CreatorUserId\"},{\"value\":\"开始时间\",\"field\":\"F_StartTime\"},{\"value\":\"结束时间\",\"field\":\"F_EndTime\"},{\"value\":\"报工类型\",\"field\":\"F_State\"},{\"value\":\"工资单价\",\"field\":\"F_WagePrice\"},{\"value\":\"结算状态\",\"field\":\"F_SettleStatus\"},{\"value\":\"结算人\",\"field\":\"F_SettleUserId\"},{\"value\":\"结算时间\",\"field\":\"F_SettleTime\"},{\"value\":\"附件\",\"field\":\"F_Files\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdReportService"/>类型的新实例.
    /// </summary>
    public AProdReportService(
        ISqlSugarRepository<AProdProcessingEntity> repositoryProcessing,
        ISqlSugarRepository<AProdMachineEntity> repositoryMachine,
        ISqlSugarRepository<UserEntity> repositoryUser,
        ISqlSugarRepository<AProdDefectEntity> repositoryProdDefect,
        ISqlSugarRepository<AProdReportLogEntity> repositoryLog,
        ISqlSugarRepository<AProdReportDefectEntity> repositoryDefect,
        ISqlSugarRepository<AProdTaskEntity> repositoryTask,
        ISqlSugarRepository<AProdProcessEntity> repositoryProcess,
        ISqlSugarRepository<AProdReportEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryProcess = repositoryProcess;
        _repositoryMachine = repositoryMachine;
        _repositoryUser = repositoryUser;
        _repositoryProdDefect = repositoryProdDefect;
        _repositoryLog = repositoryLog;
        _repositoryDefect = repositoryDefect;
        _repositoryTask = repositoryTask;
        _repositoryProcessing = repositoryProcessing;
        _repository = repository;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_prod_report.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.AProdReportDefectList)
            .Select(it => new AProdReportEntity
            {
                id = it.id,
                F_ProdUserId = it.F_ProdUserId,
                F_TaskId = it.F_TaskId,
                F_GoodQty = it.F_GoodQty,
                F_GoodNotQty = it.F_GoodNotQty,
                F_StartTime = it.F_StartTime,
                F_EndTime = it.F_EndTime,
                F_Files = it.F_Files,
                F_SettleStatus = it.F_SettleStatus,
                F_SettleUserId = it.F_SettleUserId,
                F_SettleTime = it.F_SettleTime,
                F_WagePrice = it.F_WagePrice,
                F_State = it.F_State,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime,
                AProdReportDefectList = it.AProdReportDefectList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AProdReportInfoOutput>(); 

        if (data == null) return data;
        var taseEntity = await _repositoryTask.GetFirstAsync(it => it.id == data.F_TaskId);
        if(taseEntity != null)
        {
            data.F_ProcessId = taseEntity.F_ProcessId;
        }
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField579169, async aProdReportDefect =>
        {
            // 创建时间
            aProdReportDefect.F_CreatorTime = aProdReportDefect.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

    /// <summary>
    /// 获取a_prod_report列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdReportListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aProdReportDefectAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<AProdReportListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_prod_report"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_prod_report_defect"))) aProdReportDefectAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_prod_report_defect"))?.conditionalModel;
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var startF_GoodQty = input.F_GoodQty?.FirstOrDefault()?.ParseToDecimal() == null ? decimal.MinValue : input.F_GoodQty?.First();
        var endF_GoodQty = input.F_GoodQty?.LastOrDefault()?.ParseToDecimal() == null ? decimal.MaxValue : input.F_GoodQty?.Last();
        var startF_GoodNotQty = input.F_GoodNotQty?.FirstOrDefault()?.ParseToDecimal() == null ? decimal.MinValue : input.F_GoodNotQty?.First();
        var endF_GoodNotQty = input.F_GoodNotQty?.LastOrDefault()?.ParseToDecimal() == null ? decimal.MaxValue : input.F_GoodNotQty?.Last();

        var taskList = new List<string>(); 
        if(!string.IsNullOrEmpty(input.F_ProcessingId) || !string.IsNullOrEmpty(input.F_ProcessId))
        {
            taskList = await _repositoryTask.AsQueryable().WhereIF(!string.IsNullOrEmpty(input.F_ProcessingId), it => it.F_ProcessingId.Equals(input.F_ProcessingId)).WhereIF(!string.IsNullOrEmpty(input.F_ProcessId), it => it.F_ProcessId.Equals(input.F_ProcessId)).Select(it => it.id).ToListAsync();
        }

        var idsQ = await _repository.AsQueryable()
            .Includes(x => x.AProdReportDefectList
            .WhereIF(!string.IsNullOrEmpty(input.tableField579169_F_DefectId), it => it.F_DefectId.Equals(input.tableField579169_F_DefectId))
            .Where(it => it.DeleteMark == null)
            .ToList())
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProcessingId) || !string.IsNullOrEmpty(input.F_ProcessId), it => taskList.Contains(it.F_TaskId))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProdUserId), it => it.F_ProdUserId.Equals(input.F_ProdUserId))
            .WhereIF(input.F_GoodQty?.Count() > 0, it => SqlFunc.Between(it.F_GoodQty, startF_GoodQty, endF_GoodQty))
            .WhereIF(input.F_GoodNotQty?.Count() > 0, it => SqlFunc.Between(it.F_GoodNotQty, startF_GoodNotQty, endF_GoodNotQty))
            .WhereIF(input.F_StartTime?.Count() > 0, it => SqlFunc.Between(it.F_StartTime, input.F_StartTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_StartTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(input.F_EndTime?.Count() > 0, it => SqlFunc.Between(it.F_EndTime, input.F_EndTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_EndTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_Description), it => it.F_Description.Contains(input.F_Description))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .WhereIF(!string.IsNullOrEmpty(input.tableField579169_F_DefectId), it => it.AProdReportDefectList.Any(aProdReportDefect => aProdReportDefect.F_DefectId.Equals(input.tableField579169_F_DefectId)))
            .Where(authorizeWhere)
            .WhereIF(aProdReportDefectAuthorizeWhere?.Count > 0, it => it.AProdReportDefectList.Any(aProdReportDefectAuthorizeWhere))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select((it) => new AProdReportListOutput
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
            .RightJoin<AProdTaskEntity>((it, ts) => ts.id.Equals(it.F_TaskId))
            .RightJoin<AProdProcessingEntity>((it, ts, ps) => ps.id.Equals(ts.F_ProcessingId) && ps.DeleteMark == null)
            .LeftJoin<AProdProcessEntity>((it, ts, ps, pr) => pr.id.Equals(ts.F_ProcessId) && pr.DeleteMark == null)
            .Select((it, ts, ps, pr) => new AProdReportListOutput
            {
                id = it.id,
                F_ProdUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_ProdUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_GoodQty = it.F_GoodQty,
                F_GoodNotQty = it.F_GoodNotQty,
                F_Num = (it.F_GoodQty ?? 0) + (it.F_GoodNotQty ?? 0),
                F_StartTime = it.F_StartTime.Value.ToString("yyyy-MM-dd"),
                F_EndTime = it.F_EndTime.Value.ToString("yyyy-MM-dd"),
                F_DurationMinutes = SqlFunc.DateDiff(DateType.Minute, it.F_StartTime.Value, it.F_EndTime.Value),
                F_Files = it.F_Files,
                F_SettleStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_SettleStatus) && dic.DictionaryTypeId.Equals("2014214471169478656")).Select(dic => dic.FullName),
                F_SettleStatusCode = it.F_SettleStatus,
                F_SettleUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_SettleUserId)).Select(u => u.RealName),
                F_SettleTime = it.F_SettleTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_WagePrice = it.F_WagePrice,
                F_State = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_State) && dic.DictionaryTypeId.Equals("2014253420491444224")).Select(dic => dic.FullName),
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),

                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == ps.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == ps.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == ps.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_PlanStartDate = ts.F_PlanStartDate.Value.ToString("yyyy-MM-dd"),
                F_PlanEndDate = ts.F_PlanEndDate.Value.ToString("yyyy-MM-dd"),
                F_ProcessName = pr.F_ProcessName,
                F_PriceType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(pr.F_PriceType) && dic.DictionaryTypeId.Equals("2011642610875240448")).Select(dic => dic.FullName),
                F_ProcessingId = ps.F_ProcessNo,
                F_ProcessingState = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(ps.F_State) && dic.DictionaryTypeId.Equals("2013819135649255424")).Select(dic => dic.FullName),
                F_ProcessingStateCode = ps.F_State,
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 生产人员
            //if(item.F_ProdUserId != null)
            //{
            //    var F_ProdUserIdUserSelect = item.F_ProdUserId.ToObject<List<string>>();
            //    item.F_ProdUserId = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => F_ProdUserIdUserSelect.Contains(it.Id)).Select(it => it.RealName).ToListAsync());
            //}

            if (item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }

            // 格式化报工时长
            var hours = item.F_DurationMinutes / 60;
            var minutes = item.F_DurationMinutes % 60;
            item.F_DurationStr = $"{hours}小时{minutes}分钟";

        });

        data.pagination = idsQ.pagination;

        return PageResult<AProdReportListOutput>.SqlSugarPageResult(data);
    }


    /// <summary>
    /// 获取报工下的不良品项列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("DefectList")]
    public async Task<dynamic> GetDefectList([FromBody] AProdReportListQueryInput input)
    {
        var data = await _repositoryDefect.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_ReportId), it => it.F_ReportId.Contains(input.F_ReportId))
            .Where(it => it.DeleteMark == null)
            .Select(it => new AProdReportDefectListOutput
            {
                F_Id = it.F_Id,
                F_DefectId = SqlFunc.Subqueryable<AProdDefectEntity>().EnableTableFilter().Where(d => d.id.Equals(it.F_DefectId) && d.DeleteMark == null).Select(d => d.F_DefectName),
                F_Num = it.F_Num,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        var resData = data.list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);

        data.list = resData.ToObject<List<AProdReportDefectListOutput>>(CommonConst.options);

        return PageResult<AProdReportDefectListOutput>.SqlSugarPageResult(data);
    }


    /// <summary>
    /// 新建a_prod_report.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] AProdReportCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdReportEntity>();
        entity.id = SnowflakeIdHelper.NextId();

        var taskEntity = await _repositoryTask.GetFirstAsync(it => it.id == entity.F_TaskId);
        var processingEntity = await _repositoryProcessing.GetFirstAsync(it => it.id == taskEntity.F_ProcessingId);
        if (processingEntity.F_State != "1")
        {
            throw Oops.Oh(ErrorCode.COM1018, "编号为【" + processingEntity.F_ProcessNo + "】的加工单不是生产中状态不可报工");
        }

        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdReportEntity>(new AProdReportEntity()));
        ConfigModel tableField579169Config = new ConfigModel
        {
            tableName = "a_prod_report_defect",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "不良品项",
            children = ExportImportDataHelper.GetTemplateParsing<AProdReportDefectEntity>(new AProdReportDefectEntity())
        };
        FieldsModel tableField579169Model = new FieldsModel()
        {
            __config__ = tableField579169Config,
            __vModel__ = "tableField579169"
        };
        fieldList.Add(tableField579169Model);
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        //日志
        var logEntity = new AProdReportLogEntity();
        logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        logEntity.F_CreatorUserId = _userManager.UserId;
        logEntity.id = SnowflakeIdHelper.NextId();
        logEntity.F_ReportId = entity.id;
        logEntity.F_Title = "新增报工";
        logEntity.F_Content = "新增报工信息";
        await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();


        var aProdReportDefectEntityList = input.tableField579169.Adapt<List<AProdReportDefectEntity>>();
        if(aProdReportDefectEntityList != null)
        {
            foreach (var item in aProdReportDefectEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.AProdReportDefectList = aProdReportDefectEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.AProdReportDefectList)
            .ExecuteCommandAsync();


        //如果工单总合格数量大于计划数，则工单状态自动变为已完成
        var taskNumEntity = await _repositoryTask.GetFirstAsync(it => it.id == entity.F_TaskId);
        if (taskNumEntity != null)
        {
            var outsourceAcceptNum = (await _repository.AsQueryable().Where(it => it.F_TaskId == entity.F_TaskId && it.DeleteMark == null).SumAsync(it => it.F_GoodQty)) ?? 0;
            if (outsourceAcceptNum >= taskNumEntity.F_ProdQty)
            {
                taskNumEntity.F_TaskStatus = "2";
                await _repositoryTask.AsUpdateable(taskNumEntity)
                 .UpdateColumns(it => new {
                     it.F_TaskStatus,
                 })
                 .ExecuteCommandAsync();
            }
        }
    }

    /// <summary>
    /// 更新a_prod_report.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] AProdReportUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        var entity = input.Adapt<AProdReportEntity>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdReportEntity>(new AProdReportEntity()));
        ConfigModel tableField579169Config = new ConfigModel
        {
            tableName = "a_prod_report_defect",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "不良品项",
            children = ExportImportDataHelper.GetTemplateParsing<AProdReportDefectEntity>(new AProdReportDefectEntity())
        };
        FieldsModel tableField579169Model = new FieldsModel()
        {
            __config__ = tableField579169Config,
            __vModel__ = "tableField579169"
        };
        fieldList.Add(tableField579169Model);

        // 移除报工不良品项信息可能被删除数据
        if (input.tableField579169 !=null && input.tableField579169.Any())
            await _repository.AsSugarClient().Deleteable<AProdReportDefectEntity>().Where(it => it.F_ReportId == entity.id && !input.tableField579169.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<AProdReportDefectEntity>().Where(it => it.F_ReportId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增报工不良品项信息新数据
        var aProdReportDefectEntityList = input.tableField579169.Adapt<List<AProdReportDefectEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_ProdUserId,
                    it.F_GoodQty,
                    it.F_GoodNotQty,
                    it.F_StartTime,
                    it.F_EndTime,
                    it.F_Files,
                    it.F_WagePrice,
                    it.F_Description,
                })
                .ExecuteCommandAsync();

            //日志
            var changeList = new List<string>();
            string safeToStr(object v) => v == null ? string.Empty : v.ToString();

            void AddChange(string label, object oldVal, object newVal)
            {
                var o = safeToStr(oldVal);
                var n = safeToStr(newVal);
                if (o != n)
                {
                    changeList.Add($"{label} 值由 {o} 改为 {n}");
                }
            }
            AddChange("【良品数】", oldEntity.F_GoodQty, entity.F_GoodQty);
            if (oldEntity.F_ProdUserId != entity.F_ProdUserId)
            {
                string oldName = "";
                string newName = "";
                if (oldEntity.F_ProdUserId != null)
                {
                    oldName = (await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => it.Id == oldEntity.F_ProdUserId).Select(it => it.RealName).ToListAsync())?.FirstOrDefault();
                }
                if (entity.F_ProdUserId != null)
                {
                    newName = (await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => it.Id == entity.F_ProdUserId).Select(it => it.RealName).ToListAsync())?.FirstOrDefault();
                }
                changeList.Add($"【生产人员】 值由 {oldName} 改为 {newName}");
            }
            AddChange("【不良品数】", oldEntity.F_GoodNotQty, entity.F_GoodNotQty);
            AddChange("【开始时间】", oldEntity.F_StartTime, entity.F_StartTime);
            AddChange("【结束时间】", oldEntity.F_EndTime, entity.F_EndTime);
            AddChange("【工资单价】", oldEntity.F_WagePrice, entity.F_WagePrice);
            AddChange("【备注】", oldEntity.F_Description, entity.F_Description);

            if (changeList.Any())
            {

                var logEntity = new AProdReportLogEntity();
                logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                logEntity.F_CreatorUserId = _userManager.UserId;
                logEntity.id = SnowflakeIdHelper.NextId();
                logEntity.F_ReportId = entity.id;
                logEntity.F_Title = "修改报工";
                logEntity.F_Content = "修改内容：" + string.Join("；", changeList);
                await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
            }

            if (aProdReportDefectEntityList != null)
            {
                foreach (var item in aProdReportDefectEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_ReportId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorTime = null;
                        item.F_ReportId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }



            //如果工单总合格数量大于计划数，则工单状态自动变为已完成
            //var taskNumEntity = await _repositoryTask.GetFirstAsync(it => it.id == entity.F_TaskId);
            //if (taskNumEntity != null)
            //{
            //    var outsourceAcceptNum = (await _repository.AsQueryable().Where(it => it.F_TaskId == entity.F_TaskId && it.DeleteMark == null).SumAsync(it => it.F_GoodQty)) ?? 0;
            //    if (outsourceAcceptNum >= taskNumEntity.F_ProdQty)
            //    {
            //        taskNumEntity.F_TaskStatus = "2";
            //        await _repositoryTask.AsUpdateable(taskNumEntity)
            //         .UpdateColumns(it => new {
            //             it.F_TaskStatus,
            //         })
            //         .ExecuteCommandAsync();
            //    }
            //    else
            //    {
            //        taskNumEntity.F_TaskStatus = "1";
            //        await _repositoryTask.AsUpdateable(taskNumEntity)
            //         .UpdateColumns(it => new {
            //             it.F_TaskStatus,
            //         })
            //         .ExecuteCommandAsync();
            //    }
            //}
        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 结算.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("SettleUpdate/{id}")]
    [UnitOfWork]
    public async Task SettleUpdate(string id, [FromBody] AProdReportUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        var entity = input.Adapt<AProdReportEntity>();
        entity.id = id;
        entity.F_SettleTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_SettleUserId = _userManager.UserId;
        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_SettleStatus,
                    it.F_SettleTime,
                    it.F_SettleUserId,
                })
                .ExecuteCommandAsync();
        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }

        if (oldEntity.F_SettleStatus != entity.F_SettleStatus) 
        {
            var logEntity = new AProdReportLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_ReportId = entity.id;
            logEntity.F_Title = "修改报工结算状态";
            logEntity.F_Content = "报工结算状态修改为：" + await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(entity.F_SettleStatus) && it.DictionaryTypeId.Equals("2014214471169478656")).Select(it => it.FullName).FirstAsync() + "，修改备注：" + input.F_DescriptionXin;
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }
    }


    /// <summary>
    /// 删除a_prod_report.
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
    /// 批量删除a_prod_report.
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
            // 批量删除a_prod_report
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_prod_report详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.id == id)
            .Select(it => new AProdReportDetailOutput
            {
                id = it.id,
                F_ProdUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_ProdUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_GoodQty = it.F_GoodQty.ToString(),
                F_GoodNotQty = it.F_GoodNotQty.ToString(),
                F_StartTime = it.F_StartTime.Value.ToString("yyyy-MM-dd"),
                F_EndTime = it.F_EndTime.Value.ToString("yyyy-MM-dd"),
                F_Files = it.F_Files,
                F_SettleStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_SettleStatus) && dic.DictionaryTypeId.Equals("2014214471169478656")).Select(dic => dic.FullName),
                F_SettleUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_SettleUserId)).Select(u => u.RealName),
                F_SettleTime = it.F_SettleTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_WagePrice = it.F_WagePrice,
                F_State = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_State) && dic.DictionaryTypeId.Equals("2014253420491444224")).Select(dic => dic.FullName),
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 生产人员
            //if(item.F_ProdUserId != null)
            //{
            //    var F_ProdUserIdUserSelect = item.F_ProdUserId.ToObject<List<string>>();
            //    item.F_ProdUserId = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_ProdUserIdUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            //}

            if(item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }
        });

        return data.FirstOrDefault();
    }

    #region 大屏

    /// <summary>
    /// 生产中控大屏-不良品项分布.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("SCZK_BLPXFB")]
    public async Task<dynamic> GetSCZK_BLPXFB()
    {
        var data = await _repositoryProdDefect.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
             .Select(it => new AProdDefectListOutput
             {
                 id = it.id,
                 F_DefectName = it.F_DefectName,
                 F_AllNum = 0,
                 F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
             }).MergeTable().OrderBy(it => it.F_CreatorTime, OrderByType.Desc).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            item.F_AllNum = (await _repositoryDefect.AsQueryable().Where(it => it.DeleteMark == null && it.F_DefectId == item.id).SumAsync(it => it.F_Num)) ?? 0;
        });

        return new {
            data = data
        };
    }

    /// <summary>
    /// 生产中控大屏-人员不良品率.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("SCZK_RYBLPL")]
    public async Task<dynamic> GetSCZK_RYBLPL()
    {
        // 1. 查询原始报工数据（保留原始ID，不转换姓名）
        var rawData = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null && it.FlowId == null)
            .Where(it => !SqlFunc.IsNullOrEmpty(it.F_ProdUserId))
            .Select(it => new {
                it.F_ProdUserId,    // 原始JSON ID列表
                it.F_GoodNotQty,
                it.F_GoodQty
            })
            .ToListAsync();

        // 2. 展开并统计（基于ID，避免ThenMapper修改原始数据）
        var personStats = rawData
            .SelectMany(item => {
                // 解析JSON ID列表
                var userIds = item.F_ProdUserId.ToObject<List<string>>();
                return userIds.Select(id => new {
                    UserId = id,
                    BadQty = item.F_GoodNotQty ?? 0,
                    GoodQty = item.F_GoodQty ?? 0
                });
            })
            .GroupBy(x => x.UserId)
            .Select(g => new {
                UserId = g.Key,
                TotalBadQty = g.Sum(x => x.BadQty),
                TotalGoodQty = g.Sum(x => x.GoodQty),
                TotalQty = g.Sum(x => x.BadQty + x.GoodQty),
                BadRate = g.Sum(x => x.BadQty + x.GoodQty) == 0 ? 0
                    : Math.Round((decimal)g.Sum(x => x.BadQty) / g.Sum(x => x.BadQty + x.GoodQty) * 100, 2)
            })
            .OrderByDescending(x => x.BadRate)
            .Take(5)
            .ToList();

        // 3. 补充用户姓名（一次性查询）
        var userIds = personStats.Select(p => p.UserId).ToList();

        var users = _repositoryUser.AsQueryable()
            .Where(it => it.DeleteMark == null && userIds.Contains(it.Id))
            .Select(it => new {
                it.Id,
                UserName = SqlFunc.MergeString(it.RealName, "/", it.Account) ?? "未知用户"
            })
            .ToList()
            .ToDictionary(u => u.Id, u => u.UserName);

        // 3. 在内存中关联数据
        var result = personStats.Select(p => new SCZK_RYBLPLOutput
        {
            UserId = p.UserId,
            UserName = users.TryGetValue(p.UserId, out var name) ? name : "未知用户",
            TotalBadQty = p.TotalBadQty,
            TotalGoodQty = p.TotalGoodQty,
            BadRate = p.BadRate
        }).ToList();

        return new { data = result };
    }


    /// <summary>
    /// 生产中控大屏-实时报工数据.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("SCZK_SSBGSJ")]
    public async Task<dynamic> GetSCZK_SSBGSJ()
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .RightJoin<AProdTaskEntity>((it, ts) => ts.id.Equals(it.F_TaskId) && ts.F_Type == "approver")
            .RightJoin<AProdProcessingEntity>((it, ts, ps) => ps.id.Equals(ts.F_ProcessingId) && ps.DeleteMark == null)
            .LeftJoin<AProdProcessEntity>((it, ts, ps, pr) => pr.id.Equals(ts.F_ProcessId) && pr.DeleteMark == null)
            .Select((it, ts, ps, pr) => new AProdReportListOutput
            {
                id = it.id,
                F_GoodQty = it.F_GoodQty,
                F_GoodNotQty = it.F_GoodNotQty,
                F_Num = (it.F_GoodQty ?? 0) + (it.F_GoodNotQty ?? 0),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("MM-dd HH:mm:ss"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => u.RealName),

                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == ps.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == ps.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_ProcessName = pr.F_ProcessName,
            }).MergeTable().OrderBy(it => it.F_CreatorTime, OrderByType.Desc).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

        });

        return new {
            data = data
        };
    }

    /// <summary>
    /// 生产中控大屏-员工产能趋势.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("SCZK_YGCNQS")]
    public async Task<dynamic> GetSCZK_YGCNQS()
    {
        // 1. 生成近30天日期范围（转为字符串 "yyyy-MM-dd" 用于匹配）
        var endDate = DateTime.Now.Date;
        var startDate = endDate.AddDays(-29);
        var dateRange = Enumerable.Range(0, 30)
            .Select(i => startDate.AddDays(i).ToString("yyyy-MM-dd"))  // 关键：转为字符串！
            .ToList();

        // 2. 查询数据库
        var dbData = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null
                && it.F_CreatorUserId == _userManager.UserId
                && it.FlowId == null)
            .Where(it => it.F_CreatorTime >= startDate && it.F_CreatorTime < endDate.AddDays(1))
            .GroupBy(it => SqlFunc.MappingColumn<string>("DATE_FORMAT(F_CreatorTime, '%Y-%m-%d')"))
            .Select(it => new {
                DateStr = SqlFunc.MappingColumn<string>("DATE_FORMAT(F_CreatorTime, '%Y-%m-%d')"),
                TotalNum = SqlFunc.AggregateSum(SqlFunc.IsNull(it.F_GoodQty, 0) + SqlFunc.IsNull(it.F_GoodNotQty, 0))
            })
            .ToListAsync();

        // 3. 合并数据（补全无数据的日期为0）
        var result = dateRange.Select(dateStr =>  // dateStr 现在是 "yyyy-MM-dd" 格式
        {
            var dbItem = dbData.FirstOrDefault(d => d.DateStr == dateStr);
            return new {
                Date = DateTime.ParseExact(dateStr, "yyyy-MM-dd", null).ToString("MM-dd"),  // 转为 "01-30"
                Value = dbItem?.TotalNum ?? 0
            };
        }).ToList();

        // 在 result 生成后，如果需要用户详细信息
        var finalResult = result.Select(item => new
        {
            item.Date,
            item.Value,
            UserName = _userManager.RealName,
            UserId = _userManager.UserId
        }).ToList();

        return new {
            data = finalResult
        };
    }

    /// <summary>
    /// 生产中控大屏-人员生产统计.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("SCZK_RYSCTJ")]
    public async Task<dynamic> GetRYSCTJ()
    {
        var now = DateTime.Now;
        var todayStart = now.Date;
        var todayEnd = todayStart.AddDays(1);
        var weekStart = now.AddDays(-((int)now.DayOfWeek - 1 + 7) % 7).Date; // 本周一
        var weekEnd = weekStart.AddDays(7);
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var monthEnd = monthStart.AddMonths(1);

        // 【第1次查询】一次性取出本月所有报工数据（内存中分组统计）
        var allRecords = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Where(it => it.F_CreatorTime >= monthStart && it.F_CreatorTime < monthEnd) // 只查本月（覆盖今日、本周）
            .Select(it => new {
                UserId = it.F_CreatorUserId,
                Qty = SqlFunc.IsNull(it.F_GoodQty, 0) + SqlFunc.IsNull(it.F_GoodNotQty, 0),
                Time = it.F_CreatorTime.Value
            })
            .ToListAsync();

        // 内存中按用户聚合（避免循环查询数据库）
        var statsDict = allRecords
            .GroupBy(x => x.UserId)
            .ToDictionary(
                g => g.Key,
                g => new {
                    DayNum = g.Where(x => x.Time >= todayStart && x.Time < todayEnd).Sum(x => x.Qty),
                    WeekNum = g.Where(x => x.Time >= weekStart && x.Time < weekEnd).Sum(x => x.Qty),
                    MonthNum = g.Sum(x => x.Qty)
                }
            );

        // 【第2次查询】获取用户列表
        var userList = await _repositoryUser.AsQueryable()
            .Where(it => it.DeleteMark == null && it.EnabledMark == 1 && it.Account != "admin" && it.Account != "System")
            .Select(it => new SCZK_RYSCTJOutput
            {
                id = it.Id,
                RealName = it.RealName,
                DayNum = 0,
                WeekNum = 0,
                MonthNum = 0,
            })
            .ToListAsync();

        // 合并统计数据
        foreach (var item in userList)
        {
            if (statsDict.TryGetValue(item.id, out var stat))
            {
                item.DayNum = stat.DayNum;
                item.WeekNum = stat.WeekNum;
                item.MonthNum = stat.MonthNum;
            }
        }

        return new { data = userList };
    }

    /// <summary>
    /// 生产中控大屏-机台状态详情.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("SCZK_JTZTXQ")]
    public async Task<dynamic> GetSCZK_JTZTXQ()
    {
        // 1. 获取机器基础信息
        var machineList = await _repositoryMachine.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Select(it => new SCZK_JTZTXQOutput
            {
                id = it.id,
                F_MachineName = it.F_MachineName,
                F_MachineCode = it.F_MachineCode,
                F_MachineStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_MachineStatus) && dic.DictionaryTypeId.Equals("2011620395194650624")).Select(dic => dic.FullName),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToListAsync();

        if (machineList.Any())
        {
            var machineIds = machineList.Select(m => m.id).ToList();

            // 1. 查出所有未删除的工序（不过滤机器ID）
            var allProcess = await _repositoryProcess.AsQueryable()
                .Where(it => it.DeleteMark == null)
                .Select(it => new { it.id, it.F_MachineId })
                .ToListAsync();

            // 2. 内存中过滤：解析逗号分隔并匹配
            var processToMachine = allProcess
             .SelectMany(p =>
             {
                 // 先创建一个强类型的列表
                 var result = new List<(string ProcessId, string MachineId)>();

                 try
                 {
                     var ids = JsonSerializer.Deserialize<List<string>>(p.F_MachineId);
                     if (ids != null)
                     {
                         result.AddRange(ids.Select(mid => (p.id, mid)));
                     }
                 }
                 catch
                 {
                     var manualIds = (p.F_MachineId ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(mid => mid.Trim().Trim('"', ' ', '\r', '\n', '[', ']'));
                     result.AddRange(manualIds.Select(mid => (p.id, mid)));
                 }

                 return result;
             })
             .Where(x => machineIds.Contains(x.MachineId))  // 现在 x 是 ValueTuple
             .ToList();

            // 使用方式略有不同
            var processIds = processToMachine.Select(p => p.ProcessId).Distinct().ToList();
            var processMachineDict = processToMachine
                .GroupBy(x => x.ProcessId)
                .ToDictionary(g => g.Key, g => g.First().MachineId);


            // 查询任务并映射机器ID
            var tasks = await _repositoryTask.AsQueryable()
                .Where(it => processIds.Contains(it.F_ProcessId))
                .Select(it => new { it.id, it.F_ProcessId })
                .ToListAsync();

            var taskMapping = tasks.Select(t => new {
                MachineId = processMachineDict[t.F_ProcessId],
                TaskId = t.id
            }).ToList();
            var allTaskIds = taskMapping.Select(t => t.TaskId).Distinct().ToList();

            if (allTaskIds.Any())
            {
                // 3. 一次性统计所有任务的产量（1次查询）
                var reportStats = await _repository.AsQueryable()
                    .Where(r => allTaskIds.Contains(r.F_TaskId))
                    .GroupBy(r => r.F_TaskId)
                    .Select(r => new {
                        TaskId = r.F_TaskId,
                        GoodQty = SqlFunc.AggregateSum(SqlFunc.IsNull(r.F_GoodQty, 0)),
                        BadQty = SqlFunc.AggregateSum(SqlFunc.IsNull(r.F_GoodNotQty, 0))
                    })
                    .ToListAsync();

                // 4. 内存中汇总到机器级别
                var statByMachine = taskMapping
                    .GroupJoin(reportStats, t => t.TaskId, s => s.TaskId, (t, sList) => new { t.MachineId, Stats = sList.FirstOrDefault() })
                    .Where(x => x.Stats != null)
                    .GroupBy(x => x.MachineId)
                    .Select(g => new {
                        MachineId = g.Key,
                        TotalGood = g.Sum(x => (decimal)x.Stats.GoodQty),
                        TotalBad = g.Sum(x => (decimal)x.Stats.BadQty)
                    })
                    .ToList();

                var statDict = statByMachine.ToDictionary(x => x.MachineId);

                // 5. 回填数据并计算不良率
                foreach (var item in machineList)
                {
                    if (statDict.TryGetValue(item.id, out var stat))
                    {
                        item.TotalGoodQty = stat.TotalGood;
                        item.TotalBadQty = stat.TotalBad;
                        var total = stat.TotalGood + stat.TotalBad;
                        item.BadRate = total == 0 ? 0 : Math.Round(stat.TotalBad / total * 100, 2);
                    }
                }
            }
        }

        return new { data = machineList };
    }

    #endregion

    #region APP

    /// <summary>
    /// 获取a_prod_report列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("APPYGList")]
    public async Task<dynamic> GetAPPYGList([FromBody] AProdReportListQueryInput input)
    {
        var data = await _repository.AsQueryable()
            //.Where(it => it.DeleteMark == null && it.F_TaskId == input.F_TaskId && it.F_ProdUserId.Contains(input.F_ProdUserId))
            .Where(it => it.DeleteMark == null && it.F_TaskId == input.F_TaskId)
            .Select((it) => new AProdReportAPPListOutput
            {
                id = it.id,
                F_ProdUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_ProdUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_GoodQty = it.F_GoodQty,
                F_GoodNotQty = it.F_GoodNotQty,
                F_Num = (it.F_GoodQty ?? 0) + (it.F_GoodNotQty ?? 0),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 生产人员
            //if (item.F_ProdUserId != null)
            //{
            //    var F_ProdUserIdUserSelect = item.F_ProdUserId.ToObject<List<string>>();
            //    item.F_ProdUserId = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => F_ProdUserIdUserSelect.Contains(it.Id)).Select(it => it.RealName).ToListAsync());
            //}

        });
        return PageResult<AProdReportAPPListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_prod_report.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("APPYGCreate")]
    [UnitOfWork]
    public async Task<dynamic> APPYGCreate([FromBody] AProdReportCrInput input)
    {
        var entity = new AProdReportEntity();
        entity.id = SnowflakeIdHelper.NextId();
        entity.F_GoodNotQty = input.F_GoodNotQty;
        entity.F_GoodQty = input.F_GoodQty;
        entity.F_TaskId = input.F_TaskId;

        var taskEntity = await _repositoryTask.GetFirstAsync(it => it.id == entity.F_TaskId);
        var processingEntity = await _repositoryProcessing.GetFirstAsync(it => it.id == taskEntity.F_ProcessingId);
        if (processingEntity.F_State != "1")
        {
            return new {
                code = 201,
                info = "编号为【" + processingEntity.F_ProcessNo + "】的加工单不是生产中状态不可报工"
            };
        }

        //entity.F_ProdUserId = $"[\"{input.F_ProdAPPUserId}\"]";

        entity.F_ProdUserId = input.F_ProdAPPUserId;
        entity.F_StartTime = DateTime.Today;
        entity.F_EndTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_State = "0";
        entity.F_SettleStatus = "0";
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorUserId = input.F_ProdAPPUserId;
        entity.F_CreatorOrganizeId = (_repositoryUser.GetFirst(it => it.Id == input.F_ProdAPPUserId && it.DeleteMark == null))?.OrganizeId;

        await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();

        //日志
        var logEntity = new AProdReportLogEntity();
        logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        logEntity.F_CreatorUserId = input.F_ProdAPPUserId;
        logEntity.id = SnowflakeIdHelper.NextId();
        logEntity.F_ReportId = entity.id;
        logEntity.F_Title = "新增报工";
        logEntity.F_Content = "新增报工信息";
        await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();

        //如果工单总合格数量大于计划数，则工单状态自动变为已完成
        var taskNumEntity = await _repositoryTask.GetFirstAsync(it => it.id == entity.F_TaskId);
        if (taskNumEntity != null)
        {
            var outsourceAcceptNum = (await _repository.AsQueryable().Where(it => it.F_TaskId == entity.F_TaskId && it.DeleteMark == null).SumAsync(it => it.F_GoodQty)) ?? 0;
            if (outsourceAcceptNum >= taskNumEntity.F_ProdQty)
            {
                taskNumEntity.F_TaskStatus = "2";
                await _repositoryTask.AsUpdateable(taskNumEntity)
                 .UpdateColumns(it => new {
                     it.F_TaskStatus,
                 })
                 .ExecuteCommandAsync();
            }
        }

        return new {
            code = 200,
            info = "报工成功"
        };
    }


    #endregion


}

/// 生产中控大屏-机台状态详情.
public class SCZK_JTZTXQOutput
{
    public string id { get; set; }
    public string F_CreatorTime { get; set; }
    public string F_MachineName { get; set; }
    public string F_MachineCode { get; set; }
    public string F_MachineStatus { get; set; }

    public decimal TotalBadQty { get; set; }
    public decimal TotalGoodQty { get; set; }
    public decimal BadRate { get; set; }
}

/// 生产中控大屏-人员生产统计.
public class SCZK_RYSCTJOutput
{
    public string id { get; set; }
    public string RealName { get; set; }
    public int? DayNum { get; set; }
    public int? WeekNum { get; set; }
    public int? MonthNum { get; set; }
}

/// 生产中控大屏-人员不良品率.
public class SCZK_RYBLPLOutput
{
    public string UserId { get; set; }          // 用户ID（如 001）
    public string UserName { get; set; }        // 用户名（如 超级管理员）

    public decimal TotalBadQty { get; set; }    // 总不良数
    public decimal TotalGoodQty { get; set; }   // 总合格数
    public decimal BadRate { get; set; }        // 不良率（%）
}