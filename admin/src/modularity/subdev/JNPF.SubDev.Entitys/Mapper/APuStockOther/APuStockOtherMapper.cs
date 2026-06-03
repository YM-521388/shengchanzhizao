using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuStockOther;
using JNPF.example.Entitys.Dto.APuStockOtherItem;
using JNPF.example.Entitys.Dto.APuStockOtherLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.APuStockOther;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuStockOtherCrInput, APuStockOtherEntity>()
			.Map(dest => dest.F_StockInNo, src => src.F_StockInNo != null ? src.F_StockInNo : null)
			.Map(dest => dest.F_SupplierId, src => src.F_SupplierId != null ? src.F_SupplierId : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null && src.F_WarehouseId.Count > 0 ? src.F_WarehouseId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_StockInType, src => src.F_StockInType != null ? src.F_StockInType : null)
			.Map(dest => dest.F_StockInDate, src => src.F_StockInDate != null ? src.F_StockInDate : null)
			.Map(dest => dest.F_StockInUserId, src => src.F_StockInUserId != null ? src.F_StockInUserId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuStockOtherEntity, APuStockOtherInfoOutput>()
			.Map(dest => dest.F_StockInNo, src => src.F_StockInNo != null ? src.F_StockInNo : null)
			.Map(dest => dest.F_SupplierId, src => src.F_SupplierId != null ? src.F_SupplierId : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_StockInType, src => src.F_StockInType != null ? src.F_StockInType : null)
			.Map(dest => dest.F_StockInDate, src => src.F_StockInDate != null ? src.F_StockInDate : null)
			.Map(dest => dest.F_StockInUserId, src => src.F_StockInUserId != null ? src.F_StockInUserId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableFieldecf5cb, src => src.APuStockOtherItemList.Adapt<List<APuStockOtherItemInfoOutput>>())
            .Map(dest => dest.tableFielde1e6d9, src => src.APuStockOtherLogList.Adapt<List<APuStockOtherLogInfoOutput>>())
		;
		config.ForType<APuStockOtherEntity, APuStockOtherDetailOutput>()
            .Map(dest => dest.tableFieldecf5cb, src => src.APuStockOtherItemList.Adapt<List<APuStockOtherItemDetailOutput>>())
            .Map(dest => dest.tableFielde1e6d9, src => src.APuStockOtherLogList.Adapt<List<APuStockOtherLogDetailOutput>>())
		;
	}
}
