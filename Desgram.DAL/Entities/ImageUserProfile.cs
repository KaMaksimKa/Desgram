using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class ImageUserProfile:Image
    {
        public Guid UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
