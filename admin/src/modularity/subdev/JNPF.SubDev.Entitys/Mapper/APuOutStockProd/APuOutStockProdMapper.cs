using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuOutStockProd;
using JNPF.example.Entitys.Dto.APuOutStockProdItem;
using JNPF.example.Entitys.Dto.APuOutStockProdLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.APuOutStockProd;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuOutStockProdCrInput, APuOutStockProdEntity>()
			.Map(dest => dest.F_StockOutNo, src => src.F_StockOutNo != null ? src.F_StockOutNo : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null && src.F_WarehouseId.Count > 0 ? src.F_WarehouseId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_StockOutType, src => src.F_StockOutType != null ? src.F_StockOutType : null)
			.Map(dest => dest.F_StockOutDate, src => src.F_StockOutDate != null ? src.F_StockOutDate : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
			.Map(dest => dest.F_Method, src => src.F_Method != null ? src.F_Method : null)
			.Map(dest => dest.F_WorkOrderId, src => src.F_WorkOrderId != null ? src.F_WorkOrderId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuOutStockProdEntity, APuOutStockProdInfoOutput>()
			.Map(dest => dest.F_StockOutNo, src => src.F_StockOutNo != null ? src.F_StockOutNo : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_StockOutType, src => src.F_StockOutType != null ? src.F_StockOutType : null)
			.Map(dest => dest.F_StockOutDate, src => src.F_StockOutDate != null ? src.F_StockOutDate : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
			.Map(dest => dest.F_Method, src => src.F_Method != null ? src.F_Method : null)
			.Map(dest => dest.F_WorkOrderId, src => src.F_WorkOrderId != null ? src.F_WorkOrderId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableField2752cf, src => src.APuOutStockProdItemList.Adapt<List<APuOutStockProdItemInfoOutput>>())
            .Map(dest => dest.tableField49228c, src => src.APuOutStockProdLogList.Adapt<List<APuOutStockProdLogInfoOutput>>())
		;
		config.ForType<APuOutStockProdEntity, APuOutStockProdDetailOutput>()
            .Map(dest => dest.tableField2752cf, src => src.APuOutStockProdItemList.Adapt<List<APuOutStockProdItemDetailOutput>>())
            .Map(dest => dest.tableField49228c, src => src.APuOutStockProdLogList.Adapt<List<APuOutStockProdLogDetailOutput>>())
		;
	}
}
