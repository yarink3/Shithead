using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor;
using UnityEngine.UI;


public class UserPlayer : Player
{

    protected UserPlayer() {
            
        }

    public void setup(){
         MyCards = new List<GameObject>() ;
        My3OpenCards = new List<GameObject>() ;
        My3CloseCards = new List<GameObject>() ;
        this.open_cards_transform =GameObject.Find("MyOpenCardsArea").transform;
        this.username="Player";
        this.gameHandler = GameObject.FindObjectOfType<GameHandler>();
        DropZone [] dropzones =GameObject.FindObjectsOfType<DropZone>();
        for(int i=0; i< dropzones.Length; i++){
            if(dropzones[i].transform.name == "MyOpenCardsArea"){
                MyOpenCardsArea=dropzones[i];
            }
        }
        
        MyCloseCard [] closeCards =GameObject.FindObjectsOfType<MyCloseCard>();
        for(int i=0; i<3 ; i++){ // player's close cards
                My3CloseCards.Insert(0,closeCards[i].gameObject);

        }
        gameHandler.CloseDeck.pullCardsToUser(this,6,true);
        gameHandler.CloseDeck.pullCardsToUser(this,3,false);

    }
  
}
