using dnlib.DotNet;
using dnlib.DotNet.Writer;
using Obfuscator.Internal;
using Obfuscator.Internal.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Obfuscator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lvassemblys.Items.RemoveAt(0);


        }
        #region Rules
        private bool enable_antiildasm = false;
        private bool enable_constantprotection = false;
        private bool enable_constantmutation = false;
        private bool enable_renamer = false;
        #endregion
        private BrushConverter bc = new BrushConverter();
        private List<string> assemblys = new List<string>();

        
        private void btnObf_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in lvassemblys.Items)
            {
                var current = item as LvAssembly;
              
                AssemblyDef asm = AssemblyDef.Load(current.Path);
                SpectreContext spctx = new SpectreContext(asm);
                var rule = new Rule(enable_renamer, enable_antiildasm, enable_constantprotection, enable_constantmutation);
                new Core(spctx,rule).DoObfuscation();
                var opts = new ModuleWriterOptions(spctx.ManifestModule);
                opts.Logger = DummyLogger.NoThrowInstance;
                asm.Write(current.Path + "_obf.exe", opts);

            }
        }

        private void lvassemblys_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] droppedFilePaths = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                foreach (var path in droppedFilePaths)
                {
                    if (File.Exists(path)) this.lvassemblys.Items.Add(new LvAssembly { Path = path }); else continue;
                }
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                btnDel.IsEnabled = true;
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            lvassemblys.Items.Remove(lvassemblys.SelectedItem);
            btnDel.IsEnabled = false;
        }

        private void btnAntiILDasm_Click(object sender, RoutedEventArgs e)
        {
            if (btnAntiILDasm.Background == Brushes.Orange)
            {
                btnAntiILDasm.Background = (Brush)bc.ConvertFrom("#FF252424");
                enable_antiildasm = false;
            }
            else
            {
                btnAntiILDasm.Background = Brushes.Orange;
                enable_antiildasm = true;
            }
        }

        private void btnEnableConstantProtection_Click(object sender, RoutedEventArgs e)
        {
            if (btnEnableConstantProtection.Background == Brushes.Orange)
            {
                btnEnableConstantProtection.Background = (Brush)bc.ConvertFrom("#FF252424");
                enable_constantprotection = false;
            }
            else
            {
                btnEnableConstantProtection.Background = Brushes.Orange;
                enable_constantprotection = true;
            }
        }
        private void btnEnableConstantMutation_Click(object sender, RoutedEventArgs e)
        {
            if (btnEnableConstantMutation.Background == Brushes.Orange)
            {
                btnEnableConstantMutation.Background = (Brush)bc.ConvertFrom("#FF252424");
                enable_constantmutation = false;
            }
            else
            {
                btnEnableConstantMutation.Background = Brushes.Orange;
                enable_constantmutation = true;
            }
        }

        private void btnRenamer_Click(object sender, RoutedEventArgs e)
        {
            if (btnRenamer.Background == Brushes.Orange)
            {
                btnRenamer.Background = (Brush)bc.ConvertFrom("#FF252424");
                enable_renamer = false;
            }
            else
            {
                btnRenamer.Background = Brushes.Orange;
                enable_renamer = true;
            }
        }
    }
}
