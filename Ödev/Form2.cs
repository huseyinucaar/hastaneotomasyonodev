using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.OleDb;

namespace Ödev
{
    public partial class Form2 : Form
    {
        OleDbConnection bağlantı = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0;Data Source=hasta_bilgisi.mdb");
        public Form2()
        {
            InitializeComponent();
        }



        private void Form2_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add("randevu_id", "Randevu_id");
            dataGridView1.Columns.Add("hasta_isim", "Hasta_isim");
            dataGridView1.Columns.Add("randevu_tarihi", "Randevu_tarihi");
            dataGridView1.Columns.Add("teşhis", "Teşhis");
            dataGridView1.Columns.Add("tedavi", "Tedavi");
            dataGridView1.Columns.Add("verilen_ilaç", "Verilen_ilaç");


            bağlantı.Open();

            OleDbCommand komut = new OleDbCommand("SELECT tbl_Randevu.randevu_id, tbl_Hasta.isim, tbl_Randevu.randevu_tarihi, tbl_Randevu.teşhis, tbl_Randevu.tedavi," +
                " tbl_İlaç.ilaç_adı FROM tbl_İlaç INNER JOIN (tbl_Hasta INNER JOIN tbl_Randevu ON tbl_Hasta.hasta_id = tbl_Randevu.randevu_hasta_id) ON tbl_İlaç.ilaç_id" +
                " = tbl_Randevu.verilen_ilaç_id where randevu_hasta_id=" + textBox6.Text, bağlantı);
            OleDbDataReader oku = komut.ExecuteReader();

            while (oku.Read())
            {
                dataGridView1.Rows.Add(oku["randevu_id"], oku["isim"], oku["randevu_tarihi"], oku["teşhis"], oku["tedavi"], oku["ilaç_adı"]);
            }

            bağlantı.Close();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bağlantı.Open();
            int sütun, satır;
            object hücre;
            sütun = e.ColumnIndex;
            satır = e.RowIndex;
            hücre = dataGridView1[sütun, satır].Value;

            OleDbCommand komut = new OleDbCommand("select isim from tbl_Hasta where hasta_id=" + textBox6.Text, bağlantı);
            OleDbDataReader oku = komut.ExecuteReader();

            while(oku.Read())
            {
                textBox1.Text = oku["isim"].ToString();
            }
            
            


            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {

                switch (i)
                {
                    case 0:
                        string kelime = dataGridView1[i, satır].Value.ToString();
                        label5.Text = kelime; break;

                    

                    case 2:
                        kelime = dataGridView1[i, satır].Value.ToString();
                        dateTimePicker1.Text = kelime;
                        textBox4.Text = kelime; break ;

                    case 3:
                        kelime = dataGridView1[i, satır].Value.ToString();
                        textBox2.Text = kelime; break;

                    case 4:
                        kelime = dataGridView1[i, satır].Value.ToString();
                        textBox3.Text = kelime; break;
                        break;

                    case 5:
                        kelime = dataGridView1[i, satır].Value.ToString();

                        comboBox1.Text = kelime;

                        break;



                    default: break;

                }
                bağlantı.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
            dateTimePicker1.Text = "";
            comboBox1.Text = "";
            textBox2.Clear();
            textBox3.Clear();
            label5.Text = "randevu_id";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bağlantı.Open();

            int ilacc = comboBox1.SelectedIndex + 1;

            OleDbCommand ekle_komut = new OleDbCommand("insert into tbl_Randevu(randevu_hasta_id,teşhis,tedavi,randevu_tarihi,verilen_ilaç_id)" +
                " values(" + textBox6.Text + ",'" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "'," +ilacc  + ")", bağlantı);


            MessageBox.Show(ekle_komut.ExecuteNonQuery() + "randevu eklendi");



            bağlantı.Close(); 
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            textBox4.Text= dateTimePicker1.Value.ToShortDateString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bağlantı.Open();
            int ilacc = comboBox1.SelectedIndex + 1;
            OleDbCommand güncelle_komut = new OleDbCommand("update tbl_Randevu set teşhis='" + textBox2.Text + "',tedavi='" + textBox3.Text + "',randevu_tarihi=" +
                "'" + textBox4.Text + "',verilen_ilaç_id=" + ilacc + " where randevu_id=" + label5.Text, bağlantı);
            MessageBox.Show(güncelle_komut.ExecuteNonQuery() + "randevu güncellendi");
            bağlantı.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bağlantı.Open();
            OleDbCommand sil_komutt = new OleDbCommand("delete from tbl_Randevu where randevu_id = " + label5.Text, bağlantı);
            MessageBox.Show(sil_komutt.ExecuteNonQuery() + "randevu silindi");


            

            bağlantı.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}