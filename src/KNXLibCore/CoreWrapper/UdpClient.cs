using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace KNXLibCore.CoreWrapper
{
    public class UdpClient : System.Net.Sockets.UdpClient
    {
        private IPEndPoint _localEndpoint;

       // public System.Net.Sockets.Socket Client { get; internal set; }

        public UdpClient(IPEndPoint _localEndpoint)
        {
            this._localEndpoint = _localEndpoint;
            this.Client.Bind(this._localEndpoint);                  
        }

        internal async void Send(byte[] datagram, int length, IPEndPoint _remoteEndpoint)
        {
            try { 
                await this.SendAsync(datagram, length, _remoteEndpoint);
            }
            catch (Exception ex)
            {
                //fired on socket close
                Console.WriteLine("Error " + ex.Message);                
            }
}

        //internal void JoinMulticastGroup(IPAddress ipAddress, IPAddress localIp)
        //{
        //    throw new NotImplementedException();
        //}

        internal void Close()
        {
            if(this.Client != null)
                this.Client.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            if (this.Client != null)
                this.Client.Dispose();
            this.Dispose();
            //this.Client = null;
        }

        byte[] lastData;
        IPEndPoint lastEndPoint;
        internal void BeginReceive(Action<UdpClient> onReceive, object[] v)
        {
            //throw new NotImplementedException("for routing");

            Task.Run(async () => 
            {
                try
                {
                    var result = await this.ReceiveAsync();
                    lastData = result.Buffer;
                    lastEndPoint = result.RemoteEndPoint;
                    onReceive(this);
                }
                catch (Exception ex)
                {
                    //fired on socket close
                    Console.WriteLine("Error " + ex.Message);
                }
            });            
        }

        internal byte[] EndReceive()
        {        
            return lastData;            
        }

        internal byte[] Receive(ref IPEndPoint _localEndpoint)
        {
            try
            {
                var data = ReceiveAsync().Result;
                if (data != null)
                    return data.Buffer;
                else
                    return null;
            }
            catch (Exception ex)
            {
                //fired on socket close
                Console.WriteLine("Error " + ex.Message);
                return null;
            }
        }

        //internal void DropMulticastGroup(IPAddress ipAddress)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
