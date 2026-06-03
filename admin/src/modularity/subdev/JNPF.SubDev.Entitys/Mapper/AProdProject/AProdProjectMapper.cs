using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdProject;
using JNPF.example.Entitys.Dto.AProdProjectItem;
using Mapster;

namespace JNPF.example.Entitys.Mapper.AProdProject;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdProjectCrInput, AProdProjectEntity>()
			.Map(dest => dest.F_ProjectNo, src => src.F_ProjectNo != null ? src.F_ProjectNo : null)
			.Map(dest => dest.F_ProjectName, src => src.F_ProjectName != null ? src.F_ProjectName : null)
			.Map(dest => dest.F_ContractId, src => src.F_ContractId != null ? src.F_ContractId : null)
			.Map(dest => dest.F_ProjectType, src => src.F_ProjectType != null ? src.F_ProjectType : null)
			.Map(dest => dest.F_Status, src => src.F_Status != null ? src.F_Status : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_UpdateUserId, src => src.F_UpdateUserId != null ? src.F_UpdateUserId : null)
		;
		config.ForType<AProdProjectEntity, AProdProjectInfoOutput>()
			.Map(dest => dest.F_ProjectNo, src => src.F_ProjectNo != null ? src.F_ProjectNo : null)
			.Map(dest => dest.F_ProjectName, src => src.F_ProjectName != null ? src.F_ProjectName : null)
			.Map(dest => dest.F_ContractId, src => src.F_ContractId != null ? src.F_ContractId : null)
			.Map(dest => dest.F_ProjectType, src => src.F_ProjectType != null ? src.F_ProjectType : null)
			.Map(dest => dest.F_Status, src => src.F_Status != null ? src.F_Status : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
			.Map(dest => dest.F_UpdateUserId, src => src.F_UpdateUserId != null ? src.F_UpdateUserId : null)
			.Map(dest => dest.F_UpdateTime, src => src.F_UpdateTime != null ? string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_UpdateTime) : null)
            .Map(dest => dest.tableField751ecb, src => src.AProdProjectItemList.Adapt<List<AProdProjectItemInfoOutput>>())
		;
		config.ForType<AProdProjectEntity, AProdProjectDetailOutput>()
            .Map(dest => dest.tableField751ecb, src => src.AProdProjectItemList.Adapt<List<AProdProjectItemDetailOutput>>())
		;
	}
}
