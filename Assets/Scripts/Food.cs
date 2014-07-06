using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {
	
	public int powerValue = 15;
	
	private float gravity = 8f;
	private Vector2 velocity;
	
	private float speed = 3f;
	private float MaxFall {
		get{ return 5f; }
	}
	private float Margin {
		get{ return boxCol.size.x / 4f; }
	}
	private Rect box;
	private bool grounded = false;
	private bool falling;
	private Transform t;
	private BoxCollider2D boxCol;
	private int verticalRays = 4;
	private int layerMask;
	
	private bool canCollect = false;
	
	// Use this for initialization
	void Start () {
		velocity = Random.insideUnitCircle * speed;
		velocity = new Vector2(velocity.x, Mathf.Abs (velocity.y));
		
		t = transform;
		boxCol = GetComponent<BoxCollider2D>();
		layerMask = 1 << LayerMask.NameToLayer("NormalCollisions");
		
	}
	// Update is called once per frame
	void FixedUpdate () {
		box = new Rect(
			t.transform.position.x + boxCol.center.x - boxCol.size.x/2,
			t.transform.position.y + boxCol.center.y - boxCol.size.y/2,
			boxCol.size.x, 
			boxCol.size.y
		);
		
		//an elegant way to apply gravity. Subtract from y speed, with terminal velocity=maxfall
		if (!grounded)
			velocity = new Vector2(velocity.x, velocity.y - gravity * Time.deltaTime);
		
		if (velocity.y < 0){
			falling = true;
		}
		
		if (falling){	//don't check anything if I'm moving up in the air
			
			Vector2 startPoint = new Vector2(box.xMin + Margin, box.center.y);
			Vector2 endPoint = new Vector2(box.xMax - Margin, box.center.y);
			RaycastHit2D[] hitInfos = new RaycastHit2D[verticalRays];
			
						//add half my box height since I'm starting in the centre
			float distance = box.height/2 + (grounded? Margin : Mathf.Abs(velocity.y * Time.deltaTime));
										//this is a ternary operator that chooses how long
										//the ray will be based on whether I'm grounded
			float smallestFraction = Mathf.Infinity;
			int indexUsed = 0;
			
			//check if I hit anything. Starts false because if any ray connects I'm grounded
			bool connected = false;
			
			for (int i = 0; i < verticalRays; i ++){
									//verticalRays -1 because otherwise we don't get to 1.0
				float lerpAmount = (float)i / (float) (verticalRays - 1);
				Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);
				
				
				hitInfos[i] = Physics2D.Raycast(origin, -Vector2.up, distance, layerMask);
				if (hitInfos[i].fraction > 0){
					connected = true;
					if (hitInfos[i].fraction < smallestFraction){
						indexUsed = i;
						smallestFraction = hitInfos[i].fraction;
					}
				}
			}
			
			if (connected){
				grounded = true;
				falling = false;
				transform.Translate(Vector3.down * (hitInfos[indexUsed].fraction * distance - box.height/2));
				velocity = new Vector2(0, 0);
				canCollect = true;
			}
			else{
				grounded = false;
				
			}
		}
		t.Translate (velocity * Time.deltaTime);
	}
	
	void OnTriggerEnter2D (Collider2D other){
		if (!canCollect){
			return;
		}
		Rickie rick = other.GetComponent<Rickie>();
		
		if (rick){
			rick.AddPower(powerValue);
			Destroy (gameObject);
		}
	}
}
