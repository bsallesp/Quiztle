using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiztle.CoreBusiness.Entities.Paid
{
    public class Paid
    {
        public Guid Id { get; set; } = new();
        public Guid UserId { get; set; } = new();
        public string? CustomerId { get; set; }
        public string? PriceId { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
