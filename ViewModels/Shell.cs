using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly Random random;
        private int counter;

        public Shell()
		{
            send = new SerialPort("COM2", 115200, Parity.None, 8, StopBits.One);
            send.Open();

            receive = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One);
            receive.DataReceived += OnDataReceived;
            receive.Open();

            random = new Random();
            Timer timer = new Timer(500);
            timer.Elapsed += (s, a) => Send();
            timer.Start();

            Data = new ObservableDictionary<int, byte>();
            Enumerable.Range(1, 50).ForEach(index => Data.Add(index, 0));
            counter = Data.Count;
		}

        [Notify] public string Received { get; set; }
        [Notify] public ObservableDictionary<int, byte> Data { get; set; }
	    
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
//            if (count != 24) throw new ArgumentException("Wrong package size: " + count);
            if (count != 24) return;

            var buffer = new byte[count];
            port.Read(buffer, 0, buffer.Length);

            var data = buffer.Select(b => b.ToString("X"))
                             .Aggregate((a, b) => a + b);

            Received = $"Data: {buffer[0]}";

            Execute.OnUIThread(() => Add(buffer[0]));
        }

        private void Add(byte value)
        {
            counter++;
            Data.Remove(Data.First().Key);
            Data.Add(counter, value);
        }
	}
}