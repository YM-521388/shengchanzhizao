using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdReportDefect;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdReportDefect;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdReportDefectCrInput, AProdReportDefectEntity>()
			.Map(dest => dest.F_DefectId, src => src.F_DefectId != null ? src.F_DefectId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
		;
		config.ForType<AProdReportDefectEntity, AProdReportDefectInfoOutput>()
			.Map(dest => dest.F_DefectId, src => src.F_DefectId != null ? src.F_DefectId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<AProdReportDefectEntity, AProdReportDefectDetailOutput>()
			.Map(dest => dest.F_DefectId, src => src.F_DefectId != null ? src.F_DefectId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
