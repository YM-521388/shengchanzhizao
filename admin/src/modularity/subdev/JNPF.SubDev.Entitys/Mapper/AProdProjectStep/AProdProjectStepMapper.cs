using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdProjectStep;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdProjectStep;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdProjectStepCrInput, AProdProjectStepEntity>()
			.Map(dest => dest.F_ProjectItemId, src => src.F_ProjcetItemId != null ? src.F_ProjcetItemId : null)
			.Map(dest => dest.F_ProcessId, src => src.F_ProcessId != null ? src.F_ProcessId : null)
			.Map(dest => dest.F_StartDate, src => src.F_StartDate != null ? src.F_StartDate : null)
			.Map(dest => dest.F_EndDate, src => src.F_EndDate != null ? src.F_EndDate : null)
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
		;
		config.ForType<AProdProjectStepEntity, AProdProjectStepInfoOutput>()
			.Map(dest => dest.F_ProjcetItemId, src => src.F_ProjectItemId != null ? src.F_ProjectItemId : null)
			.Map(dest => dest.F_ProcessId, src => src.F_ProcessId != null ? src.F_ProcessId : null)
			.Map(dest => dest.F_StartDate, src => src.F_StartDate != null ? src.F_StartDate : null)
			.Map(dest => dest.F_EndDate, src => src.F_EndDate != null ? src.F_EndDate : null)
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
