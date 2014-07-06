using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameObjectCreator {

	private static Vector2 ROAD_COLLIDER_SIZE = new Vector2(1,4);
	private static Vector2 BUILDING_COLLIDER_SIZE = new Vector2(2,2);
	private static Vector2 TRIM_COLLIDER_SIZE = new Vector2(1,0.25f);
	private static Vector2 GARBAGE_COLLIDER_SIZE = new Vector2(0.5f,0.5f);

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
		buildings = GameObjectFactory.createGameObject ("Buildings",world.transform);
		roads = GameObjectFactory.createGameObject ("Roads",world.transform);
		props = GameObjectFactory.createGameObject ("Props",world.transform);
	}


	public void createBuilding(Vector2 position, Sprite wallSprite, Sprite trim, int buildingWidth, int buildingHeight){
		GameObject newBuilding = GameObjectFactory.createGameObject ("Building " + statNbBuilding, buildings.transform);
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
			createTrim(parent, trim, new Vector2(trimX,buildingHeight*2 - 0.5f));
			createTrim(parent, trim, new Vector2(trimX+1,0));
			createTrim(parent, trim, new Vector2(trimX+1,buildingHeight*2 - 0.5f));
		}

		if (Random.Range (0, 100) < 70) {
			createGarbage(position + new Vector2(1,0));
		}
		if (Random.Range (0, 100) < 40) {
			createRecycle(position + new Vector2(2,0));
		}
		
		statNbBuilding++;
		newBuilding.transform.Translate (position);
	}

	
	public void createRoad(Vector2 position){
		Sprite roadSprite = roadSprites [Random.Range (0, roadSprites.Length-1)];
		Vector2 p = new Vector2 (position.x , position.y - 3f);
		createSpriteGameObject (roads.transform, -2, roadSprite, p, ROAD_COLLIDER_SIZE, false);
	}
	public void createRoad(Vector2 position, Sprite sprite){
		Vector2 p = new Vector2 (position.x , position.y - 3f);
		createSpriteGameObject (roads.transform, -2, sprite, p, ROAD_COLLIDER_SIZE, false);
	}

	public void createBuildingPart(Transform parent, Sprite sprite, Vector2 position){
		GameObject part = createSpriteGameObject(parent, -10, sprite, position, BUILDING_COLLIDER_SIZE, true);

		ItemIntegrity itemIntegrity = part.AddComponent<ItemIntegrity>();
		itemIntegrity.hp = 10;
		part.AddComponent<ExampleDestruction>();
	}

	public void createTrim(Transform parent, Sprite sprite, Vector2 position){
		Vector2 p = new Vector2 (position.x , position.y - 0.75f);
		GameObject trim = createSpriteGameObject(parent, -3, sprite, p, TRIM_COLLIDER_SIZE, true);

		ItemIntegrity itemIntegrity = trim.AddComponent<ItemIntegrity>();
		itemIntegrity.hp = 10;
		trim.AddComponent<ExampleDestruction>();
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
		itemIntegrity.hp = 100;
		prop.AddComponent<ExampleDestruction>();

		Props propComp = prop.AddComponent<Props> ();
		propComp.setName (spriteName);
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
