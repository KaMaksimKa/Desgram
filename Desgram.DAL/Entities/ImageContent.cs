using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class ImageContent
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public virtual ICollection<Image> ImageCandidates { get; set; } = null!;
    }
}
