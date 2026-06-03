using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuReturnSo;
using JNPF.example.Entitys.Dto.APuReturnSoItem;
using JNPF.example.Entitys.Dto.APuReturnSoLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.APuReturnSo;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuReturnSoCrInput, APuReturnSoEntity>()
			.Map(dest => dest.F_ReturnInNo, src => src.F_ReturnInNo != null ? src.F_ReturnInNo : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null && src.F_WarehouseId.Count > 0 ? src.F_WarehouseId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_ReturnInType, src => src.F_ReturnInType != null ? src.F_ReturnInType : null)
			.Map(dest => dest.F_ReturnInDate, src => src.F_ReturnInDate != null ? src.F_ReturnInDate : null)
			.Map(dest => dest.F_ContractId, src => src.F_ContractId != null ? src.F_ContractId : null)
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_ContactId, src => src.F_ContactId != null ? src.F_ContactId : null)
			.Map(dest => dest.F_AddressId, src => src.F_AddressId != null ? src.F_AddressId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuReturnSoEntity, APuReturnSoInfoOutput>()
			.Map(dest => dest.F_ReturnInNo, src => src.F_ReturnInNo != null ? src.F_ReturnInNo : null)
            .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_ReturnInType, src => src.F_ReturnInType != null ? src.F_ReturnInType : null)
			.Map(dest => dest.F_ReturnInDate, src => src.F_ReturnInDate != null ? src.F_ReturnInDate : null)
			.Map(dest => dest.F_ContractId, src => src.F_ContractId != null ? src.F_ContractId : null)
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_ContactId, src => src.F_ContactId != null ? src.F_ContactId : null)
			.Map(dest => dest.F_AddressId, src => src.F_AddressId != null ? src.F_AddressId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableFieldcfb049, src => src.APuReturnSoItemList.Adapt<List<APuReturnSoItemInfoOutput>>())
            .Map(dest => dest.tableField0c9c23, src => src.APuReturnSoLogList.Adapt<List<APuReturnSoLogInfoOutput>>())
		;
		config.ForType<APuReturnSoEntity, APuReturnSoDetailOutput>()
            .Map(dest => dest.tableFieldcfb049, src => src.APuReturnSoItemList.Adapt<List<APuReturnSoItemDetailOutput>>())
            .Map(dest => dest.tableField0c9c23, src => src.APuReturnSoLogList.Adapt<List<APuReturnSoLogDetailOutput>>())
		;
	}
}
