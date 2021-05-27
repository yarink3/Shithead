using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Popup : MonoBehaviour
{
    [SerializeField] Button LeftButton;
    [SerializeField] Button RightButton;
    [SerializeField] Button CenterButton;

    [SerializeField] Text LeftButtonText;
    [SerializeField] Text RightButtonText;
    [SerializeField] Text CenterButtonText;

    [SerializeField] Text popupText;
	public GameHandler gameHandler;
    

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    public void No_Selected(){
        // gameHandler.gameStatus="started";
        GameObject.Destroy(this.gameObject);
        if(gameHandler.gameStatus == "finished" ){
            //load homescreen
            Debug.Log("Load homescreen");
			

        }
        else if( gameHandler.gameStatus=="holdExit" || gameHandler.gameStatus=="holdNewGame" ){
            gameHandler.gameStatus = gameHandler.HoldGameStatus;
            //  Load homescreen 
            // Scene scene = SceneManager.GetActiveScene();
            // SceneManager.LoadScene(scene.name);
            // gameHandler.gameStatus="not started";

        }

        

        // else if(gameHandler.gameStatus=="hold"){

        // }
        
    }
    
    public void Yes_Selected(){
        
        GameObject.Destroy(this.gameObject);
        if(gameHandler.gameStatus=="finished" || gameHandler.gameStatus=="holdNewGame" ){
            gameHandler.gameStatus = "started";
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            
        }
        else if( gameHandler.gameStatus=="holdExit"){
            Application.Quit();
            
        }
        else {
            gameHandler.player.getAllCardsFromDeck(gameHandler.player);
            gameHandler.PcPlayer.MyTurn();
        }
        
        

    }
    public void OkForCloseCard(){
            GameObject.Destroy(this.gameObject);
        }
    public void Init2Buttons(Transform canvas, string popupMessage, string btn1Text ,string btn2Text){
        // if(popupMessage == "Are you sure you want to start a new game?"){
        //     Yes_Selected=(()=>{


        //     } );
        // }


        this.gameHandler = GameObject.FindObjectOfType<GameHandler>();
        popupText.text=popupMessage;
        LeftButtonText.text = btn1Text;
        RightButtonText.text = btn2Text;

        

        transform.SetParent(canvas);
        transform.localScale= Vector3.one;
        GetComponent<RectTransform>().offsetMin =Vector2.zero;
        GetComponent<RectTransform>().offsetMax =Vector2.zero;
        Transform smallPop=this.transform.GetChild(0);
        for(int i=smallPop.childCount-1 ; i>(-1); --i){
            if(smallPop.GetChild(i).name=="OkButton"){
                GameObject.Destroy(smallPop.GetChild(i).gameObject);
                break;
            }
        }
    }

    public void Init1Button(Transform canvas, string popupMessage, string btn1Text ){
        this.gameHandler = GameObject.FindObjectOfType<GameHandler>();
        popupText.text=popupMessage;
        LeftButtonText.text = btn1Text;

        transform.SetParent(canvas);
        transform.localScale= Vector3.one;
        GetComponent<RectTransform>().offsetMin =Vector2.zero;
        GetComponent<RectTransform>().offsetMax =Vector2.zero;
       Transform smallPop=this.transform.GetChild(0);
        for(int i=smallPop.childCount-1 ; i>(-1); --i){
                // Debug.Log(smallPop.GetChild(i).name);

                if(smallPop.GetChild(i).name=="YesButton" || smallPop.GetChild(i).name=="NoButton"){
                     GameObject.Destroy(smallPop.GetChild(i).gameObject);
 
                }
        }
    }
    public void InitNoButtons(Transform canvas, string popupMessage ){
        this.gameHandler = GameObject.FindObjectOfType<GameHandler>();
        popupText.text=popupMessage;
        
        transform.SetParent(canvas);
        transform.localScale= Vector3.one;
        GetComponent<RectTransform>().offsetMin =Vector2.zero;
        GetComponent<RectTransform>().offsetMax =Vector2.zero;
       Transform smallPop=this.transform.GetChild(0);
        for(int i=smallPop.childCount-1 ; i>(-1); --i){
                // Debug.Log(smallPop.GetChild(i).name);

                if(smallPop.GetChild(i).name=="YesButton" || smallPop.GetChild(i).name=="NoButton" || smallPop.GetChild(i).name=="OkButton"){
                     GameObject.Destroy(smallPop.GetChild(i).gameObject);
                    // i--;
                    // break;
                }
                else if(smallPop.GetChild(i).name=="Text"){
                    smallPop.GetChild(i).transform.position = smallPop.position;
                    // RectTransform textRect=smallPop.GetChild(i).gameObject.GetComponent<RectTransform>();
                    // textRect.offsetMin =new Vector2(0,15);
                    // textRect.offsetMax =new Vector2(0,15);
                }
        }
        // Debug.Log(smallPop.GetChild(0).gameObject);
        // RectTransform textRect=smallPop.GetChild(0).gameObject.GetComponent<RectTransform>();
        // textRect.offsetMin =new Vector2(0,0);
        // textRect.offsetMax =new Vector2(0,0);
    }


}
