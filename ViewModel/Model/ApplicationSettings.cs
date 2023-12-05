using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Model
{
    public class ApplicationSettings
    {
        public string AuthKey { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ServerId { get; set; }
        public long LocationId { get; set; }
        public string MachineAddress { get; set; }
        public string MachineName { get; set; }
        public string JWEToken { get; set; }
    }
}
