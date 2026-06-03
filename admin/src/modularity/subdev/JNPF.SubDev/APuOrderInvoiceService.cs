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
using JNPF.example.Entitys.Dto.APuOrderInvoice;
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
/// 业务实现：采购单发票单管理.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuOrderInvoice", Order = 200)]
[Route("api/example/[controller]")]
public class APuOrderInvoiceService : IAPuOrderInvoiceService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuOrderInvoiceEntity> _repository;
    private readonly ISqlSugarRepository<APuOrderEntity> _repositoryorder;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"采购单\",\"field\":\"F_OrderId\"},{\"value\":\"供应商\",\"field\":\"F_SupplierId\"},{\"value\":\"发票金额\",\"field\":\"F_Money\"},{\"value\":\"开票日期\",\"field\":\"F_InvoiceDate\"},{\"value\":\"附件\",\"field\":\"F_Files\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="APuOrderInvoiceService"/>类型的新实例.
    /// </summary>
    public APuOrderInvoiceService(
        ISqlSugarRepository<APuOrderInvoiceEntity> repository,
        ISqlSugarRepository<APuOrderEntity> repositoryorder,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repository = repository;
        _repositoryorder = repositoryorder;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取采购单发票单管理.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new APuOrderInvoiceEntity
            {
                id = it.id,
                F_OrderId = it.F_OrderId,
                F_SupplierId = it.F_SupplierId,
                F_Money = it.F_Money,
                F_InvoiceDate = it.F_InvoiceDate,
                F_Files = it.F_Files,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuOrderInvoiceInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取采购单发票单管理列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuOrderInvoiceListQueryInput input)
    {
        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuOrderInvoiceEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();
        var F_SupplierIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_SupplierId").DbColumnName;
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_SupplierIdDbColumnName, input.F_SupplierId))
            .WhereIF(input.F_InvoiceDate?.Count() > 0, it => SqlFunc.Between(it.F_InvoiceDate, input.F_InvoiceDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_InvoiceDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new APuOrderInvoiceListOutput
            {
                id = it.id,
                F_OrderId = it.F_OrderId,
                F_SupplierId = it.F_SupplierId,
                F_Money = it.F_Money,
                F_InvoiceDate = it.F_InvoiceDate.Value.ToString("yyyy-MM-dd"),
                F_Files = it.F_Files,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 供应商ID
            if(item.F_SupplierId != null)
            {
                var F_SupplierIdData = await _dataInterfaceService.GetDynamicList("F_SupplierId", "2008457397298925568", "F_Id", "F_SupplierName", "");
                item.F_SupplierId = F_SupplierIdData.Find(it => it.id.Equals(item.F_SupplierId))?.fullName;
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

        var resData = data.list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<APuOrderInvoiceEntity>(new APuOrderInvoiceEntity()));
        resData = await _controlParsing.GetParsDataList(resData, "F_OrderId", "popupSelect", _userManager.TenantId, fieldList);

        data.list = resData.ToObject<List<APuOrderInvoiceListOutput>>(CommonConst.options);

        return PageResult<APuOrderInvoiceListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建采购单发票单管理.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] APuOrderInvoiceCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuOrderInvoiceEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuOrderInvoiceEntity>(new APuOrderInvoiceEntity()));
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 更新采购单发票单管理.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] APuOrderInvoiceUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuOrderInvoiceEntity>();
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_OrderId,
                it.F_SupplierId,
                it.F_Money,
                it.F_InvoiceDate,
                it.F_Files,
                it.F_Description,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除采购单发票单管理.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除采购单发票单管理.
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
            // 批量删除采购单发票单管理
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 根据采购单ID获取供应商和金额信息.
    /// </summary>
    /// <param name="orderId">采购单ID.</param>
    /// <returns></returns>
    [HttpGet("GetOrderSupplierAndMoney/{orderId}")]
    public async Task<dynamic> GetOrderSupplierAndMoney(string orderId)
    {
        var data = await _repositoryorder.AsQueryable()
            .Where(it => it.id.Equals(orderId))
            .Select(it => new
            {
                F_SupplierId = it.F_SupplierId,
                F_Money = it.F_Money
            })
            .FirstAsync();

        return data;
    }

    /// <summary>
    /// 采购单发票单管理详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new APuOrderInvoiceDetailOutput
            {
                id = it.id,
                F_OrderId = it.F_OrderId,
                F_SupplierId = it.F_SupplierId,
                F_Money = it.F_Money,
                F_InvoiceDate = it.F_InvoiceDate.Value.ToString("yyyy-MM-dd"),
                F_Files = it.F_Files,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 供应商ID
            if(item.F_SupplierId != null)
            {
                var F_SupplierIdData = await _dataInterfaceService.GetDynamicList("F_SupplierId", "2008457397298925568", "F_Id", "F_SupplierName", "");
                item.F_SupplierId = F_SupplierIdData.Find(it => it.id.Equals(item.F_SupplierId))?.fullName;
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

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<APuOrderInvoiceEntity>(new APuOrderInvoiceEntity()));
        resData = await _controlParsing.GetParsDataList(resData, "F_OrderId", "popupSelect", _userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }
}
