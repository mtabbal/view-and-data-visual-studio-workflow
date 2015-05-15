//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
using System.Web.Http;

using System.Web.Http.Cors;
using System.Threading.Tasks;
using LmvAuthTokenServer.Models;


namespace LmvAuthTokenServer.Controllers
{
    [EnableCors(origins: "http://localhost:63577", headers: "*", methods: "*")]

    public class AuthTokenController : ApiController
    {
        // Cache token locally.
        static AuthToken ApiToken = new AuthToken();

        public async Task<string> GetAuthApiToken()
        {
            await ApiToken.GetApiToken();
            return AuthToken.TheBrokerToken.AccessToken;
        }
    }
}
