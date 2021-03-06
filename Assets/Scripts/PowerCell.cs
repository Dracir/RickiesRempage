﻿using UnityEngine;
using System.Collections;
[RequireComponent(typeof(GUITexture))]
public class PowerCell : MonoBehaviour {
	
	public Texture activeTexture;
	public Texture inactiveTexture;
	public float interval = 0.2f;
	float growAmount = 1.5f;
	float shrinkTiming = 0.4f;
	float shrinkTimer = 0;
	Vector3 initScale;
	GUITexture tex;
	GUITexture Tex{
		get{
			if (!tex){
				tex = GetComponent<GUITexture>();
			}
			return tex;
		}
	}
	
	// Use this for initialization
	void Start () {
		initScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localScale.magnitude > initScale.magnitude){
			transform.localScale = Vector3.Lerp (initScale * growAmount, initScale, shrinkTimer / shrinkTiming);
			shrinkTimer += Time.deltaTime;
			if (shrinkTimer >= shrinkTiming){
				transform.localScale = initScale;
			}
		}
	}
	
	public void Activate (){
		Tex.texture = activeTexture;
		if (initScale != Vector3.zero){
			Pop ();
		}
	}
	
	public void Deactivate (){
		Tex.texture = inactiveTexture;
	}
	
	public void Poof () {
		Tex.texture = null;
	}
	
	public void Pop () {
		transform.localScale = initScale * growAmount;
		shrinkTimer = 0;
	}
	
	public void MoveOver (int i) {
		transform.position = new Vector3(transform.position.x + interval * i, transform.position.y, transform.position.z);
	}
	
	public bool IsActive {
		get {
			return Tex.texture == activeTexture;
		}
	}
	
	public bool HasTexture {
		get{
			return Tex.texture != null;
		}
	}
}
