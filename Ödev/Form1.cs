using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace Ödev
{
    public partial class Form1 : Form
    {

        OleDbConnection bağlantı = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0;Data Source=hasta_bilgisi.mdb");
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add("id","ID");
            dataGridView1.Columns.Add("isim","İsim");
            dataGridView1.Columns.Add("dtarihi","Dtarihi");
            dataGridView1.Columns.Add("dyeri","Dyeri");
            dataGridView1.Columns.Add("kangrubu","Kangrubu");
            dataGridView1.Columns.Add("cinsiyet","Cinsiyet");
            dataGridView1.Columns.Add("adres","Adres");
            dataGridView1.Columns.Add("tel","Tel");

            bağlantı.Open();

            OleDbCommand komut = new OleDbCommand("select hasta_id,isim,d_tarihi,d_yeri,kan_grubu,cinsiyet,adres,tel from" +
                " tbl_Hasta where isim LIKE 'A%'", bağlantı);
            OleDbDataReader oku = komut.ExecuteReader();

            while (oku.Read())
            {
                dataGridView1.Rows.Add(oku["hasta_id"], oku["isim"], oku["d_tarihi"], oku["d_yeri"], oku["kan_grubu"], oku["cinsiyet"],
                    oku["adres"], oku["tel"]);
            }

            bağlantı.Close();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            ArrayList harfler = new ArrayList();

            bağlantı.Open();
            
            string isim = tabControl1.SelectedTab.Text;
            foreach(char harf in isim)
            {
                if (char.IsLetter(harf))
                {
                    harfler.Add(harf);
                }
                
            }
            for(int i = 0; i < harfler.Count; i++)
            {
                string sorgu = "SELECT hasta_id,isim,d_tarihi,d_yeri,kan_grubu,cinsiyet,adres,tel  FROM tbl_Hasta WHERE isim LIKE '" + harfler[i] + "%'";
                OleDbCommand komut = new OleDbCommand(sorgu, bağlantı);
                OleDbDataReader oku = komut.ExecuteReader();


                while (oku.Read())
                {
                    dataGridView1.Rows.Add(oku["hasta_id"], oku["isim"], oku["d_tarihi"], oku["d_yeri"], oku["kan_grubu"], oku["cinsiyet"],
                        oku["adres"], oku["tel"]);
                }


            }



            bağlantı.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int sütun, satır;
            object hücre;
            sütun = e.ColumnIndex;
            satır = e.RowIndex;
            hücre = dataGridView1[sütun, satır].Value;
           


            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {

                switch (i)
                {
                    case 0: string kelime = dataGridView1[i,satır].Value.ToString();
                        label8.Text = kelime;  break;

                    case 1: 
                         kelime = dataGridView1[i, satır].Value.ToString();
                        textBox1.Text=kelime; break;

                    case 2:
                         kelime = dataGridView1[i, satır].Value.ToString();
                        textBox2.Text=kelime; break;

                    case 3:
                         kelime = dataGridView1[i, satır].Value.ToString();
                        comboBox1.Text=kelime; break;

                    case 4:
                        kelime = dataGridView1[i, satır].Value.ToString();
                        comboBox2.Text=kelime; break;

                    case 5:
                        kelime = dataGridView1[i, satır].Value.ToString();

                        if (kelime == radioButton1.Text)
                        {
                            radioButton1.Checked = true;
                        }

                        else
                        {
                            radioButton2.Checked = true;
                        }
                        
                        break;

                    case 6:
                        kelime = dataGridView1[i, satır].Value.ToString();
                        richTextBox1.Text=kelime; break;


                    case 7:
                        kelime = dataGridView1[i, satır].Value.ToString();
                        textBox4.Text=kelime; break;

                        default: break;

                }
                    
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            richTextBox1.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            comboBox1.Text="";
            comboBox2.Text = "";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cinsiyet;
            bağlantı.Open();
            
            if(radioButton1.Checked == false){

                cinsiyet = "K";
            }

            else
            {
                cinsiyet = "E";
            }

            OleDbCommand ekle_komut=new OleDbCommand("insert into tbl_Hasta(isim,d_tarihi,d_yeri,kan_grubu,cinsiyet,adres,tel)" +
                " values('"+textBox1.Text+"','"+textBox2.Text+"','"+comboBox1.Text+"','"+comboBox2.Text+"','"+cinsiyet+"'," +
                "'"+richTextBox1.Text+"','"+textBox4.Text+"')",bağlantı);


            MessageBox.Show(ekle_komut.ExecuteNonQuery()+"kişi eklendi");



            bağlantı.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string cinsiyet;
            int comboo = comboBox1.SelectedIndex + 1;
            bağlantı.Open();
            if (radioButton1.Checked == false)
            {

                cinsiyet = "K";
            }

            else
            {
                cinsiyet = "E";
            }
            OleDbCommand güncelle_komut = new OleDbCommand("update tbl_Hasta set isim='"+textBox1.Text+"',d_tarihi='"+textBox2.Text+"',d_yeri='"+comboBox1.Text+"'," +
                "kan_grubu='"+comboBox2.Text+"',cinsiyet='"+cinsiyet+"',adres='"+richTextBox1.Text+"',tel='"+textBox4.Text+"' where hasta_id="+label8.Text,bağlantı);
            MessageBox.Show(güncelle_komut.ExecuteNonQuery()+"hasta güncellendi");
            bağlantı.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bağlantı.Open();
            OleDbCommand sil_komutt = new OleDbCommand("delete from tbl_Randevu where randevu_hasta_id = " + label8.Text, bağlantı);
            MessageBox.Show(sil_komutt.ExecuteNonQuery() + "randevu silindi");


            OleDbCommand sil_komut = new OleDbCommand("delete from tbl_Hasta where hasta_id="+label8.Text,bağlantı);
            
            MessageBox.Show(sil_komut.ExecuteNonQuery() + "hasta silindi");
            
            bağlantı.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.textBox6.Text = label8.Text;
            form2.textBox1.Text=textBox1.Text;
            form2.Show();
            this.Hide();


           



            }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
            
        }
    }
}
