using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.ACtcQuote;
using JNPF.example.Entitys.Dto.ACtcQuoteItem;
using Mapster;

namespace JNPF.example.Entitys.Mapper.ACtcQuote;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ACtcQuoteCrInput, ACtcQuoteEntity>()
			.Map(dest => dest.F_QuoteCode, src => src.F_QuoteCode != null ? src.F_QuoteCode : null)
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_ContactId, src => src.F_ContactId != null ? src.F_ContactId : null)
			.Map(dest => dest.F_AddressId, src => src.F_AddressId != null ? src.F_AddressId : null)
			.Map(dest => dest.F_QuoteAmount, src => src.F_QuoteAmount != null ? src.F_QuoteAmount : null)
			.Map(dest => dest.F_GoodsTotal, src => src.F_GoodsTotal != null ? src.F_GoodsTotal : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_QuoteDate, src => src.F_QuoteDate != null ? src.F_QuoteDate : null)
			.Map(dest => dest.F_SalesmanId, src => src.F_SalesmanId != null ? src.F_SalesmanId : null)
			.Map(dest => dest.F_QuoteStatus, src => src.F_QuoteStatus != null ? src.F_QuoteStatus : null)
			.Map(dest => dest.F_AuditStatus, src => src.F_AuditStatus != null ? src.F_AuditStatus : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<ACtcQuoteEntity, ACtcQuoteInfoOutput>()
			.Map(dest => dest.F_QuoteCode, src => src.F_QuoteCode != null ? src.F_QuoteCode : null)
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_ContactId, src => src.F_ContactId != null ? src.F_ContactId : null)
			.Map(dest => dest.F_AddressId, src => src.F_AddressId != null ? src.F_AddressId : null)
			.Map(dest => dest.F_QuoteAmount, src => src.F_QuoteAmount != null ? src.F_QuoteAmount : null)
			.Map(dest => dest.F_GoodsTotal, src => src.F_GoodsTotal != null ? src.F_GoodsTotal : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_QuoteDate, src => src.F_QuoteDate != null ? src.F_QuoteDate : null)
			.Map(dest => dest.F_SalesmanId, src => src.F_SalesmanId != null ? src.F_SalesmanId : null)
			.Map(dest => dest.F_QuoteStatus, src => src.F_QuoteStatus != null ? src.F_QuoteStatus : null)
			.Map(dest => dest.F_AuditStatus, src => src.F_AuditStatus != null ? src.F_AuditStatus : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableFielddc29f7, src => src.ACtcQuoteItemList.Adapt<List<ACtcQuoteItemInfoOutput>>())
		;
		config.ForType<ACtcQuoteEntity, ACtcQuoteDetailOutput>()
            .Map(dest => dest.tableFielddc29f7, src => src.ACtcQuoteItemList.Adapt<List<ACtcQuoteItemDetailOutput>>())
		;
	}
}
