using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuStockPu;
using JNPF.example.Entitys.Dto.APuStockPuItem;
using JNPF.example.Entitys.Dto.APuStockPuLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.APuStockPu;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuStockPuCrInput, APuStockPuEntity>()
			.Map(dest => dest.F_StockInNo, src => src.F_StockInNo != null ? src.F_StockInNo : null)
            //.Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null && src.F_WarehouseId.Count > 0 ? src.F_WarehouseId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_StockInType, src => src.F_StockInType != null ? src.F_StockInType : null)
			.Map(dest => dest.F_StockInDate, src => src.F_StockInDate != null ? src.F_StockInDate : null)
			.Map(dest => dest.F_StockInUserId, src => src.F_StockInUserId != null ? src.F_StockInUserId : null)
            .Map(dest => dest.F_pu_Orderld, src => src.F_pu_Orderld != null ? src.F_pu_Orderld : null)
            .Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
		;
		config.ForType<APuStockPuEntity, APuStockPuInfoOutput>()
			.Map(dest => dest.F_StockInNo, src => src.F_StockInNo != null ? src.F_StockInNo : null)
            //.Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_StockInType, src => src.F_StockInType != null ? src.F_StockInType : null)
			.Map(dest => dest.F_StockInDate, src => src.F_StockInDate != null ? src.F_StockInDate : null)
            .Map(dest => dest.F_pu_Orderld, src => src.F_pu_Orderld != null ? src.F_pu_Orderld : null)
            .Map(dest => dest.F_StockInUserId, src => src.F_StockInUserId != null ? src.F_StockInUserId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
            .Map(dest => dest.tableField4bd139, src => src.APuStockPuItemList.Adapt<List<APuStockPuItemInfoOutput>>())
            .Map(dest => dest.tableFieldca527d, src => src.APuStockPuLogList.Adapt<List<APuStockPuLogInfoOutput>>())
		;
		config.ForType<APuStockPuEntity, APuStockPuDetailOutput>()
            .Map(dest => dest.tableField4bd139, src => src.APuStockPuItemList.Adapt<List<APuStockPuItemDetailOutput>>())
            .Map(dest => dest.tableFieldca527d, src => src.APuStockPuLogList.Adapt<List<APuStockPuLogDetailOutput>>())
		;
	}
}
