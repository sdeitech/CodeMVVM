using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Model
{
    public class ModelNew
    {
        public Int64 CompanyId { get; set; }
        public Int64 CompanyServerId { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AuthToken { get; set; }
        public string UserId { get; set; }
        public string JWEToken { get; set; }
    }
}
