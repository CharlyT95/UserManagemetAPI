using Aduanas.Aci.Seguridad.Api.Helpers;

namespace Aduanas.Aci.Seguridad.Api.Services
{
    public class AuthService
    {
        private readonly JwtHelper _jwtHelper;

        public AuthService(JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }

        public string Login(string username, string password)
        {
            if (username == "admin" && password == "1234")
            {
                return _jwtHelper.GenerateToken(username);
            }

            return null;
        }
    }
}
