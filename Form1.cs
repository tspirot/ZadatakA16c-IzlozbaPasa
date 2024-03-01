using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZadatakA16c
{
    public partial class Form1 : Form
    {
        SqlConnection konekcija;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                konekcija = new SqlConnection
                (@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\A16.mdf;Integrated Security=True");
                osveziPse();
                osveziIzlozbe1();
                osveziIzlozbe2();
                osveziKategorije();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greska: " + ex.Message);
            }
        }
        private void osveziPse()
        {
            try
            {
                SqlCommand komanda = new SqlCommand
                    ("SELECT PasID, CONCAT(PasID,' - ',Ime) AS ImePsa  " +
                    "FROM Pas",
                    konekcija);
                SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                DataTable tabela = new DataTable();
                adapter.Fill(tabela);
                comboBoxPas.Items.Clear();
                comboBoxPas.DataSource = tabela;
                comboBoxPas.DisplayMember = "ImePsa";
                comboBoxPas.ValueMember = "PasID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greska: " + ex.Message);
            }
        }
        private void osveziIzlozbe1() // u buducnosti
        {
            try
            {
                SqlCommand komanda = new SqlCommand
                    ("SELECT IzlozbaID, " +
                    "CONCAT(IzlozbaID,' - ',Mesto,' - ',Datum) AS ImeIzlozbe  " +
                    "FROM Izlozba " +
                    "WHERE Datum>=GETDATE()",
                    konekcija);
                SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                DataTable tabela = new DataTable();
                adapter.Fill(tabela);
                comboBoxIzlozba.DataSource = tabela;
                comboBoxIzlozba.DisplayMember = "ImeIzlozbe";
                comboBoxIzlozba.ValueMember = "IzlozbaID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greska: " + ex.Message);
            }
        }
        private void osveziIzlozbe2()
        {
            try
            {
                SqlCommand komanda = new SqlCommand
                    ("SELECT IzlozbaID, " +
                    "CONCAT(IzlozbaID,' - ',Mesto,' - ',Datum) AS ImeIzlozbe  " +
                    "FROM Izlozba " +
                    "WHERE Datum<=GETDATE()",
                    konekcija);
                SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                DataTable tabela = new DataTable();
                adapter.Fill(tabela);
                comboBox1.DataSource = tabela;
                comboBox1.DisplayMember = "ImeIzlozbe";
                comboBox1.ValueMember = "IzlozbaID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greska: " + ex.Message);
            }
        }

        private void osveziKategorije()
        {
            try
            {
                SqlCommand komanda = new SqlCommand
                    ("SELECT KategorijaID, CONCAT(KategorijaID,' - ',Naziv) " +
                    "AS ImeKategorije  " +
                    "FROM Kategorija",
                    konekcija);
                SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                DataTable tabela = new DataTable();
                adapter.Fill(tabela);
                comboBoxKategorija.Items.Clear();
                comboBoxKategorija.DataSource = tabela;
                comboBoxKategorija.DisplayMember = "ImeKategorije";
                comboBoxKategorija.ValueMember = "KategorijaID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greska: " + ex.Message);
            }
        }

        private void buttonPrijava_Click(object sender, EventArgs e)
        {
            DataTable tabela = new DataTable();
            try
            {
                SqlCommand provera = new SqlCommand
                    ("SELECT * FROM Rezultat " +
                    "WHERE PasID=@PasID " +
                    "AND IzlozbaID=@IzlozbaID " +
                    "AND KategorijaID=@KategorijaID",
                     konekcija);
                provera.Parameters.AddWithValue
                    ("@PasID", comboBoxPas.SelectedValue);
                provera.Parameters.AddWithValue
                    ("@IzlozbaID", comboBoxIzlozba.SelectedValue);
                provera.Parameters.AddWithValue
                    ("@KategorijaID", comboBoxKategorija.SelectedValue);
                SqlDataAdapter adapter = new SqlDataAdapter(provera);
                adapter.Fill(tabela);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greska: " + ex.Message);
                return;
            }
            // provera da li je pas vec prijavljen
            if (tabela.Rows.Count > 0)
            {
                MessageBox.Show("Pas je vec prijavljen!");
                return;
            }
            // pas nije prijavljen
            try
            {
                SqlCommand sqlPrijava = new SqlCommand
                    ("INSERT INTO Rezultat " +
                     "(PasID,IzlozbaID,KategorijaID) " +
                     "VALUES (@PasID,@IzlozbaID,@KategorijaID)",
                     konekcija);
                sqlPrijava.Parameters.AddWithValue
                    ("@PasID", comboBoxPas.SelectedValue);
                sqlPrijava.Parameters.AddWithValue
                    ("@IzlozbaID", comboBoxIzlozba.SelectedValue);
                sqlPrijava.Parameters.AddWithValue
                    ("@KategorijaID", comboBoxKategorija.SelectedValue);
                konekcija.Open();
                sqlPrijava.ExecuteNonQuery();
                MessageBox.Show("Pas je uspesno prijavljen!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greska pri upisu: " + ex.Message);
            }
            finally
            {
                konekcija.Close();
            }
        }

        private void buttonPrikazi_Click(object sender, EventArgs e)
        {
            string upit = "SELECT Kategorija.KategorijaID AS Sifra, " +
                "Kategorija.Naziv AS NazivKategorije, " +
                "COUNT(*) AS BrojPasa " +
                "FROM Kategorija, Rezultat " +
                "WHERE Kategorija.KategorijaID=Rezultat.KategorijaID " +
                "AND Rezultat.IzlozbaID=@IzlozbaID " +
                "AND LEN(Rezultat.Napomena)>0 " +
                "GROUP BY Kategorija.KategorijaID, Kategorija.Naziv";
            try
            {
                SqlCommand komanda = new SqlCommand(upit, konekcija);
                komanda.Parameters.AddWithValue
                    ("@IzlozbaID", comboBox1.SelectedValue);
                SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                DataTable tabela = new DataTable();
                adapter.Fill(tabela);
                dataGridView1.DataSource = tabela;
                chart1.DataSource = tabela;
                chart1.Series[0].XValueMember = "NazivKategorije";
                chart1.Series[0].YValueMembers = "BrojPasa";
                chart1.Series[0].IsValueShownAsLabel = true;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greska: " + ex.Message);
            }
            // broja prijavljenih pasa
            string upitPrijavljeno= "SELECT COUNT(*) " +
                "FROM Rezultat " +
                "WHERE IzlozbaID=@IzlozbaID";
            string upitTakmicilo= "SELECT COUNT(*) " +
                "FROM Rezultat " +
                "WHERE IzlozbaID=@IzlozbaID " +
                "AND LEN(Napomena)>0";
            try
            {
                SqlCommand komandaPrijavljeno = new SqlCommand(upitPrijavljeno, konekcija);
                komandaPrijavljeno.Parameters.AddWithValue
                    ("@IzlozbaID", comboBox1.SelectedValue);
                SqlCommand komandaTakmicilo = new SqlCommand(upitTakmicilo, konekcija);
                komandaTakmicilo.Parameters.AddWithValue
                    ("@IzlozbaID", comboBox1.SelectedValue);
                konekcija.Open();
                int prijavljeno = (int)komandaPrijavljeno.ExecuteScalar();
                int takmicilo = (int)komandaTakmicilo.ExecuteScalar();
                labelPrijavljeno.Text = "Ukupan broj pasa koji je prijavljen " 
                    + prijavljeno;
                labelUcestvovalo.Text = "Ukupan broj pasa koji se takmicio " +
                    komandaTakmicilo.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greska: " + ex.Message);
            }
            finally
            {
                konekcija.Close();
            }

        }
    }
}
