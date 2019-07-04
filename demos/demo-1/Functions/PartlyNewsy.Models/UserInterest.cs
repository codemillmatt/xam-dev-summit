using System;

namespace PartlyNewsy.Models
{
    // This is what will be stored in cosmos
    public class UserInterest
    {      
        public string NewsCategoryName { get; set; }
        public DateTime DateAdded => DateTime.UtcNow;
    }
}
