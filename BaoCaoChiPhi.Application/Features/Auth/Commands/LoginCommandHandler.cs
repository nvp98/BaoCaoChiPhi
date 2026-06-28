using BaoCaoChiPhi.Application.Common;
using BaoCaoChiPhi.Application.Interfaces;
using MediatR;

namespace BaoCaoChiPhi.Application.Features.Auth.Commands;

public class LoginCommandHandler(IAuthService authService)
    : IRequestHandler<LoginCommand, Result<string>>
{
    public Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var token = authService.Login(request.Username, request.Password);
        return Task.FromResult(token is null
            ? Result<string>.Failure("Sai tên đăng nhập hoặc mật khẩu")
            : Result<string>.Success(token));
    }
}
