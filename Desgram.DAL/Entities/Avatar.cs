﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class Avatar:ImageContent
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
