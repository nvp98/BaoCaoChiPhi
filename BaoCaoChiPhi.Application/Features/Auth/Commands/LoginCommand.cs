using BaoCaoChiPhi.Application.Common;
using MediatR;

namespace BaoCaoChiPhi.Application.Features.Auth.Commands;

public record LoginCommand(string Username, string Password) : IRequest<Result<string>>;
