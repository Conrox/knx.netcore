using System;
using System.Net;
using KNXLib.Exceptions;
using KNXLibCore.CoreWrapper;

namespace KNXLib
{
    internal class KnxConnectionConfiguration
    {
        public KnxConnectionConfiguration(string host, int port)
        {
            Host = host;
            Port = port;

            IpAddress = null;
            try
            {
                IpAddress = IPAddress.Parse(host);
            }
            catch
            {
                try
                {
                    IpAddress = DnsM.GetHostAddress(host);
                }
                catch (Exception)
                {
                    throw new InvalidHostException(host);
                }
            }

            if (IpAddress == null)
                throw new InvalidHostException(host);

            EndPoint = new IPEndPoint(IpAddress, port);
        }

        public string Host { get; private set; }

        public int Port { get; private set; }

        public IPAddress IpAddress { get; private set; }

        public IPEndPoint EndPoint { get; private set; }
    }
}
