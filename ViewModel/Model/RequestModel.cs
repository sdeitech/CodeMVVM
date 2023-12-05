using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Model
{
    /// <summary>
    /// Model class representing data for a request to the server.
    /// </summary>
    public class RequestModel
    {
        /// <summary>
        /// Gets or sets the CompanyId.
        /// </summary>
       
       

        /// <summary>
        /// Gets or sets the CompanyName.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        // The following properties are commented out and might be used in the future.
        //public Int64 CompanyServerId { get; set; }
        //public string? AuthToken { get; set; }
        //public string? UserId { get; set; }
        //public string? JWEToken { get; set; }
        //public Int64 CompanyId { get; set; }

    }
}
