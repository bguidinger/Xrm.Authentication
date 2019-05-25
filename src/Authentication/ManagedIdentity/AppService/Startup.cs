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
    using System.Linq;
    using Microsoft.AspNet.OData.Builder;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Tooling.Connector;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IOrganizationService>((provider) =>
            {
                var connectionString = Configuration.GetConnectionString("Azure:Auth:ConnectionString");
                CrmServiceClient.AuthOverrideHook = new ManagedIdentityAuthHook(connectionString);
                var serviceUrl = Configuration.GetValue<Uri>("CRM:Url");
                var service = new CrmServiceClient(serviceUrl, true);

                return service;
            });
            services.AddOData();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Account>("Accounts");

            var model = builder.GetEdmModel();

            app.UseMvc(route =>
            {
                route.Select().Filter().Expand().OrderBy().Count().MaxTop(null);
                route.MapODataServiceRoute("odata", null, model);
            });
        }
    }
}
