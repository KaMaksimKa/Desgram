using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel
{
    public static class CodeGenerator
    {
        public static string GetCode(int length)
        {
            var code = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < length; i++)
            {
                code.Append(random.Next(10).ToString());
            }

            return code.ToString();
        }
    }
}
