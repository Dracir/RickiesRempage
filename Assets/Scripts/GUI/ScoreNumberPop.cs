using UnityEngine;
using System.Collections;

public class ScoreNumberPop : MonoBehaviour {
	GUIText text;
	int initialSize;
	int growAmount = 10;
	// Use this for initialization
	void Start () {
		text = GetComponent<GUIText>();
		initialSize = text.fontSize;
	}
	
	// Update is called once per frame
	void Update () {
		if (text.fontSize > initialSize){
			text.fontSize --;
		}
	}
	
	public void Pop () {
		text.fontSize = initialSize + growAmount;
	}
}
