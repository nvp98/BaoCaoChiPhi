namespace BaoCaoChiPhi.Application.Interfaces;

public interface IAuthService
{
    bool Validate(string username, string password);
}
