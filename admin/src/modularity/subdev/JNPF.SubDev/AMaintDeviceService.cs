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
using JNPF.example.Entitys.Dto.AMaintDevice;
using JNPF.example.Entitys.Dto.AMaintDeviceItem;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_maint_device.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AMaintDevice", Order = 200)]
[Route("api/example/[controller]")]
public class AMaintDeviceService : IAMaintDeviceService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AMaintDeviceEntity> _repository;
    private readonly ISqlSugarRepository<AMaintDeviceItemEntity> _repositoryItem;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"设备编号\",\"field\":\"F_DeviceNo\"},{\"value\":\"设备名称\",\"field\":\"F_DeviceName\"},{\"value\":\"设备规格\",\"field\":\"F_DeviceSpec\"},{\"value\":\"设备状态\",\"field\":\"F_DeviceStatus\"},{\"value\":\"设备类别\",\"field\":\"F_DeviceType\"},{\"value\":\"设备二维码\",\"field\":\"F_DeviceQrCode\"},{\"value\":\"设备负责人\",\"field\":\"F_DeviceUsersId\"},{\"value\":\"分组\",\"field\":\"F_GroupId\"},{\"value\":\"车间\",\"field\":\"F_WorkshopId\"},{\"value\":\"产线\",\"field\":\"F_LineId\"},{\"value\":\"启用状态\",\"field\":\"F_EnabledMark\"},{\"value\":\"创建人\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AMaintDeviceService"/>类型的新实例.
    /// </summary>
    public AMaintDeviceService(
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<AMaintDeviceItemEntity> repositoryItem,
        ISqlSugarRepository<AMaintDeviceEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryInventory = repositoryInventory;
        _repositoryItem = repositoryItem;
        _repository = repository;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_maint_device.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.AMaintDeviceItemList)
            .Select(it => new AMaintDeviceEntity
            {
                id = it.id,
                F_DeviceNo = it.F_DeviceNo,
                F_DeviceName = it.F_DeviceName,
                F_DeviceSpec = it.F_DeviceSpec,
                F_DeviceStatus = it.F_DeviceStatus,
                F_DeviceType = it.F_DeviceType,
                F_GroupId = it.F_GroupId,
                F_DeviceUsersId = it.F_DeviceUsersId,
                F_WorkshopId = it.F_WorkshopId,
                F_LineId = it.F_LineId,
                F_DeviceQrCode = it.F_DeviceQrCode,
                F_DeviceImages = it.F_DeviceImages,
                F_EnabledMark = it.F_EnabledMark,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                AMaintDeviceItemList = it.AMaintDeviceItemList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AMaintDeviceInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField72c841, async aMaintDeviceItem =>
        {
            // 创建时间
            aMaintDeviceItem.F_CreatorTime = aMaintDeviceItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            var goodsEntity = await _repository.AsSugarClient().Queryable<AGoodsEntity>().Where(it => it.id.Equals(aMaintDeviceItem.F_GoodsId)).Select(it => new { F_Specification = it.F_Specification, F_Unit = it.F_Unit }).FirstAsync();

            aMaintDeviceItem.F_InventoryNum = (await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == aMaintDeviceItem.F_GoodsId && it.F_Num > 0).SumAsync(it => it.F_Num)) ?? 0;

            aMaintDeviceItem.F_Specification = goodsEntity.F_Specification;
            aMaintDeviceItem.F_Unit = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(goodsEntity.F_Unit) && it.DictionaryTypeId.Equals("2008448689420505088")).Select(it => it.FullName).FirstAsync();
        });
        return data;
    }

    /// <summary>
    /// 获取a_maint_device列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AMaintDeviceListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aMaintDeviceItemAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<AMaintDeviceListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_maint_device"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_maint_device_item"))) aMaintDeviceItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_maint_device_item"))?.conditionalModel;
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var F_DeviceType = input.F_DeviceType?.Last();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_DeviceNo), it => it.F_DeviceNo.Contains(input.F_DeviceNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_DeviceName), it => it.F_DeviceName.Contains(input.F_DeviceName))
            .WhereIF(!string.IsNullOrEmpty(input.F_DeviceStatus), it => it.F_DeviceStatus.Equals(input.F_DeviceStatus))
            .WhereIF(!string.IsNullOrEmpty(input.F_DeviceType?.ToString()), it => it.F_DeviceType.Contains(F_DeviceType))
            .Where(authorizeWhere)
            .WhereIF(aMaintDeviceItemAuthorizeWhere?.Count > 0, it => it.AMaintDeviceItemList.Any(aMaintDeviceItemAuthorizeWhere))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new AMaintDeviceListOutput
            {
                id = it.id,
                F_DeviceNo = it.F_DeviceNo,
                F_DeviceName = it.F_DeviceName,
                F_DeviceSpec = it.F_DeviceSpec,
                F_DeviceStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_DeviceStatus) && dic.DictionaryTypeId.Equals("2011620395194650624")).Select(dic => dic.FullName),
                F_DeviceStatusCode = it.F_DeviceStatus,
                F_DeviceType = it.F_DeviceType,
                F_GroupId = SqlFunc.Subqueryable<GroupEntity>().EnableTableFilter().Where(c => c.Id.Equals(it.F_GroupId)).Select(c => c.FullName),
                F_DeviceUsersId = it.F_DeviceUsersId,
                F_WorkshopId = it.F_WorkshopId,
                F_LineId = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_LineId) && dic.DictionaryTypeId.Equals("2011623836151320576")).Select(dic => dic.FullName),
                F_DeviceQrCode = !string.IsNullOrEmpty(it.F_DeviceQrCode) ? it.F_DeviceQrCode : "未绑定",
                F_EnabledMark = SqlFunc.IIF(SqlFunc.ToInt32(it.F_EnabledMark) == 1, "启用", "禁用"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            // 设备类别
            var F_DeviceTypeData = await _dataInterfaceService.GetDynamicList("F_DeviceType", "2011621811959238656", "F_Id", "F_Name", "children");
            if(item.F_DeviceType != null)
            {
                var F_DeviceTypeCascader = item.F_DeviceType.ToObject<List<string>>();
                item.F_DeviceType = string.Join(",", F_DeviceTypeData.FindAll(it => F_DeviceTypeCascader.Contains(it.id)).Select(s => s.fullName));
            }

            // 设备负责人
            if(item.F_DeviceUsersId != null)
            {
                var F_DeviceUsersIdUserSelect = item.F_DeviceUsersId.ToObject<List<string>>();
                item.F_DeviceUsersId = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => F_DeviceUsersIdUserSelect.Contains(it.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 车间
            var F_WorkshopIdData = await _dataInterfaceService.GetDynamicList("F_WorkshopId", "2011621894347952128", "F_Id", "F_Name", "children");
            if(item.F_WorkshopId != null)
            {
                var F_WorkshopIdCascader = item.F_WorkshopId.ToObject<List<string>>();
                item.F_WorkshopId = string.Join(",", F_WorkshopIdData.FindAll(it => F_WorkshopIdCascader.Contains(it.id)).Select(s => s.fullName));
            }
        });
        return PageResult<AMaintDeviceListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 获取商品列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("GoodsList")]
    public async Task<dynamic> GetGoodsList([FromBody] AMaintDeviceListQueryInput input)
    {
        var data = await _repositoryItem.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.F_DeviceId), it => it.F_DeviceId.Contains(input.F_DeviceId))
            .Select(it => new AMaintDeviceItemInfoOutput
            {
                F_Id = it.F_Id,
                F_GoodsId = it.F_GoodsId,
                F_GoodsName = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsName),
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_Unit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit)) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                F_InventoryNum = 0,
                F_Num = it.F_Num,
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_Id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);
        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            //库存
            var inventory = await _repositoryInventory.AsQueryable().Where(it => it.DeleteMark == null && it.F_GoodsId == item.F_GoodsId).SumAsync(it => it.F_Num);
            if (inventory != null)
            {
                item.F_InventoryNum += inventory;
            }

        });
        return PageResult<AMaintDeviceItemInfoOutput>.SqlSugarPageResult(data);
    }


    /// <summary>
    /// 新建a_maint_device.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] AMaintDeviceCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AMaintDeviceEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AMaintDeviceEntity>(new AMaintDeviceEntity()));
        ConfigModel tableField72c841Config = new ConfigModel
        {
            tableName = "a_maint_device_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "物料清单",
            children = ExportImportDataHelper.GetTemplateParsing<AMaintDeviceItemEntity>(new AMaintDeviceItemEntity())
        };
        FieldsModel tableField72c841Model = new FieldsModel()
        {
            __config__ = tableField72c841Config,
            __vModel__ = "tableField72c841"
        };
        fieldList.Add(tableField72c841Model);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        if (await _repository.IsAnyAsync(it => it.F_DeviceNo.Equals(input.F_DeviceNo) && it.FlowId == null && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "设备编号");

        // 自动生成编号逻辑：前缀 yyyyMM，后缀 3 位序号
        if (string.IsNullOrEmpty(entity.F_DeviceNo))
        {
            var prefix = "D" + DateTime.Now.ToString("yyyyMM");

            // 查询数据库中已有相同前缀的编号
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_DeviceNo != null && it.F_DeviceNo.StartsWith(prefix))
                .Select(it => it.F_DeviceNo)
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
            entity.F_DeviceNo = prefix + nextSeq.ToString("D3");
        }


        var aMaintDeviceItemEntityList = input.tableField72c841.Adapt<List<AMaintDeviceItemEntity>>();
        if(aMaintDeviceItemEntityList != null)
        {
            foreach (var item in aMaintDeviceItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.AMaintDeviceItemList = aMaintDeviceItemEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.AMaintDeviceItemList)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取a_maint_device无分页列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    private async Task<dynamic> GetNoPagingList(AMaintDeviceListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aMaintDeviceItemAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<AMaintDeviceListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_maint_device"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_maint_device_item"))) aMaintDeviceItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_maint_device_item"))?.conditionalModel;
        }


        var selectIds = input.selectIds?.Split(",").ToList();
        var F_DeviceType = input.F_DeviceType?.Last();

        var list = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_DeviceNo), it => it.F_DeviceNo.Contains(input.F_DeviceNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_DeviceName), it => it.F_DeviceName.Contains(input.F_DeviceName))
            .WhereIF(!string.IsNullOrEmpty(input.F_DeviceStatus), it => it.F_DeviceStatus.Equals(input.F_DeviceStatus))
            .WhereIF(!string.IsNullOrEmpty(input.F_DeviceType?.ToString()), it => it.F_DeviceType.Contains(F_DeviceType))
            .Where(authorizeWhere)
            .WhereIF(aMaintDeviceItemAuthorizeWhere?.Count > 0, it => it.AMaintDeviceItemList.Any(aMaintDeviceItemAuthorizeWhere))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new AMaintDeviceListOutput
            {
                id = it.id,
                F_DeviceNo = it.F_DeviceNo,
                F_DeviceName = it.F_DeviceName,
                F_DeviceSpec = it.F_DeviceSpec,
                F_DeviceStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_DeviceStatus) && dic.DictionaryTypeId.Equals("2011620395194650624")).Select(dic => dic.FullName),
                F_DeviceStatusCode = it.F_DeviceStatus,
                F_DeviceType = it.F_DeviceType,
                F_GroupId = SqlFunc.Subqueryable<GroupEntity>().EnableTableFilter().Where(c => c.Id.Equals(it.F_GroupId)).Select(c => c.FullName),
                F_DeviceUsersId = it.F_DeviceUsersId,
                F_WorkshopId = it.F_WorkshopId,
                F_LineId = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_LineId) && dic.DictionaryTypeId.Equals("2011623836151320576")).Select(dic => dic.FullName),
                F_DeviceQrCode = !string.IsNullOrEmpty(it.F_DeviceQrCode) ? it.F_DeviceQrCode : "未绑定",
                F_EnabledMark = SqlFunc.IIF(SqlFunc.ToInt32(it.F_EnabledMark) == 1, "启用", "禁用"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 设备类别
            var F_DeviceTypeData = await _dataInterfaceService.GetDynamicList("F_DeviceType", "2011621811959238656", "F_Id", "F_Name", "children");
            if (item.F_DeviceType != null)
            {
                var F_DeviceTypeCascader = item.F_DeviceType.ToObject<List<string>>();
                item.F_DeviceType = string.Join(",", F_DeviceTypeData.FindAll(it => F_DeviceTypeCascader.Contains(it.id)).Select(s => s.fullName));
            }

            // 设备负责人
            if (item.F_DeviceUsersId != null)
            {
                var F_DeviceUsersIdUserSelect = item.F_DeviceUsersId.ToObject<List<string>>();
                item.F_DeviceUsersId = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => F_DeviceUsersIdUserSelect.Contains(it.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 车间
            var F_WorkshopIdData = await _dataInterfaceService.GetDynamicList("F_WorkshopId", "2011621894347952128", "F_Id", "F_Name", "children");
            if (item.F_WorkshopId != null)
            {
                var F_WorkshopIdCascader = item.F_WorkshopId.ToObject<List<string>>();
                item.F_WorkshopId = string.Join(",", F_WorkshopIdData.FindAll(it => F_WorkshopIdCascader.Contains(it.id)).Select(s => s.fullName));
            }
        });
        return list;
    }

    /// <summary>
    /// 导出a_maint_device.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("Actions/Export")]
    public async Task<dynamic> Export([FromBody] AMaintDeviceListQueryInput input)
    {
        var exportData = new List<AMaintDeviceListOutput>();
        if (input.dataType == 0)
            exportData = Clay.Object(await GetList(input)).Solidify<PageResult<AMaintDeviceListOutput>>().list;
        else
            exportData = await GetNoPagingList(input);
        var excelName = string.Format("{0}_{1:yyyyMMddHHmmss}", _controlParsing.GetModuleNameById(input.menuId), DateTime.Now);
        _cacheManager.Set(excelName + ".xls", string.Empty);
        return _exportImportDataHelper.GetDataExport(excelName, input.selectKey, _userManager.UserId,exportData.ToJsonString().ToObjectOld<List<Dictionary<string, object>>>(), paramList, false);
    }

    /// <summary>
    /// 更新a_maint_device.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] AMaintDeviceUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AMaintDeviceEntity>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AMaintDeviceEntity>(new AMaintDeviceEntity()));
        ConfigModel tableField72c841Config = new ConfigModel
        {
            tableName = "a_maint_device_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "物料清单",
            children = ExportImportDataHelper.GetTemplateParsing<AMaintDeviceItemEntity>(new AMaintDeviceItemEntity())
        };
        FieldsModel tableField72c841Model = new FieldsModel()
        {
            __config__ = tableField72c841Config,
            __vModel__ = "tableField72c841"
        };
        fieldList.Add(tableField72c841Model);

        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_DeviceNo.Equals(input.F_DeviceNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "设备编号");

        if (string.IsNullOrEmpty(entity.F_DeviceNo))
        {
            entity.F_DeviceNo = oldEntity.F_DeviceNo;
        }

        // 移除设备物料清单可能被删除数据
        if (input.tableField72c841 !=null && input.tableField72c841.Any())
            await _repository.AsSugarClient().Deleteable<AMaintDeviceItemEntity>().Where(it => it.F_DeviceId == entity.id && !input.tableField72c841.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<AMaintDeviceItemEntity>().Where(it => it.F_DeviceId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增设备物料清单新数据
        var aMaintDeviceItemEntityList = input.tableField72c841.Adapt<List<AMaintDeviceItemEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_DeviceNo,
                    it.F_DeviceName,
                    it.F_DeviceSpec,
                    it.F_DeviceStatus,
                    it.F_DeviceType,
                    it.F_GroupId,
                    it.F_DeviceUsersId,
                    it.F_WorkshopId,
                    it.F_LineId,
                    it.F_DeviceQrCode,
                    it.F_DeviceImages,
                    it.F_EnabledMark,
                })
                .ExecuteCommandAsync();

            if(aMaintDeviceItemEntityList != null)
            {
                foreach (var item in aMaintDeviceItemEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_DeviceId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorTime = null;
                        item.F_DeviceId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
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
    /// 更新 设备二维码.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("UpdateQr/{id}")]
    [UnitOfWork]
    public async Task UpdateQr(string id, [FromBody] AMaintDeviceUpInput input)
    {
        var entity = await _repository.GetFirstAsync(it => it.id == id);
        entity.F_DeviceQrCode = input.F_DeviceQrCode;
        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_DeviceQrCode,
                })
                .ExecuteCommandAsync();
        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 解除二维码绑定.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("DeleteQr/{id}")]
    [UnitOfWork]
    public async Task UpdateQr(string id)
    {
        var entityQr =  await _repository.AsSugarClient().Queryable<AMaintDeviceQrEntity>().Where(it => it.DeleteMark == null && it.id == id).Select(it => it.F_Code).ToListAsync();
        var entity = await _repository.GetFirstAsync(it => it.F_DeviceQrCode == entityQr[0]);
        entity.F_DeviceQrCode = null;
        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_DeviceQrCode,
                })
                .ExecuteCommandAsync();
        }
        catch (Exception)
        {
            throw Oops.Oh(ErrorCode.COM1001);
        }
    }

    /// <summary>
    /// 删除a_maint_device.
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
    /// 批量删除a_maint_device.
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
            // 批量删除a_maint_device
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_maint_device详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.id == id)
            .Select(it => new AMaintDeviceDetailOutput
            {
                id = it.id,
                F_DeviceNo = it.F_DeviceNo,
                F_DeviceName = it.F_DeviceName,
                F_DeviceSpec = it.F_DeviceSpec,
                F_DeviceStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_DeviceStatus) && dic.DictionaryTypeId.Equals("2011620395194650624")).Select(dic => dic.FullName),
                F_DeviceType = it.F_DeviceType,
                F_GroupId = SqlFunc.Subqueryable<GroupEntity>().EnableTableFilter().Where(c => c.Id.Equals(it.F_GroupId)).Select(c => c.FullName),
                F_DeviceUsersId = it.F_DeviceUsersId,
                F_WorkshopId = it.F_WorkshopId,
                F_LineId = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_LineId) && dic.DictionaryTypeId.Equals("2011623836151320576")).Select(dic => dic.FullName),
                F_DeviceQrCode = !string.IsNullOrEmpty(it.F_DeviceQrCode) ? it.F_DeviceQrCode : "未绑定",
                F_DeviceImages = it.F_DeviceImages,
                F_EnabledMark = SqlFunc.IIF(SqlFunc.ToInt32(it.F_EnabledMark) == 1, "启用", "禁用"),
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 设备类别
            var F_DeviceTypeData = await _dataInterfaceService.GetDynamicList("F_DeviceType", "2011621811959238656", "F_Id", "F_Name", "children");
            if (item.F_DeviceType != null)
            {
                var F_DeviceTypeCascader = item.F_DeviceType.ToObject<List<string>>();
                item.F_DeviceType = string.Join(",", F_DeviceTypeData.FindAll(it => F_DeviceTypeCascader.Contains(it.id)).Select(s => s.fullName));
            }

            // 设备负责人
            if (item.F_DeviceUsersId != null)
            {
                var F_DeviceUsersIdUserSelect = item.F_DeviceUsersId.ToObject<List<string>>();
                item.F_DeviceUsersId = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => F_DeviceUsersIdUserSelect.Contains(it.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 车间
            var F_WorkshopIdData = await _dataInterfaceService.GetDynamicList("F_WorkshopId", "2011621894347952128", "F_Id", "F_Name", "children");
            if (item.F_WorkshopId != null)
            {
                var F_WorkshopIdCascader = item.F_WorkshopId.ToObject<List<string>>();
                item.F_WorkshopId = string.Join(",", F_WorkshopIdData.FindAll(it => F_WorkshopIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

            if (item.F_DeviceImages != null)
            {
                item.F_DeviceImages = item.F_DeviceImages.ToString().ToObject<List<FileControlsModel>>();
            }
            else
            {
                item.F_DeviceImages = new List<FileControlsModel>();
            }

        });

        return data.FirstOrDefault();
    }


    /// <summary>
    /// 弹窗获取设备列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("PopupList")]
    public async Task<dynamic> GetPopupList()
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_EnabledMark == "1")
            .Select(it => new AMaintDeviceListOutput
            {
                id = it.id,
                F_DeviceNo = it.F_DeviceNo,
                F_DeviceName = it.F_DeviceName,
                F_DeviceSpec = it.F_DeviceSpec,
                F_DeviceStatus = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_DeviceStatus) && dic.DictionaryTypeId.Equals("2011620395194650624")).Select(dic => dic.FullName),
                F_DeviceType = it.F_DeviceType,
                F_GroupId = SqlFunc.Subqueryable<GroupEntity>().EnableTableFilter().Where(c => c.Id.Equals(it.F_GroupId)).Select(c => c.FullName),
                F_DeviceUsersId = it.F_DeviceUsersId,
                F_WorkshopId = it.F_WorkshopId,
                F_LineId = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(it.F_LineId) && dic.DictionaryTypeId.Equals("2011623836151320576")).Select(dic => dic.FullName),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderBy(it => it.F_CreatorTime, OrderByType.Desc).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            // 设备类别
            var F_DeviceTypeData = await _dataInterfaceService.GetDynamicList("F_DeviceType", "2011621811959238656", "F_Id", "F_Name", "children");
            if (item.F_DeviceType != null)
            {
                var F_DeviceTypeCascader = item.F_DeviceType.ToObject<List<string>>();
                item.F_DeviceType = string.Join(",", F_DeviceTypeData.FindAll(it => F_DeviceTypeCascader.Contains(it.id)).Select(s => s.fullName));
            }

            // 设备负责人
            if (item.F_DeviceUsersId != null)
            {
                var F_DeviceUsersIdUserSelect = item.F_DeviceUsersId.ToObject<List<string>>();
                item.F_DeviceUsersId = string.Join(",", await _repository.AsSugarClient().Queryable<UserEntity>().Where(it => F_DeviceUsersIdUserSelect.Contains(it.Id)).Select(it => it.RealName).ToListAsync());
            }

            // 车间
            var F_WorkshopIdData = await _dataInterfaceService.GetDynamicList("F_WorkshopId", "2011621894347952128", "F_Id", "F_Name", "children");
            if (item.F_WorkshopId != null)
            {
                var F_WorkshopIdCascader = item.F_WorkshopId.ToObject<List<string>>();
                item.F_WorkshopId = string.Join(",", F_WorkshopIdData.FindAll(it => F_WorkshopIdCascader.Contains(it.id)).Select(s => s.fullName));
            }

        });


        return new {
            data = data
        };
    }

}
