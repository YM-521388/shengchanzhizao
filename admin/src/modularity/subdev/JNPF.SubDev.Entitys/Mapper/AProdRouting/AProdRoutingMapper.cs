using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdRouting;
using JNPF.example.Entitys.Dto.AProdRoutingStep;
using Mapster;

namespace JNPF.example.Entitys.Mapper.AProdRouting;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdRoutingCrInput, AProdRoutingEntity>()
			.Map(dest => dest.F_RoutingNo, src => src.F_RoutingNo != null ? src.F_RoutingNo : null)
			.Map(dest => dest.F_RoutingName, src => src.F_RoutingName != null ? src.F_RoutingName : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AProdRoutingEntity, AProdRoutingInfoOutput>()
			.Map(dest => dest.F_RoutingNo, src => src.F_RoutingNo != null ? src.F_RoutingNo : null)
			.Map(dest => dest.F_RoutingName, src => src.F_RoutingName != null ? src.F_RoutingName : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableField7b5631, src => src.AProdRoutingStepList.Adapt<List<AProdRoutingStepInfoOutput>>())
		;
		config.ForType<AProdRoutingEntity, AProdRoutingDetailOutput>()
            .Map(dest => dest.tableField7b5631, src => src.AProdRoutingStepList.Adapt<List<AProdRoutingStepDetailOutput>>())
		;
	}
}
