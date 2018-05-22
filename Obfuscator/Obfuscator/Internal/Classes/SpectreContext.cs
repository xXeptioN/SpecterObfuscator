using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscator.Internal.Classes
{
    class SpectreContext
    {
        public AssemblyDef Assembly;
        public ModuleDef ManifestModule;
        public TypeDef GlobalType;
        public Importer Imp;
        public MethodDef cctor;

        public SpectreContext(AssemblyDef asm)
        {
            this.Assembly = asm;
            this.ManifestModule = asm.ManifestModule;
            this.GlobalType = this.ManifestModule.GlobalType;
            this.Imp = new Importer(ManifestModule);
            this.cctor = this.GlobalType.FindOrCreateStaticConstructor();
        }

      


    }
}
