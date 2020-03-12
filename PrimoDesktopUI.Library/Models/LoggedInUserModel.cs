using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimoDesktopUI.Library.Models
{
    public class LoggedInUserModel : ILoggedInUserModel
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }

        //It's not best practice to write functions here (in a model) but we will do it temporarily
        public void LogOffUser()
        {
            Token = "";
            Id = "";
            FirstName = "";
            LastName = "";
            EmailAddress = "";
            CreatedDate = DateTime.MinValue;
        }

    }
}
