using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

using System.Net;
using System.Security.Claims;

using PartlyNewsy.Models;


namespace PartlyNewsy.Functions
{
    public static class UserInfoPermission
    {
        static readonly string dbName = Environment.GetEnvironmentVariable("CosmosDBName");
        static readonly string collectionName = Environment.GetEnvironmentVariable("CosmosDBCollection");

        [FunctionName("UserInfoPermission")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "permissions/UserInfoPermission")] HttpRequest req,
            ClaimsPrincipal principal,
            [CosmosDB(databaseName: "user-preferences", collectionName: "Info", ConnectionStringSetting = "CosmosConnectionString")] DocumentClient client,
            ILogger log)
        {
            try
            {
                string userId = GetUserId(principal, log);
             
                if (string.IsNullOrWhiteSpace(userId))
                    return new ForbidResult();
                            
                Permission token = await GetPartitionPermission(userId, client, dbName, collectionName);

                return new OkObjectResult(token.Token);   
            }    
            catch (Exception ex)     
            {
                log.Equals($"Error: {ex}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        static async Task<Permission> GetPartitionPermission(string userId, DocumentClient client, string databaseId, string collectionId)
        {
            string permissionId = "";            
            Permission partitionPermission = new Permission();
            
            permissionId = $"{userId}-partition-info-{collectionId}";            

            Uri permissionUri = UriFactory.CreatePermissionUri(databaseId, userId, permissionId);
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);

            try
            {
                partitionPermission = await client.ReadPermissionAsync(permissionUri);                
            } 
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    await CreateUserIfNotExistAsync(userId, client, databaseId);

                    var newPermission = new Permission
                    {
                        PermissionMode = PermissionMode.All,
                        Id = permissionId,
                        ResourceLink = collectionUri.ToString()
                    };

                    newPermission.ResourcePartitionKey = new PartitionKey($"user-{userId}");
                    
                    var userUri = UriFactory.CreateUserUri(dbName, userId);
                    partitionPermission = await client.CreatePermissionAsync(userUri, newPermission);
                }
            }

            return partitionPermission;
        }
        static string GetUserId(ClaimsPrincipal principal, ILogger log)
        {
            var objectClaimTypeName = @"http://schemas.microsoft.com/identity/claims/objectidentifier";

            var objectClaim = principal.Claims.FirstOrDefault(c => c.Type == objectClaimTypeName);

            foreach (var claim in principal.Claims)
            {
                log.LogInformation($"Claim Name: {claim.Type} || Claim Value: {claim.Value}");
            }

            if (objectClaim == null)
                return "";
            else 
                return objectClaim.Value;            
        }
    
        static async Task CreateUserIfNotExistAsync(string userId, DocumentClient client, string databaseId)
        {
            try
            {
                await client.ReadUserAsync(UriFactory.CreateUserUri(databaseId, userId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateUserAsync(UriFactory.CreateDatabaseUri(databaseId), new User { Id = userId });
                }
            }
        }

        static string SerializePermission(Permission permission)
        {
            string serializedPermission = "";

            using (var memStream = new MemoryStream())
            {
                permission.SaveTo(memStream);
                memStream.Position = 0;

                using (StreamReader sr = new StreamReader(memStream))
                {
                    serializedPermission = sr.ReadToEnd();
                }
            }

            return serializedPermission;
        }
    }
}
