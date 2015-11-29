using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSInterfaces.Models
{
    public class Session
    {
        public Guid SessionId { get; set; }
        public User SessionUser { get; set; }
    }
}
