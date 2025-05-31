using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDesktop.Token
{
    public class AuthToken
    {
        public string AccessToken { get; set; } 
        public DateTime Expiration { get; set; }

        public AuthToken()
        {
            AccessToken = string.Empty;
        }
    }
}
