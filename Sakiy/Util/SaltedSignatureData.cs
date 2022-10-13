using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakiy.Util
{
    public sealed class SaltedSignatureData
    {
        public readonly long Salt;
        public readonly byte[] Signature;
        internal SaltedSignatureData(long salt, byte[] signature)
        {
            Salt = salt;
            Signature = signature;
        }
    }
}
