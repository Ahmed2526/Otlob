using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Cart
    {
        public string? Id { get; set; }
        public List<CartItem> items { get; set; } = new List<CartItem>();
    }
}
