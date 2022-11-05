using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class HashTag
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public List<Publication>? Publications { get; set; }
    }
}
