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
    


    public void No_Selected(){
        GameObject.Destroy(this.gameObject);
        if(gameHandler.gameStatus == "finished" ){
            // Debug.Log("Load homescreen");
		
        }
        else if( gameHandler.gameStatus=="holdExit" || gameHandler.gameStatus=="holdNewGame" ){
            gameHandler.gameStatus = gameHandler.HoldGameStatus;
         
        }
        
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
            if(smallPop.GetChild(i).name=="YesButton" || smallPop.GetChild(i).name=="NoButton" || smallPop.GetChild(i).name=="OkButton"){
                    GameObject.Destroy(smallPop.GetChild(i).gameObject);
            }
            else if(smallPop.GetChild(i).name=="Text"){
                smallPop.GetChild(i).transform.position = smallPop.position;
            }
        }

    }

}
