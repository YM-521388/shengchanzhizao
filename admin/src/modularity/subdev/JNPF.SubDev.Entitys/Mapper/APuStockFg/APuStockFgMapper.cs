using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuStockFg;
using JNPF.example.Entitys.Dto.APuStockFgItem;
using JNPF.example.Entitys.Dto.APuStockFgLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.APuStockFg;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuStockFgCrInput, APuStockFgEntity>()
			.Map(dest => dest.F_StockInNo, src => src.F_StockInNo != null ? src.F_StockInNo : null)
			.Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
            //.Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null && src.F_WarehouseId.Count > 0 ? src.F_WarehouseId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_StockInType, src => src.F_StockInType != null ? src.F_StockInType : null)
			.Map(dest => dest.F_StockInDate, src => src.F_StockInDate != null ? src.F_StockInDate : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuStockFgEntity, APuStockFgInfoOutput>()
			.Map(dest => dest.F_StockInNo, src => src.F_StockInNo != null ? src.F_StockInNo : null)
			.Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
          //.Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_StockInType, src => src.F_StockInType != null ? src.F_StockInType : null)
			.Map(dest => dest.F_StockInDate, src => src.F_StockInDate != null ? src.F_StockInDate : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableFieldc43f9b, src => src.APuStockFgItemList.Adapt<List<APuStockFgItemInfoOutput>>())
            .Map(dest => dest.tableFieldfcaa70, src => src.APuStockFgLogList.Adapt<List<APuStockFgLogInfoOutput>>())
		;
		config.ForType<APuStockFgEntity, APuStockFgDetailOutput>()
            .Map(dest => dest.tableFieldc43f9b, src => src.APuStockFgItemList.Adapt<List<APuStockFgItemDetailOutput>>())
            .Map(dest => dest.tableFieldfcaa70, src => src.APuStockFgLogList.Adapt<List<APuStockFgLogDetailOutput>>())
		;
	}
}
