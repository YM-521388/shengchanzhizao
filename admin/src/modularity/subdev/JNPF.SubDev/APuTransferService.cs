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
using JNPF.example.Entitys.Dto.APuTransfer;
using JNPF.example.Entitys.Dto.APuTransferItem;
using JNPF.example.Entitys.Dto.APuTransferLog;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_pu_transfer.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuTransfer", Order = 200)]
[Route("api/example/[controller]")]
public class APuTransferService : IAPuTransferService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuTransferEntity> _repository;
    private readonly ISqlSugarRepository<APuTransferItemEntity> _repositoryItem;
    private readonly ISqlSugarRepository<APuTransferLogEntity> _repositoryLog;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<APuWarehouseEntity> _repositoryWarehouse;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"调拨日期\",\"field\":\"F_TransferDate\"},{\"value\":\"调拨人\",\"field\":\"F_TransferUserId\"},{\"value\":\"发出仓库\",\"field\":\"F_FromWarehouseId\"},{\"value\":\"收入仓库\",\"field\":\"F_ToWarehouseId\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="APuTransferService"/>类型的新实例.
    /// </summary>
    public APuTransferService(
        ISqlSugarRepository<APuWarehouseEntity> repositoryWarehouse,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<APuTransferLogEntity> repositoryLog,
        ISqlSugarRepository<APuTransferItemEntity> repositoryItem,
        ISqlSugarRepository<APuTransferEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryWarehouse = repositoryWarehouse;
        _repositoryItem = repositoryItem;
        _repositoryLog = repositoryLog;
        _repositoryInventory = repositoryInventory;
        _repository = repository;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_pu_transfer.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.APuTransferItemList)
            .Where(it => it.DeleteMark == null)
            .Select(it => new APuTransferEntity
            {
                id = it.id,
                F_TransferNo = it.F_TransferNo,
                F_TransferDate = it.F_TransferDate,
                F_TransferUserId = it.F_TransferUserId,
                F_FromWarehouseId = it.F_FromWarehouseId,
                F_ToWarehouseId = it.F_ToWarehouseId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                APuTransferItemList = it.APuTransferItemList.Where(item => item.DeleteMark == null).ToList(),
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuTransferInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFieldaf2f04, async aPuTransferItem =>
        {
            // 创建人员
            aPuTransferItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuTransferItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuTransferItem.F_CreatorTime = aPuTransferItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

    /// <summary>
    /// 获取a_pu_transfer列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuTransferListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuTransferItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuTransferLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuTransferListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_transfer"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_transfer_item"))) aPuTransferItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_transfer_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_transfer_log"))) aPuTransferLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_transfer_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuTransferEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuTransferEntity>();
        var F_TransferUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_TransferUserId").DbColumnName;
        //var F_FromWarehouseId = input.F_FromWarehouseId?.Last();
        //var F_ToWarehouseId = input.F_ToWarehouseId?.Last();

        var fromWarehouseList = new List<string>();
        if (!string.IsNullOrEmpty(input.F_FromWarehouseId?.ToString()))
        {
            fromWarehouseList = await _repositoryWarehouse.AsQueryable().Where(it => it.DeleteMark == null && it.F_Name.Contains(input.F_FromWarehouseId)).Select(it => it.id).ToListAsync();
        }

        var toWarehouseList = new List<string>();
        if (!string.IsNullOrEmpty(input.F_ToWarehouseId?.ToString()))
        {
            toWarehouseList = await _repositoryWarehouse.AsQueryable().Where(it => it.DeleteMark == null && it.F_Name.Contains(input.F_ToWarehouseId)).Select(it => it.id).ToListAsync();
        }

        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(input.F_TransferDate?.Count() > 0, it => SqlFunc.Between(it.F_TransferDate, input.F_TransferDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_TransferDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_TransferUserIdDbColumnName, input.F_TransferUserId))
            //.WhereIF(!string.IsNullOrEmpty(input.F_FromWarehouseId?.ToString()), it => it.F_FromWarehouseId.Contains(F_FromWarehouseId))
            //.WhereIF(!string.IsNullOrEmpty(input.F_ToWarehouseId?.ToString()), it => it.F_ToWarehouseId.Contains(F_ToWarehouseId))
            .WhereIF(!string.IsNullOrEmpty(input.F_FromWarehouseId?.ToString()), it => fromWarehouseList.Any(w => it.F_FromWarehouseId.Contains(w)))
            .WhereIF(!string.IsNullOrEmpty(input.F_ToWarehouseId?.ToString()), it => toWarehouseList.Any(w => it.F_ToWarehouseId.Contains(w)))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .Where(authorizeWhere)
            .WhereIF(aPuTransferItemAuthorizeWhere?.Count > 0, it => it.APuTransferItemList.Any(aPuTransferItemAuthorizeWhere))
            .WhereIF(aPuTransferLogAuthorizeWhere?.Count > 0, it => it.APuTransferLogList.Any(aPuTransferLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            // .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
             .OrderByIF(string.IsNullOrEmpty(input.sidx), "F_CreatorTime DESC, id ASC").OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new APuTransferListOutput
            {
                id = it.id,
                F_TransferNo = it.F_TransferNo,
                F_TransferDate = it.F_TransferDate.Value.ToString("yyyy-MM-dd"),
                F_TransferUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_TransferUserId)).Select(u => u.RealName),
                F_FromWarehouseId = it.F_FromWarehouseId,
                F_ToWarehouseId = it.F_ToWarehouseId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            // 发出仓库
            var F_FromWarehouseIdData = await _dataInterfaceService.GetDynamicList("F_FromWarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (item.F_FromWarehouseId != null)
            {
                var F_FromWarehouseIdCascader = item.F_FromWarehouseId.ToObject<List<string>>();
                item.F_FromWarehouseId = string.Join("/", F_FromWarehouseIdData.FindAll(it => F_FromWarehouseIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

            // 收入仓库
            var F_ToWarehouseIdData = await _dataInterfaceService.GetDynamicList("F_ToWarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (item.F_ToWarehouseId != null)
            {
                var F_ToWarehouseIdCascader = item.F_ToWarehouseId.ToObject<List<string>>();
                item.F_ToWarehouseId = string.Join("/", F_ToWarehouseIdData.FindAll(it => F_ToWarehouseIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

        });

        return PageResult<APuTransferListOutput>.SqlSugarPageResult(data);
    }


    /// <summary>
    /// 获取商品列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("GoodsList")]
    public async Task<dynamic> GetGoodsList([FromBody] APuTransferListQueryInput input)
    {
        var data = await _repositoryItem.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_TransferId), it => it.F_TransferId.Contains(input.F_TransferId))
            .Select(it => new APuTransferItemInfoOutput
            {
                F_Id = it.F_Id,
                F_CustomerId = it.F_CustomerId,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_UnitTwo = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_Unit)) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                F_Num = it.F_Num,
                F_Price = it.F_Price,
                F_SalesPrice = it.F_SalesPrice,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_Id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);
        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

        });
        return PageResult<APuTransferItemInfoOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取日志列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("LogList")]
    public async Task<dynamic> GetLogList([FromBody] APuTransferListQueryInput input)
    {
        var data = await _repositoryLog.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_TransferId), it => it.F_TransferId.Contains(input.F_TransferId))
            .Select(it => new APuTransferLogInfoOutput
            {
                F_Id = it.F_Id,
                F_Title = it.F_Title,
                F_Content = it.F_Content,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime,OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        return PageResult<APuTransferLogInfoOutput>.SqlSugarPageResult(data);
    }


    /// <summary>
    /// 新建a_pu_transfer.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] APuTransferCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuTransferEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuTransferEntity>(new APuTransferEntity()));
        ConfigModel tableFieldaf2f04Config = new ConfigModel
        {
            tableName = "a_pu_transfer_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuTransferItemEntity>(new APuTransferItemEntity())
        };
        FieldsModel tableFieldaf2f04Model = new FieldsModel()
        {
            __config__ = tableFieldaf2f04Config,
            __vModel__ = "tableFieldaf2f04"
        };
        fieldList.Add(tableFieldaf2f04Model);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        // 自动生成编号逻辑：前缀 yyyyMMdd，后缀 3 位序号
        var prefix = "KCDB" + DateTime.Now.ToString("yyyyMMdd");

        // 查询数据库中已有相同前缀的编号
        var existingNos = await _repository.AsQueryable()
            .Where(it => it.F_TransferNo != null && it.F_TransferNo.StartsWith(prefix))
            .Select(it => it.F_TransferNo)
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
        entity.F_TransferNo = prefix + nextSeq.ToString("D3");


        var aPuTransferItemEntityList = input.tableFieldaf2f04.Adapt<List<APuTransferItemEntity>>();
        if(aPuTransferItemEntityList != null)
        {
            foreach (var item in aPuTransferItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuTransferItemList = aPuTransferItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.APuTransferItemList)
            .Include(it => it.APuTransferLogList)
            .ExecuteCommandAsync();

        //日志
        var logEntity = new APuTransferLogEntity();
        logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        logEntity.F_CreatorUserId = _userManager.UserId;
        logEntity.F_Id = SnowflakeIdHelper.NextId();
        logEntity.F_TransferId = entity.id;
        logEntity.F_Title = "新增调拨单";
        logEntity.F_Content = "调拨单号：" + entity.F_TransferNo;
        await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();


        //库存
        if (aPuTransferItemEntityList != null)
        {
            foreach (var item in aPuTransferItemEntityList)
            {
                var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == entity.F_FromWarehouseId);
                if (inventory != null)
                {
                    inventory.F_Num = (inventory.F_Num - item.F_Num) < 0 ? 0 : (inventory.F_Num - item.F_Num);
                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

                    var inventoryTwo = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == entity.F_ToWarehouseId);
                    if (inventoryTwo != null)
                    {
                        inventoryTwo.F_Num += item.F_Num;
                        await _repositoryInventory.AsSugarClient().Updateable(inventoryTwo).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                    else
                    {
                        var inventoryEntity = new AGoodsInventoryEntity();
                        inventoryEntity.id = SnowflakeIdHelper.NextId();
                        inventoryEntity.F_CreatorUserId = _userManager.UserId;
                        inventoryEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        inventoryEntity.F_Num = item.F_Num;
                        inventoryEntity.F_GoodsId = item.F_CustomerId;
                        inventoryEntity.F_WarehouseId = entity.F_ToWarehouseId;
                        await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 更新a_pu_transfer.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] APuTransferUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        var entity = input.Adapt<APuTransferEntity>();

        // 获取原始数据用于日志记录
        var originalEntity = await _repository.AsQueryable()
            .Where(it => it.id == id && it.DeleteMark == null)
            .FirstAsync();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuTransferEntity>(new APuTransferEntity()));
        ConfigModel tableFieldaf2f04Config = new ConfigModel
        {
            tableName = "a_pu_transfer_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuTransferItemEntity>(new APuTransferItemEntity())
        };
        FieldsModel tableFieldaf2f04Model = new FieldsModel()
        {
            __config__ = tableFieldaf2f04Config,
            __vModel__ = "tableFieldaf2f04"
        };
        fieldList.Add(tableFieldaf2f04Model);

        var oldItemList = await _repository.AsSugarClient().Queryable<APuTransferItemEntity>().Where(it => it.F_TransferId == entity.id).Select(it => new { F_Id = it.F_Id, F_CustomerId = it.F_CustomerId, F_Num = it.F_Num }).ToListAsync();

        // 移除调拨单商品列表可能被删除数据
        if (input.tableFieldaf2f04 !=null && input.tableFieldaf2f04.Any())
            await _repository.AsSugarClient().Deleteable<APuTransferItemEntity>().Where(it => it.F_TransferId == entity.id && !input.tableFieldaf2f04.Select(it => it.F_Id).ToList().Contains(it.F_Id)).ExecuteCommandAsync();
        else
            await _repository.AsSugarClient().Deleteable<APuTransferItemEntity>().Where(it => it.F_TransferId == entity.id).ExecuteCommandAsync();

        // 新增调拨单商品列表新数据
        var aPuTransferItemEntityList = input.tableFieldaf2f04.Adapt<List<APuTransferItemEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_TransferDate,
                    it.F_TransferUserId,
                    it.F_FromWarehouseId,
                    it.F_ToWarehouseId,
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

            AddChange("【调拨日期】", oldEntity.F_TransferDate, entity.F_TransferDate);
            if (oldEntity.F_TransferUserId != entity.F_TransferUserId)
            {
                string oldName = "";
                string newName = "";
                if (oldEntity.F_TransferUserId != null)
                {
                    oldName = await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => it.Id.Equals(oldEntity.F_TransferUserId)).Select(it => it.RealName).FirstAsync();
                }
                if (entity.F_TransferUserId != null)
                {
                    newName = await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => it.Id.Equals(entity.F_TransferUserId)).Select(it => it.RealName).FirstAsync();
                }
                changeList.Add($"【调拨人】 值由 {oldName} 改为 {newName}");
            }
            AddChange("【备注】", oldEntity.F_Description, entity.F_Description);
            if (changeList.Any())
            {
                var logEntity = new APuTransferLogEntity();
                logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                logEntity.F_CreatorUserId = _userManager.UserId;
                logEntity.F_Id = SnowflakeIdHelper.NextId();
                logEntity.F_TransferId = entity.id;
                logEntity.F_Title = "修改调拨单";
                logEntity.F_Content = "调拨单号：" + entity.F_TransferNo + "；修改内容：" + string.Join("；", changeList);
                await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
            }


            //库存

            if (aPuTransferItemEntityList != null)
            {
                //删除的商品减掉库存

                // 1. 先拿到“存在”的 ID 集合
                var existIds = aPuTransferItemEntityList.Select(x => x.F_Id).ToHashSet();

                // 2. 找出“缺失”的行并统一扣库存
                var missingList = oldItemList.Where(it => !existIds.Contains(it.F_Id)).ToList();

                foreach (var missing in missingList)
                {

                    var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == missing.F_CustomerId && it.F_WarehouseId == entity.F_ToWarehouseId);
                    inventory.F_Num = (inventory.F_Num - missing.F_Num) < 0 ? 0 : (inventory.F_Num - missing.F_Num); // 扣减库存
                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

                    var inventoryTwo = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == missing.F_CustomerId && it.F_WarehouseId == entity.F_FromWarehouseId);
                    if (inventoryTwo != null)
                    {
                        inventoryTwo.F_Num += missing.F_Num;// 增加库存
                        await _repositoryInventory.AsSugarClient().Updateable(inventoryTwo).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }

                //新增、修改商品库存
                foreach (var item in aPuTransferItemEntityList)
                {
                    var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == entity.F_FromWarehouseId);

                    if (item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_TransferId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();

                       
                        //修改库存
                        if (inventory != null)
                        {
                            inventory.F_Num = (inventory.F_Num - item.F_Num) < 0 ? 0 : (inventory.F_Num - item.F_Num);
                            await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

                            var inventoryTwo = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == entity.F_ToWarehouseId);
                            if (inventoryTwo != null)
                            {
                                inventoryTwo.F_Num += item.F_Num;
                                await _repositoryInventory.AsSugarClient().Updateable(inventoryTwo).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                            }
                            else
                            {
                                var inventoryEntity = new AGoodsInventoryEntity();
                                inventoryEntity.id = SnowflakeIdHelper.NextId();
                                inventoryEntity.F_CreatorUserId = _userManager.UserId;
                                inventoryEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                                inventoryEntity.F_Num = item.F_Num;
                                inventoryEntity.F_GoodsId = item.F_CustomerId;
                                inventoryEntity.F_WarehouseId = entity.F_ToWarehouseId;
                                await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
                            }
                        }

                    }
                    else
                    {
                        // 对比子表修改前后的值，记录变更
                        var oldItem = await _repository.AsSugarClient().Queryable<APuTransferItemEntity>().FirstAsync(it => it.F_Id == item.F_Id);

                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_TransferId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

                        //修改库存
                        var num = item.F_Num - oldItem.F_Num;

                        inventory.F_Num = (inventory.F_Num - num) < 0 ? 0 : (inventory.F_Num - num);
                        await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

                        var inventoryTwo = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == entity.F_ToWarehouseId);
                        if (inventoryTwo != null)
                        {
                            inventoryTwo.F_Num = (inventoryTwo.F_Num + num) < 0 ? 0 : (inventoryTwo.F_Num + num);
                            await _repositoryInventory.AsSugarClient().Updateable(inventoryTwo).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 删除a_pu_transfer.
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
    /// 批量删除a_pu_transfer.
    /// </summary>
    /// <param name="input">主键数组.</param>
    /// <returns></returns>
    [HttpPost("batchRemove")]
    [UnitOfWork]
    public async Task BatchRemove([FromBody] BatchRemoveInput input)
    {
        var oldEntity = await _repository.GetFirstAsync(it => it.DeleteMark == null && it.id == input.ids[0]);

        var entitys = await _repository.AsQueryable().In(it => it.id, input.ids).Where(it => it.DeleteMark == null).ToListAsync();
        if (entitys.Count > 0)
        {
            // 批量删除a_pu_transfer
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

            //删除 减掉库存
            var missingList = await _repository.AsSugarClient().Queryable<APuTransferItemEntity>().Where(it => it.F_TransferId == input.ids[0]).Select(it => new { F_Id = it.F_Id, F_CustomerId = it.F_CustomerId, F_Num = it.F_Num }).ToListAsync();

            foreach (var missing in missingList)
            {
                var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == missing.F_CustomerId && it.F_WarehouseId == oldEntity.F_ToWarehouseId);
                inventory.F_Num = (inventory.F_Num - missing.F_Num) < 0 ? 0 : (inventory.F_Num - missing.F_Num); // 扣减库存
                await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

                var inventoryTwo = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == missing.F_CustomerId && it.F_WarehouseId == oldEntity.F_FromWarehouseId);
                if (inventoryTwo != null)
                {
                    inventoryTwo.F_Num += missing.F_Num;// 增加库存
                    await _repositoryInventory.AsSugarClient().Updateable(inventoryTwo).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
            }
        }
    }

    /// <summary>
    /// a_pu_transfer详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.id == id)
            .Select(it => new APuTransferDetailOutput
            {
                id = it.id,
                F_TransferNo = it.F_TransferNo,
                F_TransferDate = it.F_TransferDate.Value.ToString("yyyy-MM-dd"),
                F_TransferUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_TransferUserId)).Select(u => u.RealName),
                F_FromWarehouseId = it.F_FromWarehouseId,
                F_ToWarehouseId = it.F_ToWarehouseId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 发出仓库
            var F_FromWarehouseIdData = await _dataInterfaceService.GetDynamicList("F_FromWarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (item.F_FromWarehouseId != null)
            {
                var F_FromWarehouseIdCascader = item.F_FromWarehouseId.ToObject<List<string>>();
                item.F_FromWarehouseId = F_FromWarehouseIdData.FindAll(it => F_FromWarehouseIdCascader.Contains(it.id)).Select(s => s.fullName).FirstOrDefault();
            }

            // 收入仓库
            var F_ToWarehouseIdData = await _dataInterfaceService.GetDynamicList("F_ToWarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (item.F_ToWarehouseId != null)
            {
                var F_ToWarehouseIdCascader = item.F_ToWarehouseId.ToObject<List<string>>();
                item.F_ToWarehouseId = F_ToWarehouseIdData.FindAll(it => F_ToWarehouseIdCascader.Contains(it.id)).Select(s => s.fullName).FirstOrDefault();
            }

        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        return resData.FirstOrDefault();
    }

    
}
