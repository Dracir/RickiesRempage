using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameObjectCreator {

	private static Vector2 ROAD_COLLIDER_SIZE = new Vector2(1,4);
	private static Vector2 BUILDING_COLLIDER_SIZE = new Vector2(2,2);
	private static Vector2 TRIM_COLLIDER_SIZE = new Vector2(1,0.25f);

	public GameObject world;
	public GameObject buildings;
	public GameObject roads;

	public RandomBuildingConfig config1;
	public Sprite[] roadSprites;


	public GameObjectCreator(){
	
	}

	public void createNewWorld(){
		world = GameObjectFactory.createGameObject ("World");
		buildings = GameObjectFactory.createGameObject ("Buildings",world.transform);
		roads = GameObjectFactory.createGameObject ("Roads",world.transform);
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


	private GameObject createSpriteGameObject(Transform parent, int sortingOrder, Sprite sprite, Vector2 position, Vector2 colliderSize, bool isTrigger){
		GameObject buildingPart = GameObjectFactory.createGameObject("Part", parent);

		SpriteRenderer sr = buildingPart.AddComponent<SpriteRenderer>();
		sr.sprite = sprite;
		sr.sortingOrder = sortingOrder;


		BoxCollider2D box = buildingPart.AddComponent<BoxCollider2D>();
		box.size = new Vector2 (1, 4);//colliderSize;
		//box.isTrigger = isTrigger;
		buildingPart.layer = LayerMask.NameToLayer("NormalCollisions");
		
		buildingPart.transform.Translate(position);

		return buildingPart;
	}

}
