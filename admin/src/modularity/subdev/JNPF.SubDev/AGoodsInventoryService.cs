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
using JNPF.Systems.Entitys.System;
using JNPF.example.Entitys.Dto.AGoodsInventory;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.DatabaseAccessor;
using JNPF.Common.Dtos;
using System.Reactive;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.Data;
using JNPF.Common.Helper;

namespace JNPF.example;

/// <summary>
/// 业务实现：商品库存.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AGoodsInventory", Order = 200)]
[Route("api/example/[controller]")]
public class AGoodsInventoryService : IAGoodsInventoryService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repository;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositoryGoods;
    private readonly ISqlSugarRepository<APuWarehouseEntity> _repositoryWarehouse;
    private readonly ISqlSugarRepository<DictionaryDataEntity> _repositoryDictionaryData;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"商品\",\"field\":\"F_GoodsId\"},{\"value\":\"仓库\",\"field\":\"F_WarehouseId\"},{\"value\":\"库存数量\",\"field\":\"F_Num\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AGoodsInventoryService"/>类型的新实例.
    /// </summary>
    public AGoodsInventoryService(
        ISqlSugarRepository<APuWarehouseEntity> repositoryWarehouse,
        ISqlSugarRepository<AGoodsEntity> repositoryGoods,
        ISqlSugarRepository<AGoodsInventoryEntity> repository,
        ISqlSugarRepository<DictionaryDataEntity> repositoryDictionaryData,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryWarehouse = repositoryWarehouse;
        _repositoryGoods = repositoryGoods;
        _repository = repository;
        _repositoryDictionaryData = repositoryDictionaryData;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取商品库存.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new AGoodsInventoryEntity
            {
                id = it.id,
                F_GoodsId = it.F_GoodsId,
                F_WarehouseId = it.F_WarehouseId,
                F_Num = it.F_Num,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AGoodsInventoryInfoOutput>(); 

            return data;
    }


    /// <summary>
    /// 获取商品库存列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("GoodsAllList")]
    public async Task<dynamic> GetGoodsAllList([FromBody] AGoodsInventoryListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AGoodsInventoryListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var warehouseList = new List<string>();
        if (!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()))
        {
            warehouseList = await _repositoryWarehouse.AsQueryable().Where(it => it.DeleteMark == null && it.F_Name.Contains(input.F_WarehouseId)).Select(it => it.id).ToListAsync();
        }

        // 第一步：查询原始数据（不分页）
        var rawData = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_GoodsId), it => it.F_GoodsId.Equals(input.F_GoodsId))
            .WhereIF(!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()), it => warehouseList.Any(w => it.F_WarehouseId.Contains(w)))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .LeftJoin<AGoodsEntity>((it, g) => g.id.Equals(it.F_GoodsId) && g.DeleteMark == null)
            .Select((it, g) => new AGoodsInventoryListOutput
            {
                id = it.id,
                F_GoodsId = g.id,
                F_GoodsName = g.F_GoodsName,
                F_GoodsNo = g.F_GoodsNo,
                F_Unit = g.F_Unit,
                F_WarehouseId = it.F_WarehouseId,
                F_Num = it.F_Num,
            })
            .ToListAsync();

        // ===================== 核心修改：分组统计 =====================
        var groupedData = rawData
            .GroupBy(x => new {
                WarehouseFirstLevel = x.F_WarehouseId?.ToObject<List<string>>().FirstOrDefault(),
                x.F_GoodsId
            })
            .Select(g => new AGoodsInventoryListOutput
            {
                F_GoodsId = g.Key.F_GoodsId,
                F_GoodsName = g.First().F_GoodsName,
                F_GoodsNo = g.First().F_GoodsNo,
                F_Unit = g.First().F_Unit,
                F_WarehouseListId = new List<string> { g.Key.WarehouseFirstLevel },
                F_Count = g.Count(),
                F_Num = g.Sum(x => x.F_Num),
            })
            .ToList();

        // ===================== 排序 + 分页 =====================
        var pagedData = groupedData
            .OrderBy(it => it.F_GoodsNo)
            .Skip((input.currentPage - 1) * input.pageSize)
            .Take(input.pageSize)
            .ToList();

        // ===================== 处理名称显示 =====================
        await _repository.AsSugarClient().ThenMapperAsync(pagedData, async item =>
        {
            var warehouseData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            var wareHouseIds = item.F_WarehouseListId.ToObject<List<string>>();
            item.F_WarehouseId = string.Join("/", warehouseData.FindAll(it => wareHouseIds.Contains(it.id)).Select(s => s.fullName));

            var unitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(item.F_Unit))
            {
                var unitIds = item.F_Unit.ToObject<List<string>>();
                if (unitIds.Count > 1)
                {
                    item.F_NumInfo = item.F_Count + unitData.First(it => it.id == unitIds[0]).fullName + "/" +
                                     Math.Floor(item.F_Num.Value) +
                                     unitData.First(it => it.id == unitIds[1]).fullName;
                }
                else
                {
                    item.F_NumInfo = Math.Floor(item.F_Num.Value).ToString() +
                                     unitData.First(it => it.id == unitIds[0]).fullName;
                }
            }
        });

        return new {
            currentPage = input.currentPage,
            pageSize = input.pageSize,
            total = groupedData.Count,
            list = pagedData
        };
    }

    /// <summary>
    /// 获取商品库存阈值告警列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("AlertList")]
    public async Task<dynamic> GetAlertList([FromBody] AGoodsInventoryAlertQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AGoodsInventoryListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        // 第一步：从商品表查询设置了库存阈值且未删除的有效商品
        var goodsWithThreshold = await _repositoryGoods.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Where(it => it.F_StockUpperLimit != null && it.F_StockUpperLimit > 0)
            .Where(it => it.F_StockLowerLimit != null && it.F_StockLowerLimit >= 0)
            .WhereIF(!string.IsNullOrEmpty(input.F_GoodsName), it => it.F_GoodsName.Contains(input.F_GoodsName))
            .Where(authorizeWhere)
            .Select(it => new {
                id = it.id,
                F_GoodsId = it.id,
                F_GoodsNo = it.F_GoodsNo,
                F_GoodsName = it.F_GoodsName,
                F_Unit = it.F_Unit,
                F_Specification = it.F_Specification,
                F_StockUpperLimit = it.F_StockUpperLimit ?? 0,
                F_StockLowerLimit = it.F_StockLowerLimit ?? 0,
            })
            .ToListAsync();

        if (!goodsWithThreshold.Any())
        {
            return new {
                currentPage = input.currentPage,
                pageSize = input.pageSize,
                total = 0,
                list = new List<AGoodsInventoryAlertOutput>()
            };
        }

        var goodsIds = goodsWithThreshold.Select(it => it.F_GoodsId).ToList();

        // 第二步：从库存表查询这些商品的总库存数量
        var inventorySummary = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Where(it => goodsIds.Contains(it.F_GoodsId))
            .GroupBy(it => it.F_GoodsId)
            .Select(it => new {
                F_GoodsId = it.F_GoodsId,
                F_TotalNum = SqlFunc.AggregateSum(it.F_Num)
            })
            .ToListAsync();

        var inventoryDict = inventorySummary.ToDictionary(it => it.F_GoodsId, it => it.F_TotalNum ?? 0);

        // 第三步：获取字典数据用于告警状态显示
        var alertStatus1 = await _repositoryDictionaryData.AsQueryable()
            .Where(it => it.Id == "2044625747976523776")
            .Select(it => it.FullName)
            .FirstAsync() ?? "库存不足";
        var alertStatus2 = await _repositoryDictionaryData.AsQueryable()
            .Where(it => it.Id == "2044625677482856448")
            .Select(it => it.FullName)
            .FirstAsync() ?? "库存过剩";

        // 第四步：关联计算告警状态
        var alertList = new List<AGoodsInventoryAlertOutput>();
        foreach (var goods in goodsWithThreshold)
        {
            var currentNum = inventoryDict.ContainsKey(goods.F_GoodsId) ? inventoryDict[goods.F_GoodsId] : 0;

            // 判断告警类型
            int alertType = 0;
            string enCode = "";
            string alertStatus = "";
            decimal deviationValue = 0;

            if (currentNum < goods.F_StockLowerLimit)
            {
                alertType = 1; // 库存不足
                enCode = "LowStock"; // 库存不足编码
                deviationValue = goods.F_StockLowerLimit - currentNum;
                alertStatus = alertStatus1; // 从字典获取
            }
            else if (currentNum > goods.F_StockUpperLimit)
            {
                alertType = 2; // 库存过剩
                enCode = "OverStock"; // 库存过剩编码
                deviationValue = currentNum - goods.F_StockUpperLimit;
                alertStatus = alertStatus2; // 从字典获取
            }

            // 根据查询条件过滤（使用enCode筛选）
            if (!string.IsNullOrEmpty(input.alertType) && input.alertType != enCode) continue;

            alertList.Add(new AGoodsInventoryAlertOutput
            {
                id = goods.id,
                F_GoodsId = goods.F_GoodsId,
                F_GoodsNo = goods.F_GoodsNo,
                F_GoodsName = goods.F_GoodsName,
                F_Unit = goods.F_Unit,
                F_Specification = goods.F_Specification,
                F_CurrentNum = currentNum,
                F_StockUpperLimit = goods.F_StockUpperLimit,
                F_StockLowerLimit = goods.F_StockLowerLimit,
                alertType = alertType,
                enCode = enCode,
                alertStatus = alertStatus,
                deviationValue = deviationValue
            });
        }

        // 排序：库存不足排前面，然后是库存过剩
        alertList = alertList
            .OrderByDescending(it => it.alertType > 0)
            .ThenBy(it => it.F_GoodsNo)
            .ToList();

        // 分页
        var total = alertList.Count;
        var pagedData = alertList
            .Skip((input.currentPage - 1) * input.pageSize)
            .Take(input.pageSize)
            .ToList();

        // 第四步：处理名称显示（单位转换）
        if (pagedData.Any())
        {
            await _repository.AsSugarClient().ThenMapperAsync(pagedData, async item =>
            {
                var unitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
                if (!string.IsNullOrEmpty(item.F_Unit))
                {
                    var unitIds = item.F_Unit.ToObject<List<string>>();
                    if (unitIds.Count > 0)
                    {
                        var unitName = unitData.FirstOrDefault(it => unitIds.Contains(it.id))?.fullName;
                        if (!string.IsNullOrEmpty(unitName))
                        {
                            item.F_Unit = unitName;
                        }
                    }
                }
            });
        }

        return new {
            currentPage = input.currentPage,
            pageSize = input.pageSize,
            total = total,
            list = pagedData
        };
    }


    /// <summary>
    /// 获取商品库存列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AGoodsInventoryListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AGoodsInventoryListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var startF_Num = input.F_Num?.FirstOrDefault()?.ParseToDecimal() == null ? decimal.MinValue : input.F_Num?.First();
        var endF_Num = input.F_Num?.LastOrDefault()?.ParseToDecimal() == null ? decimal.MaxValue : input.F_Num?.Last();
        //var F_WarehouseId = input.F_WarehouseId?.Last();
        var warehouseList = new List<string>();
        if(!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()))
        {
            warehouseList = await _repositoryWarehouse.AsQueryable().Where(it => it.DeleteMark == null && it.F_Name.Contains(input.F_WarehouseId)).Select(it => it.id).ToListAsync();
        }

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_GoodsId), it => it.F_GoodsId.Equals(input.F_GoodsId))
            //.WhereIF(!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()), it => it.F_WarehouseId.Contains(F_WarehouseId))
            .WhereIF(!string.IsNullOrEmpty(input.F_WarehouseId?.ToString()), it => warehouseList.Any(w => it.F_WarehouseId.Contains(w)))
            .WhereIF(input.F_Num?.Count() > 0, it => SqlFunc.Between(it.F_Num, startF_Num, endF_Num))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new AGoodsInventoryListOutput
            {
                id = it.id,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_Code = it.F_Code,
            })
            //把多表结果变成“单表”再排序：
            .MergeTable()
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .OrderBy(it => it.F_Code, OrderByType.Desc)
            .ToPagedListAsync(input.currentPage, input.pageSize);

        var ids = idsQ.list.Select(x => x.id).ToList();   // 20 个 long

        // 2. 再用 ids 一次性补全字段（此时只有 20 行，随便 join 18 张表都毫秒级）
        var data = await _repository.AsQueryable()
            .Where(it => ids.Contains(it.id))
            .LeftJoin<AGoodsEntity>((it, g) => g.id.Equals(it.F_GoodsId) && g.DeleteMark == null)
            .Select((it, g) => new AGoodsInventoryListOutput
            {
                id = it.id,
                F_Code = it.F_Code,
                F_GoodsId = g.F_GoodsName,
                F_GoodsNo = g.F_GoodsNo,
                F_Unit = g.F_Unit,
                F_WarehouseId = it.F_WarehouseId,
                F_Num = it.F_Num,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderBy(it => it.F_CreatorTime, OrderByType.Desc).OrderBy(it => it.F_Code, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            // 仓库ID
            var F_WarehouseIdData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (item.F_WarehouseId != null)
            {
                var F_WarehouseIdCascader = item.F_WarehouseId.ToObject<List<string>>();
                item.F_WarehouseId = string.Join("/", F_WarehouseIdData.FindAll(it => F_WarehouseIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(item.F_Unit))
            {
                var F_UnitIdCascader = item.F_Unit.ToObject<List<string>>();
                if (F_UnitIdCascader.Count > 1)
                {
                    item.F_NumInfo = "1" + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName + Math.Floor(item.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    item.F_NumInfo = Math.Floor(item.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }
        });

        data.pagination = idsQ.pagination;

        return PageResult<AGoodsInventoryListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建商品库存.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AGoodsInventoryCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AGoodsInventoryEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AGoodsInventoryEntity>(new AGoodsInventoryEntity()));
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 更新商品库存.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AGoodsInventoryUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AGoodsInventoryEntity>();
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_GoodsId,
                it.F_WarehouseId,
                it.F_Num,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除商品库存.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除商品库存.
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
            // 批量删除商品库存
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 商品库存详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AGoodsInventoryDetailOutput
            {
                id = it.id,
                F_GoodsId = it.F_GoodsId,
                F_WarehouseId = it.F_WarehouseId,
                F_Num = it.F_Num,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
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

    /// <summary>
    /// 导入商品库存.
    /// </summary>
    /// <param name="file">Excel文件.</param>
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

            // 读取仓库名称
            var warehouseName = row[0]?.ToString()?.Trim();
            if (string.IsNullOrEmpty(warehouseName))
            {
                errors.Add($"第{i + 1}行：仓库名称为空");
                continue;
            }

            // 读取商品名称
            var goodsName = row[1]?.ToString()?.Trim();
            if (string.IsNullOrEmpty(goodsName))
            {
                errors.Add($"第{i + 1}行：商品名称为空");
                continue;
            }

            // 读取商品规格
            var goodsSpec = row[2]?.ToString()?.Trim();

            // 读取库存数量
            var numStr = row[3]?.ToString()?.Trim();
            if (!decimal.TryParse(numStr, out decimal num) || num < 0)
            {
                errors.Add($"第{i + 1}行：库存数量格式不正确");
                continue;
            }

            // 根据仓库名称查询仓库
            var warehouse = await _repositoryWarehouse.AsQueryable()
                .Where(it => it.F_Name == warehouseName && it.DeleteMark == null)
                .FirstAsync();

            if (warehouse == null)
            {
                errors.Add($"第{i + 1}行：仓库[{warehouseName}]不存在");
                continue;
            }

            // 根据商品名称和规格查询商品
            var goodsQuery = _repositoryGoods.AsQueryable()
                .Where(it => it.F_GoodsName == goodsName && it.DeleteMark == null);
            
            if (!string.IsNullOrEmpty(goodsSpec))
            {
                goodsQuery = goodsQuery.Where(it => it.F_Specification == goodsSpec);
            }
            
            var goods = await goodsQuery.FirstAsync();

            if (goods == null)
            {
                var specInfo = string.IsNullOrEmpty(goodsSpec) ? "" : $"，规格[{goodsSpec}]";
                errors.Add($"第{i + 1}行：商品[{goodsName}]{specInfo}不存在");
                continue;
            }

            // 生成F_Code编码（参考AGoodsInventoryQrService的Create方法）
            string fCode = await GenerateInventoryCode(goods);

            // 创建库存实体
            var entity = new AGoodsInventoryEntity
            {
                id = SnowflakeIdHelper.NextId(),
                F_WarehouseId = new List<string> { warehouse.id }.ToJsonString(),
                F_GoodsId = goods.id,
                F_Num = num,
                F_Code = fCode,
                F_CreatorUserId = _userManager.UserId,
                F_CreatorTime = DateTime.Now,
            };

            // 保存到数据库
            var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
            if (isOk > 0)
            {
                successCount++;
            }
            else
            {
                errors.Add($"第{i + 1}行：保存失败");
            }
        }

        if (errors.Any())
        {
            throw Oops.Oh($"导入完成，成功{successCount}条，失败{errors.Count}条：\n{string.Join("\n", errors)}");
        }
    }

    /// <summary>
    /// 生成库存编码（参考AGoodsInventoryQrService的Create方法）
    /// </summary>
    /// <param name="goodsEntity">商品实体</param>
    /// <returns>库存编码</returns>
    private async Task<string> GenerateInventoryCode(AGoodsEntity goodsEntity)
    {
        if (!string.IsNullOrEmpty(goodsEntity.F_CategoryId))
        {
            var categoryList = goodsEntity.F_CategoryId.ToObject<List<string>>();
            var code = "";
            foreach (var categoryId in categoryList)
            {
                var categoryCode = await _repository.AsSugarClient().Queryable<AGoodsCategoryEntity>()
                    .Where(it => it.id == categoryId)
                    .Select(it => it.F_Code)
                    .FirstAsync();
                
                if (categoryCode != null)
                {
                    code = code + categoryCode;
                }
            }
            
            var goodsNoWithCode = code + goodsEntity.F_GoodsNo;

            if (!string.IsNullOrEmpty(goodsNoWithCode))
            {
                // 查询a_goods_inventory中已有相同前缀的编号
                var existingNos = await _repository.AsQueryable()
                    .Where(it => it.F_Code != null && it.F_Code.StartsWith(goodsNoWithCode) && it.DeleteMark == null)
                    .Select(it => it.F_Code)
                    .ToListAsync();

                int maxSeq = 0;
                foreach (var no in existingNos)
                {
                    if (no.Length > goodsNoWithCode.Length)
                    {
                        var suffix = no.Substring(goodsNoWithCode.Length);
                        if (int.TryParse(suffix, out int seq))
                        {
                            if (seq > maxSeq) maxSeq = seq;
                        }
                    }
                }

                var nextSeq = maxSeq + 1;
                return goodsNoWithCode + nextSeq;
            }
            else
            {
                throw Oops.Oh(ErrorCode.COM1018, "商品编号为空，请先设置商品编号");
            }
        }
        else
        {
            throw Oops.Oh(ErrorCode.COM1018, "商品类别为空，请先设置商品类别");
        }
    }


    /// <summary>
    /// 弹窗 根据仓库获取商品列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("PopupByIdList")]
    public async Task<dynamic> GetPopupList(string warehouseId)
    {
        var warehouseList = warehouseId.ToObject<List<string>>();
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_WarehouseId.Contains(warehouseList.LastOrDefault()))
            .Where(it => it.FlowId == null)
            .LeftJoin<AGoodsEntity>((it, g) => g.id.Equals(it.F_GoodsId) && g.DeleteMark == null)
            .Select((it, g) => new AGoodsInventoryListOutput
            {
                id = it.id,
                F_GoodsId = it.F_GoodsId,
                F_GoodsNo = g.F_GoodsNo,
                F_GoodsName = g.F_GoodsName,
                F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(g.F_Type) && dic.DictionaryTypeId.Equals("2029481827470807040")).Select(dic => dic.FullName),
                F_CategoryId = g.F_CategoryId,
                /*F_Unit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(g.F_Unit) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),*/
                F_Specification = g.F_Specification,
                F_InspectRule = g.F_InspectRule,
                F_SalePrice = g.F_SalePrice,
                F_CostPrice = g.F_CostPrice,
                F_CreatorTime = g.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_Remark = g.F_Remark,
                F_Num = it.F_Num,
            }).MergeTable()
            .OrderBy("F_CreatorTime desc") // 按创建时间降序
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 商品分类ID
            var F_CategoryIdData = await _dataInterfaceService.GetDynamicList("F_CategoryId", "2008414575283802112", "F_Id", "F_Name", "children");
            //if (item.F_CategoryId != null)
            //{
            //    var F_CategoryIdCascader = item.F_CategoryId.ToObject<List<string>>();
            //    item.F_CategoryId = F_CategoryIdData.FindAll(it => F_CategoryIdCascader.Contains(it.id)).Select(s => s.fullName).FirstOrDefault();
            //}
        });
        return new {
            data = data
        };
    }

    /// <summary>
    /// 弹窗 根据仓库获取商品列表(库存盘点).
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("CheckPopupByIdList")]
    public async Task<dynamic> GetCheckPopupList(string warehouseId)
    {
        var warehouseList = warehouseId.ToObject<List<string>>();
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_WarehouseId.Contains(warehouseList.LastOrDefault()))
            .Where(it => it.FlowId == null)
            .LeftJoin<AGoodsEntity>((it, g) => g.id.Equals(it.F_GoodsId) && g.DeleteMark == null)
            .Select((it, g) => new AGoodsInventoryListOutput
            {
                id = it.id,
                F_GoodsId = it.F_GoodsId,
                F_GoodsNo = g.F_GoodsNo,
                F_GoodsName = g.F_GoodsName,
                F_Code = it.F_Code,
                F_Type = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(g.F_Type) && dic.DictionaryTypeId.Equals("2029481827470807040")).Select(dic => dic.FullName),
                F_CategoryId = g.F_CategoryId,
                F_Unit = g.F_Unit,
                F_Specification = g.F_Specification,
                F_InspectRule = g.F_InspectRule,
                F_SalePrice = g.F_SalePrice,
                F_CostPrice = g.F_CostPrice,
                F_CreatorTime = g.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_Remark = g.F_Remark,
                F_Num = it.F_Num,
                F_Unit_Ratio = g.F_Unit_Ratio,
            }).MergeTable()
            .OrderBy("F_CreatorTime desc") // 按创建时间降序
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(item.F_Unit))
            {
                var F_UnitIdCascader = item.F_Unit.ToObject<List<string>>();
                if(F_UnitIdCascader.Count > 1)
                {
                    item.F_NumInfo = "1" + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName + Math.Floor(item.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    item.F_NumInfo = Math.Floor(item.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }
        });
        return new {
            data = data
        };
    }

    /// <summary>
    /// 门户-商品库存信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("MH-SPKCXX")]
    public async Task<dynamic> GetMH_SPKCXX()
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .LeftJoin<AGoodsEntity>((it, g) => g.id.Equals(it.F_GoodsId) && g.DeleteMark == null)
            .Select((it, g) => new AGoodsInventoryListOutput
            {
                id = it.id,
                F_Code = it.F_Code,
                F_GoodsId = SqlFunc.MergeString(g.F_GoodsName, "/", g.F_GoodsNo),
                F_Unit = g.F_Unit,
                F_WarehouseId = it.F_WarehouseId,
                F_Num = it.F_Num,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderBy(it => it.F_CreatorTime, OrderByType.Desc).OrderBy(it => it.F_Code, OrderByType.Desc).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            // 仓库ID
            var F_WarehouseIdData = await _dataInterfaceService.GetDynamicList("F_WarehouseId", "2021045201115680768", "F_Id", "F_Name", "children");
            if (item.F_WarehouseId != null)
            {
                var F_WarehouseIdCascader = item.F_WarehouseId.ToObject<List<string>>();
                item.F_WarehouseId = string.Join("/", F_WarehouseIdData.FindAll(it => F_WarehouseIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(item.F_Unit))
            {
                var F_UnitIdCascader = item.F_Unit.ToObject<List<string>>();
                if (F_UnitIdCascader.Count > 1)
                {
                    item.F_NumInfo = "1" + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName + Math.Floor(item.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    item.F_NumInfo = Math.Floor(item.F_Num.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }
        });

        return new { data = data };
    }

}

