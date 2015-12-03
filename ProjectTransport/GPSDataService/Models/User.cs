using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSDataService.Models
{
    public class User
    {
        public string UserId { get; internal set; }
        public string Login { get; internal set; }
        public string Password { get; internal set; }

        public User()
        {

        }

        public User(string login, string password)
        {
            Login = login;
            Password = password;
            UserId = Helpers.MD5Encoder.EncodeMD5(login);
        }
    }
}
