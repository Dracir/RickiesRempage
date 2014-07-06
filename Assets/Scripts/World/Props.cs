using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Props : MonoBehaviour {

	private SpriteRenderer sr;

	public Sprite[] sprites;
	public string destructionSound;
	public string hitSound;
	public float hitShake;
	public float destroyedShake;
	public float halfLifeShake;
	public float shakeStrenght;
	public Vector3 destroyedTranslation;

	private float shakeTime;
	private Vector3 initialPosition;
	private bool halfLifed =false;
	private bool destroyed = false;


	void Start () {
		this.initialPosition = this.transform.localPosition;
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
		if (destroyed) return;

		if (shakeTime > 0f) {
			shakeTime -= Time.deltaTime;
			Vector2 v = Random.insideUnitCircle;
			this.transform.localPosition = initialPosition + new Vector3(v.x,v.y,0) * shakeStrenght;
		} else {
			this.transform.localPosition = this.initialPosition;
			shakeTime = 0;
		}
	}

	void HalfHealth () {
		halfLifed = true;
		AudioPlayer.Play(this.hitSound,this.gameObject);
		sr.sprite = sprites[2];
		startShake (halfLifeShake);
	}
	
	void Destroyed() {
		sr.sprite = sprites[3];
		AudioPlayer.Play(this.destructionSound,this.gameObject);
		Destroy (this.GetComponent<BoxCollider2D> ());
		startShake (destroyedShake);
		if (destroyedTranslation != null) {
			this.transform.Translate(destroyedTranslation);
		}
		destroyed = true;
	}

	void Hit(){
		if (!halfLifed) {
			sr.sprite = sprites [1];
		} else {
			AudioPlayer.Play(this.hitSound,this.gameObject);		
		}

		startShake (hitShake);
	}

	private void startShake(float time){
		if (shakeTime == 0) {
			this.initialPosition = this.transform.localPosition;
		}
		shakeTime += time;
	}

}
