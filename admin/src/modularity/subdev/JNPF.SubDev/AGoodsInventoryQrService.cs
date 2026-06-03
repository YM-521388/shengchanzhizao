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
using JNPF.VisualDev.Engine;
using JNPF.example.Entitys.Dto.AGoodsInventoryQr;
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
/// 业务实现：库存二维码.
/// </summary>
[ApiDescriptionSettings(Tag = "example", Name = "AGoodsInventoryQr", Order = 200)]
[Route("api/example/[controller]")]
public class AGoodsInventoryQrService : IAGoodsInventoryQrService, IDynamicApiController, ITransient
{
    /// <summary>
    /// 服务基础仓储.
    /// </summary>
    private readonly ISqlSugarRepository<AGoodsInventoryQrEntity> _repository;
    private readonly ISqlSugarRepository<AGoodsInventoryEntity> _repositoryInventory;
    private readonly ISqlSugarRepository<AGoodsEntity> _repositoryGoods;
    private readonly ISqlSugarRepository<AGoodsCategoryEntity> _repositoryCategory;


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
    private readonly List<ParamsModel> paramList = "[{\"value\":\"商品\",\"field\":\"F_GoodsId\"},{\"value\":\"二维码编号\",\"field\":\"F_Code\"},{\"value\":\"备注\",\"field\":\"F_Description\"},{\"value\":\"创建用户\",\"field\":\"F_CreatorUserId\"},{\"value\":\"创建时间\",\"field\":\"F_CreatorTime\"}]".ToList<ParamsModel>();

