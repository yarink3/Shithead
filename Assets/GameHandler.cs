using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// [System.Serializable]

public class GameHandler : MonoBehaviour,IDropHandler
{
    // public Player [] Players_list = null;
    public UserPlayer player = null;
    public PcPlayer PcPlayer = null;
    public TempOpenDeckObject TempOpenDeck = null;
    public CloseDeckObject CloseDeck = null;
    public string gameStatus = "not started"; 
    public string HoldGameStatus = "not started"; 
    public int turnId = 0; 
    public AudioSource LayCard;
    public AudioSource WinAudio;

    
    
    protected GameHandler() {
        // Debug.Log("GameHandler constructor");
        
    }
    public void OnDrop(PointerEventData eventData){
        // when user put card on the board
        CardObject card= eventData.pointerDrag.GetComponent<CardObject>();
        if(card != null && !card.applied && !card.isShared){
            UserPlayer user = this.player;
            card.gameObject.transform.SetParent(user.MyOpenCardsArea.transform);
            card.parentToReturnTo = user.MyOpenCardsArea.transform;
            TempOpenDeck.removeCardFromApplyList(card);



        }
    }


    void Start()
    {
       
        
        this.CloseDeck = GameObject.FindObjectOfType<CloseDeckObject>();
        this.TempOpenDeck = GameObject.FindObjectOfType<TempOpenDeckObject>();
        
        this.player = GameObject.FindObjectOfType<UserPlayer>();
        this.PcPlayer = GameObject.FindObjectOfType<PcPlayer>();
        this.CloseDeck.StartGame.Play();
        this.CloseDeck.setup();
        this.PcPlayer.setup();
        this.player.setup();

        


    }

    
}
