using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Props : MonoBehaviour {

	private SpriteRenderer sr;
	public string spriteName;

	public Sprite[] sprites;

	void Start () {

	}

	public void setName(string spriteName){
		this.spriteName = spriteName;
		sr = this.GetComponent<SpriteRenderer> ();

		sprites = Resources.LoadAll<Sprite> (spriteName);
		
		sr.sprite = sprites[0];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void HalfHealth () {
		Debug.Log ("YOO 1/2");
		sr.sprite = sprites[2];
	}
	
	void Destroyed() {
		Debug.Log ("YOO");
		sr.sprite = sprites[3];
	}

	void Hit(){
		Debug.Log ("aaa");
		sr.sprite = sprites[1];
	}
}
