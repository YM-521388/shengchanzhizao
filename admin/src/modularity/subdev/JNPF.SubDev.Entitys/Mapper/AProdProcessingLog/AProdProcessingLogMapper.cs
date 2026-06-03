using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdProcessingLog;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdProcessingLog;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdProcessingLogCrInput, AProdProcessingLogEntity>()
			.Map(dest => dest.F_ProcessingId, src => src.F_ProcessingId != null ? src.F_ProcessingId : null)
			.Map(dest => dest.F_Title, src => src.F_Title != null ? src.F_Title : null)
			.Map(dest => dest.F_Content, src => src.F_Content != null ? src.F_Content : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AProdProcessingLogEntity, AProdProcessingLogInfoOutput>()
			.Map(dest => dest.F_ProcessingId, src => src.F_ProcessingId != null ? src.F_ProcessingId : null)
			.Map(dest => dest.F_Title, src => src.F_Title != null ? src.F_Title : null)
			.Map(dest => dest.F_Content, src => src.F_Content != null ? src.F_Content : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
