using System.IO.Ports;
using Caliburn.Micro;
using ViewModels.Services;

namespace ViewModels
{
	public interface IShell {}

    public class Shell : Conductor<IScreen>.Collection.AllActive, IShell
	{
        private readonly SerialPort send;
        private SerialPort receive;

        public Shell()
		{
            send = new SerialPort("COM2");
            send.Open();

            receive = new SerialPort("COM3");
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