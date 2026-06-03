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
using JNPF.example.Entitys.Dto.APuCheck;
using JNPF.example.Entitys.Dto.APuCheckItem;
using JNPF.example.Entitys.Dto.APuCheckLog;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;

namespace JNPF.example;

/// <summary>
/// 业务实现：a_pu_check.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuCheck", Order = 200)]
[Route("api/example/[controller]")]
public class APuCheckService : IAPuCheckService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuCheckEntity> _repository;
    private readonly ISqlSugarRepository<APuCheckItemEntity> _repositoryItem;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<APuCheckLogEntity> _repositoryLog;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositoryGoods;
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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"盘点日期\",\"field\":\"F_CheckDate\"},{\"value\":\"盘点人\",\"field\":\"F_CheckUserId\"},{\"value\":\"盘点仓库\",\"field\":\"F_WarehouseId\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"},{\"value\":\"备注\",\"field\":\"F_Description\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="APuCheckService"/>类型的新实例.
    /// </summary>
    public APuCheckService(
        ISqlSugarRepository<APuWarehouseEntity> repositoryWarehouse,
        ISqlSugarRepository<AGoodsEntity> repositoryGoods,
        ISqlSugarRepository<APuCheckItemEntity> repositoryItem,
        ISqlSugarRepository<APuCheckLogEntity> repositoryLog,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<APuCheckEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryWarehouse = repositoryWarehouse;
        _repositoryGoods = repositoryGoods;
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
    /// 获取a_pu_check.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.APuCheckItemList)
            .Select(it => new APuCheckEntity
            {
                id = it.id,
                F_CheckNo = it.F_CheckNo,
                F_CheckDate = it.F_CheckDate,
                F_CheckUserId = it.F_CheckUserId,
                F_WarehouseId = it.F_WarehouseId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                APuCheckItemList = it.APuCheckItemList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuCheckInfoOutput>();

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField91ea29, async aPuCheckItem =>
        {
            // 创建人员
            aPuCheckItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuCheckItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuCheckItem.F_CreatorTime = aPuCheckItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            aPuCheckItem.F_Differences = aPuCheckItem.F_CheckQtyDone - aPuCheckItem.F_CheckQtyBefore;

            var goodEntity = await _repositoryGoods.GetFirstAsync(it => it.id == aPuCheckItem.F_CustomerId);
            aPuCheckItem.F_Specification = goodEntity?.F_Specification;

        });
        return data;
    }

    /// <summary>
    /// 获取a_pu_check列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuCheckListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuCheckItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuCheckLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuCheckListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_check"))?.conditionalModel;
            if (allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_check_item"))) aPuCheckItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_check_item"))?.conditionalModel;
            if (allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_check_log"))) aPuCheckLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_check_log"))?.conditionalModel;
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        //var F_WarehouseId = input.F_WarehouseId?.Last();
        var warehouseList = new List<string>();
        if (!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()))
        {
            warehouseList = await _repositoryWarehouse.AsQueryable().Where(it => it.DeleteMark == null && it.F_Name.Contains(input.F_WarehouseId)).Select(it => it.id).ToListAsync();
        }
        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(input.F_CheckDate?.Count() > 0, it => SqlFunc.Between(it.F_CheckDate, input.F_CheckDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CheckDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(!string.IsNullOrEmpty(input.F_CheckUserId), it => it.F_CheckUserId.Equals(input.F_CheckUserId))
            //.WhereIF(!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()), it => it.F_WarehouseId.Contains(F_WarehouseId))
            .WhereIF(!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()), it => warehouseList.Any(w => it.F_WarehouseId.Contains(w)))
            .Where(authorizeWhere)
            .WhereIF(aPuCheckItemAuthorizeWhere?.Count > 0, it => it.APuCheckItemList.Any(aPuCheckItemAuthorizeWhere))
            .WhereIF(aPuCheckLogAuthorizeWhere?.Count > 0, it => it.APuCheckLogList.Any(aPuCheckLogAuthorizeWhere))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new APuCheckListOutput
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
            .Select(it => new APuCheckListOutput
            {
                id = it.id,
                F_CheckNo = it.F_CheckNo,
                F_CheckDate = it.F_CheckDate.Value.ToString("yyyy-MM-dd"),
                F_CheckUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(g => g.Id == it.F_CheckUserId && g.DeleteMark == null).Select(g => g.RealName),
                F_WarehouseId = it.F_WarehouseId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            // 盘点仓库
            var F_WarehouseIdData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (item.F_WarehouseId != null)
            {
                var F_WarehouseIdCascader = item.F_WarehouseId.ToObject<List<string>>();
                // 获取所有匹配的仓库名称，用逗号连接
                var warehouseNames = F_WarehouseIdData.FindAll(it => F_WarehouseIdCascader.Contains(it.id)).Select(s => s.fullName);
                item.F_WarehouseId = string.Join(",", warehouseNames);
            }

        });

        data.pagination = idsQ.pagination;

        return PageResult<APuCheckListOutput>.SqlSugarPageResult(data);
    }


    /// <summary>
    /// 获取商品列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("GoodsList")]
    public async Task<dynamic> GetGoodsList([FromBody] APuCheckListQueryInput input)
    {
        var idsQ = await _repositoryItem.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_CheckId), it => it.F_CheckId.Contains(input.F_CheckId))
            .Select(it => new APuCheckItemInfoOutput
            {
                F_Id = it.F_Id,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            //把多表结果变成“单表”再排序：
            .MergeTable()
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .ToPagedListAsync(input.currentPage, input.pageSize);

        var ids = idsQ.list.Select(x => x.F_Id).ToList();   // 20 个 long

        // 2. 再用 ids 一次性补全字段（此时只有 20 行，随便 join 18 张表都毫秒级）
        var data = await _repositoryItem.AsQueryable()
            .Where(it => ids.Contains(it.F_Id))
             .Select(it => new APuCheckItemInfoOutput
             {
                 F_Id = it.F_Id,
                 F_CustomerId = it.F_CustomerId,
                 F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                 F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                 F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_Specification),
                 F_UnitTwo = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_Unit)) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                 F_CheckQtyBefore = it.F_CheckQtyBefore,
                 F_CheckQtyDone = it.F_CheckQtyDone,
                 F_Unit = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_CustomerId && g.DeleteMark == null).Select(g => g.F_Unit),
                 F_Differences = it.F_CheckQtyDone - it.F_CheckQtyBefore,
                 F_Description = it.F_Description,
                 F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                 F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
             }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(item.F_Unit))
            {
                var F_UnitIdCascader = item.F_Unit.ToObject<List<string>>();
                if (F_UnitIdCascader.Count > 1)
                {
                    item.F_CheckQtyBeforeInfo = "1" + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName + Math.Floor(item.F_CheckQtyBefore.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                    item.F_CheckQtyDoneInfo = "1" + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName + Math.Floor(item.F_CheckQtyDone.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                    item.F_DifferencesInfo = Math.Floor(item.F_Differences.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    item.F_CheckQtyBeforeInfo = Math.Floor(item.F_CheckQtyBefore.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                    item.F_CheckQtyDoneInfo = Math.Floor(item.F_CheckQtyDone.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                    item.F_DifferencesInfo = Math.Floor(item.F_Differences.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }
        });

        data.pagination = idsQ.pagination;

        return PageResult<APuCheckItemInfoOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取日志列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("LogList")]
    public async Task<dynamic> GetLogList([FromBody] APuCheckListQueryInput input)
    {
        var idsQ = await _repositoryLog.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_CheckId), it => it.F_CheckId.Contains(input.F_CheckId))
            .Select(it => new APuCheckLogInfoOutput
            {
                F_Id = it.F_Id,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            //把多表结果变成“单表”再排序：
            .MergeTable()
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .ToPagedListAsync(input.currentPage, input.pageSize);

        var ids = idsQ.list.Select(x => x.F_Id).ToList();   // 20 个 long

        // 2. 再用 ids 一次性补全字段（此时只有 20 行，随便 join 18 张表都毫秒级）
        var data = await _repositoryLog.AsQueryable()
            .Where(it => ids.Contains(it.F_Id))
            .Select(it => new APuCheckLogInfoOutput
            {
                F_Id = it.F_Id,
                F_Title = it.F_Title,
                F_Content = it.F_Content,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
        });

        data.pagination = idsQ.pagination;

        return PageResult<APuCheckLogInfoOutput>.SqlSugarPageResult(data);
    }


    /// <summary>
    /// 新建a_pu_check.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] APuCheckCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuCheckEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuCheckEntity>(new APuCheckEntity()));
        ConfigModel tableField91ea29Config = new ConfigModel
        {
            tableName = "a_pu_check_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择库存商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuCheckItemEntity>(new APuCheckItemEntity())
        };
        FieldsModel tableField91ea29Model = new FieldsModel()
        {
            __config__ = tableField91ea29Config,
            __vModel__ = "tableField91ea29"
        };
        fieldList.Add(tableField91ea29Model);

        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
        // 自动生成编号逻辑：前缀 yyyyMMdd，后缀 3 位序号
        if (string.IsNullOrEmpty(entity.F_CheckNo))
        {
            var prefix = "PK" + DateTime.Now.ToString("yyyyMMdd");

            // 查询数据库中已有相同前缀的编号
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_CheckNo != null && it.F_CheckNo.StartsWith(prefix))
                .Select(it => it.F_CheckNo)
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
            entity.F_CheckNo = prefix + nextSeq.ToString("D3");
        }


        var aPuCheckItemEntityList = input.tableField91ea29.Adapt<List<APuCheckItemEntity>>();
        if (aPuCheckItemEntityList != null)
        {
            foreach (var item in aPuCheckItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();

            }

            entity.APuCheckItemList = aPuCheckItemEntityList;
        }


        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.APuCheckItemList)
            .Include(it => it.APuCheckLogList)
            .ExecuteCommandAsync();

        //日志
        var logEntity = new APuCheckLogEntity();
        logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        logEntity.F_CreatorUserId = _userManager.UserId;
        logEntity.F_Id = SnowflakeIdHelper.NextId();
        logEntity.F_CheckId = entity.id;
        logEntity.F_Title = "新增盘库单";
        logEntity.F_Content = "盘库单号：" + entity.F_CheckNo;
        await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();

        //库存
        if (aPuCheckItemEntityList != null)
        {
            foreach (var item in aPuCheckItemEntityList)
            {
                var warehouseList = entity.F_WarehouseId.ToObject<List<string>>();
                var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && warehouseList.Any(w => it.F_WarehouseId.Contains(w)) && it.F_Code == item.F_Code);
                if (inventory != null)
                {
                    inventory.F_Num = item.F_CheckQtyDone;
                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
            }
        }
    }

    /// <summary>
    /// 更新a_pu_check.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] APuCheckUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        var entity = input.Adapt<APuCheckEntity>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuCheckEntity>(new APuCheckEntity()));
        ConfigModel tableField91ea29Config = new ConfigModel
        {
            tableName = "a_pu_check_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择库存商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuCheckItemEntity>(new APuCheckItemEntity())
        };
        FieldsModel tableField91ea29Model = new FieldsModel()
        {
            __config__ = tableField91ea29Config,
            __vModel__ = "tableField91ea29"
        };
        fieldList.Add(tableField91ea29Model);

        var oldItemList = await _repository.AsSugarClient().Queryable<APuCheckItemEntity>().Where(it => it.F_CheckId == entity.id).Select(it => new { F_Id = it.F_Id, F_CustomerId = it.F_CustomerId, F_CheckQtyBefore = it.F_CheckQtyBefore, F_CheckQtyDone = it.F_CheckQtyDone }).ToListAsync();

        // 移除库存盘点商品列表可能被删除数据
        if (input.tableField91ea29 != null && input.tableField91ea29.Any())
            await _repository.AsSugarClient().Deleteable<APuCheckItemEntity>().Where(it => it.F_CheckId == entity.id && !input.tableField91ea29.Select(it => it.F_Id).ToList().Contains(it.F_Id)).ExecuteCommandAsync();
        else
            await _repository.AsSugarClient().Deleteable<APuCheckItemEntity>().Where(it => it.F_CheckId == entity.id).ExecuteCommandAsync();

        // 新增库存盘点商品列表新数据
        var aPuCheckItemEntityList = input.tableField91ea29.Adapt<List<APuCheckItemEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_CheckDate,
                    it.F_CheckUserId,
                    it.F_WarehouseId,
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

            AddChange("【盘点日期】", oldEntity.F_CheckDate, entity.F_CheckDate);
            if (oldEntity.F_CheckUserId != entity.F_CheckUserId)
            {
                string oldName = "";
                string newName = "";
                if (oldEntity.F_CheckUserId != null)
                {
                    oldName = await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => it.Id.Equals(oldEntity.F_CheckUserId)).Select(it => it.RealName).FirstAsync();
                }
                if (entity.F_CheckUserId != null)
                {
                    newName = await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => it.Id.Equals(entity.F_CheckUserId)).Select(it => it.RealName).FirstAsync();
                }
                changeList.Add($"【盘点人】 值由 {oldName} 改为 {newName}");
            }
            if (oldEntity.F_WarehouseId != entity.F_WarehouseId)
            {
                string oldName = "";
                string newName = "";
                if (oldEntity.F_WarehouseId != null)
                {
                    oldName = await _repository.AsSugarClient().Queryable<APuWarehouseEntity>().Where(it => it.id.Equals(oldEntity.F_WarehouseId)).Select(it => it.F_Name).FirstAsync();
                }
                if (entity.F_WarehouseId != null)
                {
                    newName = await _repository.AsSugarClient().Queryable<APuWarehouseEntity>().Where(it => it.id.Equals(entity.F_WarehouseId)).Select(it => it.F_Name).FirstAsync();
                }
                changeList.Add($"【盘点仓库】 值由 {oldName} 改为 {newName}");
            }
            AddChange("【备注】", oldEntity.F_Description, entity.F_Description);
            if (changeList.Any())
            {
                var logEntity = new APuCheckLogEntity();
                logEntity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                logEntity.F_CreatorUserId = _userManager.UserId;
                logEntity.F_Id = SnowflakeIdHelper.NextId();
                logEntity.F_CheckId = entity.id;
                logEntity.F_Title = "编辑盘库单";
                logEntity.F_Content = "盘库单号：" + entity.F_CheckNo + "；修改内容：" + string.Join("；", changeList);
                await _repositoryLog.AsSugarClient().Insertable(logEntity).ExecuteCommandAsync();
            }

            //库存
            if (aPuCheckItemEntityList != null)
            {
                //删除的商品回复盘点前库存

                // 1. 先拿到“存在”的 ID 集合
                var existIds = aPuCheckItemEntityList.Select(x => x.F_Id).ToHashSet();

                // 2. 找出“缺失”的行并统一扣库存
                var missingList = oldItemList.Where(it => !existIds.Contains(it.F_Id)).ToList();

                foreach (var missing in missingList)
                {
                    var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == missing.F_CustomerId && it.F_WarehouseId == entity.F_WarehouseId);
                    if (inventory != null)
                    {
                        inventory.F_Num = missing.F_CheckQtyBefore;
                        await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }

                //新增、修改商品库存
                foreach (var item in aPuCheckItemEntityList)
                {
                    if (item.F_Id.IsNullOrEmpty())
                    {
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_CheckId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }
                    else
                    {
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_CheckId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

                    }
                    var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == item.F_CustomerId && it.F_WarehouseId == entity.F_WarehouseId);

                    //修改库存
                    inventory.F_Num = item.F_CheckQtyDone;
                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
            }

        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 删除a_pu_check.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [UnitOfWork]
    public async Task Delete(string id)
    {
        if (!await _repository.IsAnyAsync(it => it.id == id))
        {
            throw Oops.Oh(ErrorCode.COM1005);
        }

        var entity = await _repository.GetFirstAsync(it => it.id.Equals(id));
        await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
    }

    /// <summary>
    /// 批量删除a_pu_check.
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
            // 批量删除a_pu_check
            await _repository.AsDeleteable().In(it => it.id, input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

            //删除的商品回复盘点前库存

            var missingList = await _repository.AsSugarClient().Queryable<APuCheckItemEntity>().Where(it => it.F_CheckId == input.ids[0]).Select(it => new { F_Id = it.F_Id, F_CustomerId = it.F_CustomerId, F_CheckQtyBefore = it.F_CheckQtyBefore }).ToListAsync();

            foreach (var missing in missingList)
            {
                var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == missing.F_CustomerId && it.F_WarehouseId == oldEntity.F_WarehouseId);
                if (inventory != null)
                {
                    inventory.F_Num = missing.F_CheckQtyBefore;
                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
            }
        }
    }

    /// <summary>
    /// a_pu_check详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.id == id)
            .Select(it => new APuCheckDetailOutput
            {
                id = it.id,
                F_CheckNo = it.F_CheckNo,
                F_CheckDate = it.F_CheckDate.Value.ToString("yyyy-MM-dd"),
                F_CheckUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CheckUserId)).Select(u => u.RealName),
                F_WarehouseId = it.F_WarehouseId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 盘点仓库
            var F_WarehouseIdData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (item.F_WarehouseId != null)
            {
                var F_WarehouseIdCascader = item.F_WarehouseId.ToObject<List<string>>();
                // 获取所有匹配的仓库名称，用逗号连接
                var warehouseNames = F_WarehouseIdData.FindAll(it => F_WarehouseIdCascader.Contains(it.id)).Select(s => s.fullName);
                item.F_WarehouseId = string.Join(",", warehouseNames);
            }

        });

        return data.FirstOrDefault();
    }
}
