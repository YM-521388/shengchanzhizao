using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuOutReturnPu;
using JNPF.example.Entitys.Dto.APuOutReturnPuItem;
using JNPF.example.Entitys.Dto.APuOutReturnPuLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.APuOutReturnPu;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuOutReturnPuCrInput, APuOutReturnPuEntity>()
			.Map(dest => dest.F_ReturnOutNo, src => src.F_ReturnOutNo != null ? src.F_ReturnOutNo : null)
             .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null && src.F_WarehouseId.Count > 0 ? src.F_WarehouseId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_ReturnOutType, src => src.F_ReturnOutType != null ? src.F_ReturnOutType : null)
			.Map(dest => dest.F_ReturnOutDate, src => src.F_ReturnOutDate != null ? src.F_ReturnOutDate : null)
			.Map(dest => dest.F_OrderId, src => src.F_OrderId != null ? src.F_OrderId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuOutReturnPuEntity, APuOutReturnPuInfoOutput>()
			.Map(dest => dest.F_ReturnOutNo, src => src.F_ReturnOutNo != null ? src.F_ReturnOutNo : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_ReturnOutType, src => src.F_ReturnOutType != null ? src.F_ReturnOutType : null)
			.Map(dest => dest.F_ReturnOutDate, src => src.F_ReturnOutDate != null ? src.F_ReturnOutDate : null)
			.Map(dest => dest.F_OrderId, src => src.F_OrderId != null ? src.F_OrderId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableFieldf5c8ef, src => src.APuOutReturnPuItemList.Adapt<List<APuOutReturnPuItemInfoOutput>>())
            .Map(dest => dest.tableField3ff267, src => src.APuOutReturnPuLogList.Adapt<List<APuOutReturnPuLogInfoOutput>>())
		;
		config.ForType<APuOutReturnPuEntity, APuOutReturnPuDetailOutput>()
            .Map(dest => dest.tableFieldf5c8ef, src => src.APuOutReturnPuItemList.Adapt<List<APuOutReturnPuItemDetailOutput>>())
            .Map(dest => dest.tableField3ff267, src => src.APuOutReturnPuLogList.Adapt<List<APuOutReturnPuLogDetailOutput>>())
		;
	}
}
