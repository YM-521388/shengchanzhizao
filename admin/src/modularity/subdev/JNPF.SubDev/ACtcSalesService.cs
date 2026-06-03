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
using JNPF.example.Entitys.Dto.ACtcSales;
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
/// 业务实现：销售费用.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "ACtcSales", Order = 200)]
[Route("api/example/[controller]")]
public class ACtcSalesService : IACtcSalesService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<ACtcSalesEntity> _repository;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"客户\",\"field\":\"F_CustomerId\"},{\"value\":\"合同\",\"field\":\"F_ContractId\"},{\"value\":\"支出日期\",\"field\":\"F_ExpendDate\"},{\"value\":\"支出类别\",\"field\":\"F_ExpendType\"},{\"value\":\"金额\",\"field\":\"F_Money\"},{\"value\":\"审核状态\",\"field\":\"F_AuditStatus\"},{\"value\":\"附件\",\"field\":\"F_Files\"},{\"value\":\"结算状态\",\"field\":\"F_SettleStatus\"},{\"value\":\"结算附件\",\"field\":\"F_SettleFiles\"},{\"value\":\"结算备注\",\"field\":\"F_SettleDesc\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="ACtcSalesService"/>类型的新实例.
    /// </summary>
    public ACtcSalesService(
        ISqlSugarRepository<ACtcSalesEntity> repository,
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
    /// 获取销售费用.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new ACtcSalesEntity
            {
                id = it.id,
                F_CustomerId = it.F_CustomerId,
                F_ContractId = it.F_ContractId,
                F_ExpendDate = it.F_ExpendDate,
                F_ExpendType = it.F_ExpendType,
                F_Money = it.F_Money,
                F_AuditStatus = it.F_AuditStatus,
                F_Files = it.F_Files,
                F_SettleStatus = it.F_SettleStatus,
                F_SettleFiles = it.F_SettleFiles,
                F_SettleDesc = it.F_SettleDesc,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<ACtcSalesInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取销售费用列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] ACtcSalesListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<ACtcSalesListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcSalesEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();
        var F_CustomerIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_CustomerId").DbColumnName;
        var F_ExpendTypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_ExpendType").DbColumnName;
        var startF_Money = input.F_Money?.FirstOrDefault()?.ParseToDecimal() == null ? decimal.MinValue : input.F_Money?.First();
        var endF_Money = input.F_Money?.LastOrDefault()?.ParseToDecimal() == null ? decimal.MaxValue : input.F_Money?.Last();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_CustomerIdDbColumnName, input.F_CustomerId))
            .WhereIF(input.F_ExpendDate?.Count() > 0, it => SqlFunc.Between(it.F_ExpendDate, input.F_ExpendDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_ExpendDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_ExpendTypeDbColumnName, input.F_ExpendType))
            .WhereIF(input.F_Money?.Count() > 0, it => SqlFunc.Between(it.F_Money, startF_Money, endF_Money))
            .WhereIF(!string.IsNullOrEmpty(input.F_AuditStatus), it => it.F_AuditStatus.Contains(input.F_AuditStatus))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new ACtcSalesListOutput
            {
                id = it.id,
                F_CustomerId = it.F_CustomerId,
                F_ContractId = it.F_ContractId,
                F_ExpendDate = it.F_ExpendDate.Value.ToString("yyyy-MM-dd"),
                F_ExpendType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_ExpendType) && dic.DictionaryTypeId.Equals("2010895416983425024")).Select(dic => dic.FullName),
                F_Money = it.F_Money,
                F_AuditStatus = it.F_AuditStatus,
                F_Files = it.F_Files,
                F_SettleStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_SettleStatus) && dic.DictionaryTypeId.Equals("2014214471169478656")).Select(dic => dic.FullName),
                F_SettleStatusCode = it.F_SettleStatus,
                F_SettleFiles = it.F_SettleFiles,
                F_SettleDesc = it.F_SettleDesc,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 客户ID
            if(item.F_CustomerId != null)
            {
                var F_CustomerIdData = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2009458830353764352", "F_Id", "F_CustomerName", "");
                item.F_CustomerId = F_CustomerIdData.Find(it => it.id.Equals(item.F_CustomerId))?.fullName;
            }

            if(item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }

            if(item.F_SettleFiles != null)
            {
                item.F_SettleFiles = item.F_SettleFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_SettleFiles = new List<FileControlsModel>();
            }

        });

        var resData = data.list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcSalesEntity>(new ACtcSalesEntity()));
        resData = await _controlParsing.GetParsDataList(resData, "F_ContractId", "popupSelect", _userManager.TenantId, fieldList);

        data.list = resData.ToObject<List<ACtcSalesListOutput>>(CommonConst.options);

        return PageResult<ACtcSalesListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建销售费用.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] ACtcSalesCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACtcSalesEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        entity.F_SettleStatus = "0";
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ACtcSalesEntity>(new ACtcSalesEntity()));
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 获取销售费用无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(ACtcSalesListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<ACtcSalesListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ACtcSalesEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();
        var F_CustomerIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_CustomerId").DbColumnName;
        var F_ExpendTypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_ExpendType").DbColumnName;
        var startF_Money = input.F_Money?.FirstOrDefault()?.ParseToDecimal() == null ? decimal.MinValue : input.F_Money?.First();
        var endF_Money = input.F_Money?.LastOrDefault()?.ParseToDecimal() == null ? decimal.MaxValue : input.F_Money?.Last();
        var list = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_CustomerIdDbColumnName, input.F_CustomerId))
            .WhereIF(input.F_ExpendDate?.Count() > 0, it => SqlFunc.Between(it.F_ExpendDate, input.F_ExpendDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_ExpendDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_ExpendTypeDbColumnName, input.F_ExpendType))
            .WhereIF(input.F_Money?.Count() > 0, it => SqlFunc.Between(it.F_Money, startF_Money, endF_Money))
            .WhereIF(!string.IsNullOrEmpty(input.F_AuditStatus), it => it.F_AuditStatus.Contains(input.F_AuditStatus))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new ACtcSalesListOutput
            {
                id = it.id,
                F_CustomerId = it.F_CustomerId,
                F_ContractId = it.F_ContractId,
                F_ExpendDate = it.F_ExpendDate.Value.ToString("yyyy-MM-dd"),
                F_ExpendType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_ExpendType) && dic.DictionaryTypeId.Equals("2010895416983425024")).Select(dic => dic.FullName),
                F_Money = it.F_Money,
                F_AuditStatus = it.F_AuditStatus,
                F_Files = it.F_Files,
                F_SettleStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_SettleStatus) && dic.DictionaryTypeId.Equals("2014214471169478656")).Select(dic => dic.FullName),
                F_SettleFiles = it.F_SettleFiles,
                F_SettleDesc = it.F_SettleDesc,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 客户ID
            if(item.F_CustomerId != null)
            {
                var F_CustomerIdData = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2009458830353764352", "F_Id", "F_CustomerName", "");
                item.F_CustomerId = F_CustomerIdData.Find(it => it.id.Equals(item.F_CustomerId))?.fullName;
            }

            if(item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }

            if(item.F_SettleFiles != null)
            {
                item.F_SettleFiles = item.F_SettleFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_SettleFiles = new List<FileControlsModel>();
            }

        });

        var resData = list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcSalesEntity>(new ACtcSalesEntity()));
        resData = await _controlParsing.GetParsDataList(resData, "F_ContractId", "popupSelect", _userManager.TenantId, fieldList);

        list = resData.ToObject<List<ACtcSalesListOutput>>(CommonConst.options);

        return list;
    }

    /// <summary>
    /// 导出销售费用.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] ACtcSalesListQueryInput input)
    {
        var exportData = new List<ACtcSalesListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<ACtcSalesListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName,input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新销售费用.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] ACtcSalesUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACtcSalesEntity>();
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_CustomerId,
                it.F_ContractId,
                it.F_ExpendDate,
                it.F_ExpendType,
                it.F_Money,
                it.F_AuditStatus,
                it.F_Files,
                it.F_SettleStatus,
                it.F_SettleFiles,
                it.F_SettleDesc,
                it.F_Description,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 结算.
    /// </summary>
    [HttpPut("Check/{id}")]
    public async Task Check(string id, [FromBody] ACtcSalesUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ACtcSalesEntity>();
        entity.F_SettleStatus = "1";
        var isOk = 0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new {
                it.F_SettleStatus,
                it.F_SettleFiles,
                it.F_SettleDesc,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }


    /// <summary>
    /// 删除销售费用.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除销售费用.
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
            // 批量删除销售费用
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 销售费用详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new ACtcSalesDetailOutput
            {
                id = it.id,
                F_CustomerId = it.F_CustomerId,
                F_ContractId = it.F_ContractId,
                F_ExpendDate = it.F_ExpendDate.Value.ToString("yyyy-MM-dd"),
                F_ExpendType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_ExpendType) && dic.DictionaryTypeId.Equals("2010895416983425024")).Select(dic => dic.FullName),
                F_Money = it.F_Money,
                F_AuditStatus = it.F_AuditStatus,
                F_Files = it.F_Files,
                F_SettleStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_SettleStatus) && dic.DictionaryTypeId.Equals("2014214471169478656")).Select(dic => dic.FullName),
                F_SettleFiles = it.F_SettleFiles,
                F_SettleDesc = it.F_SettleDesc,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 客户ID
            if(item.F_CustomerId != null)
            {
                var F_CustomerIdData = await _dataInterfaceService.GetDynamicList("F_CustomerId", "2009458830353764352", "F_Id", "F_CustomerName", "");
                item.F_CustomerId = F_CustomerIdData.Find(it => it.id.Equals(item.F_CustomerId))?.fullName;
            }

            if(item.F_Files != null)
            {
                item.F_Files = item.F_Files.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Files = new List<FileControlsModel>();
            }

            if(item.F_SettleFiles != null)
            {
                item.F_SettleFiles = item.F_SettleFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_SettleFiles = new List<FileControlsModel>();
            }

        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ACtcSalesEntity>(new ACtcSalesEntity()));
        resData = await _controlParsing.GetParsDataList(resData, "F_ContractId", "popupSelect", _userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }
}
