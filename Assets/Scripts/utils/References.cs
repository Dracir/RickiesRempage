using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class References : MonoBehaviour {
	
	public AudioSettings[] sFX;
	public AudioSettings[] music;
	
	static public AudioSettings[] SFX;
	static public AudioSettings[] Music;
			
	void Awake(){
		if (Application.isPlaying){
			SetReferences();
			this.hideFlags = HideFlags.NotEditable;
		}
	}

	void Update (){
		if (!Application.isPlaying){
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			SetReferences();
		}
	}
			
	void SetReferences(){
		foreach (AudioSettings sound in sFX){
			sound.references = this;
			sound.name = sound.clip.name;
			sound.priority = Mathf.Clamp(sound.priority, 0, 255);
			sound.volume = Mathf.Clamp01(sound.volume);
			sound.dopplerLevel = Mathf.Clamp(sound.dopplerLevel, 0, 5);
			sound.minDistance = Mathf.Max(sound.minDistance, 0);
			sound.panLevel = Mathf.Clamp01(sound.panLevel);
			sound.spread = Mathf.Clamp(sound.spread, 0, 360);
			sound.maxDistance = Mathf.Max(sound.maxDistance, 1.01F);
		}
		SFX = sFX;
		
		foreach (AudioSettings sound in music){
			sound.references = this;
			sound.name = sound.clip.name;
			sound.priority = Mathf.Clamp(sound.priority, 0, 255);
			sound.volume = Mathf.Clamp01(sound.volume);
			sound.dopplerLevel = Mathf.Clamp(sound.dopplerLevel, 0, 5);
			sound.minDistance = Mathf.Max(sound.minDistance, 0);
			sound.panLevel = Mathf.Clamp01(sound.panLevel);
			sound.spread = Mathf.Clamp(sound.spread, 0, 360);
			sound.maxDistance = Mathf.Max(sound.maxDistance, 1.01F);
		}
		Music = music;
	}
	
	[System.Serializable]
	public class AudioSettings{
		[HideInInspector] public References references;
		[HideInInspector] public string name;
		[HideInInspector] public AudioSource audioSource;
		
		public AudioClip clip;
		public bool loop;
		public int priority = 128;
		public float volume = 1;
		public float pitch = 1;
		public float randomPitch;
		public float dopplerLevel = 0;
		public AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;
		public float minDistance = 0;
		public float panLevel = 1;
		public float spread = 0;
		public float maxDistance = 500;
		
		public void Play(GameObject GO){
			GameObject gameObject;
			
			gameObject = new GameObject();
			gameObject.name = "AudioSource: " + clip.name;
			gameObject.transform.parent = GO.transform;
			gameObject.transform.localPosition = Vector3.zero;
			
			audioSource = gameObject.AddComponent<AudioSource>();
			
			audioSource.clip = clip;
			audioSource.playOnAwake = false;
			audioSource.loop = loop;
			audioSource.priority = priority;
			audioSource.volume = volume;
			audioSource.pitch = Random.Range(-randomPitch, randomPitch) + pitch;
			audioSource.dopplerLevel = dopplerLevel;
			audioSource.rolloffMode = rolloffMode;
			audioSource.minDistance = minDistance;
			audioSource.panLevel = panLevel;
			audioSource.spread = spread;
			audioSource.maxDistance = maxDistance;
			audioSource.Play();
			
			references.StartCoroutine(RemoveOnFinish(audioSource));
		}

		IEnumerator RemoveOnFinish(AudioSource audioSource){
			while (audioSource.isPlaying){
				yield return new WaitForSeconds(0);
			}
			if (audioSource.transform.parent.name == "AudioSource"){
				Destroy(audioSource.transform.parent.gameObject);
			}
			else{
				Destroy(audioSource.gameObject);
			}
		}
	}
}
