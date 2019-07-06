using System;
using System.Threading.Tasks;

namespace PartlyNewsy.Core
{
    public interface IAuthenticationService
    {
        bool IsLoggedIn();
        Task<bool> Login();        
    }
}
