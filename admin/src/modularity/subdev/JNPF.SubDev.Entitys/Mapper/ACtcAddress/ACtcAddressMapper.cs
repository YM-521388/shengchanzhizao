using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.ACtcAddress;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.ACtcAddress;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ACtcAddressCrInput, ACtcAddressEntity>()
			.Map(dest => dest.F_IsDefault, src => src.F_IsDefault)
			.Map(dest => dest.F_Region, src => src.F_Region != null && src.F_Region.Count > 0 ? src.F_Region.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_Address, src => src.F_Address != null ? src.F_Address : null)
		;
		config.ForType<ACtcAddressEntity, ACtcAddressListOutput>()
			.Map(dest => dest.F_IsDefault, src => src.F_IsDefault != null ? src.F_IsDefault : null)
			.Map(dest => dest.F_Region, src => src.F_Region != null ? src.F_Region : null)
			.Map(dest => dest.F_Address, src => src.F_Address != null ? src.F_Address : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<ACtcAddressEntity, ACtcAddressInfoOutput>()
			.Map(dest => dest.F_IsDefault, src => src.F_IsDefault != null ? src.F_IsDefault : null)
			.Map(dest => dest.F_Region, src => src.F_Region != null ? src.F_Region.ToObject<List<string>>() : null)
			.Map(dest => dest.F_Address, src => src.F_Address != null ? src.F_Address : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<ACtcAddressEntity, ACtcAddressDetailOutput>()
			.Map(dest => dest.F_IsDefault, src => src.F_IsDefault != null ? src.F_IsDefault : null)
			.Map(dest => dest.F_Region, src => src.F_Region != null ? src.F_Region : null)
			.Map(dest => dest.F_Address, src => src.F_Address != null ? src.F_Address : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
