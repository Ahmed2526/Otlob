using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Identity
{
    public class ApplicationUser :IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address? Address { get; set; }
    }
}
