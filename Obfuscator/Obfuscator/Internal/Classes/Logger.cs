using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Obfuscator.Internal.Classes
{
    class Logger
    {
        public static void Log(string msg)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).rtlog.AppendText(msg);
                }
            }
        }
    }
}
