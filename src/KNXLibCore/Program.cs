using KNXLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace knx.netcore
{
    public class Program
    {
        private static KnxConnection _connection;

        public static void Main(string[] args)
        {
            /*test output werte richtig anzeigen
testen knx connection unten abschalten
auf nas probiren*/
            _connection = new KnxConnectionTunneling("192.168.100.250", 3671, "192.168.100.194", 3671) { Debug = false };
            //_connection = new KnxConnectionTunneling("192.168.100.250", 3671, "172.17.0.2", 3672) { Debug = true };
            //_connection = new KnxConnectionTunneling("192.168.100.250", 3671, "192.168.100.90", 3672) { Debug = false }; /*DOCKER NAS*/
            //_connection = new KnxConnectionTunneling("192.168.100.250", 3671, "192.168.100.172", 3671) { Debug = false };
            _connection.KnxConnectedDelegate += Connected;
            _connection.KnxDisconnectedDelegate += Disconnected;
            _connection.KnxEventDelegate += Event;
            _connection.KnxStatusDelegate += Status;
            _connection.Connect();

            Console.WriteLine("Done. Press [ENTER] to finish");
            //Console.ReadLine();
            //Console.WriteLine("send");
            //_connection.Action("2/1/3", true);
            //Console.ReadLine();
            //_connection.Action("2/1/3", false);
            Console.ReadLine();

            _connection.KnxDisconnectedDelegate -= Disconnected;
            _connection.Disconnect();
            Environment.Exit(0);
        }
        private static void Event(string address, string state)
        {            
            if (address.Equals("4/0/6") || address.Equals("5/1/1") || address.Equals("5/1/4") || address.Equals("5/1/10") || address.Equals("5/1/11"))
            {
                Console.WriteLine("New Event: device " + address + " has status (" + state + ") --> " + _connection.FromDataPoint("9.001", state));
            }
            else
            {
                var data = string.Empty;

                if (state.Length == 1)
                {
                    data = ((byte)state[0]).ToString();
                }
                else
                {
                    var bytes = new byte[state.Length];
                    for (var i = 0; i < state.Length; i++)
                    {
                        bytes[i] = Convert.ToByte(state[i]);
                    }

                    data = state.Aggregate(data, (current, t) => current + t.ToString());
                }

                Console.WriteLine("New Event: device " + address + " has status (" + state + ") --> " + data);
            }
           
        }

        private static void Status(string address, string state)
        {
            Console.WriteLine("New Status: device " + address + " has status " + state);
        }

        private static void Connected()
        {
            Console.WriteLine("Connected!");
        }

        private static void Disconnected()
        {
            Console.WriteLine("Disconnected! Reconnecting");
            if (_connection == null)
                return;

            System.Threading.Thread.Sleep(1000);
            _connection.Connect();
        }
    }
}
