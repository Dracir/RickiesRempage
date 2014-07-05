using UnityEngine;
using System.Collections;
[RequireComponent(typeof(BoxCollider2D))]
public class PhysicsTest : MonoBehaviour {
	
	//basic physics properties, all in units/second
	float acceleration = 6.5f;
	float maxSpeed = 150f;
	float gravity = 8f;
	float maxfall = 220f;
	float jump = 220f;
	
	//a layer mask that I set in the Start() function
	int layerMask;
	
	//a Rectangle class has some useful tools for us
	Rect box;
	
	//my 2D velocity I use for most calculations
	Vector2 velocity;
	
	//checks
	bool grounded = false;
	bool falling = false;
	
	//variables for raycasting: how many rays, etc
	int horizontalRays = 7;
	int verticalRays = 5;
	int margin = 2;	//I don't check the very edge of the collider
	
	
	//variables for jumping
	float lastJumpDownTime = 0;
	bool jumpPressedLastFrame = false;
	float jumpPressLeeway = 0.1f;
	
	//angles and slopes
	float angleLeeway = 5f;
	
	//components
	Transform t;
	BoxCollider2D boxCol;
	
	// Use this for initialization
	void Start () {
		layerMask = 1 << LayerMask.NameToLayer("NormalCollisions");
		boxCol = GetComponent<BoxCollider2D>();
		t = transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//this line of code will save us a lot of typing
		box = new Rect(
			t.transform.position.x + boxCol.center.x - boxCol.size.x/2,
			t.transform.position.y + boxCol.center.y - boxCol.size.y/2,
			boxCol.size.x, 
			boxCol.size.y
		);
		
		//an elegant way to apply gravity. Subtract from y speed, with terminal velocity=maxfall
		if (!grounded)
			velocity = new Vector2(velocity.x, Mathf.Max(velocity.y - gravity, -maxfall));
		
		if (velocity.y < 0){
			falling = true;
		}
		
		if (grounded || falling){	//don't check anything if I'm moving up in the air
			
			Vector2 startPoint = new Vector2(box.xMin + margin, box.center.y);
			Vector2 endPoint = new Vector2(box.xMax - margin, box.center.y);
			RaycastHit2D[] hitInfos = new RaycastHit2D[verticalRays];
			
						//add half my box height since I'm starting in the centre
			float distance = box.height/2 + (grounded? margin : Mathf.Abs(velocity.y * Time.deltaTime));
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
				
				
				hitInfos[i] = Physics2D.Raycast(origin, -Vector2.up, distance);
				
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
				velocity = new Vector2(velocity.x, 0);
			}
			else{
				grounded = false;
			}
		}
		
		
		//--------------------------------------------------------------------------\\
		//-----------------------------Lateral movement-----------------------------\\
		//--------------------------------------------------------------------------\\
		
		
		float horizontalAxis = Input.GetAxisRaw("Horizontal");
		
		float newVelocityX = velocity.x;
		if (horizontalAxis != 0){		//apply movement according to input
			newVelocityX += acceleration * horizontalAxis;
			newVelocityX = Mathf.Clamp(newVelocityX, -maxSpeed, maxSpeed);
		}
		else if (velocity.x != 0){		//apply deceleration due to no input
			int modifier = velocity.x > 0? -1 : 1;
			newVelocityX += acceleration * modifier;
		}
		
		velocity = new Vector2(newVelocityX, velocity.y);
		
		if (velocity.x != 0){			//do physics checks if I'm going to move
			Vector2 startPoint = new Vector2(box.center.x, box.yMin/* + margin*/);
			Vector2 endPoint = new Vector2(box.center.x, box.yMax/* - margin*/);
			
			RaycastHit2D[] hitInfos = new RaycastHit2D[horizontalRays];
			int amountConnected = 0;
			float lastFraction = 0;
			
			float sideRayLength = box.width / 2 + Mathf.Abs(newVelocityX * Time.deltaTime);
			Vector2 direction = newVelocityX > 0? Vector2.right : -Vector2.right;
			bool connected = false;
			
			for (int i = 0; i < horizontalRays; i ++){		//go through all the rays!
				float lerpAmount = (float) i / (float) (horizontalRays -1);
				Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);
				
				//did I connect with the thing?
				hitInfos[i] = Physics2D.Raycast(origin, direction, sideRayLength);
				
				//there's a wall there don't go through it...
				if (hitInfos[i].fraction > 0){
					connected = true;
					
					if (lastFraction > 0){
						float angle = Vector2.Angle(hitInfos[i].point - hitInfos[i-1].point, Vector2.right);
						
						if (Mathf.Abs(angle - 90) < angleLeeway){
							transform.Translate(direction * (hitInfos[i].fraction * sideRayLength - box.width / 2));
							velocity = new Vector2(0, velocity.y);
							break;
						}
					}
					amountConnected ++;
				}
				lastFraction = hitInfos[i].fraction;
			}
			
			if (connected){
				
			}
			
		}
		
		//--------------------------------------------------------------------------\\
		//----------------------------Check for ceiling-----------------------------\\
		//--------------------------------------------------------------------------\\
		
		bool canJump = true;
		
		if (grounded || velocity.y > 0){
			float upRayLength = grounded? margin : velocity.y * Time.deltaTime;
			
			bool connection = false;
			int lastConnection = 0;
			Vector2 min = new Vector2(box.xMin + margin, box.center.y);
			Vector2 max = new Vector2(box.xMax - margin, box.center.y);
			RaycastHit2D[] upRays = new RaycastHit2D[verticalRays];
			
			for (int i = 0; i < verticalRays; i ++){
				Vector2 start = Vector2.Lerp(min, max, (float)i / (float) verticalRays);
				Vector2 end = start + Vector2.up * (upRayLength + box.height/2);
				upRays[i] = Physics2D.Linecast(start, end);
				if (upRays[i].fraction > 0){
					connection = true;
					lastConnection = i;
				}
			}
			
			if (connection){
				velocity = new Vector2(velocity.x, 0);
				t.position += Vector3.up * (upRays[lastConnection].point.y - box.yMax);
				SendMessage("OnHeadHit", SendMessageOptions.DontRequireReceiver);
			}
		}
		
		
		
		//--------------------------------------------------------------------------\\
		//---------------------------------Jumping----------------------------------\\
		//--------------------------------------------------------------------------\\
		
		if (canJump){
			bool input = Input.GetButton("Jump");
		
			if (input && !jumpPressedLastFrame){
				lastJumpDownTime = Time.time;
			}
			else if (!input){
				lastJumpDownTime = 0;
			}
			
			if (grounded && Time.time - lastJumpDownTime < jumpPressLeeway){
				velocity = new Vector2(velocity.x, jump);
				lastJumpDownTime = 0;
			}
			
		    jumpPressedLastFrame = input;
			
			//apply movement. Time.deltaTime=time since last frame
			transform.Translate(velocity * Time.deltaTime);
		}
		
	}
	
	void LateUpdate () {
		
	}
}
