using UnityEngine;
using System.Collections.Generic;

public class StartScript : MonoBehaviour {
	
	public static List<Dictionary<string, bool>> frameSet;
	//public static Dictionary<string, bool> ledStateList;
	public GameObject prefab;
	public UnityEngine.Material OnMat;
	public UnityEngine.Material OffMat;
	public UnityEngine.Camera mainCamera;
	
	char axisLetter;
	public GameObject xPlane;
	public GameObject yPlane;
	public GameObject zPlane;
	static GameObject[] leds = new GameObject[512];
	
	Menu menuScript;

	// Use this for initialization
	void Start () {
		frameSet = new List<Dictionary<string, bool>>();
		menuScript = gameObject.GetComponent("Menu") as Menu;
		
		//ledStateList = new Dictionary<string, bool>();
		frameSet.Add(new Dictionary<string, bool>());
		
		float cx = (float)((10.0)/(Screen.width));
		float cy = (float)((50.0)/(Screen.height));
		float cw = (float)((Screen.width-110)/(Screen.width*1.0));
		float ch = (float)((Screen.height-60)/(Screen.height*1.0));
		mainCamera.rect = new Rect(cx,cy,cw,ch);
		
		int i = 0;
		for(int z=0; z<15; z+=2){
			for (int y=0; y<15; y+=2) {
		        for (int x=0;x<15;x+=2) {
		            Vector3 pos = new Vector3(x, z, y);
		            GameObject newLed = (GameObject)(Instantiate(prefab, pos, Quaternion.identity));
		            newLed.name = (i).ToString();
		            //ledStateList[newLed.name] = false;
					frameSet[Menu.curFrame][newLed.name] = false;
		            newLed.renderer.enabled = false;
		            leds[i] = newLed;
		            i++;
		        }
		    }
		}
		
		axisLetter = 'x';
		yPlane.SetActive(false);
		zPlane.SetActive(false);
		
		for(int z=0; z<8; z++)
		    for (int y=0; y<8; y++){
		    	leds[64*z + 8*y].renderer.enabled = true;
				leds[64*z + 8*y].layer = 0;
			}
	
	}
	
	
	public void redrawLeds(){
		int num;
		switch(axisLetter){
			case 'x':
				for(var z=0; z<8; z++)
					for(var y=0; y<8; y++)
		    			for (var x=0; x<8; x++){
							num = 64*z + 8*y + x;
					
							if((x <= xPlane.transform.position.x/2) && (Menu.toggleShow || frameSet[Menu.curFrame][(num).ToString()]))
		    					leds[num].renderer.enabled = true;
							else
		    					leds[num].renderer.enabled = false;
		    				
		    				if(x == xPlane.transform.position.x/2){
								//leds[num].renderer.enabled = true;
		    					leds[num].layer = 0;
							}else
		    					leds[num].layer = 2;
					
							if(frameSet[Menu.curFrame][(num).ToString()]){
								leds[num].renderer.material = OnMat;
							}
							else{
								leds[num].renderer.material = OffMat;
							}
		    			}
		    	break;
		    	
		    case 'y':
		    	for(var z=0; z<8; z++)
					for(var y=0; y<8; y++)
		    			for (var x=0; x<8; x++){
		    				num = 8*z + 64*y + x;
					
							if((y <= yPlane.transform.position.y/2) && (Menu.toggleShow || frameSet[Menu.curFrame][(num).ToString()]))
		    					leds[num].renderer.enabled = true;
							else
		    					leds[num].renderer.enabled = false;
		    				
		    				if(y == yPlane.transform.position.y/2){
								//leds[num].renderer.enabled = true;
		    					leds[num].layer = 0;
							}else
		    					leds[num].layer = 2;
					
							if(frameSet[Menu.curFrame][(num).ToString()]){
								leds[num].renderer.material = OnMat;
							}
							else{
								leds[num].renderer.material = OffMat;
							}
		    			}
		    	break;
		    	
		    case 'z':
		    	for(var z=0; z<8; z++)
					for(var y=0; y<8; y++)
		    			for (var x=0; x<8; x++){
		    				num = 8*z + 64*y + x;
					
							if((z <= zPlane.transform.position.z/2) && (Menu.toggleShow || frameSet[Menu.curFrame][(num).ToString()]))
		    					leds[num].renderer.enabled = true;
							else
		    					leds[num].renderer.enabled = false;
		    				
		    				if(z == zPlane.transform.position.z/2){
								//leds[num].renderer.enabled = true;
		    					leds[num].layer = 0;
							}else
		    					leds[num].layer = 2;
					
							if(frameSet[Menu.curFrame][(num).ToString()]){
								leds[num].renderer.material = OnMat;
							}
							else{
								leds[num].renderer.material = OffMat;
							}
		    			}
			break;
		   }
	}
	
