using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    /// <summary>
    /// Static class providing API access methods and their corresponding URIs.
    /// </summary>
    public static class APIAccess
    {
        private static Dictionary<APIMethod, String> m_MethodUrls = new Dictionary<APIMethod, string>
        {
            { APIMethod.GetCompanyForSetup, "/api/Setup/GetCompanyForSetup" },
          
        };
        /// <summary>
        /// Gets the URI for a specific API method.
        /// </summary>
        /// <param name="method">The API method.</param>
        /// <returns>The URI associated with the specified API method.</returns>
        public static string GetMethodUri(this APIMethod method)
        {    
            // Retrieve URI for the specified API method
            if (m_MethodUrls.ContainsKey(method))
            {
                return m_MethodUrls[method];
            }
            // Throw exception if the method is not found in the dictionary
            throw new ArgumentException("method");
        }
    }
    /// <summary>
    /// Enumeration representing various API methods.
    /// </summary>
    public enum APIMethod
    {
        GetCompanyForSetup,
    }
    }
