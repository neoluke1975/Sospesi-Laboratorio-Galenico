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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirebirdSql.Data.FirebirdClient;

namespace Sospesi_Laboratorio_Galenico
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FbConnection connessione = new FbConnection("User=SYSDBA;Password=masterkey;Database=" + Sospesi_Laboratorio_Galenico.Properties.Settings.Default.database + ";DataSource=" + Sospesi_Laboratorio_Galenico.Properties.Settings.Default.server + ";Port = 3050; Dialect = 3; Charset = NONE; Role =; Connection lifetime = 15; Pooling = true;MinPoolSize = 0; MaxPoolSize = 50; Packet Size = 8192; ServerType = 0; ");
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

           

            FbDataReader lettore = null;
            if (e.Key == Key.Enter)
            {
                try
                {
                    tbxCodiceSospeso.Text = tbxCodiceSospeso.Text.Trim('S');
                    connessione.Open();
                    FbCommand query = new FbCommand("select s.NUM_PROG,(select a.DESKEY from anaforn a where a.CODKEY=s.COD_CLI),s.NOTE_SMS,s.NOTE,s.QT_PERVENUTA from sospesi s where s.num_prog=" +int.Parse(tbxCodiceSospeso.Text) + "", connessione);
                    lettore = query.ExecuteReader();
                    while (lettore.Read())
                    {
                        tbxCliente.Text = lettore[1].ToString();
                        tbxSms.Text = lettore[2].ToString();
                        tbxNote.Text = lettore[3].ToString();
                    }
                    tbxPrezzo.Focus();
                   
                }
                catch (Exception)
                {

                    MessageBox.Show("SOSPESO NON TROVATO!!!");
                }



                lettore.Close();
                connessione.Close();





            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            parametri parametri = new parametri();
            parametri.Show();
                

        }

        private void wind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F9)
            {
                btnParametri.IsEnabled = true;
            }
        }

        private void btnSalva_Loaded(object sender, RoutedEventArgs e)
        {
            tbxCodiceSospeso.Focus();
            tbxCodiceSospeso.Focusable = true;
        }

        private void btnSalva_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                
                connessione.Open();
                FbCommand update_prezzo = new FbCommand("update lg_righe r "+
                                                        "set r.E_PREZZO_MOV ="+(tbxPrezzo.Text.Replace(',','.'))+
                                                        ", r.E_PREZZO_PUB ="+(tbxPrezzo.Text.Replace(',', '.')) +
                                                        ", r.E_TOT_LIBERA =" + (tbxPrezzo.Text.Replace(',', '.')) +
                                                        " where r.NUM_PROG = (select s.NUM_GIORNALE from SOSPESI s where s.NUM_GIORNALE = r.NUM_PROG and s.NUM_PROG ="+int.Parse(tbxCodiceSospeso.Text)+") and r.SOSPESO = 'S' ", connessione);
                update_prezzo.ExecuteNonQuery();

                FbCommand update_qt_pervenuta = new FbCommand("update sospesi s "+
                                                              "set s.QT_PERVENUTA = (select r.QUANT from LG_RIGHE r where r.NUM_PROG = s.NUM_GIORNALE and r.SOSPESO = 'S')"+
                                                              " where s.NUM_PROG ="+int.Parse(tbxCodiceSospeso.Text)+"", connessione);
                update_qt_pervenuta.ExecuteNonQuery();

                FbCommand update_detrazione = new FbCommand("update lg_righe r " +
                                                    "set r.E_PREZZO_MOV =" + ((float.Parse(tbxPrezzo.Text.Replace('.', ','))  - (float.Parse(tbxPrezzo.Text.Replace('.', ',')) *2 ))).ToString().Replace(',','.') +
                                                    ", r.E_PREZZO_PUB =" + ((float.Parse(tbxPrezzo.Text.Replace('.', ',')) - (float.Parse(tbxPrezzo.Text.Replace('.', ',')) * 2))).ToString().Replace(',', '.') +
                                                    ", r.E_TOT_LIBERA =" + ((float.Parse(tbxPrezzo.Text.Replace('.', ',')) - (float.Parse(tbxPrezzo.Text.Replace('.', ',')) * 2))).ToString().Replace(',', '.') +
                                                    " where r.NUM_PROG = (select s.NUM_GIORNALE from SOSPESI s where s.NUM_GIORNALE = r.NUM_PROG and s.NUM_PROG =" + int.Parse(tbxCodiceSospeso.Text) + ") and r.KDES='Detrazione Sps.' ", connessione);

                update_detrazione.ExecuteNonQuery();
            }
            catch (Exception)
            {

                MessageBox.Show("Aggiornamento non trovato");
            }

            connessione.Close();
            try
            {
                tbxCodiceSospeso.Text = "";
                tbxCliente.Text = "";
                tbxNote.Text = "";
                tbxPrezzo.Text = "";
                tbxSms.Text = "";
                tbxCodiceSospeso.Focus();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void tbxPrezzo_KeyDown(object sender, KeyEventArgs e)
        {
           

              if (e.Key == Key.Enter)
            {
                btnSalva.Focus();
            }


        }

        private void tbxPrezzo_KeyUp(object sender, KeyEventArgs e)
        {
            funzione(e);
           
        }

        private void funzione(KeyEventArgs e)
        {
            
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.OemComma))
            {

            }
            else if (e.Key == Key.Decimal)
            {

            }
           
            else
            {
                tbxPrezzo.Text = "";
            }
            
        }
    }  
}
    

