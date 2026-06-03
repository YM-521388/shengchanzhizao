using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.ACtcSales;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.ACtcSales;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ACtcSalesCrInput, ACtcSalesEntity>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_ContractId, src => src.F_ContractId != null ? src.F_ContractId : null)
			.Map(dest => dest.F_ExpendDate, src => src.F_ExpendDate != null ? src.F_ExpendDate : null)
			.Map(dest => dest.F_ExpendType, src => src.F_ExpendType != null ? src.F_ExpendType : null)
			.Map(dest => dest.F_Money, src => src.F_Money != null ? src.F_Money : null)
			.Map(dest => dest.F_AuditStatus, src => src.F_AuditStatus != null ? src.F_AuditStatus : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null && src.F_Files.Count > 0 ? src.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_SettleStatus, src => src.F_SettleStatus != null ? src.F_SettleStatus : null)
			.Map(dest => dest.F_SettleFiles, src => src.F_SettleFiles != null && src.F_SettleFiles.Count > 0 ? src.F_SettleFiles.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_SettleDesc, src => src.F_SettleDesc != null ? src.F_SettleDesc : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<ACtcSalesEntity, ACtcSalesInfoOutput>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_ContractId, src => src.F_ContractId != null ? src.F_ContractId : null)
			.Map(dest => dest.F_ExpendDate, src => src.F_ExpendDate != null ? src.F_ExpendDate : null)
			.Map(dest => dest.F_ExpendType, src => src.F_ExpendType != null ? src.F_ExpendType : null)
			.Map(dest => dest.F_Money, src => src.F_Money != null ? src.F_Money : null)
			.Map(dest => dest.F_AuditStatus, src => src.F_AuditStatus != null ? src.F_AuditStatus : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null ? src.F_Files.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_SettleStatus, src => src.F_SettleStatus != null ? src.F_SettleStatus : null)
			.Map(dest => dest.F_SettleFiles, src => src.F_SettleFiles != null ? src.F_SettleFiles.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_SettleDesc, src => src.F_SettleDesc != null ? src.F_SettleDesc : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
