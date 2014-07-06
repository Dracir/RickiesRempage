using UnityEngine;
using System.Collections;

public class MusicTest : MonoBehaviour {

	AudioSource music;
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.L)){
			music = AudioPlayer.Play("Rampage");
		}
		
		if (Input.GetKeyDown(KeyCode.P)){
			AudioPlayer.Play("Destruction_Glass_Big", gameObject);
		}
		
		if (Input.GetKeyDown(KeyCode.K)){
			if (music){
				music.Stop();
			}
		}
	}
}
