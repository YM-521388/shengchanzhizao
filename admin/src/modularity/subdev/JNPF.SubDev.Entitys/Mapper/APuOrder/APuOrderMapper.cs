using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuOrder;
using JNPF.example.Entitys.Dto.APuOrderItem;
using JNPF.example.Entitys.Dto.APuOrderLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.APuOrder;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuOrderCrInput, APuOrderEntity>()
			.Map(dest => dest.F_OrderNo, src => src.F_OrderNo != null ? src.F_OrderNo : null)
			.Map(dest => dest.F_SupplierId, src => src.F_SupplierId != null ? src.F_SupplierId : null)
			.Map(dest => dest.F_ProdPlanId, src => src.F_ProdPlanId != null ? src.F_ProdPlanId : null)
			.Map(dest => dest.F_Money, src => src.F_Money != null ? src.F_Money : null)
			.Map(dest => dest.F_UserId, src => src.F_UserId != null ? src.F_UserId : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_PurchaseNum, src => src.F_PurchaseNum != null ? src.F_PurchaseNum : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuOrderEntity, APuOrderInfoOutput>()
			.Map(dest => dest.F_OrderNo, src => src.F_OrderNo != null ? src.F_OrderNo : null)
			.Map(dest => dest.F_SupplierId, src => src.F_SupplierId != null ? src.F_SupplierId : null)
			.Map(dest => dest.F_ProdPlanId, src => src.F_ProdPlanId != null ? src.F_ProdPlanId : null)
			.Map(dest => dest.F_Money, src => src.F_Money != null ? src.F_Money : null)
			.Map(dest => dest.F_UserId, src => src.F_UserId != null ? src.F_UserId : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_PurchaseNum, src => src.F_PurchaseNum != null ? src.F_PurchaseNum : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableFieldf01abd, src => src.APuOrderItemList.Adapt<List<APuOrderItemInfoOutput>>())
            .Map(dest => dest.tableField8b2a57, src => src.APuOrderLogList.Adapt<List<APuOrderLogInfoOutput>>())
		;
		config.ForType<APuOrderEntity, APuOrderDetailOutput>()
            .Map(dest => dest.tableFieldf01abd, src => src.APuOrderItemList.Adapt<List<APuOrderItemDetailOutput>>())
            .Map(dest => dest.tableField8b2a57, src => src.APuOrderLogList.Adapt<List<APuOrderLogDetailOutput>>())
		;
	}
}
