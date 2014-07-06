using UnityEngine;
using System.Collections;

public class MusicTest : MonoBehaviour {

	void Update () {
		if (Input.GetKeyDown(KeyCode.L)){
			AudioPlayer.Play("Rampage");
		}
		
		if (Input.GetKeyDown(KeyCode.P)){
			AudioPlayer.Play("Destruction_Glass_Big", gameObject);
		}
	}
}
