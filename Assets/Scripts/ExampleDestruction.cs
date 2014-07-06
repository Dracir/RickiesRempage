using UnityEngine;
using System.Collections;

public class ExampleDestruction : MonoBehaviour {
	bool destroyed = false;
	void HalfHealth () {
		Debug.Log ("I have half health!");
	}
	
	void Destroyed() {
		if (!destroyed){
			destroyed = true;
			GUIHandler.instance.AddPoints(233);
		}
		
		Debug.Log ("I am destroyed!");
	}
	
}
