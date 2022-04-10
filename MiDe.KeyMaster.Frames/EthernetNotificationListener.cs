using Microsoft.Extensions.Logging;
using MiDe.KeyMaster.App;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiDe.KeyMaster.Frames
{
    public class EthernetNotificationListener
    {
        TcpListener server = null;
        public event EventHandler<MessageEventArgs> MessageReceived;
        public event EventHandler ConnectionTemporarilyLost;
        public event EventHandler ConnectionPermanentlyLost;

        private readonly ILogger logger;

        public EthernetNotificationListener(string ip, int port, ILogger logger)
        {
            this.logger = logger;
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            server.Start();
        }

        public async Task StartListenerAsync()
        {
            try
            {
                while (true)
                {                    
                    var client = await server.AcceptTcpClientAsync();
                    logger.LogInformation("Connected!");
                    
                    if (client != null)
                    {
                      Task.Run(() => HandleDeviceAsync(client));
                    }                    
                }
            }
            catch (SocketException e)
            {
                logger.LogInformation("SocketException: {0}", e);
                server.Stop();
                OnConnectionPermanentlyLost();
            }
        }

        public async Task HandleDeviceAsync(TcpClient client)
        {
            var stream = client.GetStream();
            string imei = String.Empty;

            string data = null;
            Byte[] bytes = new Byte[256];
            int i;
            try
            {
                while ((i = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                {
                    string hex = BitConverter.ToString(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    OnMessageReceived(new MessageEventArgs(data));
                }
            }
            catch (Exception e)
            {
                logger.LogInformation("Exception: {0}", e.ToString());
                client.Close();
                OnConnectionTemporarilyLost();
            }
        }

        protected virtual void OnMessageReceived(MessageEventArgs e)
        {
            logger.LogTrace("Received message {message}", e.Message);
            EventHandler<MessageEventArgs> handler = MessageReceived;
            handler?.Invoke(this, e);
        }

        protected virtual void OnConnectionTemporarilyLost()
        {
            EventHandler handler = ConnectionTemporarilyLost;
            handler?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnConnectionPermanentlyLost()
        {
            EventHandler handler = ConnectionPermanentlyLost;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
