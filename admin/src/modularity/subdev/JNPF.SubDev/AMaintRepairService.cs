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
using JNPF.VisualDev.Engine;
using JNPF.example.Entitys.Dto.AMaintRepair;
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
/// 业务实现：维修工单.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AMaintRepair", Order = 200)]
[Route("api/example/[controller]")]
public class AMaintRepairService : IAMaintRepairService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AMaintRepairEntity> _repository;
    private readonly ISqlSugarRepository<AMaintRepairLogEntity> _repositoryLog;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"工单编号\",\"field\":\"F_RepairNo\"},{\"value\":\"维修设备\",\"field\":\"F_DeviceId\"},{\"value\":\"处理人\",\"field\":\"F_HandlerUserId\"},{\"value\":\"分组\",\"field\":\"F_GroupId\"},{\"value\":\"接单时间\",\"field\":\"F_ReceiveTime\"},{\"value\":\"处理时间\",\"field\":\"F_HandleTime\"},{\"value\":\"故障描述\",\"field\":\"F_Description\"},{\"value\":\"状态\",\"field\":\"F_State\"},{\"value\":\"暂停原因\",\"field\":\"F_PauseReason\"},{\"value\":\"取消原因\",\"field\":\"F_CancelReason\"},{\"value\":\"报修人\",\"field\":\"F_CreatorUserId\"},{\"value\":\"审核人\",\"field\":\"F_AuditorUserId\"},{\"value\":\"审核时间\",\"field\":\"F_AuditTime\"},{\"value\":\"是否解决问题\",\"field\":\"F_IsSolved\"},{\"value\":\"处理结果\",\"field\":\"F_HandleResult\"},{\"value\":\"报修时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 格式化时间跨度为"xx小时xx分钟xx秒"格式.
    /// </summary>
    private string FormatTimeSpan(TimeSpan timeSpan)
    {
        if (timeSpan.TotalSeconds <= 0) return string.Empty;

        var hours = (int)timeSpan.TotalHours;
        var minutes = timeSpan.Minutes;
        var seconds = timeSpan.Seconds;

        var parts = new List<string>();
        if (hours > 0) parts.Add($"{hours}小时");
        if (minutes > 0 || hours > 0) parts.Add($"{minutes}分钟");
        parts.Add($"{seconds}秒");

        return string.Join(string.Empty, parts);
    }

    /// <summary>
    /// 初始化一个<see cref="AMaintRepairService"/>类型的新实例.
    /// </summary>
    public AMaintRepairService(
        ISqlSugarRepository<AMaintRepairLogEntity> repositoryLog,
        ISqlSugarRepository<AMaintRepairEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryLog = repositoryLog;
        _repository = repository;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取维修工单.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new AMaintRepairEntity
            {
                id = it.id,
                F_RepairNo = it.F_RepairNo,
                F_DeviceId = it.F_DeviceId,
                F_GroupId = it.F_GroupId,
                F_DispatchType = it.F_DispatchType,
                F_HandlerUserId = it.F_HandlerUserId,
                F_Description = it.F_Description,
                F_ReceiveTime = it.F_ReceiveTime,
                F_HandleTime = it.F_HandleTime,
                F_State = it.F_State,
                F_PauseReason = it.F_PauseReason,
                F_CancelReason = it.F_CancelReason,
                F_AuditorUserId = it.F_AuditorUserId,
                F_AuditTime = it.F_AuditTime,
                F_IsSolved = it.F_IsSolved,
                F_HandleResult = it.F_HandleResult,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                F_CreatorOrganizeId = it.F_CreatorOrganizeId,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AMaintRepairInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取维修工单列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AMaintRepairListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AMaintRepairListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_RepairNo), it => it.F_RepairNo.Contains(input.F_RepairNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_HandlerUserId), it => it.F_HandlerUserId.Equals(input.F_HandlerUserId))
            .WhereIF(!string.IsNullOrEmpty(input.F_State), it => it.F_State.Equals(input.F_State))
            .WhereIF(!string.IsNullOrEmpty(input.F_AuditorUserId), it => it.F_AuditorUserId.Equals(input.F_AuditorUserId))
            .WhereIF(!string.IsNullOrEmpty(input.F_IsSolved), it => it.F_IsSolved.Equals(input.F_IsSolved))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new AMaintRepairListOutput
            {
                id = it.id,
                F_RepairNo = it.F_RepairNo,
                F_DeviceId = it.F_DeviceId,
                F_GroupId = SqlFunc.Subqueryable<GroupEntity>().EnableTableFilter().Where(g => g.Id.Equals(it.F_GroupId)).Select(g => g.FullName),
                F_HandlerUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_HandlerUserId)).Select(u => u.RealName),
                F_Description = it.F_Description,
                F_ReceiveTime = it.F_ReceiveTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_HandleTime = it.F_HandleTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                //F_RepairTotal = (it.F_HandleTime.HasValue && it.F_ReceiveTime.HasValue ? (it.F_HandleTime.Value - it.F_ReceiveTime.Value).TotalSeconds : 0).ToString(),
                //F_WorkOrderTotal = (it.F_HandleTime.HasValue && it.F_CreatorTime.HasValue ? (it.F_HandleTime.Value - it.F_CreatorTime.Value).TotalSeconds : 0).ToString(),
                F_State = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_State) && dic.DictionaryTypeId.Equals("2016473505696190464")).Select(dic => dic.FullName),
                F_StateCode = it.F_State,
                F_PauseReason = it.F_PauseReason,
                F_CancelReason = it.F_CancelReason,
                F_AuditorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_AuditorUserId)).Select(u => u.RealName),
                F_AuditTime = it.F_AuditTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_IsSolved = it.F_IsSolved,
                F_HandleResult = it.F_HandleResult,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime,OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 是否解决问题
            var F_IsSolvedData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"0\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_IsSolved != null)
            {
                item.F_IsSolved = F_IsSolvedData.Find(it => item.F_IsSolved.Equals(it.id))?.fullName;
            }
            if (item.F_DeviceId != null)
            {
                var F_DeviceIdData = await _dataInterfaceService.GetDynamicList("F_DeviceId", "2016476904974061568", "id", "F_DeviceName", "");
                var F_DeviceIdSelect = item.F_DeviceId.ToObject<string[]>();
                item.F_DeviceId = string.Join(",", F_DeviceIdData.FindAll(it => F_DeviceIdSelect.Contains(it.id)).Select(s => s.fullName));
            }

        });

        var resData = data.list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        data.list = resData.ToObject<List<AMaintRepairListOutput>>(CommonConst.options);

        return PageResult<AMaintRepairListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建维修工单.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AMaintRepairCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AMaintRepairEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        if (entity.F_DispatchType == "2")
        {
            entity.F_State = "0";
        }
        else 
        {
            entity.F_State = "1";
        }
        if (entity.F_DispatchType != "0")
        {
            entity.F_HandlerUserId = null;
        }

        if (await _repository.IsAnyAsync(it => it.F_RepairNo.Equals(input.F_RepairNo) && it.FlowId == null && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "工单编号");

        // 自动生成编号逻辑：前缀 yyyyMM，后缀 3 位序号
        if (string.IsNullOrEmpty(entity.F_RepairNo))
        {
            var prefix = DateTime.Now.ToString("yyyyMM");

            // 查询数据库中已有相同前缀的编号
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_RepairNo != null && it.F_RepairNo.StartsWith(prefix))
                .Select(it => it.F_RepairNo)
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
            entity.F_RepairNo = prefix + nextSeq.ToString("D3");
        }

        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);

        var logEntity = new AMaintRepairLogEntity();
        logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        logEntity.F_CreatorUserId = _userManager.UserId;
        logEntity.id = SnowflakeIdHelper.NextId();
        logEntity.F_RepairId = entity.id;
        logEntity.F_Title = "新增维修工单";
        logEntity.F_Content = "维修工单编号：" + entity.F_RepairNo + "；故障描述：" + entity.F_Description;
        await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();

    }

    /// <summary>
    /// 获取维修工单无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(AMaintRepairListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AMaintRepairListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var list = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_RepairNo), it => it.F_RepairNo.Contains(input.F_RepairNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_HandlerUserId), it => it.F_HandlerUserId.Equals(input.F_HandlerUserId))
            .WhereIF(!string.IsNullOrEmpty(input.F_State), it => it.F_State.Equals(input.F_State))
            .WhereIF(!string.IsNullOrEmpty(input.F_AuditorUserId), it => it.F_AuditorUserId.Equals(input.F_AuditorUserId))
            .WhereIF(!string.IsNullOrEmpty(input.F_IsSolved), it => it.F_IsSolved.Equals(input.F_IsSolved))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new AMaintRepairListOutput
            {
                id = it.id,
                F_RepairNo = it.F_RepairNo,
                F_DeviceId = it.F_DeviceId,
                F_GroupId = SqlFunc.Subqueryable<GroupEntity>().EnableTableFilter().Where(g => g.Id.Equals(it.F_GroupId)).Select(g => g.FullName),
                F_HandlerUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_HandlerUserId)).Select(u => u.RealName),
                F_Description = it.F_Description,
                F_ReceiveTime = it.F_ReceiveTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_HandleTime = it.F_HandleTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_State = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_State) && dic.DictionaryTypeId.Equals("2016473505696190464")).Select(dic => dic.FullName),
                F_PauseReason = it.F_PauseReason,
                F_CancelReason = it.F_CancelReason,
                F_AuditorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_AuditorUserId)).Select(u => u.RealName),
                F_AuditTime = it.F_AuditTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_IsSolved = it.F_IsSolved,
                F_HandleResult = it.F_HandleResult,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 是否解决问题
            var F_IsSolvedData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"0\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_IsSolved != null)
            {
                item.F_IsSolved = F_IsSolvedData.Find(it => item.F_IsSolved.Equals(it.id))?.fullName;
            }

            if (item.F_DeviceId != null)
            {
                var F_DeviceIdData = await _dataInterfaceService.GetDynamicList("F_DeviceId", "2016476904974061568", "id", "F_DeviceName", "");
                var F_DeviceIdSelect = item.F_DeviceId.ToObject<string[]>();
                item.F_DeviceId = string.Join(",", F_DeviceIdData.FindAll(it => F_DeviceIdSelect.Contains(it.id)).Select(s => s.fullName));
            }
        });

        var resData = list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);

        list = resData.ToObject<List<AMaintRepairListOutput>>(CommonConst.options);

        return list;
    }

    /// <summary>
    /// 导出维修工单.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] AMaintRepairListQueryInput input)
    {
        var exportData = new List<AMaintRepairListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<AMaintRepairListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName,input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新维修工单.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AMaintRepairUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        var entity = input.Adapt<AMaintRepairEntity>();

        if (await _repository.IsAnyAsync(it => it.F_RepairNo.Equals(input.F_RepairNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "工单编号");
        if (string.IsNullOrEmpty(entity.F_RepairNo))
        {
            entity.F_RepairNo = oldEntity.F_RepairNo;
        }
        if (entity.F_DispatchType != "0")
        {
            entity.F_HandlerUserId = null;
        }

        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_RepairNo,
                it.F_DeviceId,
                it.F_GroupId,
                it.F_DispatchType,
                it.F_HandlerUserId,
                it.F_Description,
                it.F_ReceiveTime,
                it.F_HandleTime,
                it.F_State,
                it.F_PauseReason,
                it.F_CancelReason,
                it.F_AuditorUserId,
                it.F_AuditTime,
                it.F_IsSolved,
                it.F_HandleResult,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);

        //增加日志
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

        AddChange("【工单编号】", oldEntity.F_RepairNo, entity.F_RepairNo);
        if (oldEntity.F_DeviceId != entity.F_DeviceId)
        {
            string oldName = "";
            string newName = "";
            var F_DeviceIdData = await _dataInterfaceService.GetDynamicList("F_DeviceId", "2016476904974061568", "id", "F_DeviceName", "");
            if (oldEntity.F_DeviceId != null)
            {
                var F_DeviceIdSelect = oldEntity.F_DeviceId.ToObject<string[]>();
                oldName = string.Join(",", F_DeviceIdData.FindAll(it => F_DeviceIdSelect.Contains(it.id)).Select(s => s.fullName));
            }
            if (entity.F_DeviceId != null)
            {
                var F_DeviceIdSelect = entity.F_DeviceId.ToObject<string[]>();
                newName = string.Join(",", F_DeviceIdData.FindAll(it => F_DeviceIdSelect.Contains(it.id)).Select(s => s.fullName));
            }
            changeList.Add($"【维修设备】 值由 {oldName} 改为 {newName}");
        }
        if (oldEntity.F_GroupId != entity.F_GroupId)
        {
            string oldName = "";
            string newName = "";
            if (oldEntity.F_GroupId != null)
            {
                oldName = await _repository.AsSugarClient().Queryable<GroupEntity>().Where(it => it.Id.Equals(oldEntity.F_GroupId)).Select(it => it.FullName).FirstAsync();
            }
            if (entity.F_GroupId != null)
            {
                newName = await _repository.AsSugarClient().Queryable<GroupEntity>().Where(it => it.Id.Equals(entity.F_GroupId)).Select(it => it.FullName).FirstAsync();
            }
            changeList.Add($"【分组】 值由 {oldName} 改为 {newName}");
        }
        if (oldEntity.F_DispatchType != entity.F_DispatchType)
        {
            string oldName = "";
            string newName = "";
            if (oldEntity.F_DispatchType != null)
            {
                oldName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(oldEntity.F_DispatchType) && it.DictionaryTypeId.Equals("2016473380785623040")).Select(it => it.FullName).FirstAsync();
            }
            if (entity.F_DispatchType != null)
            {
                newName = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(entity.F_DispatchType) && it.DictionaryTypeId.Equals("2016473380785623040")).Select(it => it.FullName).FirstAsync();
            }
            changeList.Add($"【派单方式】 值由 {oldName} 改为 {newName}");
        }
        if ((entity.F_DispatchType == "0" && oldEntity.F_HandlerUserId != entity.F_HandlerUserId))
        {
            string oldName = "";
            string newName = "";
            if (oldEntity.F_HandlerUserId != null)
            {
                oldName = await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => it.Id.Equals(oldEntity.F_HandlerUserId)).Select(it => it.RealName).FirstAsync();
            }
            if (entity.F_HandlerUserId != null)
            {
                newName = await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => it.Id.Equals(entity.F_HandlerUserId)).Select(it => it.RealName).FirstAsync();
            }
            changeList.Add($"【处理人】 值由 {oldName} 改为 {newName}");
        }
        AddChange("【故障描述】", oldEntity.F_Description, entity.F_Description);

        if (changeList.Any())
        {

            var logEntity = new AMaintRepairLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_RepairId = entity.id;
            logEntity.F_Title = "编辑维修工单";
            logEntity.F_Content = "维修工单编号：" + entity.F_RepairNo + "；修改内容：" + string.Join("；", changeList);
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }

    }


    /// <summary>
    /// 修改状态.
    /// </summary>
    [HttpGet("Check/{id}/{state}")]
    public async Task Check(string id, string state)
    {
        AMaintRepairEntity? entity = await _repository.GetFirstAsync(it => it.id == id);

        if (state == "8")
        {
            int isOk = await _repository.AsUpdateable().SetColumns(it => new AMaintRepairEntity()
            {
                F_State = "2",
            }).Where(it => it.id == id).ExecuteCommandAsync();

            var logEntity = new AMaintRepairLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_RepairId = entity.id;
            logEntity.F_Title = "继续维修工单";
            logEntity.F_Content = "继续维修工单：" + entity.F_RepairNo;
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }
        else
        {
            if (state == "2")
            {
                int isOk = await _repository.AsUpdateable().SetColumns(it => new AMaintRepairEntity()
                {
                    F_State = state,
                    F_ReceiveTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime(),
                }).Where(it => it.id == id).ExecuteCommandAsync();

                var logEntity = new AMaintRepairLogEntity();
                logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                logEntity.F_CreatorUserId = _userManager.UserId;
                logEntity.id = SnowflakeIdHelper.NextId();
                logEntity.F_RepairId = entity.id;
                logEntity.F_Title = "接单";
                logEntity.F_Content = "【"+_userManager.RealName + "】接受维修工单";
                await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
            }
            else if (state == "5")
            {

                int isOk = await _repository.AsUpdateable().SetColumns(it => new AMaintRepairEntity()
                {
                    F_State = "5",
                }).Where(it => it.id == id).ExecuteCommandAsync();

                var logEntity = new AMaintRepairLogEntity();
                logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                logEntity.F_CreatorUserId = _userManager.UserId;
                logEntity.id = SnowflakeIdHelper.NextId();
                logEntity.F_RepairId = entity.id;
                logEntity.F_Title = "完成工单";
                logEntity.F_Content = "完成维修工单";
                await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
            }
        }
    }

    /// <summary>
    /// 派单.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("ReceiveUpdate/{id}")]
    [UnitOfWork]
    public async Task ReceiveUpdate(string id, [FromBody] AMaintRepairUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        if(input.F_DispatchType != "2")
        {
            var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
            var entity = input.Adapt<AMaintRepairEntity>();
            entity.F_State = "1";
            if (entity.F_DispatchType != "0")
            {
                entity.F_HandlerUserId = null;
            }

            var isOk = 0;
            isOk = await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_State,
                    it.F_GroupId,
                    it.F_DispatchType,
                    it.F_HandlerUserId,
                }).ExecuteCommandAsync();
            if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);

            //增加日志
            var GroupName = await _repository.AsSugarClient().Queryable<GroupEntity>().Where(it => it.Id.Equals(entity.F_GroupId)).Select(it => it.FullName).FirstAsync();
            var UserName = await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => it.Id.Equals(entity.F_HandlerUserId)).Select(it => it.RealName).FirstAsync();

            var logEntity = new AMaintRepairLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_RepairId = entity.id;
            logEntity.F_Title = "维修工单派单";
            if (entity.F_DispatchType == "0")
            {
                logEntity.F_Content = "派单给：" + UserName;
            }
            if (entity.F_DispatchType == "1")
            {
                logEntity.F_Content = "派单给：" + GroupName;
            }
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }
        else
        {
            var entity = await _repository.GetFirstAsync(it => it.id == id);
            entity.F_DispatchType = "2";
            entity.F_GroupId = input.F_GroupId;

            var isOk = 0;
            isOk = await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_DispatchType,
                    it.F_GroupId
                }).ExecuteCommandAsync();
            if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 回执.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("ReceiptUpdate/{id}")]
    [UnitOfWork]
    public async Task ReceiptUpdate(string id, [FromBody] AMaintRepairUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = await _repository.GetFirstAsync(it => it.id == id);
        entity.F_IsSolved = input.F_IsSolved;
        entity.F_HandleResult = input.F_HandleResult;
        entity.F_HandleTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();

        var isOk = 0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new {
                it.F_IsSolved,
                it.F_HandleResult,
                it.F_HandleTime
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
        var changeList = new List<string>();
        string safeToStr(object v) => v == null ? string.Empty : v.ToString();

        var F_IsSolvedData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"0\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();

        var newName = F_IsSolvedData.Find(it => entity.F_IsSolved.Equals(it.id))?.fullName;

        changeList.Add($"【是否解决问题】 ：{newName}");

        changeList.Add($"【处理结果】 ：{entity.F_HandleResult}");

        if (changeList.Any())
        {
            var logEntity = new AMaintRepairLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_RepairId = entity.id;
            logEntity.F_Title = "回执";
            logEntity.F_Content = string.Join("；", changeList);
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }
    }


    /// <summary>
    /// 暂停、取消、审核原因.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("ReasonUpdate/{id}/{type}")]
    [UnitOfWork]
    public async Task ReasonUpdate(string id, string type, [FromBody] AMaintRepairUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AMaintRepairEntity>();
        entity.id = id;
        if(type == "6")
        {
            entity.F_State = "6";
            entity.F_PauseReason = input.F_Reason;
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_State,
                    it.F_PauseReason
                })
                .ExecuteCommandAsync();

            var logEntity = new AMaintRepairLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_RepairId = entity.id;
            logEntity.F_Title = "暂停维修工单";
            logEntity.F_Content = "暂停原因：" + input.F_Reason;
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }
        else if (type == "7")
        {
            entity.F_State = "7";
            entity.F_CancelReason = input.F_Reason;
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_State,
                    it.F_CancelReason
                })
                .ExecuteCommandAsync();
            var logEntity = new AMaintRepairLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_RepairId = entity.id;
            logEntity.F_Title = "取消维修工单";
            logEntity.F_Content = "取消原因：" + input.F_Reason;
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }
        else if (type == "10")
        {
            entity.F_State = "5";
            entity.F_AuditTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            entity.F_AuditorUserId = _userManager.UserId;
            entity.F_AuditReason = input.F_Reason;
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_State,
                    it.F_AuditTime,
                    it.F_AuditorUserId,
                    it.F_AuditReason
                })
                .ExecuteCommandAsync();
            var logEntity = new AMaintRepairLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_RepairId = entity.id;
            logEntity.F_Title = "审核通过";
            logEntity.F_Content = "审核备注：" + input.F_Reason;
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }
        else if (type == "4")
        {
            entity.F_State = "4";
            entity.F_AuditTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            entity.F_AuditorUserId = _userManager.UserId;
            entity.F_AuditReason = input.F_Reason;
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_State,
                    it.F_AuditTime,
                    it.F_AuditorUserId,
                    it.F_AuditReason
                })
                .ExecuteCommandAsync();
            var logEntity = new AMaintRepairLogEntity();
            logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            logEntity.F_CreatorUserId = _userManager.UserId;
            logEntity.id = SnowflakeIdHelper.NextId();
            logEntity.F_RepairId = entity.id;
            logEntity.F_Title = "审核退回";
            logEntity.F_Content = "审核备注：" + input.F_Reason;
            await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
        }
    }


    /// <summary>
    /// 删除维修工单.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除维修工单.
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
            // 批量删除维修工单
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 维修工单详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AMaintRepairDetailOutput
            {
                id = it.id,
                F_RepairNo = it.F_RepairNo,
                F_DeviceId = it.F_DeviceId,
                F_GroupId = SqlFunc.Subqueryable<GroupEntity>().EnableTableFilter().Where(g => g.Id.Equals(it.F_GroupId)).Select(g => g.FullName),
                F_DispatchType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_DispatchType) && dic.DictionaryTypeId.Equals("2016473380785623040")).Select(dic => dic.FullName),
                F_HandlerUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_HandlerUserId)).Select(u => u.RealName),
                F_Description = it.F_Description,
                F_ReceiveTime = it.F_ReceiveTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_HandleTime = it.F_HandleTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_State = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_State) && dic.DictionaryTypeId.Equals("2016473505696190464")).Select(dic => dic.FullName),
                F_PauseReason = it.F_PauseReason,
                F_CancelReason = it.F_CancelReason,
                F_AuditorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_AuditorUserId)).Select(u => u.RealName),
                F_AuditTime = it.F_AuditTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_AuditReason = it.F_AuditReason,
                F_IsSolved = it.F_IsSolved,
                F_HandleResult = it.F_HandleResult,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_CreatorOrganizeId = it.F_CreatorOrganizeId,
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 是否解决问题
            var F_IsSolvedData = "[{\"id\":\"1\",\"fullName\":\"是\"},{\"id\":\"0\",\"fullName\":\"否\"}]".ToObject<List<StaticDataModel>>();
            if(item.F_IsSolved != null)
            {
                item.F_IsSolved = F_IsSolvedData.Find(it => item.F_IsSolved.Equals(it.id))?.fullName;
            }

            if (item.F_DeviceId != null)
            {
                var F_DeviceIdData = await _dataInterfaceService.GetDynamicList("F_DeviceId", "2016476904974061568", "id", "F_DeviceName", "");
                var F_DeviceIdSelect = item.F_DeviceId.ToObject<string[]>();
                item.F_DeviceId = string.Join(",", F_DeviceIdData.FindAll(it => F_DeviceIdSelect.Contains(it.id)).Select(s => s.fullName));
            }
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();

        return resData.FirstOrDefault();
    }
}
