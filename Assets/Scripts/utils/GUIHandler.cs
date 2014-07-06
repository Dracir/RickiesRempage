using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIHandler : MonoBehaviour {
	
	private PowerCell[] cells;
	public PowerCell protoCell;
	public GUIText scoreObject;
	
	private int cellNumber;
	public static int score = 0;
	
	public static GUIHandler instance;
	
	// Use this for initialization
	private string initScoreString;
	void Awake () {
		if(instance == null){
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}
	void Start () {
		if (!protoCell){
			Debug.LogWarning("You need to set the protoCell in the inspector!");
		}
		if (!scoreObject){
			Debug.LogWarning("Set the score object in the inspector!");
		}
		
		initScoreString = scoreObject.text;
		
		cellNumber = Rickie.maxPower;
		
		List<PowerCell> cellList = new List<PowerCell>();
		
		cellList.Add (protoCell);
		
		for (int i = 1; i < cellNumber; i ++){
			PowerCell newGuy = Instantiate(protoCell, protoCell.transform.position, protoCell.transform.rotation) as PowerCell;
			//PowerCell cell = newGuy.GetComponent<PowerCell>();
			cellList.Add (newGuy);
			newGuy.transform.parent = protoCell.transform.parent;
			PowerCell dude = cellList[cellList.Count - 1];
			//dude.Activate ();
			dude.MoveOver(i);
		}
		
		cells = cellList.ToArray();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void LosePower () {
		int index = 0;
		for (index = cells.Length - 1; index >= 0; index --){
			if (cells[index].IsActive){
				cells[index].Deactivate();
				return;
			}
		}
		
		Debug.Log ("RAMPAGE!");
	}
	
	public void RampageTick () {
		int index = 0;
		for (index = cells.Length - 1; index >= 0; index --){
			if (cells[index].HasTexture){
				cells[index].Poof();
				return;
			}
		}
		Debug.Log ("GAME OVER");
	}
	
	public void AddPower(int value){
		int index = 0;
		for (index = cells.Length - 1; index >= 0; index --){
			if (cells[index].IsActive){
				index --;
				break;
			}
		}
		
		for (int i = index; i < index + value; i ++){
			if (i >= cells.Length){
				return;
			}
			cells[i].Activate ();
		}
	}
	
	public void AddPoints(int value){
		score += value;
		string scoreString = score.ToString();
		string endString = "";
		for (int i = 0; i < scoreString.Length; i ++){
			endString += "0";
		}
		if (endString == ""){
			if (scoreString.Length > initScoreString.Length){
				List<char> newChars = new List<char>();
				for (int i = 0; i < scoreString.Length; i ++){
					newChars.Add ('9');
				}
				scoreString = new string(newChars.ToArray());
				score = int.Parse (scoreString);
			}
			
		} else {
			scoreString = endString + scoreString;
		}
		
		scoreObject.text = scoreString;
	}
}
