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
using JNPF.example.Entitys.Dto.ACtcInvoice;
using JNPF.example.Entitys.Dto.ACtcInvoiceLog;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_ctc_invoice.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "ACtcInvoice", Order = 200)]
[Route("api/example/[controller]")]
public class ACtcInvoiceService : IACtcInvoiceService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<ACtcInvoiceEntity> _repository;
    private readonly ISqlSugarRepository<ACtcContractItemEntity> _repositorysp;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"合同ID\",\"field\":\"F_ContractId\"},{\"value\":\"开票金额(元)\",\"field\":\"F_Money\"},{\"value\":\"开票状态\",\"field\":\"F_Status\"},{\"value\":\"申请附件\",\"field\":\"F_ApplyFiles\"},{\"value\":\"申请备注\",\"field\":\"F_ApplyRemark\"},{\"value\":\"开票附件\",\"field\":\"F_InvoiceFiles\"},{\"value\":\"开票备注\",\"field\":\"F_InvoiceRemark\"},{\"value\":\"申请人员\",\"field\":\"F_ApplyUserId\"},{\"value\":\"申请时间\",\"field\":\"F_ApplyTime\"},{\"value\":\"开票人员\",\"field\":\"F_InvoiceUserId\"},{\"value\":\"开票时间\",\"field\":\"F_InvoiceTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="ACtcInvoiceService"/>类型的新实例.
    /// </summary>
    public ACtcInvoiceService(
        ISqlSugarRepository<ACtcInvoiceEntity> repository,
         ISqlSugarRepository<ACtcContractItemEntity> repositorysp,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repository = repository;
        _repositorysp = repositorysp;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_ctc_invoice.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.ACtcInvoiceLogList)
            .Where(it => it.DeleteMark == null)
            .Select(it => new ACtcInvoiceEntity
            {
                id = it.id,
                F_ContractId = it.F_ContractId,
                F_Money = it.F_Money,
                F_Status = it.F_Status,
                F_ApplyFiles = it.F_ApplyFiles,
                F_ApplyRemark = it.F_ApplyRemark,
                F_InvoiceFiles = it.F_InvoiceFiles,
                F_InvoiceRemark = it.F_InvoiceRemark,
                F_ApplyUserId = it.F_ApplyUserId,
                F_ApplyTime = it.F_ApplyTime,
                F_InvoiceUserId = it.F_InvoiceUserId,
                F_InvoiceTime = it.F_InvoiceTime,
                ACtcInvoiceLogList = it.ACtcInvoiceLogList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<ACtcInvoiceInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField2cca74, async aCtcInvoiceLog =>
        {
            // 创建人员
            aCtcInvoiceLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aCtcInvoiceLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aCtcInvoiceLog.F_CreatorTime = aCtcInvoiceLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

    /// <summary>
    /// 获取a_ctc_invoice列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] ACtcInvoiceListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aCtcInvoiceLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<ACtcInvoiceListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_ctc_invoice"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_ctc_invoice_log"))) aCtcInvoiceLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_ctc_invoice_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcInvoiceEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcInvoiceLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField2cca74-", entityInfo, 1);
        List<IConditionalModel> aCtcInvoiceLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aCtcInvoiceLogConditionalModel.AddRange(aCtcInvoiceLogAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcInvoiceEntity>();
        var F_StatusDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_Status").DbColumnName;
        var F_ApplyUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_ApplyUserId").DbColumnName;
        var F_InvoiceUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_InvoiceUserId").DbColumnName;
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_StatusDbColumnName, input.F_Status))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_ApplyUserIdDbColumnName, input.F_ApplyUserId))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_InvoiceUserIdDbColumnName, input.F_InvoiceUserId))
            .WhereIF(input.F_InvoiceTime?.Count() > 0, it => SqlFunc.Between(it.F_InvoiceTime, input.F_InvoiceTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_InvoiceTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .WhereIF(aCtcInvoiceLogAuthorizeWhere?.Count > 0, it => it.ACtcInvoiceLogList.Any(aCtcInvoiceLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aCtcInvoiceLogConditionalModel.Count > 0, it => it.ACtcInvoiceLogList.Any(aCtcInvoiceLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_ApplyTime, OrderByType.Desc)
            .OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new ACtcInvoiceListOutput
            {
                id = it.id,
                F_ContractId = it.F_ContractId,
                F_Money = it.F_Money,
                F_Status = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Status) && dic.DictionaryTypeId.Equals("2039943312961572864")).Select(dic => dic.FullName),
                F_StatusInfo = it.F_Status,
                F_ApplyFiles = it.F_ApplyFiles,
                F_ApplyRemark = it.F_ApplyRemark,
                F_InvoiceFiles = it.F_InvoiceFiles,
                F_InvoiceRemark = it.F_InvoiceRemark,
                F_ApplyUserId = it.F_ApplyUserId,
                F_ApplyTime = it.F_ApplyTime.Value.ToString("yyyy-MM-dd"),
                F_InvoiceUserId = it.F_InvoiceUserId,
                F_InvoiceTime = it.F_InvoiceTime.Value.ToString("yyyy-MM-dd"),
            })
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            if(item.F_ApplyFiles != null)
            {
                item.F_ApplyFiles = item.F_ApplyFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_ApplyFiles = new List<FileControlsModel>();
            }

            if(item.F_InvoiceFiles != null)
            {
                item.F_InvoiceFiles = item.F_InvoiceFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_InvoiceFiles = new List<FileControlsModel>();
            }

            // 申请人员
            if(item.F_ApplyUserId != null)
            {
                item.F_ApplyUserId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_ApplyUserId)))?.RealName;
            }

            // 开票人员
            if(item.F_InvoiceUserId != null)
            {
                item.F_InvoiceUserId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_InvoiceUserId)))?.RealName;
            }

        });

        var resData = data.list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcInvoiceEntity>(new ACtcInvoiceEntity()));
        resData = await _controlParsing.GetParsDataList(resData, "F_ContractId", "popupSelect", _userManager.TenantId, fieldList);

        data.list = resData.ToObject<List<ACtcInvoiceListOutput>>(CommonConst.options);
        return PageResult<ACtcInvoiceListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_ctc_invoice.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] ACtcInvoiceCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACtcInvoiceEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ACtcInvoiceEntity>(new ACtcInvoiceEntity()));
        ConfigModel tableField2cca74Config = new ConfigModel
        {
            tableName = "a_ctc_invoice_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "操作日志",
            children = ExportImportDataHelper.GetTemplateParsing<ACtcInvoiceLogEntity>(new ACtcInvoiceLogEntity())
        };
        FieldsModel tableField2cca74Model = new FieldsModel()
        {
            __config__ = tableField2cca74Config,
            __vModel__ = "tableField2cca74"
        };
        fieldList.Add(tableField2cca74Model);


        var aCtcInvoiceLogEntityList = input.tableField2cca74.Adapt<List<ACtcInvoiceLogEntity>>();
        if(aCtcInvoiceLogEntityList != null)
        {
            foreach (var item in aCtcInvoiceLogEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.ACtcInvoiceLogList = aCtcInvoiceLogEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.ACtcInvoiceLogList)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取a_ctc_invoice无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(ACtcInvoiceListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aCtcInvoiceLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<ACtcInvoiceListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_ctc_invoice"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_ctc_invoice_log"))) aCtcInvoiceLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_ctc_invoice_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcInvoiceEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcInvoiceLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField2cca74-", entityInfo, 1);
        List<IConditionalModel> aCtcInvoiceLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);


        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcInvoiceEntity>();
        var F_StatusDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_Status").DbColumnName;
        var F_ApplyUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_ApplyUserId").DbColumnName;
        var F_InvoiceUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_InvoiceUserId").DbColumnName;

        var list = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_StatusDbColumnName, input.F_Status))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_ApplyUserIdDbColumnName, input.F_ApplyUserId))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_InvoiceUserIdDbColumnName, input.F_InvoiceUserId))
            .WhereIF(input.F_InvoiceTime?.Count() > 0, it => SqlFunc.Between(it.F_InvoiceTime, input.F_InvoiceTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_InvoiceTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .WhereIF(aCtcInvoiceLogAuthorizeWhere?.Count > 0, it => it.ACtcInvoiceLogList.Any(aCtcInvoiceLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aCtcInvoiceLogConditionalModel.Count > 0, it => it.ACtcInvoiceLogList.Any(aCtcInvoiceLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new ACtcInvoiceListOutput
            {
                id = it.id,
                F_ContractId = it.F_ContractId,
                F_Money = it.F_Money,
                F_Status = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Status) && dic.DictionaryTypeId.Equals("2039943312961572864")).Select(dic => dic.FullName),
                F_ApplyFiles = it.F_ApplyFiles,
                F_ApplyRemark = it.F_ApplyRemark,
                F_InvoiceFiles = it.F_InvoiceFiles,
                F_InvoiceRemark = it.F_InvoiceRemark,
                F_ApplyUserId = it.F_ApplyUserId,
                F_ApplyTime = it.F_ApplyTime.Value.ToString("yyyy-MM-dd"),
                F_InvoiceUserId = it.F_InvoiceUserId,
                F_InvoiceTime = it.F_InvoiceTime.Value.ToString("yyyy-MM-dd"),
            })
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            if(item.F_ApplyFiles != null)
            {
                item.F_ApplyFiles = item.F_ApplyFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_ApplyFiles = new List<FileControlsModel>();
            }

            if(item.F_InvoiceFiles != null)
            {
                item.F_InvoiceFiles = item.F_InvoiceFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_InvoiceFiles = new List<FileControlsModel>();
            }

            // 申请人员
            if(item.F_ApplyUserId != null)
            {
                item.F_ApplyUserId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_ApplyUserId)))?.RealName;
            }

            // 开票人员
            if(item.F_InvoiceUserId != null)
            {
                item.F_InvoiceUserId = (await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(it => it.Id.Equals(item.F_InvoiceUserId)))?.RealName;
            }

        });

        var resData = list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcInvoiceEntity>(new ACtcInvoiceEntity()));
        resData = await _controlParsing.GetParsDataList(resData, "F_ContractId", "popupSelect", _userManager.TenantId, fieldList);

        list = resData.ToObject<List<ACtcInvoiceListOutput>>(CommonConst.options);
        return list;
    }

    /// <summary>
    /// 导出a_ctc_invoice.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] ACtcInvoiceListQueryInput input)
    {
        var exportData = new List<ACtcInvoiceListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<ACtcInvoiceListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName, input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新a_ctc_invoice.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] ACtcInvoiceUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACtcInvoiceEntity>();

        // 获取更新前的旧数据，用于生成操作日志
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id && it.DeleteMark == null);
        if (oldEntity == null)
        {
            throw Oops.Oh(ErrorCode.COM1005);
        }

        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ACtcInvoiceEntity>(new ACtcInvoiceEntity()));
        ConfigModel tableField2cca74Config = new ConfigModel
        {
            tableName = "a_ctc_invoice_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "操作日志",
            children = ExportImportDataHelper.GetTemplateParsing<ACtcInvoiceLogEntity>(new ACtcInvoiceLogEntity())
        };
        FieldsModel tableField2cca74Model = new FieldsModel()
        {
            __config__ = tableField2cca74Config,
            __vModel__ = "tableField2cca74"
        };
        fieldList.Add(tableField2cca74Model);

        // 移除合同发票管理日志可能被删除数据
        if (input.tableField2cca74 !=null && input.tableField2cca74.Any())
            await _repository.AsSugarClient().Deleteable<ACtcInvoiceLogEntity>().Where(it => it.F_InvoiceId == entity.id && !input.tableField2cca74.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<ACtcInvoiceLogEntity>().Where(it => it.F_InvoiceId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增合同发票管理日志新数据
        var aCtcInvoiceLogEntityList = input.tableField2cca74.Adapt<List<ACtcInvoiceLogEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_ContractId,
                    it.F_Money,
                    it.F_Status,
                    it.F_ApplyFiles,
                    it.F_ApplyRemark,
                    it.F_InvoiceFiles,
                    it.F_InvoiceRemark,
                    it.F_ApplyUserId,
                    it.F_ApplyTime,
                    it.F_InvoiceUserId,
                    it.F_InvoiceTime,
                })
                .ExecuteCommandAsync();

            if(aCtcInvoiceLogEntityList != null)
            {
                foreach (var item in aCtcInvoiceLogEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_InvoiceId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_InvoiceId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }

            // 生成操作日志：比较新旧数据，记录变更的字段
            await GenerateOperationLog(oldEntity, entity);

        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 生成操作日志：比较新旧数据，记录变更的字段
    /// </summary>
    /// <param name="oldEntity">更新前的旧数据</param>
    /// <param name="newEntity">更新后的新数据</param>
    /// <returns></returns>
    private async Task GenerateOperationLog(ACtcInvoiceEntity oldEntity, ACtcInvoiceEntity newEntity)
    {
        var operationLogs = new List<ACtcInvoiceLogEntity>();

        // 定义字段映射：字段名 -> 显示名称
        var fieldMappings = new Dictionary<string, string>
        {
            { "F_ContractId", "合同ID" },
            { "F_Money", "开票金额(元)" },
            { "F_Status", "开票状态" },
            { "F_ApplyFiles", "申请附件" },
            { "F_ApplyRemark", "申请备注" },
            { "F_InvoiceFiles", "开票附件" },
            { "F_InvoiceRemark", "开票备注" },
            { "F_ApplyUserId", "申请人员" },
            { "F_ApplyTime", "申请时间" },
            { "F_InvoiceUserId", "开票人员" },
            { "F_InvoiceTime", "开票时间" }
        };

        // 使用反射比较字段值
        var properties = typeof(ACtcInvoiceEntity).GetProperties()
            .Where(p => fieldMappings.ContainsKey(p.Name));

        foreach (var property in properties)
        {
            var oldValue = property.GetValue(oldEntity);
            var newValue = property.GetValue(newEntity);

            // 比较值是否发生变化
            if (!Equals(oldValue, newValue))
            {
                var fieldName = fieldMappings[property.Name];
                var oldValueStr = oldValue?.ToString() ?? "空";
                var newValueStr = newValue?.ToString() ?? "空";

                var log = new ACtcInvoiceLogEntity
                {
                    F_Id = SnowflakeIdHelper.NextId(),
                    F_InvoiceId = newEntity.id,
                    F_Title = "修改数据",
                    F_Content = $"修改了字段 [{fieldName}]，从 [{oldValueStr}] 改为 [{newValueStr}]",
                    F_CreatorUserId = _userManager.UserId,
                    F_CreatorTime = DateTime.Now
                };

                operationLogs.Add(log);
            }
        }

        // 批量插入操作日志
        if (operationLogs.Any())
        {
            await _repository.AsSugarClient().Insertable(operationLogs).ExecuteCommandAsync();
        }
    }





    /// <summary>
    /// 删除a_ctc_invoice.
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
    /// 批量删除a_ctc_invoice.
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
            // 批量删除a_ctc_invoice
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_ctc_invoice详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.ACtcInvoiceLogList)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.id == id)
            .Select(it => new ACtcInvoiceDetailOutput
            {
                id = it.id,
                F_ContractId = it.F_ContractId,
                F_Money = it.F_Money,
                F_Status = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Status) && dic.DictionaryTypeId.Equals("2039943312961572864")).Select(dic => dic.FullName),
                F_ApplyFiles = it.F_ApplyFiles,
                F_ApplyRemark = it.F_ApplyRemark,
                F_InvoiceFiles = it.F_InvoiceFiles,
                F_InvoiceRemark = it.F_InvoiceRemark,
                F_ApplyUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_ApplyUserId)).Select(u => u.RealName),
                F_ApplyTime = it.F_ApplyTime.Value.ToString("yyyy-MM-dd"),
                F_InvoiceUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_InvoiceUserId)).Select(u => u.RealName),
                F_InvoiceTime = it.F_InvoiceTime.Value.ToString("yyyy-MM-dd"),
                tableField2cca74 = it.ACtcInvoiceLogList.Adapt<List<ACtcInvoiceLogDetailOutput>>(),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            if(item.F_ApplyFiles != null)
            {
                item.F_ApplyFiles = item.F_ApplyFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_ApplyFiles = new List<FileControlsModel>();
            }

            if(item.F_InvoiceFiles != null)
            {
                item.F_InvoiceFiles = item.F_InvoiceFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_InvoiceFiles = new List<FileControlsModel>();
            }

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField2cca74), async aCtcInvoiceLog =>
        {
            var aCtcInvoice = data.ToList().Find(it => it.id.Equals(aCtcInvoiceLog.F_InvoiceId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aCtcInvoiceLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aCtcInvoiceLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aCtcInvoiceLog.F_CreatorTime = aCtcInvoiceLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcInvoiceEntity>(new ACtcInvoiceEntity()));
        resData = await _controlParsing.GetParsDataList(resData,"F_ContractId","popupSelect",_userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }

    /// <summary>
    /// 计算指定合同的项目总金额，计算公式： (F_Price * F_SaleQty) * (F_Discount/10)
    /// </summary>
    /// <param name="F_ContractId">合同ID</param>
    /// <returns>总金额（保留两位小数）</returns>
    [HttpGet("CalculateContractAmount/{F_ContractId}")]
    public async Task<dynamic> CalculateContractAmount(string F_ContractId)
    {
        if (string.IsNullOrEmpty(F_ContractId)) return 0M;

        var items = await _repositorysp.AsQueryable()
            .Where(it => it.F_ContractId == F_ContractId && it.DeleteMark == null)
            .ToListAsync();

        decimal total = 0M;
        foreach (var item in items)
        {
            decimal price = item.F_Price ?? 0M;
            decimal qty = (decimal)(item.F_SaleQty ?? 0);
            decimal discount = item.F_Discount ?? 10M;
            decimal lineAmount = (price * qty) * (discount / 10M);
            total += lineAmount;
        }

        return Math.Round(total, 2);
    }

    
}
