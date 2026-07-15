using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using BaoCaoChiPhi.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace BaoCaoChiPhi.API.Authentication;

public class BasicAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IAuthService authService)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
            return Task.FromResult(AuthenticateResult.NoResult());

        if (!AuthenticationHeaderValue.TryParse(authHeader, out var header) ||
            !string.Equals(header.Scheme, "Basic", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(AuthenticateResult.NoResult());

        string credentials;
        try
        {
            credentials = Encoding.UTF8.GetString(Convert.FromBase64String(header.Parameter ?? ""));
        }
        catch
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Basic auth credentials"));
        }

        var separatorIndex = credentials.IndexOf(':');
        if (separatorIndex < 0)
            return Task.FromResult(AuthenticateResult.Fail("Invalid Basic auth format"));

        var username = credentials[..separatorIndex];
        var password = credentials[(separatorIndex + 1)..];

        if (!authService.Validate(username, password))
            return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));

        var claims = new[] { new Claim(ClaimTypes.Name, username) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers.WWWAuthenticate = "Basic realm=\"BaoCaoChiPhi\"";
        return base.HandleChallengeAsync(properties);
    }
}
