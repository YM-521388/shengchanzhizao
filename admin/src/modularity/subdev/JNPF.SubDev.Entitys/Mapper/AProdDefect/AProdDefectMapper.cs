using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdDefect;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdDefect;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdDefectCrInput, AProdDefectEntity>()
			.Map(dest => dest.F_DefectCode, src => src.F_DefectCode != null ? src.F_DefectCode : null)
			.Map(dest => dest.F_DefectName, src => src.F_DefectName != null ? src.F_DefectName : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AProdDefectEntity, AProdDefectInfoOutput>()
			.Map(dest => dest.F_DefectCode, src => src.F_DefectCode != null ? src.F_DefectCode : null)
			.Map(dest => dest.F_DefectName, src => src.F_DefectName != null ? src.F_DefectName : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
