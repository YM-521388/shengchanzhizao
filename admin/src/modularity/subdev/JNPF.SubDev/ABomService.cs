using JNPF.ClayObject;
using JNPF.Common.CodeGen.DataParsing;
using JNPF.Common.CodeGen.ExportImport;
using JNPF.Common.Const;
using JNPF.Common.Core.Manager;
using JNPF.Common.Core.Manager.Files;
using JNPF.Common.Dtos;
using JNPF.Common.Dtos;
using JNPF.Common.Dtos.Datainterface;
using JNPF.Common.Enums;
using JNPF.Common.Extension;
using JNPF.Common.Filter;
using JNPF.Common.Manager;
using JNPF.Common.Models;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Models.NPOI;
using JNPF.Common.Security;
using JNPF.DatabaseAccessor;
using JNPF.DependencyInjection;
using JNPF.DynamicApiController;
using JNPF.Engine.Entity.Model;
using JNPF.example.Entitys;
using JNPF.example.Entitys.Dto.ABom;
using JNPF.example.Entitys.Dto.ABomGoods;
using JNPF.example.Interfaces;
using JNPF.FriendlyException;
using JNPF.Systems.Entitys.Permission;
using JNPF.Systems.Entitys.System;
using JNPF.Systems.Interfaces.System;
using JNPF.VisualDev.Engine;
using JNPF.VisualDev.Engine.Core;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Linq;
using System.Reactive;
using System.Text.RegularExpressions;

namespace JNPF.example;

