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
using JNPF.example.Entitys.Dto.AProdRouting;
using JNPF.example.Entitys.Dto.AProdRoutingStep;
using JNPF.example.Entitys;
using JNPF.example.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using JNPF.Common.Models.Authorize;
using JNPF.Common.Dtos;
using JNPF.example.Entitys.Dto.AProdRoutingStepItem;
using Newtonsoft.Json;
using Aop.Api.Domain;

namespace JNPF.example;

/// <summary>
/// 业务实现：a_prod_routing.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdRouting", Order = 200)]
[Route("api/example/[controller]")]
public class AProdRoutingService : IAProdRoutingService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdRoutingEntity> _repository;
    private readonly ISqlSugarRepository<AProdProcessEntity> _repositoryProcess;
    private readonly ISqlSugarRepository<AProdRoutingStepEntity> _repositoryStep;
    private readonly ISqlSugarRepository<AProdRoutingStepItemEntity> _repositoryStepItem;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"工艺路线编号\",\"field\":\"F_RoutingNo\"},{\"value\":\"工艺路线名称\",\"field\":\"F_RoutingName\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdRoutingService"/>类型的新实例.
    /// </summary>
    public AProdRoutingService(
        ISqlSugarRepository<AProdRoutingStepItemEntity> repositoryStepItem,
        ISqlSugarRepository<AProdRoutingStepEntity> repositoryStep,
        ISqlSugarRepository<AProdProcessEntity> repositoryProcess,
        ISqlSugarRepository<AProdRoutingEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryStepItem = repositoryStepItem;
        _repositoryStep = repositoryStep;
        _repositoryProcess = repositoryProcess;
        _repository = repository;
        _exportImportDataHelper = exportImportDataHelper;
        _fileManager = fileManager;
        _dataInterfaceService = dataInterfaceService;
        _cacheManager = cacheManager;
        _controlParsing = controlParsing;
        _userManager = userManager;
    }

    /// <summary>
    /// 获取a_prod_routing.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Includes(it => it.AProdRoutingStepList)
            .Select(it => new AProdRoutingEntity
            {
                id = it.id,
                F_RoutingNo = it.F_RoutingNo,
                F_RoutingName = it.F_RoutingName,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
                AProdRoutingStepList = it.AProdRoutingStepList,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AProdRoutingInfoOutput>(); 

        if (data == null) return data;
        await _repository.AsSugarClient().ThenMapperAsync(data.tableField7b5631, async aProdRoutingStep =>
        {
            // 创建时间
            aProdRoutingStep.F_CreatorTime = aProdRoutingStep.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            aProdRoutingStep.F_ProcessName = (await _repositoryProcess.GetFirstAsync(it => it.id == aProdRoutingStep.F_ProcessId)).F_ProcessName;


            aProdRoutingStep.tableField3b6f08 = await _repositoryStepItem.AsQueryable().Where(it => it.F_StepId == aProdRoutingStep.F_Id).Select(it => new AProdRoutingStepItemListOutput
            {
                id = it.id,
                F_GoodsId = it.F_GoodsId,
                F_GoodsNo = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_GoodsNo),
                F_Specification = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Specification),
                F_Unit = SqlFunc.Subqueryable<DictionaryDataEntity>().EnableTableFilter().Where(dic => dic.EnCode.Equals(SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => g.F_Unit)) && dic.DictionaryTypeId.Equals("2008448689420505088")).Select(dic => dic.FullName),
                F_InventoryNum = SqlFunc.Subqueryable<AGoodsInventoryEntity>().EnableTableFilter().Where(dic => dic.F_GoodsId.Equals(it.F_GoodsId) && dic.DeleteMark == null && dic.F_Num > 0).Sum(it => it.F_Num) ?? 0,
                F_Num = it.F_Num.ToString(),
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).ToListAsync();
        });
        return data;
    }

    /// <summary>
    /// 获取a_prod_routing列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdRoutingListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();
        var aProdRoutingStepAuthorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            var allAuthorizeWhere = await _userManager.GetCodeGenAuthorizeModuleResource<AProdRoutingListOutput>(input.menuId, "F_Id", 1, _userManager.UserOrigin.Equals("pc") ? true : false);
            authorizeWhere = allAuthorizeWhere.Find(it => it.FieldRule == 0 || it.TableName.Equals("a_prod_routing"))?.conditionalModel;
            if(allAuthorizeWhere.Any(it => it.TableName.Equals("a_prod_routing_step"))) aProdRoutingStepAuthorizeWhere = allAuthorizeWhere.Find(it => it.TableName.Equals("a_prod_routing_step"))?.conditionalModel;
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_RoutingNo), it => it.F_RoutingNo.Contains(input.F_RoutingNo))
            .WhereIF(!string.IsNullOrEmpty(input.F_RoutingName), it => it.F_RoutingName.Contains(input.F_RoutingName))
            .Where(authorizeWhere)
            .WhereIF(aProdRoutingStepAuthorizeWhere?.Count > 0, it => it.AProdRoutingStepList.Any(aProdRoutingStepAuthorizeWhere))
            .Where(it => it.DeleteMark == null)
            .OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort)
            .Select(it => new AProdRoutingListOutput
            {
                id = it.id,
                F_RoutingNo = it.F_RoutingNo,
                F_RoutingName = it.F_RoutingName,
                F_ProcessNum = SqlFunc.Subqueryable<AProdRoutingStepEntity>().EnableTableFilter().Where(ro => ro.F_RoutingId.Equals(it.id) && ro.F_Type == "approver").Count(),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            })
            .ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

            //var setpList = await _repositoryStep.AsQueryable().Where(set => set.F_RoutingId == item.id).OrderBy(set => set.F_SortCode).Select(set => set.F_ProcessId).ToListAsync();
            //foreach (var setp in setpList) {
            //    var processEntity = await _repositoryProcess.GetFirstAsync(pro => pro.id == setp);
            //    if (string.IsNullOrEmpty(item.F_ProcessList))
            //    {
            //        item.F_ProcessList = processEntity.F_ProcessName;
            //    }
            //    else
            //    {

            //        item.F_ProcessList = item.F_ProcessList + "," + processEntity.F_ProcessName;
            //    }
            //}
        });

        var resData = data.list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);
        data.list = resData.ToObject<List<AProdRoutingListOutput>>(CommonConst.options);
        return PageResult<AProdRoutingListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建a_prod_routing.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    [UnitOfWork]
    public async Task Create([FromBody] AProdRoutingCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdRoutingEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdRoutingEntity>(new AProdRoutingEntity()));
        ConfigModel tableField7b5631Config = new ConfigModel
        {
            tableName = "a_prod_routing_step",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "工序",
            children = ExportImportDataHelper.GetTemplateParsing<AProdRoutingStepEntity>(new AProdRoutingStepEntity())
        };
        FieldsModel tableField7b5631Model = new FieldsModel()
        {
            __config__ = tableField7b5631Config,
            __vModel__ = "tableField7b5631"
        };
        fieldList.Add(tableField7b5631Model);
        entity.F_CreatorUserId = _userManager.UserId;
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;

        if (await _repository.IsAnyAsync(it => it.F_RoutingNo.Equals(input.F_RoutingNo) && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "工艺路线编号");
        if (await _repository.IsAnyAsync(it => it.F_RoutingName.Equals(input.F_RoutingName)  && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "工艺路线名称");

        // 自动生成编号逻辑：前缀 LX + yyyyMMddHH，后缀 2 位序号
        if (string.IsNullOrEmpty(entity.F_RoutingNo))
        {
            var prefix = "LX" + DateTime.Now.ToString("yyyyMMddHH");

            // 查询数据库中已有相同前缀的编号
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_RoutingNo != null && it.F_RoutingNo.StartsWith(prefix))
                .Select(it => it.F_RoutingNo)
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
            entity.F_RoutingNo = prefix + nextSeq.ToString("D2");
        }

        var aProdRoutingStepEntityList = input.tableField7b5631.Adapt<List<AProdRoutingStepEntity>>();
        if(aProdRoutingStepEntityList != null)
        {
            foreach (var item in aProdRoutingStepEntityList)
            {
                item.F_Id = SnowflakeIdHelper.NextId();
                item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
            }

            entity.AProdRoutingStepList = aProdRoutingStepEntityList;
        }

        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.AProdRoutingStepList)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新a_prod_routing.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [UnitOfWork]
    public async Task Update(string id, [FromBody] AProdRoutingUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdRoutingEntity>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdRoutingEntity>(new AProdRoutingEntity()));
        ConfigModel tableField7b5631Config = new ConfigModel
        {
            tableName = "a_prod_routing_step",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "工序",
            children = ExportImportDataHelper.GetTemplateParsing<AProdRoutingStepEntity>(new AProdRoutingStepEntity())
        };
        FieldsModel tableField7b5631Model = new FieldsModel()
        {
            __config__ = tableField7b5631Config,
            __vModel__ = "tableField7b5631"
        };
        fieldList.Add(tableField7b5631Model);

        if(await _repository.IsAnyAsync(it => it.F_RoutingName.Equals(input.F_RoutingName)  && !it.id.Equals(id)  && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "工艺路线名称");

        var oldEntity = await _repository.GetFirstAsync(it => it.id == id);
        if (await _repository.IsAnyAsync(it => it.F_RoutingNo.Equals(input.F_RoutingNo)  && !it.id.Equals(id) && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "工艺路线编号");

        if (string.IsNullOrEmpty(entity.F_RoutingNo))
        {
            entity.F_RoutingNo = oldEntity.F_RoutingNo;
        }

        // 移除工艺路线工序信息可能被删除数据
        if (input.tableField7b5631 != null && input.tableField7b5631.Any())
            await _repository.AsSugarClient().Deleteable<AProdRoutingStepEntity>().Where(it => it.F_RoutingId == entity.id && !input.tableField7b5631.Select(it => it.F_Id).ToList().Contains(it.F_Id)).ExecuteCommandAsync();
        else
            await _repository.AsSugarClient().Deleteable<AProdRoutingStepEntity>().Where(it => it.F_RoutingId == entity.id).ExecuteCommandAsync();

        // 新增工艺路线工序信息新数据
        var aProdRoutingStepEntityList = input.tableField7b5631.Adapt<List<AProdRoutingStepEntity>>();

        try
        {
            await _repository.AsUpdateable(entity)
                .UpdateColumns(it => new {
                    it.F_RoutingNo,
                    it.F_RoutingName,
                })
                .ExecuteCommandAsync();

            if(aProdRoutingStepEntityList != null)
            {
                foreach (var item in aProdRoutingStepEntityList)
                {
                    if(item.F_Id.IsNullOrEmpty()){
                        item.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                        item.F_Id = SnowflakeIdHelper.NextId();
                        item.F_RoutingId = entity.id;
                        await _repository.AsSugarClient().Insertable(item).ExecuteCommandAsync();
                    }else{
                        item.F_CreatorTime = null;
                        item.F_RoutingId = entity.id;
                        await _repository.AsSugarClient().Updateable(item).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    }

                    // 移除工序物料信息可能被删除数据
                    if (item.tableField3b6f08 != null && item.tableField3b6f08.Any())
                        await _repositoryStep.AsSugarClient().Deleteable<AProdRoutingStepItemEntity>().Where(it => it.F_StepId == item.F_Id && !item.tableField3b6f08.Select(it => it.id).ToList().Contains(it.id)).ExecuteCommandAsync();
                    else
                        await _repositoryStep.AsSugarClient().Deleteable<AProdRoutingStepItemEntity>().Where(it => it.F_StepId == item.F_Id).ExecuteCommandAsync();

                    var aProdRoutingStepItemEntityList = item.tableField3b6f08.Adapt<List<AProdRoutingStepItemEntity>>();
                    if (aProdRoutingStepItemEntityList != null)
                    {
                        foreach (var itemTwo in aProdRoutingStepItemEntityList)
                        {
                            if (itemTwo.id.IsNullOrEmpty())
                            {
                                itemTwo.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
                                itemTwo.id = SnowflakeIdHelper.NextId();
                                itemTwo.F_StepId = item.F_Id;
                                await _repositoryStep.AsSugarClient().Insertable(itemTwo).ExecuteCommandAsync();
                            }
                            else
                            {
                                itemTwo.F_CreatorTime = null;
                                itemTwo.F_StepId = item.F_Id;
                                await _repositoryStep.AsSugarClient().Updateable(itemTwo).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                            }


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
    /// 获取工艺路线流程信息.
    /// </summary>
    /// <param name="id">路线ID.</param>
    /// <returns></returns>
    [HttpGet("FlowInfo/{id}")]
    public async Task<dynamic> GetFlowInfo(string id)
    {
        // 1. 查询路线信息
        var entity = await _repository.GetFirstAsync(it => it.id.Equals(id) && it.DeleteMark == null);
        if (entity == null)
            throw Oops.Oh(ErrorCode.COM1005);

        // 2. 查询工序节点列表
        var stepList = await _repositoryStep.AsQueryable()
            .Where(it => it.F_RoutingId.Equals(id) && it.DeleteMark == null)
            .OrderBy(it => it.F_SortCode)
            .ToListAsync();

        // 3. 查询所有工序节点的物料信息
        var stepIds = stepList.Select(it => it.F_Id).ToList();
        var stepItemList = await _repositoryStepItem.AsQueryable()
            .Where(it => stepIds.Contains(it.F_StepId))
            .ToListAsync();
        var stepItemDict = stepItemList.GroupBy(it => it.F_StepId).ToDictionary(g => g.Key, g => g.ToList());

        // 4. 构建流程节点字典
        var flowNodes = new Dictionary<string, FlowNode>();
        
        foreach (var step in stepList)
        {
            var node = new FlowNode
            {
                nodeId = step.F_NodeId,
                type = step.F_Type,
                nodeName = step.F_NodeName,
            };

            // 构建formData内容（工序详细字段）
            var content = new FlowNodeContent
            {
                F_BOMId = step.F_BOMId,
                F_ProcessId = step.F_ProcessId,
                F_ResponsibleUserId = step.F_ResponsibleUserId,
                F_WagePrice = step.F_WagePrice,
                F_StdHour = step.F_StdHour,
                F_StdMinute = step.F_StdMinute,
                F_StdSecond = step.F_StdSecond,
                F_PriceType = step.F_PriceType,
                F_UnitRatio = step.F_UnitRatioProd,
                F_ReportUnit = step.F_UnitRatioBase,
                F_IsOutsource = step.F_IsOutsource,
                F_IsQc = step.F_IsQc,
                F_DefectHandle = step.F_DefectHandle,
                F_Files = step.F_Files != null ? step.F_Files.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>(),
                F_Description = step.F_Description
            };

            // 填充工序物料信息
            if (stepItemDict.TryGetValue(step.F_Id, out var items))
            {
                content.tableField3b6f08 = items.Adapt<List<AProdRoutingStepItemCrInput>>();
            }
            else
            {
                content.tableField3b6f08 = new List<AProdRoutingStepItemCrInput>();
            }

            // 如果是 approver 节点，填充路线编号和名称
            if (step.F_Type == "approver")
            {
                content.F_RoutingNo = entity.F_RoutingNo;
                content.F_RoutingName = entity.F_RoutingName;

                node.content = await _repository.AsSugarClient().Queryable<UserEntity>().Where(a => a.Id == step.F_ResponsibleUserId).Select(it => SqlFunc.MergeString(it.RealName, "/", it.Account)).FirstAsync();
            }

            // formData直接返回对象
            node.formData = content;

            // 根据节点类型解析特定数据
            if (!string.IsNullOrEmpty(step.F_Description))
            {
                try
                {
                    switch (step.F_Type)
                    {
                        case "global":
                            // 解析全局节点数据
                            node.formId = "";
                            node.formName = "";
                            break;
                        case "approver":
                            // 解析审批节点数据
                            break;
                    }
                }
                catch
                {
                    // 解析失败时忽略
                }
            }

            flowNodes[step.F_NodeId] = node;
        }

        // 5. 构建返回结果
        var result = new {
            id = entity.id,
            F_RoutingNo = entity.F_RoutingNo,
            F_RoutingName = entity.F_RoutingName,
            flowId = entity.FlowId,
            flowXml = entity.F_XML,
            flowNodes = flowNodes,
            flowConfig = (string)null // 如果有流程配置，可以从数据库读取
        };

        return result;
    }

    /// <summary>
    /// 新建a_prod_routing.
    /// </summary>
    /// <param name="input">参数.</param> 
    /// <returns></returns>
    [HttpPost("FlowCreate")]
    [UnitOfWork]
    public async Task FlowCreate([FromBody] AProdRoutingFlowInput input)
    {
        if (input == null)
            throw Oops.Oh(ErrorCode.COM1000);

        // 1. 解析路线ID
        var routingId = input.id;
        if (string.IsNullOrEmpty(routingId) || routingId == "-1")
        {
            routingId = SnowflakeIdHelper.NextId();
        }

        // 2. 解析版本ID
        var flowId = input.flowId;

        // 3. 解析XML字符串(需要URL解码)
        var flowXml = input.flowXml;
        if (!string.IsNullOrEmpty(flowXml))
        {
            flowXml = Uri.UnescapeDataString(flowXml);
        }

        // 4. 从第一个 approver 节点获取路线编号和名称
        string? routingNo = null;
        string? routingName = null;
        if (input.flowNodes != null && input.flowNodes.Count > 0)
        {
            var firstApproverNode = input.flowNodes.Values.FirstOrDefault(n => n.type == "approver");
            if (firstApproverNode?.formData != null)
            {
                routingNo = firstApproverNode.formData.F_RoutingNo;
                routingName = firstApproverNode.formData.F_RoutingName;
            }
        }

        if (!string.IsNullOrEmpty(routingName) && await _repository.IsAnyAsync(it => it.F_RoutingName.Equals(routingName) && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "工艺路线名称");

        // 5. 自动生成编号逻辑：前缀 LX + yyyyMMddHH，后缀 2 位序号
        if (string.IsNullOrEmpty(routingNo))
        {
            var prefix = "LX" + DateTime.Now.ToString("yyyyMMddHH");
            var existingNos = await _repository.AsQueryable()
                .Where(it => it.F_RoutingNo != null && it.F_RoutingNo.StartsWith(prefix))
                .Select(it => it.F_RoutingNo)
                .ToListAsync();

            int maxSeq = 0;
            foreach (var no in existingNos)
            {
                if (no.Length > prefix.Length && int.TryParse(no.Substring(prefix.Length), out int seq))
                {
                    if (seq > maxSeq) maxSeq = seq;
                }
            }
            routingNo = prefix + (maxSeq + 1).ToString("D2");
        }

        // 6. 检查重复
        if (await _repository.IsAnyAsync(it => it.F_RoutingNo.Equals(routingNo)  && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "工艺路线编号");

        // 7. 创建路线实体
        var entity = new AProdRoutingEntity
        {
            id = routingId,
            FlowId = flowId,
            F_XML = flowXml,
            F_RoutingNo = routingNo,
            F_RoutingName = routingName,
            F_CreatorUserId = _userManager.UserId,
            F_CreatorTime = DateTime.Now,
            F_CreatorOrganizeId = _userManager.User.OrganizeId
        };

        // 8. 解析流程节点并创建工序列表(保存所有节点类型)
        var aProdRoutingStepList = new List<AProdRoutingStepEntity>();
        var flowNodes = input.flowNodes;
        if (flowNodes != null && flowNodes.Count > 0)
        {
            int sortCode = 0;
            foreach (var node in flowNodes)
            {
                var nodeId = node.Key;
                var nodeValue = node.Value;

                // 跳过空节点
                if (nodeValue == null) continue;

                // 从formData中获取节点内容字段
                var content = nodeValue.formData;

                var stepEntity = new AProdRoutingStepEntity
                {
                    F_Id = SnowflakeIdHelper.NextId(),
                    F_RoutingId = routingId,
                    F_NodeId = nodeId,
                    F_NodeName = nodeValue.nodeName,
                    F_Type = nodeValue.type,
                    F_SortCode = sortCode++,
                    F_CreatorTime = entity.F_CreatorTime
                };

                if (content != null)
                {
                    if (string.IsNullOrEmpty(content.F_ProcessId))
                    {
                        throw Oops.Bah(ErrorCode.COM1018, "请填写工序");
                    }
                    //if (string.IsNullOrEmpty(content.F_ResponsibleUserId))
                    //{
                    //    throw Oops.Bah(ErrorCode.COM1018, "请填写负责人");
                    //}

                    stepEntity.F_BOMId = content.F_BOMId;
                    stepEntity.F_ProcessId = content.F_ProcessId;
                    stepEntity.F_ParentId = nodeValue.parentId;
                    stepEntity.F_ResponsibleUserId = content.F_ResponsibleUserId;
                    stepEntity.F_WagePrice = content.F_WagePrice;
                    stepEntity.F_StdHour = content.F_StdHour;
                    stepEntity.F_StdMinute = content.F_StdMinute;
                    stepEntity.F_StdSecond = content.F_StdSecond;
                    stepEntity.F_PriceType = content.F_PriceType;
                    stepEntity.F_UnitRatioProd = content.F_UnitRatio;
                    stepEntity.F_UnitRatioBase = content.F_ReportUnit;
                    stepEntity.F_IsOutsource = content.F_IsOutsource;
                    stepEntity.F_IsQc = content.F_IsQc;
                    stepEntity.F_DefectHandle = content.F_DefectHandle;
                    stepEntity.F_Files = content.F_Files != null && content.F_Files.Count > 0 ? content.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null;
                    stepEntity.F_Description = content.F_Description;



                    // 处理工序物料信息(tableField3b6f08)
                    if (content.tableField3b6f08 != null && content.tableField3b6f08.Count > 0)
                    {
                        stepEntity.AProdRoutingStepItemList = content.tableField3b6f08.Adapt<List<AProdRoutingStepItemEntity>>();
                        foreach (var item in stepEntity.AProdRoutingStepItemList)
                        {
                            item.id = SnowflakeIdHelper.NextId();
                            item.F_CreatorTime = DateTime.Now;
                        }
                    }
                }

                aProdRoutingStepList.Add(stepEntity);
            }
        }

        // 9. 关联工序列表到路线实体
        entity.AProdRoutingStepList = aProdRoutingStepList;

        // 10. 保存到数据库(使用导航属性插入，会自动级联保存工序物料)
        await _repository.AsSugarClient().InsertNav(entity)
            .Include(it => it.AProdRoutingStepList)
            .ThenInclude(it => it.AProdRoutingStepItemList)
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新a_prod_routing流程.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("FlowUpdate/{id}")]
    [UnitOfWork]
    public async Task FlowUpdate(string id, [FromBody] AProdRoutingFlowInput input)
    {
        if (input == null)
            throw Oops.Oh(ErrorCode.COM1000);

        // 1. 查询原有路线信息
        var entity = await _repository.GetFirstAsync(it => it.id.Equals(id) && it.DeleteMark == null);
        if (entity == null)
            throw Oops.Oh(ErrorCode.COM1005);

        // 2. 解析版本ID
        var flowId = input.flowId;

        // 3. 解析XML字符串(需要URL解码)
        var flowXml = input.flowXml;
        if (!string.IsNullOrEmpty(flowXml))
        {
            flowXml = Uri.UnescapeDataString(flowXml);
        }

        // 4. 从第一个 approver 节点获取路线编号和名称
        string? routingNo = null;
        string? routingName = null;
        if (input.flowNodes != null && input.flowNodes.Count > 0)
        {
            var firstApproverNode = input.flowNodes.Values.FirstOrDefault(n => n.type == "approver");
            if (firstApproverNode?.formData != null)
            {
                routingNo = firstApproverNode.formData.F_RoutingNo;
                routingName = firstApproverNode.formData.F_RoutingName;
            }
        }

        // 5. 如果未提供编号/名称，保留原值
        if (string.IsNullOrEmpty(routingNo))
        {
            routingNo = entity.F_RoutingNo;
        }
        if (string.IsNullOrEmpty(routingName))
        {
            routingName = entity.F_RoutingName;
        }

        if (!string.IsNullOrEmpty(routingName) && await _repository.IsAnyAsync(it => it.F_RoutingName.Equals(routingName) && !it.id.Equals(id) && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "工艺路线名称");

        // 6. 检查重复（排除当前记录）
        if (await _repository.IsAnyAsync(it => it.F_RoutingNo.Equals(routingNo) && !it.id.Equals(id) && it.DeleteMark == null))
            throw Oops.Bah(ErrorCode.COM1023, "工艺路线编号");

        // 7. 更新路线信息
        entity.FlowId = flowId;
        entity.F_XML = flowXml;
        entity.F_RoutingNo = routingNo;
        entity.F_RoutingName = routingName;

        // 8. 查询原有的工序节点
        var existingSteps = await _repositoryStep.AsQueryable()
            .Where(it => it.F_RoutingId.Equals(id) && it.DeleteMark == null)
            .ToListAsync();
        var existingStepDict = existingSteps.ToDictionary(it => it.F_NodeId, it => it);

        // 6. 解析流程节点并处理增删改
        var stepsToInsert = new List<AProdRoutingStepEntity>();
        var stepsToUpdate = new List<AProdRoutingStepEntity>();
        var processedNodeIds = new HashSet<string>();
        var flowNodes = input.flowNodes;

        if (flowNodes != null && flowNodes.Count > 0)
        {
            // 先构建节点层级关系，然后按层级设置排序码
            var nodeHierarchy = BuildNodeHierarchy(flowNodes);
            var sortCodesByParent = new Dictionary<string, int>();
            
            foreach (var node in nodeHierarchy)
            {
                var nodeId = node.Key;
                var nodeValue = node.Value;

                // 跳过空节点
                if (nodeValue == null) continue;

                processedNodeIds.Add(nodeId);

                // 从formData中获取节点内容字段
                var content = nodeValue.formData;

                // 根据nodeId判断是新增还是修改
                if (existingStepDict.TryGetValue(nodeId, out var existingStep))
                {
                    // 存在相同nodeId，更新原有记录
                    existingStep.F_NodeName = nodeValue.nodeName;
                    existingStep.F_Type = nodeValue.type;
                    existingStep.F_ParentId = nodeValue.parentId;
                    
                    // 设置排序码：相同上级ID的节点有相同的排序码，下级节点基于自己的排序码加1
                    if (string.IsNullOrEmpty(nodeValue.parentId))
                    {
                        // 根节点，使用独立排序码
                        if (!sortCodesByParent.ContainsKey("root"))
                            sortCodesByParent["root"] = 0;
                        existingStep.F_SortCode = sortCodesByParent["root"]++;
                    }
                    else
                    {
                        // 子节点，使用父级的排序码加1
                        if (!sortCodesByParent.ContainsKey(nodeValue.parentId))
                            sortCodesByParent[nodeValue.parentId] = 0;
                        existingStep.F_SortCode = sortCodesByParent[nodeValue.parentId]++;
                    }
                    // 从formData中更新字段
                    if (content != null)
                    {
                        if (string.IsNullOrEmpty(content.F_ProcessId))
                        {
                            throw Oops.Bah(ErrorCode.COM1018, "请填写工序");
                        }
                        //if (string.IsNullOrEmpty(content.F_ResponsibleUserId))
                        //{
                        //    throw Oops.Bah(ErrorCode.COM1018, "请填写负责人");
                        //}

                        existingStep.F_BOMId = content.F_BOMId;
                        existingStep.F_ProcessId = content.F_ProcessId;
                        existingStep.F_ResponsibleUserId = content.F_ResponsibleUserId;
                        existingStep.F_WagePrice = content.F_WagePrice;
                        existingStep.F_StdHour = content.F_StdHour;
                        existingStep.F_StdMinute = content.F_StdMinute;
                        existingStep.F_StdSecond = content.F_StdSecond;
                        existingStep.F_PriceType = content.F_PriceType;
                        existingStep.F_UnitRatioProd = content.F_UnitRatio;
                        existingStep.F_UnitRatioBase = content.F_ReportUnit;
                        existingStep.F_IsOutsource = content.F_IsOutsource;
                        existingStep.F_IsQc = content.F_IsQc;
                        existingStep.F_DefectHandle = content.F_DefectHandle;
                        existingStep.F_Files = content.F_Files != null && content.F_Files.Count > 0 ? content.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null;
                        existingStep.F_Description = content.F_Description;


                        // 处理工序物料信息(tableField3b6f08)
                        if (content.tableField3b6f08 != null)
                        {
                            existingStep.AProdRoutingStepItemList = content.tableField3b6f08.Adapt<List<AProdRoutingStepItemEntity>>();
                            foreach (var item in existingStep.AProdRoutingStepItemList)
                            {
                                item.F_StepId = existingStep.F_Id;
                                if (string.IsNullOrEmpty(item.id))
                                {
                                    item.id = SnowflakeIdHelper.NextId();
                                    item.F_CreatorTime = DateTime.Now;
                                }
                            }
                        }
                    }

                    // 根据节点类型处理特定数据
                    switch (nodeValue.type)
                    {
                        case "global":
                            // 全局节点：保存表单信息到备注
                            break;
                        case "approver":
                            // 审批节点/工序节点：保存审批人列表
                            break;
                        case "start":
                        case "end":
                        case "connect":
                        default:
                            break;
                    }

                    stepsToUpdate.Add(existingStep);
                }
                else
                {
                    // 不存在相同nodeId，新增记录
                    var stepEntity = new AProdRoutingStepEntity();
                    stepEntity.F_Id = SnowflakeIdHelper.NextId();
                    stepEntity.F_RoutingId = id;
                    stepEntity.F_NodeId = nodeId;
                    stepEntity.F_NodeName = nodeValue.nodeName;
                    stepEntity.F_Type = nodeValue.type;
                    stepEntity.F_ParentId = nodeValue.parentId;
                    
                    // 设置排序码：相同上级ID的节点有相同的排序码，下级节点基于自己的排序码加1
                    if (string.IsNullOrEmpty(nodeValue.parentId))
                    {
                        // 根节点，使用独立排序码
                        if (!sortCodesByParent.ContainsKey("root"))
                            sortCodesByParent["root"] = 0;
                        stepEntity.F_SortCode = sortCodesByParent["root"]++;
                    }
                    else
                    {
                        // 子节点，使用父级的排序码加1
                        if (!sortCodesByParent.ContainsKey(nodeValue.parentId))
                            sortCodesByParent[nodeValue.parentId] = 0;
                        stepEntity.F_SortCode = sortCodesByParent[nodeValue.parentId]++;
                    }
                    
                    stepEntity.F_CreatorTime = DateTime.Now;

                    // 从formData中设置字段
                    if (content != null)
                    {
                        if (string.IsNullOrEmpty(content.F_ProcessId))
                        {
                            throw Oops.Bah(ErrorCode.COM1018, "请填写工序");
                        }
                        //if (string.IsNullOrEmpty(content.F_ResponsibleUserId))
                        //{
                        //    throw Oops.Bah(ErrorCode.COM1018, "请填写负责人");
                        //}

                        stepEntity.F_BOMId = content.F_BOMId;
                        stepEntity.F_ProcessId = content.F_ProcessId;
                        stepEntity.F_ResponsibleUserId = content.F_ResponsibleUserId;
                        stepEntity.F_WagePrice = content.F_WagePrice;
                        stepEntity.F_StdHour = content.F_StdHour;
                        stepEntity.F_StdMinute = content.F_StdMinute;
                        stepEntity.F_StdSecond = content.F_StdSecond;
                        stepEntity.F_PriceType = content.F_PriceType;
                        stepEntity.F_UnitRatioProd = content.F_UnitRatio;
                        stepEntity.F_UnitRatioBase = content.F_ReportUnit;
                        stepEntity.F_IsOutsource = content.F_IsOutsource;
                        stepEntity.F_IsQc = content.F_IsQc;
                        stepEntity.F_DefectHandle = content.F_DefectHandle;
                        stepEntity.F_Files = content.F_Files != null && content.F_Files.Count > 0 ? content.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null;
                        stepEntity.F_Description = content.F_Description;

                        // 处理工序物料信息(tableField3b6f08)
                        if (content.tableField3b6f08 != null && content.tableField3b6f08.Count > 0)
                        {
                            stepEntity.AProdRoutingStepItemList = content.tableField3b6f08.Adapt<List<AProdRoutingStepItemEntity>>();
                            foreach (var item in stepEntity.AProdRoutingStepItemList)
                            {
                                item.id = SnowflakeIdHelper.NextId();
                                item.F_CreatorTime = DateTime.Now;
                            }
                        }
                    }

                    // 根据节点类型处理特定数据
                    switch (nodeValue.type)
                    {
                        case "global":
                            // 全局节点：保存表单信息到备注
                            break;
                        case "approver":
                            // 审批节点/工序节点：保存审批人列表
                            break;
                        case "start":
                        case "end":
                        case "connect":
                        default:
                            // 其他类型节点：保存基本信息即可
                            break;
                    }

                    stepsToInsert.Add(stepEntity);
                }
            }
        }

        // 添加构建节点层级关系的方法
        Dictionary<string, FlowNode> BuildNodeHierarchy(Dictionary<string, FlowNode> nodes)
        {
            // 先添加所有节点
            var hierarchy = new Dictionary<string, FlowNode>(nodes);
            
            // 按层级排序：先处理根节点，再处理子节点
            return hierarchy
                .OrderBy(n => string.IsNullOrEmpty(n.Value.parentId) ? 0 : 1)
                .ThenBy(n => n.Key)
                .ToDictionary(n => n.Key, n => n.Value);
        }

        // 7. 删除前端未传递的节点（已被删除的节点）及其关联的物料
        var stepsToDelete = existingSteps
            .Where(it => !processedNodeIds.Contains(it.F_NodeId))
            .ToList();

        if (stepsToDelete.Count > 0)
        {
            var stepIdsToDelete = stepsToDelete.Select(it => it.F_Id).ToList();

            // 先删除关联的工序物料
            await _repositoryStep.AsSugarClient().Deleteable<AProdRoutingStepItemEntity>()
                .Where(it => stepIdsToDelete.Contains(it.F_StepId))
                .ExecuteCommandAsync();

            // 再删除工序节点
            await _repositoryStep.AsDeleteable()
                .In(it => it.F_Id, stepIdsToDelete)
                .ExecuteCommandAsync();
        }

        // 8. 更新路线信息
        await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.FlowId,
                it.F_XML,
                it.F_RoutingNo,
                it.F_RoutingName
            })
            .ExecuteCommandAsync();

        // 9. 批量更新已有的工序节点及其物料
        foreach (var step in stepsToUpdate)
        {
            // 更新工序节点基本信息
            await _repositoryStep.AsUpdateable(step)
                .UpdateColumns(it => new
                {
                    it.F_NodeName,
                    it.F_Type,
                    it.F_SortCode,
                    it.F_Description,
                    it.F_ProcessId,
                    it.F_ResponsibleUserId,
                    it.F_BOMId,
                    it.F_WagePrice,
                    it.F_StdHour,
                    it.F_StdMinute,
                    it.F_StdSecond,
                    it.F_PriceType,
                    it.F_UnitRatioProd,
                    it.F_UnitRatioBase,
                    it.F_IsOutsource,
                    it.F_IsQc,
                    it.F_DefectHandle,
                    it.F_Files
                })
                .ExecuteCommandAsync();

            // 处理工序物料：先删除旧的，再插入新的
            if (step.AProdRoutingStepItemList != null)
            {
                // 删除该工序下所有旧的物料
                await _repositoryStep.AsSugarClient().Deleteable<AProdRoutingStepItemEntity>()
                    .Where(it => it.F_StepId == step.F_Id)
                    .ExecuteCommandAsync();

                // 插入新的物料
                if (step.AProdRoutingStepItemList.Count > 0)
                {
                    await _repositoryStep.AsSugarClient().Insertable(step.AProdRoutingStepItemList)
                        .ExecuteCommandAsync();
                }
            }
        }

        // 10. 批量插入新增的工序节点及其物料
        foreach (var step in stepsToInsert)
        {
            // 插入工序节点
            await _repositoryStep.AsInsertable(step).ExecuteCommandAsync();

            // 插入关联的物料
            if (step.AProdRoutingStepItemList != null && step.AProdRoutingStepItemList.Count > 0)
            {
                foreach (var item in step.AProdRoutingStepItemList)
                {
                    item.F_StepId = step.F_Id;
                }
                await _repositoryStep.AsSugarClient().Insertable(step.AProdRoutingStepItemList)
                    .ExecuteCommandAsync();
            }
        }
    }


    //public async Task FlowCreate2([FromBody] AProdRoutingFlowInput input)
    //{
    //    if (input == null)
    //        throw Oops.Oh(ErrorCode.COM1000);

    //    // 1. 解析路线ID
    //    var routingId = input.id;
    //    if (string.IsNullOrEmpty(routingId) || routingId == "-1")
    //    {
    //        routingId = SnowflakeIdHelper.NextId();
    //    }

    //    // 2. 解析版本ID
    //    var flowId = input.flowId;

    //    // 3. 解析XML字符串(需要URL解码)
    //    var flowXml = input.flowXml;
    //    if (!string.IsNullOrEmpty(flowXml))
    //    {
    //        flowXml = Uri.UnescapeDataString(flowXml);
    //    }

    //    // 4. 从第一个 approver 节点获取路线编号和名称
    //    string? routingNo = null;
    //    string? routingName = null;
    //    if (input.flowNodes != null && input.flowNodes.Count > 0)
    //    {
    //        var firstApproverNode = input.flowNodes.Values.FirstOrDefault(n => n.type == "approver");
    //        if (firstApproverNode?.formData != null)
    //        {
    //            routingNo = firstApproverNode.formData.F_RoutingNo;
    //            routingName = firstApproverNode.formData.F_RoutingName;
    //        }
    //    }

    //    if (!string.IsNullOrEmpty(routingName) && await _repository.IsAnyAsync(it => it.F_RoutingName.Equals(routingName) && it.DeleteMark == null))
    //        throw Oops.Bah(ErrorCode.COM1023, "工艺路线名称");

    //    // 5. 自动生成编号逻辑：前缀 LX + yyyyMMddHH，后缀 2 位序号
    //    if (string.IsNullOrEmpty(routingNo))
    //    {
    //        var prefix = "LX" + DateTime.Now.ToString("yyyyMMddHH");
    //        var existingNos = await _repository.AsQueryable()
    //            .Where(it => it.F_RoutingNo != null && it.F_RoutingNo.StartsWith(prefix))
    //            .Select(it => it.F_RoutingNo)
    //            .ToListAsync();

    //        int maxSeq = 0;
    //        foreach (var no in existingNos)
    //        {
    //            if (no.Length > prefix.Length && int.TryParse(no.Substring(prefix.Length), out int seq))
    //            {
    //                if (seq > maxSeq) maxSeq = seq;
    //            }
    //        }
    //        routingNo = prefix + (maxSeq + 1).ToString("D2");
    //    }

    //    // 6. 检查重复
    //    if (await _repository.IsAnyAsync(it => it.F_RoutingNo.Equals(routingNo) && it.DeleteMark == null))
    //        throw Oops.Bah(ErrorCode.COM1023, "工艺路线编号");

    //    // 7. 创建路线实体
    //    var entity = new AProdRoutingEntity
    //    {
    //        id = routingId,
    //        FlowId = flowId,
    //        F_XML = flowXml,
    //        F_RoutingNo = routingNo,
    //        F_RoutingName = routingName,
    //        F_CreatorUserId = _userManager.UserId,
    //        F_CreatorTime = DateTime.Now,
    //        F_CreatorOrganizeId = _userManager.User.OrganizeId
    //    };

    //    // 8. 解析流程节点并创建工序列表(保存所有节点类型)
    //    var aProdRoutingStepList = new List<AProdRoutingStepEntity>();
    //    var flowNodes = input.flowNodes;
    //    if (flowNodes != null && flowNodes.Count > 0)
    //    {
    //        int sortCode = 0;
    //        foreach (var node in flowNodes)
    //        {
    //            var nodeId = node.Key;
    //            var nodeValue = node.Value;

    //            // 跳过空节点
    //            if (nodeValue == null) continue;

    //            // 从formData中获取节点内容字段
    //            var content = nodeValue.formData;

    //            var stepEntity = new AProdRoutingStepEntity
    //            {
    //                F_Id = SnowflakeIdHelper.NextId(),
    //                F_RoutingId = routingId,
    //                F_NodeId = nodeId,
    //                F_NodeName = nodeValue.nodeName,
    //                F_Type = nodeValue.type,
    //                F_SortCode = sortCode++,
    //                F_CreatorTime = entity.F_CreatorTime
    //            };

    //            if (content != null)
    //            {
    //                if (string.IsNullOrEmpty(content.F_ProcessId))
    //                {
    //                    throw Oops.Bah(ErrorCode.COM1018, "请填写工序");
    //                }
    //                if (string.IsNullOrEmpty(content.F_ResponsibleUserId))
    //                {
    //                    throw Oops.Bah(ErrorCode.COM1018, "请填写负责人");
    //                }

    //                stepEntity.F_BOMId = content.F_BOMId;
    //                stepEntity.F_ProcessId = content.F_ProcessId;
    //                stepEntity.F_ResponsibleUserId = content.F_ResponsibleUserId;
    //                stepEntity.F_WagePrice = content.F_WagePrice;
    //                stepEntity.F_StdHour = content.F_StdHour;
    //                stepEntity.F_StdMinute = content.F_StdMinute;
    //                stepEntity.F_StdSecond = content.F_StdSecond;
    //                stepEntity.F_PriceType = content.F_PriceType;
    //                stepEntity.F_UnitRatioProd = content.F_UnitRatio;
    //                stepEntity.F_UnitRatioBase = content.F_ReportUnit;
    //                stepEntity.F_IsOutsource = content.F_IsOutsource;
    //                stepEntity.F_IsQc = content.F_IsQc;
    //                stepEntity.F_DefectHandle = content.F_DefectHandle;
    //                stepEntity.F_Files = content.F_Files != null && content.F_Files.Count > 0 ? content.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null;
    //                stepEntity.F_Description = content.F_Description;



    //                // 处理工序物料信息(tableField3b6f08)
    //                if (content.tableField3b6f08 != null && content.tableField3b6f08.Count > 0)
    //                {
    //                    stepEntity.AProdRoutingStepItemList = content.tableField3b6f08.Adapt<List<AProdRoutingStepItemEntity>>();
    //                    foreach (var item in stepEntity.AProdRoutingStepItemList)
    //                    {
    //                        item.id = SnowflakeIdHelper.NextId();
    //                        item.F_CreatorTime = DateTime.Now;
    //                    }
    //                }
    //            }

    //            aProdRoutingStepList.Add(stepEntity);
    //        }
    //    }

    //    // 9. 关联工序列表到路线实体
    //    entity.AProdRoutingStepList = aProdRoutingStepList;

    //    // 10. 保存到数据库(使用导航属性插入，会自动级联保存工序物料)
    //    await _repository.AsSugarClient().InsertNav(entity)
    //        .Include(it => it.AProdRoutingStepList)
    //        .ThenInclude(it => it.AProdRoutingStepItemList)
    //        .ExecuteCommandAsync();
    //}

    //public async Task FlowUpdate2(string id, [FromBody] AProdRoutingFlowInput input)
    //{
    //    if (input == null)
    //        throw Oops.Oh(ErrorCode.COM1000);

    //    // 1. 查询原有路线信息
    //    var entity = await _repository.GetFirstAsync(it => it.id.Equals(id) && it.DeleteMark == null);
    //    if (entity == null)
    //        throw Oops.Oh(ErrorCode.COM1005);

    //    // 2. 解析版本ID
    //    var flowId = input.flowId;

    //    // 3. 解析XML字符串(需要URL解码)
    //    var flowXml = input.flowXml;
    //    if (!string.IsNullOrEmpty(flowXml))
    //    {
    //        flowXml = Uri.UnescapeDataString(flowXml);
    //    }

    //    // 4. 从第一个 approver 节点获取路线编号和名称
    //    string? routingNo = null;
    //    string? routingName = null;
    //    if (input.flowNodes != null && input.flowNodes.Count > 0)
    //    {
    //        var firstApproverNode = input.flowNodes.Values.FirstOrDefault(n => n.type == "approver");
    //        if (firstApproverNode?.formData != null)
    //        {
    //            routingNo = firstApproverNode.formData.F_RoutingNo;
    //            routingName = firstApproverNode.formData.F_RoutingName;
    //        }
    //    }

    //    // 5. 如果未提供编号/名称，保留原值
    //    if (string.IsNullOrEmpty(routingNo))
    //    {
    //        routingNo = entity.F_RoutingNo;
    //    }
    //    if (string.IsNullOrEmpty(routingName))
    //    {
    //        routingName = entity.F_RoutingName;
    //    }

    //    if (!string.IsNullOrEmpty(routingName) && await _repository.IsAnyAsync(it => it.F_RoutingName.Equals(routingName) && !it.id.Equals(id) && it.DeleteMark == null))
    //        throw Oops.Bah(ErrorCode.COM1023, "工艺路线名称");

    //    // 6. 检查重复（排除当前记录）
    //    if (await _repository.IsAnyAsync(it => it.F_RoutingNo.Equals(routingNo) && !it.id.Equals(id) && it.DeleteMark == null))
    //        throw Oops.Bah(ErrorCode.COM1023, "工艺路线编号");

    //    // 7. 更新路线信息
    //    entity.FlowId = flowId;
    //    entity.F_XML = flowXml;
    //    entity.F_RoutingNo = routingNo;
    //    entity.F_RoutingName = routingName;

    //    // 8. 查询原有的工序节点
    //    var existingSteps = await _repositoryStep.AsQueryable()
    //        .Where(it => it.F_RoutingId.Equals(id) && it.DeleteMark == null)
    //        .ToListAsync();
    //    var existingStepDict = existingSteps.ToDictionary(it => it.F_NodeId, it => it);

    //    // 6. 解析流程节点并处理增删改
    //    var stepsToInsert = new List<AProdRoutingStepEntity>();
    //    var stepsToUpdate = new List<AProdRoutingStepEntity>();
    //    var processedNodeIds = new HashSet<string>();
    //    var flowNodes = input.flowNodes;

    //    if (flowNodes != null && flowNodes.Count > 0)
    //    {
    //        int sortCode = 0;
    //        foreach (var node in flowNodes)
    //        {
    //            var nodeId = node.Key;
    //            var nodeValue = node.Value;

    //            // 跳过空节点
    //            if (nodeValue == null) continue;

    //            processedNodeIds.Add(nodeId);

    //            // 从formData中获取节点内容字段
    //            var content = nodeValue.formData;

    //            // 根据nodeId判断是新增还是修改
    //            if (existingStepDict.TryGetValue(nodeId, out var existingStep))
    //            {
    //                // 存在相同nodeId，更新原有记录
    //                existingStep.F_NodeName = nodeValue.nodeName;
    //                existingStep.F_Type = nodeValue.type;
    //                existingStep.F_SortCode = sortCode++;

    //                // 从formData中更新字段
    //                if (content != null)
    //                {
    //                    if (string.IsNullOrEmpty(content.F_ProcessId))
    //                    {
    //                        throw Oops.Bah(ErrorCode.COM1018, "请填写工序");
    //                    }
    //                    if (string.IsNullOrEmpty(content.F_ResponsibleUserId))
    //                    {
    //                        throw Oops.Bah(ErrorCode.COM1018, "请填写负责人");
    //                    }

    //                    existingStep.F_BOMId = content.F_BOMId;
    //                    existingStep.F_ProcessId = content.F_ProcessId;
    //                    existingStep.F_ResponsibleUserId = content.F_ResponsibleUserId;
    //                    existingStep.F_WagePrice = content.F_WagePrice;
    //                    existingStep.F_StdHour = content.F_StdHour;
    //                    existingStep.F_StdMinute = content.F_StdMinute;
    //                    existingStep.F_StdSecond = content.F_StdSecond;
    //                    existingStep.F_PriceType = content.F_PriceType;
    //                    existingStep.F_UnitRatioProd = content.F_UnitRatio;
    //                    existingStep.F_UnitRatioBase = content.F_ReportUnit;
    //                    existingStep.F_IsOutsource = content.F_IsOutsource;
    //                    existingStep.F_IsQc = content.F_IsQc;
    //                    existingStep.F_DefectHandle = content.F_DefectHandle;
    //                    existingStep.F_Files = content.F_Files != null && content.F_Files.Count > 0 ? content.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null;
    //                    existingStep.F_Description = content.F_Description;


    //                    // 处理工序物料信息(tableField3b6f08)
    //                    if (content.tableField3b6f08 != null)
    //                    {
    //                        existingStep.AProdRoutingStepItemList = content.tableField3b6f08.Adapt<List<AProdRoutingStepItemEntity>>();
    //                        foreach (var item in existingStep.AProdRoutingStepItemList)
    //                        {
    //                            item.F_StepId = existingStep.F_Id;
    //                            if (string.IsNullOrEmpty(item.id))
    //                            {
    //                                item.id = SnowflakeIdHelper.NextId();
    //                                item.F_CreatorTime = DateTime.Now;
    //                            }
    //                        }
    //                    }
    //                }

    //                // 根据节点类型处理特定数据
    //                switch (nodeValue.type)
    //                {
    //                    case "global":
    //                        // 全局节点：保存表单信息到备注
    //                        break;
    //                    case "approver":
    //                        // 审批节点/工序节点：保存审批人列表
    //                        break;
    //                    case "start":
    //                    case "end":
    //                    case "connect":
    //                    default:
    //                        break;
    //                }

    //                stepsToUpdate.Add(existingStep);
    //            }
    //            else
    //            {
    //                // 不存在相同nodeId，新增记录
    //                var stepEntity = new AProdRoutingStepEntity();
    //                stepEntity.F_Id = SnowflakeIdHelper.NextId();
    //                stepEntity.F_RoutingId = id;
    //                stepEntity.F_NodeId = nodeId;
    //                stepEntity.F_NodeName = nodeValue.nodeName;
    //                stepEntity.F_Type = nodeValue.type;
    //                stepEntity.F_SortCode = sortCode++;
    //                stepEntity.F_CreatorTime = DateTime.Now;

    //                // 从formData中设置字段
    //                if (content != null)
    //                {
    //                    if (string.IsNullOrEmpty(content.F_ProcessId))
    //                    {
    //                        throw Oops.Bah(ErrorCode.COM1018, "请填写工序");
    //                    }
    //                    if (string.IsNullOrEmpty(content.F_ResponsibleUserId))
    //                    {
    //                        throw Oops.Bah(ErrorCode.COM1018, "请填写负责人");
    //                    }

    //                    stepEntity.F_BOMId = content.F_BOMId;
    //                    stepEntity.F_ProcessId = content.F_ProcessId;
    //                    stepEntity.F_ResponsibleUserId = content.F_ResponsibleUserId;
    //                    stepEntity.F_WagePrice = content.F_WagePrice;
    //                    stepEntity.F_StdHour = content.F_StdHour;
    //                    stepEntity.F_StdMinute = content.F_StdMinute;
    //                    stepEntity.F_StdSecond = content.F_StdSecond;
    //                    stepEntity.F_PriceType = content.F_PriceType;
    //                    stepEntity.F_UnitRatioProd = content.F_UnitRatio;
    //                    stepEntity.F_UnitRatioBase = content.F_ReportUnit;
    //                    stepEntity.F_IsOutsource = content.F_IsOutsource;
    //                    stepEntity.F_IsQc = content.F_IsQc;
    //                    stepEntity.F_DefectHandle = content.F_DefectHandle;
    //                    stepEntity.F_Files = content.F_Files != null && content.F_Files.Count > 0 ? content.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null;
    //                    stepEntity.F_Description = content.F_Description;

    //                    // 处理工序物料信息(tableField3b6f08)
    //                    if (content.tableField3b6f08 != null && content.tableField3b6f08.Count > 0)
    //                    {
    //                        stepEntity.AProdRoutingStepItemList = content.tableField3b6f08.Adapt<List<AProdRoutingStepItemEntity>>();
    //                        foreach (var item in stepEntity.AProdRoutingStepItemList)
    //                        {
    //                            item.id = SnowflakeIdHelper.NextId();
    //                            item.F_CreatorTime = DateTime.Now;
    //                        }
    //                    }
    //                }

    //                // 根据节点类型处理特定数据
    //                switch (nodeValue.type)
    //                {
    //                    case "global":
    //                        // 全局节点：保存表单信息到备注
    //                        break;
    //                    case "approver":
    //                        // 审批节点/工序节点：保存审批人列表
    //                        break;
    //                    case "start":
    //                    case "end":
    //                    case "connect":
    //                    default:
    //                        // 其他类型节点：保存基本信息即可
    //                        break;
    //                }

    //                stepsToInsert.Add(stepEntity);
    //            }
    //        }
    //    }

    //    // 7. 删除前端未传递的节点（已被删除的节点）及其关联的物料
    //    var stepsToDelete = existingSteps
    //        .Where(it => !processedNodeIds.Contains(it.F_NodeId))
    //        .ToList();

    //    if (stepsToDelete.Count > 0)
    //    {
    //        var stepIdsToDelete = stepsToDelete.Select(it => it.F_Id).ToList();

    //        // 先删除关联的工序物料
    //        await _repositoryStep.AsSugarClient().Deleteable<AProdRoutingStepItemEntity>()
    //            .Where(it => stepIdsToDelete.Contains(it.F_StepId))
    //            .ExecuteCommandAsync();

    //        // 再删除工序节点
    //        await _repositoryStep.AsDeleteable()
    //            .In(it => it.F_Id, stepIdsToDelete)
    //            .ExecuteCommandAsync();
    //    }

    //    // 8. 更新路线信息
    //    await _repository.AsUpdateable(entity)
    //        .UpdateColumns(it => new {
    //            it.FlowId,
    //            it.F_XML,
    //            it.F_RoutingNo,
    //            it.F_RoutingName
    //        })
    //        .ExecuteCommandAsync();

    //    // 9. 批量更新已有的工序节点及其物料
    //    foreach (var step in stepsToUpdate)
    //    {
    //        // 更新工序节点基本信息
    //        await _repositoryStep.AsUpdateable(step)
    //            .UpdateColumns(it => new {
    //                it.F_NodeName,
    //                it.F_Type,
    //                it.F_SortCode,
    //                it.F_Description,
    //                it.F_ProcessId,
    //                it.F_ResponsibleUserId,
    //                it.F_BOMId,
    //                it.F_WagePrice,
    //                it.F_StdHour,
    //                it.F_StdMinute,
    //                it.F_StdSecond,
    //                it.F_PriceType,
    //                it.F_UnitRatioProd,
    //                it.F_UnitRatioBase,
    //                it.F_IsOutsource,
    //                it.F_IsQc,
    //                it.F_DefectHandle,
    //                it.F_Files
    //            })
    //            .ExecuteCommandAsync();

    //        // 处理工序物料：先删除旧的，再插入新的
    //        if (step.AProdRoutingStepItemList != null)
    //        {
    //            // 删除该工序下所有旧的物料
    //            await _repositoryStep.AsSugarClient().Deleteable<AProdRoutingStepItemEntity>()
    //                .Where(it => it.F_StepId == step.F_Id)
    //                .ExecuteCommandAsync();

    //            // 插入新的物料
    //            if (step.AProdRoutingStepItemList.Count > 0)
    //            {
    //                await _repositoryStep.AsSugarClient().Insertable(step.AProdRoutingStepItemList)
    //                    .ExecuteCommandAsync();
    //            }
    //        }
    //    }

    //    // 10. 批量插入新增的工序节点及其物料
    //    foreach (var step in stepsToInsert)
    //    {
    //        // 插入工序节点
    //        await _repositoryStep.AsInsertable(step).ExecuteCommandAsync();

    //        // 插入关联的物料
    //        if (step.AProdRoutingStepItemList != null && step.AProdRoutingStepItemList.Count > 0)
    //        {
    //            foreach (var item in step.AProdRoutingStepItemList)
    //            {
    //                item.F_StepId = step.F_Id;
    //            }
    //            await _repositoryStep.AsSugarClient().Insertable(step.AProdRoutingStepItemList)
    //                .ExecuteCommandAsync();
    //        }
    //    }
    //}

    /// <summary>
    /// 删除a_prod_routing.
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
    /// 批量删除a_prod_routing.
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
            // 批量删除a_prod_routing
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// a_prod_routing详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Includes(it => it.AProdRoutingStepList.Where(it => it.DeleteMark == null))
            .Where(it => it.id == id)
            .Select(it => new AProdRoutingDetailOutput
            {
                id = it.id,
                F_RoutingNo = it.F_RoutingNo,
                F_RoutingName = it.F_RoutingName,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                tableField7b5631 = it.AProdRoutingStepList.Adapt<List<AProdRoutingStepDetailOutput>>(),
            }).ToListAsync();


        await _repository.AsSugarClient().ThenMapperAsync(data.SelectMany(it => it.tableField7b5631), async aProdRoutingStep =>
        {
            var aProdRouting = data.ToList().Find(it => it.id.Equals(aProdRoutingStep.F_RoutingId));
            var linkageParameters = new List<DataInterfaceParameter>();
            // 计价方式
            if(aProdRoutingStep.F_PriceType != null)
            {
                aProdRoutingStep.F_PriceType = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(aProdRoutingStep.F_PriceType) && it.DictionaryTypeId.Equals("2011642610875240448")).Select(it => it.FullName).FirstAsync();
            }

            // 单位关系(单位)
            if(aProdRoutingStep.F_UnitRatioBase != null)
            {
                aProdRoutingStep.F_UnitRatioBase = await _repository.AsSugarClient().Queryable<DictionaryDataEntity>().Where(it => it.EnCode.Equals(aProdRoutingStep.F_UnitRatioBase) && it.DictionaryTypeId.Equals("2008448689420505088")).Select(it => it.FullName).FirstAsync();
            }

            // 工序需外协
            aProdRoutingStep.F_IsOutsource = aProdRoutingStep.F_IsOutsource=="1" ? "开" : "关";

            // 工序需质检
            aProdRoutingStep.F_IsQc = aProdRoutingStep.F_IsQc=="1" ? "开" : "关";

            // 不良品需报废、返工
            aProdRoutingStep.F_DefectHandle = aProdRoutingStep.F_DefectHandle=="1" ? "开" : "关";

            // 生产任务计时
            aProdRoutingStep.F_TaskTimer = aProdRoutingStep.F_TaskTimer=="1" ? "是" : "否";

            // 创建时间
            aProdRoutingStep.F_CreatorTime = aProdRoutingStep.F_CreatorTime?.ParseToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<AProdRoutingEntity>(new AProdRoutingEntity()));
        ConfigModel tableField7b5631Config = new ConfigModel
        {
            tableName = "a_prod_routing_step",
            jnpfKey = JnpfKeyConst.TABLE,
            label = "工序",
            children = ExportImportDataHelper.GetDataConversionTemplateParsing<AProdRoutingStepEntity>(new AProdRoutingStepEntity())
        };
        FieldsModel tableField7b5631 = new FieldsModel()
        {
            __config__ = tableField7b5631Config,
            __vModel__ = "tableField7b5631"
        };
        fieldList.Add(tableField7b5631);

        resData = await _controlParsing.GetParsDataList(resData,"tableField7b5631-F_ProcessId","popupSelect",_userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }


    /// <summary>
    /// 接口获取工艺路线列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("PopupList")]
    public async Task<dynamic> GetPopupList()
    {
        var data = await _repository.AsQueryable()
            .Where(it => it.DeleteMark == null)
             .Select(it => new AProdRoutingListOutput
             {
                 id = it.id,
                 F_RoutingNo = it.F_RoutingNo,
                 F_RoutingName = it.F_RoutingName + "/" + it.F_RoutingNo,
                 F_ProcessNum = SqlFunc.Subqueryable<AProdRoutingStepEntity>().EnableTableFilter().Where(ro => ro.F_RoutingId.Equals(it.id) && ro.F_Type == "approver").Count(),
                 F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
             }).MergeTable().OrderBy(it => it.F_CreatorTime, OrderByType.Desc).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();

        });

        return new {
            data = data
        };
    }



}
