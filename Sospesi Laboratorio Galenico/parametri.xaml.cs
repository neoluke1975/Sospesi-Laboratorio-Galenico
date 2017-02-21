using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Sospesi_Laboratorio_Galenico
{
    /// <summary>
    /// Logica di interazione per parametri.xaml
    /// </summary>
    public partial class parametri : Window
    {
        public parametri()
        {
            InitializeComponent();
        }

        private void btnSalva_Click(object sender, RoutedEventArgs e)
        {
             Sospesi_Laboratorio_Galenico.Properties.Settings.Default.server= tbxPercorsoServer.Text;
             Sospesi_Laboratorio_Galenico.Properties.Settings.Default.database=tbxPercorsoDatabase.Text;
             Sospesi_Laboratorio_Galenico.Properties.Settings.Default.Save();
             this.Close();

        }

        private void btnSalva_Loaded(object sender, RoutedEventArgs e)
        {
            tbxPercorsoServer.Text = Sospesi_Laboratorio_Galenico.Properties.Settings.Default.server;
            tbxPercorsoDatabase.Text = Sospesi_Laboratorio_Galenico.Properties.Settings.Default.database;
        }
    }
}
