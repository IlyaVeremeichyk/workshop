using PortfolioManagerProxy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PortfolioManagerProxy.Controllers
{
    public class UsersController : ApiController
    {
        private readonly UserService _userService;

        public UsersController()
        {
            _userService = new UserService();
        }

        public int Post([FromBody]string userName)
        {
            return this._userService.CreateUser(userName);
        }

    }
}
