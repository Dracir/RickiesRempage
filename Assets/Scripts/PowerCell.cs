using UnityEngine;
using System.Collections;
[RequireComponent(typeof(GUITexture))]
public class PowerCell : MonoBehaviour {
	
	public Texture activeTexture;
	public Texture inactiveTexture;
	public float interval = 0.2f;
	float growAmount = 1.3f;
	float shrinkTiming = 1.5f;
	float shrinkTimer = 0;
	Vector3 initScale;
	GUITexture tex;
	
	// Use this for initialization
	void Start () {
		tex = GetComponent<GUITexture>();
		Activate ();
		initScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localScale.magnitude > initScale.magnitude){
			Vector3.Lerp (initScale * growAmount, initScale, shrinkTimer / shrinkTiming);
			shrinkTimer += Time.deltaTime;
			if (shrinkTimer >= shrinkTiming){
				transform.localScale = initScale;
			}
		}
	}
	
	public void Activate (){
		tex.texture = activeTexture;
		Pop ();
	}
	
	public void Deactivate (){
		tex.texture = inactiveTexture;
	}
	
	public void Poof () {
		tex.texture = null;
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
			return tex.texture == activeTexture;
		}
	}
	
	public bool HasTexture {
		get{
			return tex.texture != null;
		}
	}
}
