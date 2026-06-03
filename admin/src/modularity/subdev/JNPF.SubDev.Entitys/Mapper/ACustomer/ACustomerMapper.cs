using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.ACustomer;
using JNPF.example.Entitys.Dto.ACtcAddress;
using JNPF.example.Entitys.Dto.ACtcContact;
using Mapster;

namespace JNPF.example.Entitys.Mapper.ACustomer;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ACustomerCrInput, ACustomerEntity>()
			.Map(dest => dest.F_CustomerName, src => src.F_CustomerName != null ? src.F_CustomerName : null)
			.Map(dest => dest.F_CustomerCode, src => src.F_CustomerCode != null ? src.F_CustomerCode : null)
			.Map(dest => dest.F_QRCode, src => src.F_QRCode != null ? src.F_QRCode : null)
			.Map(dest => dest.F_IsPublic, src => src.F_IsPublic != null ? src.F_IsPublic : null)
			.Map(dest => dest.F_CustomerTags, src => src.F_CustomerTags != null ? src.F_CustomerTags : null)
            .Map(dest => dest.F_ShareUsers, src => src.F_ShareUsers != null && src.F_ShareUsers.Count > 0 ? src.F_ShareUsers.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_IsFollow, src => src.F_IsFollow != null ? src.F_IsFollow : null)
			.Map(dest => dest.F_State, src => src.F_State)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null && src.F_Files.Count > 0 ? src.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_RequisitionTime, src => src.F_RequisitionTime != null ? src.F_RequisitionTime : null)
			.Map(dest => dest.F_RequisitionUserId, src => src.F_RequisitionUserId != null ? src.F_RequisitionUserId : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<ACustomerEntity, ACustomerInfoOutput>()
			.Map(dest => dest.F_CustomerName, src => src.F_CustomerName != null ? src.F_CustomerName : null)
			.Map(dest => dest.F_CustomerCode, src => src.F_CustomerCode != null ? src.F_CustomerCode : null)
			.Map(dest => dest.F_QRCode, src => src.F_QRCode != null ? src.F_QRCode : null)
			.Map(dest => dest.F_IsPublic, src => src.F_IsPublic != null ? src.F_IsPublic : null)
			.Map(dest => dest.F_CustomerTags, src => src.F_CustomerTags != null ? src.F_CustomerTags : null)
            .Map(dest => dest.F_ShareUsers, src => src.F_ShareUsers != null ? src.F_ShareUsers.ToObject<List<string>>() : null)
            .Map(dest => dest.F_IsFollow, src => src.F_IsFollow != null ? src.F_IsFollow : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null ? src.F_Files.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_RequisitionTime, src => src.F_RequisitionTime != null ? src.F_RequisitionTime : null)
			.Map(dest => dest.F_RequisitionUserId, src => src.F_RequisitionUserId != null ? src.F_RequisitionUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
            .Map(dest => dest.tableField6eed25, src => src.ACtcAddressList.Adapt<List<ACtcAddressInfoOutput>>())
            .Map(dest => dest.tableFieldddabab, src => src.ACtcContactList.Adapt<List<ACtcContactInfoOutput>>())
		;
		config.ForType<ACustomerEntity, ACustomerDetailOutput>()
            .Map(dest => dest.tableField6eed25, src => src.ACtcAddressList.Adapt<List<ACtcAddressDetailOutput>>())
            .Map(dest => dest.tableFieldddabab, src => src.ACtcContactList.Adapt<List<ACtcContactDetailOutput>>())
		;
	}
}
