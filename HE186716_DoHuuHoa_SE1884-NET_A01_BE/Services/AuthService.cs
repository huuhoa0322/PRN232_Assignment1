using HE186716_DoHuuHoa_SE1884_NET_A01_BE.DTOs;
using HE186716_DoHuuHoa_SE1884_NET_A01_BE.Repositories;

namespace HE186716_DoHuuHoa_SE1884_NET_A01_BE.Services;

public class AuthService : IAuthService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IAccountRepository accountRepository, IConfiguration configuration)
    {
        _accountRepository = accountRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        // Check if it's the admin account from appsettings.json
        var adminResult = await ValidateAdminAsync(request.Email, request.Password);
        if (adminResult != null)
            return adminResult;

        // Check database accounts
        var account = await _accountRepository.AuthenticateAsync(request.Email, request.Password);
        if (account == null)
            return null;

        return new LoginResponseDto
        {
            AccountId = account.AccountId,
            AccountName = account.AccountName,
            AccountEmail = account.AccountEmail,
            AccountRole = account.AccountRole,
            RoleName = GetRoleName(account.AccountRole),
            IsAdmin = false
        };
    }

    public Task<LoginResponseDto?> ValidateAdminAsync(string email, string password)
    {
        var adminEmail = _configuration["AdminAccount:Email"];
        var adminPassword = _configuration["AdminAccount:Password"];

        if (email == adminEmail && password == adminPassword)
        {
            return Task.FromResult<LoginResponseDto?>(new LoginResponseDto
            {
                AccountId = 0,
                AccountName = "Administrator",
                AccountEmail = adminEmail,
                AccountRole = 0,
                RoleName = "Admin",
                IsAdmin = true
            });
        }

        return Task.FromResult<LoginResponseDto?>(null);
    }

    public string GetRoleName(int? role, bool isAdmin = false)
    {
        if (isAdmin) return "Admin";
        return role switch
        {
            1 => "Staff",
            2 => "Lecturer",
            _ => "Unknown"
        };
    }
}
