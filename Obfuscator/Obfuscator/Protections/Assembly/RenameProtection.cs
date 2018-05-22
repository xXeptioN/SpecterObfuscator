using dnlib.DotNet;
using Obfuscator.Helpers;
using Obfuscator.Internal.Classes;
using Obfuscator.Internal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscator.Protections.Assembly
{
    class RenameProtection : IProtector
    {
        string IProtector.Name => "RenameProtection";
        string IProtector.Descrption => "Will rename all symbols";
        string IProtector.ProtectionType => "Assembly";

        public void InjectPhase(SpectreContext spctx) { }

        private enum RenameMode
        {
            ASCII = 0,
            UNREADABLE = 1
        }

        class Utils
        {
            public static string GenerateName(RenameMode rm)
            {
                string ascii = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string unreadable = "✓ ✔ ☑ ♥ ❤ ❥ ❣ ☂ ☔ ☎ ☏ ☒ ☘ ☠ ☹ ☺ ☻ ♬ ♻ ♲ ♿ ⚠ ☃ ʚϊɞ ✖ ✗ ✘ ♒ ♬ ✄ ✂ ✆ ✉ ✦ ✧ ♱ ♰ ♂ ♀ ☿ ❤ ❥ ❦ ❧ ™ ® © ♡ ♦ ♢ ♔ ♕ ♚ ♛ ★ ☆ ✮ ✯ ☄ ☾ ☽ ☼ ☀ ☁ ☂ ☃ ☻ ☺ ☹ ۞ ۩ εїз ☎ ☏ ¢ ☚ ☛ ☜ ☝ ☞ ☟ ✍ ✌ ☢ ☣ ☠ ☮ ☯ ♠ ♤ ♣ ♧ ♥࿂ ე ჳ ᆡ ༄ ♨ ๑ ❀ ✿ ψ ♆ ☪ ☭ ♪ ♩ ♫ ℘ ℑ ℜ ℵ ♏ η α ʊ ϟ ღ ツ 回 ₪ ™ © ® ¿ ¡ № ⇨ ❝ ❞ ℃ƺ ◠ ◡ ╭ ╮ ╯ ╰ ★ ☆ ⊙¤ ㊣★☆♀◆◇◣◢◥▲▼△▽⊿◤ ◥▆ ▇ █ █ ■ ";
                if (rm == RenameMode.ASCII)
                    return RandomString(LeetRandom.rnd.Next(0, 20), ascii);
                else
                    return RandomString(LeetRandom.rnd.Next(0, 20), unreadable);

            }
            private static string RandomString(int length, string chars)
            {
                return new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[LeetRandom.rnd.Next(s.Length)]).ToArray());
            }
            public static bool CanRename(TypeDef type)
            {
                if (type.IsGlobalModuleType)
                    return false;
                try
                {
                    if (type.Namespace.Contains("My"))
                        return false;
                }
                catch { }

                if (type.Interfaces.Count > 0)
                    return false;
                if (type.IsSpecialName)
                    return false;
                if (type.IsRuntimeSpecialName)
                    return false;
                else
                    return true;
            }
            public static bool CanRename(MethodDef method)
            {
                if (method.IsConstructor)
                    return false;
                /* if (method.GetType().GetInterfaces().Count() > 0)
                     return false; */
                if (method.IsFamily)
                    return false;
                if (method.IsRuntimeSpecialName)
                    return false;
                if (method.DeclaringType.IsForwarder)
                    return false;
                else
                    return true;
            }
            public static bool CanRename(FieldDef field)
            {

                if (field.IsRuntimeSpecialName)
                    return false;
                if (field.IsLiteral && field.DeclaringType.IsEnum)
                    return false;

                else
                    return true;
            }
            public static bool CanRename(EventDef ev)
            {
                if (ev.IsRuntimeSpecialName)
                    return false;
                else
                    return true;
            }
        }


        public void ProtectionPhase(SpectreContext spctx)
        {
            RenameMode mode = RenameMode.UNREADABLE;
            foreach (ModuleDef module in spctx.Assembly.Modules)
            {
                foreach (TypeDef type in module.Types)
                {
                    if (Utils.CanRename(type))
                        type.Name = Utils.GenerateName(mode);

                    foreach (MethodDef method in type.Methods)
                    {
                        if (Utils.CanRename(method))
                            method.Name = Utils.GenerateName(mode);

                        foreach (var param in method.Parameters)
                            param.Name = Utils.GenerateName(mode);
                    }
                    foreach (FieldDef field in type.Fields)
                        if (Utils.CanRename(field))
                            field.Name = Utils.GenerateName(mode);

                    foreach (EventDef eventf in type.Events)
                        if (Utils.CanRename(eventf))
                            eventf.Name = Utils.GenerateName(mode);

                }
            }
        }
    }
}
