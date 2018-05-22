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
    class ConstantMutation : IProtector

    {
        string IProtector.Name => "Constant Mutation";
        string IProtector.Descrption => "Will mutate all numeric values";
        string IProtector.ProtectionType => "Constant";

        private CilBody body;
        public void InjectPhase(SpectreContext spctx) { }


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
                                Mutate(spctx, method, i);
                                i += 2;
                            }

                        }
                        body.SimplifyBranches();
                        body.OptimizeBranches();

                    }
                }
            }
        }
        private void Mutate(SpectreContext spctx, MethodDef method, int i)
        {
            body = method.Body;
            int rndkey = LeetRandom.rnd.Next(0, int.MaxValue);
            int newoperand = body.Instructions[i].GetLdcI4Value() + rndkey;
            body.Instructions[i] = Instruction.CreateLdcI4(newoperand);
            body.Instructions.Insert(i + 1, Instruction.CreateLdcI4(rndkey));
            body.Instructions.Insert(i + 2, OpCodes.Sub.ToInstruction());

        }


    }
}
