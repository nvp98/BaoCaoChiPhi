using BaoCaoChiPhi.Application.Interfaces;
using BaoCaoChiPhi.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace BaoCaoChiPhi.Infrastructure.Services;

public class AuthService(TokenService tokenService, IOptions<DefaultUserSettings> userOptions) : IAuthService
{
    private readonly DefaultUserSettings _user = userOptions.Value;

    public string? Login(string username, string password)
    {
        if (!string.Equals(username, _user.Username, StringComparison.OrdinalIgnoreCase)
            || password != _user.Password)
            return null;

        return tokenService.GenerateToken(username);
    }
}