/// <summary>
/// 业务实现：a_bom.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "ABom", Order = 200)]
[Route("api/example/[controller]")]
public class ABomService : IABomService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<ABomEntity> _repository;
    private readonly ISqlSugarRepository<ABomlogEntity> _repositorylog;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositorysp;
    private readonly ISqlSugarRepository<ABomGoodsEntity> _repositoryGoods;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<AGoodsSpecificationEntity> _repositorygg;


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
    /// 数据库管理.
    /// </summary>
    private readonly IDataBaseManager _dataBaseManager;

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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"BOM编号\",\"field\":\"F_BomCode\"},{\"value\":\"BOM名称\",\"field\":\"F_BomName\"},{\"value\":\"类别\",\"field\":\"F_CategoryId\"},{\"value\":\"商品\",\"field\":\"F_GoodsId\"},{\"value\":\"默认BOM\",\"field\":\"F_IsDefault\"},{\"value\":\"备注\",\"field\":\"F_State\"},{\"value\":\"设计子表-投料工序\",\"field\":\"tableFieldd2a80d-F_Process\"},{\"value\":\"设计子表-单位用量\",\"field\":\"tableFieldd2a80d-F_Unit\"},{\"value\":\"设计子表-备注\",\"field\":\"tableFieldd2a80d-F_Description\"},{\"value\":\"设计子表-创建人员\",\"field\":\"tableFieldd2a80d-F_CreatorUserId\"},{\"value\":\"设计子表-创建时间\",\"field\":\"tableFieldd2a80d-F_CreatorTime\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();


    /// <summary>
    /// 导入字段.
    /// </summary>
    private readonly List<ParamsModel> childParanList = "[{\"value\":\"添加数据\",\"field\":\"tableFieldd2a80d\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 导入字段.
    /// </summary>
    private readonly string[] uploaderKey = new string[] { "tableFieldd2a80d-F_ParentId", "tableFieldd2a80d-F_Unit", "tableFieldd2a80d-F_Description" };


    /// <summary>
    /// 初始化一个<see cref="ABomService"/>类型的新实例.
    /// </summary>
    public ABomService(
        ISqlSugarRepository<AGoodsSpecificationEntity> repositorygg,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<ABomGoodsEntity> repositoryGoods,
        ISqlSugarRepository<ABomEntity> repository,
        ISqlSugarRepository<ABomlogEntity> repositorylog,
         ISqlSugarRepository<AGoodsEntity> repositorysp,
        IDataInterfaceService dataInterfaceService,
         IDataBaseManager dataBaseManager,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositorygg = repositorygg;
        _repositoryInventory = repositoryInventory;
        _dataBaseManager = dataBaseManager;
        _repositoryGoods = repositoryGoods;
        _repository = repository;
        _repositorylog = repositorylog;
        _repositorysp = repositorysp;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_bom.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.ABomGoodsList)
            .Where(it => it.DeleteMark == null)
            .Select(it => new ABomEntity
            {
                id = it.id,
                F_BomCode = it.F_BomCode,
                F_BomName = it.F_BomName,
                F_CategoryId = it.F_CategoryId,
                F_GoodsId = it.F_GoodsId,
                F_State = it.F_State,
                F_IsDefault = it.F_IsDefault,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                ABomGoodsList = it.ABomGoodsList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<ABomInfoOutput>();

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFieldd2a80d, async aBomGoods =>
        {
            // 创建人员
            aBomGoods.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aBomGoods.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aBomGoods.F_CreatorTime = aBomGoods.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            var unit = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(it => it.id.Equals(aBomGoods.F_GoodsId)).Select(it => it.F_Unit).FirstAsync();

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(unit))
            {
                var F_UnitIdCascader = unit.ToObject<List<string>>();
                aBomGoods.F_Unitsp = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));


                if (F_UnitIdCascader.Count > 1)
                {
                    aBomGoods.F_NumUnit = (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    aBomGoods.F_NumUnit = (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }

            aBomGoods.F_InventoryNum = (await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == aBomGoods.F_GoodsId && it.F_Num > 0).SumAsync(it => it.F_Num)) ?? 0;
        });
        return data;
    }

    /// <summary>
    /// 获取a_bom列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] ABomListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<ABomListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_bom"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ABomEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        var selectIds = input.selectIds?.Split(",").ToList();

        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_BomCode), it => it.F_BomCode.Contains(input.F_BomCode))
            .WhereIF(!string.IsNullOrEmpty(input.F_BomName), it => it.F_BomName.Contains(input.F_BomName))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            //.OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id)
            .Select(it => new ABomListOutput
            {
                id = it.id,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            //把多表结果变成“单表”再排序：
            .MergeTable()
            .OrderBy("F_CreatorTime desc")
            .ToPagedListAsync(input.currentPage, input.pageSize);

        var ids = idsQ.list.Select(x => x.id).ToList();   // 20 个 long

        // 2. 再用 ids 一次性补全字段（此时只有 20 行，随便 join 18 张表都毫秒级）
        var data = await _repository.AsQueryable()
            .Where(it => ids.Contains(it.id))
             .Select(it => new ABomListOutput
             {
                 id = it.id,
                 F_BomCode = it.F_BomCode,
                 F_BomName = it.F_BomName,
                 F_CategoryId = it.F_CategoryId,
                 F_GoodsId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_GoodsId)).Select(u => SqlFunc.MergeString(u.F_GoodsName, "-", u.F_GoodsNo)),
                 F_State = it.F_State,
                 F_IsDefault = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsDefault) == 1, "开", "关"),
                 F_CreatorUserId = it.F_CreatorUserId,
                 F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
             }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), "F_CreatorTime desc").OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 类别
            var F_CategoryIdData = await _dataInterfaceService.GetDynamicList("F_CategoryId", "2008713719516893184", "F_Id", "F_Name", "children");
            if (item.F_CategoryId != null)
            {
                var F_CategoryIdCascader = item.F_CategoryId.ToObject<List<string>>();
                item.F_CategoryId = F_CategoryIdData.FindAll(it => F_CategoryIdCascader.Contains(it.id)).Select(s => s.fullName).FirstOrDefault();
            }

            // 创建人员
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;

        });

        data.pagination = idsQ.pagination;

        return PageResult<ABomListOutput>.SqlSugarPageResult(data);

    }

    /// <summary>
    /// 新建a_bom.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] ABomCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ABomEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ABomEntity>(new ABomEntity()));
        ConfigModel tableFieldd2a80dConfig = new ConfigModel
        {
            tableName = "a_bom_goods",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "设计子表",
            children = ExportImportDataHelper.GetTemplateParsing<ABomGoodsEntity>(new ABomGoodsEntity())
        };
        FieldsModel tableFieldd2a80dModel = new FieldsModel()
        {
            __config__ = tableFieldd2a80dConfig,
            __vModel__ = "tableFieldd2a80d"
        };
        fieldList.Add(tableFieldd2a80dModel);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        // 如果前端没有传F_BomCode，则自动生成编码
        if (string.IsNullOrEmpty(input.F_BomCode))
        {
            var today = DateTime.Now.ToString("yyyyMMdd");
            // 查询今天已存在的BOM数量，用于生成序号
            var todayCount = await _repository.AsQueryable()
                .Where(it => it.F_BomCode != null && it.F_BomCode.StartsWith($"BOM{today}"))
                .CountAsync();
            var sequenceNumber = (todayCount + 1).ToString("D4"); // 4位序号
            entity.F_BomCode = $"BOM{today}{sequenceNumber}";
        }

        var aBomGoodsEntityList = input.tableFieldd2a80d?.Adapt<List<ABomGoodsEntity>>();
        if (aBomGoodsEntityList != null)
        {
            // Keep original front-end F_BomId labels so we can map "1" -> parent's generated F_Id
            var originalList = aBomGoodsEntityList.Select(it => new { Item = it, OrigLabel = it.F_BomId }).ToList();

            // Assign IDs / creator info and set parent relation to the BOM entity
            foreach (var entry in originalList)
            {
                var item = entry.Item;
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                // parent is the BOM main entity
                item.F_ParentId = entity.id;
            }

            // Build mapping from every original label to its generated F_Id.
            // This allows resolving immediate parent labels at any nesting level.
            var labelToId = new Dictionary<string, string>();
            foreach (var entry in originalList)
            {
                var lab = entry.OrigLabel;
                if (!string.IsNullOrEmpty(lab))
                {
                    labelToId[lab] = entry.Item.F_Id;
                }
            }

            // Now assign F_BomId for each row:
            // - If the original label is a pure integer (e.g. "1"), we should NOT store that label in DB per requirement.
            //   So clear F_BomId (keep mapping for children).
            // - If the original label is nested (e.g. "1.1" or "1.1.1"), find its immediate parent label by removing
            //   the last segment (e.g. "1.1.1" -> "1.1") and map to that parent's generated F_Id.
            foreach (var entry in originalList)
            {
                var lab = entry.OrigLabel;
                if (string.IsNullOrEmpty(lab))
                {
                    entry.Item.F_BomId = null;
                    continue;
                }

                if (Regex.IsMatch(lab, @"^\d+$"))
                {
                    // integer group: do not persist the integer label itself
                    entry.Item.F_BomId = null;
                    continue;
                }

                // nested label: immediate parent is label without the last ".segment"
                var lastDot = lab.LastIndexOf('.');
                if (lastDot > 0)
                {
                    var parentLab = lab.Substring(0, lastDot);
                    if (labelToId.TryGetValue(parentLab, out var parentId))
                    {
                        entry.Item.F_BomId = parentId;
                    }
                    else
                    {
                        // parent not found in the provided rows: clear to avoid wrong references
                        entry.Item.F_BomId = null;
                    }
                }
                else
                {
                    // unexpected format fallback
                    entry.Item.F_BomId = null;
                }
            }

            entity.ABomGoodsList = aBomGoodsEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.ABomGoodsList)
            .ExecuteCommandAsync();

        // 记录操作日志到 a_bom_log 表
        try
        {
            var log = new ABomlogEntity
            {
                id = SnowflakeIdHelper.NextId(),
                F_BomId = entity.id,
                F_Content = "新增商品BOM",
                TenantId = "0",
                F_CreatorUserId = _userManager.UserId,
                F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
            };

            await _repositorylog.AsInsertable(log).ExecuteCommandAsync();
        }
        catch (Exception)
        {
            // 记录日志失败不影响主流程，但记录异常以便排查
        }
    }

    /// <summary>
    /// 根据 BOM Id 获取操作日志列表
    /// </summary>
    /// <param name="id">BOM 主键</param>
    /// <returns></returns>
    [HttpGet("Logs/{id}")]
    public async Task<dynamic> GetLogs(string id)
    {
        var data = await _repositorylog.AsQueryable()
            .Where(it => it.F_BomId == id)
            .OrderBy(it => it.F_CreatorTime, OrderByType.Desc)
            .Select(it => new ABomLogOutput
            {
                id = it.id,
                F_BomId = it.F_BomId,
                F_Type = it.F_Type,
                F_Title = it.F_Title,
                F_Content = it.F_Content,
                F_Reason = it.F_Reason,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
            })
            .ToListAsync();

        return data;
    }

    /// <summary>
    /// 获取a_bom无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(ABomListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aBomGoodsAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<ABomListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_bom"))?.conditionalModel;
            if (allAuthorizeWhere.Any(it => it.TableName.Equals("a_bom_goods"))) aBomGoodsAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_bom_goods"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ABomEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ABomGoodsEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldd2a80d-", entityInfo, 1);
        List<IConditionalModel> aBomGoodsConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);


        var selectIds = input.selectIds?.Split(",").ToList();

        var list = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_BomCode), it => it.F_BomCode.Contains(input.F_BomCode))
            .WhereIF(!string.IsNullOrEmpty(input.F_BomName), it => it.F_BomName.Contains(input.F_BomName))
            .WhereIF(!string.IsNullOrEmpty(input.F_CreatorUserId), it => it.F_CreatorUserId.Equals(input.F_CreatorUserId))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd 00:00:00"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd 23:59:59")))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            //.OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), "F_CreatorTime desc") // 👈 修改这里：默认按创建时间降序
            .OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new ABomListOutput
            {
                id = it.id,
                F_BomCode = it.F_BomCode,
                F_BomName = it.F_BomName,
                F_CategoryId = it.F_CategoryId,
                F_GoodsId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_GoodsId)).Select(u => SqlFunc.MergeString(u.F_GoodsName, "-", u.F_GoodsNo)),
                F_State = it.F_State,
                F_IsDefault = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsDefault) == 1, "开", "关"),
                F_CreatorUserId = it.F_CreatorUserId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 类别
            var F_CategoryIdData = await _dataInterfaceService.GetDynamicList("F_CategoryId", "2008713719516893184", "F_Id", "F_Name", "children");
            if (item.F_CategoryId != null)
            {
                var F_CategoryIdCascader = item.F_CategoryId.ToObject<List<string>>();
                item.F_CategoryId = F_CategoryIdData.FindAll(it => F_CategoryIdCascader.Contains(it.id)).Select(s => s.fullName).FirstOrDefault();
            }

            // 创建人员
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;

        });

        var resData = list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);


        list = resData.ToObject<List<ABomListOutput>>(CommonConst.options);
        return list;
    }

    /// <summary>
    /// 导出a_bom.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] ABomListQueryInput input)
    {
        var exportData = new List<ABomListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<ABomListOutput>>().list;
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
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ABomEntity>(new ABomEntity()));
        ConfigModel tableFieldd2a80dConfig = new ConfigModel
        {
            tableName = "a_bom_goods",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "设计子表",
            children = ExportImportDataHelper.GetTemplateParsing<ABomGoodsEntity>(new ABomGoodsEntity())
        };
        FieldsModel tableFieldd2a80dModel = new FieldsModel()
        {
            __config__ = tableFieldd2a80dConfig,
            __vModel__ = "tableFieldd2a80d"
        };
        fieldList.Add(tableFieldd2a80dModel);

        List<Dictionary<string, object>>? dataList = new List<Dictionary<string, object>>();

        // 赋予默认值
        var dicItem = ExportImportDataHelper.GetTemplateHeader<ABomEntity>(new ABomEntity(), 1);
        var tableFieldd2a80d = ExportImportDataHelper.GetTemplateHeader<ABomGoodsEntity>(new ABomGoodsEntity(), 3, "tableFieldd2a80d");

        dicItem.Add("id", "id");
        var table1 = dicItem.Concat(tableFieldd2a80d).ToDictionary(k => k.Key, v => v.Value);
        dataList.Add(table1);

        var excelName = string.Format("{0}导入模板", _controlParsing.GetModuleNameById(menuId));
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetTemplateExport(excelName, string.Join(",", uploaderKey), _userManager.UserId, dataList, fieldList, childParanList);
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
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ABomEntity>(new ABomEntity()));
        ConfigModel tableFieldd2a80dConfig = new ConfigModel
        {
            tableName = "a_bom_goods",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "设计子表",
            children = ExportImportDataHelper.GetTemplateParsing<ABomGoodsEntity>(new ABomGoodsEntity())
        };
        FieldsModel tableFieldd2a80d = new FieldsModel()
        {
            __config__ = tableFieldd2a80dConfig,
            __vModel__ = "tableFieldd2a80d"
        };
        fieldList.Add(tableFieldd2a80d);

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ABomEntity>();
        List<DbTableRelationModel> tables = new List<DbTableRelationModel>() { ExportImportDataHelper.GetTableRelation(entityInfo, "1") };
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ABomGoodsEntity>();
        tables.Add(ExportImportDataHelper.GetTableRelation(entityInfo, "0", "F_BomId", "a_bom", "F_Id"));
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
        // 有 recordId 时：只往子表 a_bom_goods 追加数据，不新增主表 a_bom；F_BomId、F_ParentId 均用 recordId
        if (!string.IsNullOrEmpty(input.recordId) && input.list != null && input.list.Count > 0)
        {
            return await ImportChildOnlyAsync(input.recordId, input.list);
        }

        // 无 recordId：走原有主表+子表一起导入
        var childFields = ExportImportDataHelper.GetTemplateParsing<ABomGoodsEntity>(new ABomGoodsEntity()).ToList();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ABomEntity>(new ABomEntity()));
        ConfigModel tableFieldd2a80dConfig = new ConfigModel
        {
            tableName = "a_bom_goods",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "设计子表",
            children = childFields
        };
        FieldsModel tableFieldd2a80d = new FieldsModel()
        {
            __config__ = tableFieldd2a80dConfig,
            __vModel__ = "tableFieldd2a80d"
        };
        fieldList.Add(tableFieldd2a80d);

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ABomEntity>();
        List<DbTableRelationModel> tables = new List<DbTableRelationModel>() { ExportImportDataHelper.GetTableRelation(entityInfo, "1") };
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ABomGoodsEntity>();
        tables.Add(ExportImportDataHelper.GetTableRelation(entityInfo, "0", "F_BomId", "a_bom", "F_Id"));
        DbLinkEntity link = _dataBaseManager.GetTenantDbLink(_userManager.TenantId, _userManager.TenantDbName);
        var tInfo = new TemplateParsingBase(link, fieldList, tables, "F_Id", 2, 1, uploaderKey.ToList(), "1", true);
        return await _exportImportDataHelper.ImportMenuData(tInfo, input);
    }

    /// <summary>
    /// 仅导入子表 a_bom_goods（不新增主表），F_ParentId 均为传入的 parentId.
    /// </summary>
    private async Task<DataImportOutput> ImportChildOnlyAsync(string parentId, List<Dictionary<string, object>> list)
    {
        var result = new DataImportOutput();
        var mainList = list.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        var allChildDicts = new List<Dictionary<string, object>>();

        // Excel 的“上级”列会映射到子表字段 F_ParentId（见 uploaderKey: tableFieldd2a80d-F_ParentId）。
        // 这里把它当作“商品编号”(F_GoodsNo)来解析，批量去 a_goods 匹配拿到商品 id，最终写入子表 F_GoodsId。
        var goodsNoSet = new HashSet<string>();

        foreach (var item in mainList)
        {
            if (!item.ContainsKey("tableFieldd2a80d") || item["tableFieldd2a80d"] == null)
                continue;
            var childList = item["tableFieldd2a80d"].ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
            if (childList == null)
                continue;
            foreach (var c in childList)
            {
                // 采集商品编号（来自 Excel 的“上级”列）
                var goodsNo = c.ContainsKey("F_ParentId") ? c["F_ParentId"]?.ToString() : null;
                if (!string.IsNullOrWhiteSpace(goodsNo))
                {
                    goodsNo = goodsNo.Trim();
                    goodsNoSet.Add(goodsNo);
                    // 暂存：后面会覆盖 F_ParentId 为 recordId
                    c["__goodsNo"] = goodsNo;
                }

                // 关联主表ID、父级ID：统一由后端赋值，不取 Excel
                c["F_ParentId"] = parentId;
                allChildDicts.Add(c);
            }
        }

        if (allChildDicts.Count == 0)
        {
            result.snum = 0;
            result.fnum = 0;
            result.resultType = 0;
            result.failResult = new List<Dictionary<string, object>>();
            return result;
        }

        // 批量匹配商品编号 -> 商品id
        Dictionary<string, string> goodsNoToId = new Dictionary<string, string>();
        if (goodsNoSet.Count > 0)
        {
            var goodsPairs = await _repositorysp.AsQueryable()
                .Where(it => it.DeleteMark == null && goodsNoSet.Contains(it.F_GoodsNo))
                .Select(it => new { it.F_GoodsNo, it.id })
                .ToListAsync();
            goodsNoToId = goodsPairs
                .Where(x => x.F_GoodsNo != null && x.id != null)
                .GroupBy(x => x.F_GoodsNo)
                .ToDictionary(g => g.Key, g => g.First().id);
        }

        var entities = allChildDicts.Adapt<List<ABomGoodsEntity>>();
        foreach (var e in entities)
        {
            e.F_Id = SnowflakeIdHelper.NextId();
            e.F_ParentId = parentId;
            e.F_CreatorUserId = _userManager.UserId;
            e.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        }

        // 将 Excel 的“上级”(goodsNo) -> goodsId 映射到实体 F_GoodsId
        for (int i = 0; i < entities.Count; i++)
        {
            var rawGoodsNo = allChildDicts[i].ContainsKey("__goodsNo") ? allChildDicts[i]["__goodsNo"]?.ToString() : null;
            if (!string.IsNullOrWhiteSpace(rawGoodsNo))
            {
                var key = rawGoodsNo.Trim();
                if (goodsNoToId.TryGetValue(key, out var gid))
                {
                    entities[i].F_GoodsId = gid;
                }
                else
                {
                    // 匹配不到则保持为空（也可以改成抛错）
                    entities[i].F_GoodsId = null;
                }
            }
        }

        await _repositoryGoods.AsSugarClient().Insertable(entities).ExecuteCommandAsync();

        result.snum = entities.Count;
        result.fnum = 0;
        result.resultType = 0;
        result.failResult = new List<Dictionary<string, object>>();
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
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ABomEntity>(new ABomEntity()));
        ConfigModel tableFieldd2a80dConfig = new ConfigModel
        {
            tableName = "a_bom_goods",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "设计子表",
            children = ExportImportDataHelper.GetTemplateParsing<ABomGoodsEntity>(new ABomGoodsEntity())
        };
        FieldsModel tableFieldd2a80d = new FieldsModel()
        {
            __config__ = tableFieldd2a80dConfig,
            __vModel__ = "tableFieldd2a80d"
        };
        fieldList.Add(tableFieldd2a80d);

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ABomEntity>();
        List<DbTableRelationModel> tables = new List<DbTableRelationModel>() { ExportImportDataHelper.GetTableRelation(entityInfo, "1") };
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<ABomGoodsEntity>();
        tables.Add(ExportImportDataHelper.GetTableRelation(entityInfo, "0", "F_BomId", "a_bom", "F_Id"));
        DbLinkEntity link = _dataBaseManager.GetTenantDbLink(_userManager.TenantId, _userManager.TenantDbName); // 当前数据库连接
        var tInfo = new TemplateParsingBase(link, fieldList, tables, "F_Id", 2, 1, uploaderKey.ToList(), "1", true);
        tInfo.FullName = _controlParsing.GetModuleNameById(list.menuId);

        // 错误数据
        tInfo.selectKey.Add("errorsInfo");
        tInfo.AllFieldsModel.Add(new FieldsModel() { __vModel__ = "errorsInfo", __config__ = new ConfigModel() { label = "异常原因" } });
        for (var i = 0; i < list.list.Count(); i++) list.list[i].Add("id", i);

        var result = ExportImportDataHelper.GetCreateFirstColumnsHeader(tInfo.selectKey, list.list, childParanList);
        var firstColumns = result.Item1;
        var resultList = result.Item2;
        var fName = string.Format("{0}错误报告{1:yyyyMMddHHmmss}", tInfo.FullName, DateTime.Now);
        _cacheManager.Set(fName + ".xls", string.Empty);
        return firstColumns.Any()
            ? await _exportImportDataHelper.ExcelCreateModel(tInfo, resultList, fName, firstColumns)
            : await _exportImportDataHelper.ExcelCreateModel(tInfo, resultList, fName);
    }

    /// <summary>
    /// 更新a_bom.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] ABomUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<ABomEntity>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<ABomEntity>(new ABomEntity()));
        ConfigModel tableFieldd2a80dConfig = new ConfigModel
        {
            tableName = "a_bom_goods",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "设计子表",
            children = ExportImportDataHelper.GetTemplateParsing<ABomGoodsEntity>(new ABomGoodsEntity())
        };
        FieldsModel tableFieldd2a80dModel = new FieldsModel()
        {
            __config__ = tableFieldd2a80dConfig,
            __vModel__ = "tableFieldd2a80d"
        };
        fieldList.Add(tableFieldd2a80dModel);

        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_BomCode.Equals(input.F_BomCode) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "BOOM编号");

        if (string.IsNullOrEmpty(entity.F_BomCode))
        {
            entity.F_BomCode = oldEntity.F_BomCode;
        }

        // 移除BOM商品结构信息可能被删除数据
        if (input.tableFieldd2a80d != null && input.tableFieldd2a80d.Any())
            await _repository.AsSugarClient().Deleteable<ABomGoodsEntity>().Where(it => it.F_ParentId == entity.id && !input.tableFieldd2a80d.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<ABomGoodsEntity>().Where(it => it.F_ParentId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增/更新BOM商品结构信息 - 支持多级 F_BomId 标签映射（例如 "1", "1.1", "1.1.1"）
        var aBomGoodsEntityList = input.tableFieldd2a80d?.Adapt<List<ABomGoodsEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_BomCode,
                    it.F_BomName,
                    it.F_CategoryId,
                    it.F_GoodsId,
                    it.F_State,
                    it.F_IsDefault,
                })
                .ExecuteCommandAsync();

            if (aBomGoodsEntityList != null)
            {
                // Preserve original labels to build mapping
                var originalList = aBomGoodsEntityList.Select(it => new { Item = it, OrigLabel = it.F_BomId, WasNew = string.IsNullOrEmpty(it.F_Id) }).ToList();

                // Assign IDs for newly added rows and set base fields
                foreach (var entry in originalList)
                {
                    var item = entry.Item;
                    if (entry.WasNew)
                    {
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                    }
                    // set parent bom id for all child rows
                    item.F_ParentId = entity.id;
                }

                // Build mapping from original label -> generated/existing F_Id
                var labelToId = new Dictionary<string, string>();
                foreach (var entry in originalList)
                {
                    var lab = entry.OrigLabel;
                    if (!string.IsNullOrEmpty(lab))
                    {
                        labelToId[lab] = entry.Item.F_Id;
                    }
                }

                // Assign F_BomId on each row:
                // - pure integer labels (e.g. "1"): should not be stored -> clear F_BomId
                // - nested labels (e.g. "1.1.1"): map to immediate parent's F_Id ("1.1")
                foreach (var entry in originalList)
                {
                    var lab = entry.OrigLabel;
                    if (string.IsNullOrEmpty(lab))
                    {
                        entry.Item.F_BomId = null;
                        continue;
                    }
                    if (Regex.IsMatch(lab, @"^\d+$"))
                    {
                        // integer group: do not persist the integer label itself
                        entry.Item.F_BomId = null;
                        continue;
                    }
                    var lastDot = lab.LastIndexOf('.');
                    if (lastDot > 0)
                    {
                        var parentLab = lab.Substring(0, lastDot);
                        if (labelToId.TryGetValue(parentLab, out var parentId))
                        {
                            entry.Item.F_BomId = parentId;
                        }
                        else
                        {
                            // parent not found in provided rows -> clear to avoid wrong references
                            entry.Item.F_BomId = null;
                        }
                    }
                    else
                    {
                        entry.Item.F_BomId = null;
                    }
                }

                // Persist rows: insert new ones, update existing ones
                foreach (var entry in originalList)
                {
                    var item = entry.Item;
                    if (entry.WasNew)
                    {
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }
                    else
                    {
                        // clear creator info so update won't overwrite those fields
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_ParentId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }

        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
        // 记录更新操作日志到 a_bom_log 表
        try
        {
            var log = new ABomlogEntity
            {
                id = SnowflakeIdHelper.NextId(),
                F_BomId = id,
                F_Content = "更新商品BOM",
                F_Reason = input?.F_Reason,
                TenantId = "0",
                F_CreatorUserId = _userManager.UserId,
                F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
            };

            await _repositorylog.AsInsertable(log).ExecuteCommandAsync();
        }
        catch (Exception)
        {
            // 记录日志失败不影响主流程，但记录异常以便排查
        }
    }

    /// <summary>
    /// 删除a_bom.
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
    /// 批量删除a_bom.
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
            // 批量删除a_bom
            await _repository.AsDeleteable().In(it => it.id, input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_bom详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.ABomGoodsList)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.id == id)
            .Select(it => new ABomDetailOutput
            {
                id = it.id,
                F_BomCode = it.F_BomCode,
                F_BomName = it.F_BomName,
                F_CategoryId = it.F_CategoryId,
                F_GoodsId = it.F_GoodsId,
                F_State = it.F_State,
                F_IsDefault = SqlFunc.IIF(SqlFunc.ToInt32(it.F_IsDefault) == 1, "开", "关"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableFieldd2a80d = it.ABomGoodsList.Adapt<List<ABomGoodsDetailOutput>>(),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            var F_CategoryIdData = await _dataInterfaceService.GetDynamicList("F_CategoryId", "2008713719516893184", "F_Id", "F_Name", "children");
            if (item.F_CategoryId != null)
            {
                var F_CategoryIdCascader = item.F_CategoryId.ToObject<List<string>>();
                item.F_CategoryId = F_CategoryIdData.FindAll(it => F_CategoryIdCascader.Contains(it.id)).Select(s => s.fullName).FirstOrDefault();
            }

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFieldd2a80d), async aBomGoods =>
        {
            var aBom = data.ToList().Find(it => it.id.Equals(aBomGoods.F_ParentId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 投料工序
            if (aBomGoods.F_Process != null)
            {
                aBomGoods.F_Process = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(aBomGoods.F_Process) && it.DictionaryTypeId.Equals("2008728907364306944")).Select(it => it.FullName).FirstAsync();
            }

            var unit = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(it => it.id.Equals(aBomGoods.F_GoodsId)).Select(it => it.F_Unit).FirstAsync();

            var F_UnitData = await _dataInterfaceService.GetDynamicList("F_Unit", "2034507723852353536", "F_Id", "F_Name", "children");
            if (!string.IsNullOrEmpty(unit))
            {
                var F_UnitIdCascader = unit.ToObject<List<string>>();
                aBomGoods.F_Unitsp = string.Join("/", F_UnitData.FindAll(it => F_UnitIdCascader.Contains(it.id)).Select(s => s.fullName));


                if (F_UnitIdCascader.Count > 1)
                {
                    aBomGoods.F_NumUnit = Math.Floor(aBomGoods.F_Unit.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[1] == it.id))[0].fullName;
                }
                else
                {
                    aBomGoods.F_NumUnit = Math.Floor(aBomGoods.F_Unit.Value).ToString() + (F_UnitData.FindAll(it => F_UnitIdCascader[0] == it.id))[0].fullName;
                }
            }

            // 创建人员
            aBomGoods.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aBomGoods.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aBomGoods.F_CreatorTime = aBomGoods.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<ABomEntity>(new ABomEntity()));
        ConfigModel tableFieldd2a80dConfig = new ConfigModel
        {
            tableName = "a_bom_goods",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "设计子表",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<ABomGoodsEntity>(new ABomGoodsEntity())
        };
        FieldsModel tableFieldd2a80d = new FieldsModel()
        {
            __config__ = tableFieldd2a80dConfig,
            __vModel__ = "tableFieldd2a80d"
        };
        fieldList.Add(tableFieldd2a80d);

        resData = await _controlParsing.GetParsDataList(resData, "F_GoodsId,tableFieldd2a80d-F_GoodsId", "popupSelect", _userManager.TenantId, fieldList);
        // enrich child table rows with additional fields from AGoodsEntity (by F_GoodsId_id)
        try
        {
            var goodsIds = new List<string>();
            foreach (var item in resData)
            {
                if (item.ContainsKey("tableFieldd2a80d") && item["tableFieldd2a80d"] is List<Dictionary<string, object>> tableRows)
                {
                    foreach (var row in tableRows)
                    {
                        if (row.TryGetValue("F_GoodsId_id", out var gid) && gid != null)
                        {
                            var idStr = gid.ToString();
                            if (!string.IsNullOrEmpty(idStr)) goodsIds.Add(idStr);
                        }
                    }
                }
            }
            var distinctGoodsIds = goodsIds.Distinct().ToList();
            if (distinctGoodsIds.Any())
            {
                var goodsList = await _repositorysp.AsSugarClient().Queryable<AGoodsEntity>()
                    .Where(it => distinctGoodsIds.Contains(it.id))
                    .Select(it => new AGoodsEntity
                    {
                        id = it.id,
                        F_GoodsNo = it.F_GoodsNo,
                        F_Unit = it.F_Unit,
                        F_Source = it.F_Source,
                        F_Specification = it.F_Specification,
                        F_SalePrice = it.F_SalePrice,
                        F_CostPrice = it.F_CostPrice,
                        F_Image = it.F_Image
                    }).ToListAsync();

                var goodsDict = goodsList.ToDictionary(g => g.id, g => g);
                foreach (var item in resData)
                {
                    if (item.ContainsKey("tableFieldd2a80d") && item["tableFieldd2a80d"] is List<Dictionary<string, object>> tableRows)
                    {
                        foreach (var row in tableRows)
                        {
                            if (row.TryGetValue("F_GoodsId_id", out var gid) && gid != null)
                            {
                                var idStr = gid.ToString();
                                if (goodsDict.TryGetValue(idStr, out var goods))
                                {
                                    // set additional fields (override if exists)
                                    row["F_GoodsNo"] = goods.F_GoodsNo;
                                    // keep child table's original F_Unit, put goods table's unit into F_Unitsp
                                    row["F_Source"] = goods.F_Source;
                                    row["F_Specification"] = goods.F_Specification;
                                    row["F_SalePrice"] = goods.F_SalePrice;
                                    row["F_CostPrice"] = goods.F_CostPrice;
                                    row["F_Image"] = goods.F_Image;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            // ignore enrichment errors, return base data
        }

        return resData.FirstOrDefault();
    }

    /// <summary>
    /// 获取BOM列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("PopupList")]
    public async Task<dynamic> GetPopupList(string goodsId)
    {
        var data = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(goodsId), it => it.F_GoodsId == goodsId)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .OrderBy("F_CreatorTime desc")
            .Select(it => new ABomListOutput
            {
                id = it.id,
                F_BomCode = it.F_BomCode,
                F_BomName = it.F_BomName,
                F_GoodsId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
            })
            .ToListAsync();


        return new {
            data = data
        };
    }


    /// <summary>
    /// 根据BOM获取用料清单.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("BOMGoodsList/{bomId}")]
    public async Task<dynamic> GetBOMGoodsList(string bomId)
    {
        var data = await _repositoryGoods.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_ParentId == bomId)
            .OrderBy("F_CreatorTime desc")
            .Select(it => new ABomGoodsListOutput
            {
                id = it.F_Id,
                F_GoodsId = it.F_GoodsId,
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_UnitTwo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit),
                F_Unit = it.F_Unit,
                F_InventoryNum = SqlFunc.Subqueryable<AGoodsInventoryEntity>().EnableTableFilter().Where(dic => dic.F_GoodsId.Equals(it.F_GoodsId) && dic.DeleteMark == null && dic.F_Num > 0).Sum(it => it.F_Num) ?? 0,
            })
            .ToListAsync();
        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            // 单位（级联路径，如 箱子/盒/瓶，直接查 AGoodsSpecificationEntity）
            if (!string.IsNullOrEmpty(item.F_UnitTwo))
            {
                var unitIds = item.F_UnitTwo.ToObject<List<string>>();
                var specData = await _repositorygg.AsSugarClient().Queryable<AGoodsSpecificationEntity>()
                    .Where(it => unitIds.Contains(it.id) && it.DeleteMark == null)
                    .Select(it => new { it.id, it.F_Name })
                    .ToListAsync();
                var specMap = specData.ToDictionary(it => it.id, it => it.F_Name ?? "");
                item.F_UnitTwo = string.Join("/", unitIds.Where(id => specMap.ContainsKey(id)).Select(id => specMap[id]));
                if(specData.Count > 1)
                {
                    item.F_NumUnit = specData[1]?.F_Name;
                }
                else
                {
                    item.F_NumUnit = specData[0]?.F_Name;
                }
            }


        });


        return data;
    }




}
