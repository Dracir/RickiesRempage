using UnityEngine;
using System.Collections;

public class FoodSpawner : MonoBehaviour {
	
	private Food[] foodStuffs;
	public int minFood = 0;
	public int maxFood = 1;
	
	// Use this for initialization
	void Start () {
		foodStuffs = Resources.LoadAll<Food>("Prefabs/Food/");
	}
	
	public void SpawnFood () {
		int nb = Random.Range (minFood, maxFood);
		for (int i = 0; i < nb; i++) {
			int index = Random.Range (0, foodStuffs.Length);	
			Instantiate(foodStuffs[index], transform.position, transform.rotation);
		}
	}
}
