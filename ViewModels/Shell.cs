using System;
using System.IO.Ports;
using System.Linq;
using System.Timers;
using Caliburn.Micro;
using MoreLinq;
using ViewModels.Services;

namespace ViewModels
{
	public interface IShell {}

    public class Shell : Conductor<IScreen>.Collection.AllActive, IShell
	{
        private readonly SerialPort send;
        private SerialPort receive;
        private Random random;

        public Shell()
		{
            send = new SerialPort("COM2", 115200, Parity.None, 8, StopBits.One);
            send.Open();

            receive = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One);
            receive.DataReceived += OnDataReceived;
            receive.Open();

            random = new Random();
            Timer timer = new Timer(100);
            timer.Elapsed += (s, a) => Send();
            timer.Start();
		}

        [Notify]
	    public string Received { get; set; }
	    
	    public void Send()
	    {
	        var buffer = new byte[24];
	        random.NextBytes(buffer);

            send.Write(buffer, 0, buffer.Length);

        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            SerialPort port = (SerialPort)sender;

            int count = port.BytesToRead;
            if (count != 24) throw new ArgumentException("Wrong package size: " + count);

            var buffer = new byte[count];
            var read = port.Read(buffer, 0, buffer.Length);
            var data = buffer.Select(b => b.ToString())
                .Aggregate((a, b) => a+b);

            Received = $"Received: {read} bytes\n\nData: {data}";
        }
    }
}