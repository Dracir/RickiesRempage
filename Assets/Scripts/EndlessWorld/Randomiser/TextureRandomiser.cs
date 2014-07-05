using UnityEngine;
using System.Collections;

public class TextureRandomiser : MonoBehaviour {

	public Sprite[] spritesChoices;

	private Sprite choosenSprite;
	// Use this for initialization
	void Start () {
		this.choosenSprite = spritesChoices[Random.Range(0,spritesChoices.Length)];
		this.GetComponent<SpriteRenderer> ().sprite = this.choosenSprite;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
