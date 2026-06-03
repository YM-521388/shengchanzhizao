using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuOutStockOther;
using JNPF.example.Entitys.Dto.APuOutStockOtherItem;
using JNPF.example.Entitys.Dto.APuOutStockOtherLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.APuOutStockOther;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuOutStockOtherCrInput, APuOutStockOtherEntity>()
			.Map(dest => dest.F_StockOutNo, src => src.F_StockOutNo != null ? src.F_StockOutNo : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null && src.F_WarehouseId.Count > 0 ? src.F_WarehouseId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_StockOutType, src => src.F_StockOutType != null ? src.F_StockOutType : null)
			.Map(dest => dest.F_StockOutDate, src => src.F_StockOutDate != null ? src.F_StockOutDate : null)
			.Map(dest => dest.F_StockOutUserId, src => src.F_StockOutUserId != null ? src.F_StockOutUserId : null)
			.Map(dest => dest.F_ProcessNo, src => src.F_ProcessNo != null ? src.F_ProcessNo : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuOutStockOtherEntity, APuOutStockOtherInfoOutput>()
			.Map(dest => dest.F_StockOutNo, src => src.F_StockOutNo != null ? src.F_StockOutNo : null)
                .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_StockOutType, src => src.F_StockOutType != null ? src.F_StockOutType : null)
			.Map(dest => dest.F_StockOutDate, src => src.F_StockOutDate != null ? src.F_StockOutDate : null)
			.Map(dest => dest.F_StockOutUserId, src => src.F_StockOutUserId != null ? src.F_StockOutUserId : null)
			.Map(dest => dest.F_ProcessNo, src => src.F_ProcessNo != null ? src.F_ProcessNo : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableField44e07d, src => src.APuOutStockOtherItemList.Adapt<List<APuOutStockOtherItemInfoOutput>>())
            .Map(dest => dest.tableField211772, src => src.APuOutStockOtherLogList.Adapt<List<APuOutStockOtherLogInfoOutput>>())
		;
		config.ForType<APuOutStockOtherEntity, APuOutStockOtherDetailOutput>()
            .Map(dest => dest.tableField44e07d, src => src.APuOutStockOtherItemList.Adapt<List<APuOutStockOtherItemDetailOutput>>())
            .Map(dest => dest.tableField211772, src => src.APuOutStockOtherLogList.Adapt<List<APuOutStockOtherLogDetailOutput>>())
		;
	}
}
