using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace EasyCardProject
{
                                             //SignOn,  購貨,    現金加值,加值取消,購貨取消,退貨,  查詢交易, 餘額查詢 結帳
  //ProcessingCode : Array[0..8] of String = ('881999','811599','811799','811899','823899','851999','999999','200000','900000'); //20151119 modi by 02262
    public partial class Form1 : Form
    {
        EZRs232 ezRs232 = new EZRs232();
        public short cntOfRetrySendMsg;
        delegate void Display(byte[] xinfo);
        //delegate void Display(string xinfo);
        const Int32 STX = 0x02;
        const Int32 ETX = 0x03;
        const Int32 ACK = 0x06;
        const Int32 NAK = 0x15;

        public Form1()
        {
            InitializeComponent();
        }

        private void showComport()
        {
            comboBox1.Items.Clear();
            comboBox1.Text = "請選擇ComPort";
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            showComport();
            msg.Text = "";

            
            if (ezRs232.serialPort.IsOpen == true)
            {
                MessageBox.Show("comport已開啟");
                return;
            }

            ezRs232.serialPort.PortName = comboBox1.Text;
            ezRs232.serialPort.BaudRate = 9600*12;
            ezRs232.serialPort.DataBits = 8;
            ezRs232.serialPort.Parity = Parity.None;
            ezRs232.serialPort.StopBits = StopBits.One;
            ezRs232.serialPort.ReadTimeout = 20000;
            ezRs232.ackTimeout = 20000;
            ezRs232.cntOfRetrySendMsg = 3;
            ezRs232.sendMsg = "";
            ezRs232.recvMsg = "";

            

            /*for(int i=0; i<richTextBox2.Lines.Length; i++)
            {
               //ezRs232.sendMsg+=listBox2.GetItemText(i);
                ezRs232.sendMsg+=richTextBox2.Lines[i].ToString();
            }
            ezRs232.run();*/
        }

        public bool OnlyHexInString(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }
        public string ReplaceLowOrderASCIICharacters(string tmp)
        {
            StringBuilder info = new StringBuilder();
            foreach (char cc in tmp)
            {
                int ss = (int)cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                    info.AppendFormat(" ", ss);//&#x{0:X};
                else info.Append(cc);
            }
            return info.ToString();
        }
        private void port_DataReceived1(object sender, SerialDataReceivedEventArgs e)
        {
            /*try
            {*/
                //ezRs232 .Encoding = System.Text.Encoding.Default;
                
                //string response = serialPort1.ReadExisting();
                /*richTextBox1.Invoke(new EventHandler(delegate
                {
                    richTextBox1.AppendText(response + "\n");
                }));*/

                

                /*int length = serialPort1.BytesToRead;
                byte[] buf = new byte[length];

                serialPort1.Read(buf, 0, length);*/
                //System.Diagnostics.Debug.WriteLine("Received Data:" + buf.ToString());
              /*  Display d = new Display(DisplayText);
                this.Invoke(d, buf);*/

                /*Encoding enc8 = Encoding.Default;
                Decoder decoder8 = enc8.GetDecoder();

                int length = serialPort1.BytesToRead;
                Byte[] bytes = new Byte[2048];
                
                int nBytes = serialPort1.Read(bytes, 0, bytes.Length);

                char[] chars = new char[length];
                length = decoder8.GetChars(bytes, 0, nBytes, chars, 0);
                string output = new String(chars, 0, length);

                Display d = new Display(DisplayText);
                this.Invoke(d, output);*/

        }


        private void DisplayText(byte[] xinfo)
        //private void DisplayText(string xinfo)
        {
            //richTextBox1.AppendText(Encoding.Default.GetString(xinfo) + "\n");
            richTextBox1.AppendText(Encoding.UTF8.GetString(xinfo) + "\n");
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ezRs232.serialPort.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           /* if (!ezRs232.serialPort.IsOpen) return;
            msg.Text = "[" + (sender as Button).Text + "]";
           Byte[] buffer = new Byte[1];
            buffer[0] = 6;
            serialPort1.Write(buffer, 0, buffer.Length);
        */
            //serialPort1.Write(Char.ConvertFromUtf32(ACK));
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!ezRs232.serialPort.IsOpen) return;
            msg.Text = "[" + (sender as Button).Text + "]";
            richTextBox2.Text = "<EDC><TRANS><T0100>0110</T0100>" +
                                "<T0300>811599</T0300>" +
                                "<T0200>DigiWinXX</T0200>" +
                                "<T0212>01</T0212>" +
                                "<T0213>08</T0213>" +
                                "<T0214>00</T0214>" +
                                "<T0400>88</T0400>" +
                                "<T0408>259</T0408>" +
                                "<T0410>347</T0410>" +
                                "<T1101>008090</T1101>" +
                                "<T1200>191745</T1200>" +
                                "<T1201>191744</T1201>" +
                                "<T1300>20170322</T1300>" +
                                "<T1301>20170322</T1301>" +
                                "<T1402>20230101</T1402>" +
                                "<T3700>20170322000056</T3700>" +
                                "<T3701>170322523100208090</T3701>" +
                                "<T3800></T3800>" +
                                "<T3900>0000</T3900>" +
                                "<T4100>11100DD02C00</T4100>" +
                                "<T4101>A8D6DC00</T4101>" +
                                "<T4102>192.168.0.121</T4102>" +
                                "<T4103>000001120056922</T4103>" +
                                "<T4104>2E4EDF59</T4104>" +
                                "<T4200>00011472</T4200>" +
                                "<T4210>0</T4210>" +
                                "<T4800>00</T4800>" +
                                "<T4801>00210000A4B0CE5840F40100FC01004D000001005ED24D00000000000000000000000000000000000000000000</T4801>" +
                                "<T4802>02</T4802>" +
                                "<T4803>31</T4803>" +
                                "<T4804>01</T4804>" +
                                "<T5501>17032297</T5501>" +
                                "<T5503>0000000009</T5503>" +
                                "</TRANS>" +
                                "</EDC>";

            /*int len = richTextBox2.Text.Length + 3;
            Byte[] buffer = new Byte[len];
            buffer[0] = 2;
            for(int i=0;i<richTextBox2.Text.Length;i++)
            {
                buffer[i + 1] = (byte)richTextBox2.Text[i];
            }
            buffer[len - 2] = 3;
            buffer[len - 1] = 10;
            serialPort1.Write(buffer, 0, buffer.Length);*/
            //serialPort1.Write(Char.ConvertFromUtf32(STX) + richTextBox2.Text + Char.ConvertFromUtf32(ETX));
            ezRs232.sendMsg = richTextBox2.Text;
            ezRs232.run();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!ezRs232.serialPort.IsOpen) return;
            msg.Text = "[" + (sender as Button).Text + "]";
            richTextBox2.Text = "<EDC><TRANS><T0100>0810</T0100>" +
                                "<T0300>881999</T0300>" +
                                "<T1100>000011</T1100>" +
                                "<T1101>000010</T1101>" +
                                "<T1200>060050</T1200>" +
                                "<T1201>060053</T1201>" +
                                "<T1300>20170323</T1300>" +
                                "<T1301>20170323</T1301>" +
                                "<T3700>20170323000011</T3700>" +
                                "<T3800>LV0cub</T3800>" +
                                "<T3900>0000</T3900>" +
                                "<T4100>11100DD02C00</T4100>" +
                                "<T4101>A8D6DC00</T4101>" +
                                "<T4102>192.168.0.121</T4102>" +
                                "<T4103>000001120056922</T4103>" +
                                "<T4104>2E4EDF59</T4104>" +
                                "<T4200>00011472</T4200>" +
                                "<T4210>0</T4210>" +
                                "<T5501>17032298</T5501>" +
                                "<T5503>0000000009</T5503>" +
                                "<T5504>02</T5504>" +
                                "<T5510>0174</T5510>" +
                                "<T5588><T558801>01</T558801>" +
                                "<T558803>0000</T558803>" +
                                "</T5588>" +
                                "<T5588><T558801>02</T558801>" +
                                "<T558802></T558802>" +
                                "<T558803>04874</T558803>" +
                                "</T5588>" +
                                "<T5588><T558801>03</T558801>" +
                                "<T558802>ECCAPP</T558802>" +
                                "<T558803>000016</T558803>" +
                                "</T5588>" +
                                "<T5588><T558801>04</T558801>" +
                                "<T558803>000003</T558803>" +
                                "</T5588>" +
                                "</TRANS>" +
                                "</EDC>";

            /*int len = richTextBox2.Text.Length + 3;
            Byte[] buffer = new Byte[len];
            buffer[0] = 2;
            for (int i = 0; i < richTextBox2.Text.Length; i++)
            {
                buffer[i + 1] = (byte)richTextBox2.Text[i];
            }
            buffer[len - 2] = 3;
            buffer[len - 1] = 0;
            serialPort1.Write(buffer, 0, buffer.Length);*/
            ezRs232.sendMsg = richTextBox2.Text;
            //serialPort1.Write(Char.ConvertFromUtf32(STX) + richTextBox2.Text + Char.ConvertFromUtf32(ETX) + Char.ConvertFromUtf32(0x00));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!ezRs232.serialPort.IsOpen) return;
            /*msg.Text = "[" + (sender as Button).Text + "]";
            //找不到卡片!
            richTextBox2.Text =  "<EDC><TRANS>"+
                "<T3900>6201</T3900>"+
                "<ErrMsg>找不到卡片!!!錯誤代碼:6201</ErrMsg>"+
                "</TRANS>" +
                "</EDC>";

            byte[] bytes = Encoding.UTF8.GetBytes(richTextBox2.Text);
            int len = bytes.Length + 4;
            Byte[] buffer = new Byte[len];
            
            buffer[0] = 2;
            for (int i = 0; i < bytes.Length; i++)
            {
                buffer[i + 1] = (byte)bytes[i];
            }
            buffer[len - 2] = 3;
            buffer[len - 1] = 0;*/
            richTextBox2.Text =  "<EDC><TRANS>"+
                "<T3900>6201</T3900>"+
                "<ErrMsg>找不到卡片!!!錯誤代碼:6201</ErrMsg>"+
                "</TRANS>" +
                "</EDC>";
            //serialPort1.Write(buffer, 0, buffer.Length);
            ezRs232.sendMsg = richTextBox2.Text;
            ezRs232.run();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ezRs232.serialPort.IsOpen)
                {
                    ezRs232.serialPort.Open();
                    label2.Text = "連線狀態：已連線";
                }
                //MessageBox.Show("連線成功！");
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
                label2.Text = "連線狀態：斷線";
            }
            finally
            {
 
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ezRs232.serialPort.PortName = comboBox1.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (ezRs232.serialPort.IsOpen)
                {
                ezRs232.serialPort.Close();
                label2.Text = "連線狀態：斷線";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            /*if (!ezRs232.serialPort) return;
            if (richTextBox2.Text.Trim() == "") return;
            byte[] bytes = Encoding.UTF8.GetBytes(richTextBox2.Text);
            int len = bytes.Length + 4;
            Byte[] buffer = new Byte[len];

            buffer[0] = 2;
            for (int i = 0; i < bytes.Length; i++)
            {
                buffer[i + 1] = (byte)bytes[i];
            }
            buffer[len - 2] = 3;
            buffer[len - 1] = 0;*/
            ezRs232.sendMsg = richTextBox2.Text;
            ezRs232.run();
            //serialPort1.Write(Char.ConvertFromUtf32(STX) + richTextBox2.Text + Char.ConvertFromUtf32(ETX));
            //serialPort1.Write(buffer, 0, buffer.Length);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            msg.Text = "";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //餘額查詢
            if (!ezRs232.serialPort.IsOpen) return;
            msg.Text = "[" + (sender as Button).Text + "]";
            richTextBox2.Text = "<EDC><TRANS><T0100>0110</T0100>" +
                                "<T0300>200000</T0300>" +
                                "<T0200>DigiWinXX</T0200>" +
                                "<T0211>9122013608221781</T0211>" +
                                "<T0212>00</T0212>" +
                                "<T0213>00</T0213>" +
                                "<T0214>00</T0214>" +
                                "<T0410>120</T0410>" +
                                "<T1402>20331101</T1402>" +
                                "<T3900>0000</T3900>" +
                                "<T4802>02</T4802>" +
                                "<T4803>00</T4803>" +
                                "<T4804>01</T4804>" +
                                "<T4805>0000</T4805>" +
                                "</TRANS>" +
                                "</EDC>";

            /*int len = richTextBox2.Text.Length + 4;
            Byte[] buffer = new Byte[len];
            buffer[0] = 2;
            for (int i = 0; i < richTextBox2.Text.Length; i++)
            {
                buffer[i + 1] = (byte)richTextBox2.Text[i];
            }
            buffer[len - 2] = 3;
            buffer[len - 1] = 0;*/            
            //serialPort1.Write(buffer, 0, buffer.Length);
            ezRs232.sendMsg = richTextBox2.Text;
            ezRs232.run();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //結帳
            if (!ezRs232.serialPort.IsOpen) return;
            msg.Text = "[" + (sender as Button).Text + "]";
            richTextBox2.Text = "<EDC><TRANS><T0100>0510</T0100>" +
                                "<T0300>900000</T0300>" +
                                "<T1100>000009</T1100>" +
                                "<T1101>000000</T1101>" +
                                "<T1200>215323</T1200>" +
                                "<T1201>215324</T1201>" +
                                "<T1300>20161207</T1300>" +
                                "<T1301>161207</T1301>" +
                                "<T3700>20161207000009</T3700>" +
                                "<T3800>m166b5</T3800>" +
                                "<T3900>0000</T3900>" +
                                "<T4100>11100DD02C00</T4100>" +
                                "<T4101>A8D6DC00</T4101>" +
                                "<T4102>192.168.0.121</T4102>" +
                                "<T4103>000001120056911</T4103>" +
                                "<T4104>2B57085D</T4104>" +
                                "<T4200>00011472</T4200>" +
                                "<T4210>0</T4210>" +
                                "<T5501>16120702</T5501>" +
                                "<T5503>0000000009</T5503>" +
                                "<T5504>02</T5504>" +
                                "<T5591>000002</T5591>" +
                                "<T5592><T559201>60</T559201>" +
                                "<T559202>0</T559202>" +
                                "<T559203>0</T559203>" +
                                "</T5592>" +
                                "</TRANS>" +
                                "</EDC>";

            /*int len = richTextBox2.Text.Length + 4;
            Byte[] buffer = new Byte[len];
            buffer[0] = 2;
            for (int i = 0; i < richTextBox2.Text.Length; i++)
            {
                buffer[i + 1] = (byte)richTextBox2.Text[i];
            }
            buffer[len - 2] = 3;
            buffer[len - 1] = 0;

            serialPort1.Write(buffer, 0, buffer.Length);*/
            ezRs232.sendMsg = richTextBox2.Text;
            ezRs232.run();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //購貨取消
            if (!ezRs232.serialPort.IsOpen) return;
            msg.Text = "[" + (sender as Button).Text + "]";
            richTextBox2.Text = "<EDC><TRANS><T0100>0110</T0100>" +
                                "<T0300>823899</T0300>" +
                                "<T0200>DigiWinXX</T0200>" +
                                "<T0212>01</T0212>" +
                                "<T0213>08</T0213>" +
                                "<T0214>00</T0214>" +
                                "<T0400>30</T0400>" +
                                "<T0408>373</T0408>" +
                                "<T0410>343</T0410>" +
                                "<T1101>004361</T1101>" +
                                "<T1200>160253</T1200>" +
                                "<T1201>160147</T1201>" +
                                "<T1300>20161207</T1300>" +
                                "<T1301>161207</T1301>" +
                                "<T1402>20171101</T1402>" +
                                "<T3700>20161207000006</T3700>" +
                                "<T3701>161207523100204361</T3701>" +
                                "<T3800>Sp3vH0</T3800>" +
                                "<T3900>0000</T3900>" +
                                "<T4100>11100DD02C00</T4100>" +
                                "<T4101>A8D6DC00</T4101>" +
                                "<T4102>192.168.0.121</T4102>" +
                                "<T4103>000001120056911</T4103>" +
                                "<T4104>2B57085D</T4104>" +
                                "<T4200>00011472</T4200>" +
                                "<T4210>0</T4210>" +
                                "<T4800>00</T4800>" +
                                "<T4801>006F0000735C3B5840F401008702009C000001008BDB9C00000000000000000000000000000000000000000000</T4801>" +
                                "<T4802>02</T4802>" +
                                "<T4803>39</T4803>" +
                                "<T4804>01</T4804>" +
                                "<T5501>16120702</T5501>" +
                                "<T5503>0000000009</T5503>" +
                                "</TRANS>" +
                                "</EDC>";

            /*int len = richTextBox2.Text.Length + 4;
            Byte[] buffer = new Byte[len];
            buffer[0] = 2;
            for (int i = 0; i < richTextBox2.Text.Length; i++)
            {
                buffer[i + 1] = (byte)richTextBox2.Text[i];
            }
            buffer[len - 2] = 3;
            buffer[len - 1] = 0;
            serialPort1.Write(buffer, 0, buffer.Length);*/
            ezRs232.sendMsg = richTextBox2.Text;
            ezRs232.run();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            msg.Text = "";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //查詢交易(成功)
            if (!ezRs232.serialPort.IsOpen) return;
            msg.Text = "[" + (sender as Button).Text + "]";
            richTextBox2.Text = "<EDC><TRANS><T0100>0110</T0100>" +
                                "<T0300>999999</T0300>" +
                                "<T0200>DigiWinXX</T0200>" +
                                "<T0212>00</T0212>" +
                                "<T0213>05</T0213>" +
                                "<T0214>00</T0214>" +
                                "<T0400>230</T0400>" +
                                "<T0408>332</T0408>" +
                                "<T0410>562</T0410>" +
                                "<T1101>008680</T1101>" +
                                "<T1200>202818</T1200>" +
                                "<T1201>202753</T1201>" +
                                "<T1300>20170221</T1300>" +
                                "<T1301>20170221</T1301>" +
                                "<T1402>20330301</T1402>" +
                                "<T3700>20170221001074</T3700>" +
                                "<T3701>170221523100208680</T3701>" +
                                "<T3800></T3800>" +
                                "<T3900>0000</T3900>" +
                                "<T4100>11100DD02C00</T4100>" +
                                "<T4101>A8D6DC00</T4101>" +
                                "<T4102>192.168.0.121</T4102>" +
                                "<T4103>000001120056922</T4103>" +
                                "<T4104>2E4EDF59</T4104>" +
                                "<T4200>00011472</T4200>" +
                                "<T4210>0</T4210>" +
                                "<T4800>00</T4800>" +
                                "<T4801>00710000EE0E865830E80300EC05004800000100D4DC4800000000000000000000000000000000000000000000</T4801>" +
                                "<T4802>02</T4802>" +
                                "<T4803>00</T4803>" +
                                "<T4804>01</T4804>" +
                                "<T5501>17022139</T5501>" +
                                "<T5503>0000000009</T5503>" +
                                "</TRANS>" +
                                "</EDC>";

            /*int len = richTextBox2.Text.Length + 4;
            Byte[] buffer = new Byte[len];
            buffer[0] = 2;
            for (int i = 0; i < richTextBox2.Text.Length; i++)
            {
                buffer[i + 1] = (byte)richTextBox2.Text[i];
            }
            buffer[len - 2] = 3;
            buffer[len - 1] = 0;
            serialPort1.Write(buffer, 0, buffer.Length);*/
            ezRs232.sendMsg = richTextBox2.Text;
            ezRs232.run();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //查詢交易(失敗)
            if (!ezRs232.serialPort.IsOpen) return;
            msg.Text = "[" + (sender as Button).Text + "]";
            richTextBox2.Text = "<EDC><TRANS><T3900>FF58</T3900>" +
                                "<ErrMsg>上筆交易不成功!!!錯誤代碼:FF58</ErrMsg>" +
                                "</TRANS>" +
                                "</EDC>";
            /*Byte[] buffer = new Byte[1];
            buffer[0] = 21;
            serialPort1.Write(buffer, 0, buffer.Length);*/
            /*byte[] bytes = Encoding.UTF8.GetBytes(richTextBox2.Text);
            int len = bytes.Length + 4;
            Byte[] buffer = new Byte[len];

            buffer[0] = 2;
            for (int i = 0; i < bytes.Length; i++)
            {
                buffer[i + 1] = (byte)bytes[i];
            }
            buffer[len - 2] = 3;
            buffer[len - 1] = 0;

            serialPort1.Write(buffer, 0, buffer.Length);*/
            ezRs232.sendMsg = richTextBox2.Text;
            ezRs232.run();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(richTextBox1.Text);
            XmlNodeList NodeLists = xmlDoc.SelectNodes("TM/TRANS/T0300");
            //xmlDoc.LoadXml(""); 讀純文字xml格式
            foreach (XmlNode OneNode in NodeLists)
            {
                //String StrAttrName = OneNode.Attributes.Name;
                //string StrAttrValue = OneNode.Attributes[" MyAttr1 "].Value;
                richTextBox2.AppendText(OneNode.InnerText);
            }
        }

        public class EZRs232
        {
            public SerialPort serialPort = new SerialPort();
            public String recvMsg;
            public String sendMsg;

            public int ackTimeout;
            public short cntOfRetrySendMsg;

            const Int32 STX = 0x02;
            const Int32 ETX = 0x03;
            const Int32 ACK = 0x06;
            const Int32 NAK = 0x15;


            public void run()
            {
                bool recvFinished;
                Int32 receivedValue;
                int origReadTimeout;
                int byteCount;
                String testMsg;
                int i;

                /*open*/
               /* if (serialPort.IsOpen == true)
                {
                    MessageBox.Show("COM Port已開啟");
                    return;
                }*/

                try
                {

                    //open Comport
                    serialPort.Open();

                    //data
                    sendMsg = Char.ConvertFromUtf32(STX) + sendMsg + Char.ConvertFromUtf32(ETX);

                    /*waitting two ack from EDC*/
                    origReadTimeout = serialPort.ReadTimeout;//bk orig timeout
                    serialPort.ReadTimeout = ackTimeout;//set receive ACK Timeout


                    //recv
                    //recvMsg += "ready to receive while loop(" + cntOfRetrySendMsg + ");";
                    while ((cntOfRetrySendMsg--) > 0)
                    {
                        recvFinished = false;
                        /*send Msg total retry cntOfRetrySendMsg, if got ACK fail*/
                        serialPort.Write(sendMsg);

                        if ((receivedValue = serialPort.ReadByte()) != ACK)
                        {
                            MessageBox.Show("format error, must be ACK");
                            break;
                        }
                        else if (receivedValue == NAK)
                            continue;
                        else if (receivedValue == ACK)
                        {
                            serialPort.ReadTimeout = origReadTimeout;
                            while (true)
                            {
                                if ((byteCount = serialPort.BytesToRead) > 0)
                                {
                                    byte[] buffer = new byte[byteCount];
                                    serialPort.Read(buffer, 0, byteCount);
                                    recvMsg += ConvertByteArrayToString(buffer);
                                    cntOfRetrySendMsg = 0;
                                    break;
                                }
                                Thread.Sleep(2000);
                            }
                        }
                    }

                    /*Send ACK*/
                    //recvMsg += "sendACK(" + cntOfRetrySendMsg + ");\n";
                    serialPort.Write(Char.ConvertFromUtf32(ACK));



                    //close comport
                    serialPort.Close();
                }
                catch (TimeoutException err)
                {
                    /*close*/
                    serialPort.Close();
                    recvMsg += err.Message;
                }
                catch (IOException ioe)
                {
                    recvMsg += ioe.Message;
                }

            }

            private static string ConvertByteArrayToString(Byte[] ByteOutput)
            {

                string StringOutput = System.Text.Encoding.UTF8.GetString(ByteOutput);

                return StringOutput;

            }

            public static byte[] ConvertStringToByte(string Input)
            {
                return System.Text.Encoding.UTF8.GetBytes(Input);
            }





        }
    }
}
