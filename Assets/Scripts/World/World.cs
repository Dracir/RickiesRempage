using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
	
	public static GameObject world;

	void Start () {
		World.world = this.gameObject;
	}
}
