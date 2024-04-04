using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunoTheBot.CoreBusiness.Entities.Quiz
{
    public class Reference
    {
        public int ID { get; set; }
        public string? Name { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
