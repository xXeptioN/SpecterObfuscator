using Obfuscator.Internal.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscator.Internal.Interfaces
{


    interface IProtector
    {
        string Name { get; }
        string Descrption { get; }
        string ProtectionType { get; }
        void InjectPhase(SpectreContext spctx);
        void ProtectionPhase(SpectreContext spctx);
        

    }
}

