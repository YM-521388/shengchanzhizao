using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AGoods;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AGoods;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AGoodsCrInput, AGoodsEntity>()
			.Map(dest => dest.F_GoodsNo, src => src.F_GoodsNo != null ? src.F_GoodsNo : null)
			.Map(dest => dest.F_GoodsName, src => src.F_GoodsName != null ? src.F_GoodsName : null)
			.Map(dest => dest.F_CategoryId, src => src.F_CategoryId != null && src.F_CategoryId.Count > 0 ? src.F_CategoryId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_Unit, src => src.F_Unit != null && src.F_Unit.Count > 0 ? src.F_Unit.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_Unit_Ratio, src => src.F_Unit_Ratio != null ? src.F_Unit_Ratio : null)
            .Map(dest => dest.F_SalePrice, src => src.F_SalePrice != null ? src.F_SalePrice : null)
			.Map(dest => dest.F_CostPrice, src => src.F_CostPrice != null ? src.F_CostPrice : null)
			.Map(dest => dest.F_CommissionFixed, src => src.F_CommissionFixed != null ? src.F_CommissionFixed : null)
			.Map(dest => dest.F_Source, src => src.F_Source != null ? src.F_Source : null)
			.Map(dest => dest.F_OutsourcePrice, src => src.F_OutsourcePrice != null ? src.F_OutsourcePrice : null)
			.Map(dest => dest.F_SupplierId, src => src.F_SupplierId != null ? src.F_SupplierId : null)
			.Map(dest => dest.F_Specification, src => src.F_Specification != null ? src.F_Specification : null)
			.Map(dest => dest.F_InspectRule, src => src.F_InspectRule != null ? src.F_InspectRule : null)
			.Map(dest => dest.F_StockUpperLimit, src => src.F_StockUpperLimit != null ? src.F_StockUpperLimit : null)
			.Map(dest => dest.F_StockLowerLimit, src => src.F_StockLowerLimit != null ? src.F_StockLowerLimit : null)
            .Map(dest => dest.F_Encoding, src => src.F_Encoding != null ? src.F_Encoding : null)
            .Map(dest => dest.F_Image, src => src.F_Image != null && src.F_Image.Count > 0 ? src.F_Image.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_AttachmentUrl, src => src.F_AttachmentUrl != null && src.F_AttachmentUrl.Count > 0 ? src.F_AttachmentUrl.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_Remark, src => src.F_Remark != null ? src.F_Remark : null)
			.Map(dest => dest.F_Intro, src => src.F_Intro != null ? src.F_Intro : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
            .Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
        ;
		config.ForType<AGoodsEntity, AGoodsInfoOutput>()
			.Map(dest => dest.F_GoodsNo, src => src.F_GoodsNo != null ? src.F_GoodsNo : null)
			.Map(dest => dest.F_GoodsName, src => src.F_GoodsName != null ? src.F_GoodsName : null)
			.Map(dest => dest.F_CategoryId, src => src.F_CategoryId != null ? src.F_CategoryId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_Unit, src => src.F_Unit != null ? src.F_Unit.ToObject<List<string>>() : null)
            .Map(dest => dest.F_Unit_Ratio, src => src.F_Unit_Ratio != null ? src.F_Unit_Ratio : null)
            .Map(dest => dest.F_SalePrice, src => src.F_SalePrice != null ? src.F_SalePrice : null)
			.Map(dest => dest.F_CostPrice, src => src.F_CostPrice != null ? src.F_CostPrice : null)
			.Map(dest => dest.F_CommissionFixed, src => src.F_CommissionFixed != null ? src.F_CommissionFixed : null)
			.Map(dest => dest.F_Source, src => src.F_Source != null ? src.F_Source : null)
			.Map(dest => dest.F_OutsourcePrice, src => src.F_OutsourcePrice != null ? src.F_OutsourcePrice : null)
			.Map(dest => dest.F_SupplierId, src => src.F_SupplierId != null ? src.F_SupplierId : null)
            .Map(dest => dest.F_Encoding, src => src.F_Encoding != null ? src.F_Encoding : null)
            .Map(dest => dest.F_Specification, src => src.F_Specification != null ? src.F_Specification : null)
			.Map(dest => dest.F_InspectRule, src => src.F_InspectRule != null ? src.F_InspectRule : null)
			.Map(dest => dest.F_StockUpperLimit, src => src.F_StockUpperLimit != null ? src.F_StockUpperLimit : null)
			.Map(dest => dest.F_StockLowerLimit, src => src.F_StockLowerLimit != null ? src.F_StockLowerLimit : null)
			.Map(dest => dest.F_Image, src => src.F_Image != null ? src.F_Image.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_AttachmentUrl, src => src.F_AttachmentUrl != null ? src.F_AttachmentUrl.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_Remark, src => src.F_Remark != null ? src.F_Remark : null)
			.Map(dest => dest.F_Intro, src => src.F_Intro != null ? src.F_Intro : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
            .Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
        ;
	}
}
