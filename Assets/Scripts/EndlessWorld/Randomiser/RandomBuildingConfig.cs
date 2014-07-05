using UnityEngine;
using System.Collections;

[System.Serializable]
public class RandomBuildingConfig {

	public Dimension min;
	public Dimension max;

	public Sprite[] buildingSprites;
	public Sprite[] trimSprites;

	public Sprite getRandomBuildingSprite(){
		return buildingSprites[Random.Range(0,buildingSprites.Length)];
	}

	public Sprite getRandomTrimSprite(){
		return trimSprites[Random.Range(0,trimSprites.Length)];
	}
}
