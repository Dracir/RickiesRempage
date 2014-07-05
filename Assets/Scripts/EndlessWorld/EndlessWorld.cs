using UnityEngine;
using System.Collections;

public class EndlessWorld : MonoBehaviour {

	public Vector2 createdWorldDimension;

	public int seed;
	public Random random;
	public int worldCreationDistance=2;
	public int worldCreationWidthToCreate=1;
	public int statNbBuilding = 0;
	public int buildingInARow = 0;
	public GameObject world;
	public GameObject buildings;

	public GameObject assetBuilding;
	public GameObject assetRoad;

	void Start () {
		world = GameObjectFactory.createGameObject ("World");
		buildings = GameObjectFactory.createGameObject ("Buildings",world.transform);

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
			int widthRemaining = worldCreationWidthToCreate;
			while(widthRemaining > 0){
				//if(buildingInARow )
				int widthCreated = generateOneBuilding();
				widthRemaining -= widthCreated;
			}
		}
	}

	private bool needToCreate(){
		return this.transform.position.x + worldCreationDistance > createdWorldDimension.x;
	}

	private int generateOneBuilding(){
		statNbBuilding++;
		int buildingWidth = Random.Range(1,3);
		GameObject newBuilding = GameObjectFactory.createGameObject ("BuildingPart" + statNbBuilding, this.buildings.transform);
		for (int x = 0; x < buildingWidth; x++) {
			for (int y = 0; y < Random.Range(1,5); y++) {
				GameObject buildingPart = GameObjectFactory.createCopyGameObject (this.assetBuilding, "Part" + statNbBuilding, newBuilding);
				buildingPart.transform.Translate(new Vector2(x,y));
			}		
		}
		Debug.Log (buildingWidth * 2);
		newBuilding.transform.Translate (createdWorldDimension);
		createdWorldDimension = new Vector2 (createdWorldDimension.x + buildingWidth * 2, createdWorldDimension.y);
		return buildingWidth * 2;
	}
}
