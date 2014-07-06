using UnityEngine;
using System.Collections;
[RequireComponent(typeof(GUITexture))]
public class PowerCell : MonoBehaviour {
	
	public Texture activeTexture;
	public Texture inactiveTexture;
	public float interval = 0.2f;
	
	GUITexture tex;
	
	// Use this for initialization
	void Start () {
		tex = GetComponent<GUITexture>();
		Activate ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Activate (){
		tex.texture = activeTexture;
	}
	
	public void Deactivate (){
		tex.texture = inactiveTexture;
	}
	
	public void Poof () {
		tex.texture = null;
	}
	
	public void MoveOver (int i) {
		transform.position = new Vector3(transform.position.x + interval * i, transform.position.y, transform.position.z);
	}
	
	public bool IsActive {
		get {
			return tex.texture == activeTexture;
		}
	}
	
	public bool HasTexture {
		get{
			return tex.texture != null;
		}
	}
}
