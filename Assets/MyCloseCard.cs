using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MyCloseCard : MonoBehaviour , IDropHandler
{

    public GameHandler gameHandler = null;
    public CardObject OpenCardOnMe = null;
    
    public void onClick(){
        if(this.OpenCardOnMe != null){
            gameHandler.player.closeCardClicked(gameHandler.player,this.gameObject);
        }

    }

    public void OnDrop(PointerEventData eventData){
        gameHandler.LayCard.Play();
        CardObject card= eventData.pointerDrag.GetComponent<CardObject>();
       
        if(card!=null && !card.applied && !card.isShared){
            UserPlayer user = gameHandler.player;

            if(OpenCardOnMe==null){
                card.transform.SetParent(this.transform);
                card.parentToReturnTo=(this.transform);
                OpenCardOnMe=card;
                OpenCardOnMe.isShared = true;
                GameObject go=card.gameObject;
                
                user.My3OpenCards.Add(go);
                user.MyCards.Remove(go);
            }
            else{
                card.transform.SetParent(user.MyOpenCardsArea.transform);
                card.parentToReturnTo = user.MyOpenCardsArea.transform;
                
            }
                // //change card angel
                card.transform.localPosition = Vector3.zero;
                card.transform.localRotation = Quaternion.identity;

        }
    }
    
    void Start()
    {
        this.gameHandler = GameObject.FindObjectOfType<GameHandler>();

    }


}
