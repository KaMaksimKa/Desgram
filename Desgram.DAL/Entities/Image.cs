using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class Image:Attach
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
