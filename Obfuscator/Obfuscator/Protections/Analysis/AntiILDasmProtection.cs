using dnlib.DotNet;
using Obfuscator.Internal.Classes;
using Obfuscator.Internal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscator.Protections.Analysis
{
    class AntiILDasmProtection : IProtector
    {
        string IProtector.Name => "AntiILDasmProtection";
        string IProtector.Descrption => "Will prevent users from using ildasm on target application";
        string IProtector.ProtectionType => "Analysis";

        

    
        public void InjectPhase(SpectreContext spctx) {}

        public void ProtectionPhase(SpectreContext spctx)
        {
            var ManifestModule = spctx.ManifestModule;
            //Create Ref
            TypeRef supressref = ManifestModule.CorLibTypes.GetTypeRef("System.Runtime.CompilerServices", "SuppressIldasmAttribute"); 
            var ctorRef = new MemberRefUser(ManifestModule, ".ctor", MethodSig.CreateInstance(ManifestModule.CorLibTypes.Void), supressref);

            var supressattribute = new CustomAttribute(ctorRef);
            //add Attribute
            ManifestModule.CustomAttributes.Add(supressattribute);
        }

    }
}
