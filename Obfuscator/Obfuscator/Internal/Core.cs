using Obfuscator.Internal.Classes;
using Obfuscator.Internal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Obfuscator.Internal
{
    class Core
    {
        private SpectreContext spctx;
        private Rule rule;
        private List<IProtector> Tasks = new List<IProtector>();
        public Core(SpectreContext spctx, Rule rule)
        {
            this.spctx = spctx;
            this.rule = rule;
        }

        public void DoObfuscation()
        {
            rule.AddTasksToList();
            Tasks = rule.GetTasks();
            Console.WriteLine(Tasks.Count.ToString());
            if (!(Tasks.Count == 0))
                RunTasks();
            else { throw new Exception("You have to select at least one module!"); }
        }
        private void RunTasks()
        {
            foreach(IProtector prot in Tasks)
            {
                try
                {
                    Logger.Log("Current Module: " + prot.Name + "\n" + "Description: " + prot.Descrption + "\n" +"Protectiontype: " + prot.ProtectionType  +"\n");
                    Logger.Log("Injecting..." + "\n");
                    prot.InjectPhase(spctx);
                    Logger.Log("Protecting..." + "\n");
                    prot.ProtectionPhase(spctx);
                    Logger.Log("Finished with " + prot.Name + "\n");
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.StackTrace);
                }
            }

            
        }

    }
}
