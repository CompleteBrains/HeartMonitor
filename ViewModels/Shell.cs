using System.IO.Ports;
using Caliburn.Micro;
using ViewModels.Services;

namespace ViewModels
{
	public interface IShell {}

    public class Shell : Conductor<IScreen>.Collection.AllActive, IShell
	{
        private readonly SerialPort send;

        public Shell()
		{
            send = new SerialPort("COM2", 115200, Parity.None, 8, StopBits.One);
            send.Open();

            var receive = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One);
            receive.DataReceived += OnDataReceived;
            receive.Open();
		}

        [Notify]
	    public string Received { get; set; }
	    
	    public void Send()
	    {
            send.WriteLine("Any text");
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            SerialPort port = (SerialPort)sender;

            Received += port.ReadLine();
        }
    }
}