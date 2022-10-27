using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class User
    {
        public Guid Id { get; init; }
        
        public string Name { get; init; }
        public string Email { get; init; }
        public string PasswordHash { get; init; }
        public DateTimeOffset BirthDate { get; init; }
    }
}
