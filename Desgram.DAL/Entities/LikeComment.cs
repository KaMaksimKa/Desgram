using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class LikeComment:Like
    {
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
