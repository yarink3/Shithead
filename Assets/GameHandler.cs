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
    public OpenDeckObject OpenDeck = null;
    public CloseDeckObject CloseDeck = null;
    public string gameStatus = "not started"; // options: started , finished 
    // public string [] Shapes = {"H","C","S","D"};
    public int turnId = 0; // 0 - me , 1 - PC
    public AudioSource LayCard;
    public AudioSource WinAudio;
    [SerializeField]  GameObject WinnerPanel;
    [SerializeField]  GameObject LoserPanel;
    [SerializeField]  GameObject NoCardsPanel;
    
    
    public void ch(){
        NoCardsPanel.transform.localScale = new Vector2(220,90);
    }
    protected GameHandler() {
        // Debug.Log("GameHandler constructor");
        
    }
    public void OnDrop(PointerEventData eventData){
        CardObject card= eventData.pointerDrag.GetComponent<CardObject>();
        if(card != null && !card.applied && !card.isShared){
            UserPlayer user = this.player;
            card.gameObject.transform.SetParent(user.MyOpenCardsArea.transform);
            card.parentToReturnTo = user.MyOpenCardsArea.transform;
            TempOpenDeck.removeCardFromApplyList(card);



        }
    }

    // public void PlayAgain(){
    //     // GameObject WinnerPanel= GameObject.Find("WinnerPanel");
    //     // GameObject LoserPanel= GameObject.Find("LoserPanel");

    //     this.WinnerPanel.SetActive(false);
    //     this.LoserPanel.SetActive(false);
    //     Scene scene = SceneManager.GetActiveScene();
    //     SceneManager.LoadScene(scene.name);

    // }

    // public void GoToHomeScreen(){
    //     Debug.Log("decide to go to home screen");
    //     // GameObject WinnerPanel= GameObject.Find("WinnerPanel");
    //     // GameObject LoserPanel= GameObject.Find("LoserPanel");

    //     // WinnerPanel.SetActive(false);
    //     // LoserPanel.SetActive(false);

    //     // SceneManager.LoadScene("Homescreen");
    // }
    // public void setNocardsActive(){
    //     this.NoCardsPanel.SetActive(true);

    // }
    // public void NoCardsSubmitted(){
	// 	this.NoCardsPanel.SetActive(false);
    //     this.player.getAllCardsFromDeck(this.player);
	// 	this.PcPlayer.MyTurn();
    // }

    // public void NoCardsCancled(){
	// 	this.NoCardsPanel.SetActive(false);
    // }

    






    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameHandler constructor");
        // this.WinnerPanel= GameObject.Find("WinnerPanel");
        // this.LoserPanel= GameObject.Find("LoserPanel");
        // this.NoCardsPanel=GameObject.Find("NoCardsPanel");
        
        this.CloseDeck = GameObject.FindObjectOfType<CloseDeckObject>();
        this.TempOpenDeck = GameObject.FindObjectOfType<TempOpenDeckObject>();
        this.OpenDeck = GameObject.FindObjectOfType<OpenDeckObject>();
        
        this.player = GameObject.FindObjectOfType<UserPlayer>();
        this.PcPlayer = GameObject.FindObjectOfType<PcPlayer>();
        this.CloseDeck.StartGame.Play();
        this.CloseDeck.setup();
        this.PcPlayer.setup();
        this.player.setup();

        


    }

    
}
