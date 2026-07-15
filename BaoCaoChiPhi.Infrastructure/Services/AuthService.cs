using BaoCaoChiPhi.Application.Interfaces;
using BaoCaoChiPhi.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace BaoCaoChiPhi.Infrastructure.Services;

public class AuthService(IOptions<DefaultUserSettings> userOptions) : IAuthService
{
    private readonly DefaultUserSettings _user = userOptions.Value;

    public bool Validate(string username, string password)
        => string.Equals(username, _user.Username, StringComparison.OrdinalIgnoreCase)
           && password == _user.Password;
}
