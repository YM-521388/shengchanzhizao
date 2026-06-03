using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.ACtcContact;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.ACtcContact;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ACtcContactCrInput, ACtcContactEntity>()
			.Map(dest => dest.F_IsDefault, src => src.F_IsDefault)
			.Map(dest => dest.F_ContactName, src => src.F_ContactName != null ? src.F_ContactName : null)
			.Map(dest => dest.F_ContactPhone, src => src.F_ContactPhone != null ? src.F_ContactPhone : null)
		;
		config.ForType<ACtcContactEntity, ACtcContactListOutput>()
			.Map(dest => dest.F_IsDefault, src => src.F_IsDefault != null ? src.F_IsDefault : null)
			.Map(dest => dest.F_ContactName, src => src.F_ContactName != null ? src.F_ContactName : null)
			.Map(dest => dest.F_ContactPhone, src => src.F_ContactPhone != null ? src.F_ContactPhone : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<ACtcContactEntity, ACtcContactInfoOutput>()
			.Map(dest => dest.F_IsDefault, src => src.F_IsDefault != null ? src.F_IsDefault : null)
			.Map(dest => dest.F_ContactName, src => src.F_ContactName != null ? src.F_ContactName : null)
			.Map(dest => dest.F_ContactPhone, src => src.F_ContactPhone != null ? src.F_ContactPhone : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<ACtcContactEntity, ACtcContactDetailOutput>()
			.Map(dest => dest.F_IsDefault, src => src.F_IsDefault != null ? src.F_IsDefault : null)
			.Map(dest => dest.F_ContactName, src => src.F_ContactName != null ? src.F_ContactName : null)
			.Map(dest => dest.F_ContactPhone, src => src.F_ContactPhone != null ? src.F_ContactPhone : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
