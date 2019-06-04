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
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    public class AccountsController : ODataController
    {
        private readonly IOrganizationService _service;

        public AccountsController(IOrganizationService service)
        {
            _service = service;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            var accounts = new List<Account>();

            var query = new QueryExpression("account");
            query.ColumnSet = new ColumnSet("accountid", "name");

            var results = _service.RetrieveMultiple(query);

            foreach (var result in results.Entities)
            {
                accounts.Add(new Account()
                {
                    ID = result.Id,
                    Name = result.GetAttributeValue<string>("name")
                });
            }

            return Ok(accounts.AsQueryable());
        }
    }
}