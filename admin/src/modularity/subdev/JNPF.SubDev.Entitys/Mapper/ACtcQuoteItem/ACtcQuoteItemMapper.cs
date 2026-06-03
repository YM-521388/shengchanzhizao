using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.ACtcQuoteItem;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.ACtcQuoteItem;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ACtcQuoteItemCrInput, ACtcQuoteItemEntity>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_Price, src => src.F_Price != null ? src.F_Price : null)
			.Map(dest => dest.F_SaleQty, src => src.F_SaleQty != null ? src.F_SaleQty : null)
			.Map(dest => dest.F_Discount, src => src.F_Discount != null ? src.F_Discount : null)
			.Map(dest => dest.F_DiscountAmount, src => src.F_DiscountAmount != null ? src.F_DiscountAmount : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null && src.F_Files.Count > 0 ? src.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<ACtcQuoteItemEntity, ACtcQuoteItemListOutput>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_Price, src => src.F_Price != null ? src.F_Price : null)
			.Map(dest => dest.F_SaleQty, src => src.F_SaleQty != null ? src.F_SaleQty : null)
			.Map(dest => dest.F_Discount, src => src.F_Discount != null ? src.F_Discount : null)
			.Map(dest => dest.F_DiscountAmount, src => src.F_DiscountAmount != null ? src.F_DiscountAmount : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null ? src.F_Files : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<ACtcQuoteItemEntity, ACtcQuoteItemInfoOutput>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_Price, src => src.F_Price != null ? src.F_Price : null)
			.Map(dest => dest.F_SaleQty, src => src.F_SaleQty != null ? src.F_SaleQty : null)
			.Map(dest => dest.F_Discount, src => src.F_Discount != null ? src.F_Discount : null)
			.Map(dest => dest.F_DiscountAmount, src => src.F_DiscountAmount != null ? src.F_DiscountAmount : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null ? src.F_Files.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<ACtcQuoteItemEntity, ACtcQuoteItemDetailOutput>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_Price, src => src.F_Price != null ? src.F_Price : null)
			.Map(dest => dest.F_SaleQty, src => src.F_SaleQty != null ? src.F_SaleQty : null)
			.Map(dest => dest.F_Discount, src => src.F_Discount != null ? src.F_Discount : null)
			.Map(dest => dest.F_DiscountAmount, src => src.F_DiscountAmount != null ? src.F_DiscountAmount : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null ? src.F_Files.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
