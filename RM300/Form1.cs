using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ThingMagic;


namespace RM300
{
    public partial class Form1 : Form
    {
        public Reader objReader = null;

        public Form1()
        {
            InitializeComponent();

            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                CmbSerialPort.Items.Add(port);
            }
        }

        

        public void SerialPortRead()
        {
            SerialPort SerialPort1 = new SerialPort();

            SerialPort1.BaudRate = 115200;
            SerialPort1.PortName = CmbSerialPort.SelectedItem.ToString();

            try
            {
                SerialPort1.Open();
                MessageBox.Show("Connect Successful!");
                MessageBox.Show(SerialPort1.PortName + "," + SerialPort1.Parity +","+SerialPort1.BaudRate + ","+ SerialPort1.StopBits);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
        private void btnConn_Click(object sender, EventArgs e)
        {
            SerialPortRead();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                int TotalTagCount = 0;
                btnRead.Enabled = false;
                TagReadData[] tagID = objReader.Read(int.Parse(tbxReadTimeout.Text));
                arr = new ReadTags[tagID.Length];
                for (int i = 0; i < tagID.Length; i++)
                {
                    arr[i] = new ReadTags(tagID[i]);
                    TotalTagCount = TotalTagCount + tagID[i].ReadCount;
                }
                dataGrid1.DataSource = arr;
                generatedatagrid();
                lblTotalTagCount.Text = TotalTagCount.ToString();
                lblUniqueTagCount.Text = tagID.Length.ToString();
                btnRead.Enabled = true;
                btnRead.Focus();

            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Error");
                objReader.Destroy();
                objReader = null;
                btnRead.Enabled = false;
                btnConnect.Text = "Connect";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Reader Message");
                btnRead.Enabled = true;
            }
        }

        public class ReadTags
        {
            private string epc;
            private string timestamp;
            private string readcount;
            public string rssi;

            public ReadTags()
            {
            }
            public ReadTags(TagReadData adddata)
            {
                epc = adddata.EpcString;
                timestamp = adddata.Time.ToString();
                readcount = adddata.ReadCount.ToString();
                rssi = adddata.Rssi.ToString();
            }
            public string EPC
            {
                get { return epc; }
            }
            public string Timestamp
            {
                get { return timestamp; }
            }
            public string ReadCount
            {
                get { return readcount; }
            }
            public string Rssi
            {
                get { return rssi; }
            }

        }
    }
}
