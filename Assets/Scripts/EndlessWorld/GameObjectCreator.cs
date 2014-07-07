using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameObjectCreator {

	private static Vector2 ROAD_COLLIDER_SIZE = new Vector2(1,3.7f);
	private static Vector2 BUILDING_COLLIDER_SIZE = new Vector2(2,2);
	private static Vector2 TRIM_COLLIDER_SIZE = new Vector2(1,0.25f);
	private static Vector2 DOOR_COLLIDER_SIZE = new Vector2(1f,2f);

	private static Vector2 GARBAGE_COLLIDER_SIZE = new Vector2(1.25f,1.7f);

	public GameObject world;
	public GameObject buildings;
	public GameObject roads;
	public GameObject props;

	public RandomBuildingConfig config1;
	public Sprite[] roadSprites;

	
	public int statNbBuilding = 0;

	public GameObjectCreator(){
	
	}

	public void createNewWorld(){
		world = GameObjectFactory.createGameObject ("World");
		world.AddComponent<World> ();
		buildings = GameObjectFactory.createGameObject ("Buildings",world.transform);
		roads = GameObjectFactory.createGameObject ("Roads",world.transform);
		props = GameObjectFactory.createGameObject ("Props",world.transform);
	}


	public void createBuilding(Vector2 position, Sprite wallSprite, Sprite trim, Sprite[] door, int buildingWidth, int buildingHeight){
		GameObject newBuilding = GameObjectFactory.createGameObject ("Building " + statNbBuilding, buildings.transform);
		newBuilding.AddComponent<RemovedByInvisibleWall> ();
		Transform parent = newBuilding.transform;
		for (int x = 0; x < buildingWidth; x++) {
			for (int y = 0; y < buildingHeight; y++) {
				createBuildingPart(parent, wallSprite, new Vector2(x*2,y*2));
			}
			float roadX = position.x + x*2 - (1f/2f);
			createRoad(new Vector2(roadX,0));
			createRoad(new Vector2(roadX + 1f,0));
			float trimX = x*2 - (1f/2f);
			createTrim(parent, trim, new Vector2(trimX,0));
			createTrim(parent, trim, new Vector2(trimX,buildingHeight*2 - 0.1f));
			createTrim(parent, trim, new Vector2(trimX+1,0));
			createTrim(parent, trim, new Vector2(trimX+1,buildingHeight*2 - 0.1f));
		}

		int doorX = Random.Range (0, buildingWidth);
		createDoor (parent, new Vector2 (doorX, 0), door);

		if (Random.Range (0, 100) < 70) {
			createGarbage(position + new Vector2(doorX + Random.Range(1.3f,2f),-0.2f));
			if (Random.Range (0, 100) < 50) {
				createRecycle(position + new Vector2(doorX + Random.Range(3f,4f),-0.2f));
			}
		}else if (Random.Range (0, 100) < 60) {
			createRecycle(position + new Vector2(doorX + Random.Range(1.3f,2f),-0.2f));
		}
		
		statNbBuilding++;
		newBuilding.transform.Translate (position);
	}

	
	public void createRoad(Vector2 position){
		Sprite roadSprite = roadSprites [Random.Range (0, roadSprites.Length-1)];
		Vector2 p = new Vector2 (position.x , position.y - 3f);
		GameObject o = createSpriteGameObject (roads.transform, -20, roadSprite, p, ROAD_COLLIDER_SIZE, false);
		o.AddComponent<RemovedByInvisibleWall> ();
	}
	public void createRoad(Vector2 position, Sprite sprite){
		Vector2 p = new Vector2 (position.x , position.y - 3f);
		GameObject o = createSpriteGameObject (roads.transform, -20, sprite, p, ROAD_COLLIDER_SIZE, false);
		o.AddComponent<RemovedByInvisibleWall> ();
	}

	public void createBuildingPart(Transform parent, Sprite sprite, Vector2 position){
		/*GameObject part = */createSpriteGameObject(parent, -10, sprite, position, BUILDING_COLLIDER_SIZE, true);

		//ItemIntegrity itemIntegrity = part.AddComponent<ItemIntegrity>();
		//itemIntegrity.hp = 10;
		//part.AddComponent<ExampleDestruction>();
	}

	public void createTrim(Transform parent, Sprite sprite, Vector2 position){
		Vector2 p = new Vector2 (position.x , position.y - 0.88f);
		GameObject trim = createSpriteGameObject(parent, -9, sprite, p, TRIM_COLLIDER_SIZE, true);

		ItemIntegrity itemIntegrity = trim.AddComponent<ItemIntegrity>();
		itemIntegrity.hp = 10;
		trim.AddComponent<ExampleDestruction>();
	}
	
	public void createDoor(Transform parent, Vector2 position, Sprite[] sprites){
		GameObject prop = createSpriteGameObject (parent, -8, null, position, DOOR_COLLIDER_SIZE, true);
		
		ItemIntegrity itemIntegrity = prop.AddComponent<ItemIntegrity>();
		itemIntegrity.hp = 100;
		
		Props propComp = prop.AddComponent<Props> ();
		propComp.destroyedShake = 0.25f;
		propComp.destructionSound = "Destruction_Glass_Big";
		propComp.halfLifeShake = 0.3f;
		propComp.hitShake = 0.1f;
		propComp.hitSound = "Hit_Metal_Medium";
		propComp.shakeStrenght = 0.05f;
		propComp.setSprites(sprites);
		propComp.pointsForDestroyed = 78;
		propComp.pointsForHited = 10;

		FoodSpawner fs = prop.AddComponent<FoodSpawner>();
		fs.minFood = 2;
		fs.minFood = 3;
	}

	public void createGarbage(Vector2 position){
		createProps (position, "Sprite/Props/LevelSprite-GarbageCan");
	}

	public void createRecycle(Vector2 position){
		createProps (position, "Sprite/Props/LevelSprite-RecycleBin");
	}

	public void createProps(Vector2 position, string spriteName){
		GameObject prop = createSpriteGameObject (props.transform, -2, null, position, GARBAGE_COLLIDER_SIZE, true);

		ItemIntegrity itemIntegrity = prop.AddComponent<ItemIntegrity>();
		itemIntegrity.hp = 40;

		Props propComp = prop.AddComponent<Props> ();
		propComp.setName (spriteName);
		propComp.destroyedShake = 0.35f;
		propComp.destructionSound = "Destruction_Glass_Big";
		propComp.halfLifeShake = 0.4f;
		propComp.hitShake = 0.15f;
		propComp.hitSound = "Destruction_Rock_Medium";
		propComp.shakeStrenght = 0.1f;
		propComp.destroyedTranslation = new Vector3 (0,-0.3f,0);
		propComp.pointsForDestroyed = 54;
		propComp.pointsForHited = 5;

		FoodSpawner fs = prop.AddComponent<FoodSpawner>();
		fs.minFood = 0;
		fs.minFood = 1;

		prop.AddComponent<RemovedByInvisibleWall> ();
	}


	private GameObject createSpriteGameObject(Transform parent, int sortingOrder, Sprite sprite, Vector2 position, Vector2 colliderSize, bool isTrigger){
		GameObject buildingPart = GameObjectFactory.createGameObject("Part", parent);

		SpriteRenderer sr = buildingPart.AddComponent<SpriteRenderer>();
		sr.sprite = sprite;
		sr.sortingOrder = sortingOrder;


		BoxCollider2D box = buildingPart.AddComponent<BoxCollider2D>();
		box.size = colliderSize;
		box.isTrigger = isTrigger;
		if (!isTrigger) {
			buildingPart.layer = LayerMask.NameToLayer("NormalCollisions");		
		}

		
		buildingPart.transform.Translate(position);

		return buildingPart;
	}

}
