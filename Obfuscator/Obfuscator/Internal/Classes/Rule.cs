using Obfuscator.Internal.Interfaces;
using Obfuscator.Protections.Analysis;
using Obfuscator.Protections.Assembly;
using Obfuscator.Protections.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscator.Internal.Classes
{
    class Rule
    {
        private bool enable_renamer = false;
        private bool enable_antiildasm = false;
        private bool enable_constantprotection = false;
        private bool enable_constantmutation = false;

        private List<IProtector> TaskList = new List<IProtector>();
        public Rule(bool enable_renamer, bool enable_antiildasm, bool enable_constantprotection, bool enable_constantmutation)
        {
            this.enable_renamer = enable_renamer;
            this.enable_antiildasm = enable_antiildasm;
            this.enable_constantprotection = enable_constantprotection;
            this.enable_constantmutation = enable_constantmutation;
            this.enable_renamer = enable_renamer;
        }

        public void AddTasksToList()
        {
            if (enable_antiildasm) TaskList.Add(new AntiILDasmProtection());
            if (enable_constantmutation) TaskList.Add(new ConstantMutation());
            if (enable_constantprotection) TaskList.Add(new ConstantProtection());
            if (enable_renamer) TaskList.Add(new RenameProtection());


        }

        public List<IProtector> GetTasks()
        {
            return TaskList;
        }

    }

}
