using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Obfuscator.Helpers;
using Obfuscator.Internal.Classes;
using Obfuscator.Internal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscator.Protections.Constants
{
    class ConstantProtection : IProtector
    {
        string IProtector.Name => "ConstantProtection";
        string IProtector.Descrption => "Will encode all numeric and string values";
        string IProtector.ProtectionType => "Constant";



        private CilBody body;
        private MethodDef decryptionmethod;
        public void InjectPhase(SpectreContext spctx) {

            ModuleDefMD typeModule = ModuleDefMD.Load(typeof(Runtime.ConstantRuntime).Module);
            TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.ConstantRuntime).MetadataToken));
            IEnumerable<IDnlibDef> members = InjectHelper.Inject(typeDef, spctx.GlobalType, spctx.ManifestModule);

            decryptionmethod = (MethodDef)members.Single(method => method.Name == "DecodeNum");

        }



        public void ProtectionPhase(SpectreContext spctx)
        {
            foreach (ModuleDef module in spctx.Assembly.Modules)
            {
                foreach (TypeDef type in module.Types)
                {
                    foreach (MethodDef method in type.Methods)
                    {
                        if (!method.HasBody) continue;
                        if (method.HasBody) if (!method.Body.HasInstructions) continue;

                        body = method.Body;
                        for (int i = 0; i < body.Instructions.Count; i++)
                        {
                            if (body.Instructions[i].IsLdcI4())
                            {
                                EncodeNumeric(spctx,method, i);
                                i += 2;
                            }

                        }
                        body.SimplifyBranches();
                        body.OptimizeBranches();

                    }
                }
            }
        }

      
        private void EncodeNumeric(SpectreContext spctx, MethodDef method, int i)
        {
            body = method.Body;
            int key = LeetRandom.rnd.Next(0, int.MaxValue);

            FieldDef field = new FieldDefUser(Guid.NewGuid().ToString(), new FieldSig(spctx.ManifestModule.CorLibTypes.Int32), FieldAttributes.Public | FieldAttributes.Static);
            //Add Field
            spctx.GlobalType.Fields.Add(field);
            int cctorbdycount = spctx.cctor.Body.Instructions.Count;
            //Init Field
            spctx.cctor.Body.Instructions.Insert(cctorbdycount - 1, Instruction.Create(OpCodes.Stsfld, field));
            spctx.cctor.Body.Instructions.Insert(cctorbdycount - 1, Instruction.CreateLdcI4(key));

            int operand = body.Instructions[i].GetLdcI4Value();
            int newoperand = EncodeNum(operand, key);
            body.Instructions[i] = Instruction.CreateLdcI4(newoperand);
            body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Ldsfld, field)); // insert int field
            body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Call, decryptionmethod));

        }

        private unsafe int EncodeNum(int num, int key)
        {

            int sub = sizeof(DateTime) - sizeof(TimeSpan);
            int num2 = num - sub;
            int xored = num ^ key;

            return xored;

        }

    }
}
