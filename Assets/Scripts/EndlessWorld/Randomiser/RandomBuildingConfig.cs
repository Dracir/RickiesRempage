using UnityEngine;
using System.Collections;

[System.Serializable]
public class RandomBuildingConfig {

	public Dimension min;
	public Dimension max;

	public Sprite[] buildingSprites;
	public Sprite[] trimSprites;

	public Sprite[] doorsSprites;
	public Sprite[] windowsSprites;

	public Sprite getRandomBuildingSprite(){
		return buildingSprites[Random.Range(0,buildingSprites.Length)];
	}

	public Sprite getRandomTrimSprite(){
		return trimSprites[Random.Range(0,trimSprites.Length)];
	}

	public Sprite[] getRandomDoorSprite(){
		Sprite s = doorsSprites [Random.Range (0, doorsSprites.Length -1)];
		Sprite[] sprites = Resources.LoadAll<Sprite> ("Sprite/Buildings/Doors/" + s.texture.name);
		return sprites;
	}
	public Sprite[] getRandomWindowSprite(){
		Sprite s = windowsSprites [Random.Range (0, windowsSprites.Length - 1)];
		Sprite[] sprites = Resources.LoadAll<Sprite> ("Sprite/Buildings/Windows/" + s.texture.name);
		return sprites;
	}
}
