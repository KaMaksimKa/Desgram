using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class EmailCode
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string CodeHash { get; set; } = null!;
        public DateTimeOffset ExpiredDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
    }
}
