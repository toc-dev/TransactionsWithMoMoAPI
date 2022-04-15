using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XendBuySellEscrow.Models.ResponseModels
{
    public class ApiKeyResponse
    {
        public string ApiResponse { get; set; }
    }

    public class TokenKeyResponse
    {
        public string ApiToken { get; set; }
    }
}
