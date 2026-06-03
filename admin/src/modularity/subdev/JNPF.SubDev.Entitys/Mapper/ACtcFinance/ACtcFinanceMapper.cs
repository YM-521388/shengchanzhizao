using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.ACtcFinance;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.ACtcFinance;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ACtcFinanceCrInput, ACtcFinanceEntity>()
			.Map(dest => dest.F_ContractId, src => src.F_ContractId != null ? src.F_ContractId : null)
			.Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
			.Map(dest => dest.F_Money, src => src.F_Money != null ? src.F_Money : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null && src.F_Files.Count > 0 ? src.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_AuditStatus, src => src.F_AuditStatus != null ? src.F_AuditStatus : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<ACtcFinanceEntity, ACtcFinanceInfoOutput>()
			.Map(dest => dest.F_ContractId, src => src.F_ContractId != null ? src.F_ContractId : null)
			.Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
			.Map(dest => dest.F_Money, src => src.F_Money != null ? src.F_Money : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null ? src.F_Files.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_AuditStatus, src => src.F_AuditStatus != null ? src.F_AuditStatus : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
