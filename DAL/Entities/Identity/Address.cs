using DAL.Entities.baseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Identity
{
    public class Address : BaseEntity
    {
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
    }
}
