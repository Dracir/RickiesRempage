using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Props : MonoBehaviour {

	private SpriteRenderer sr;

	public Sprite[] sprites;

	void Start () {

	}

	public void setName(string spriteName){
		sr = this.GetComponent<SpriteRenderer> ();

		sprites = Resources.LoadAll<Sprite> (spriteName);
		
		sr.sprite = sprites[0];
	}

	public void setSprites(Sprite[] s){
		sr = this.GetComponent<SpriteRenderer> ();
		this.sprites = s;
		sr.sprite = sprites[0];
	}

	// Update is called once per frame
	void Update () {
	
	}

	void HalfHealth () {
		sr.sprite = sprites[2];
	}
	
	void Destroyed() {
		sr.sprite = sprites[3];
		Destroy (this.GetComponent<BoxCollider2D> ());
	}

	void Hit(){
		sr.sprite = sprites[1];
	}
}
