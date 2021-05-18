using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TempOpenDeckObject : MonoBehaviour, IDropHandler 
{

    

    public int lastCardValue ;
    public int realLastValue ;
    public int currCount ;
    public List<CardObject>  cardsToApply;
    public int currListVal;
    public GameHandler gameHandler = null;
    Transform real_deck_transform ;
    Transform temp_deck_transform ;
    [SerializeField] Text CurrCountText;


    public void setToZero(){
        CurrCountText.text="Current count: 0";
    }
    public void addToList(CardObject card){
        // add card to cardsListToapply (before user clicked done)
        cardsToApply.Add(card);
        // lastCardValue=card.value;
        if(this.currListVal ==(-1)){
            this.currListVal=card.value;
        }
    }


    public bool isLegal(int old, int newValue){
        if(old==-1 || newValue==-1){
            return false;
        }
        if((newValue==10 && old!=7) ||newValue == 3 || newValue == 2 ){
            return true;
        }
        if(this.cardsToApply.Count != 0){
            return this.cardsToApply[0].value == newValue;
        }

        if(old==7){

            return newValue <= old;
        }  

        return newValue >= old;
    }

    public void cleanDeck(){
        this.currCount=0;
        this.lastCardValue=2;
        this.realLastValue=2;
        for (int i=0 ; i<real_deck_transform.childCount; i++){
            // Debug.Log("cleaning...");
            Destroy(real_deck_transform.GetChild(i).gameObject);
        }
        for (int i=0 ; i<temp_deck_transform.childCount; i++){
            // Debug.Log("cleaning...");
            Destroy(temp_deck_transform.GetChild(i).gameObject);
        }
        // Destroy(real_deck_transform.GetChild(0).gameObject);
        CurrCountText.text="Current count: 0";
    }
    
    public bool PutNewCard(int newValue,int count){ // return if the deck cleaned or not
        Debug.Log("put new card: " +newValue + " count: "+count);
        if(newValue==3){
            if(lastCardValue==3){
                this.currCount=this.currCount+count;
                CurrCountText.text="Current count: "+this.currCount;

            }
            else{
                lastCardValue=3;
                this.currCount=count;
                CurrCountText.text="Current count: "+this.currCount;
            }

        }
        else{
            lastCardValue=this.realLastValue;
            if(newValue==this.realLastValue){
                this.currCount=this.currCount+count;
                CurrCountText.text="Current count: "+this.currCount;

            }
            else{
                this.realLastValue=newValue;
                this.currCount=count;
                CurrCountText.text="Current count: "+this.currCount;
            }

        }


        int children = this.transform.childCount;
        // Transform real_deck_transform =GameObject.Find("OpenDeckItem").transform;
        // int Tindex=0;
        Transform LastChild = this.transform.GetChild(this.transform.childCount-1).transform; 
        for (int i = this.transform.childCount-2; i > -1; --i){
            Transform child=this.transform.GetChild(i).transform;
            if(child.name!="CurrCountImage"){
            child.GetComponent<CardObject>().applied=true;
            gameHandler.player.MyCards.Remove( child.gameObject);
             if(newValue!=3){ 

                child.SetParent(real_deck_transform);
                // int new_children =real_deck_transform.childCount;
                child.localPosition = Vector3.one;
                child.localRotation = Quaternion.identity;
                // change the card angle
           
            
            }
            else{
               Image sr= child.GetComponent<Image>();
              sr.color = new Color32(255, 255, 255, 100);
            //   Tindex++;
            //    sr.color=emit;
            }
            }
        }
        LastChild.GetComponent<CardObject>().applied=true;
            gameHandler.player.MyCards.Remove( LastChild.gameObject);
             if(newValue!=3){ 

                LastChild.SetParent(real_deck_transform);
                // int new_LastChildren =real_deck_transform.LastChildCount;
                LastChild.localPosition = Vector3.one;
                LastChild.localRotation = Quaternion.identity;
                // change the card angle
           
            
            }
            else{
               Image sr= LastChild.GetComponent<Image>();
              sr.color = new Color32(255, 255, 255, 100);
            //   Tindex++;
            //    sr.color=emit;
            }
        bool ret=false;
        if(this.currCount==4 || newValue==10 ){
            cleanDeck();
            ret = true;
        }
        cardsToApply.Clear() ;
        currListVal= -1; 

        return ret;
        
          
    }

    public void removeCardFromApplyList(CardObject card){
        Debug.Log("inside remove card function");
        if(this.cardsToApply.Count == 1){
            this.currListVal= -1;
            this.lastCardValue = this.realLastValue;
        }
        Debug.Log("value to remove: "+card.value);
        for(int i=0; i<cardsToApply.Count; i++){
            if(cardsToApply[i].value ==card.value){
                Debug.Log("inside cardsToApply[i].value ==card.value");

                bool x =cardsToApply.Remove(card);
                Debug.Log(x + " returned, now length of cardsToApply is: "+ cardsToApply.Count);
                
            }
        }

    }


    public void OnDrop(PointerEventData eventData){
        CardObject card= eventData.pointerDrag.GetComponent<CardObject>();
        Debug.Log(card.value + " dropped in" + gameObject.name);
        if(!card.applied && !card.isShared && gameHandler.gameStatus == "started"){
            if(this.isLegal(this.realLastValue,card.value) ||  this.currListVal==card.value )
            {
                //add to curr cards list to apply
                this.addToList(card);

                //change parent to open deck
                card.transform.SetParent(this.transform);
                card.parentToReturnTo=(this.transform);
                        
                //change card angel
                card.transform.localPosition = Vector3.zero;
                card.transform.localRotation = Quaternion.identity;

                // set the (maybe) new curr value
                currListVal= card.value;


            }
            
            else{
                // UserPlayer user = (UserPlayer) gameHandler.Players_list[0];
                UserPlayer user = gameHandler.player;
                card.transform.SetParent(user.MyOpenCardsArea.transform);
                card.parentToReturnTo = (user.MyOpenCardsArea.transform);

            }
        }
    }

    void Start(){
        this.gameHandler = GameObject.FindObjectOfType<GameHandler>();
        this.cardsToApply=  new List<CardObject>();
        this.real_deck_transform =GameObject.Find("OpenDeckItem").transform;
        this.temp_deck_transform =GameObject.Find("TempDeck").transform;
        this.currListVal=-1;
        this.currCount=0;
        CurrCountText.text="Current count: 0";
        
        this.lastCardValue = 2;
        this.realLastValue = 2;
    }

}
