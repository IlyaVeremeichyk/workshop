using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using PortfolioManagerProxy.Models;
using PortfolioManagerProxy.Repositories;

namespace PortfolioManagerProxy.Services
{
    public class UserService
    {
        private readonly UsersRepository _usersRepository;
        public UserService()
        {
            _usersRepository = new UsersRepository();
        }

        public void CreateUser(User user)
        {
            _usersRepository.Create(user);
        }
    }
}