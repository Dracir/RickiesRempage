using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

	public Transform toFollow;
	
	Transform trans;
	
	private float lerpAmount = 0.1f;
	
	public bool followInYAxis = false;
	public static CameraFollow cam;
	
	// Use this for initialization
	
	void Awake () {
		trans = transform;
		if (cam == null){
			cam = this;
		} else {
			Destroy(this);
		}
	}
	
	void Start () {
		if (toFollow == null){
			
			Debug.LogError ("There's no follow object assigned in the inspector. FAIL");
			
			
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
			Vector3 target = new Vector3(toFollow.position.x , (followInYAxis? toFollow.position.y : trans.position.y) , trans.position.z);
			trans.position = Vector3.Lerp(trans.position, target, lerpAmount);
			
	}
	
}