    /// <summary>
    /// 初始化一个<see cref="AGoodsInventoryQrService"/>类型的新实例.
    /// </summary>
    public AGoodsInventoryQrService(
        ISqlSugarRepository<AGoodsCategoryEntity> repositoryCategory,
        ISqlSugarRepository<AGoodsEntity> repositoryGoods,
        ISqlSugarRepository<AGoodsInventoryEntity> repositoryInventory,
        ISqlSugarRepository<AGoodsInventoryQrEntity> repository,
        IDataInterfaceService dataInterfaceService,
        ExportImportDataHelper exportImportDataHelper,
        IFileManager fileManager,
        ICacheManager cacheManager,
        ControlParsing controlParsing,
        IUserManager userManager)
    {
        _repositoryCategory = repositoryCategory;
        _repositoryGoods = repositoryGoods;
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
    /// 获取库存二维码.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<dynamic> GetInfo(string id)
    {
        var data = (await _repository.AsQueryable()
            .Select(it => new AGoodsInventoryQrEntity
            {
                id = it.id,
                F_GoodsId = it.F_GoodsId,
                F_Code = it.F_Code,
                F_CreatorOrganizeId = it.F_CreatorOrganizeId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime,
            })
            .FirstAsync(it => it.id.Equals(id))).Adapt<AGoodsInventoryQrInfoOutput>(); 

            if (data!=null && data.F_CreatorOrganizeId.IsNullOrEmpty()) data.F_CreatorOrganizeId = " ";
            else data.F_CreatorOrganizeId = _repository.AsSugarClient().Queryable<OrganizeEntity>().Where(it => data.F_CreatorOrganizeId.ToObject<List<string>>().LastOrDefault().Equals(it.Id)).Select(it => it.FullName).First();
            return data;
    }

    /// <summary>
    /// 获取库存二维码列表.
    /// </summary>
    /// <param name="input">请求参数.</param>
    /// <returns></returns>
    [HttpPost("List")]
    public async Task<dynamic> GetList([FromBody] AGoodsInventoryQrListQueryInput input)
    {
        var authorizeWhere = new List<IConditionalModel>();

        // 数据权限过滤
        if (_userManager.User.IsAdministrator == 0)
        {
            authorizeWhere = await _userManager.GetConditionAsync<AGoodsInventoryQrListOutput>(input.menuId, "F_Id", _userManager.UserOrigin.Equals("pc") ? true : false, 1);
        }

        var selectIds = input.selectIds?.Split(",").ToList();
        var data = await _repository.AsQueryable()
            .WhereIF(selectIds!=null && selectIds.Any() && input.dataType.Equals(2), it => selectIds.Contains(it.id))
            .WhereIF(!string.IsNullOrEmpty(input.F_Code), it => it.F_Code.Contains(input.F_Code))
            .Where(authorizeWhere)
            .Where(it => it.DeleteMark == null)
            .Where(it => it.FlowId==null)
            .Select(it => new AGoodsInventoryQrListOutput
            {
                id = it.id,
                F_GoodsId = SqlFunc.Subqueryable<AGoodsEntity>().EnableTableFilter().Where(g => g.id == it.F_GoodsId && g.DeleteMark == null).Select(g => SqlFunc.MergeString(g.F_GoodsName, "-", g.F_GoodsNo)),
                F_Code = it.F_Code,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().OrderBy(it => it.F_CreatorTime,OrderByType.Desc).OrderBy(it => it.F_Code, OrderByType.Desc).OrderByIF(!string.IsNullOrEmpty(input.sidx), input.sidx + " " + input.sort).ToPagedListAsync(input.currentPage, input.pageSize);

        await _repository.AsSugarClient().ThenMapperAsync(data.list, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
        });

        var resData = data.list.ToObject<List<Dictionary<string, object>>>(CommonConst.options);

        data.list = resData.ToObject<List<AGoodsInventoryQrListOutput>>(CommonConst.options);

        return PageResult<AGoodsInventoryQrListOutput>.SqlSugarPageResult(data);
    }

    /// <summary>
    /// 新建库存二维码.
    /// </summary>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task Create([FromBody] AGoodsInventoryQrCrInput input)
    {
        // 查询商品编号
        var goodsEntity = await _repositoryGoods.AsQueryable()
            .Where(it => it.id.Equals(input.F_GoodsId))
            .FirstAsync();
        if (!string.IsNullOrEmpty(goodsEntity.F_CategoryId))
        {
            var categoryList = goodsEntity.F_CategoryId.ToObject<List<string>>();
            var code = "";
            foreach (var categoryId in categoryList)
            {
                if (code == "")
                {
                    code = (await _repositoryCategory.GetFirstAsync(it => it.id == categoryId)).F_Code;
                }
                else
                {
                    code = code + (await _repositoryCategory.GetFirstAsync(it => it.id == categoryId)).F_Code;
                }
                if (await _repositoryGoods.IsAnyAsync(it => it.F_CategoryId == categoryId && it.DeleteMark == null))
                {
                    goodsEntity.F_CategoryId = string.Join(",", goodsEntity.F_CategoryId);
                    break;
                }
            }
            goodsEntity.F_GoodsNo = code + goodsEntity.F_GoodsNo;

            if (!string.IsNullOrEmpty(goodsEntity.F_GoodsNo))
            {
                // 查询数据库中已有相同前缀的编号
                var existingNos1 = await _repository.AsQueryable()
                    .Where(it => it.F_Code != null && it.F_Code.StartsWith(goodsEntity.F_GoodsNo) && it.DeleteMark == null)
                    .Select(it => it.F_Code)
                    .ToListAsync();

                var existingNos2 = await _repositoryInventory.AsQueryable()
                    .Where(it => it.F_Code != null && it.F_Code.StartsWith(goodsEntity.F_GoodsNo) && it.DeleteMark == null)
                    .Select(it => it.F_Code)
                    .ToListAsync();

                int maxSeq1 = 0, maxSeq2 = 0;
                
                foreach (var no in existingNos1)
                {
                    if (no.Length > goodsEntity.F_GoodsNo.Length)
                    {
                        var suffix = no.Substring(goodsEntity.F_GoodsNo.Length);
                        if (int.TryParse(suffix, out int seq))
                        {
                            if (seq > maxSeq1) maxSeq1 = seq;
                        }
                    }
                }
                
                foreach (var no in existingNos2)
                {
                    if (no.Length > goodsEntity.F_GoodsNo.Length)
                    {
                        var suffix = no.Substring(goodsEntity.F_GoodsNo.Length);
                        if (int.TryParse(suffix, out int seq))
                        {
                            if (seq > maxSeq2) maxSeq2 = seq;
                        }
                    }
                }
                
                int maxSeq = Math.Max(maxSeq1, maxSeq2);
                var nextSeq = maxSeq + 1;
                // 构造带连续编号的实体列表
                var list = Enumerable.Range(0, input.F_Num.Value)
                    .Select(offset =>
                    {
                        var entity = input.Adapt<AGoodsInventoryQrEntity>();
                        entity.id = SnowflakeIdHelper.NextId();
                        entity.F_CreatorUserId = _userManager.UserId;
                        entity.F_CreatorTime = DateTime.Now;
                        entity.F_CreatorOrganizeId = _userManager.User.OrganizeId;
                        entity.F_Code = goodsEntity.F_GoodsNo + (nextSeq + offset);
                        return entity;
                    }).ToList();

                // 3. 一次性插入
                var rows = await _repository.AsInsertable(list).ExecuteCommandAsync();
                if (rows != input.F_Num) throw Oops.Oh(ErrorCode.COM1000);
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
    /// 更新库存二维码.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <param name="input">参数.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task Update(string id, [FromBody] AGoodsInventoryQrUpInput input)
    {
        input = CodeGenHelper.SetEmptyStringNull(input);
        var entity = input.Adapt<AGoodsInventoryQrEntity>();
        var isOk =0;
        isOk = await _repository.AsUpdateable(entity)
            .UpdateColumns(it => new
            {
                it.F_GoodsId,
                it.F_Code,
                it.F_Description,
            }).ExecuteCommandAsync();
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1001);
    }

    /// <summary>
    /// 删除库存二维码.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var isOk = await _repository.AsDeleteable().Where(it => it.id.Equals(id)).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);   
        if (!(isOk > 0)) throw Oops.Oh(ErrorCode.COM1002);
    }

    /// <summary>
    /// 批量删除库存二维码.
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
            // 批量删除库存二维码
            await _repository.AsDeleteable().In(it => it.id,input.ids).Where(it => it.DeleteMark == null).IsLogic().ExecuteCommandAsync("F_Delete_Mark",1, "F_DELETE_TIME", "F_DELETE_USER_ID", _userManager.UserId);
        }
    }

    /// <summary>
    /// 库存二维码详情.
    /// </summary>
    /// <param name="id">主键值.</param>
    /// <returns></returns>
    [HttpGet("Detail/{id}")]
    [UnifySerializerSetting("special")]
    public async Task<dynamic> GetDetails(string id)
    {
        var data = await _repository.AsQueryable()
            .Select(it => new AGoodsInventoryQrDetailOutput
            {
                id = it.id,
                F_GoodsId = it.F_GoodsId,
                F_Code = it.F_Code,
                F_CreatorOrganizeId = it.F_CreatorOrganizeId,
                F_Description = it.F_Description,
                F_CreatorUserId = SqlFunc.Subqueryable<UserEntity>().EnableTableFilter().Where(u => u.Id.Equals(it.F_CreatorUserId)).Select(u => SqlFunc.MergeString(u.RealName, "/", u.Account)),
                F_CreatorTime = it.F_CreatorTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            }).MergeTable().Where(it => it.id == id).ToListAsync();

        await _repository.AsSugarClient().ThenMapperAsync(data, async item =>
        {
            var linkageParameters = new List<DataInterfaceParameter>();
            // 创建组织
            item.F_CreatorOrganizeId = _controlParsing.GetCurrOrganizeName("last",item.F_CreatorOrganizeId);
        });

        var resData = data.ToJsonStringOld().ToObjectOld<List<Dictionary<string, object>>>();
        List<FieldsModel> fieldList = new List<FieldsModel>();
        fieldList.AddRange(ExportImportDataHelper.GetDataConversionTemplateParsing<AGoodsInventoryQrEntity>(new AGoodsInventoryQrEntity()));
        resData = await _controlParsing.GetParsDataList(resData, "F_GoodsId", "popupSelect", _userManager.TenantId, fieldList);

        return resData.FirstOrDefault();
    }

    /// <summary>
    /// 获取商品的最大序号.
    /// </summary>
    /// <param name="input">商品ID列表.</param>
    /// <returns>每个商品的最大序号.</returns>
    [HttpPost("GetMaxSeq")]
    public async Task<List<AGoodsInventoryQrMaxSeqOutput>> GetMaxSeq([FromBody] AGoodsInventoryQrMaxSeqInput input)
    {
        var result = new List<AGoodsInventoryQrMaxSeqOutput>();

        if (input.goodsIds == null || !input.goodsIds.Any())
        {
            return result;
        }

        // 查询所有商品信息
        var goodsList = await _repositoryGoods.AsQueryable()
            .Where(it => input.goodsIds.Contains(it.id) && it.DeleteMark == null)
            .ToListAsync();

        foreach (var goods in goodsList)
        {
            var output = new AGoodsInventoryQrMaxSeqOutput
            {
                F_GoodsId = goods.id,
                MaxSeq = 0,
                GoodsNoPrefix = ""
            };

            // 组装商品编码前缀
            if (!string.IsNullOrEmpty(goods.F_CategoryId))
            {
                var categoryList = goods.F_CategoryId.ToObject<List<string>>();
                var code = "";
                foreach (var categoryId in categoryList)
                {
                    var category = await _repositoryCategory.GetFirstAsync(it => it.id == categoryId);
                    if (category != null)
                    {
                        code += category.F_Code;
                    }
                }
                output.GoodsNoPrefix = code + goods.F_GoodsNo;
            }

            // 如果编码前缀为空，跳过
            if (string.IsNullOrEmpty(output.GoodsNoPrefix))
            {
                result.Add(output);
                continue;
            }

            // 查询库存二维码表中的最大序号
            var existingNos1 = await _repository.AsQueryable()
                .Where(it => it.F_Code != null && it.F_Code.StartsWith(output.GoodsNoPrefix) && it.DeleteMark == null)
                .Select(it => it.F_Code)
                .ToListAsync();

            // 查询库存表中的最大序号
            var existingNos2 = await _repositoryInventory.AsQueryable()
                .Where(it => it.F_Code != null && it.F_Code.StartsWith(output.GoodsNoPrefix) && it.DeleteMark == null)
                .Select(it => it.F_Code)
                .ToListAsync();

            int maxSeq1 = 0;
            int maxSeq2 = 0;

            // 解析二维码表中的序号
            foreach (var no in existingNos1)
            {
                if (no.Length > output.GoodsNoPrefix.Length)
                {
                    var suffix = no.Substring(output.GoodsNoPrefix.Length);
                    if (int.TryParse(suffix, out int seq))
                    {
                        if (seq > maxSeq1) maxSeq1 = seq;
                    }
                }
            }

            // 解析库存表中的序号
            foreach (var no in existingNos2)
            {
                if (no.Length > output.GoodsNoPrefix.Length)
                {
                    var suffix = no.Substring(output.GoodsNoPrefix.Length);
                    if (int.TryParse(suffix, out int seq))
                    {
                        if (seq > maxSeq2) maxSeq2 = seq;
                    }
                }
            }

            output.MaxSeq = Math.Max(maxSeq1, maxSeq2) + 1;
            result.Add(output);
        }

        return result;
    }

    /// <summary>
    /// 获取商品的最大序号（单个商品ID）.
    /// </summary>
    /// <param name="input">商品ID列表.</param>
    /// <returns>每个商品的最大序号.</returns>
    [HttpPost("MaxCode/{goodsId}")]
    public async Task<dynamic> GetMaxCode(string goodsId)
    {
        // 查询商品编号
        var goodsEntity = await _repositoryGoods.AsQueryable()
            .Where(it => it.id.Equals(goodsId))
            .FirstAsync();
        if (!string.IsNullOrEmpty(goodsEntity.F_CategoryId))
        {
            var categoryList = goodsEntity.F_CategoryId.ToObject<List<string>>();
            var code = "";
            foreach (var categoryId in categoryList)
            {
                if (code == "")
                {
                    code = (await _repositoryCategory.GetFirstAsync(it => it.id == categoryId)).F_Code;
                }
                else
                {
                    code = code + (await _repositoryCategory.GetFirstAsync(it => it.id == categoryId)).F_Code;
                }
                if (await _repositoryGoods.IsAnyAsync(it => it.F_CategoryId == categoryId && it.DeleteMark == null))
                {
                    goodsEntity.F_CategoryId = string.Join(",", goodsEntity.F_CategoryId);
                    break;
                }
            }
            goodsEntity.F_GoodsNo = code + goodsEntity.F_GoodsNo;

            if (!string.IsNullOrEmpty(goodsEntity.F_GoodsNo))
            {
                // 查询数据库中已有相同前缀的编号
                var existingNos1 = await _repository.AsQueryable()
                    .Where(it => it.F_Code != null && it.F_Code.StartsWith(goodsEntity.F_GoodsNo) && it.DeleteMark == null)
                    .Select(it => it.F_Code)
                    .ToListAsync();

                var existingNos2 = await _repositoryInventory.AsQueryable()
                    .Where(it => it.F_Code != null && it.F_Code.StartsWith(goodsEntity.F_GoodsNo) && it.DeleteMark == null)
                    .Select(it => it.F_Code)
                    .ToListAsync();

                int maxSeq1 = 1, maxSeq2 = 1;

                foreach (var no in existingNos1)
                {
                    if (no.Length > goodsEntity.F_GoodsNo.Length)
                    {
                        var suffix = no.Substring(goodsEntity.F_GoodsNo.Length);
                        if (int.TryParse(suffix, out int seq))
                        {
                            if (seq > maxSeq1) maxSeq1 = seq;
                        }
                    }
                }

                foreach (var no in existingNos2)
                {
                    if (no.Length > goodsEntity.F_GoodsNo.Length)
                    {
                        var suffix = no.Substring(goodsEntity.F_GoodsNo.Length);
                        if (int.TryParse(suffix, out int seq))
                        {
                            if (seq > maxSeq2) maxSeq2 = seq;
                        }
                    }
                }

                int maxSeq = Math.Max(maxSeq1, maxSeq2);
                return maxSeq + 1;
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
    /// 获取商品最大序号输入参数.
    /// </summary>
    public class AGoodsInventoryQrMaxSeqInput
{
    /// <summary>
    /// 商品ID列表.
///     /// </summary>
    public List<string> goodsIds { get; set; }
}



/// <summary>
/// 商品最大序号输出参数.
/// </summary>
public class AGoodsInventoryQrMaxSeqOutput
{
    /// <summary>
    /// 商品ID.
/// </summary>
    public string F_GoodsId { get; set; }

    /// <summary>
    /// 最大序号.
/// </summary>
    public int MaxSeq { get; set; }

    /// <summary>
    /// 商品编码前缀.
/// </summary>
    public string GoodsNoPrefix { get; set; }
}



}
