using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProjectNo1
{
    public partial class Cable : Form
    {
        public Cable()
        {
            InitializeComponent();
            //Chart yükleme sorununa karşılık eklenmelidir
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        // Combobox'a aktif bağlantıları listeler
        private void PortYaz()
        {
            comboBox1.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
                comboBox1.Items.Add(port);

        }
        private void Cable_Load(object sender, EventArgs e)
        {
            PortYaz();
            //Combobox'ta varsayılan seçenekleri seçme
            if (comboBox1.Items.Count != 0)
                comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 4;

            //Chart yakınlaştırmak uzaklaştırmak için kullanılır. 
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.MouseWheel += chart1_MouseWheel;

        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            PortYaz();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //Bağlantı başlatılırken grafikte değer varsa silinir sıfırdan başlatılır.
            if (chart1.Series.Count == 1)
            {
                //Port ve Baudrate alınarak Seri port açılır.
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = int.Parse(comboBox2.Text);
                serialPort1.Open();
            }

            else
                chart1.Series.Clear();

        }
        string result;
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Seri portttan bir karakter okunur
            var data = serialPort1.ReadChar();
            //Durdurma işleçlerinden biri değilse result stringine çözülerek eklenir.
            if (data != 13 && data != 10)
                result += Convert.ToChar(data);
            else
            {
                //eğer durdurma işleci gelirse ve result boş değilse grafiğe yazılıp result sıfırlanır.
                if (result != null)
                    chart1.Series[0].Points.AddY(result);
                Console.WriteLine(result);
                result = null;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void serialPort1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("Veri aktarımı sağlanamıyor");
        }

        private void temizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
        }

        //Zoom işlemlerini yapar
        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;

            try
            {
                if (e.Delta < 0) // Scrolled down.
                {
                    xAxis.ScaleView.ZoomReset();
                    yAxis.ScaleView.ZoomReset();
                }
                else if (e.Delta > 0) // Scrolled up.
                {
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;
                    var yMin = yAxis.ScaleView.ViewMinimum;
                    var yMax = yAxis.ScaleView.ViewMaximum;

                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;
                    var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 4;
                    var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 4;

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    yAxis.ScaleView.Zoom(posYStart, posYFinish);
                }
            }
            catch { }
        }

        //Ekran Görüntüsü alma
        private Bitmap Snapshot()
        {
            // bitmap nesnesi oluştur
            Bitmap Screenshot = new Bitmap(this.Width, this.Height);

            // bitmapten grafik nesnesi oluştur
            Graphics GFX = Graphics.FromImage(Screenshot);

            // ekrandan programın bulunduğu konumun resmini alalım
            GFX.CopyFromScreen(this.Right-chart1.Right, this.Bottom-chart1.Bottom, 0,0, chart1.Size);
            return Screenshot;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Snapshot().Save("C:\\Users\\İdris Bölge\\Pictures\\Program\\program_goruntusu.jpg");
            MessageBox.Show("Ekran Görüntüsü alındı.");

        }

      
    }
}

