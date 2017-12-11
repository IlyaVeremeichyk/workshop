using PortfolioManagerProxy.Services;
using System.Web.Http;
using PortfolioManagerProxy.Models;

namespace PortfolioManagerProxy.Controllers
{
    public class UsersController : ApiController
    {
        private readonly UserService _userService;

        public UsersController()
        {
            _userService = new UserService();
        }

        //{
        //    FirstName :"John",
        //    SecondName: "Black",
        //    Patronymic:"Mc",
        //    BirthDate: "11/12/1981",
        //    Profession: "Driver"
        //}
        public IHttpActionResult Post([FromBody]User user)
        {
            _userService.CreateUser(user);
            return Ok();
        }

    }
}
