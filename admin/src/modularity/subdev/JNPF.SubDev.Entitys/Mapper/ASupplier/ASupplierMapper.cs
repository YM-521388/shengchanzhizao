using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.ASupplier;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.ASupplier;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ASupplierCrInput, ASupplierEntity>()
			.Map(dest => dest.F_SupplierName, src => src.F_SupplierName != null ? src.F_SupplierName : null)
			.Map(dest => dest.F_SupplierNo, src => src.F_SupplierNo != null ? src.F_SupplierNo : null)
			.Map(dest => dest.F_ContactPerson, src => src.F_ContactPerson != null ? src.F_ContactPerson : null)
			.Map(dest => dest.F_ContactPhone, src => src.F_ContactPhone != null ? src.F_ContactPhone : null)
			.Map(dest => dest.F_Region, src => src.F_Region != null && src.F_Region.Count > 0 ? src.F_Region.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_DetailAddress, src => src.F_DetailAddress != null ? src.F_DetailAddress : null)
			.Map(dest => dest.F_SupplierTags, src => src.F_SupplierTags != null && src.F_SupplierTags.Count > 0 ? src.F_SupplierTags.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_AttachmentUrls, src => src.F_AttachmentUrls != null && src.F_AttachmentUrls.Count > 0 ? src.F_AttachmentUrls.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_StartUsing, src => src.F_StartUsing)
		;
		config.ForType<ASupplierEntity, ASupplierInfoOutput>()
			.Map(dest => dest.F_SupplierName, src => src.F_SupplierName != null ? src.F_SupplierName : null)
			.Map(dest => dest.F_SupplierNo, src => src.F_SupplierNo != null ? src.F_SupplierNo : null)
			.Map(dest => dest.F_ContactPerson, src => src.F_ContactPerson != null ? src.F_ContactPerson : null)
			.Map(dest => dest.F_ContactPhone, src => src.F_ContactPhone != null ? src.F_ContactPhone : null)
			.Map(dest => dest.F_Region, src => src.F_Region != null ? src.F_Region.ToObject<List<string>>() : null)
			.Map(dest => dest.F_DetailAddress, src => src.F_DetailAddress != null ? src.F_DetailAddress : null)
			.Map(dest => dest.F_SupplierTags, src => src.F_SupplierTags != null ? src.F_SupplierTags.ToObject<List<string>>() : null)
			.Map(dest => dest.F_AttachmentUrls, src => src.F_AttachmentUrls != null ? src.F_AttachmentUrls.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
			.Map(dest => dest.F_StartUsing, src => src.F_StartUsing != null ? src.F_StartUsing : null)
		;
	}
}
