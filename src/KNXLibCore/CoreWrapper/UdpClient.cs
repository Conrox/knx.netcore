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
            await this.SendAsync(datagram, length, _remoteEndpoint);            
        }

        //internal void JoinMulticastGroup(IPAddress ipAddress, IPAddress localIp)
        //{
        //    throw new NotImplementedException();
        //}

        internal void Close()
        {            
            //
        }
        
        byte[] lastData;
        IPEndPoint lastEndPoint;
        internal void BeginReceive(Action<UdpClient> onReceive, object[] v)
        {
            //throw new NotImplementedException("for routing");

            Task.Run(async () => 
            {
                var result = await this.ReceiveAsync();
                lastData = result.Buffer;
                lastEndPoint = result.RemoteEndPoint;                                
                onReceive(this);
            });            
        }

        internal byte[] EndReceive()
        {        
            return lastData;            
        }

        internal byte[] Receive(ref IPEndPoint _localEndpoint)
        {
            var data = ReceiveAsync().Result;
            if (data != null)
                return data.Buffer;
            else
                return null;
        }

        //internal void DropMulticastGroup(IPAddress ipAddress)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
