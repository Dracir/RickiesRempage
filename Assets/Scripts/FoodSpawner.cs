using UnityEngine;
using System.Collections;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FoodSpawner : MonoBehaviour {
	
	private Food[] foodStuffs;
	
	// Use this for initialization
	void Start () {
		foodStuffs = Resources.LoadAll<Food>("Prefabs/Food/");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F7)){
			SpawnFood();
		}
	}
	
	public void SpawnFood () {
		int index = Random.Range (0, foodStuffs.Length);
		
		Instantiate(foodStuffs[index], transform.position, transform.rotation);
	}
}
