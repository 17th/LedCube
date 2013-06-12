using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class Menu : MonoBehaviour {
	StartScript startScript;
	public static uart cubeConnector;
	public static bool toggleAuto = true;
	public static bool toggleShow = true;
	
	List<GUIContent> portList;
	private ComboBox portBoxControl;
	private GUIStyle listStyle;
	List<string> portnames;
	
	public Texture2D comOkpic;
	public Texture2D comBadpic;
	static bool comOk = false;
	
	public static int curFrame = 0;
	public static int maxFrame = 0;
	static int frameDelay = 100;
	static int tempDelay = 0;
	static bool play = false;
	
	// Use this for initialization
	void Start () {
		startScript = gameObject.GetComponent<StartScript>();
		
		portList = new List<GUIContent>();
		listStyle = new GUIStyle();
		portnames = new List<string>();
		cubeConnector = new uart();
		
		portnames = cubeConnector.listPorts();		
		int i = 0;
		foreach(string name in portnames){
			portList.Add(new GUIContent(name));
			i++;
		}
 
		listStyle.normal.textColor = Color.white;
		listStyle.onHover.background =
		listStyle.hover.background = new Texture2D(2, 2);
		listStyle.padding.left =
		listStyle.padding.right =
		listStyle.padding.top =
		listStyle.padding.bottom = 4;
 
		portBoxControl = new ComboBox(new Rect(Screen.width-90,295,60,20), portList[0], portList.ToArray(), "button", "box", listStyle);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate(){
		if(play){
			if(tempDelay == 0){
				curFrame++;
				if(curFrame > maxFrame)
					curFrame = 0;
				
				cubeConnector.loadMtrx(StartScript.frameSet[curFrame]);
				startScript.redrawLeds();
				tempDelay = frameDelay;
			}else
				tempDelay--;
		}
	}
	
	public void updateMtrx(){
		if(toggleAuto){
			cubeConnector.loadMtrx(StartScript.frameSet[curFrame]);
		}
	}
	
	void OnGUI () {
		GUI.backgroundColor = Color.green;
		
		if (GUI.Button (new Rect (Screen.width-90,100,80,20), "Send")) {
			if(!cubeConnector.loadMtrx(StartScript.frameSet[curFrame]))
				comOk = false;
		}
		toggleAuto = GUI.Toggle (new Rect (Screen.width-90,125,80,20), toggleAuto, " auto");
		
		if (GUI.Button (new Rect (Screen.width-90,170,80,20), "Clear")) {
			startScript.clearMtrx();
			if(!cubeConnector.clearMtrx())
				comOk = false;
		}
		
		if (GUI.Button (new Rect (Screen.width-90,195,80,20), "Show all")) {
			toggleShow = !toggleShow;
			startScript.redrawLeds();
		}
		
		if (GUI.Button (new Rect (Screen.width-90,240,80,20), "Reset")) {
			if(!cubeConnector.reset())
				comOk = false;
			startScript.clearMtrx();
		}
		
		if (GUI.Button (new Rect (Screen.width-90,270,80,20), "Open")) {
			if(cubeConnector.openPort(portList[portBoxControl.SelectedItemIndex].text))
				comOk = true;
			else
				comOk = false;
		}
		
		portBoxControl.Show();
		if(comOk)
			//GUI.Box (new Rect (Screen.width-25,295, 20,20), new GUIContent(comOkpic));
			GUI.Label (new Rect (Screen.width-25,295, 20,20), new GUIContent(comOkpic));
		else {
			//GUI.Box (new Rect (Screen.width-25,295, 20,20), new GUIContent(comBadpic));
			GUI.Label (new Rect (Screen.width-25,295, 20,20), new GUIContent(comBadpic));
		}
		
		if (GUI.Button (new Rect (Screen.width-90,400,80,20), "save")) {
			IFormatter formatter = new BinaryFormatter();
	        FileStream fs = File.Create("LedCube.ani");
	        formatter.Serialize(fs, StartScript.frameSet);
	        fs.Close();
		}
		
		if (GUI.Button (new Rect (Screen.width-90,425,80,20), "load")) {
			IFormatter formatter = new BinaryFormatter();
	        FileStream fs = File.OpenRead("LedCube.mtrx");
	        StartScript.frameSet = new List<Dictionary<string, bool>>(formatter.Deserialize(fs) as List<Dictionary<string, bool>>);
	        fs.Close();
			
			maxFrame = StartScript.frameSet.Count-1;
		}
		
		curFrame = (int)GUI.HorizontalSlider(new Rect(10, Screen.height-45, Screen.width-110, 20), curFrame, 0.0f, (float)maxFrame);
		
		if (GUI.Button (new Rect ((Screen.width-110)*0.5f-100,Screen.height-25,40,20), "<<")) {
			curFrame = 0;
			startScript.redrawLeds();
		}
		
		if (GUI.Button (new Rect ((Screen.width-110)*0.5f-50,Screen.height-25,30,20), "<")) {
			curFrame--;
			if(curFrame < 0)
				curFrame = 0;
			startScript.redrawLeds();
		}
		
		curFrame = int.Parse(GUI.TextField(new Rect ((Screen.width-110)*0.5f-15, Screen.height-25, 30, 20), curFrame.ToString()));
		
		if (GUI.Button (new Rect ((Screen.width-110)*0.5f+60,Screen.height-25,40,20), ">>")) {
			curFrame = maxFrame;
			startScript.redrawLeds();
		}
		
		if (GUI.Button (new Rect ((Screen.width-110)*0.5f+20,Screen.height-25,30,20), ">")) {
			curFrame++;
			if(curFrame > maxFrame)
				curFrame = maxFrame;
			startScript.redrawLeds();
		}
		
		if (GUI.Button (new Rect (Screen.width-180,Screen.height-25,30,20), "-")) {
			maxFrame--;
			if(curFrame > maxFrame)
				curFrame = maxFrame;
			startScript.delFrame();
		}
		
		if (GUI.Button (new Rect (Screen.width-140,Screen.height-25,30,20), "+")) {
			maxFrame++;
			startScript.addFrame();
			curFrame = maxFrame;
		}
		
		frameDelay = int.Parse(GUI.TextField(new Rect (Screen.width-70, Screen.height-85, 40, 20), frameDelay.ToString()));
		
		if (GUI.Button (new Rect (Screen.width-90,Screen.height-60,80,20), "play")) {
			play = true;
		}
		
		if (GUI.Button (new Rect (Screen.width-90,Screen.height-35,80,20), "stop")) {
			play = false;
		}
		
	}
}
