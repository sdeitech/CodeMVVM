using System;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Model;
using static System.Net.WebRequestMethods;
using static ViewModel.Model.Http;

namespace ViewModel.Repository
{
    /// <summary>
    /// Repository class responsible for handling setup-related operations.
    /// </summary>
    public class SetupRepository
    {
        /// <summary>
        /// Retrieves company information for setup from the server using an HTTP POST request.
        /// </summary>
        /// <param name="companySetupModel">Request model containing setup information.</param>
        /// <returns>A JsonResponse containing the response data, status, and message.</returns>
        public static JsonResponse<ModelNew> GetCompanyForSetup(RequestModel companySetupModel)
        {
            // Execute a JSON POST request to the specified API method and return the JsonResponse
            return Model.Http.ExecuteJSONPOST<ModelNew>(APIAccess.GetMethodUri(APIMethod.GetCompanyForSetup), companySetupModel, null);
        }
    }
}