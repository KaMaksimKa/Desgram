﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.BadRequestExceptions
{
    public abstract class InvalidPropertyException:BadRequestException
    {
        public abstract string PropertyName { get; }
    }
}
