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
using JNPF.example.Entitys.Dto.AProdOutsourceAccept;
using JNPF.example.Entitys.Dto.AProdOutsourceAcceptItem;
using JNPF.example.Entitys.Dto.AProdOutsourceSettleLog;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_prod_outsource_accept.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdOutsourceAccept", Order = 200)]
[Route("api/example/[controller]")]
public class AProdOutsourceAcceptService : IAProdOutsourceAcceptService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdOutsourceAcceptEntity> _repository;
    private readonly ISqlSugarRepository<AProdOutsourceEntity> _repositoryOut;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"外协工单\",\"field\":\"F_OutsourceId\"},{\"value\":\"合格数量\",\"field\":\"F_PassNum\"},{\"value\":\"不合格数量\",\"field\":\"F_FailNum\"},{\"value\":\"外协类型\",\"field\":\"F_OutsourceType\"},{\"value\":\"结算状态\",\"field\":\"F_SettleStatus\"},{\"value\":\"结算单价\",\"field\":\"F_SettleUnitPrice\"},{\"value\":\"结算人\",\"field\":\"F_SettleUserId\"},{\"value\":\"结算时间\",\"field\":\"F_SettleTime\"},{\"value\":\"结算附件\",\"field\":\"F_SettleFiles\"},{\"value\":\"结算备注\",\"field\":\"F_SettleDesc\"},{\"value\":\"创建用户\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdOutsourceAcceptService"/>类型的新实例.
    /// </summary>
    public AProdOutsourceAcceptService(
        ISqlSugarRepository<AProdOutsourceEntity> repositoryOut,
        ISqlSugarRepository<AProdOutsourceAcceptEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryOut = repositoryOut;
        _repository = repository;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_prod_outsource_accept.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.AProdOutsourceAcceptItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.AProdOutsourceSettleLogList)
            .Select(it => new AProdOutsourceAcceptEntity
            {
                id = it.id,
                F_OutsourceId = it.F_OutsourceId,
                F_PassNum = it.F_PassNum,
                F_FailNum = it.F_FailNum,
                F_OutsourceType = it.F_OutsourceType,
                F_SettleStatus = it.F_SettleStatus,
                F_SettleUnitPrice = it.F_SettleUnitPrice,
                F_SettleUserId = it.F_SettleUserId,
                F_SettleTime = it.F_SettleTime,
                F_SettleFiles = it.F_SettleFiles,
                F_SettleDesc = it.F_SettleDesc,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                AProdOutsourceAcceptItemList = it.AProdOutsourceAcceptItemList.Where(it => it.DeleteMark == null).ToList(),
                AProdOutsourceSettleLogList = it.AProdOutsourceSettleLogList.Where(it => it.DeleteMark == null).ToList(),
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AProdOutsourceAcceptInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField561a64, async aProdOutsourceAcceptItem =>
        {
            // 创建用户
            aProdOutsourceAcceptItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aProdOutsourceAcceptItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aProdOutsourceAcceptItem.F_CreatorTime = aProdOutsourceAcceptItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField4b440e, async aProdOutsourceSettleLog =>
        {
            // 创建用户
            aProdOutsourceSettleLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aProdOutsourceSettleLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aProdOutsourceSettleLog.F_CreatorTime = aProdOutsourceSettleLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

    /// <summary>
    /// 获取a_prod_outsource_accept列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdOutsourceAcceptListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aProdOutsourceAcceptItemAuthorizeWhere = new List<IConditionalModel>();
        var aProdOutsourceSettleLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<AProdOutsourceAcceptListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_prod_outsource_accept"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_prod_outsource_accept_item"))) aProdOutsourceAcceptItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_prod_outsource_accept_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_prod_outsource_settle_log"))) aProdOutsourceSettleLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_prod_outsource_settle_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AProdOutsourceAcceptEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AProdOutsourceAcceptItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField561a64-", entityInfo, 1);
        List<IConditionalModel> aProdOutsourceAcceptItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aProdOutsourceAcceptItemConditionalModel.AddRange(aProdOutsourceAcceptItemAuthorizeWhere);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AProdOutsourceSettleLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField4b440e-", entityInfo, 1);
        List<IConditionalModel> aProdOutsourceSettleLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aProdOutsourceSettleLogConditionalModel.AddRange(aProdOutsourceSettleLogAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AProdOutsourceAcceptEntity>();
        var F_SettleUserIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_SettleUserId").DbColumnName;
        var idsQ = await _repository.AsQueryable()
            .WhereIF(selectIds != null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_OutsourceType), it => it.F_OutsourceType.Contains(input.F_OutsourceType))
            .WhereIF(!string.IsNullOrEmpty(input.F_SettleStatus), it => it.F_SettleStatus.Equals(input.F_SettleStatus))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_SettleUserIdDbColumnName, input.F_SettleUserId))
            .WhereIF(input.F_SettleTime?.Count() > 0, it => SqlFunc.Between(it.F_SettleTime, input.F_SettleTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_SettleTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .WhereIF(aProdOutsourceAcceptItemAuthorizeWhere?.Count > 0, it => it.AProdOutsourceAcceptItemList.Any(aProdOutsourceAcceptItemAuthorizeWhere))
            .WhereIF(aProdOutsourceSettleLogAuthorizeWhere?.Count > 0, it => it.AProdOutsourceSettleLogList.Any(aProdOutsourceSettleLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aProdOutsourceAcceptItemConditionalModel.Count > 0, it => it.AProdOutsourceAcceptItemList.Any(aProdOutsourceAcceptItemConditionalModel))
            .WhereIF(aProdOutsourceSettleLogConditionalModel.Count > 0, it => it.AProdOutsourceSettleLogList.Any(aProdOutsourceSettleLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId == null)
            .Select(it => new AProdOutsourceAcceptListOutput
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
            .Select(it => new AProdOutsourceAcceptListOutput
            {
                id = it.id,
                F_OutsourceId = SqlFunc.Subqueryable<AProdOutsourceEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_OutsourceId)).Select(u => u.F_OutsourceNo),
                F_PassNum = it.F_PassNum.ToString(),
                F_FailNum = it.F_FailNum.ToString(),
                F_OutsourceType = it.F_OutsourceType,
                F_SettleStatus = it.F_SettleStatus,
                F_SettleUnitPrice = it.F_SettleUnitPrice,
                F_SettleUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_SettleUserId)).Select(u => u.RealName),
                F_SettleTime = it.F_SettleTime.Value.ToString("yyyy-MM-dd"),
                F_SettleFiles = it.F_SettleFiles,
                F_SettleDesc = it.F_SettleDesc,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(1, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 结算状态
            if (item.F_SettleStatus != null)
            {
                item.F_SettleStatus = (await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().FirstAsync(dic => dic.EnCode.Equals(item.F_SettleStatus) && dic.DictionaryTypeId.Equals("2014214471169478656")))?.FullName;
            }

            if (item.F_SettleFiles != null)
            {
                item.F_SettleFiles = item.F_SettleFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_SettleFiles = new List<FileControlsModel>();
            }

        });

        data.pagination = idsQ.pagination;

        return PageResult<AProdOutsourceAcceptListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_prod_outsource_accept.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] AProdOutsourceAcceptCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdOutsourceAcceptEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdOutsourceAcceptEntity>(new AProdOutsourceAcceptEntity()));
        ConfigModel tableField561a64Config = new ConfigModel
        {
            tableName = "a_prod_outsource_accept_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择验收内容",
            children = ExportImportDataHelper.GetTemplateParsing<AProdOutsourceAcceptItemEntity>(new AProdOutsourceAcceptItemEntity())
        };
        FieldsModel tableField561a64Model = new FieldsModel()
        {
            __config__ = tableField561a64Config,
            __vModel__ = "tableField561a64"
        };
        fieldList.Add(tableField561a64Model);
        ConfigModel tableField4b440eConfig = new ConfigModel
        {
            tableName = "a_prod_outsource_settle_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "操作日志",
            children = ExportImportDataHelper.GetTemplateParsing<AProdOutsourceSettleLogEntity>(new AProdOutsourceSettleLogEntity())
        };
        FieldsModel tableField4b440eModel = new FieldsModel()
        {
            __config__ = tableField4b440eConfig,
            __vModel__ = "tableField4b440e"
        };
        fieldList.Add(tableField4b440eModel);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        var aProdOutsourceAcceptItemEntityList = input.tableField561a64.Adapt<List<AProdOutsourceAcceptItemEntity>>();
        if(aProdOutsourceAcceptItemEntityList != null)
        {
            foreach (var item in aProdOutsourceAcceptItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.AProdOutsourceAcceptItemList = aProdOutsourceAcceptItemEntityList;
        }

        var aProdOutsourceSettleLogEntityList = input.tableField4b440e.Adapt<List<AProdOutsourceSettleLogEntity>>();
        if(aProdOutsourceSettleLogEntityList != null)
        {
            foreach (var item in aProdOutsourceSettleLogEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.AProdOutsourceSettleLogList = aProdOutsourceSettleLogEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.AProdOutsourceAcceptItemList)
            .Include(it => it.AProdOutsourceSettleLogList)
            .ExecuteCommandAsync();

        //如果外协工单总合格数量大于计划数，则工单状态自动变为已完成
        var outsourceEntity = await _repositoryOut.GetFirstAsync(it => it.id == entity.F_OutsourceId && it.DeleteMark == null);
        if (outsourceEntity != null)
        {
            var outsourceAcceptNum = (await _repository.AsQueryable().Where(it => it.F_OutsourceId == entity.F_OutsourceId && it.DeleteMark == null).SumAsync(it => it.F_PassNum)) ?? 0;
            if ( outsourceAcceptNum >= outsourceEntity.F_PlanNum)
            {
                outsourceEntity.F_State = "2";
                await _repositoryOut.AsUpdateable(outsourceEntity)
                 .UpdateColumns(it => new {
                     it.F_State,
                 })
                 .ExecuteCommandAsync();
            }
        }

    }

    /// <summary>
    /// 更新a_prod_outsource_accept.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] AProdOutsourceAcceptUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdOutsourceAcceptEntity>();

        // 获取原始数据用于日志记录
        var originalEntity = await _repository.GetFirstAsync(it => it.id.Equals(id) && it.DeleteMark == null);
        if (originalEntity == null)
        {
            throw Oops.Oh(ErrorCode.COM1005);
        }
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdOutsourceAcceptEntity>(new AProdOutsourceAcceptEntity()));
        ConfigModel tableField561a64Config = new ConfigModel
        {
            tableName = "a_prod_outsource_accept_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择验收内容",
            children = ExportImportDataHelper.GetTemplateParsing<AProdOutsourceAcceptItemEntity>(new AProdOutsourceAcceptItemEntity())
        };
        FieldsModel tableField561a64Model = new FieldsModel()
        {
            __config__ = tableField561a64Config,
            __vModel__ = "tableField561a64"
        };
        fieldList.Add(tableField561a64Model);
        ConfigModel tableField4b440eConfig = new ConfigModel
        {
            tableName = "a_prod_outsource_settle_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "操作日志",
            children = ExportImportDataHelper.GetTemplateParsing<AProdOutsourceSettleLogEntity>(new AProdOutsourceSettleLogEntity())
        };
        FieldsModel tableField4b440eModel = new FieldsModel()
        {
            __config__ = tableField4b440eConfig,
            __vModel__ = "tableField4b440e"
        };
        fieldList.Add(tableField4b440eModel);

        // 移除外协验收内容可能被删除数据
        if (input.tableField561a64 !=null && input.tableField561a64.Any())
            await _repository.AsSugarClient().Deleteable<AProdOutsourceAcceptItemEntity>().Where(it => it.F_OutsourceAcceptId == entity.id && !input.tableField561a64.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<AProdOutsourceAcceptItemEntity>().Where(it => it.F_OutsourceAcceptId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增外协验收内容新数据
        var aProdOutsourceAcceptItemEntityList = input.tableField561a64.Adapt<List<AProdOutsourceAcceptItemEntity>>();


        // 移除外协结算日志可能被删除数据
        if (input.tableField4b440e !=null && input.tableField4b440e.Any())
            await _repository.AsSugarClient().Deleteable<AProdOutsourceSettleLogEntity>().Where(it => it.F_AcceptId == entity.id && !input.tableField4b440e.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<AProdOutsourceSettleLogEntity>().Where(it => it.F_AcceptId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增外协结算日志新数据
        var aProdOutsourceSettleLogEntityList = input.tableField4b440e.Adapt<List<AProdOutsourceSettleLogEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_OutsourceId,
                    it.F_PassNum,
                    it.F_FailNum,
                    it.F_OutsourceType,
                    it.F_SettleStatus,
                    it.F_SettleUnitPrice,
                    it.F_SettleUserId,
                    it.F_SettleTime,
                    it.F_SettleFiles,
                    it.F_SettleDesc,
                    it.F_Description,
                })
                .ExecuteCommandAsync();

            // 生成操作日志
            await GenerateOperationLogs(originalEntity, entity);

            if(aProdOutsourceAcceptItemEntityList != null)
            {
                foreach (var item in aProdOutsourceAcceptItemEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_OutsourceAcceptId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_OutsourceAcceptId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }

            if(aProdOutsourceSettleLogEntityList != null)
            {
                foreach (var item in aProdOutsourceSettleLogEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_AcceptId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_AcceptId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }


            //如果外协工单总合格数量大于计划数，则工单状态自动变为已完成
            //var outsourceEntity = await _repositoryOut.GetFirstAsync(it => it.id == entity.F_OutsourceId && it.DeleteMark == null);
            //if (outsourceEntity != null)
            //{
            //    var outsourceAcceptNum = (await _repository.AsQueryable().Where(it => it.F_OutsourceId == entity.F_OutsourceId && it.DeleteMark == null).SumAsync(it => it.F_PassNum)) ?? 0;
            //    if (outsourceAcceptNum >= outsourceEntity.F_PlanNum)
            //    {
            //        outsourceEntity.F_State = "2";
            //        await _repositoryOut.AsUpdateable(outsourceEntity)
            //         .UpdateColumns(it => new {
            //             it.F_State,
            //         })
            //         .ExecuteCommandAsync();
            //    }
            //    else
            //    {
            //        outsourceEntity.F_State = "1";
            //        await _repositoryOut.AsUpdateable(outsourceEntity)
            //         .UpdateColumns(it => new {
            //             it.F_State,
            //         })
            //         .ExecuteCommandAsync();
            //    }
            //}
        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 结算.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("Settlement/{id}")]
    [UnitOfWork]
    public async Task UpdateSettlement(string id, [FromBody] AProdOutsourceAcceptUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdOutsourceAcceptEntity>();
        entity.F_SettleUserId = _userManager.UserId;
        entity.F_SettleTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_SettleStatus,
                    it.F_SettleUnitPrice,
                    it.F_SettleUserId,
                    it.F_SettleTime,
                    it.F_SettleFiles,
                    it.F_SettleDesc,
                })
                .ExecuteCommandAsync();

        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 删除a_prod_outsource_accept.
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
    /// 批量删除a_prod_outsource_accept.
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
            // 批量删除a_prod_outsource_accept
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_prod_outsource_accept详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.AProdOutsourceAcceptItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.AProdOutsourceSettleLogList)
            .Where(it => it.id == id)
            .Select(it => new AProdOutsourceAcceptDetailOutput
            {
                id = it.id,
                F_OutsourceId = SqlFunc.Subqueryable<AProdOutsourceEntity>().EnableTableFilter().Where(u => u.id.Equals(it.F_OutsourceId)).Select(u => u.F_OutsourceNo),
                F_PassNum = it.F_PassNum.ToString(),
                F_FailNum = it.F_FailNum.ToString(),
                F_OutsourceType = it.F_OutsourceType,
                F_SettleStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_SettleStatus) && dic.DictionaryTypeId.Equals("2014214471169478656")).Select(dic => dic.FullName),
                F_SettleUnitPrice = it.F_SettleUnitPrice,
                F_SettleUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_SettleUserId)).Select(u => u.RealName),
                F_SettleTime = it.F_SettleTime.Value.ToString("yyyy-MM-dd"),
                F_SettleFiles = it.F_SettleFiles,
                F_SettleDesc = it.F_SettleDesc,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableField561a64 = it.AProdOutsourceAcceptItemList.Where(it => it.DeleteMark == null).Adapt<List<AProdOutsourceAcceptItemDetailOutput>>(),
                tableField4b440e = it.AProdOutsourceSettleLogList.Where(it => it.DeleteMark == null).Adapt<List<AProdOutsourceSettleLogDetailOutput>>(),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            if(item.F_SettleFiles != null)
            {
                item.F_SettleFiles = item.F_SettleFiles.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_SettleFiles = new List<FileControlsModel>();
            }

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField561a64), async aProdOutsourceAcceptItem =>
        {
            var aProdOutsourceAccept = data.ToList().Find(it => it.id.Equals(aProdOutsourceAcceptItem.F_OutsourceAcceptId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建用户
            aProdOutsourceAcceptItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aProdOutsourceAcceptItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aProdOutsourceAcceptItem.F_CreatorTime = aProdOutsourceAcceptItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField4b440e), async aProdOutsourceSettleLog =>
        {
            var aProdOutsourceAccept = data.ToList().Find(it => it.id.Equals(aProdOutsourceSettleLog.F_AcceptId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建用户
            aProdOutsourceSettleLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aProdOutsourceSettleLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aProdOutsourceSettleLog.F_CreatorTime = aProdOutsourceSettleLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<AProdOutsourceAcceptEntity>(new AProdOutsourceAcceptEntity()));
        ConfigModel tableField561a64Config = new ConfigModel
        {
            tableName = "a_prod_outsource_accept_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "选择验收内容",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<AProdOutsourceAcceptItemEntity>(new AProdOutsourceAcceptItemEntity())
        };
        FieldsModel tableField561a64 = new FieldsModel()
        {
            __config__ = tableField561a64Config,
            __vModel__ = "tableField561a64"
        };
        fieldList.Add(tableField561a64);

        resData = await _controlParsing.GetParsDataList(resData,"F_OutsourceId,tableField561a64-F_AcceptId","popupSelect",_userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }

    /// <summary>
    /// 生成操作日志
    /// </summary>
    /// <param name="originalEntity">原始实体</param>
    /// <param name="newEntity">新实体</param>
    /// <returns></returns>
    private async Task GenerateOperationLogs(AProdOutsourceAcceptEntity originalEntity, AProdOutsourceAcceptEntity newEntity)
    {
        var logRecords = new List<AProdOutsourceSettleLogEntity>();

        // 定义字段映射关系（字段名 -> 中文名称）
        var fieldMappings = new Dictionary<string, string>
        {
            { "F_OutsourceId", "外协工单" },
            { "F_PassNum", "合格数量" },
            { "F_FailNum", "不合格数量" },
            { "F_OutsourceType", "外协类型" },
            { "F_SettleStatus", "结算状态" },
            { "F_SettleUnitPrice", "结算单价" },
            { "F_SettleUserId", "结算人" },
            { "F_SettleTime", "结算时间" },
            { "F_SettleFiles", "结算附件" },
            { "F_SettleDesc", "结算备注" }
        };

        // 获取两个实体的属性
        var originalProperties = originalEntity.GetType().GetProperties();
        var newProperties = newEntity.GetType().GetProperties();

        foreach (var fieldName in fieldMappings.Keys)
        {
            var originalProperty = originalProperties.FirstOrDefault(p => p.Name == fieldName);
            var newProperty = newProperties.FirstOrDefault(p => p.Name == fieldName);

            if (originalProperty != null && newProperty != null)
            {
                var originalValue = originalProperty.GetValue(originalEntity);
                var newValue = newProperty.GetValue(newEntity);

                // 比较值是否发生变化
                if (!object.Equals(originalValue, newValue))
                {
                    var fieldDisplayName = fieldMappings[fieldName];

                    // 格式化值显示
                    var originalValueStr = FormatFieldValue(originalValue, fieldName);
                    var newValueStr = FormatFieldValue(newValue, fieldName);

                    // 创建日志记录
                    var logRecord = new AProdOutsourceSettleLogEntity
                    {
                        F_Id = SnowflakeIdHelper.NextId(),
                        F_AcceptId = newEntity.id,
                        F_Title = "修改数据",
                        F_Content = $"将{fieldDisplayName}从\"{originalValueStr}\"修改为\"{newValueStr}\"",
                        F_CreatorUserId = _userManager.UserId,
                        F_CreatorTime = DateTime.Now
                    };

                    logRecords.Add(logRecord);
                }
            }
        }

        // 批量插入日志记录
        if (logRecords.Any())
        {
            await _repository.AsSugarClient().Insertable(logRecords).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 格式化字段值用于日志显示
    /// </summary>
    /// <param name="value">字段值</param>
    /// <param name="fieldName">字段名</param>
    /// <returns>格式化后的字符串</returns>
    private string FormatFieldValue(object value, string fieldName)
    {
        if (value == null)
        {
            return "空";
        }

        // 特殊处理日期时间字段
        if (fieldName == "F_SettleTime" && value is DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // 特殊处理用户ID字段，转换为用户名
        if (fieldName == "F_SettleUserId" && value is string userId && !string.IsNullOrEmpty(userId))
        {
            try
            {
                var user = _repository.AsSugarClient().Queryable<UserEntity>()
                    .Where(u => u.Id == userId)
                    .Select(u => new { u.RealName, u.Account })
                    .First();
                return user != null ? $"{user.RealName}/{user.Account}" : userId;
            }
            catch
            {
                return userId;
            }
        }

        // 特殊处理结算状态字段，转换为字典值
        if (fieldName == "F_SettleStatus" && value is string statusCode && !string.IsNullOrEmpty(statusCode))
        {
            try
            {
                var dictData = _repository.AsSugarClient().Queryable<DictionaryDataEntity>()
                    .Where(d => d.EnCode == statusCode && d.DictionaryTypeId == "2014214471169478656")
                    .Select(d => d.FullName)
                    .First();
                return dictData ?? statusCode;
            }
            catch
            {
                return statusCode;
            }
        }

        return value.ToString();
    }
}
