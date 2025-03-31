using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyaMarketplace.Model
{
    public class LoginData : IData
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
