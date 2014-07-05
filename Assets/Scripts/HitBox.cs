using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour {

	CircleCollider2D col;
	public int damage = 10;
	void Start (){
		Destroy (gameObject, 0.3f);
	}
	void OnTriggerEnter2D (Collider2D other){
		ItemIntegrity item = other.GetComponent<ItemIntegrity>();
		
		if (item){
			item.SubtractIntegrity(damage + Rickie.rickie.ExtraDamage);
			Destroy (gameObject);
		}
	}
}
