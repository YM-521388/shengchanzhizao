using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuOutStockSo;
using JNPF.example.Entitys.Dto.APuOutStockSoItem;
using JNPF.example.Entitys.Dto.APuOutStockSoLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.APuOutStockSo;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuOutStockSoCrInput, APuOutStockSoEntity>()
			.Map(dest => dest.F_StockOutNo, src => src.F_StockOutNo != null ? src.F_StockOutNo : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null && src.F_WarehouseId.Count > 0 ? src.F_WarehouseId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_StockOutType, src => src.F_StockOutType != null ? src.F_StockOutType : null)
			.Map(dest => dest.F_StockOutDate, src => src.F_StockOutDate != null ? src.F_StockOutDate : null)
			.Map(dest => dest.F_StockOutUserId, src => src.F_StockOutUserId != null ? src.F_StockOutUserId : null)
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_ContactId, src => src.F_ContactId != null ? src.F_ContactId : null)
			.Map(dest => dest.F_AddressId, src => src.F_AddressId != null ? src.F_AddressId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuOutStockSoEntity, APuOutStockSoInfoOutput>()
			.Map(dest => dest.F_StockOutNo, src => src.F_StockOutNo != null ? src.F_StockOutNo : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_StockOutType, src => src.F_StockOutType != null ? src.F_StockOutType : null)
			.Map(dest => dest.F_StockOutDate, src => src.F_StockOutDate != null ? src.F_StockOutDate : null)
			.Map(dest => dest.F_StockOutUserId, src => src.F_StockOutUserId != null ? src.F_StockOutUserId : null)
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_ContactId, src => src.F_ContactId != null ? src.F_ContactId : null)
			.Map(dest => dest.F_AddressId, src => src.F_AddressId != null ? src.F_AddressId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableFielde48def, src => src.APuOutStockSoItemList.Adapt<List<APuOutStockSoItemInfoOutput>>())
            .Map(dest => dest.tableField67a4d7, src => src.APuOutStockSoLogList.Adapt<List<APuOutStockSoLogInfoOutput>>())
		;
		config.ForType<APuOutStockSoEntity, APuOutStockSoDetailOutput>()
            .Map(dest => dest.tableFielde48def, src => src.APuOutStockSoItemList.Adapt<List<APuOutStockSoItemDetailOutput>>())
            .Map(dest => dest.tableField67a4d7, src => src.APuOutStockSoLogList.Adapt<List<APuOutStockSoLogDetailOutput>>())
		;
	}
}
