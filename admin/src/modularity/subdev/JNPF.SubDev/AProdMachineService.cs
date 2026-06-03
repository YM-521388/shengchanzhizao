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
using JNPF.example.Entitys.Dto.AProdMachine;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.DatabaseAccessor;
using JNPF.Common.Dtos;
using JNPF.example.Entitys.Dto.AProdProcess;
using Aop.Api.Domain;

namespace JNPF.example;

/// <summary>
/// 业务实现：机台管理.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdMachine", Order = 200)]
[Route("api/example/[controller]")]
public class AProdMachineService : IAProdMachineService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdMachineEntity> _repository;

    private readonly ISqlSugarRepository<AProdProcessEntity> _repositoryProcess;

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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"机台编号\",\"field\":\"F_MachineCode\"},{\"value\":\"机台名称\",\"field\":\"F_MachineName\"},{\"value\":\"机台规格\",\"field\":\"F_MachineSpec\"},{\"value\":\"机台状态\",\"field\":\"F_MachineStatus\"},{\"value\":\"机台类别\",\"field\":\"F_MachineType\"},{\"value\":\"单日运行时长\",\"field\":\"F_DailyRunHours\"},{\"value\":\"标准产出\",\"field\":\"F_StdOutput\"},{\"value\":\"车间\",\"field\":\"F_WorkshopId\"},{\"value\":\"产线\",\"field\":\"F_LineId\"},{\"value\":\"启用状态\",\"field\":\"F_State\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdMachineService"/>类型的新实例.
    /// </summary>
    public AProdMachineService(
        ISqlSugarRepository<AProdProcessEntity> repositoryProcess,
        ISqlSugarRepository<AProdMachineEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryProcess = repositoryProcess;
        _repository = repository;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取机台管理.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new AProdMachineEntity
            {
                id = it.id,
                F_MachineCode = it.F_MachineCode,
                F_MachineName = it.F_MachineName,
                F_MachineSpec = it.F_MachineSpec,
                F_MachineStatus = it.F_MachineStatus,
                F_Image = it.F_Image,
                F_MachineType = it.F_MachineType,
                F_WorkshopId = it.F_WorkshopId,
                F_LineId = it.F_LineId,
                F_DailyRunHours = it.F_DailyRunHours,
                F_StdHour = it.F_StdHour,
                F_StdMinute = it.F_StdMinute,
                F_StdSecond = it.F_StdSecond,
                F_StdOutput = it.F_StdOutput,
                F_State = it.F_State,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AProdMachineInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取机台管理列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdMachineListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AProdMachineListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var F_MachineType = input.F_MachineType?.Last();
        var F_WorkshopId = input.F_WorkshopId?.Last();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_MachineCode), it => it.F_MachineCode.Contains(input.F_MachineCode))
            .WhereIF(!string.IsNullOrEmpty(input.F_MachineName), it => it.F_MachineName.Contains(input.F_MachineName))
            .WhereIF(!string.IsNullOrEmpty(input.F_MachineStatus), it => it.F_MachineStatus.Equals(input.F_MachineStatus))
            .WhereIF(!string.IsNullOrEmpty(input.F_MachineType?.ToString()), it => it.F_MachineType.Contains(F_MachineType))
            .WhereIF(!string.IsNullOrEmpty(input.F_WorkshopId?.ToString()), it => it.F_WorkshopId.Contains(F_WorkshopId))
            .WhereIF(!string.IsNullOrEmpty(input.F_LineId), it => it.F_LineId.Equals(input.F_LineId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new AProdMachineListOutput
            {
                id = it.id,
                F_MachineCode = it.F_MachineCode,
                F_MachineName = it.F_MachineName,
                F_MachineSpec = it.F_MachineSpec,
                F_MachineStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_MachineStatus) && dic.DictionaryTypeId.Equals("2011620395194650624")).Select(dic => dic.FullName),
                F_MachineType = it.F_MachineType,
                F_WorkshopId = it.F_WorkshopId,
                F_LineId = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_LineId) && dic.DictionaryTypeId.Equals("2011623836151320576")).Select(dic => dic.FullName),
                F_DailyRunHours = it.F_DailyRunHours.ToString(),
                F_StdOutput = it.F_StdOutput.ToString(),
                F_State = it.F_State,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            var F_MachineTypeData = await _dataInterfaceService.GetDynamicList("F_MachineType", "2011621811959238656", "F_Id", "F_Name", "children");
            if (item.F_MachineType != null)
            {
                var F_MachineTypeCascader = item.F_MachineType.ToObject<List<string>>();

                item.F_MachineType = string.Join("/", F_MachineTypeData.FindAll(it => F_MachineTypeCascader.Contains(it.id)).Select(s => s.fullName));
            }

            var F_WorkshopIdData = await _dataInterfaceService.GetDynamicList("F_WorkshopId", "2011621894347952128", "F_Id", "F_Name", "children");
            if (item.F_WorkshopId != null)
            {
                var F_WorkshopIdCascader = item.F_WorkshopId.ToObject<List<string>>();
                item.F_WorkshopId = string.Join("/", F_WorkshopIdData.FindAll(it => F_WorkshopIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

        });

        return PageResult<AProdMachineListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建机台管理.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AProdMachineCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdMachineEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        if (await _repository.IsAnyAsync(it => it.F_MachineCode.Equals(input.F_MachineCode) && it.FlowId == null && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "机台编号");
        if (await _repository.IsAnyAsync(it => it.F_MachineName.Equals(input.F_MachineName) && it.FlowId==null   && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "机台名称");

        // 自动生成编号逻辑：前缀 D + yyyyMMdd，后缀 3 位序号
        if (string.IsNullOrEmpty(entity.F_MachineCode))
        {
            var prefix = "D" + DateTime.Now.ToString("yyyyMMdd");

            // 查询数据库中已有相同前缀的编号
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_MachineCode != null && it.F_MachineCode.StartsWith(prefix))
                .Select(it => it.F_MachineCode)
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
            entity.F_MachineCode = prefix + nextSeq.ToString("D3");
        }

        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 更新机台管理.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AProdMachineUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdMachineEntity>();
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_MachineName.Equals(input.F_MachineName) && it.FlowId == null && it.DeleteMark == null && !it.id.Equals(id)))
            throw Oops.Bah(ErrorCode.COM1023, "机台名称");
        if (await _repository.IsAnyAsync(it => it.F_MachineCode.Equals(input.F_MachineCode) && it.FlowId == null && it.DeleteMark == null && !it.id.Equals(id)))
            throw Oops.Bah(ErrorCode.COM1023, "机台编号");

        if (string.IsNullOrEmpty(entity.F_MachineCode))
        {
            entity.F_MachineCode = oldEntity.F_MachineCode;
        }
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_MachineCode,
                it.F_MachineName,
                it.F_MachineSpec,
                it.F_MachineStatus,
                it.F_Image,
                it.F_MachineType,
                it.F_WorkshopId,
                it.F_LineId,
                it.F_DailyRunHours,
                it.F_StdHour,
                it.F_StdMinute,
                it.F_StdSecond,
                it.F_StdOutput,
                it.F_State,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除机台管理.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除机台管理.
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
            // 批量删除机台管理
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 机台管理详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AProdMachineDetailOutput
            {
                id = it.id,
                F_MachineCode = it.F_MachineCode,
                F_MachineName = it.F_MachineName,
                F_MachineSpec = it.F_MachineSpec,
                F_MachineStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_MachineStatus) && dic.DictionaryTypeId.Equals("2011620395194650624")).Select(dic => dic.FullName),
                F_Image = it.F_Image,
                F_MachineType = it.F_MachineType,
                F_WorkshopId = it.F_WorkshopId,
                F_LineId = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_LineId) && dic.DictionaryTypeId.Equals("2011623836151320576")).Select(dic => dic.FullName),
                F_DailyRunHours = it.F_DailyRunHours.ToString(),
                F_StdHour = it.F_StdHour.ToString(),
                F_StdMinute = it.F_StdMinute.ToString(),
                F_StdSecond = it.F_StdSecond.ToString(),
                F_StdOutput = it.F_StdOutput.ToString(),
                F_State = SqlFunc.IIF(SqlFunc.ToInt32(it.F_State) == 1, "启用", "禁用"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            if(item.F_Image != null)
            {
                item.F_Image = item.F_Image.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Image = new List<FileControlsModel>();
            }

            var F_MachineTypeData = await _dataInterfaceService.GetDynamicList("F_MachineType", "2011621811959238656", "F_Id", "F_Name", "children");
            if (item.F_MachineType != null)
            {
                var F_MachineTypeCascader = item.F_MachineType.ToObject<List<string>>();

                item.F_MachineType = string.Join("/", F_MachineTypeData.FindAll(it => F_MachineTypeCascader.Contains(it.id)).Select(s => s.fullName));
            }

            var F_WorkshopIdData = await _dataInterfaceService.GetDynamicList("F_WorkshopId", "2011621894347952128", "F_Id", "F_Name", "children");
            if (item.F_WorkshopId != null)
            {
                var F_WorkshopIdCascader = item.F_WorkshopId.ToObject<List<string>>();
                item.F_WorkshopId = string.Join("/", F_WorkshopIdData.FindAll(it => F_WorkshopIdCascader.Contains(it.id)).Select(s => s.fullName));
            }


        });

        data[0].ProcessList = await  GetProcessList(id);
        return data.FirstOrDefault();
    }



    /// <summary>
    /// 弹窗获取机台列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("PopupList")]
    public async Task<dynamic> GetPopupList()
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_State == "1")
            .Where(it => it.FlowId == null)
            .Select(it => new AProdMachineListOutput
            {
                id = it.id,
                F_MachineName = it.F_MachineCode + "-" +it.F_MachineName,
                F_MachineSpec = it.F_MachineSpec,
                F_MachineStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_MachineStatus) && dic.DictionaryTypeId.Equals("2011620395194650624")).Select(dic => dic.FullName),
                F_MachineType = it.F_MachineType,
                F_WorkshopId = it.F_WorkshopId,
                F_LineId = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_LineId) && dic.DictionaryTypeId.Equals("2011623836151320576")).Select(dic => dic.FullName),
                F_DailyRunHours = it.F_DailyRunHours.ToString(),
                F_StdOutput = it.F_StdOutput.ToString(),
                F_State = it.F_State,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderBy(it => it.F_CreatorTime, OrderByType.Desc).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 机台类别
            var F_MachineTypeData = await _dataInterfaceService.GetDynamicList("F_MachineType", "2011621811959238656", "F_Id", "F_Name", "children");
            if (item.F_MachineType != null)
            {
                var F_MachineTypeCascader = item.F_MachineType.ToObject<List<string>>();

                item.F_MachineType = string.Join("/", F_MachineTypeData.FindAll(it => F_MachineTypeCascader.Contains(it.id)).Select(s => s.fullName));
            }

            var F_WorkshopIdData = await _dataInterfaceService.GetDynamicList("F_WorkshopId", "2011621894347952128", "F_Id", "F_Name", "children");
            if (item.F_WorkshopId != null)
            {
                var F_WorkshopIdCascader = item.F_WorkshopId.ToObject<List<string>>();
                item.F_WorkshopId = string.Join("/", F_WorkshopIdData.FindAll(it => F_WorkshopIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

        });

        return new {
            data = data
        };
    }


    /// <summary>
    /// 获取机台下工序信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    public async Task<dynamic> GetProcessList(string machineId)
    {
        var data = await _repositoryProcess.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_MachineId.Contains(machineId))
            .Where(it => it.FlowId == null)
            .Select(it => new AProdProcessListOutput
            {
                id = it.id,
                F_ProcessName = it.F_ProcessName,
                F_ProcessCode = it.F_ProcessCode,
                F_WorkshopId = it.F_WorkshopId,
                F_PriceType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_PriceType) && dic.DictionaryTypeId.Equals("2011642610875240448")).Select(dic => dic.FullName),
                F_Line = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Line) && dic.DictionaryTypeId.Equals("2011623836151320576")).Select(dic => dic.FullName),
                F_ReportUnit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_ReportUnit) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                F_UnitRatio = "生产1个计划数， 需要" + (it.F_UnitRatio.ToString() ?? "0"),
                F_ProdUserIds = it.F_ProdUserIds,
                F_WagePrice = it.F_WagePrice.ToString() + "元/小时",
                F_DefectIds = it.F_DefectIds,
                F_QcUserIds = it.F_QcUserIds,
                F_StdHour = (it.F_StdHour.ToString() ?? "0") + "小时" + (it.F_StdMinute.ToString() ?? "0") + "分钟" + (it.F_StdSecond.ToString() ?? "0") + "秒",
                F_State = it.F_State,
                F_MachineId = it.F_MachineId,
                F_IsOutsource = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsOutsource) == 1, "启用", "禁用"),
                F_IsQc = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsQc) == 1, "启用", "禁用"),
                F_DefectHandle = SqlFunc.IIF(SqlFunc.ToInt32(it.F_DefectHandle) == 1, "启用", "禁用"),
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderBy( it => it.F_CreatorTime, OrderByType.Desc).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            var F_WorkshopIdData = await _dataInterfaceService.GetDynamicList("F_WorkshopId", "2011621894347952128", "F_Id", "F_Name", "children");
            if (item.F_WorkshopId != null)
            {
                var F_WorkshopIdCascader = item.F_WorkshopId.ToObject<List<string>>();
                item.F_WorkshopId = string.Join("/", F_WorkshopIdData.FindAll(it => F_WorkshopIdCascader.Contains(it.id)).Select(s => s.fullName));
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

            // 机台
            if (item.F_MachineId != null)
            {
                var F_MachineIdData = await _dataInterfaceService.GetDynamicList("F_MachineId", "2011729284707782656", "id", "F_MachineName", "");
                var F_MachineIdSelect = item.F_MachineId.ToObject<string[]>();
                item.F_MachineId = string.Join(",", F_MachineIdData.FindAll(it => F_MachineIdSelect.Contains(it.id)).Select(s => s.fullName));
            }
        });

        return data;
    }

}
