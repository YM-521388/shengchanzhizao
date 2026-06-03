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
using JNPF.example.Entitys.Dto.AProdProjectStep;
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
/// 业务实现：项目商品工序信息.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AProdProjectStep", Order = 200)]
[Route("api/example/[controller]")]
public class AProdProjectStepService : IAProdProjectStepService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AProdProjectStepEntity> _repository;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"项目商品\",\"field\":\"F_ProjcetItemId\"},{\"value\":\"工序\",\"field\":\"F_ProcessId\"},{\"value\":\"计划开始\",\"field\":\"F_StartDate\"},{\"value\":\"计划结束\",\"field\":\"F_EndDate\"},{\"value\":\"排序\",\"field\":\"F_SortCode\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AProdProjectStepService"/>类型的新实例.
    /// </summary>
    public AProdProjectStepService(
        ISqlSugarRepository<AProdProjectStepEntity> repository,
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
    /// 获取项目商品工序信息.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .FirstAsync(it => it.id.Equals(id))).Adapt<AProdProjectStepInfoOutput>(); 

            return data;
    }

    /// <summary>
    /// 获取项目商品工序信息列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AProdProjectStepListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AProdProjectStepListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProjcetItemId), it => it.F_ProjectItemId.Equals(input.F_ProjcetItemId))
            .WhereIF(!string.IsNullOrEmpty(input.F_ProcessId), it => it.F_ProcessId.Equals(input.F_ProcessId))
            .WhereIF(input.F_StartDate?.Count() > 0, it => SqlFunc.Between(it.F_StartDate, input.F_StartDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_StartDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .WhereIF(input.F_EndDate?.Count() > 0, it => SqlFunc.Between(it.F_EndDate, input.F_EndDate.First().ParseToDateTime("yyyy-MM-dd HH:mm:ss"), input.F_EndDate.Last().ParseToDateTime("yyyy-MM-dd HH:mm:ss")))
            .Where(authorizeWhere)
            .Where(it => it.FlowId==null)
            .Select(it => new AProdProjectStepListOutput
            {
                id = it.id,
                F_ProjectItemId = it.F_ProjectItemId,
                F_ProcessId = it.F_ProcessId,
                F_Description = it.F_Description,
                F_StartDate = it.F_StartDate,
                F_EndDate = it.F_EndDate,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                F_SortCode = it.F_SortCode,
            }).MergeTable().OrderBy(it => it.F_SortCode).OrderByIF(string.IsNullOrEmpty(input.sidx), it => it.F_CreatorTime,OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 项目商品ID
            if(item.F_ProjectItemId != null)
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
                var F_ProjcetItemIdData = await _dataInterfaceService.GetDynamicList("F_ProjcetItemId", "2012085692393459712", "id", "F_GoodsName", "", linkageParameters);
                item.F_ProjectItemId = F_ProjcetItemIdData.Find(it => it.id.Equals(item.F_ProjectItemId))?.fullName;
            }

            // 工序ID
            if(item.F_ProcessId != null)
            {
                var F_ProcessIdData = await _dataInterfaceService.GetDynamicList("F_ProcessId", "2012092092830060544", "id", "F_ProcessName", "");
                item.F_ProcessId = F_ProcessIdData.Find(it => it.id.Equals(item.F_ProcessId))?.fullName;
            }

        });

        return PageResult<AProdProjectStepListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建项目商品工序信息.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AProdProjectStepCrInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdProjectStepEntity>();
        entity.id = SnowflakeIdHelper.NextId();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetTemplateParsing<AProdProjectStepEntity>(new AProdProjectStepEntity()));
        entity.F_CreatorTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now).ParseToDateTime();
        var isOk = await _repository.AsInsertable(entity).IgnoreColumns(ignoreNullColumn: true).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1000);
    }

    /// <summary>
    /// 更新项目商品工序信息.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AProdProjectStepUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AProdProjectStepEntity>();
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_ProjectItemId,
                it.F_ProcessId,
                it.F_StartDate,
                it.F_EndDate,
                it.F_SortCode,
                it.F_Description,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除项目商品工序信息.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).ExecuteCommandAsync();   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除项目商品工序信息.
    /// </summary>
    /// <param name="input">主键数组.</param>
    /// <returns></returns>
    [HttpPost("batchRemove")]
    [UnitOfWork]
    public async Task BatchRemove([FromBody] BatchRemoveInput input)
    {
        var entitys = await _repository.AsQueryable().In(it => it.id, input.ids).ToListAsync();
        if (entitys.Count > 0)
        {
            // 批量删除项目商品工序信息
            await _repository.AsDeleteable().In(it => it.id,input.ids).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 项目商品工序信息详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AProdProjectStepDetailOutput
            {
                id = it.id,
                F_ProjcetItemId = it.F_ProjectItemId,
                F_ProcessId = it.F_ProcessId,
                F_StartDate = it.F_StartDate.Value.ToString("yyyy-MM-dd"),
                F_EndDate = it.F_EndDate.Value.ToString("yyyy-MM-dd"),
                F_SortCode = it.F_SortCode.ToString(),
                F_Description = it.F_Description,
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 项目商品ID
            if(item.F_ProjcetItemId != null)
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
                var F_ProjcetItemIdData = await _dataInterfaceService.GetDynamicList("F_ProjcetItemId", "2012085692393459712", "id", "F_GoodsName", "", linkageParameters);
                item.F_ProjcetItemId = F_ProjcetItemIdData.Find(it => it.id.Equals(item.F_ProjcetItemId))?.fullName;
            }

            // 工序ID
            if(item.F_ProcessId != null)
            {
                var F_ProcessIdData = await _dataInterfaceService.GetDynamicList("F_ProcessId", "2012092092830060544", "id", "F_ProcessName", "");
                item.F_ProcessId = F_ProcessIdData.Find(it => it.id.Equals(item.F_ProcessId))?.fullName;
            }

        });
        return data.FirstOrDefault();
    }
}
