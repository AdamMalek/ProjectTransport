using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GPSDataService.Models
{
    [DataContract(IsReference =true)]
    public class User
    {
        [DataMember]
        public string UserId { get; internal set; }
        [DataMember]
        public string Login { get; internal set; }
        [DataMember]
        public string Password { get; internal set; }

        [DataMember]
        public virtual ICollection<Route> Routes { get; set; }

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
