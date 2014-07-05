using UnityEngine;
using System.Collections;

public class EndlessWorld : MonoBehaviour {

	public Vector2 createdWorldDimension;

	public int seed;
	public Random random;
	public int worldCreationDistance=2;
	public int worldCreationWidthToCreate=1;
	public int statNbBuilding = 0;
	public float remainingWithInARow = 0;
	public GameObject world;
	public GameObject buildings;
	public GameObject roads;

	public GameObject assetBuilding;
	public GameObject assetRoad;

	public RandomBuildingConfig config1;
	public Sprite[] roadSprites;

	void Start () {
		world = GameObjectFactory.createGameObject ("World");
		buildings = GameObjectFactory.createGameObject ("Buildings",world.transform);
		roads = GameObjectFactory.createGameObject ("Roads",world.transform);

		random = new Random ();
		Random.seed = seed;
		createdWorldDimension = Vector2.zero;

		loadAssets ();
	}

	void loadAssets(){
		this.assetBuilding = Resources.Load<GameObject>("Prefab/Building");
		this.assetRoad = Resources.Load<GameObject>("Prefab/Road");
	}

	void Update () {
		if (needToCreate ()) {
			float widthRemaining = worldCreationWidthToCreate;
			while(widthRemaining > 0){
				if(remainingWithInARow <= 0){
					int skipedWidth = Random.Range(2,6);
					for(int i = 0; i < skipedWidth; i++){
						Sprite roadSprite = roadSprites [Random.Range (0, roadSprites.Length)];
						createSpriteGameObject(roads.transform, 2, roadSprite, new Vector2(createdWorldDimension.x+i - (1f/2f),-3f));
					}
					createdWorldDimension = new Vector2 (createdWorldDimension.x + skipedWidth, createdWorldDimension.y);
					remainingWithInARow = Random.Range(10,20);

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
		RandomBuildingConfig c = this.config1;
		int buildingWidth = Random.Range(c.min.width,c.max.width);
		int buildingHeight = Random.Range(c.min.height,c.max.height);
		if (buildingWidth <= 0) buildingWidth = 1;
		if (buildingHeight <= 0) buildingHeight = 1;

		//float offset = Random.Range (0, 0.2f);
		Sprite buildingSprite = c.getRandomBuildingSprite ();
		Sprite trimSprite = c.getRandomTrimSprite ();
		Sprite roadSprite = roadSprites [Random.Range (0, roadSprites.Length)];

		GameObject newBuilding = GameObjectFactory.createGameObject ("Building " + statNbBuilding, this.buildings.transform);
		for (int x = 0; x < buildingWidth; x++) {
			for (int y = 0; y < buildingHeight; y++) {
				createSpriteGameObject(newBuilding.transform, 0, buildingSprite, new Vector2(x*2,y*2));
			}
			float roadX = createdWorldDimension.x + x*2 - (1f/2f);
			createSpriteGameObject(roads.transform, 2, roadSprite, new Vector2(roadX,-3f));
			createSpriteGameObject(roads.transform, 2, roadSprite, new Vector2(roadX+1f,-3f));
			float trimX = x*2 - (1f/2f);
			createSpriteGameObject(newBuilding.transform, 1, trimSprite, new Vector2(trimX,-0.75f));
			createSpriteGameObject(newBuilding.transform, 1, trimSprite, new Vector2(trimX,buildingHeight*2 - 1.25f));
			createSpriteGameObject(newBuilding.transform, 1, trimSprite, new Vector2(trimX+1f,-0.75f));
			createSpriteGameObject(newBuilding.transform, 1, trimSprite, new Vector2(trimX+1f,buildingHeight*2 - 1.25f));
		}




		float wholeWidth = buildingWidth * 2;
		newBuilding.transform.Translate (createdWorldDimension);
		createdWorldDimension = new Vector2 (createdWorldDimension.x + wholeWidth, createdWorldDimension.y);
		return wholeWidth;
	}

	private void createSpriteGameObject(Transform parent, int sortingOrder, Sprite sprite, Vector2 position){
		GameObject buildingPart = GameObjectFactory.createGameObject("Part", parent);
		buildingPart.AddComponent<SpriteRenderer>();
		SpriteRenderer sr = buildingPart.GetComponent<SpriteRenderer> ();
		sr.sprite = sprite;
		sr.sortingOrder = sortingOrder;

		buildingPart.transform.Translate(position);
	}
}
