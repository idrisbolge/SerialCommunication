using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace ProjectNo1
{
    public partial class Wireless : Form
    {
        public Wireless()
        {
            InitializeComponent();
        }

        //Firebase Bilgileri 
        IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "nhvKyN6uaGwRuOrV5vmzHAIbFBPyfnwvYTAtNG9H",
            BasePath = "https://wificommunication-fe74c.firebaseio.com/"
        };
        //Client Tanımlama
        IFirebaseClient client;

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }

        //Firebase cevabı
        FirebaseResponse response;
       static bool state;
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            //State bağlantı durumunu kontrol etmek için kullanılan bool yapısı
            state = true;
            //Client başlatma
            client = new FireSharp.FirebaseClient(Config);
            if (client != null)
            {
                MessageBox.Show("Bağlantı işlemi başarılı");
            }
            //state true olduğu sürece Firebaseden her an veri alımı yapılır
            //Veriler Data internal classı yardımıyla çekilir 
            //Alınan veri charta yazılır
            while (state)
            {
                FirebaseResponse response = await client.GetTaskAsync("");
                Data obj = response.ResultAs<Data>();
                chart1.Series[0].Points.AddY(obj.number);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            client = null;
            state = false;
        }

        private void Wireless_Load(object sender, EventArgs e)
        {

        }
        private Bitmap Snapshot()
        {
            // bitmap nesnesi oluştur
            Bitmap Screenshot = new Bitmap(this.Width, this.Height);

            // bitmapten grafik nesnesi oluştur
            Graphics GFX = Graphics.FromImage(Screenshot);

            // ekrandan programın bulunduğu konumun resmini alalım
            GFX.CopyFromScreen(this.Right - chart1.Right, this.Bottom - chart1.Bottom, 0, 0, chart1.Size);
            return Screenshot;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Snapshot().Save("C:\\Users\\İdris Bölge\\Pictures\\Program\\program_goruntusu.jpg");
            MessageBox.Show("Ekran Görüntüsü alındı.");
        }
    }
}
