using UnityEngine;
using System.Collections;

public class EndlessWorld : MonoBehaviour {

	public GameObjectCreator creator;

	public Vector2 createdWorldDimension = new Vector3(0,0,0);

	public int seed;
	public Random random;
	public int worldCreationDistance=20;
	public int worldCreationWidthToCreate=20;
	public float remainingWithInARow = 0;

	void Start () {
		random = new Random ();
		createdWorldDimension = Vector2.zero;
		creator.createNewWorld ();

		generateTutoStuff ();
	}

	private void generateTutoStuff(){
		Food[] foodStuffs = Resources.LoadAll<Food>("Prefabs/Food/");
		Instantiate(foodStuffs[0], new Vector3(70,0.5f,0), transform.rotation);
		Instantiate(foodStuffs[0], new Vector3(77,0.5f,0), transform.rotation);
		Instantiate(foodStuffs[1], new Vector3(83,0.5f,0), transform.rotation);
	}

	void Update () {
		if (needToCreate ()) {
			float widthRemaining = worldCreationWidthToCreate;
			while(widthRemaining > 0){
				if(remainingWithInARow <= 0){
					int skipedWidth = Random.Range(2,6);
					for(int i = 0; i < skipedWidth; i++){
						creator.createRoad(new Vector2(createdWorldDimension.x+i - (1f/2f),0));
					}
					createdWorldDimension = new Vector2 (createdWorldDimension.x + skipedWidth, createdWorldDimension.y);
					remainingWithInARow = Random.Range(19,25);

				}
				float widthCreated = generateOneBuilding();
				widthRemaining -= widthCreated;
				remainingWithInARow -= widthCreated;
			}
		}
	}

	private bool needToCreate(){
		return this.transform.position.x + worldCreationDistance > createdWorldDimension.x;
	}

	private float generateOneBuilding(){
		RandomBuildingConfig c = this.creator.config1;
		int buildingWidth = Random.Range(c.min.width,c.max.width);
		int buildingHeight = Random.Range(c.min.height,c.max.height);
		if (buildingWidth <= 0) buildingWidth = 1;
		if (buildingHeight <= 0) buildingHeight = 1;

		//float offset = Random.Range (0, 0.2f);
		Sprite buildingSprite = c.getRandomBuildingSprite ();
		Sprite trimSprite = c.getRandomTrimSprite ();

		creator.createBuilding (createdWorldDimension,buildingSprite, trimSprite, c.getRandomDoorSprite() , buildingWidth, buildingHeight);

		float wholeWidth = buildingWidth * 2;
		createdWorldDimension = new Vector2 (createdWorldDimension.x + wholeWidth, createdWorldDimension.y);
		return wholeWidth;
	}


}
