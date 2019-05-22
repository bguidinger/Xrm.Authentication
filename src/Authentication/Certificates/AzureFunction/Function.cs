/*================================================================================================================================

  This Sample Code is provided for the purpose of illustration only and is not intended to be used in a production environment.  

  THIS SAMPLE CODE AND ANY RELATED INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, 
  INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.  

  We grant You a nonexclusive, royalty-free right to use and modify the Sample Code and to reproduce and distribute the object 
  code form of the Sample Code, provided that You agree: (i) to not use Our name, logo, or trademarks to market Your software 
  product in which the Sample Code is embedded; (ii) to include a valid copyright notice on Your software product in which the 
  Sample Code is embedded; and (iii) to indemnify, hold harmless, and defend Us and Our suppliers from and against any claims 
  or lawsuits, including attorneys’ fees, that arise or result from the use or distribution of the Sample Code.

 =================================================================================================================================*/
namespace BGuidinger.Xrm.Authentication
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Xrm.Tooling.Connector;

    public static class Function
    {
        [FunctionName("Test")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "GET")]HttpRequestMessage req, ILogger log)
        {
            var clientId = ConfigurationManager.AppSettings["ClientId"];
            var instanceUri = new Uri(ConfigurationManager.AppSettings["InstanceUrl"]);
            var certificateString = ConfigurationManager.AppSettings["Certificate"];

            var certificateBytes = Convert.FromBase64String(certificateString);
            var certificate = new X509Certificate2(certificateBytes, string.Empty, X509KeyStorageFlags.MachineKeySet);

            using (var client = new CrmServiceClient(certificate, StoreName.My, null, instanceUri, true, null, clientId, null, null))
            {
                if (client.IsReady)
                {
                    return req.CreateResponse(HttpStatusCode.OK, $"Connected to {client.ConnectedOrgUniqueName}");
                }
                else
                {
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
        }
    }
}