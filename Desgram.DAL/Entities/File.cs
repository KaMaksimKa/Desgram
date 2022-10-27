using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class File
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
