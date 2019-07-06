using System;
namespace PartlyNewsy.Models
{
    public class UserInterestDocumentDb
    {
        public string PartitionKey { get; set; }
        public string id { get; set; }
        public UserInterest Document { get; set; }
    }
}
