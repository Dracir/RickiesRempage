using UnityEngine;
using System.Collections;

public class ItemIntegrity : MonoBehaviour {
	
	public int hp = 30;
	private int initHP;
	
	void Start () {
		initHP = hp;
	}
	
	public void SubtractIntegrity (int amount){
		hp -= amount;
		Debug.Log ("my new health is " + hp);
		if (hp < (initHP / 2)){
			SendMessage("HalfHealth");
		}
		if (hp <= 0){
			SendMessage("Destroyed");
		}
		
	}
	
}
