using UnityEngine;
using System.Collections;

public class FoodSpawner : MonoBehaviour {
	
	private Food[] foodStuffs;
	public float chanceToSpawn;
	
	// Use this for initialization
	void Start () {
		foodStuffs = Resources.LoadAll<Food>("Prefabs/Food/");
	}
	
	public void SpawnFood () {
		float rng = Random.Range (0f, 1f);
		if (rng <= chanceToSpawn) {
			int index = Random.Range (0, foodStuffs.Length);	
			if(index == 0){
				AudioPlayer.Play("Food_Drop_Banana",this.gameObject);
			}else{
				AudioPlayer.Play("Food_Drop_Regular",this.gameObject);
			}
			Instantiate(foodStuffs[index], transform.position, transform.rotation);	
		}
	}
}
