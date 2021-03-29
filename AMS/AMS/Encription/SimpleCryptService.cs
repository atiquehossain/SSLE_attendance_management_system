using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Encription
{
    public class SimpleCryptService : SimpleCryptServiceBase
    {
        protected override string EncryptionKey => "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static SimpleCryptService Factory()
        {
            return new SimpleCryptService();
        }
    }
}
