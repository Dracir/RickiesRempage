using UnityEngine;
using System.Collections;

public class EndlessWorld : MonoBehaviour {

	public Vector2 createdWorldDimension;

	public int seed;
	public Random random;
	public int worldCreationDistance=2;
	public int worldCreationWidthToCreate=1;
	public int statNbBuilding = 0;
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
				int widthCreated = generateOneBuilding();
				widthRemaining -= widthCreated;
			}
		}
	}

	private bool needToCreate(){
		Debug.Log (this.transform.position.x + worldCreationDistance);
		return this.transform.position.x + worldCreationDistance > createdWorldDimension.x;
	}

	private int generateOneBuilding(){
		statNbBuilding++;
		int buildingWidth = 1;
		GameObject newBuilding = GameObjectFactory.createCopyGameObject (this.assetBuilding, "Building" + statNbBuilding, this.buildings);
		newBuilding.transform.Translate (createdWorldDimension);
		createdWorldDimension = new Vector2 (createdWorldDimension.x + buildingWidth, createdWorldDimension.y);
		return buildingWidth;
	}
}
