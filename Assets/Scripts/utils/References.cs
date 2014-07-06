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
		foreach (AudioSettings s in sFX){
			s.references = this;
			s.name = s.clip.name;
		}
		SFX = sFX;
		
		foreach (AudioSettings m in music){
			m.references = this;
			m.name = m.clip.name;
		}
		Music = music;
	}
	
	[System.Serializable]
	public class AudioSettings{
		[HideInInspector] public References references;
		[HideInInspector] public string name;
		
		public AudioClip clip;
		public bool loop;
		public float randomPitch;
		
		public void Play(GameObject GO){
			GameObject gameObject;
			AudioSource audioSource;
			
			gameObject = new GameObject();
			gameObject.name = "AudioSource: " + clip.name;
			gameObject.transform.parent = GO.transform;
			gameObject.transform.localPosition = Vector3.zero;
			
			audioSource = gameObject.AddComponent<AudioSource>();
			
			audioSource.clip = clip;
			audioSource.loop = loop;
			audioSource.pitch = Random.Range(-randomPitch, randomPitch) + 1;
			audioSource.Play();
			
			references.StartCoroutine(RemoveOnFinish(audioSource));
		}

		IEnumerator RemoveOnFinish(AudioSource audioSource){
			while (audioSource.isPlaying){
				yield return new WaitForSeconds(0);
			}
			Destroy(audioSource.gameObject);
		}
	}
}
