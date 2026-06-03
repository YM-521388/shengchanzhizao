using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuWarehouse;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.APuWarehouse;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuWarehouseCrInput, APuWarehouseEntity>()
			.Map(dest => dest.F_ParentId, src => src.F_ParentId != null && src.F_ParentId.Count > 0 ? src.F_ParentId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_Name, src => src.F_Name != null ? src.F_Name : null)
			.Map(dest => dest.F_UsersId, src => src.F_UsersId != null && src.F_UsersId.Count > 0 ? src.F_UsersId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_Location, src => src.F_Location != null && src.F_Location.Count > 0 ? src.F_Location.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_Address, src => src.F_Address != null ? src.F_Address : null)
			.Map(dest => dest.F_QRCode, src => src.F_QRCode != null ? src.F_QRCode : null)
			.Map(dest => dest.F_State, src => src.F_State)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuWarehouseEntity, APuWarehouseInfoOutput>()
			.Map(dest => dest.F_ParentId, src => src.F_ParentId != null ? src.F_ParentId.ToObject<List<string>>() : null)
			.Map(dest => dest.F_Name, src => src.F_Name != null ? src.F_Name : null)
			.Map(dest => dest.F_UsersId, src => src.F_UsersId != null ? src.F_UsersId.ToObject<List<string>>() : null)
			.Map(dest => dest.F_Location, src => src.F_Location != null ? src.F_Location.ToObject<List<string>>() : null)
			.Map(dest => dest.F_Address, src => src.F_Address != null ? src.F_Address : null)
			.Map(dest => dest.F_QRCode, src => src.F_QRCode != null ? src.F_QRCode : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
