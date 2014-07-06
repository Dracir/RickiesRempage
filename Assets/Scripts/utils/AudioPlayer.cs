using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {
	
	static public AudioSource Play(string soundName, GameObject GO = null){
		AudioSource audioSource = new AudioSource();
		
		if (!GO){
				GO = new GameObject();
				GO.name = "AudioSource";
				GO.transform.position = Vector3.zero;
			}
		
		foreach (References.AudioSettings sound in References.Music){
			if (sound.name == soundName){
				sound.Play(GO);
				audioSource = sound.audioSource;
			}
		}
		
		foreach (References.AudioSettings sound in References.SFX){
			if (sound.name == soundName){
				sound.Play(GO);
				audioSource = sound.audioSource;
			}
		}
		return audioSource;
	}

}
