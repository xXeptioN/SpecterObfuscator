using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscator.Runtime
{
    public static class ConstantRuntime
    {
        public static unsafe int DecodeNum(int num, int key)
        {
            int sub = sizeof(DateTime) - sizeof(TimeSpan);
            int num2 = num + sub;
            int xored = num ^ key;

            return xored;

        }
    }
}
