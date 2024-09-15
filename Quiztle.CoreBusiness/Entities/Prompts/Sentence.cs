using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiztle.CoreBusiness.Entities.Prompts
{
    public class Sentence
    {
        public Guid Id { get; set; } = new();
        public string? Text { get; set; }
        public DateTime Created = DateTime.UtcNow;
    }
}
