using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler{
    public GameHandler gameHandler;

    protected DropZone(){
            
        }
    public void OnDrop(PointerEventData eventData){
        Debug.Log(eventData.pointerDrag.name + "dropped in" + gameObject.name);
        CardObject card= eventData.pointerDrag.GetComponent<CardObject>();
        if(card != null && !card.isShared && !card.applied){

            // if the user put a card on deck and regrets: 
            if(  gameObject.name == "MyOpenCardsArea" && card.applied==false)
            {
                // make the card sit in MyCardsArea :
                card.transform.SetParent(this.transform);
                card.parentToReturnTo = this.transform;
                
                // Debug.Log(card.value + " "+card.shape+ " is out of deck");
                this.gameHandler.TempOpenDeck.removeCardFromApplyList(card);
                card.transform.localPosition = Vector3.zero;
                card.transform.localRotation = Quaternion.identity;
                    
               
            }
            
            

        }
    }

    void Start(){
        this.gameHandler =GameObject.FindObjectOfType<GameHandler>();
        
    }

   
}
