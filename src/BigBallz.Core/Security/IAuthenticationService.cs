namespace BigBallz.Core.Security
{
    public interface IAuthenticationService
    {
        void ChangePassword(string currentPassword, string newPassword);
        Usuario GetUsuarioByLogin(string loginRede);
        Usuario GetUsuarioById(int idUsuario);
    }
}