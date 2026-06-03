using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuReturnPrd;
using JNPF.example.Entitys.Dto.APuReturnPrdItem;
using JNPF.example.Entitys.Dto.APuReturnPrdLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.APuReturnPrd;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuReturnPrdCrInput, APuReturnPrdEntity>()
			.Map(dest => dest.F_ReturnInNo, src => src.F_ReturnInNo != null ? src.F_ReturnInNo : null)
            //.Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null && src.F_WarehouseId.Count > 0 ? src.F_WarehouseId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_ReturnInType, src => src.F_ReturnInType != null ? src.F_ReturnInType : null)
			.Map(dest => dest.F_ReturnInDate, src => src.F_ReturnInDate != null ? src.F_ReturnInDate : null)
			.Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
			.Map(dest => dest.F_WorkOrderId, src => src.F_WorkOrderId != null ? src.F_WorkOrderId : null)
			.Map(dest => dest.F_Method, src => src.F_Method != null ? src.F_Method : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuReturnPrdEntity, APuReturnPrdInfoOutput>()
			.Map(dest => dest.F_ReturnInNo, src => src.F_ReturnInNo != null ? src.F_ReturnInNo : null)
            //.Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_ReturnInType, src => src.F_ReturnInType != null ? src.F_ReturnInType : null)
			.Map(dest => dest.F_ReturnInDate, src => src.F_ReturnInDate != null ? src.F_ReturnInDate : null)
			.Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
			.Map(dest => dest.F_WorkOrderId, src => src.F_WorkOrderId != null ? src.F_WorkOrderId : null)
			.Map(dest => dest.F_Method, src => src.F_Method != null ? src.F_Method : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableFieldbb1084, src => src.APuReturnPrdItemList.Adapt<List<APuReturnPrdItemInfoOutput>>())
            .Map(dest => dest.tableFieldceb7bd, src => src.APuReturnPrdLogList.Adapt<List<APuReturnPrdLogInfoOutput>>())
		;
		config.ForType<APuReturnPrdEntity, APuReturnPrdDetailOutput>()
            .Map(dest => dest.tableFieldbb1084, src => src.APuReturnPrdItemList.Adapt<List<APuReturnPrdItemDetailOutput>>())
            .Map(dest => dest.tableFieldceb7bd, src => src.APuReturnPrdLogList.Adapt<List<APuReturnPrdLogDetailOutput>>())
		;
	}
}
