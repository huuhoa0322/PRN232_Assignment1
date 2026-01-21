using HE186716_DoHuuHoa_SE1884_NET_A01_BE.DTOs;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    Task<LoginResponseDto?> ValidateAdminAsync(string email, string password);
    string GetRoleName(int? role, bool isAdmin = false);
}
