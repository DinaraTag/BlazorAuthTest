using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWATest.Models
{
    public class LocalUserInfo
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string accessToken { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string position { get; set; }
    }
}