	public void clearMtrx(){
		foreach(GameObject led in leds){
			led.renderer.material = OffMat;
			//ledStateList[led.name] = false;
			frameSet[Menu.curFrame][led.name] = false;
		}
	}
	
	public void addFrame(){
		//frameSet.Add(frameSet[frameSet.Count-1]);
		//Dictionary<string, bool> list = new Dictionary<string, bool>();
		//for(int i=0; i< 512; i++)
		//	list.Add(i.ToString(), false);
		frameSet.Add(new Dictionary<string, bool>(frameSet[frameSet.Count-1]));
		//frameSet.Add(list);
	}
	
	public void delFrame(){
		frameSet.Remove(frameSet[frameSet.Count-1]);
	}
		
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(0)){
	    	Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
	        RaycastHit hit;
	        if (Physics.Raycast(ray,out hit, (float)(100.0))) {
				frameSet[Menu.curFrame][hit.collider.name] = !frameSet[Menu.curFrame][hit.collider.name];
				redrawLeds();
				menuScript.updateMtrx();
	        }
		}
		
		var moveDir = 0;
		if(Input.GetAxis("Mouse ScrollWheel")>0)
			moveDir = 2;
		if(Input.GetAxis("Mouse ScrollWheel")<0)
			moveDir = -2;
			
		switch(axisLetter){
			case 'x':
				if(Input.GetKeyDown(KeyCode.Mouse2)){
					xPlane.SetActive(false);
					yPlane.SetActive(true);
					axisLetter = 'y';
					
					redrawLeds();
				}
				
				if(xPlane.transform.position.x+moveDir<15 && xPlane.transform.position.x+moveDir>-1 && moveDir!=0){
					Vector3 pos = xPlane.transform.position;
					pos.x += moveDir;
					xPlane.transform.position = pos;
					
					redrawLeds();
				}
				
				break;
					
			case 'y':
				if(Input.GetKeyDown(KeyCode.Mouse2)){
					yPlane.SetActive(false);
					zPlane.SetActive(true);
					axisLetter = 'z';
					
					redrawLeds();
				}
				
				if(yPlane.transform.position.y+moveDir<15 && yPlane.transform.position.y+moveDir>-1 && moveDir!=0){
					Vector3 pos = yPlane.transform.position;
					pos.y += moveDir;
					yPlane.transform.position = pos;
					
					redrawLeds();
				}
				break;
				
			case 'z':
				if(Input.GetKeyDown(KeyCode.Mouse2)){
					zPlane.SetActive(false);
					xPlane.SetActive(true);
					axisLetter = 'x';
					
					redrawLeds();
				}
				
				if(zPlane.transform.position.z+moveDir<15 && zPlane.transform.position.z+moveDir>-1 && moveDir!=0){
					Vector3 pos = zPlane.transform.position;
					pos.z += moveDir;
					zPlane.transform.position = pos;
					
					redrawLeds();
				}
				break;
		}
	
	}

}
