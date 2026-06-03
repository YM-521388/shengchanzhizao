using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.ACtcInvoice;
using JNPF.example.Entitys.Dto.ACtcInvoiceLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.ACtcInvoice;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ACtcInvoiceCrInput, ACtcInvoiceEntity>()
			.Map(dest => dest.F_ContractId, src => src.F_ContractId != null ? src.F_ContractId : null)
			.Map(dest => dest.F_Money, src => src.F_Money != null ? src.F_Money : null)
			.Map(dest => dest.F_Status, src => src.F_Status != null ? src.F_Status : null)
			.Map(dest => dest.F_ApplyFiles, src => src.F_ApplyFiles != null && src.F_ApplyFiles.Count > 0 ? src.F_ApplyFiles.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_ApplyRemark, src => src.F_ApplyRemark != null ? src.F_ApplyRemark : null)
			.Map(dest => dest.F_InvoiceFiles, src => src.F_InvoiceFiles != null && src.F_InvoiceFiles.Count > 0 ? src.F_InvoiceFiles.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_InvoiceRemark, src => src.F_InvoiceRemark != null ? src.F_InvoiceRemark : null)
			.Map(dest => dest.F_ApplyUserId, src => src.F_ApplyUserId != null ? src.F_ApplyUserId : null)
			.Map(dest => dest.F_ApplyTime, src => src.F_ApplyTime != null ? src.F_ApplyTime : null)
			.Map(dest => dest.F_InvoiceUserId, src => src.F_InvoiceUserId != null ? src.F_InvoiceUserId : null)
			.Map(dest => dest.F_InvoiceTime, src => src.F_InvoiceTime != null ? src.F_InvoiceTime : null)
		;
		config.ForType<ACtcInvoiceEntity, ACtcInvoiceInfoOutput>()
			.Map(dest => dest.F_ContractId, src => src.F_ContractId != null ? src.F_ContractId : null)
			.Map(dest => dest.F_Money, src => src.F_Money != null ? src.F_Money : null)
			.Map(dest => dest.F_Status, src => src.F_Status != null ? src.F_Status : null)
			.Map(dest => dest.F_ApplyFiles, src => src.F_ApplyFiles != null ? src.F_ApplyFiles.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_ApplyRemark, src => src.F_ApplyRemark != null ? src.F_ApplyRemark : null)
			.Map(dest => dest.F_InvoiceFiles, src => src.F_InvoiceFiles != null ? src.F_InvoiceFiles.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_InvoiceRemark, src => src.F_InvoiceRemark != null ? src.F_InvoiceRemark : null)
			.Map(dest => dest.F_ApplyUserId, src => src.F_ApplyUserId != null ? src.F_ApplyUserId : null)
			.Map(dest => dest.F_ApplyTime, src => src.F_ApplyTime != null ? src.F_ApplyTime : null)
			.Map(dest => dest.F_InvoiceUserId, src => src.F_InvoiceUserId != null ? src.F_InvoiceUserId : null)
			.Map(dest => dest.F_InvoiceTime, src => src.F_InvoiceTime != null ? src.F_InvoiceTime : null)
            .Map(dest => dest.tableField2cca74, src => src.ACtcInvoiceLogList.Adapt<List<ACtcInvoiceLogInfoOutput>>())
		;
		config.ForType<ACtcInvoiceEntity, ACtcInvoiceDetailOutput>()
            .Map(dest => dest.tableField2cca74, src => src.ACtcInvoiceLogList.Adapt<List<ACtcInvoiceLogDetailOutput>>())
		;
	}
}
