using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsvFileBreaker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter= "CSV files (*.csv)|*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                textBox1.Text =  openFileDialog1.FileName;
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var path = System.IO.Path.GetDirectoryName(textBox1.Text);
            using (var reader = new StreamReader(textBox1.Text))
            {
                int i = 0;
                int j = 1;
              
                List<string> line = new List<string>();
                while (!reader.EndOfStream)
                {

                    line.Add(LineBuilder(reader.ReadLine()));
                    i++;
                    if (i >= System.Convert.ToInt32(textBox2.Text))
                    {
                        using (StreamWriter writer = new StreamWriter(path +"\\"+ "file" + j + ".csv"))
                        {
                            j++;
                            i = 0;
                            foreach (var item in line)
                            {
                                writer.WriteLine(item);
                            }
                            line=new List<string> ();

                        }
                    }


                }

                using (StreamWriter writer = new StreamWriter(path + "\\" + "file" + j + ".csv"))
                {

                    foreach (var item in line)
                    {
                        writer.WriteLine(item);
                    }
                }
            }
            this.Close();
        }
        private int converter(string num)
        {
            int number = 0;
            string temp = num;
            for (int j = 3; j >= 0; j--)
            {
                var test = temp.IndexOf(".");
                if (temp.IndexOf(".") != -1)
                {
                    number += System.Convert.ToInt32(temp.Substring(0, temp.IndexOf("."))) * (j ^ 256);
                    temp = temp.Substring(temp.IndexOf(".") + 1);
                }
                else
                {
                    number += System.Convert.ToInt32(temp) * (j ^ 256);

                }


            }
            return number;
        }

        public double Dot2LongIP(string DottedIP)
        {

            int i;
            string[] arrDec;
            double num = 0;
            if (DottedIP == "")
            {
                return 0;
            }
            else
            {
                arrDec = DottedIP.Split('.');
                for (i = arrDec.Length - 1; i >= 0; i--)
                {
                    num += ((int.Parse(arrDec[i]) % 256) * Math.Pow(256, (3 - i)));
                }
                return num;
            }
        }

        public string LineBuilder(string line)
        {
            string temp1 = "", temp2 = "", rmstr = "", finalstr = "";
            rmstr = temp1 = finalstr = line;
            temp1 = temp1.Substring(0, temp1.IndexOf(","));
            rmstr = rmstr.Substring(rmstr.IndexOf(",") + 1);
            temp2 = rmstr.Substring(0, rmstr.IndexOf(","));
            temp1 = temp1.Replace("\"", "");
            temp2 = temp2.Replace("\"", "");
            for (int k = 0; k <= 1; k++)
            {
                finalstr = finalstr.Substring(finalstr.IndexOf(",") + 1);
            }
            

            return "\"" + Dot(temp1) + "\",\"" + Dot(temp2) + "\"," + finalstr;
        }

        public ulong Dot(string dottedip)
        {
            string strIP = dottedip;
            System.Net.IPAddress address;
            UInt64 ipnum=0;

            if (System.Net.IPAddress.TryParse(strIP, out address))
            {
                byte[] addrBytes = address.GetAddressBytes();

                if (System.BitConverter.IsLittleEndian)
                {
                    System.Collections.Generic.List<byte> byteList = new System.Collections.Generic.List<byte>(addrBytes);
                    byteList.Reverse();
                    addrBytes = byteList.ToArray();
                }

                if (addrBytes.Length > 8)
                {
                    //IPv6
                    ipnum = System.BitConverter.ToUInt64(addrBytes, 8);
                    ipnum <<= 64;
                    ipnum += System.BitConverter.ToUInt64(addrBytes, 0);
                 
                }
                else
                {
                    //IPv4
                    ipnum = System.BitConverter.ToUInt32(addrBytes, 0);
          
                }
              
            }
            return ipnum;
        }
    }
}