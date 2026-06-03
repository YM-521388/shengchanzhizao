using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AConfig;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AConfig;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AConfigCrInput, AConfigEntity>()
			.Map(dest => dest.F_Name, src => src.F_Name != null ? src.F_Name : null)
			.Map(dest => dest.F_Value, src => src.F_Value)
			.Map(dest => dest.F_Code, src => src.F_Code != null ? src.F_Code : null)
		;
		config.ForType<AConfigEntity, AConfigInfoOutput>()
			.Map(dest => dest.F_Name, src => src.F_Name != null ? src.F_Name : null)
			.Map(dest => dest.F_Value, src => src.F_Value != null ? src.F_Value : null)
			.Map(dest => dest.F_Code, src => src.F_Code != null ? src.F_Code : null)
		;
	}
}
