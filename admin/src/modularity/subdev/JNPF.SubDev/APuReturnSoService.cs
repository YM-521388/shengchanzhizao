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
using JNPF.example.Entitys.Dto.APuReturnSo;
using JNPF.example.Entitys.Dto.APuReturnSoItem;
using JNPF.example.Entitys.Dto.APuReturnSoLog;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
using System.Text.Json;
 
namespace JNPF.example;

/// <summary>
/// 业务实现：a_pu_return_so.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "APuReturnSo", Order = 200)]
[Route("api/example/[controller]")]
public class APuReturnSoService : IAPuReturnSoService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<APuReturnSoEntity> _repository;
    private readonly ISqlSugarRepository<AProdProcessingEntity> _repositorying;
    private readonly ISqlSugarRepository<ACtcContractItemEntity> _repositoryctcitem;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<ACtcContractEntity> _repositoryctccontract;

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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"销售退货单号\",\"field\":\"F_ReturnInNo\"},{\"value\":\"入库类型\",\"field\":\"F_ReturnInType\"},{\"value\":\"合同单号\",\"field\":\"F_ContractId\"},{\"value\":\"退货日期\",\"field\":\"F_ReturnInDate\"},{\"value\":\"客户\",\"field\":\"F_CustomerId\"},{\"value\":\"联系人\",\"field\":\"F_ContactId\"},{\"value\":\"地址\",\"field\":\"F_AddressId\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建人员\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"},{\"value\":\"商品-商品ID\",\"field\":\"tableFieldcfb049-F_CustomerId\"},{\"value\":\"商品-退货数量\",\"field\":\"tableFieldcfb049-F_Num\"},{\"value\":\"商品-销售单价(元)\",\"field\":\"tableFieldcfb049-F_Price\"},{\"value\":\"商品-备注\",\"field\":\"tableFieldcfb049-F_Description\"},{\"value\":\"商品-创建人员\",\"field\":\"tableFieldcfb049-F_CreatorUserId\"},{\"value\":\"商品-创建时间\",\"field\":\"tableFieldcfb049-F_CreatorTime\"},{\"value\":\"操作日志-类型\",\"field\":\"tableField0c9c23-F_Type\"},{\"value\":\"操作日志-标题\",\"field\":\"tableField0c9c23-F_Title\"},{\"value\":\"操作日志-内容\",\"field\":\"tableField0c9c23-F_Content\"},{\"value\":\"操作日志-修改原因\",\"field\":\"tableField0c9c23-F_Reason\"},{\"value\":\"操作日志-创建人员\",\"field\":\"tableField0c9c23-F_CreatorUserId\"},{\"value\":\"操作日志-创建时间\",\"field\":\"tableField0c9c23-F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="APuReturnSoService"/>类型的新实例.
    /// </summary>
    public APuReturnSoService(
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<AProdProcessingEntity> repositorying,
        ISqlSugarRepository<ACtcContractItemEntity> repositoryctcitem,
        ISqlSugarRepository<APuReturnSoEntity> repository,
        ISqlSugarRepository<ACtcContractEntity> repositoryctccontract,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryInventory = repositoryInventory;
        _repositorying = repositorying;
         _repositoryctcitem = repositoryctcitem;
        _repository = repository;
        _repositoryctccontract = repositoryctccontract;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_pu_return_so.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.APuReturnSoItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuReturnSoLogList)
            .Select(it => new APuReturnSoEntity
            {
                id = it.id,
                F_ReturnInNo = it.F_ReturnInNo,
                //F_WarehouseId = it.F_WarehouseId,
                F_ReturnInType = it.F_ReturnInType,
                F_ReturnInDate = it.F_ReturnInDate,
                F_ContractId = it.F_ContractId,
                F_CustomerId = it.F_CustomerId,
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                APuReturnSoItemList = it.APuReturnSoItemList.Where(item => item.DeleteMark == null).ToList(),
                APuReturnSoLogList = it.APuReturnSoLogList.Where(log => log.DeleteMark == null).ToList(),
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<APuReturnSoInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableFieldcfb049, async aPuReturnSoItem =>
        {
            // 创建人员
            aPuReturnSoItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuReturnSoItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuReturnSoItem.F_CreatorTime = aPuReturnSoItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField0c9c23, async aPuReturnSoLog =>
        {
            // 创建人员
            aPuReturnSoLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuReturnSoLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuReturnSoLog.F_CreatorTime = aPuReturnSoLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        return data;
    }

    /// <summary>
    /// 获取a_pu_return_so列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] APuReturnSoListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aPuReturnSoItemAuthorizeWhere = new List<IConditionalModel>();
        var aPuReturnSoLogAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<APuReturnSoListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_pu_return_so"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_return_so_item"))) aPuReturnSoItemAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_return_so_item"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_pu_return_so_log"))) aPuReturnSoLogAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_pu_return_so_log"))?.conditionalModel;
        }

        var entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuReturnSoEntity>();
        var superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, string.Empty, entityInfo, 0);
        superQuery = await _controlParsing.ReplaceDynamicQuery<ConvertSuper>(superQuery);
        List<IConditionalModel> mainConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuReturnSoItemEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableFieldcfb049-", entityInfo, 1);
        List<IConditionalModel> aPuReturnSoItemConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuReturnSoItemConditionalModel.AddRange(aPuReturnSoItemAuthorizeWhere);

        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuReturnSoLogEntity>();
        superQuery = SuperQueryHelper.GetSuperQueryInput(input.superQueryJson, "tableField0c9c23-", entityInfo, 1);
        List<IConditionalModel> aPuReturnSoLogConditionalModel = SuperQueryHelper.GetSuperQueryJson(superQuery);
        aPuReturnSoLogConditionalModel.AddRange(aPuReturnSoLogAuthorizeWhere);

        var selectIds = input.selectIds?.Split(",").ToList();
        entityInfo = _repository.AsSugarClient().EntityMaintenance.GetEntityInfo<APuReturnSoEntity>();
        var F_ReturnInTypeDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_ReturnInType").DbColumnName;
        var F_ContractIdDbColumnName = entityInfo.Columns.Find(it => it.PropertyName == "F_ContractId").DbColumnName;
        var data = await _repository.AsQueryable()
            .Includes(x => x.APuReturnSoItemList
            .Where(aPuReturnSoItemConditionalModel)
            .Where(it => it.DeleteMark  == null)
            .Select(it => new APuReturnSoItemEntity()
            {
                F_CustomerId = it.F_CustomerId,
                F_Num = it.F_Num,
                F_Price = it.F_Price,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .ToList())
            .Includes(x => x.APuReturnSoLogList
            .Where(aPuReturnSoLogConditionalModel)
            .Where(it => it.DeleteMark  == null)
            .Select(it => new APuReturnSoLogEntity()
            {
                F_Type = it.F_Type,
                F_Title = it.F_Title,
                F_Content = it.F_Content,
                F_Reason = it.F_Reason,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .ToList())
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ReturnInNo), it => it.F_ReturnInNo.Contains(input.F_ReturnInNo))
           
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_ReturnInTypeDbColumnName, input.F_ReturnInType))
            .WhereIF(input.F_ReturnInDate?.Count() > 0, it => SqlFunc.Between(it.F_ReturnInDate, input.F_ReturnInDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_ReturnInDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(_controlParsing.GenerateMultipleSelectionCriteriaForQuerying(F_ContractIdDbColumnName, input.F_ContractId))
            .Where(authorizeWhere)
            .WhereIF(aPuReturnSoItemAuthorizeWhere?.Count > 0, it => it.APuReturnSoItemList.Any(aPuReturnSoItemAuthorizeWhere))
            .WhereIF(aPuReturnSoLogAuthorizeWhere?.Count > 0, it => it.APuReturnSoLogList.Any(aPuReturnSoLogAuthorizeWhere))
            .Where(mainConditionalModel)
            .WhereIF(aPuReturnSoItemConditionalModel.Count > 0, it => it.APuReturnSoItemList.Any(aPuReturnSoItemConditionalModel))
            .WhereIF(aPuReturnSoLogConditionalModel.Count > 0, it => it.APuReturnSoLogList.Any(aPuReturnSoLogConditionalModel))
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.id).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new APuReturnSoListOutput
            {
                id = it.id,
                F_ReturnInNo = it.F_ReturnInNo,
              
                F_ReturnInType = it.F_ReturnInType,
                F_ReturnInDate = it.F_ReturnInDate.Value.ToString("yyyy-MM-dd"),
                F_ContractId = it.F_ContractId,
                F_CustomerId = it.F_CustomerId,
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
                F_Description = it.F_Description,
                F_CreatorUserId = it.F_CreatorUserId,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableFieldcfb049 = it.APuReturnSoItemList.Adapt<List<APuReturnSoItemListOutput>>(),
                tableField0c9c23 = it.APuReturnSoLogList.Adapt<List<APuReturnSoLogListOutput>>(),
            })
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
        


            // 入库类型
            if (item.F_ReturnInType != null)
            {
                item.F_ReturnInType = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.Id.Equals(item.F_ReturnInType) && it.DictionaryTypeId.Equals("2012074650393251840")).Select(it => it.FullName).FirstAsync();
            }
            // 合同单号ID
            var F_ContractIdData = await _dataInterfaceService.GetDynamicList("F_ContractId", "2010889611072638976", "F_Id", "F_ContractCode", "");
            if (item.F_ContractId != null)
            {
                item.F_ContractId = F_ContractIdData.Find(it => it.id.Equals(item.F_ContractId))?.fullName;
            }
            // 创建人员
            var F_CreatorUserIdData = await _repository.AsSugarClient().Queryable<UserEntity>().FirstAsync(u => u.Id.Equals(item.F_CreatorUserId));
            item.F_CreatorUserId = F_CreatorUserIdData != null ? string.Format("{0}/{1}", F_CreatorUserIdData.RealName, F_CreatorUserIdData.Account) : null;
            // 联系人名称（将存储的 F_ContactId 转为联系人名称 F_ContactName）
            if (!string.IsNullOrEmpty(item.F_ContactId))
            {
                var contactName = await _repository.AsSugarClient()
                    .Queryable<ACtcContactEntity>()
                    .Where(c => c.F_Id.Equals(item.F_ContactId))
                    .Select(c => c.F_ContactName)
                    .FirstAsync();
                item.F_ContactId = contactName;
            }

            // 地址文本（将存储的 F_AddressId 转为地址字段 F_Address）
            if (!string.IsNullOrEmpty(item.F_AddressId))
            {
                var addressText = await _repository.AsSugarClient()
                    .Queryable<ACtcAddressEntity>()
                    .Where(a => a.F_Id.Equals(item.F_AddressId))
                    .Select(a => a.F_Address)
                    .FirstAsync();
                item.F_AddressId = addressText;
            }
        });

        await _repository.AsSugarClient().ThenMapperAsync(data.list.SelectMany(it => it.tableFieldcfb049), async aPuReturnSoItem =>
        {
            var aPuReturnSo = data.list.ToList().Find(it => it.id.Equals(aPuReturnSoItem.F_ReturnInSOId));
            var linkageParameters = new List<DataInterfaceParameter>();

            // 创建时间
            aPuReturnSoItem.F_CreatorTime = aPuReturnSoItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });
        await _repository.AsSugarClient().ThenMapperAsync(data.list.SelectMany(it => it.tableField0c9c23), async aPuReturnSoLog =>
        {
            var aPuReturnSo = data.list.ToList().Find(it => it.id.Equals(aPuReturnSoLog.F_ReturnInSOId));
            var linkageParameters = new List<DataInterfaceParameter>();

            // 创建时间
            aPuReturnSoLog.F_CreatorTime = aPuReturnSoLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<APuReturnSoEntity>(new APuReturnSoEntity()));
        ConfigModel tableFieldcfb049Config = new ConfigModel
        {
            tableName = "a_pu_return_so_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<APuReturnSoItemEntity>(new APuReturnSoItemEntity())
        };
        FieldsModel tableFieldcfb049 = new FieldsModel()
        {
            __config__ = tableFieldcfb049Config,
            __vModel__ = "tableFieldcfb049"
        };
        fieldList.Add(tableFieldcfb049);

        resData = await _controlParsing.GetParsDataList(resData, "F_CustomerId,F_ContactId,F_AddressId,tableFieldcfb049-F_CustomerId", "popupSelect", _userManager.TenantId, fieldList);

        data.list = resData.ToObject<List<APuReturnSoListOutput>>(CommonConst.options);
        return PageResult<APuReturnSoListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_pu_return_so.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] APuReturnSoCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuReturnSoEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuReturnSoEntity>(new APuReturnSoEntity()));
        ConfigModel tableFieldcfb049Config = new ConfigModel
        {
            tableName = "a_pu_return_so_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuReturnSoItemEntity>(new APuReturnSoItemEntity())
        };
        FieldsModel tableFieldcfb049Model = new FieldsModel()
        {
            __config__ = tableFieldcfb049Config,
            __vModel__ = "tableFieldcfb049"
        };
        fieldList.Add(tableFieldcfb049Model);
        ConfigModel tableField0c9c23Config = new ConfigModel
        {
            tableName = "a_pu_return_so_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "操作日志",
            children = ExportImportDataHelper.GetTemplateParsing<APuReturnSoLogEntity>(new APuReturnSoLogEntity())
        };
        FieldsModel tableField0c9c23Model = new FieldsModel()
        {
            __config__ = tableField0c9c23Config,
            __vModel__ = "tableField0c9c23"
        };
        fieldList.Add(tableField0c9c23Model);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        // 如果前端未传入销售退货单号，则自动生成：XSTH + yyyyMMdd + 当天序号（3位）
        if (string.IsNullOrWhiteSpace(entity.F_ReturnInNo))
        {
            var today = DateTime.Now.ToString("yyyyMMdd");
            var prefix = $"XSTH{today}";
            // 统计当天已存在的单号数量（以 prefix 开头），作为序号基数
            var todayCount = await _repository.AsSugarClient().Queryable<APuReturnSoEntity>()
                .Where(it => it.DeleteMark == null && it.F_ReturnInNo != null && it.F_ReturnInNo.StartsWith(prefix))
                .CountAsync();
            var seq = todayCount + 1;
            entity.F_ReturnInNo = $"{prefix}{seq.ToString().PadLeft(3, '0')}";
        }

        var aPuReturnSoItemEntityList = input.tableFieldcfb049.Adapt<List<APuReturnSoItemEntity>>();
        if(aPuReturnSoItemEntityList != null)
        {
            foreach (var item in aPuReturnSoItemEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuReturnSoItemList = aPuReturnSoItemEntityList;
        }

        var aPuReturnSoLogEntityList = input.tableField0c9c23.Adapt<List<APuReturnSoLogEntity>>();
        if(aPuReturnSoLogEntityList != null)
        {
            foreach (var item in aPuReturnSoLogEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorUserId = _userManager.UserId;
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.APuReturnSoLogList = aPuReturnSoLogEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.APuReturnSoItemList)
            .Include(it => it.APuReturnSoLogList)
            .ExecuteCommandAsync();

        if (aPuReturnSoItemEntityList != null && aPuReturnSoItemEntityList.Any())
        {
            // 逐行处理库存，使用子表的 F_WarehouseID 字段（完整数组形式）
            foreach (var item in aPuReturnSoItemEntityList)
            {
                if (string.IsNullOrEmpty(item.F_CustomerId) || item.F_Num == null || item.F_Num <= 0)
                    continue;

                // 直接使用子表行的仓库ID（完整数组），如果为空则使用主表仓库作为兜底
                var warehouseIds = !string.IsNullOrEmpty(item.F_WarehouseID)
                    ? item.F_WarehouseID
                    : entity.F_WarehouseId;

                if (string.IsNullOrEmpty(warehouseIds))
                    continue;

                // 按库存编码 F_Encoding 查询库存记录，每个编码对应一条独立库存
                var inventory = await _repositoryInventory.AsSugarClient()
                    .Queryable<AGoodsInventoryEntity>()
                    .Where(it => it.DeleteMark == null && it.F_Code == item.F_Encoding)
                    .FirstAsync();

                if (inventory != null)
                {
                    // 累加库存，并填充商品编码
                    inventory.F_Num = (inventory.F_Num ?? 0) + item.F_Num;
                    inventory.F_Code = item.F_Encoding;
                    inventory.F_WarehouseId = warehouseIds; // 存储完整的仓库数组
                    await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                }
                else
                {
                    // 新增库存记录
                    var inventoryEntity = new AGoodsInventoryEntity
                    {
                        id = SnowflakeIdHelper.NextId(),
                        F_CreatorUserId = _userManager.UserId,
                        F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime(),
                        F_Num = item.F_Num,
                        F_GoodsId = item.F_CustomerId,
                        F_WarehouseId = warehouseIds, // 存储完整的仓库数组
                        F_Code = item.F_Encoding
                    };
                    await _repositoryInventory.AsSugarClient().Insertable(inventoryEntity).ExecuteCommandAsync();
                }
            }
        }
    }

    /// <summary>
    /// 更新a_pu_return_so.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] APuReturnSoUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<APuReturnSoEntity>();
        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_ReturnInNo.Equals(input.F_ReturnInNo) && it.FlowId == null && it.DeleteMark == null && it.id != id))
            throw Oops.Bah(ErrorCode.COM1023, "销售退货单号");

        if (string.IsNullOrEmpty(entity.F_ReturnInNo))
        {
            entity.F_ReturnInNo = oldEntity.F_ReturnInNo;
        }
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<APuReturnSoEntity>(new APuReturnSoEntity()));
        ConfigModel tableFieldcfb049Config = new ConfigModel
        {
            tableName = "a_pu_return_so_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetTemplateParsing<APuReturnSoItemEntity>(new APuReturnSoItemEntity())
        };
        FieldsModel tableFieldcfb049Model = new FieldsModel()
        {
            __config__ = tableFieldcfb049Config,
            __vModel__ = "tableFieldcfb049"
        };
        fieldList.Add(tableFieldcfb049Model);
        ConfigModel tableField0c9c23Config = new ConfigModel
        {
            tableName = "a_pu_return_so_log",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "操作日志",
            children = ExportImportDataHelper.GetTemplateParsing<APuReturnSoLogEntity>(new APuReturnSoLogEntity())
        };
        FieldsModel tableField0c9c23Model = new FieldsModel()
        {
            __config__ = tableField0c9c23Config,
            __vModel__ = "tableField0c9c23"
        };
        fieldList.Add(tableField0c9c23Model);

        // 移除销售退货单商品列表可能被删除数据
        if (input.tableFieldcfb049 !=null && input.tableFieldcfb049.Any())
            await _repository.AsSugarClient().Deleteable<APuReturnSoItemEntity>().Where(it => it.F_ReturnInSOId == entity.id && !input.tableFieldcfb049.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<APuReturnSoItemEntity>().Where(it => it.F_ReturnInSOId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增销售退货单商品列表新数据
        var aPuReturnSoItemEntityList = input.tableFieldcfb049.Adapt<List<APuReturnSoItemEntity>>();
        // 读取现有子表数据（用于对比删除/修改）
        var oldItemList = await _repository.AsSugarClient().Queryable<APuReturnSoItemEntity>()
            .Where(it => it.F_ReturnInSOId == entity.id)
            .Select(it => new { F_Id = it.F_Id, F_CustomerId = it.F_CustomerId, F_Num = it.F_Num, F_WarehouseID = it.F_WarehouseID })
            .ToListAsync();


        // 移除销售退货单日志可能被删除数据
        if (input.tableField0c9c23 !=null && input.tableField0c9c23.Any())
            await _repository.AsSugarClient().Deleteable<APuReturnSoLogEntity>().Where(it => it.F_ReturnInSOId == entity.id && !input.tableField0c9c23.Select(it => it.F_Id).ToList().Contains(it.F_Id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        else
            await _repository.AsSugarClient().Deleteable<APuReturnSoLogEntity>().Where(it => it.F_ReturnInSOId == entity.id).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

        // 新增销售退货单日志新数据
        var aPuReturnSoLogEntityList = input.tableField0c9c23.Adapt<List<APuReturnSoLogEntity>>();

        try
        {
            // 生成自动操作日志集合（用于记录主表及子表的新增/修改/删除）
            var autoLogList = new List<APuReturnSoLogEntity>();
            // 取旧主表数据用于对比
            Func<object, string> fmt = v => v == null ? string.Empty : v.ToString();
            if (oldEntity != null)
            {
                var mainChanges = new List<string>();
                if (fmt(oldEntity.F_ReturnInNo) != fmt(entity.F_ReturnInNo)) mainChanges.Add($"退货单号: '{fmt(oldEntity.F_ReturnInNo)}' => '{fmt(entity.F_ReturnInNo)}'");
                //if (fmt(oldEntity.F_WarehouseId) != fmt(entity.F_WarehouseId)) mainChanges.Add($"仓库: '{fmt(oldEntity.F_WarehouseId)}' => '{fmt(entity.F_WarehouseId)}'");
                //if (fmt(oldEntity.F_ReturnInType) != fmt(entity.F_ReturnInType)) mainChanges.Add($"入库类型: '{fmt(oldEntity.F_ReturnInType)}' => '{fmt(entity.F_ReturnInType)}'");
                if (fmt(oldEntity.F_ReturnInDate) != fmt(entity.F_ReturnInDate)) mainChanges.Add($"退货日期: '{fmt(oldEntity.F_ReturnInDate)}' => '{fmt(entity.F_ReturnInDate)}'");
                //if (fmt(oldEntity.F_ContractId) != fmt(entity.F_ContractId)) mainChanges.Add($"合同单号: '{fmt(oldEntity.F_ContractId)}' => '{fmt(entity.F_ContractId)}'");
                //if (fmt(oldEntity.F_CustomerId) != fmt(entity.F_CustomerId)) mainChanges.Add($"客户: '{fmt(oldEntity.F_CustomerId)}' => '{fmt(entity.F_CustomerId)}'");
                //if (fmt(oldEntity.F_ContactId) != fmt(entity.F_ContactId)) mainChanges.Add($"联系人: '{fmt(oldEntity.F_ContactId)}' => '{fmt(entity.F_ContactId)}'");
                //if (fmt(oldEntity.F_AddressId) != fmt(entity.F_AddressId)) mainChanges.Add($"地址: '{fmt(oldEntity.F_AddressId)}' => '{fmt(entity.F_AddressId)}'");
                if (fmt(oldEntity.F_Description) != fmt(entity.F_Description)) mainChanges.Add($"备注: '{fmt(oldEntity.F_Description)}' => '{fmt(entity.F_Description)}'");
                if (mainChanges.Any())
                {
                    autoLogList.Add(new APuReturnSoLogEntity
                    {
                        F_Id = SnowflakeIdHelper.NextId(),
                        F_ReturnInSOId = entity.id,
                        F_Title = "修改数据",
                        F_Content = string.Join("；", mainChanges),
                        F_CreatorUserId = _userManager.UserId,
                        F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                    });
                }
            }
            // 计算子表被删除的行（用于记录删除日志）
            var existIds = aPuReturnSoItemEntityList?.Select(x => x.F_Id).ToHashSet() ?? new HashSet<string>();
            var missingList = oldItemList.Where(it => !existIds.Contains(it.F_Id)).ToList();
            foreach (var missing in missingList)
            {
                autoLogList.Add(new APuReturnSoLogEntity
                {
                    F_Id = SnowflakeIdHelper.NextId(),
                    F_ReturnInSOId = entity.id,
                    F_Title = "删除数据",
                    F_Content = $"商品 {missing.F_CustomerId} 数量 {missing.F_Num}",
                    F_CreatorUserId = _userManager.UserId,
                    F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                });
            }
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_ReturnInNo,
                    it.F_WarehouseId,
                    it.F_ReturnInType,
                    it.F_ReturnInDate,
                    it.F_ContractId,
                    it.F_CustomerId,
                    it.F_ContactId,
                    it.F_AddressId,
                    it.F_Description,
                })
                .ExecuteCommandAsync();

            if(aPuReturnSoItemEntityList != null)
            {
                foreach (var item in aPuReturnSoItemEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_ReturnInSOId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                        // 记录新增子表日志
                        autoLogList.Add(new APuReturnSoLogEntity
                        {
                            F_Id = SnowflakeIdHelper.NextId(),
                            F_ReturnInSOId = entity.id,
                            F_Title = "新增数据",
                            F_Content = $"商品 {item.F_CustomerId} 数量 {item.F_Num} 单价 {item.F_Price}",
                            F_CreatorUserId = _userManager.UserId,
                            F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                        });
                    }else{
                        // 读取旧子表完整数据用于对比
                        var oldItem = await _repository.AsSugarClient().Queryable<APuReturnSoItemEntity>().FirstAsync(it => it.F_Id == item.F_Id);
                        var childChanges = new List<string>();
                        if (oldItem != null)
                        {
                            if (fmt(oldItem.F_CustomerId) != fmt(item.F_CustomerId)) childChanges.Add($"商品: '{fmt(oldItem.F_CustomerId)}' => '{fmt(item.F_CustomerId)}'");
                            if (fmt(oldItem.F_Num) != fmt(item.F_Num)) childChanges.Add($"数量: '{fmt(oldItem.F_Num)}' => '{fmt(item.F_Num)}'");
                            if (fmt(oldItem.F_Price) != fmt(item.F_Price)) childChanges.Add($"单价: '{fmt(oldItem.F_Price)}' => '{fmt(item.F_Price)}'");
                        }

                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_ReturnInSOId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

                        if (childChanges.Any())
                        {
                            autoLogList.Add(new APuReturnSoLogEntity
                            {
                                F_Id = SnowflakeIdHelper.NextId(),
                                F_ReturnInSOId = entity.id,
                                F_Title = "修改数据",
                                F_Content = string.Join("；", childChanges),
                                F_CreatorUserId = _userManager.UserId,
                                F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime()
                            });
                        }
                    }
                }
            }

            if(aPuReturnSoLogEntityList != null)
            {
                foreach (var item in aPuReturnSoLogEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorUserId = _userManager.UserId;
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_ReturnInSOId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorUserId = null;
                        item.F_CreatorTime = null;
                        item.F_ReturnInSOId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }
                }
            }

            // 将自动生成的日志写入数据库
            if (autoLogList.Any())
            {
                await _repository.AsSugarClient().Insertable(autoLogList).ExecuteCommandAsync();
            }

            // ==================== 库存调整 ====================
            // 1. 新增行：按每行的仓库增加库存
            var newItems = aPuReturnSoItemEntityList?.Where(it => !string.IsNullOrEmpty(it.F_Id) && oldItemList.All(old => old.F_Id != it.F_Id)).ToList() ?? new List<APuReturnSoItemEntity>();
            if (newItems.Any())
            {
                var addByKey = newItems
                    .Where(it => !string.IsNullOrEmpty(it.F_CustomerId) && !string.IsNullOrEmpty(it.F_WarehouseID))
                    .GroupBy(it => (F_CustomerId: it.F_CustomerId, WarehouseId: GetFirstWarehouseId(it.F_WarehouseID)))
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.F_Num ?? 0));
                await AdjustInventoryAdd(addByKey);
            }

            // 2. 删除行：按每行原仓库减少库存
            var deletedItems = oldItemList.Where(it => !existIds.Contains(it.F_Id)).ToList();
            if (deletedItems.Any())
            {
                var reduceByKey = deletedItems
                    .Where(it => !string.IsNullOrEmpty(it.F_CustomerId) && !string.IsNullOrEmpty(it.F_WarehouseID))
                    .GroupBy(it => (F_CustomerId: it.F_CustomerId, WarehouseId: GetFirstWarehouseId(it.F_WarehouseID)))
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.F_Num ?? 0));
                await AdjustInventoryReduce(reduceByKey);
            }

            // 3. 修改行：按 (商品+仓库) 差异调整库存
            var updatedItems = aPuReturnSoItemEntityList?.Where(it => !string.IsNullOrEmpty(it.F_Id) && oldItemList.Any(old => old.F_Id == it.F_Id)).ToList() ?? new List<APuReturnSoItemEntity>();
            foreach (var item in updatedItems)
            {
                var oldItem = oldItemList.FirstOrDefault(it => it.F_Id == item.F_Id);
                if (oldItem == null) continue;
                // 旧组合的减少量
                var oldQty = oldItem.F_Num ?? 0;
                var newQty = item.F_Num ?? 0;
                var oldWarehouse = !string.IsNullOrEmpty(oldItem.F_WarehouseID) ? GetFirstWarehouseId(oldItem.F_WarehouseID) : null;
                var newWarehouse = !string.IsNullOrEmpty(item.F_WarehouseID) ? GetFirstWarehouseId(item.F_WarehouseID) : null;

                if (string.IsNullOrEmpty(oldWarehouse) && string.IsNullOrEmpty(newWarehouse)) continue;

                // 新旧仓库相同：差异调整数量
                if (oldWarehouse == newWarehouse && !string.IsNullOrEmpty(oldWarehouse))
                {
                    var diff = newQty - oldQty;
                    if (diff != 0)
                    {
                        await AdjustInventorySingle(item.F_CustomerId, oldWarehouse, diff);
                    }
                }
                else
                {
                    // 旧仓库减少旧数量
                    if (!string.IsNullOrEmpty(oldWarehouse) && oldQty != 0)
                    {
                        await AdjustInventorySingle(item.F_CustomerId, oldWarehouse, -oldQty);
                    }
                    // 新仓库增加新数量
                    if (!string.IsNullOrEmpty(newWarehouse) && newQty != 0)
                    {
                        await AdjustInventorySingle(item.F_CustomerId, newWarehouse, newQty);
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
    /// 删除a_pu_return_so.
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
    /// 批量删除a_pu_return_so.
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
            // 对每个主表记录，先根据子表每行各自的仓库，减少对应库存数量
            foreach (var entity in entitys)
            {
                var items = await _repository.AsSugarClient().Queryable<APuReturnSoItemEntity>()
                    .Where(it => it.F_ReturnInSOId == entity.id && it.DeleteMark == null)
                    .Select(it => new APuReturnSoItemEntity
                    {
                        F_CustomerId = it.F_CustomerId,
                        F_Num = it.F_Num,
                        F_WarehouseID = it.F_WarehouseID
                    })
                    .ToListAsync();

                if (items != null && items.Count > 0)
                {
                    // 按 (商品ID, 仓库ID) 分组，合并同商品同仓库的减少数量
                    var reduceByKey = items
                        .Where(it => !string.IsNullOrEmpty(it.F_CustomerId) && !string.IsNullOrEmpty(it.F_WarehouseID))
                        .GroupBy(it => (F_CustomerId: it.F_CustomerId, WarehouseId: GetFirstWarehouseId(it.F_WarehouseID)))
                        .ToDictionary(g => g.Key, g => g.Sum(x => x.F_Num ?? 0));

                    foreach (var kv in reduceByKey)
                    {
                        var goodsId = kv.Key.F_CustomerId;
                        var warehouseId = kv.Key.WarehouseId;
                        var reduceQty = kv.Value;
                        if (string.IsNullOrEmpty(goodsId) || string.IsNullOrEmpty(warehouseId)) continue;
                        var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == goodsId && it.F_WarehouseId == warehouseId);
                        if (inventory != null)
                        {
                            inventory.F_Num = (inventory.F_Num ?? 0) - reduceQty;
                            if (inventory.F_Num < 0) inventory.F_Num = 0;
                            await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                        }
                    }
                }
            }

            // 软删除子表商品与日志
            await _repository.AsSugarClient().Deleteable<APuReturnSoItemEntity>()
                .In(it => it.F_ReturnInSOId, input.ids)
                .Where(it => it.DeleteMark == null)
                .IsLogic()
                .ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

            await _repository.AsSugarClient().Deleteable<APuReturnSoLogEntity>()
                .In(it => it.F_ReturnInSOId, input.ids)
                .Where(it => it.DeleteMark == null)
                .IsLogic()
                .ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);

            // 批量软删除主表 a_pu_return_so
            await _repository.AsDeleteable().In(it => it.id, input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark", 1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_pu_return_so详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.APuReturnSoItemList)
            .Where(it => it.DeleteMark == null)
            .Includes(it => it.APuReturnSoLogList)
            .Where(it => it.id == id)
            .Select(it => new APuReturnSoDetailOutput
            {
                id = it.id,
                F_ReturnInNo = it.F_ReturnInNo,
                //F_WarehouseId = it.F_WarehouseId,
                F_ReturnInType = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.Id.Equals(it.F_ReturnInType) && dic.DictionaryTypeId.Equals("2012074650393251840")).Select(dic => dic.FullName),
                F_ReturnInDate = it.F_ReturnInDate.Value.ToString("yyyy-MM-dd"),
                F_ContractId = it.F_ContractId,
                F_CustomerId = it.F_CustomerId,
                F_ContactId = it.F_ContactId,
                F_AddressId = it.F_AddressId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableFieldcfb049 = it.APuReturnSoItemList.Where(item => item.DeleteMark == null).Adapt<List<APuReturnSoItemDetailOutput>>(),
                tableField0c9c23 = it.APuReturnSoLogList.Where(log => log.DeleteMark == null).Adapt<List<APuReturnSoLogDetailOutput>>(),
            }).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
          
            if (!string.IsNullOrEmpty(item.F_ContactId))
            {
                var contactName = await _repository.AsSugarClient()
                    .Queryable<ACtcContactEntity>()
                    .Where(c => c.F_Id.Equals(item.F_ContactId))
                    .Select(c => c.F_ContactName)
                    .FirstAsync();
                item.F_ContactId = contactName;
            }

            if (!string.IsNullOrEmpty(item.F_AddressId))
            {
                var addressText = await _repository.AsSugarClient()
                    .Queryable<ACtcAddressEntity>()
                    .Where(a => a.F_Id.Equals(item.F_AddressId))
                    .Select(a => a.F_Address)
                    .FirstAsync();
                item.F_AddressId = addressText;
            }
            // 合同单号ID
            if (item.F_ContractId != null)
            {
                var F_ContractIdData = await _dataInterfaceService.GetDynamicList("F_ContractId", "2010889611072638976", "F_Id", "F_ContractCode", "");
                item.F_ContractId = F_ContractIdData.Find(it => it.id.Equals(item.F_ContractId))?.fullName;
            }

        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableFieldcfb049), async aPuReturnSoItem =>
        {
            var aPuReturnSo = data.ToList().Find(it => it.id.Equals(aPuReturnSoItem.F_ReturnInSOId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuReturnSoItem.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuReturnSoItem.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuReturnSoItem.F_CreatorTime = aPuReturnSoItem.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField0c9c23), async aPuReturnSoLog =>
        {
            var aPuReturnSo = data.ToList().Find(it => it.id.Equals(aPuReturnSoLog.F_ReturnInSOId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建人员
            aPuReturnSoLog.F_CreatorUserId = _repository.AsSugarClient().Queryable<UserEntity>().Where(p => p.Id.Equals(aPuReturnSoLog.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)).First();
            // 创建时间
            aPuReturnSoLog.F_CreatorTime = aPuReturnSoLog.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<APuReturnSoEntity>(new APuReturnSoEntity()));
        ConfigModel tableFieldcfb049Config = new ConfigModel
        {
            tableName = "a_pu_return_so_item",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "商品",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<APuReturnSoItemEntity>(new APuReturnSoItemEntity())
        };
        FieldsModel tableFieldcfb049 = new FieldsModel()
        {
            __config__ = tableFieldcfb049Config,
            __vModel__ = "tableFieldcfb049"
        };
        fieldList.Add(tableFieldcfb049);

        resData = await _controlParsing.GetParsDataList(resData,"F_CustomerId,F_ContactId,F_AddressId,tableFieldcfb049-F_CustomerId","popupSelect",_userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }

    /// <summary>
    /// 根据前端传入的合同 Id 直接查询 ACtcContractItem 表并返回 F_CustomerId
    /// </summary>
    /// <param name="id">合同 Id（前端传入）</param>
    [HttpGet("GetContractItemsByContractId/{id}")]
    public async Task<dynamic> GetContractItemsByContractId(string id)
    {
        // 直接在 ACtcContractItem 表中查询与该合同匹配的项目，并只返回需要的字段
        var contractItems = await _repositoryctcitem.AsQueryable()
            .Where(it => it.DeleteMark == null && it.F_ContractId == id)
            .Select(it => new
            {
                it.F_CustomerId,
                it.F_Price,
                it.F_SaleQty,
                it.F_Description,
            })
            .ToListAsync();

        return contractItems;
    }

    /// <summary>
    /// 根据前端传入的合同 F_Id 获取该合同对应的客商、联系人、地址字段值
    /// </summary>
    /// <param name="id">合同 F_Id（前端传入）</param>
    [HttpGet("GetContractRelationById/{id}")]
    public async Task<dynamic> GetContractRelationById(string id)
    {
        // 在 ACtcContract 表中查找对应的合同（DeleteMark == null 且 F_Id 匹配）
        var contract = await _repositoryctccontract.GetFirstAsync(it => it.DeleteMark == null && it.id == id);
        if (contract == null)
        {
            // 未找到合同，返回空对象
            return new { };
        }

        // 返回需要的三个字段给前端
        return new
        {
            contract.F_CustomerId,
            contract.F_ContactId,
            contract.F_AddressId
        };
    }

    /// <summary>
    /// 获取仓库ID数组中的第一个值（用于库存操作）
    /// </summary>
    /// <param name="warehouseIdJson">仓库ID的JSON数组字符串，如 ["2014233789731049472","2026473367191818240"]</param>
    /// <returns>第一个仓库ID，如果解析失败则返回原值</returns>
    private string GetFirstWarehouseId(string warehouseIdJson)
    {
        if (string.IsNullOrEmpty(warehouseIdJson))
            return warehouseIdJson;

        try
        {
            // 尝试解析JSON数组
            var warehouseIds = JsonSerializer.Deserialize<List<string>>(warehouseIdJson);
            if (warehouseIds != null && warehouseIds.Count > 0)
                return warehouseIds[0];
        }
        catch
        {
            // 如果解析失败，可能是旧格式，直接返回原值
        }
        return warehouseIdJson;
    }

    /// <summary>
    /// 批量增加库存（按商品+仓库分组）
    /// </summary>
    private async Task AdjustInventoryAdd(Dictionary<(string F_CustomerId, string WarehouseId), decimal> qtyByKey)
    {
        if (!qtyByKey.Any()) return;
        var keys = qtyByKey.Select(kv => new { kv.Key.F_CustomerId, kv.Key.WarehouseId }).ToList();
        var existingInventories = await _repositoryInventory.AsSugarClient().Queryable<AGoodsInventoryEntity>()
            .Where(it => it.DeleteMark == null && keys.Any(k => k.F_CustomerId == it.F_GoodsId && k.WarehouseId == it.F_WarehouseId))
            .ToListAsync();
        var existingByKey = existingInventories.ToDictionary(it => (it.F_GoodsId ?? "", it.F_WarehouseId ?? ""), it => it);
        var toInsert = new List<AGoodsInventoryEntity>();

        foreach (var kv in qtyByKey)
        {
            var goodsId = kv.Key.F_CustomerId;
            var warehouseId = kv.Key.WarehouseId;
            var addQty = kv.Value;
            var key = (goodsId, warehouseId);
            if (existingByKey.TryGetValue(key, out var inventory))
            {
                inventory.F_Num = (inventory.F_Num ?? 0) + addQty;
            }
            else
            {
                toInsert.Add(new AGoodsInventoryEntity
                {
                    id = SnowflakeIdHelper.NextId(),
                    F_CreatorUserId = _userManager.UserId,
                    F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime(),
                    F_Num = addQty,
                    F_GoodsId = goodsId,
                    F_WarehouseId = warehouseId
                });
            }
        }
        if (existingByKey.Values.Any())
            await _repositoryInventory.AsSugarClient().Updateable(existingByKey.Values.ToList()).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        if (toInsert.Any())
            await _repositoryInventory.AsSugarClient().Insertable(toInsert).ExecuteCommandAsync();
    }

    /// <summary>
    /// 批量减少库存（按商品+仓库分组）
    /// </summary>
    private async Task AdjustInventoryReduce(Dictionary<(string F_CustomerId, string WarehouseId), decimal> qtyByKey)
    {
        if (!qtyByKey.Any()) return;
        foreach (var kv in qtyByKey)
        {
            var goodsId = kv.Key.F_CustomerId;
            var warehouseId = kv.Key.WarehouseId;
            var reduceQty = kv.Value;
            var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == goodsId && it.F_WarehouseId == warehouseId);
            if (inventory != null)
            {
                inventory.F_Num = (inventory.F_Num ?? 0) - reduceQty;
                if (inventory.F_Num < 0) inventory.F_Num = 0;
                await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            }
        }
    }

    /// <summary>
    /// 对单个 (商品, 仓库) 进行增量调整（可正可负）
    /// </summary>
    private async Task AdjustInventorySingle(string goodsId, string warehouseId, decimal diff)
    {
        if (string.IsNullOrEmpty(goodsId) || string.IsNullOrEmpty(warehouseId) || diff == 0) return;
        var inventory = await _repositoryInventory.GetFirstAsync(it => it.DeleteMark == null && it.F_GoodsId == goodsId && it.F_WarehouseId == warehouseId);
        if (inventory != null)
        {
            inventory.F_Num = (inventory.F_Num ?? 0) + diff;
            if (inventory.F_Num < 0) inventory.F_Num = 0;
            await _repositoryInventory.AsSugarClient().Updateable(inventory).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        }
        else if (diff > 0)
        {
            await _repositoryInventory.AsSugarClient().Insertable(new AGoodsInventoryEntity
            {
                id = SnowflakeIdHelper.NextId(),
                F_CreatorUserId = _userManager.UserId,
                F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime(),
                F_Num = diff,
                F_GoodsId = goodsId,
                F_WarehouseId = warehouseId
            }).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 解析仓库ID的JSON数组字符串，返回ID列表
    /// </summary>
    private static List<string> ParseWarehouseIdList(string warehouseIdJson)
    {
        if (string.IsNullOrWhiteSpace(warehouseIdJson)) return new List<string>();
        try
        {
            var arr = warehouseIdJson.ToObject<List<string>>();
            return arr?.Where(s => !string.IsNullOrEmpty(s)).ToList() ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }
}
