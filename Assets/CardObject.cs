using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardObject : MonoBehaviour,IBeginDragHandler, IDragHandler , IEndDragHandler
{
   
    public int value;
    public string shape ;
    public Transform parentToReturnTo = null;
    public Transform last = null;
    public bool applied = false;
    public bool isShared = false;
    
    public GameHandler gameHandler = null;

    public void init(int val ,string s){
        value=val;
        shape=s;
    }
    public void OnBeginDrag(PointerEventData eventData){
    //when the user start to drag the card
        
        if(!applied && !isShared){
            last=this.transform.parent;
            
            this.transform.SetParent(this.transform.parent.parent);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        
    }
    public void OnEndDrag(PointerEventData eventData){
       
        if(!this.applied && !isShared ){
            // change the card angle
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            
            if(parentToReturnTo == null){
                this.transform.SetParent(last.transform);
            }
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;     
    }

    public void OnDrag(PointerEventData eventData){
        // changes the live card location while dragging
        if(!applied && !isShared){
            this.transform.position  = eventData.position;
        }
    }

    void Start(){
        this.gameHandler = GameObject.FindObjectOfType<GameHandler>();

    }

    
    
}
