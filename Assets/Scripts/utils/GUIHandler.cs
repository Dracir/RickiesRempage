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

	public PopupConfiguration popupConfiguration;
	
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
	int loseDelay = 120;
	int loseCounter = 0;
	// Update is called once per frame
	void Update () {
		if (loseCounter > 0){
			loseCounter --;
			if (loseCounter <= 0){
				Restart ();
			}
		}
	}
	
	public void LosePower () {
		int index = 0;
		for (index = cells.Length - 1; index >= 0; index --){
			if (cells[index].IsActive){
				cells[index].Deactivate();
				return;
			}
		}
		
		//RAMPAGE
		isRampaging = true;
		AudioPlayer.Play ("Rampage-01");
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
		AudioPlayer.Play ("YouLose-01", Rickie.rickie.gameObject);
		Time.timeScale = 0;
		loseCounter = loseDelay;
	}
	
	void Restart () {
		Application.LoadLevel(Application.loadedLevel);
	}
	private bool isRampaging;
	public void AddPower(int value){
		if (isRampaging){
			return;
		}
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

	public void AddPoints(Vector3 pointSource, int value){
		Vector3 v = this.scoreObject.transform.position;
		Vector2 v2 = new Vector2 (v.x * Screen.width, (1-v.y + 0.05f) * Screen.height);
		PopupText p = PopupFactory.makeLinearPopup (this.popupConfiguration, value + "", Camera.main.WorldToScreenPoint (pointSource), v2, 0.1f, 0.9f);
		ScreenEffectSystem.AddScreenEffect (p);
		AddPoints (value);
	}

	public void AddPoints(int value){
		score += value;
		string scoreString = score.ToString();
		string endString = "";
		for (int i = 0; i < initScoreString.Length - scoreString.Length; i ++){
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
		scoreObject.SendMessage("Pop");
	}
}
