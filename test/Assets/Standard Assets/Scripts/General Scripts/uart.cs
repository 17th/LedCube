using UnityEngine;
using System.Collections.Generic;
using System.IO.Ports;

public class uart {
	private SerialPort port;
	List<string> portnames = new List<string>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool openPort(string portName){
		port = new SerialPort( portName , 9600, Parity.None, 8, StopBits.One);
			try{
				port.Open();
				return true;
			}catch{
				return false;
			}
	}
	
	public List<string> listPorts(){
		portnames.AddRange(SerialPort.GetPortNames());
		return portnames;
	}
	
	public bool clearMtrx(){
		byte[] data = {3};
		try{
			port.Write(data, 0, data.Length);
			return true;
		}catch{
			return false;
		}
	}
	
	public bool loadMtrx(Dictionary<string, bool> mtrx){
		byte[] data = new byte[65];
		
		for(int z=0; z<8; z++)
		    for (int y=0; y<8; y++){
				data[8*z + y + 1] = 0;
		        for (byte x=0;x<8;x++){
					if(mtrx[(64*z + 8*y + x).ToString()])
						data[8*z + y + 1] += (byte)(128>>x);
					}
			}
		data[0] = 2;
		try{
			port.Write(data, 0, data.Length);
			return true;
		}catch{
			return false;
		}
	}
	
	public bool reset(){
		byte[] data = {1};
		try{
			port.Write(data, 0, data.Length);
			return true;
		}catch{
			return false;
		}
		
		port.Close();
	}
	
	void OnApplicationQuit(){
		port.Close();
	}
}
