using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

class SerialHandler
{
	public delegate void SerialDataReceivedEventHandler(string message);
	public event SerialDataReceivedEventHandler OnDataReceived;

	public string portName = "COM4";
	public int baudRate    = 9600;

	private SerialPort serialPort_;
	private Thread thread_;
	private bool isRunning_ = false;

	private string message_;
	private bool isNewMessageReceived_ = false;

	void Update()
	{
		if (isNewMessageReceived_) {
			OnDataReceived(message_);
		}
	}

	public SerialHandler(){
		try{
		this.Open ();
		}catch(System.Exception e){
			Debug.LogWarning (e);
		}
	}

	void OnDestroy()
	{
		Close();
	}

	public void Open()
	{
		serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
		serialPort_.Open();
		Debug.Log (serialPort_.IsOpen);


		isRunning_ = true;

		thread_ = new Thread(Read);
		thread_.Start();
	}

	public void Close()
	{
		isRunning_ = false;

		if (thread_ != null && thread_.IsAlive) {
			thread_.Join();
		}

		if (serialPort_ != null && serialPort_.IsOpen) {
			serialPort_.Close();
			serialPort_.Dispose();
		}
	}

	private void Read(){

		try{
			serialPort_.DiscardInBuffer();
		}catch(System.Exception e){
			Debug.LogWarning (e);
		}

	}


	public void Write(string message)
	{
		try {
//			Debug.Log(message);
			serialPort_.Write(message);
		} catch (System.Exception e) {
			//Debug.LogWarning(e.Message);
		}
	}
}
