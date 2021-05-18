using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// using UnityEditor;


public class MyCloseCard : MonoBehaviour , IDropHandler
{
    // public int value ;
    // public string shape  ;
    public GameHandler gameHandler = null;
    public CardObject OpenCardOnMe = null;
    
    // public bool clickable = false; 

    public void onClick(){
        
        gameHandler.player.closeCardClicked(gameHandler.player,this.OpenCardOnMe,this.gameObject);

    }

    public void OnDrop(PointerEventData eventData){
        gameHandler.LayCard.Play();
        CardObject card= eventData.pointerDrag.GetComponent<CardObject>();
        Debug.Log(card.value + " dropped in" + gameObject.name);
       
        if(card!=null && !card.applied){
            UserPlayer user = gameHandler.player;

            if(OpenCardOnMe==null){
                card.transform.SetParent(this.transform);
                card.parentToReturnTo=(this.transform);
                OpenCardOnMe=card;
                OpenCardOnMe.isShared = true;
                // card.GetComponent<CardObject>().enabled = false;
                GameObject go=card.gameObject;
                
                // user.MyCardsCount--;
                user.My3OpenCards.Add(go);
                user.MyCards.Remove(go);
                // user.My3OpenCardsCount++;
            }
            else{
                // UserPlayer user = (UserPlayer) gameHandler.Players_list[0];
                

                card.transform.SetParent(user.MyOpenCardsArea.transform);
                card.parentToReturnTo = user.MyOpenCardsArea.transform;
                
            }
 
                // //change card angel
                card.transform.localPosition = Vector3.zero;
                card.transform.localRotation = Quaternion.identity;

                // set the (maybe) new curr value
            //     currListVal= card.value;


            // }
            
            // else{
            //     card.transform.SetParent(gameHandler.MyCardsArea.transform);
            //     card.parentToReturnTo=gameHandler.MyCardsArea.transform;

            
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        this.gameHandler = GameObject.FindObjectOfType<GameHandler>();

    }


}
