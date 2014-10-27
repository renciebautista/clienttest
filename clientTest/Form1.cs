using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace clientTest
{
    public partial class Form1 : Form
    {
        UdpClient udpClient = new UdpClient();
        public Form1()
        {
            InitializeComponent();
        }

        private string Connect()
        {
            return "<?xml version=\"1.0\"?><Tig><Client.Connect Name=\"AVLS\" Version=\"2.12\" /></Tig>";
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            sendUdp();
        }

        private void sendUdp()
        {
            // UdpClient udpClient = new UdpClient();
            //udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true); // reuse port
            //udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 30512));

            Byte[] sendBytes = Encoding.ASCII.GetBytes(Connect());
            try
            {
                udpClient.Send(sendBytes, sendBytes.Length, "192.168.1.180", 30511); // send data to server via port 30511



                Thread thdUDPServer = new Thread(new ThreadStart(serverThread)); //start thread for recieving
                if (thdUDPServer.ThreadState != ThreadState.Running)
                {
                    thdUDPServer.Start();
                }
                

            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sendUdp();
        }
        public void serverThread()
        {
            try
            {
                udpClient.BeginReceive(new AsyncCallback(recv), null);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        //CallBack
        private void recv(IAsyncResult res)
        {
            //IPAddress tnxip = IPAddress.Parse("192.168.1.180");
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.180"), 0);
            byte[] received = udpClient.EndReceive(res, ref RemoteIpEndPoint);

            //Process codes

            //MessageBox.Show(Encoding.UTF8.GetString(received));
            this.Invoke((MethodInvoker)(() => txtReply.AppendText(RemoteIpEndPoint +"=>"+Encoding.UTF8.GetString(received) + Environment.NewLine)));
            udpClient.BeginReceive(new AsyncCallback(recv), null);
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true); // reuse port
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 30512));
            //try
            //{
            //    udpClient.BeginReceive(new AsyncCallback(recv), null);
            //}
            //catch(Exception ee)
            //{
            //     MessageBox.Show(ee.ToString());
            //}  
        }

        //CallBack
        //private void recv(IAsyncResult res)
        //{
        //    try
        //    {
        //        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        //        byte[] received = udpClient.EndReceive(res, ref RemoteIpEndPoint);

        //        //Process codes

        //        // MessageBox.Show(Encoding.UTF8.GetString(received));
        //        //txtReply.AppendText(Encoding.UTF8.GetString(received));
        //        this.Invoke((MethodInvoker)(() => txtReply.AppendText(Encoding.UTF8.GetString(received))));
        //        udpClient.BeginReceive(new AsyncCallback(recv), null);
        //    }

        //    catch (Exception ee)
        //    {
        //        MessageBox.Show(ee.ToString());
        //    } 
        //}

        public void serverThread_1()
        {
            UdpClient udpClient = new UdpClient();
            while (true)
            {
                //IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                //Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                //string returnData = Encoding.ASCII.GetString(receiveBytes);
                // lbConnections.Items.Add(RemoteIpEndPoint.Address.ToString() + ":" + returnData.ToString());
                // this.Invoke((MethodInvoker)(() => lbConnections.Items.Add(RemoteIpEndPoint.Address.ToString() + ":" + returnData.ToString())));

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);
                //txtReply.Text = returnData;
                this.Invoke((MethodInvoker)(() => txtReply1.Text = returnData.ToString()));
            }
        }
    }
}
