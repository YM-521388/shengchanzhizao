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
using JNPF.example.Entitys.Dto.AGoods;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.DatabaseAccessor;
using JNPF.Common.Dtos;
using Microsoft.AspNetCore.Http;
using JNPF.VisualDev.Engine.Core;
using System.Reactive;
using System.Data;
using JNPF.Common.Helper;

namespace JNPF.example;

/// <summary>
/// 业务实现：商品管理.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AGoods", Order = 200)]
[Route("api/example/[controller]")]
public class AGoodsService : IAGoodsService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AGoodsEntity> _repository;
    private readonly ISqlSugarRepository<AConfigEntity> _repositorycs;
    private readonly ISqlSugarRepository<ACtcContractItemEntity> _repositoryCtcItem;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<AGoodsInventoryQrEntity> _repositoryInventoryQr;
    private readonly ISqlSugarRepository<AGoodsSpecificationEntity> _repositorygg;
    private readonly ISqlSugarRepository<AGoodsCategoryEntity> _repositoryCategory;


    /// <summary>
    /// 数据库管理.
    /// </summary>
    private readonly IDataBaseManager _dataBaseManager;

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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"商品编号\",\"field\":\"F_GoodsNo\"},{\"value\":\"商品名称\",\"field\":\"F_GoodsName\"},{\"value\":\"类别\",\"field\":\"F_CategoryId\"},{\"value\":\"销售单价\",\"field\":\"F_SalePrice\"},{\"value\":\"成本单价\",\"field\":\"F_CostPrice\"},{\"value\":\"售后佣金\",\"field\":\"F_CommissionFixed\"},{\"value\":\"商品来源\",\"field\":\"F_Source\"},{\"value\":\"规格\",\"field\":\"F_Specification\"},{\"value\":\"材质\",\"field\":\"F_Material\"},{\"value\":\"检验规范\",\"field\":\"F_InspectRule\"},{\"value\":\"库存上限警告值\",\"field\":\"F_StockUpperLimit\"},{\"value\":\"库存下限警告值\",\"field\":\"F_StockLowerLimit\"},{\"value\":\"外协单价\",\"field\":\"F_OutsourcePrice\"},{\"value\":\"供应商\",\"field\":\"F_SupplierId\"},{\"value\":\"商品图片\",\"field\":\"F_Image\"},{\"value\":\"创建用户\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"},{\"value\":\"附件\",\"field\":\"F_AttachmentUrl\"},{\"value\":\"创建组织\",\"field\":\"F_CreatorOrganizeId\"},{\"value\":\"商品类型\",\"field\":\"F_Type\"}]".ToList<ParamsModel>();
    /// <summary>
    /// 导入字段.
    /// </summary>
    private readonly string[] uploaderKey = new string[] { "F_GoodsNo", "F_GoodsName", "F_Specification", "F_Material", "F_Type", "F_CategoryId", "F_Unit", "F_Unit_Ratio", "F_Num", "F_SalePrice", "F_CostPrice", "F_CommissionFixed", "F_Source",  "F_OutsourcePrice", "F_SupplierId", "F_InspectRule", "F_StockUpperLimit", "F_StockLowerLimit", "F_Intro", "F_Remark" };


    private readonly string[] unitKey = new string[] { "F_GoodsNo", "F_Unit","F_Num" };

    /// <summary>
    /// 初始化一个<see cref="AGoodsService"/>类型的新实例.
    /// </summary>
    public AGoodsService(
        IDataBaseManager dataBaseManager,
        ISqlSugarRepository<AGoodsInventoryQrEntity> repositoryInventoryQr,
        ISqlSugarRepository<AGoodsCategoryEntity> repositoryCategory,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<ACtcContractItemEntity> repositoryCtcItem,
        ISqlSugarRepository<AGoodsSpecificationEntity> repositorygg,
        ISqlSugarRepository<AGoodsEntity> repository,
        ISqlSugarRepository<AConfigEntity> repositorycs,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryInventoryQr = repositoryInventoryQr;
        _repositoryCategory = repositoryCategory;
        _dataBaseManager = dataBaseManager;
        _repositoryInventory = repositoryInventory;
        _repositoryCtcItem = repositoryCtcItem;
        _repositorygg = repositorygg;
        _repository = repository;
        _repositorycs = repositorycs;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取商品管理.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new AGoodsEntity
            {
                id = it.id,
                F_GoodsNo = it.F_GoodsNo,
                F_GoodsName = it.F_GoodsName,
                F_CategoryId = it.F_CategoryId,
                F_Unit = it.F_Unit,
                F_Unit_Ratio = it.F_Unit_Ratio,
                F_Type = it.F_Type,
                F_Material = it.F_Material,
                F_SalePrice = it.F_SalePrice,
                F_CostPrice = it.F_CostPrice,
                F_CommissionFixed = it.F_CommissionFixed,
                F_Source = it.F_Source,
                F_OutsourcePrice = it.F_OutsourcePrice,
                F_SupplierId = it.F_SupplierId,
                F_Specification = it.F_Specification,
                F_InspectRule = it.F_InspectRule,
                F_StockUpperLimit = it.F_StockUpperLimit,
                F_StockLowerLimit = it.F_StockLowerLimit,
                F_Encoding = it.F_Encoding,
                F_Image = it.F_Image,
                F_AttachmentUrl = it.F_AttachmentUrl,
                F_Remark = it.F_Remark,
                F_Intro = it.F_Intro,
                F_CreatorTime = it.F_CreatorTime,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AGoodsInfoOutput>();

        return data;
    }

    /// <summary>
    /// 获取商品管理列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AGoodsListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AGoodsListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AGoodsEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var F_TypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_Type").DbColumnName;
        var selectIds = input.selectIds?.Split(",").ToList();
        var F_CategoryId = input.F_CategoryId?.Last();

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_GoodsNo), it => it.F_GoodsNo.Contains(input.F_GoodsNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_GoodsName), it => it.F_GoodsName.Contains(input.F_GoodsName))
             .WhereIF(!string.IsNullOrEmpty(input.F_Encoding), it => it.F_Encoding.Contains(input.F_Encoding))
            .WhereIF(!string.IsNullOrEmpty(input.F_CategoryId?.ToString()), it => it.F_CategoryId.Contains(F_CategoryId))
            .WhereIF(!string.IsNullOrEmpty(input.F_Specification), it => it.F_Specification.Contains(input.F_Specification))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_TypeDbColumnName, input.F_Type))
            .WhereIF(!string.IsNullOrEmpty(input.F_InspectRule), it => it.F_InspectRule.Contains(input.F_InspectRule))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new AGoodsListOutput
            {
                id = it.id,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_GoodsNo = it.F_GoodsNo,
            })
            //把多表结果变成“单表”再排序：
            .MergeTable()
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .OrderBy(it => it.F_GoodsNo, OrderByType.Desc)
            .ToPagedListAsync(input.currentPage, input.pageSize);

        var ids = idsQ.list.Select(x => x.id).ToList();   // 20 个 long

        // 2. 再用 ids 一次性补全字段（此时只有 20 行，随便 join 18 张表都毫秒级）
        var data = await _repository.AsQueryable()
            .Where(it => ids.Contains(it.id))
             .Select(it => new AGoodsListOutput
             {
                 id = it.id,
                 F_GoodsNo = it.F_GoodsNo,
                 F_GoodsName = it.F_GoodsName,
                 F_CategoryId = it.F_CategoryId,
                 F_Unit = it.F_Unit,
                 F_Material = it.F_Material,
                 F_SalePrice = it.F_SalePrice,
                 F_CostPrice = it.F_CostPrice,
                 F_CommissionFixed = it.F_CommissionFixed,
                 F_Source = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Source) && dic.DictionaryTypeId.Equals("2008448951216377856")).Select(dic => dic.FullName),
                 F_OutsourcePrice = it.F_OutsourcePrice,
                 F_SupplierId = it.F_SupplierId,
                 F_Specification = it.F_Specification,
                 F_InspectRule = it.F_InspectRule,
                 F_StockUpperLimit = it.F_StockUpperLimit.ToString(),
                 F_StockLowerLimit = it.F_StockLowerLimit.ToString(),
                 F_Encoding = it.F_Encoding,
                 F_Image = it.F_Image,
                 F_AttachmentUrl = it.F_AttachmentUrl,
                 F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                 F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2029481827470807040")).Select(dic => dic.FullName),
                 F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
             }).MergeTable().OrderBy("F_CreatorTime desc").OrderBy(it => it.F_GoodsNo, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 商品分类ID
            var F_CategoryIdData = await _dataInterfaceService.GetDynamicList("F_CategoryId", "2008414575283802112", "F_Id", "F_Name", "children");
            if (item.F_CategoryId != null)
            {
                var F_CategoryIdCascader = item.F_CategoryId.ToObject<List<string>>();
                item.F_CategoryId = string.Join("/", F_CategoryIdData.FindAll(it => F_CategoryIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

            // 单位（级联路径，如 箱子/盒/瓶，直接查 AGoodsSpecificationEntity）
            if (!string.IsNullOrEmpty(item.F_Unit))
            {
                var unitIds = item.F_Unit.ToObject<List<string>>();
                var specData = await _repositorygg.AsSugarClient().Queryable<AGoodsSpecificationEntity>()
                    .Where(it => unitIds.Contains(it.id) && it.DeleteMark == null)
                    .Select(it => new { it.id, it.F_Name })
                    .ToListAsync();
                var specMap = specData.ToDictionary(it => it.id, it => it.F_Name ?? "");
                item.F_Unit = string.Join("/", unitIds.Where(id => specMap.ContainsKey(id)).Select(id => specMap[id]));
            }

            // 供应商ID
            if (item.F_SupplierId != null)
            {
                var F_SupplierIdData = await _dataInterfaceService.GetDynamicList("F_SupplierId", "2008457397298925568", "F_Id", "FSupplierName", "");
                item.F_SupplierId = F_SupplierIdData.Find(it => it.id.Equals(item.F_SupplierId))?.fullName;
            }

            if (item.F_Image != null)
            {
                item.F_Image = item.F_Image.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Image = new List<FileControlsModel>();
            }

            if (item.F_AttachmentUrl != null)
            {
                item.F_AttachmentUrl = item.F_AttachmentUrl.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_AttachmentUrl = new List<FileControlsModel>();
            }

        });

        data.pagination = idsQ.pagination;

        return PageResult<AGoodsListOutput>.SqlSugarPageResult(data);
    }


    /// <summary>
    /// 获取商品管理无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(AGoodsListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AGoodsListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AGoodsEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();
        var F_TypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_Type").DbColumnName;
        var F_CategoryId = input.F_CategoryId?.Last();
        var list = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_GoodsNo), it => it.F_GoodsNo.Contains(input.F_GoodsNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_GoodsName), it => it.F_GoodsName.Contains(input.F_GoodsName))
            .WhereIF(!string.IsNullOrEmpty(input.F_CategoryId?.ToString()), it => it.F_CategoryId.Contains(F_CategoryId))
            .WhereIF(!string.IsNullOrEmpty(input.F_Specification), it => it.F_Specification.Contains(input.F_Specification))
             .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_TypeDbColumnName, input.F_Type))
            .WhereIF(!string.IsNullOrEmpty(input.F_InspectRule), it => it.F_InspectRule.Contains(input.F_InspectRule))
             .WhereIF(!string.IsNullOrEmpty(input.F_Encoding), it => it.F_Encoding.Contains(input.F_Encoding))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new AGoodsListOutput
            {
                id = it.id,
                F_GoodsNo = it.F_GoodsNo,
                F_GoodsName = it.F_GoodsName,
                F_CategoryId = it.F_CategoryId,
                F_Unit = it.F_Unit,
                F_Material = it.F_Material,
                F_SalePrice = it.F_SalePrice,
                F_CostPrice = it.F_CostPrice,
                F_CommissionFixed = it.F_CommissionFixed,
                F_Source = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Source) && dic.DictionaryTypeId.Equals("2008448951216377856")).Select(dic => dic.FullName),
                F_OutsourcePrice = it.F_OutsourcePrice,
                F_SupplierId = it.F_SupplierId,
                F_Specification = it.F_Specification,
                F_InspectRule = it.F_InspectRule,
                F_StockUpperLimit = it.F_StockUpperLimit.ToString(),
                F_StockLowerLimit = it.F_StockLowerLimit.ToString(),
                F_Encoding = it.F_Encoding,
                F_Image = it.F_Image,
                F_AttachmentUrl = it.F_AttachmentUrl,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2029481827470807040")).Select(dic => dic.FullName),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
            }).MergeTable()
            .OrderBy("F_CreatorTime desc") // 按创建时间降序
            .OrderBy(it => it.F_GoodsNo, OrderByType.Desc)
            .OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 商品分类ID
            var F_CategoryIdData = await _dataInterfaceService.GetDynamicList("F_CategoryId", "2008414575283802112", "F_Id", "F_Name", "children");
            if (item.F_CategoryId != null)
            {
                var F_CategoryIdCascader = item.F_CategoryId.ToObject<List<string>>();
                item.F_CategoryId = string.Join("/", F_CategoryIdData.FindAll(it => F_CategoryIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

            // 单位
            // 单位（级联路径，如 箱子/盒/瓶，直接查 AGoodsSpecificationEntity）
            if (!string.IsNullOrEmpty(item.F_Unit))
            {
                var unitIds = item.F_Unit.ToObject<List<string>>();
                var specData = await _repositorygg.AsSugarClient().Queryable<AGoodsSpecificationEntity>()
                    .Where(it => unitIds.Contains(it.id) && it.DeleteMark == null)
                    .Select(it => new { it.id, it.F_Name })
                    .ToListAsync();
                var specMap = specData.ToDictionary(it => it.id, it => it.F_Name ?? "");
                item.F_Unit = string.Join("/", unitIds.Where(id => specMap.ContainsKey(id)).Select(id => specMap[id]));
            }

            // 供应商ID
            if (item.F_SupplierId != null)
            {
                var F_SupplierIdData = await _dataInterfaceService.GetDynamicList("F_SupplierId", "2008457397298925568", "F_Id", "FSupplierName", "");
                item.F_SupplierId = F_SupplierIdData.Find(it => it.id.Equals(item.F_SupplierId))?.fullName;
            }

            if (item.F_Image != null)
            {
                item.F_Image = item.F_Image.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Image = new List<FileControlsModel>();
            }

            if (item.F_AttachmentUrl != null)
            {
                item.F_AttachmentUrl = item.F_AttachmentUrl.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_AttachmentUrl = new List<FileControlsModel>();
            }

        });

        return list;
    }

    /// <summary>
    /// 导出商品管理.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] AGoodsListQueryInput input)
    {
        var exportData = new List<AGoodsListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<AGoodsListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName, input.selectKey, _userManager.UserId, exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 下载模板.
    /// </summary>
    /// <returns></returns>
    [HttpGet("TemplateDownload")]
    public async Task<dynamic> TemplateDownload(string menuId)
    {
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<AGoodsEntity>(new AGoodsEntity()));
        List<Dictionary<string, object>>? dataList = new List<Dictionary<string, object>>();

        // 赋予默认值
        var dicItem = ExportImportDataHelper.GetTemplateHeader<AGoodsEntity>(new AGoodsEntity(), 1);

        dicItem.Add("id", "id");
        dataList.Add(dicItem);

        var excelName = string.Format("{0}导入模板", _controlParsing.GetModuleNameById(menuId));
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetTemplateExport(excelName, string.Join(",", uploaderKey), _userManager.UserId, dataList, fieldList, paramList);
    }

    /// <summary>
    /// Excel导入.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("Uploader")]
    public async Task<dynamic> Uploader(IFormFile file)
    {
        var _filePath = _fileManager.GetPathByType(string.Empty);
        var _fileName = DateTime.Now.ToString("yyyyMMdd") + "_" + SnowflakeIdHelper.NextId() + Path.GetExtension(file.FileName);
        var stream = file.OpenReadStream();
        await _fileManager.UploadFileByType(stream, _filePath, _fileName);
        return new { name = _fileName, url = string.Format("/api/File/Image/{0}/{1}", string.Empty, _fileName) };
    }

    /// <summary>
    /// 导入预览.
    /// </summary>
    /// <returns></returns>
    [HttpGet("ImportPreview")]
    public async Task<dynamic> ImportPreview(string fileName)
    {
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AGoodsEntity>(new AGoodsEntity()));
        
        // 手动添加F_Unit_Ratio字段（该字段没有CodeGenUpload特性，需要手动添加）
        //if (!fieldList.Any(f => f.__vModel__ == "F_Unit_Ratio"))
        //{
        //    fieldList.Add(new FieldsModel
        //    {
        //        __vModel__ = "F_Unit_Ratio",
        //        __config__ = new ConfigModel
        //        {
        //            label = "单位比例(F_Unit_Ratio)",
        //            jnpfKey = "input",
        //            tableName = "a_goods"
        //        }
        //    });
        //}
        
        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AGoodsEntity>();
        List<DbTableRelationModel> tables = new List<DbTableRelationModel>() { ExportImportDataHelper.GetTableRelation(entityInfo, "1") };
        DbLinkEntity link = _dataBaseManager.GetTenantDbLink(_userManager.TenantId, _userManager.TenantDbName); // 当前数据库连接
        var tInfo = new TemplateParsingBase(link, fieldList, tables, "F_Id", 2, 1, uploaderKey.ToList(), "1", true);
        return await _exportImportDataHelper.GetImportPreviewData(tInfo, fileName);
    }

    /// <summary>
    /// 导入数据.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("ImportData")]
    [UnitOfWork]
    public async Task<dynamic> ImportData([FromBody] DataImportInput input)
    {
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AGoodsEntity>(new AGoodsEntity()));
        
        // 手动添加F_Unit_Ratio字段（该字段没有CodeGenUpload特性，需要手动添加）
        if (!fieldList.Any(f => f.__vModel__ == "F_Unit_Ratio"))
        {
            fieldList.Add(new FieldsModel
            {
                __vModel__ = "F_Unit_Ratio",
                __config__ = new ConfigModel
                {
                    label = "单位比例(F_Unit_Ratio)",
                    jnpfKey = "input",
                    tableName = "a_goods"
                }
            });
        }
        
        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AGoodsEntity>();
        List<DbTableRelationModel> tables = new List<DbTableRelationModel>() { ExportImportDataHelper.GetTableRelation(entityInfo, "1") };
        DbLinkEntity link = _dataBaseManager.GetTenantDbLink(_userManager.TenantId, _userManager.TenantDbName); // 当前数据库连接
        var tInfo = new TemplateParsingBase(link, fieldList, tables, "F_Id", 2, 1, uploaderKey.ToList(), "1", true);

        // 如果是预览模式，先获取预览数据
        if (input.type)
        {
            var importDataPreview = await _exportImportDataHelper.GetImportPreviewData(tInfo, input.fileName);
            input.list = importDataPreview.dataRow;
        }

        // 对导入数据进行预处理：自动生成编号和重复性校验
        var processedList = new List<Dictionary<string, object>>();
        var errorsList = new List<Dictionary<string, object>>();

        // 查询配置表中id为2008738298582929408的数据（自动生成编号配置）
        var configData = await _repositorycs.AsQueryable()
            .Where(it => it.id.Equals("2008738298582929408"))
            .FirstAsync();
        var autoGenerateNo = configData != null && configData.F_Value == "1";

        // 查询配置表中id为2008738234443632640的数据（商品名称重复校验配置）
        var nameCheckConfig = await _repositorycs.AsQueryable()
            .Where(it => it.id.Equals("2008738234443632640"))
            .FirstAsync();
        var checkDuplicateName = nameCheckConfig != null && nameCheckConfig.F_Value == "1";

        // 调试信息
        var debugInfo = $"配置：autoGenerateNo={autoGenerateNo}, checkDuplicateName={checkDuplicateName}, 输入数据条数={input.list?.Count}";
        
        // 获取当前数据库中所有商品编号（用于重复校验）
        var existingGoodsNos = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Select(it => it.F_GoodsNo)
            .ToListAsync();
        
        // 获取当前数据库中所有商品名称和规格组合（用于重复校验）
        var existingNameSpecList = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Select(it => new { it.F_GoodsName, it.F_Specification })
            .ToListAsync();

        // 用于记录本次导入中已生成的编号（避免同一批次重复）
        var generatedNosInBatch = new List<string>();

        foreach (var item in input.list)
        {
            var errorMessages = new List<string>();
            var goodsNo = item.ContainsKey("F_GoodsNo") ? item["F_GoodsNo"]?.ToString() : null;
            var goodsName = item.ContainsKey("F_GoodsName") ? item["F_GoodsName"]?.ToString() : null;
            var goodsSpecifica = item.ContainsKey("F_Specification") ? item["F_Specification"]?.ToString() : null;

            // 1. 商品名称、规格组合重复检查
            if (!string.IsNullOrEmpty(goodsName))
            {
                // 检查数据库中是否存在相同的名称+规格组合
                var existsInDb = existingNameSpecList.Any(x => 
                    x.F_GoodsName == goodsName && 
                    x.F_Specification == goodsSpecifica);
                
                // 检查本次导入中是否已存在相同的名称+规格组合
                var existsInBatch = processedList.Any(p => 
                    p.ContainsKey("F_GoodsName") && p["F_GoodsName"]?.ToString() == goodsName && 
                    p.ContainsKey("F_Specification") && p["F_Specification"]?.ToString() == goodsSpecifica);
                
                if (existsInDb || existsInBatch)
                {
                    errorMessages.Add($"商品名称[{goodsName}]和商品规格[{goodsSpecifica}]组合重复");
                }
            }

            // 2. 商品编号重复校验 - 如果已存在则自动生成新编号
            bool needGenerateNewNo = false;
            if (!string.IsNullOrEmpty(goodsNo))
            {
                if (existingGoodsNos.Contains(goodsNo) || generatedNosInBatch.Contains(goodsNo))
                {
                    // 商品编号已存在，标记需要重新生成
                    needGenerateNewNo = true;
                }
            }
            else
            {
                // 商品编号为空，需要生成
                needGenerateNewNo = true;
            }

            // 3. 自动生成商品编号（如果编号为空或已存在）
            if (needGenerateNewNo && autoGenerateNo && !errorMessages.Any())
            {
                var prefix = "SP" + DateTime.Now.ToString("yyyyMMdd");

                // 查询数据库中已有相同前缀的编号
                var existingNos = await _repository.AsQueryable()
                    .Where(it => it.F_GoodsNo != null && it.F_GoodsNo.StartsWith(prefix) && it.DeleteMark == null)
                    .Select(it => it.F_GoodsNo)
                    .ToListAsync();

                // 合并数据库中和本次导入已生成的编号
                var allExistingNos = existingNos.Concat(generatedNosInBatch).ToList();

                int maxSeq = 0;
                foreach (var no in allExistingNos)
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
                goodsNo = prefix + nextSeq.ToString("D4");
                item["F_GoodsNo"] = goodsNo;
                generatedNosInBatch.Add(goodsNo);
            }

            // 4. 处理单位和单位比例（根据ImportUnitGoods逻辑）
            var unit = item.ContainsKey("F_Unit") ? item["F_Unit"]?.ToString()?.Trim() : null;
            var numStr = item.ContainsKey("F_Num") ? item["F_Num"]?.ToString()?.Trim() : null;
            
            if (!string.IsNullOrEmpty(unit))
            {
                // 按 "/" 拆分单位
                var unitNames = unit.Split('/').Select(u => u.Trim()).Where(u => !string.IsNullOrEmpty(u)).ToList();
                
                if (unitNames.Count > 0)
                {
                    int num = 1; // 默认值为1
                    if (!string.IsNullOrEmpty(numStr))
                    {
                        int.TryParse(numStr, out num);
                    }

                    // 在规格表中查找每个单位
                    var unitIds = new List<string>();
                    var unitRatios = new List<object>();
                    bool unitError = false;

                    for (int j = 0; j < unitNames.Count; j++)
                    {
                        var spec = await _repositorygg.AsQueryable()
                            .Where(it => it.F_Name == unitNames[j] && it.DeleteMark == null)
                            .FirstAsync();

                        if (spec == null)
                        {
                            errorMessages.Add($"规格[{unitNames[j]}]不存在");
                            unitError = true;
                            break;
                        }

                        unitIds.Add(spec.id);

                        // 如果只有一级规格，qty为1，否则qty为num（如果是最后一个规格且有多级）
                        int qty = 1;
                        if (unitNames.Count > 1 && j == unitNames.Count - 1)
                        {
                            qty = num;
                        }

                        unitRatios.Add(new
                        {
                            id = spec.id,
                            name = spec.F_Name,
                            rate = 1,
                            qty = qty
                        });
                    }

                    if (!unitError)
                    {
                        // 更新单位和单位比例为JSON格式
                        item["F_Unit"] = unitIds.ToJsonString();
                        item["F_Unit_Ratio"] = unitRatios.ToJsonString();
                    }
                }
            }

            // 移除F_Num字段（数据库表中没有该列）
            item.Remove("F_Num");

            // 5. 设置默认创建组织
            item["F_CreatorOrganizeId"] = _userManager.User.OrganizeId;

            // 如果有错误，添加到错误列表
            if (errorMessages.Any())
            {
                item["errorsInfo"] = string.Join(";", errorMessages);
                errorsList.Add(item);
            }
            else
            {
                processedList.Add(item);
            }
        }

        // 更新input.list为处理后的数据（只保留无错误的数据）
        input.list = processedList;

        // 调试信息：查看处理结果
        var processInfo = $"处理结果：成功{processedList.Count}条，失败{errorsList.Count}条";
        var failReasons = errorsList.Select(e => e.ContainsKey("errorsInfo") ? e["errorsInfo"]?.ToString() : "未知错误").ToList();
        
        // 关键：将type设为false，防止ImportMenuData中重新加载预览数据覆盖处理结果
        input.type = false;
        
        // 调用原有的导入方法
        var result = await _exportImportDataHelper.ImportMenuData(tInfo, input);

        // 如果有错误数据，合并到结果中
        if (errorsList.Any())
        {
            var resultDict = result as Dictionary<string, object>;
            if (resultDict != null && resultDict.ContainsKey("failResult"))
            {
                var failResult = resultDict["failResult"] as List<Dictionary<string, object>>;
                if (failResult != null)
                {
                    failResult.AddRange(errorsList);
                }
                else
                {
                    resultDict["failResult"] = errorsList;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 导入数据的错误报告.
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    [HttpPost("ImportExceptionData")]
    [UnitOfWork]
    public async Task<dynamic> ExportExceptionData([FromBody] DataImportInput list)
    {
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AGoodsEntity>(new AGoodsEntity()));
        
        // 手动添加F_Unit_Ratio字段（该字段没有CodeGenUpload特性，需要手动添加）
        if (!fieldList.Any(f => f.__vModel__ == "F_Unit_Ratio"))
        {
            fieldList.Add(new FieldsModel
            {
                __vModel__ = "F_Unit_Ratio",
                __config__ = new ConfigModel
                {
                    label = "单位比例(F_Unit_Ratio)",
                    jnpfKey = "input",
                    tableName = "a_goods"
                }
            });
        }
        
        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AGoodsEntity>();
        List<DbTableRelationModel> tables = new List<DbTableRelationModel>() { ExportImportDataHelper.GetTableRelation(entityInfo, "1") };
        DbLinkEntity link = _dataBaseManager.GetTenantDbLink(_userManager.TenantId, _userManager.TenantDbName); // 当前数据库连接
        var tInfo = new TemplateParsingBase(link, fieldList, tables, "F_Id", 2, 1, uploaderKey.ToList(), "1", true);
        tInfo.FullName = _controlParsing.GetModuleNameById(list.menuId);

        // 错误数据
        tInfo.selectKey.Add("errorsInfo");
        tInfo.AllFieldsModel.Add(new FieldsModel() { __vModel__ = "errorsInfo", __config__ = new ConfigModel() { label = "异常原因" } });
        for (var i = 0; i < list.list.Count(); i++) list.list[i].Add("id", i);

        var result = ExportImportDataHelper.GetCreateFirstColumnsHeader(tInfo.selectKey, list.list, paramList);
        var firstColumns = result.Item1;
        var resultList = result.Item2;
        var fName = string.Format("{0}错误报告{1:yyyyMMddHHmmss}", tInfo.FullName, DateTime.Now);
        _cacheManager.Set(fName + ".xls", string.Empty);
        return firstColumns.Any()
            ? await _exportImportDataHelper.ExcelCreateModel(tInfo, resultList, fName, firstColumns)
            : await _exportImportDataHelper.ExcelCreateModel(tInfo, resultList, fName);
    }


    /// <summary>
    /// 下载模板.
    /// </summary>
    /// <returns></returns>
    [HttpGet("UnitTemplateDownload")]
    public async Task<dynamic> UnitTemplateDownload()
    {
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<AGoodsUnitEntity>(new AGoodsUnitEntity()));
        List<Dictionary<string, object>>? dataList = new List<Dictionary<string, object>>();

        // 赋予默认值
        var dicItem = ExportImportDataHelper.GetTemplateHeader<AGoodsUnitEntity>(new AGoodsUnitEntity(), 1);

        dicItem.Add("id", "id");
        dataList.Add(dicItem);

        var excelName = string.Format("商品单位导入模板");
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetTemplateExport(excelName, string.Join(",", unitKey), _userManager.UserId, dataList, fieldList, paramList);
    }


    /// <summary>
    /// 导入商品单位.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("ImportUnitGoods")]
    [UnitOfWork]
    public async Task ImportUnitGoods(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw Oops.Oh(ErrorCode.COM1018, "请选择文件");
        }

        // 读取Excel文件到DataTable
        DataTable excelData;
        using (var stream = file.OpenReadStream())
        {
            excelData = ExcelImportHelper.ToDataTable(file.FileName, stream);
        }

        if (excelData == null || excelData.Rows.Count == 0)
        {
            throw Oops.Oh(ErrorCode.COM1018, "文件内容为空");
        }

        var errors = new List<string>();
        var successCount = 0;

        // 从第1行开始读取数据（第0行是表头）
        for (int i = 1; i < excelData.Rows.Count; i++)
        {
            var row = excelData.Rows[i];
            
            // 读取商品编号
            var goodsNo = row["F_GoodsNo"]?.ToString()?.Trim();
            if (string.IsNullOrEmpty(goodsNo))
            {
                errors.Add($"第{i + 1}行：商品编号为空");
                continue;
            }

            // 读取单位
            var unit = row["F_Unit"]?.ToString()?.Trim();
            if (string.IsNullOrEmpty(unit))
            {
                errors.Add($"第{i + 1}行：单位为空");
                continue;
            }

            // 读取数量
            var numStr = row["F_Num"]?.ToString()?.Trim();
            int num = 1; // 默认值为1
            if (!string.IsNullOrEmpty(numStr) && !int.TryParse(numStr, out num))
            {
                errors.Add($"第{i + 1}行：数量格式不正确");
                continue;
            }

            // 根据商品编号查询商品
            var goods = await _repository.AsQueryable()
                .Where(it => it.F_GoodsNo == goodsNo && it.DeleteMark == null)
                .FirstAsync();

            if (goods == null)
            {
                errors.Add($"第{i + 1}行：商品编号[{goodsNo}]不存在");
                continue;
            }

            // 按 "/" 拆分单位
            var unitNames = unit.Split('/').Select(u => u.Trim()).Where(u => !string.IsNullOrEmpty(u)).ToList();
            if (unitNames.Count == 0)
            {
                errors.Add($"第{i + 1}行：单位格式不正确");
                continue;
            }

            // 在规格表中查找每个单位
            var unitIds = new List<string>();
            var unitRatios = new List<object>();
            bool skipThisRow = false;

            for (int j = 0; j < unitNames.Count; j++)
            {
                var spec = await _repositorygg.AsQueryable()
                    .Where(it => it.F_Name == unitNames[j] && it.DeleteMark == null)
                    .FirstAsync();

                if (spec == null)
                {
                    errors.Add($"第{i + 1}行：规格[{unitNames[j]}]不存在");
                    skipThisRow = true;
                    break;
                }

                unitIds.Add(spec.id);

                // 如果只有一级规格，qty为1，否则qty为num（如果是最后一个规格且有多级）
                int qty = 1;
                if (unitNames.Count > 1 && j == unitNames.Count - 1)
                {
                    qty = num;
                }

                unitRatios.Add(new
                {
                    id = spec.id,
                    name = spec.F_Name,
                    rate = 1,
                    qty = qty
                });
            }

            if (skipThisRow)
            {
                continue;
            }

            // 更新商品的单位和单位比例
            var updateResult = await _repository.AsUpdateable()
                .SetColumns(it => new AGoodsEntity
                {
                    F_Unit = unitIds.ToJsonString(),
                    F_Unit_Ratio = unitRatios.ToJsonString()
                })
                .Where(it => it.id == goods.id)
                .ExecuteCommandAsync();

            if (updateResult > 0)
            {
                successCount++;
            }
            else
            {
                errors.Add($"第{i + 1}行：商品编号[{goodsNo}]更新失败");
            }
        }

        if (errors.Any())
        {
            throw Oops.Oh($"导入完成，成功{successCount}条，失败{errors.Count}条：\n{string.Join("\n", errors)}");
        }
    }


    /// <summary>
    /// 新建商品管理.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AGoodsCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AGoodsEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        
        // 自动生成商品编号逻辑：前缀 sp + yyyyMMddHH，后缀 4 位序号（例如 sp20260101070001）
        if (string.IsNullOrEmpty(entity.F_GoodsNo))
        {
            // 查询配置表中id为2008738298582929408的数据
            var configData = await _repositorycs.AsQueryable()
                .Where(it => it.id.Equals("2008738298582929408"))
                .FirstAsync();

            if (configData != null && configData.F_Value == "1")
            {
                var prefix = "SP" + DateTime.Now.ToString("yyyyMMdd");

                // 查询数据库中已有相同前缀的编号
                var existingNos = await _repository.AsQueryable()
                    .Where(it => it.F_GoodsNo != null && it.F_GoodsNo.StartsWith(prefix) && it.DeleteMark == null)
                    .Select(it => it.F_GoodsNo)
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
                entity.F_GoodsNo = prefix + nextSeq.ToString("D4");
            }
            // F_Value为0或配置不存在时，不自动生成
        }

        //判断 商品编码是否重复
        if (await _repository.IsAnyAsync(it => it.F_GoodsNo.Equals(input.F_GoodsNo) && it.FlowId == null && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "商品编号");

        //判断 商品名称和规格是否重复
        if (await _repository.IsAnyAsync(it => it.F_GoodsName == input.F_GoodsName && it.F_Specification == input.F_Specification && !string.IsNullOrEmpty(it.F_Specification) && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1018, "商品名称和商品规格组合重复!");

        //if(!string.IsNullOrEmpty(entity.F_CategoryId))
        //{
        //    var categoryList = entity.F_CategoryId.ToObject<List<string>>();
        //    var code = "";
        //    foreach (var categoryId in categoryList)
        //    {
        //        if(code == "")
        //        {
        //            code = (await _repositoryCategory.GetFirstAsync(it => it.id == categoryId)).F_Code;
        //        }
        //        else
        //        {
        //            code = code + "-" + (await _repositoryCategory.GetFirstAsync(it => it.id == categoryId)).F_Code;
        //        }
        //        if (await _repository.IsAnyAsync(it => it.F_CategoryId == categoryId && it.DeleteMark == null))
        //        {
        //            entity.F_CategoryId = string.Join(",", entity.F_CategoryId);
        //            break;
        //        }
        //    }
        //    entity.F_GoodsNo = code + "-" + entity.F_GoodsNo;
        //}

        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 更新商品管理.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AGoodsUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AGoodsEntity>();
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_GoodsNo.Equals(input.F_GoodsNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "商品编号");

        if (string.IsNullOrEmpty(entity.F_GoodsNo))
        {
            entity.F_GoodsNo = oldEntity.F_GoodsNo;
        }
        //判断 商品名称和规格是否重复
        if (await _repository.IsAnyAsync(it => it.F_GoodsName == input.F_GoodsName && it.F_Specification == input.F_Specification &&  !string.IsNullOrEmpty(it.F_Specification) && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1018, "商品名称和商品规格组合重复!");
        var isOk = 0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new {
                it.F_GoodsNo,
                it.F_GoodsName,
                it.F_CategoryId,
                it.F_Unit,
                it.F_Unit_Ratio,
                it.F_Material,
                it.F_SalePrice,
                it.F_CostPrice,
                it.F_CommissionFixed,
                it.F_Source,
                it.F_OutsourcePrice,
                it.F_SupplierId,
                it.F_Specification,
                it.F_InspectRule,
                it.F_StockUpperLimit,
                it.F_StockLowerLimit,
                it.F_Encoding,
                it.F_Image,
                it.F_AttachmentUrl,
                it.F_Remark,
                it.F_Intro,
                it.F_Type,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除商品管理.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除商品管理.
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
            // 批量删除商品管理
            await _repository.AsDeleteable().In(it => it.id, input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 验证商品名称是否重复.
    /// </summary>
    /// <param name="input">验证参数.</param>
    /// <returns></returns>
    [HttpPost("ValidateGoodsName")]
    public async Task ValidateGoodsName([FromBody] ValidateGoodsNameInput input)
    {
        // 查询配置表中id为2008738234443632640的数据
        var configData = await _repositorycs.AsQueryable()
            .Where(it => it.id.Equals("2008738234443632640"))
            .FirstAsync();

        if (configData != null && configData.F_Value == "1")
        {
            // 需要检查重复性
            var existingGoods = await _repository.AsQueryable()
                .Where(it => it.F_GoodsName.Equals(input.F_GoodsName) && it.DeleteMark == null)
                .FirstAsync();

            if (existingGoods != null)
            {
                throw Oops.Oh(ErrorCode.COM1024);
            }
        }
        // F_Value为0或配置不存在时，允许录入
    }

    /// <summary>
    /// 商品管理详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AGoodsDetailOutput
            {
                id = it.id,
                F_GoodsNo = it.F_GoodsNo,
                F_GoodsName = it.F_GoodsName,
                F_CategoryId = it.F_CategoryId,
                F_Unit = it.F_Unit,
                F_Unit_Ratio = it.F_Unit_Ratio,
                F_Material = it.F_Material,
                F_SalePrice = it.F_SalePrice,
                F_CostPrice = it.F_CostPrice,
                F_CommissionFixed = it.F_CommissionFixed,
                F_Source = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Source) && dic.DictionaryTypeId.Equals("2008448951216377856")).Select(dic => dic.FullName),
                F_OutsourcePrice = it.F_OutsourcePrice,
                F_SupplierId = it.F_SupplierId,
                F_Specification = it.F_Specification,
                F_InspectRule = it.F_InspectRule,
                F_StockUpperLimit = it.F_StockUpperLimit.ToString(),
                F_StockLowerLimit = it.F_StockLowerLimit.ToString(),
                F_Encoding = it.F_Encoding,
                F_Image = it.F_Image,
                F_AttachmentUrl = it.F_AttachmentUrl,
                F_Remark = it.F_Remark,
                F_Intro = it.F_Intro,
                F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2029481827470807040")).Select(dic => dic.FullName),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            var F_CategoryIdData = await _dataInterfaceService.GetDynamicList("F_CategoryId", "2008414575283802112", "F_Id", "F_Name", "children");
            if (item.F_CategoryId != null)
            {
                var F_CategoryIdCascader = item.F_CategoryId.ToObject<List<string>>();
                item.F_CategoryId = string.Join("/", F_CategoryIdData.FindAll(it => F_CategoryIdCascader.Contains(it.id)).Select(s => s.fullName));
            }


            // 单位（级联路径，如 箱子/盒/瓶，直接查 AGoodsSpecificationEntity）
            if (!string.IsNullOrEmpty(item.F_Unit))
            {
                var unitIds = item.F_Unit.ToObject<List<string>>();
                var specData = await _repositorygg.AsSugarClient().Queryable<AGoodsSpecificationEntity>()
                    .Where(it => unitIds.Contains(it.id) && it.DeleteMark == null)
                    .Select(it => new { it.id, it.F_Name })
                    .ToListAsync();
                var specMap = specData.ToDictionary(it => it.id, it => it.F_Name ?? "");
                item.F_Unit = string.Join("/", unitIds.Where(id => specMap.ContainsKey(id)).Select(id => specMap[id]));
            }

            // 单位比例（F_Unit_Ratio JSON转换为 1箱子/10盒/2瓶 格式）
            if (!string.IsNullOrEmpty(item.F_Unit_Ratio))
            {
                var ratioList = item.F_Unit_Ratio.ToObject<List<UnitRatioItem>>();
                if (ratioList != null && ratioList.Any())
                {
                    var unitIds = ratioList.Select(it => it.id).ToList();
                    var specData = await _repositorygg.AsSugarClient().Queryable<AGoodsSpecificationEntity>()
                        .Where(it => unitIds.Contains(it.id) && it.DeleteMark == null)
                        .Select(it => new { it.id, it.F_Name })
                        .ToListAsync();
                    var specMap = specData.ToDictionary(it => it.id, it => it.F_Name ?? "");
                    var ratioDisplay = ratioList
                        .Where(it => specMap.ContainsKey(it.id))
                        .Select(it => it.qty + specMap[it.id]);
                    item.F_Unit_Ratio = string.Join("/", ratioDisplay);
                }
            }

            // 供应商ID
            if (item.F_SupplierId != null)
            {
                var F_SupplierIdData = await _dataInterfaceService.GetDynamicList("F_SupplierId", "2008457397298925568", "F_Id", "FSupplierName", "");
                item.F_SupplierId = F_SupplierIdData.Find(it => it.id.Equals(item.F_SupplierId))?.fullName;
            }

            if (item.F_Image != null)
            {
                item.F_Image = item.F_Image.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_Image = new List<FileControlsModel>();
            }

            if (item.F_AttachmentUrl != null)
            {
                item.F_AttachmentUrl = item.F_AttachmentUrl.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_AttachmentUrl = new List<FileControlsModel>();
            }

        });
        return data.FirstOrDefault();
    }
    /// <summary>
    /// 单位比例项（用于解析 F_Unit_Ratio JSON）
    /// </summary>
    public class UnitRatioItem
    {
        public string id { get; set; }
        public string name { get; set; }
        public int rate { get; set; }
        public int qty { get; set; }
    }

    /// <summary>
    /// 通过商品获取单位
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("GoodsUnit/{goodsId}")]
    public async Task<dynamic> GetGoodsUnit(string goodsId)
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null && it.id == goodsId)
            .Select(it => it.F_Unit).FirstAsync();

        var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
        if (!string.IsNullOrEmpty(data))
        {
            var F_UnitIdCascader = data.ToObject<List<string>>();
            data = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));
        }

        return data;
    }

    /// <summary>
    /// 通过商品获取单位和数量
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("GoodsUnitNum")]
    public async Task<dynamic> GetGoodsUnitNum(string goodsId)
    {
        var unit = "";
        var num = 1;
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null && it.id == goodsId)
            .Select(it => new
            {
                F_Unit = it.F_Unit,
                F_Unit_Ratio = it.F_Unit_Ratio,
            }).FirstAsync();

        var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
        if (!string.IsNullOrEmpty(data.F_Unit))
        {
            var F_UnitIdCascader = data.F_Unit.ToObject<List<string>>();
            unit = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));
        }

        if (!string.IsNullOrEmpty(data.F_Unit_Ratio))
        {
            var unitRatios = data.F_Unit_Ratio.ToObject<List<UnitRatioItem>>();
            if (unitRatios.Count() > 0)
            {
                if (unitRatios.Count() > 1)
                {
                    num = unitRatios[1].qty;
                }
                else
                {
                    num = unitRatios[0].qty;
                }
            }
            else
            {
                num = 1;
            }
        }
        else
        {
            num = 1;
        }

        return new {
            data = new { F_Unit = unit, F_Num = num }
        };
    }

    /// <summary>
    /// 通过商品获取单位和数量(前端使用)
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("GoodsUnitNumWeb/{goodsId}")]
    public async Task<dynamic> GetGoodsUnitNumWeb(string goodsId)
    {
        var unit = "";
        var num = 1;
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null && it.id == goodsId)
            .Select(it => new {
                F_Unit = it.F_Unit,
                F_Unit_Ratio = it.F_Unit_Ratio,
            }).FirstAsync();

        var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
        if (!string.IsNullOrEmpty(data.F_Unit))
        {
            var F_UnitIdCascader = data.F_Unit.ToObject<List<string>>();
            unit = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));
        }

        if (!string.IsNullOrEmpty(data.F_Unit_Ratio))
        {
            var unitRatios = data.F_Unit_Ratio.ToObject<List<UnitRatioItem>>();
            if (unitRatios.Count() > 0)
            {
                if (unitRatios.Count() > 1)
                {
                    num = unitRatios[1].qty;
                }
                else
                {
                    num = unitRatios[0].qty;
                }
            }
            else
            {
                num = 1;
            }
        }
        else
        {
            num = 1;
        }

        return new {
            data = new { F_Unit = unit, F_Num = num }
        };
    }

    /// <summary>
    /// 弹窗获取商品列表(不包括原材料0).接口id:2029803158963884032
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("PopupListest")]
    public async Task<dynamic> GetPopupListest(string contractId)
    {
        var ctcItemList = new List<string>();

        var data = new List<AGoodsListOutput>();
        if (!string.IsNullOrEmpty(contractId))
        {
            ctcItemList = await _repositoryCtcItem.AsQueryable().Where(it => it.DeleteMark == null && it.F_ContractId == contractId).Select(it => it.F_CustomerId).ToListAsync();

            data = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(contractId), it => ctcItemList.Contains(it.id) || it.F_Type == "1")
            .Where(it => it.DeleteMark == null)
            .Where(it => it.F_Type != "0") // 排除原材料
            .Where(it => it.FlowId == null)
            .LeftJoin<ACtcContractItemEntity>((it, c) => c.F_CustomerId == it.id && c.F_ContractId == contractId)
            .Select((it, c) => new AGoodsListOutput
            {
                id = it.id,
                F_GoodsNo = it.F_GoodsNo,
                F_GoodsName = it.F_GoodsName + "-" + it.F_GoodsNo,
                F_CategoryId = it.F_CategoryId,
                F_Unit = it.F_Unit,
                F_Unit_Ratio = it.F_Unit_Ratio,
                F_Specification = it.F_Specification,
                F_InspectRule = it.F_InspectRule,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_Remark = it.F_Remark,
                F_InventoryNum = 0,
                F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2029481827470807040")).Select(dic => dic.FullName),
                F_RoutingId = c.F_RoutingId,
                F_PlanNum = c.F_PlanNum,
                F_DeliveryDate = c.F_DeliveryDate,
                F_Priority = c.F_Priority,
                F_BomId = c.F_BomId,
                F_DrawingFiles = c.F_DrawingFiles,
                F_Category = c.F_Category
            }).MergeTable()
            .OrderBy(it => it.F_Type)
            .OrderBy("F_CreatorTime desc") // 按创建时间降序
            .ToListAsync();
        }
        else
        {
            data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Where(it => it.F_Type != "0") // 排除原材料
            .Where(it => it.FlowId == null)
            .Select(it => new AGoodsListOutput
            {
                id = it.id,
                F_GoodsNo = it.F_GoodsNo,
                F_GoodsName = it.F_GoodsName + "-" + it.F_GoodsNo,
                F_CategoryId = it.F_CategoryId,
                F_Unit = it.F_Unit,
                F_Unit_Ratio = it.F_Unit_Ratio,
                F_Specification = it.F_Specification,
                F_InspectRule = it.F_InspectRule,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_Remark = it.F_Remark,
                F_InventoryNum = 0,
                F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2029481827470807040")).Select(dic => dic.FullName),
            }).MergeTable()
            //.OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id)
            .OrderBy("F_CreatorTime desc") // 按创建时间降序
            .ToListAsync();
        }
        
            

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 商品分类ID
            //var F_CategoryIdData = await _dataInterfaceService.GetDynamicList("F_CategoryId", "2008414575283802112", "F_Id", "F_Name", "children");
            //if (item.F_CategoryId != null)
            //{
            //    var F_CategoryIdCascader = item.F_CategoryId.ToObject<List<string>>();
            //    item.F_CategoryId = string.Join("/", F_CategoryIdData.FindAll(it => F_CategoryIdCascader.Contains(it.id)).Select(s => s.fullName));
            //}
            /*item.F_InventoryNum = (await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == item.id && it.F_Num > 0).SumAsync(it => it.F_Num)) ?? 0;*/
        });

        return new {
            data = data
        };
    }



    /// <summary>
    /// 弹窗获取商品列表(不包括成品材料2).接口id:2030840782923108352
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("PopupListests")]
    public async Task<dynamic> GetPopupListests(string contractId)
    {
        var ctcItemList = new List<string>();
        var data = new List<AGoodsListOutput>();
        if (!string.IsNullOrEmpty(contractId))
        {
            ctcItemList = await _repositoryCtcItem.AsQueryable().Where(it => it.DeleteMark == null && it.F_ContractId == contractId).Select(it => it.F_CustomerId).ToListAsync();

            data = await _repository.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(contractId), it => ctcItemList.Contains(it.id))
                .Where(it => it.DeleteMark == null)
                .Where(it => it.F_Type != "2") // 排除成品材料
                .Where(it => it.FlowId == null)
                .LeftJoin<ACtcContractItemEntity>((it, c) => c.F_CustomerId == it.id && c.F_ContractId == contractId)
                .Select((it, c) => new AGoodsListOutput
                {
                    id = it.id,
                    F_GoodsNo = it.F_GoodsNo,
                    F_GoodsName = it.F_GoodsName + "-" + it.F_GoodsNo,
                    F_CategoryId = it.F_CategoryId,
                    F_Unit = it.F_Unit,
                    F_Unit_Ratio = it.F_Unit_Ratio,
                    //F_Unit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Unit) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                    F_Specification = it.F_Specification,
                    F_InspectRule = it.F_InspectRule,
                    F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    F_Remark = it.F_Remark,
                    F_InventoryNum = 0,
                    F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2029481827470807040")).Select(dic => dic.FullName),
                    F_RoutingId = c.F_RoutingId,
                    F_PlanNum = c.F_PlanNum,
                    F_DeliveryDate = c.F_DeliveryDate,
                    F_Priority = c.F_Priority,
                    F_BomId = c.F_BomId,
                    F_DrawingFiles = c.F_DrawingFiles,
                    F_Category = c.F_Category
                }).MergeTable()
                //.OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id)
                .OrderBy("F_CreatorTime desc") // 按创建时间降序
                .ToListAsync();
        }
        else
        {

            data = await _repository.AsQueryable()
                .Where(it => it.DeleteMark == null)
                .Where(it => it.F_Type != "2") // 排除成品材料
                .Where(it => it.FlowId == null)
                .Select(it => new AGoodsListOutput
                {
                    id = it.id,
                    F_GoodsNo = it.F_GoodsNo,
                    F_GoodsName = it.F_GoodsName + "-" + it.F_GoodsNo,
                    F_CategoryId = it.F_CategoryId,
                    F_Unit = it.F_Unit,
                    F_Unit_Ratio = it.F_Unit_Ratio,
                    //F_Unit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Unit) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                    F_Specification = it.F_Specification,
                    F_InspectRule = it.F_InspectRule,
                    F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    F_Remark = it.F_Remark,
                    F_InventoryNum = 0,
                    F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2029481827470807040")).Select(dic => dic.FullName),
                }).MergeTable()
                //.OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id)
                .OrderBy("F_CreatorTime desc") // 按创建时间降序
                .ToListAsync();
        }

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            // 单位（级联路径，如 箱子/盒/瓶，直接查 AGoodsSpecificationEntity）
            //if (!string.IsNullOrEmpty(item.F_Unit))
            //{
            //    var unitIds = item.F_Unit.ToObject<List<string>>();
            //    var specData = await _repositorygg.AsSugarClient().Queryable<AGoodsSpecificationEntity>()
            //        .Where(it => unitIds.Contains(it.id) && it.DeleteMark == null)
            //        .Select(it => new { it.id, it.F_Name })
            //        .ToListAsync();
            //    var specMap = specData.ToDictionary(it => it.id, it => it.F_Name ?? "");
            //    item.F_Unit = string.Join("/", unitIds.Where(id => specMap.ContainsKey(id)).Select(id => specMap[id]));
            //}
            //item.F_InventoryNum = (await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == item.id && it.F_Num > 0).SumAsync(it => it.F_Num)) ?? 0;
        });

        return new {
            data = data
        };
    }


    /// <summary>
    /// 弹窗获取商品列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("PopupList")]
    public async Task<dynamic> GetPopupList(string contractId)
    {
        var ctcItemList = new List<string>();
        var data = new List<AGoodsListOutput>();
        if (!string.IsNullOrEmpty(contractId))
        {
            ctcItemList = await _repositoryCtcItem.AsQueryable().Where(it => it.DeleteMark == null && it.F_ContractId == contractId).Select(it => it.F_CustomerId).ToListAsync();
            data = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(contractId), it => ctcItemList.Contains(it.id))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .LeftJoin<ACtcContractItemEntity>((it, c) => c.F_CustomerId == it.id && c.F_ContractId == contractId)
            .Select((it, c) => new AGoodsListOutput
            {
                id = it.id,
                F_GoodsNo = it.F_GoodsNo,
                F_GoodsName = it.F_GoodsName + "-" + it.F_GoodsNo,
                F_CategoryId = it.F_CategoryId,
                F_Unit = it.F_Unit,
                //F_Unit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Unit) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                F_Specification = it.F_Specification,
                F_InspectRule = it.F_InspectRule,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_Remark = it.F_Remark,
                F_InventoryNum = 0,
                F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2029481827470807040")).Select(dic => dic.FullName),
                F_RoutingId = c.F_RoutingId,
                F_PlanNum = c.F_PlanNum,
                F_DeliveryDate = c.F_DeliveryDate,
                F_Priority = c.F_Priority,
                F_BomId = c.F_BomId,
                F_DrawingFiles = c.F_DrawingFiles,
                F_Category = c.F_Category
            }).MergeTable()
            //.OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id)
            .OrderBy("F_CreatorTime desc") // 按创建时间降序
            .ToListAsync();
        }
        else
        {
            data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new AGoodsListOutput
            {
                id = it.id,
                F_GoodsNo = it.F_GoodsNo,
                F_GoodsName = it.F_GoodsName + "-" + it.F_GoodsNo,
                F_CategoryId = it.F_CategoryId,
                F_Unit = it.F_Unit,
                //F_Unit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Unit) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                F_Specification = it.F_Specification,
                F_InspectRule = it.F_InspectRule,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_Remark = it.F_Remark,
                F_InventoryNum = 0,
                F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Type) && dic.DictionaryTypeId.Equals("2029481827470807040")).Select(dic => dic.FullName),
            }).MergeTable()
            //.OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id)
            .OrderBy("F_CreatorTime desc") // 按创建时间降序
            .ToListAsync();
        }
        

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 商品分类ID
            //var F_CategoryIdData = await _dataInterfaceService.GetDynamicList("F_CategoryId", "2008414575283802112", "F_Id", "F_Name", "children");
            //if (item.F_CategoryId != null)
            //{
            //    var F_CategoryIdCascader = item.F_CategoryId.ToObject<List<string>>();
            //    item.F_CategoryId = string.Join("/", F_CategoryIdData.FindAll(it => F_CategoryIdCascader.Contains(it.id)).Select(s => s.fullName));
            //}
            item.F_InventoryNum = (await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == item.id && it.F_Num > 0).SumAsync(it => it.F_Num)) ?? 0;
        });

        return new {
            data = data
        };
    }


    /// <summary>
    /// 通过库存编码获取商品信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("GoodsInventoryNo")]
    public async Task<dynamic> GetGoodsInventoryNo(string goodsNo)
    {
        var goodsId = await _repositoryInventory.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_Code == goodsNo)
            .Select(it => it.F_GoodsId).FirstAsync();

        if (goodsId == null)
        {
            goodsId = await _repositoryInventoryQr.AsQueryable()
                .Where(it => it.DeleteMark == null && it.F_Code == goodsNo)
                .Select(it => it.F_GoodsId).FirstAsync();
        }

        var goodsEntity = await _repository.AsQueryable().Where(it => it.id == goodsId)
            .Select(it => new AGoodsInventoryNoInput
            {
                id = it.id,
                F_InventoryNo = goodsNo,
                F_GoodsNo = it.F_GoodsNo,
                F_GoodsName = it.F_GoodsName + "-" + it.F_GoodsNo,
                F_Unit = it.F_Unit,
                F_Specification = it.F_Specification,
                F_SalePrice = it.F_SalePrice,
                F_CostPrice = it.F_CostPrice,
                F_Remark = it.F_Remark,
                F_Unit_Ratio = it.F_Unit_Ratio,
            }).FirstAsync();
        if (!string.IsNullOrEmpty(goodsEntity.F_Unit))
        {
            var unitIds = goodsEntity.F_Unit.ToObject<List<string>>();
            var specData = await _repositorygg.AsSugarClient().Queryable<AGoodsSpecificationEntity>()
                .Where(it => unitIds.Contains(it.id) && it.DeleteMark == null)
                .Select(it => new { it.id, it.F_Name })
                .ToListAsync();
            var specMap = specData.ToDictionary(it => it.id, it => it.F_Name ?? "");
            goodsEntity.F_Unit = string.Join("/", unitIds.Where(id => specMap.ContainsKey(id)).Select(id => specMap[id]));
        }

        if (!string.IsNullOrEmpty(goodsEntity.F_Unit_Ratio))
        {
            var unitRatios = goodsEntity.F_Unit_Ratio.ToObject<List<UnitRatioItem>>();
            if(unitRatios.Count() > 0)
            {
                if(unitRatios.Count() > 1)
                {
                    goodsEntity.F_Num = unitRatios[1].qty;
                }
                else
                {
                    goodsEntity.F_Num = unitRatios[0].qty;
                }
            }
            else
            {
                goodsEntity.F_Num = 1;
            }
        }
        else
        {
            goodsEntity.F_Num = 1;
        }
        return new{ data = goodsEntity };
    }

}

public class AGoodsInventoryNoInput
{
    /// <summary>
    /// 主键.
    /// </summary>
    public string? id { get; set; }

    /// <summary>
    /// 库存编号.
    /// </summary>
    public string? F_InventoryNo { get; set; }

    /// <summary>
    /// 商品编号.
    /// </summary>
    public string? F_GoodsNo { get; set; }

    /// <summary>
    /// 商品名称.
    /// </summary>
    public string? F_GoodsName { get; set; }

    /// <summary>
    /// 数量.
    /// </summary>
    public int? F_Num { get; set; }

    /// <summary>
    /// 单位比例.
    /// </summary>
    public string? F_Unit_Ratio { get; set; }

    /// <summary>
    /// 单位.
    /// </summary>
    public string? F_Unit { get; set; }

    /// <summary>
    /// 销售单价.
    /// </summary>
    public decimal? F_SalePrice { get; set; }

    /// <summary>
    /// 成本单价.
    /// </summary>
    public decimal? F_CostPrice { get; set; }

    /// <summary>
    /// 规格.
    /// </summary>
    public string? F_Specification { get; set; }

    /// <summary>
    /// 备注.
    /// </summary>
    public string F_Remark { get; set; }

}



