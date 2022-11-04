using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class ImagePublication:Attach
    {
        public Guid PublicationId { get; set; }
        public Publication Publication { get; set; }
    }
}
