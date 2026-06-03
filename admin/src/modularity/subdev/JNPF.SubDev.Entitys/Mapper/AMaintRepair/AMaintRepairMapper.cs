using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AMaintRepair;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AMaintRepair;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AMaintRepairCrInput, AMaintRepairEntity>()
			.Map(dest => dest.F_RepairNo, src => src.F_RepairNo != null ? src.F_RepairNo : null)
            .Map(dest => dest.F_DeviceId, src => src.F_DeviceId != null && src.F_DeviceId.Count > 0 ? src.F_DeviceId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_GroupId, src => src.F_GroupId != null ? src.F_GroupId : null)
			.Map(dest => dest.F_DispatchType, src => src.F_DispatchType != null ? src.F_DispatchType : null)
			.Map(dest => dest.F_HandlerUserId, src => src.F_HandlerUserId != null ? src.F_HandlerUserId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_ReceiveTime, src => src.F_ReceiveTime != null ? src.F_ReceiveTime : null)
			.Map(dest => dest.F_HandleTime, src => src.F_HandleTime != null ? src.F_HandleTime : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_PauseReason, src => src.F_PauseReason != null ? src.F_PauseReason : null)
			.Map(dest => dest.F_CancelReason, src => src.F_CancelReason != null ? src.F_CancelReason : null)
			.Map(dest => dest.F_AuditorUserId, src => src.F_AuditorUserId != null ? src.F_AuditorUserId : null)
			.Map(dest => dest.F_AuditTime, src => src.F_AuditTime != null ? src.F_AuditTime : null)
			.Map(dest => dest.F_IsSolved, src => src.F_IsSolved != null ? src.F_IsSolved : null)
			.Map(dest => dest.F_HandleResult, src => src.F_HandleResult != null ? src.F_HandleResult : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorOrganizeId, src => src.F_CreatorOrganizeId != null ? src.F_CreatorOrganizeId : null)
		;
		config.ForType<AMaintRepairEntity, AMaintRepairInfoOutput>()
			.Map(dest => dest.F_RepairNo, src => src.F_RepairNo != null ? src.F_RepairNo : null)
            .Map(dest => dest.F_DeviceId, src => src.F_DeviceId != null ? src.F_DeviceId.ToObject<List<string>>() : null)
			.Map(dest => dest.F_GroupId, src => src.F_GroupId != null ? src.F_GroupId : null)
			.Map(dest => dest.F_DispatchType, src => src.F_DispatchType != null ? src.F_DispatchType : null)
			.Map(dest => dest.F_HandlerUserId, src => src.F_HandlerUserId != null ? src.F_HandlerUserId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_ReceiveTime, src => src.F_ReceiveTime != null ? src.F_ReceiveTime : null)
			.Map(dest => dest.F_HandleTime, src => src.F_HandleTime != null ? src.F_HandleTime : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_PauseReason, src => src.F_PauseReason != null ? src.F_PauseReason : null)
			.Map(dest => dest.F_CancelReason, src => src.F_CancelReason != null ? src.F_CancelReason : null)
			.Map(dest => dest.F_AuditorUserId, src => src.F_AuditorUserId != null ? src.F_AuditorUserId : null)
			.Map(dest => dest.F_AuditTime, src => src.F_AuditTime != null ? src.F_AuditTime : null)
			.Map(dest => dest.F_IsSolved, src => src.F_IsSolved != null ? src.F_IsSolved : null)
			.Map(dest => dest.F_HandleResult, src => src.F_HandleResult != null ? src.F_HandleResult : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
			.Map(dest => dest.F_CreatorOrganizeId, src => src.F_CreatorOrganizeId != null ? src.F_CreatorOrganizeId : null)
		;
	}
}
