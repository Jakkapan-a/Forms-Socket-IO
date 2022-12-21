using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quobject.EngineIoClientDotNet.Client;
using SocketIOClient;

namespace Forms_SocketIO
{
    public partial class Form1 : Form
    {
        SocketIO client;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new SocketIO("http://localhost:3000/");
            client.ConnectAsync();

            // Receive message from server
            client.On("message", (data) =>
            {
                SetText(data.GetValue().ToString());
            });
        }
        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.richTextBoxReceive.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richTextBoxReceive.Text = text;
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (client != null && client.Connected)
            {
                client.EmitAsync("message", txtMessage.Text);
            }
        }
    }
}
