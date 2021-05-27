using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEditor;
using UnityEngine.SceneManagement;

using System;

public class FinishTurn : MonoBehaviour
{
    // Start is called before the first frame update
	public GameHandler gameHandler = null;
    public AudioSource StartAudio ;

	public FinishTurn(){

	}

	
	public void onClick(){
		if(gameHandler.gameStatus == "not started" && gameHandler.player.My3OpenCards.Count == 3){
			
			Image image= GameObject.Find("DoneButton").transform.GetComponent<Image>();
			image.color = new Color32(255, 255, 255, 255);

			GameObject.Find("DoneButton").GetComponentInChildren<Text>().text = "DONE";
			gameHandler.gameStatus = "started";
			StartAudio.Play();
			// Debug.Log ("You have clicked Finish Turn button!");
		}
		else if(gameHandler.gameStatus == "started"){

		if(gameHandler.TempOpenDeck.cardsToApply.Count>0 ){	
			int value= gameHandler.TempOpenDeck.currListVal;
			int count= gameHandler.TempOpenDeck.cardsToApply.Count;
			string shape=gameHandler.TempOpenDeck.transform.GetChild(0).GetComponent<CardObject>().shape; // will help if its 10 || 4 same cards;


			// int numOfCardsToPull=Math.min()
			bool deckCleaned=gameHandler.TempOpenDeck.PutNewCard( value,count);
			if(deckCleaned){
				StartCoroutine(gameHandler.player. holdAndResume(gameHandler.player, value , shape));	

			}
			if(gameHandler.player.MyCards.Count<3){
				// Debug.Log("curr count: "+gameHandler.player.MyCards.Count);
				gameHandler.CloseDeck.pullCardsToUser(gameHandler.player,3-gameHandler.player.MyCards.Count,true);
				// gameHandler.player.MyCards.Count=3;
			}
			if(gameHandler.player.MyCards.Count==0){
				if(gameHandler.player.My3OpenCards.Count ==3){
					for(int i=0; i<3 ; i++){
						Transform My_open_trans = gameHandler.player.MyOpenCardsArea.transform;
						GameObject go=gameHandler.player.My3OpenCards[0];
						CardObject card = go.transform.GetComponent<CardObject>();
						// card.enabled =true; //////////////????????????????
						card.isShared=false;

						gameHandler.player.My3OpenCards[0].transform.SetParent(My_open_trans);
						gameHandler.player.MyCards.Add(go);
						gameHandler.player.My3OpenCards.Remove(go);
					}
				}
				else{ // finished shared cards and my cards
					

				}

			}
			if(value!=8 && value!=10 && !deckCleaned){
				gameHandler.PcPlayer.MyTurn();
			}
		}
		else{
			
			Popup popup = UIController.Instance.CreatePopup();
			popup.Init2Buttons(gameHandler.gameObject.transform,
				"Are you sure you want to submit 0 cards? you will get all the cards.",
				"Yes",
				"Cancel"

			 );
			
		}

		gameHandler.player.SortMycards();

		if(gameHandler.gameStatus=="started" && gameHandler.player.MyCards.Count==0 && gameHandler.player.My3CloseCards.Count==0){
            StartCoroutine( gameHandler.player.playerWonCorrutine(gameHandler.player));
			Debug.Log("won from finish turn");
            return;
        }

		}
		
	}
	public void NewGame(){
		gameHandler.HoldGameStatus=gameHandler.gameStatus;
		gameHandler.gameStatus="holdNewGame";
		Popup popup = UIController.Instance.CreatePopup();
		popup.Init2Buttons(gameHandler.gameObject.transform,
			"Are you sure you want to start a new game?",
			"Yes",
			"Cancel"

			);
		
	}
	public void ExitGame(){
		gameHandler.HoldGameStatus=gameHandler.gameStatus;
		gameHandler.gameStatus="holdExit";
		Popup popup = UIController.Instance.CreatePopup();
		popup.Init2Buttons(gameHandler.gameObject.transform,
			"Are you sure you want to exit?",
			"Exit",
			"Cancel"

			);
		
	}


	public void MuteAudio(){
		Text txtTrans=GameObject.Find("MuteButton").transform.GetChild(0).GetComponent<Text>();

		if(txtTrans.text=="MUTE"){
			txtTrans.text="UNMUTE";
		}
		else{
			txtTrans.text="MUTE";
		}

		AudioSource PullCard_audio = GameObject.Find("PullCard_audio").GetComponent<AudioSource>();
		AudioSource	StartGame_audio = GameObject.Find("StartGame_audio").GetComponent<AudioSource>();
		AudioSource LayCard_audio = GameObject.Find("LayCard_audio").GetComponent<AudioSource>();
		AudioSource Win_audio = GameObject.Find("Win_audio").GetComponent<AudioSource>();
		AudioSource start_clicked_audio = GameObject.Find("start_clicked_audio").GetComponent<AudioSource>();
		
		PullCard_audio.mute = !PullCard_audio.mute;
		StartGame_audio.mute = !StartGame_audio.mute;
		LayCard_audio.mute = !LayCard_audio.mute;
		Win_audio.mute = !Win_audio.mute;
		start_clicked_audio.mute = !start_clicked_audio.mute;

	}

	void Start(){
        this.gameHandler = GameObject.FindObjectOfType<GameHandler>();
    }

	
}
