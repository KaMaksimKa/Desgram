using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class Publication
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Description { get; set; } = String.Empty;
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<ImagePublication> ImagesPublication { get; set; } = new List<ImagePublication>();
        public List<LikePublication> LikesPublication { get; set; } = new List<LikePublication>();
        public int AmountLikes { get; set; } = 0;
    }
}
