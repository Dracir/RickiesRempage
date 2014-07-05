using UnityEngine;
using System.Collections;

public class ExampleDestruction : MonoBehaviour {

	void HalfHealth () {
		Debug.Log ("I have half health!");
	}
	
	void Destroyed() {
		Debug.Log ("I am destroyed!");
	}
	
}
