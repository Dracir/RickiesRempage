using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {
	
	public int powerValue = 15;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D (Collider2D other){
		Rickie rick = other.GetComponent<Rickie>();
		
		if (rick){
			rick.AddPower(powerValue);
			Destroy (gameObject);
		}
	}
}
