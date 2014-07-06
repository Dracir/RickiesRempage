﻿using UnityEngine;
using System.Collections;

public class ItemIntegrity : MonoBehaviour {
	
	public int hp = 30;
	private int initHP;
	
	void Start () {
		initHP = hp;
	}
	
	public void SubtractIntegrity (int amount){
		hp -= amount;
		if (hp < (initHP / 2) && hp >0) {
			SendMessage ("HalfHealth");
		} else if (hp <= 0) {
			SendMessage ("Destroyed");
		} else {
			SendMessage("Hit", SendMessageOptions.DontRequireReceiver);			
		}
		
	}
	
}
