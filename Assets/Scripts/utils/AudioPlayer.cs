using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {
	
	static public void Play(string soundName, GameObject GO = null){
		if (!GO){
				GO = new GameObject();
				GO.name = "AudioSource";
				GO.transform.position = Vector3.zero;
			}
		
		foreach (References.AudioSettings m in References.Music){
			if (m.name == soundName){
				m.Play(GO);
			}
		}
		
		foreach (References.AudioSettings s in References.SFX){
			if (s.name == soundName){
				s.Play(GO);
			}
		}
	}

}
