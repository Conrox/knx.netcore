using KNXLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KNXTestRouting
{
    public class Program
    {
        private static KnxConnection _connection;

        private const string LightOnOffAddress = "6/0/5";

        private static readonly IList<string> Lights = new List<string> { "6/0/6" };
        private static readonly IList<string> Temperatures = new List<string> { "1/1/17", "1/1/18" };

        private static void Main()
        {
            _connection = new KnxConnectionRouting { Debug = false, ActionMessageCode = 0x29 };
            _connection.KnxConnectedDelegate += Connected;
            _connection.KnxDisconnectedDelegate += Disconnected;
            _connection.KnxEventDelegate += Event;
            _connection.KnxStatusDelegate += Status;
            _connection.Connect();

            LightOnOff();
          

            Console.WriteLine("Done. Press [ENTER] to finish");
            Console.Read();
            Environment.Exit(0);
        }

        private static void LightOnOff()
        {
            Console.WriteLine("Press [ENTER] to send command ({0}) - true", LightOnOffAddress);
            Console.ReadLine();
            _connection.Action(LightOnOffAddress, true);
            Thread.Sleep(200);

            Console.WriteLine("Press [ENTER] to send command ({0}) - false", LightOnOffAddress);
            Console.ReadLine();
            _connection.Action(LightOnOffAddress, false);
            Thread.Sleep(200);
        }

        private static void Event(string address, string state)
        {
            if (Temperatures.Contains(address))
            {
                var temp = (float)_connection.FromDataPoint("9.001", state);
                Console.WriteLine("New Event: TEMPERATURE device " + address + " has status (" + state + ")" + temp);
            }
            else if (Lights.Contains(address))
            {
                Console.WriteLine("New Event: LIGHT device " + address + " has status (" + state + ")" + ((byte)state[0]).ToString());
            }
            else
            {
                Console.WriteLine("New Event: device " + address + " has status " + state);
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

            Thread.Sleep(1000);
            _connection.Connect();
        }
    }
}
