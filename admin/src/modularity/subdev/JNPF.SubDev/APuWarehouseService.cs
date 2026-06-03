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
using JNPF.example.Entitys.Dto.APuWarehouse;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.DatabaseAccessor;
using JNPF.Common.Dtos;
using System.Linq.Expressions;

namespace JNPF.example;

/// <summary>
/// 业务实现：仓库管理2.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuWarehouse", Order = 200)]
[Route("api/example/[controller]")]
public class APuWarehouseService : IAPuWarehouseService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuWarehouseEntity> _repository;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"上级\",\"field\":\"F_ParentId\"},{\"value\":\"启用状态\",\"field\":\"F_State\"},{\"value\":\"仓库名\",\"field\":\"F_Name\"},{\"value\":\"仓库位置\",\"field\":\"F_Location\"},{\"value\":\"详细地址\",\"field\":\"F_Address\"},{\"value\":\"二维码\",\"field\":\"F_QRCode\"},{\"value\":\"管理员\",\"field\":\"F_UsersId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="APuWarehouseService"/>类型的新实例.
    /// </summary>
    public APuWarehouseService(
        ISqlSugarRepository<APuWarehouseEntity> repository,
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
    /// 获取仓库管理2.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new APuWarehouseEntity
            {
                id = it.id,
                F_ParentId = it.F_ParentId,
                F_Name = it.F_Name,
                F_UsersId = it.F_UsersId,
                F_Location = it.F_Location,
                F_Address = it.F_Address,
                F_QRCode = it.F_QRCode,
                F_State = it.F_State,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuWarehouseInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取仓库管理2列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuWarehouseListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<APuWarehouseListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var F_ParentId = input.F_ParentId?.Last();
        var F_Location = input.F_Location?.Last();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ParentId?.ToString()), it => it.F_ParentId.Contains(F_ParentId))
            .WhereIF(!string.IsNullOrEmpty(input.F_Name), it => it.F_Name.Contains(input.F_Name))
            .WhereIF(!string.IsNullOrEmpty(input.F_UsersId), it => it.F_UsersId.Contains(input.F_UsersId))
            .WhereIF(!string.IsNullOrEmpty(input.F_Location?.ToString()), it => it.F_Location.Contains(F_Location))
            .WhereIF(!string.IsNullOrEmpty(input.F_Address), it => it.F_Address.Contains(input.F_Address))
            .WhereIF(!string.IsNullOrEmpty(input.F_QRCode), it => it.F_QRCode.Contains(input.F_QRCode))
            .WhereIF(input.F_CreatorTime?.Count() > 0, it => SqlFunc.Between(it.F_CreatorTime, input.F_CreatorTime.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_CreatorTime.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new APuWarehouseListOutput
            {
                id = it.id,
                F_ParentId = it.F_ParentId,
                F_Name = it.F_Name,
                F_UsersId = it.F_UsersId,
                F_Location = it.F_Location,
                F_Address = it.F_Address,
                F_QRCode = it.F_QRCode,
                F_State = SqlFunc.IIF(SqlFunc.ToInt32(it.F_State) == 1, "启用", "禁用"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_Address).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 上级ID
            var F_ParentIdData = await _dataInterfaceService.GetDynamicList("F_ParentId", "2021045201115680768", "F_Id", "F_Name", "children");
            if(item.F_ParentId != null)
            {
                var F_ParentIdCascader = item.F_ParentId.ToObject<List<string>>();
                item.F_ParentId = F_ParentIdData.FindAll(it => F_ParentIdCascader.Contains(it.id)).Select(s => s.fullName).FirstOrDefault();
            }

            // 管理员
            if(item.F_UsersId != null)
            {
                var F_UsersIdUserSelect = item.F_UsersId.ToObject<List<string>>();
                item.F_UsersId = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_UsersIdUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 仓库位置
            if(item.F_Location != null)
            {
                var F_LocationAreaSelect = item.F_Location.ToObject<List<string>>();
                item.F_Location = string.Join("/", await _repository.AsSugarClient().Queryable<ProvinceEntity>().Where(it => F_LocationAreaSelect.Contains(it.Id)).Select(it => it.FullName).ToListAsync());
            }

        });

        return PageResult<APuWarehouseListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建仓库管理2.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] APuWarehouseCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuWarehouseEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuWarehouseEntity>(new APuWarehouseEntity()));
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        if(await _repository.IsAnyAsync(it => it.F_Name.Equals(input.F_Name) && it.FlowId==null   && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "仓库名");

        // 解析上级路径
        var parentIds = input.F_ParentId?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>();
        entity.F_ParentId = parentIds.Count == 0 ? null : parentIds.ToJsonString();

        // 前端传入地址时校验唯一性，未传则自动生成
        if (!string.IsNullOrWhiteSpace(input.F_Address))
        {
            var siblings = await _repository.AsQueryable()
                .Where(it => it.DeleteMark == null && it.FlowId == null)
                .Where(it => ParentPathEquals(it.F_ParentId, parentIds) && it.F_Address == input.F_Address)
                .ToListAsync();
            if (siblings.Any())
                throw Oops.Bah(ErrorCode.COM1023, "同一上级下地址不能重复");
        }
        else
        {
            await TryAssignAutoWarehouseAddressAsync(entity);
        }

        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 未填写详细地址编码时，按层级自动生成：总仓2位 + 货架4位 + 料位3位 + 槽位3位（如 010001001001）.
    /// </summary>
    private static List<string> ParseParentIdPath(string? fParentId)
    {
        if (string.IsNullOrWhiteSpace(fParentId)) return new List<string>();
        var t = fParentId.Trim();
        if (t == "[]") return new List<string>();
        try
        {
            var list = fParentId.ToObject<List<string>>();
            return list?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }

    private static bool ParentPathEquals(string? stored, List<string> targetPath)
    {
        var p = ParseParentIdPath(stored);
        if (p.Count != targetPath.Count) return false;
        for (var i = 0; i < p.Count; i++)
        {
            if (!string.Equals(p[i], targetPath[i], StringComparison.Ordinal)) return false;
        }
        return true;
    }

    private async Task TryAssignAutoWarehouseAddressAsync(APuWarehouseEntity entity)
    {
        if (!string.IsNullOrWhiteSpace(entity.F_Address)) return;

        var parentIds = ParseParentIdPath(entity.F_ParentId);
        entity.F_ParentId = parentIds.Count == 0 ? null : parentIds.ToJsonString();

        var depth = parentIds.Count + 1;
        if (depth > 4) throw Oops.Bah("仓库结构最多支持四级（总仓-货架-料位-槽位）");

        // 各级新增段的宽度：2 + 4 + 3 + 3
        var segmentWidths = new[] { 2, 4, 3, 3 };
        var segmentLen = segmentWidths[depth - 1];

        var prefixLen = 0;
        for (var i = 0; i < depth - 1; i++) prefixLen += segmentWidths[i];

        string prefix = string.Empty;
        if (depth > 1)
        {
            var immediateParentId = parentIds[^1];
            var parentRow = await _repository.AsQueryable()
                .Where(it => it.id == immediateParentId && it.DeleteMark == null && it.FlowId == null)
                .Select(it => it.F_Address)
                .Take(1)
                .ToListAsync();
            if (parentRow.Count == 0) throw Oops.Bah("上级节点不存在或已删除");
            prefix = parentRow[0]?.Trim() ?? string.Empty;
            if (prefix.Length != prefixLen || !Regex.IsMatch(prefix, @"^\d+$", RegexOptions.None))
                throw Oops.Bah("上级节点的地址编码缺失或不是标准数字格式，请先维护上级地址编码后再新增");
        }

        var fullLen = prefixLen + segmentLen;

        List<APuWarehouseEntity> siblings;
        if (parentIds.Count == 0)
        {
            siblings = await _repository.AsQueryable()
                .Where(it => it.DeleteMark == null && it.FlowId == null)
                .Where(it => it.F_ParentId == null || it.F_ParentId == string.Empty || it.F_ParentId == "[]")
                .Select(it => new APuWarehouseEntity { F_Address = it.F_Address })
                .ToListAsync();
        }
        else
        {
            var lastId = parentIds[^1];
            var candidates = await _repository.AsQueryable()
                .Where(it => it.DeleteMark == null && it.FlowId == null)
                .Where(it => it.F_ParentId != null && it.F_ParentId.Contains(lastId))
                .Select(it => new APuWarehouseEntity { F_Address = it.F_Address, F_ParentId = it.F_ParentId })
                .ToListAsync();
            siblings = candidates.Where(it => ParentPathEquals(it.F_ParentId, parentIds)).ToList();
        }

        var maxSeg = 0;
        foreach (var s in siblings)
        {
            var addr = s.F_Address?.Trim();
            if (string.IsNullOrEmpty(addr) || !Regex.IsMatch(addr, @"^\d+$")) continue;
            if (addr.Length != fullLen) continue;
            if (depth > 1 && !addr.StartsWith(prefix, StringComparison.Ordinal)) continue;
            var segStr = addr.Substring(prefixLen, segmentLen);
            if (int.TryParse(segStr, out var n) && n > maxSeg) maxSeg = n;
        }

        var next = maxSeg + 1;
        var maxValue = (int)Math.Pow(10, segmentLen) - 1;
        if (next > maxValue) throw Oops.Bah($"当前层级地址编码已超过允许的最大序号（{maxValue}）");

        var segment = next.ToString().PadLeft(segmentLen, '0');
        entity.F_Address = prefix + segment;
    }

    /// <summary>
    /// 更新仓库管理2.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] APuWarehouseUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuWarehouseEntity>();
        if (await _repository.IsAnyAsync(it => it.F_Name.Equals(input.F_Name) && it.FlowId == null && it.DeleteMark == null && !it.id.Equals(id)))
            throw Oops.Bah(ErrorCode.COM1023, "仓库名");

        // 解析上级路径并转换为 JSON 字符串
        var parentIds = input.F_ParentId?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList() ?? new List<string>();
        entity.F_ParentId = parentIds.Count == 0 ? null : parentIds.ToJsonString();

        // 转换 List<string> 为 JSON 字符串
        entity.F_UsersId = input.F_UsersId != null && input.F_UsersId.Count > 0 ? input.F_UsersId.ToJsonString() : null;
        entity.F_Location = input.F_Location != null && input.F_Location.Count > 0 ? input.F_Location.ToJsonString() : null;

        // 地址唯一性校验（仅在地址非空时执行）
        if (!string.IsNullOrWhiteSpace(input.F_Address))
        {
            // 将上级路径转为 JSON 字符串，用直接比较替代 ParentPathEquals（SqlSugar 无法翻译 C# 方法为 SQL）
            var parentIdJson = parentIds.Count == 0 ? null : parentIds.ToJsonString();
            Expression<Func<APuWarehouseEntity, bool>> parentIdCondition;
            if (parentIdJson == null)
            {
                parentIdCondition = it => (it.F_ParentId == null || it.F_ParentId == "[]" || it.F_ParentId == string.Empty);
            }
            else
            {
                parentIdCondition = it => it.F_ParentId == parentIdJson;
            }

            var siblings = await _repository.AsQueryable()
                .Where(it => it.DeleteMark == null && it.FlowId == null && !it.id.Equals(id))
                .Where(parentIdCondition)
                .Where(it => it.F_Address == input.F_Address)
                .ToListAsync();
            if (siblings.Any())
            {
                throw Oops.Bah(ErrorCode.COM1023, "同一上级下地址不能重复");
            }
        }
        else
        {
            // 地址为空时自动生成
            await TryAssignAutoWarehouseAddressAsync(entity);
        }

        var isOk = 0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new {
                it.F_ParentId,
                it.F_Name,
                it.F_UsersId,
                it.F_Location,
                it.F_Address,
                it.F_QRCode,
                it.F_State,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除仓库管理2.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除仓库管理2.
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
            // 批量删除仓库管理2
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 仓库管理2详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new APuWarehouseDetailOutput
            {
                id = it.id,
                F_ParentId = it.F_ParentId,
                F_Name = it.F_Name,
                F_UsersId = it.F_UsersId,
                F_Location = it.F_Location,
                F_Address = it.F_Address,
                F_QRCode = it.F_QRCode,
                F_State = SqlFunc.IIF(SqlFunc.ToInt32(it.F_State) == 1, "启用", "禁用"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            var F_ParentIdData = await _dataInterfaceService.GetDynamicList("F_ParentId", "2021045201115680768", "F_Id", "F_Name", "children");
            if(item.F_ParentId != null)
            {
                var F_ParentIdCascader = item.F_ParentId.ToObject<List<string>>();
                item.F_ParentId = F_ParentIdData.FindAll(it => F_ParentIdCascader.Contains(it.id)).Select(s => s.fullName).FirstOrDefault();
            }

            // 管理员
            if(item.F_UsersId != null)
            {
                var F_UsersIdUserSelect = item.F_UsersId.ToObject<List<string>>();
                item.F_UsersId = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => F_UsersIdUserSelect.Contains(a.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 仓库位置
            if(item.F_Location != null)
            {
                var F_LocationAreaSelect = item.F_Location.ToObject<List<string>>();
                item.F_Location = string.Join("/", await _repository.AsSugarClient().Queryable<ProvinceEntity>().Where(it => F_LocationAreaSelect.Contains(it.Id)).Select(it => it.FullName).ToListAsync());
            }

        });
        return data.FirstOrDefault();
    }


    /// <summary>
    /// APP通过id获取仓库消息.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("WarehouseById")]
    public async Task<dynamic> GetWarehouseById(string code)
    {
        // 查询当前仓库信息
        var warehouse = await _repository.AsQueryable()
            .Where(it => it.F_Address == code)
            .Where(it => it.DeleteMark == null)
            .FirstAsync();

        if (warehouse == null)
        {
            return null;
        }

        // 处理 F_ParentId：将当前 id 拼接到 F_ParentId 数组中
        var parentIdList = new List<string>();
        if (!string.IsNullOrEmpty(warehouse.F_ParentId))
        {
            parentIdList = warehouse.F_ParentId.ToObject<List<string>>() ?? new List<string>();
        }
        parentIdList.Add(warehouse.id);

        // 处理 F_Name：获取上级仓库名称，用 "/" 拼接
        var nameList = new List<string>();
        if (!string.IsNullOrEmpty(warehouse.F_ParentId))
        {
            var parentIds = warehouse.F_ParentId.ToObject<List<string>>();
            if (parentIds != null && parentIds.Any())
            {
                var parentNames = await _repository.AsQueryable()
                    .Where(it => parentIds.Contains(it.id))
                    .Where(it => it.DeleteMark == null)
                    .Select(it => it.F_Name)
                    .ToListAsync();
                nameList.AddRange(parentNames);
            }
        }
        nameList.Add(warehouse.F_Name);

        return new
        {
            id = warehouse.id,
            F_ParentId = parentIdList,
            F_Name = string.Join("/", nameList)
        };
    }

}
