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
using JNPF.example.Entitys.Dto.AConfig;
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
/// 业务实现：配置信息.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AConfig", Order = 200)]
[Route("api/example/[controller]")]
public class AConfigService : IAConfigService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AConfigEntity> _repository;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"编码\",\"field\":\"F_Code\"},{\"value\":\"名称\",\"field\":\"F_Name\"},{\"value\":\"值\",\"field\":\"F_Value\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AConfigService"/>类型的新实例.
    /// </summary>
    public AConfigService(
        ISqlSugarRepository<AConfigEntity> repository,
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
    /// 获取配置信息.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .FirstAsync(it => it.id.Equals(id))).Adapt<AConfigInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取配置信息列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AConfigListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AConfigListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AConfigEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_Name), it => it.F_Name.Contains(input.F_Name))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new AConfigListOutput
            {
                id = it.id,
                F_Name = it.F_Name,
                F_Value = SqlFunc.IIF(SqlFunc.ToInt32(it.F_Value) == 1, "开", "关"),
                F_Code = it.F_Code,
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        return PageResult<AConfigListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建配置信息.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AConfigCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AConfigEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AConfigEntity>(new AConfigEntity()));
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 获取配置信息无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(AConfigListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AConfigListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<AConfigEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        var selectIds = input.selectIds?.Split(",").ToList();
        var list = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_Name), it => it.F_Name.Contains(input.F_Name))
            .Where(authorizeWhere)
            .Where(mainConditionalModel)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new AConfigListOutput
            {
                id = it.id,
                F_Name = it.F_Name,
                F_Value = SqlFunc.IIF(SqlFunc.ToInt32(it.F_Value) == 1, "开", "关"),
                F_Code = it.F_Code,
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToListAsync();

        return list;
    }

    /// <summary>
    /// 导出配置信息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] AConfigListQueryInput input)
    {
        var exportData = new List<AConfigListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<AConfigListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName,input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新配置信息.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AConfigUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AConfigEntity>();
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_Name,
                it.F_Value,
                it.F_Code,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }
  
    /// <summary>
    /// 删除配置信息.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除配置信息.
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
            // 批量删除配置信息
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 配置信息详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AConfigDetailOutput
            {
                id = it.id,
                F_Name = it.F_Name,
                F_Value = SqlFunc.IIF(SqlFunc.ToInt32(it.F_Value) == 1, "开", "关"),
                F_Code = it.F_Code,
            }).MergeTable().Where(it => it.id == id).ToListAsync();
        return data.FirstOrDefault();
    }


    /// <summary>
    /// 获取固定三条配置（商品管理）
    /// </summary>
    /// <returns></returns>
    /// <summary>
    /// 根据传入的 id 列表获取并可更新对应的 F_Value（支持 "id-value" 格式，例如 "20087...-0"）
    /// 如果前端不传参数，则回退到默认的三个固定 id。
    /// </summary>
    /// <param name="fIds">id 或 id-value 的字符串列表</param>
    [HttpPost("GetByFixedIds")]
    [UnitOfWork]
    public async Task<dynamic> GetByFixedIds([FromBody] List<string> fIds)
    {
        // 如果前端未传入，使用默认固定 id 列表（兼容原逻辑）
        var defaultIds = new List<string>
        {
            "2008738234443632640",
            "2008738298582929408",
            "2008738342568595456"
        };

        // 解析前端传入的 id/value 对（格式: id 或 id-value）
        var parsedList = new List<(string id, string? value)>();
        if (fIds == null || !fIds.Any())
        {
            parsedList.AddRange(defaultIds.Select(d => (d, (string?)null)));
        }
        else
        {
            foreach (var raw in fIds)
            {
                if (string.IsNullOrWhiteSpace(raw)) continue;
                var parts = raw.Split(new[] { '-' }, 2);
                var id = parts[0].Trim();
                var val = parts.Length > 1 ? parts[1].Trim() : null;
                if (!string.IsNullOrEmpty(id))
                {
                    parsedList.Add((id, val));
                }
            }
        }

        var ids = parsedList.Select(p => p.id).Distinct().ToList();

        // 查询数据库中存在的记录
        var entities = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .In(it => it.id, ids)
            .ToListAsync();

        if (entities != null && entities.Any())
        {
            // 根据前端提供的 value 更新对应的 F_Value（只有当 value 不为 null 时才更新）
            foreach (var entity in entities)
            {
                var match = parsedList.FirstOrDefault(p => p.id == entity.id);
                if (match != default && match.value != null)
                {
                    // 直接更新该字段
                    var updateEntity = new AConfigEntity
                    {
                        id = entity.id,
                        F_Value = match.value
                    };
                    var isOk = await _repository.AsUpdateable(updateEntity)
                        .UpdateColumns(it => new
                        {
                            it.F_Value
                        }).ExecuteCommandAsync();
                    if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
                    // 将本地对象同步为最新值，便于返回
                    entity.F_Value = match.value;
                }
            }
        }

        // 返回结果（与前端期望格式保持一致：F_Value 显示为 "开"/"关"）
        var result = entities.Select(it => new AConfigListOutput
        {
            id = it.id,
            F_Name = it.F_Name,
            F_Value = SqlFunc.IIF(SqlFunc.ToInt32(it.F_Value) == 1, "开", "关"),
            F_Code = it.F_Code
        }).ToList();

        return result;
    }



    /// <summary>
    /// 获取固定三条配置（客户管理）
    /// </summary>
    /// <returns></returns>
    /// <summary>
    /// 根据传入的 id 列表获取并可更新对应的 F_Value（支持 "id-value" 格式，例如 "20087...-0"）
    /// 如果前端不传参数，则回退到默认的三个固定 id。
    /// </summary>
    /// <param name="fIds">id 或 id-value 的字符串列表</param>
    [HttpPost("customer")]
    [UnitOfWork]
    public async Task<dynamic> Customer([FromBody] List<string> fIds)
    {
        // 如果前端未传入，使用默认固定 id 列表（兼容原逻辑）
        var defaultIds = new List<string>
        {
            "2009104993759662080",
            "2009105040731672576",
        };

        // 解析前端传入的 id/value 对（格式: id 或 id-value）
        var parsedList = new List<(string id, string? value)>();
        if (fIds == null || !fIds.Any())
        {
            parsedList.AddRange(defaultIds.Select(d => (d, (string?)null)));
        }
        else
        {
            foreach (var raw in fIds)
            {
                if (string.IsNullOrWhiteSpace(raw)) continue;
                var parts = raw.Split(new[] { '-' }, 2);
                var id = parts[0].Trim();
                var val = parts.Length > 1 ? parts[1].Trim() : null;
                if (!string.IsNullOrEmpty(id))
                {
                    parsedList.Add((id, val));
                }
            }
        }

        var ids = parsedList.Select(p => p.id).Distinct().ToList();

        // 查询数据库中存在的记录
        var entities = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
            .In(it => it.id, ids)
            .ToListAsync();

        if (entities != null && entities.Any())
        {
            // 根据前端提供的 value 更新对应的 F_Value（只有当 value 不为 null 时才更新）
            foreach (var entity in entities)
            {
                var match = parsedList.FirstOrDefault(p => p.id == entity.id);
                if (match != default && match.value != null)
                {
                    // 直接更新该字段
                    var updateEntity = new AConfigEntity
                    {
                        id = entity.id,
                        F_Value = match.value
                    };
                    var isOk = await _repository.AsUpdateable(updateEntity)
                        .UpdateColumns(it => new
                        {
                            it.F_Value
                        }).ExecuteCommandAsync();
                    if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
                    // 将本地对象同步为最新值，便于返回
                    entity.F_Value = match.value;
                }
            }
        }

        // 返回结果（与前端期望格式保持一致：F_Value 显示为 "开"/"关"）
        var result = entities.Select(it => new AConfigListOutput
        {
            id = it.id,
            F_Name = it.F_Name,
            F_Value = SqlFunc.IIF(SqlFunc.ToInt32(it.F_Value) == 1, "开", "关"),
            F_Code = it.F_Code
        }).ToList();

        return result;
    }




}
