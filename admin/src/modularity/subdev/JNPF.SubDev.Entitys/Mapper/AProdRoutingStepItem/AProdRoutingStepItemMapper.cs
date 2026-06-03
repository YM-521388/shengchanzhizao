using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdRoutingStepItem;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdRoutingStepItem;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdRoutingStepItemCrInput, AProdRoutingStepItemEntity>()
			.Map(dest => dest.F_StepId, src => src.F_StepId != null ? src.F_StepId : null)
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
		;
		config.ForType<AProdRoutingStepItemEntity, AProdRoutingStepItemInfoOutput>()
			.Map(dest => dest.F_StepId, src => src.F_StepId != null ? src.F_StepId : null)
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Num, src => src.F_Num != null ? src.F_Num : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
