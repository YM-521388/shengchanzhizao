using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AMaintDeviceQr;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AMaintDeviceQr;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AMaintDeviceQrCrInput, AMaintDeviceQrEntity>()
			.Map(dest => dest.F_Code, src => src.F_Code != null ? src.F_Code : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AMaintDeviceQrEntity, AMaintDeviceQrInfoOutput>()
			.Map(dest => dest.F_Code, src => src.F_Code != null ? src.F_Code : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
