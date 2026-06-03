using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.ACtcContract;
using JNPF.example.Entitys.Dto.ACtcContractItem;
using Mapster;

namespace JNPF.example.Entitys.Mapper.ACtcContract;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ACtcContractCrInput, ACtcContractEntity>()
			.Map(dest => dest.F_ContractCode, src => src.F_ContractCode != null ? src.F_ContractCode : null)
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_ContactId, src => src.F_ContactId != null ? src.F_ContactId : null)
			.Map(dest => dest.F_AddressId, src => src.F_AddressId != null ? src.F_AddressId : null)
			.Map(dest => dest.F_PrepayAmount, src => src.F_PrepayAmount != null ? src.F_PrepayAmount : null)
			.Map(dest => dest.F_SaleTotal, src => src.F_SaleTotal != null ? src.F_SaleTotal : null)
			.Map(dest => dest.F_ContractAmount, src => src.F_ContractAmount != null ? src.F_ContractAmount : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_IsTaxable, src => src.F_IsTaxable)
			.Map(dest => dest.F_SalesmanId, src => src.F_SalesmanId != null ? src.F_SalesmanId : null)
			.Map(dest => dest.F_AuditStatus, src => src.F_AuditStatus != null ? src.F_AuditStatus : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<ACtcContractEntity, ACtcContractInfoOutput>()
			.Map(dest => dest.F_ContractCode, src => src.F_ContractCode != null ? src.F_ContractCode : null)
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_ContactId, src => src.F_ContactId != null ? src.F_ContactId : null)
			.Map(dest => dest.F_AddressId, src => src.F_AddressId != null ? src.F_AddressId : null)
			.Map(dest => dest.F_PrepayAmount, src => src.F_PrepayAmount != null ? src.F_PrepayAmount : null)
			.Map(dest => dest.F_SaleTotal, src => src.F_SaleTotal != null ? src.F_SaleTotal : null)
			.Map(dest => dest.F_ContractAmount, src => src.F_ContractAmount != null ? src.F_ContractAmount : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_IsTaxable, src => src.F_IsTaxable != null ? src.F_IsTaxable : null)
			.Map(dest => dest.F_SalesmanId, src => src.F_SalesmanId != null ? src.F_SalesmanId : null)
			.Map(dest => dest.F_AuditStatus, src => src.F_AuditStatus != null ? src.F_AuditStatus : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableFieldf4dfaf, src => src.ACtcContractItemList.Adapt<List<ACtcContractItemInfoOutput>>())
		;
		config.ForType<ACtcContractEntity, ACtcContractDetailOutput>()
            .Map(dest => dest.tableFieldf4dfaf, src => src.ACtcContractItemList.Adapt<List<ACtcContractItemDetailOutput>>())
		;
	}
}
