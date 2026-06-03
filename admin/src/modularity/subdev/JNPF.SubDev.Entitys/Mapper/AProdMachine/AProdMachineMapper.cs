using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdMachine;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdMachine;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdMachineCrInput, AProdMachineEntity>()
			.Map(dest => dest.F_MachineCode, src => src.F_MachineCode != null ? src.F_MachineCode : null)
			.Map(dest => dest.F_MachineName, src => src.F_MachineName != null ? src.F_MachineName : null)
			.Map(dest => dest.F_MachineSpec, src => src.F_MachineSpec != null ? src.F_MachineSpec : null)
			.Map(dest => dest.F_MachineStatus, src => src.F_MachineStatus != null ? src.F_MachineStatus : null)
			.Map(dest => dest.F_Image, src => src.F_Image != null && src.F_Image.Count > 0 ? src.F_Image.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_MachineType, src => src.F_MachineType != null && src.F_MachineType.Count > 0 ? src.F_MachineType.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_WorkshopId, src => src.F_WorkshopId != null && src.F_WorkshopId.Count > 0 ? src.F_WorkshopId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_LineId, src => src.F_LineId != null ? src.F_LineId : null)
			.Map(dest => dest.F_DailyRunHours, src => src.F_DailyRunHours != null ? src.F_DailyRunHours : null)
			.Map(dest => dest.F_StdHour, src => src.F_StdHour != null ? src.F_StdHour : null)
			.Map(dest => dest.F_StdMinute, src => src.F_StdMinute != null ? src.F_StdMinute : null)
			.Map(dest => dest.F_StdSecond, src => src.F_StdSecond != null ? src.F_StdSecond : null)
			.Map(dest => dest.F_StdOutput, src => src.F_StdOutput != null ? src.F_StdOutput : null)
			.Map(dest => dest.F_State, src => src.F_State)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AProdMachineEntity, AProdMachineInfoOutput>()
			.Map(dest => dest.F_MachineCode, src => src.F_MachineCode != null ? src.F_MachineCode : null)
			.Map(dest => dest.F_MachineName, src => src.F_MachineName != null ? src.F_MachineName : null)
			.Map(dest => dest.F_MachineSpec, src => src.F_MachineSpec != null ? src.F_MachineSpec : null)
			.Map(dest => dest.F_MachineStatus, src => src.F_MachineStatus != null ? src.F_MachineStatus : null)
			.Map(dest => dest.F_Image, src => src.F_Image != null ? src.F_Image.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
            .Map(dest => dest.F_MachineType, src => src.F_MachineType != null ? src.F_MachineType.ToObject<List<string>>() : null)
            .Map(dest => dest.F_WorkshopId, src => src.F_WorkshopId != null ? src.F_WorkshopId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_LineId, src => src.F_LineId != null ? src.F_LineId : null)
			.Map(dest => dest.F_DailyRunHours, src => src.F_DailyRunHours != null ? src.F_DailyRunHours : null)
			.Map(dest => dest.F_StdHour, src => src.F_StdHour != null ? src.F_StdHour : null)
			.Map(dest => dest.F_StdMinute, src => src.F_StdMinute != null ? src.F_StdMinute : null)
			.Map(dest => dest.F_StdSecond, src => src.F_StdSecond != null ? src.F_StdSecond : null)
			.Map(dest => dest.F_StdOutput, src => src.F_StdOutput != null ? src.F_StdOutput : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
