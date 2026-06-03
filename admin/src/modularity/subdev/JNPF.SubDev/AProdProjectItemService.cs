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
using JNPF.example.Entitys.Dto.AProdProjectItem;
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
/// 业务实现：项目商品列表.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdProjectItem", Order = 200)]
[Route("api/example/[controller]")]
public class AProdProjectItemService : IAProdProjectItemService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdProjectItemEntity> _repository;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"项目ID\",\"field\":\"F_ProjectId\"},{\"value\":\"工单编号\",\"field\":\"F_WorkOrderNo\"},{\"value\":\"商品\",\"field\":\"F_GoodsId\"},{\"value\":\"计划数\",\"field\":\"F_PlanNum\"},{\"value\":\"交货日期\",\"field\":\"F_DeliveryDate\"},{\"value\":\"优先级\",\"field\":\"F_Priority\"},{\"value\":\"BOMID\",\"field\":\"F_BomId\"},{\"value\":\"门板厚度\",\"field\":\"F_DoorPlateThickness\"},{\"value\":\"箱体板厚\",\"field\":\"F_BoxPlateThickness\"},{\"value\":\"安装板或安装梁\",\"field\":\"F_InstallPlateOrBeam\"},{\"value\":\"客户\",\"field\":\"F_CustomerName\"},{\"value\":\"锁具\",\"field\":\"F_LockSet\"},{\"value\":\"铰链\",\"field\":\"F_Hinge\"},{\"value\":\"图纸\",\"field\":\"F_DrawingFiles\"},{\"value\":\"BOM图片\",\"field\":\"F_BomImages\"},{\"value\":\"颜色\",\"field\":\"F_Color\"},{\"value\":\"类别\",\"field\":\"F_Category\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdProjectItemService"/>类型的新实例.
    /// </summary>
    public AProdProjectItemService(
        ISqlSugarRepository<AProdProjectItemEntity> repository,
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
    /// 获取项目商品列表.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .FirstAsync(it => it.F_Id.Equals(id))).Adapt<AProdProjectItemInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取项目商品列表列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdProjectItemListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AProdProjectItemListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.F_Id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProjectId), it => it.F_ProjectId.Contains(input.F_ProjectId))
            .WhereIF(!string.IsNullOrEmpty(input.F_GoodsId), it => it.F_GoodsId.Equals(input.F_GoodsId))
            .WhereIF(!string.IsNullOrEmpty(input.F_WorkOrderNo), it => it.F_WorkOrderNo.Contains(input.F_WorkOrderNo))
            .Where(authorizeWhere)
            .Select(it => new AProdProjectItemListOutput
            {
                id = it.F_Id,
                F_ProjectId = it.F_ProjectId,
                F_GoodsId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_WorkOrderNo = it.F_WorkOrderNo,
                F_PlanNum = it.F_PlanNum.ToString(),
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_Priority = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Priority) && dic.DictionaryTypeId.Equals("2013182853290004480")).Select(dic => dic.FullName),
                F_BomId = SqlFunc.Subqueryable<ABomEntity>().EnableTableFilter().Where(g => g.id == it.F_BomId && g.DeleteMark == null).Select(g => g.F_BomName),
                F_DrawingFiles = it.F_DrawingFiles,
                F_DoorPlateThickness = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_DoorPlateThickness) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_BoxPlateThickness = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_BoxPlateThickness) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_InstallPlateOrBeam = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_InstallPlateOrBeam) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_CustomerName = it.F_CustomerName,
                F_LockSet = it.F_LockSet,
                F_Hinge = it.F_Hinge,
                F_BomImages = it.F_BomImages,
                F_Color = it.F_Color,
                F_Category = it.F_Category,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            if(item.F_DrawingFiles != null)
            {
                item.F_DrawingFiles = item.F_DrawingFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_DrawingFiles = new List<FileControlsModel>();
            }

            if(item.F_BomImages != null)
            {
                item.F_BomImages = item.F_BomImages.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_BomImages = new List<FileControlsModel>();
            }

            // 类别
            var F_CategoryData = await _dataInterfaceService.GetDynamicList("F_Category", "2008414575283802112", "F_Id", "F_Name", "children");
            if (item.F_Category != null)
            {
                var F_CategoryCascader = item.F_Category.ToObject<List<string>>();
                item.F_Category = string.Join("/", F_CategoryData.FindAll(it => F_CategoryCascader.Contains(it.id)).Select(s => s.fullName));
            }

        });

        return PageResult<AProdProjectItemListOutput>.SqlSugarPageResult(data);
    }

    [HttpPost("NotList")]
    public async Task<dynamic> GetNotList([FromBody] AProdProjectItemListQueryInput input)
    {
        var data = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_ProjectId), it => it.F_ProjectId.Contains(input.F_ProjectId))
            .WhereIF(!string.IsNullOrEmpty(input.F_GoodsId), it => it.F_GoodsId.Equals(input.F_GoodsId))
            .WhereIF(!string.IsNullOrEmpty(input.F_WorkOrderNo), it => it.F_WorkOrderNo.Contains(input.F_WorkOrderNo))
            .Select(it => new AProdProjectItemListOutput
            {
                id = it.F_Id,
                F_ProjectId = it.F_ProjectId,
                F_GoodsId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_WorkOrderNo = it.F_WorkOrderNo,
                F_PlanNum = it.F_PlanNum.ToString(),
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_Priority = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Priority) && dic.DictionaryTypeId.Equals("2013182853290004480")).Select(dic => dic.FullName),
                F_BomId = SqlFunc.Subqueryable<ABomEntity>().EnableTableFilter().Where(g => g.id == it.F_BomId && g.DeleteMark == null).Select(g => g.F_BomName),
                F_DrawingFiles = it.F_DrawingFiles,
                F_DoorPlateThickness = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_DoorPlateThickness) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_BoxPlateThickness = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_BoxPlateThickness) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_InstallPlateOrBeam = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_InstallPlateOrBeam) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_CustomerName = SqlFunc.Subqueryable<ACustomerEntity>().EnableTableFilter().Where(c => c.id == it.F_CustomerName && c.DeleteMark == null).Select(c => c.F_CustomerName),
                F_LockSet = it.F_LockSet,
                F_Hinge = it.F_Hinge,
                F_BomImages = it.F_BomImages,
                F_Color = it.F_Color,
                F_Category = it.F_Category,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            if (item.F_DrawingFiles != null)
            {
                item.F_DrawingFiles = item.F_DrawingFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_DrawingFiles = new List<FileControlsModel>();
            }

            if (item.F_BomImages != null)
            {
                item.F_BomImages = item.F_BomImages.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_BomImages = new List<FileControlsModel>();
            }

            // 类别
            var F_CategoryData = await _dataInterfaceService.GetDynamicList("F_Category", "2008414575283802112", "F_Id", "F_Name", "children");
            if (item.F_Category != null)
            {
                var F_CategoryCascader = item.F_Category.ToObject<List<string>>();
                item.F_Category = string.Join("/", F_CategoryData.FindAll(it => F_CategoryCascader.Contains(it.id)).Select(s => s.fullName));
            }

        });

        return PageResult<AProdProjectItemListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建项目商品列表.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AProdProjectItemCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdProjectItemEntity>();
        entity.F_Id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdProjectItemEntity>(new AProdProjectItemEntity()));
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 更新项目商品列表.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AProdProjectItemUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdProjectItemEntity>();
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_ProjectId,
                it.F_GoodsId,
                it.F_WorkOrderNo,
                it.F_PlanNum,
                it.F_DeliveryDate,
                it.F_Priority,
                it.F_BomId,
                it.F_DrawingFiles,
                it.F_DoorPlateThickness,
                it.F_BoxPlateThickness,
                it.F_InstallPlateOrBeam,
                it.F_CustomerName,
                it.F_LockSet,
                it.F_Hinge,
                it.F_BomImages,
                it.F_Color,
                it.F_Category,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除项目商品列表.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.F_Id.Equals(id)).ExecuteCommandAsync();   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除项目商品列表.
    /// </summary>
    /// <param name="input">主键数组.</param>
    /// <returns></returns>
    [HttpPost("batchRemove")]
    [UnitOfWork]
    public async Task BatchRemove([FromBody] BatchRemoveInput input)
    {
        var entitys = await _repository.AsQueryable().In(it => it.F_Id, input.ids).ToListAsync();
        if (entitys.Count > 0)
        {
            // 批量删除项目商品列表
            await _repository.AsDeleteable().In(it => it.F_Id, input.ids).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 项目商品列表详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AProdProjectItemDetailOutput
            {
                id = it.F_Id,
                F_ProjectId = it.F_ProjectId,
                F_GoodsId = it.F_GoodsId,
                F_WorkOrderNo = it.F_WorkOrderNo,
                F_PlanNum = it.F_PlanNum,
                F_DeliveryDate = it.F_DeliveryDate.Value.ToString("yyyy-MM-dd"),
                F_Priority = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_Priority) && dic.DictionaryTypeId.Equals("2013182853290004480")).Select(dic => dic.FullName),
                F_BomId = it.F_BomId,
                F_DrawingFiles = it.F_DrawingFiles,
                F_DoorPlateThickness = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_DoorPlateThickness) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_BoxPlateThickness = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_BoxPlateThickness) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_InstallPlateOrBeam = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_InstallPlateOrBeam) && dic.DictionaryTypeId.Equals("2013183327061807104")).Select(dic => dic.FullName),
                F_CustomerName = it.F_CustomerName,
                F_LockSet = it.F_LockSet,
                F_Hinge = it.F_Hinge,
                F_BomImages = it.F_BomImages,
                F_Color = it.F_Color,
                F_Category = it.F_Category,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 商品ID
            if(item.F_GoodsId != null)
            {
                linkageParameters = new List<DataInterfaceParameter>();
                linkageParameters.Add(new DataInterfaceParameter
                {
                    field = "contractId",
                    relationField = string.Empty,
                    isSubTable = false,
                    dataType = "varchar",
                    defaultValue = "",
                    fieldName = "合同ID",
                    sourceType = 3
                });
                var F_GoodsIdData = await _dataInterfaceService.GetDynamicList("F_GoodsId", "2012085692393459712", "id", "F_GoodsName", "", linkageParameters);
                item.F_GoodsId = F_GoodsIdData.Find(it => it.id.Equals(item.F_GoodsId))?.fullName;
            }

            // BOMID
            if(item.F_BomId != null)
            {
                linkageParameters = new List<DataInterfaceParameter>();
                linkageParameters.Add(new DataInterfaceParameter
                {
                    field = "goodsId",
                    relationField = string.Empty,
                    isSubTable = false,
                    dataType = "varchar",
                    defaultValue = "",
                    fieldName = "合同ID",
                    sourceType = 3
                });
                var F_BomIdData = await _dataInterfaceService.GetDynamicList("F_BomId", "2013188646957617152", "id", "F_BomName", "", linkageParameters);
                item.F_BomId = F_BomIdData.Find(it => it.id.Equals(item.F_BomId))?.fullName;
            }

            if(item.F_DrawingFiles != null)
            {
                item.F_DrawingFiles = item.F_DrawingFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_DrawingFiles = new List<FileControlsModel>();
            }

            if(item.F_BomImages != null)
            {
                item.F_BomImages = item.F_BomImages.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_BomImages = new List<FileControlsModel>();
            }

            var F_CategoryData = await _dataInterfaceService.GetDynamicList("F_Category", "2008414575283802112", "F_Id", "F_Name", "children");
            if(item.F_Category != null)
            {
                var F_CategoryCascader = item.F_Category.ToObject<List<string>>();
                item.F_Category = string.Join("/", F_CategoryData.FindAll(it => F_CategoryCascader.Contains(it.id)).Select(s => s.fullName));
            }

        });
        return data.FirstOrDefault();
    }
}
