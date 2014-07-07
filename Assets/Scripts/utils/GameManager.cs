using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		AudioPlayer.Play("Rampage", Camera.main.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
