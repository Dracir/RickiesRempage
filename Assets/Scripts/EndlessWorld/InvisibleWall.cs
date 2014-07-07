using UnityEngine;
using System.Collections;

public class InvisibleWall : MonoBehaviour {

	public float maxDistanceToPlayer = 50;


	void Start () {
	
	}
	

	void Update () {
		if ((Rickie.rickie.transform.position - this.transform.position).magnitude > maxDistanceToPlayer) {
			this.transform.Translate(new Vector3(10,0,0));
			removeBehind();
		}
	}

	private void removeBehind(){
		float removeBefore = this.transform.position.x - 20;
		RemovedByInvisibleWall[] childrends = World.world.GetComponentsInChildren<RemovedByInvisibleWall> ();
		foreach (var item in childrends) {
			if(item.transform.position.x < removeBefore){
				Destroy(item.gameObject);
			}	
		}

	}
}
