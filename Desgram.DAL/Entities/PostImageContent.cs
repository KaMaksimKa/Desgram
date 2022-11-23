using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class PostImageContent: ImageContent
    {
        public Guid PostId { get; set; }

        public virtual Post Post { get; set; } = null!;
    }
}
