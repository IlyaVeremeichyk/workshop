using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioManagerProxy.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public  string Patronymic { get; set; }
        public DateTime BirthDate { get; set; }
        public  string Profession { get; set; }
    }
}