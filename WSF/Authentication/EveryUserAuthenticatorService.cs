using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSF.Authentication
{
    public class EveryUserAuthenticatorService : IUserAuthenticatorService
    {
        public bool TryAuthenticate(string username, string password)
        {
            return true;
        }
    }
}
