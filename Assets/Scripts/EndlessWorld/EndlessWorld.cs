using UnityEngine;
using System.Collections;

public class EndlessWorld : MonoBehaviour {

	public GameObjectCreator creator;

	public Vector2 createdWorldDimension;

	public int seed;
	public Random random;
	public int worldCreationDistance=2;
	public int worldCreationWidthToCreate=1;
	public int statNbBuilding = 0;
	public float remainingWithInARow = 0;

	void Start () {
		random = new Random ();
		Random.seed = seed;
		createdWorldDimension = Vector2.zero;
		creator.createNewWorld ();
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
		statNbBuilding++;
		RandomBuildingConfig c = this.creator.config1;
		int buildingWidth = Random.Range(c.min.width,c.max.width);
		int buildingHeight = Random.Range(c.min.height,c.max.height);
		if (buildingWidth <= 0) buildingWidth = 1;
		if (buildingHeight <= 0) buildingHeight = 1;

		//float offset = Random.Range (0, 0.2f);
		Sprite buildingSprite = c.getRandomBuildingSprite ();
		Sprite trimSprite = c.getRandomTrimSprite ();


		GameObject newBuilding = GameObjectFactory.createGameObject ("Building " + statNbBuilding, this.creator.buildings.transform);
		Transform parent = newBuilding.transform;
		for (int x = 0; x < buildingWidth; x++) {
			for (int y = 0; y < buildingHeight; y++) {
				creator.createBuildingPart(parent, buildingSprite, new Vector2(x*2,y*2));
			}
			float roadX = createdWorldDimension.x + x*2 - (1f/2f);
			creator.createRoad(new Vector2(roadX,0));
			creator.createRoad(new Vector2(roadX + 1f,0));
			float trimX = x*2 - (1f/2f);
			creator.createTrim(parent, trimSprite, new Vector2(trimX,0));
			creator.createTrim(parent, trimSprite, new Vector2(trimX,buildingHeight*2 - 0.75f));
			creator.createTrim(parent, trimSprite, new Vector2(trimX+1,0));
			creator.createTrim(parent, trimSprite, new Vector2(trimX+1,buildingHeight*2 - 0.75f));
		}

		float wholeWidth = buildingWidth * 2;
		newBuilding.transform.Translate (createdWorldDimension);
		createdWorldDimension = new Vector2 (createdWorldDimension.x + wholeWidth, createdWorldDimension.y);
		return wholeWidth;
	}


}
