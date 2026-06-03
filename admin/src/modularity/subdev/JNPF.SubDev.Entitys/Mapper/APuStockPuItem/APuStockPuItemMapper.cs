using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuStockPuItem;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.APuStockPuItem;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuStockPuItemCrInput, APuStockPuItemEntity>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_Price, src => src.F_Price != null ? src.F_Price : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
            .Map(dest => dest.F_WarehouseID, src => src.F_WarehouseID != null && src.F_WarehouseID.Count > 0 ? src.F_WarehouseID.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_Encoding, src => src.F_Encoding != null ? src.F_Encoding : null)
        ;
		config.ForType<APuStockPuItemEntity, APuStockPuItemListOutput>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_Price, src => src.F_Price != null ? src.F_Price : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<APuStockPuItemEntity, APuStockPuItemInfoOutput>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_Price, src => src.F_Price != null ? src.F_Price : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
            .Map(dest => dest.F_WarehouseID, src => src.F_WarehouseID != null ? src.F_WarehouseID.ToObject<List<string>>() : null)
            .Map(dest => dest.F_Encoding, src => src.F_Encoding != null ? src.F_Encoding : null)
        ;
		config.ForType<APuStockPuItemEntity, APuStockPuItemDetailOutput>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_Price, src => src.F_Price != null ? src.F_Price : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
            .Map(dest => dest.F_WarehouseID, src => src.F_WarehouseID != null ? src.F_WarehouseID : null)
            .Map(dest => dest.F_Encoding, src => src.F_Encoding != null ? src.F_Encoding : null)
        ;
	}
}
