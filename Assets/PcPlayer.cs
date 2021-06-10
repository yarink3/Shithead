using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEditor;

public class PcPlayer : Player
{

protected PcPlayer() {
            
}

public GameObject getHighestCard(){
    List<GameObject> cards = new List<GameObject>(MyCards);
    cards.Sort(delegate(GameObject x, GameObject y) {
            return x.transform.GetComponent<CardObject>().value.CompareTo(y.transform.GetComponent<CardObject>().value);
        });
    if(cards[0].transform.GetComponent<CardObject>().value==2 || cards[0].transform.GetComponent<CardObject>().value==3){
        return cards[0];
    }
    else{
        return cards[cards.Count-1];
    }
    

}

public void setup(){
    MyCards = new List<GameObject>() ;

    
    My3OpenCards = new List<GameObject>() ;
    My3CloseCards = new List<GameObject>() ;
    this.username="Pc";
    this.open_cards_transform =GameObject.Find("PcOpenCardsArea").transform;

    this.gameHandler = GameObject.FindObjectOfType<GameHandler>();
    
    DropZone [] dropzones =GameObject.FindObjectsOfType<DropZone>();
    for(int i=0; i< dropzones.Length; i++){
        if(dropzones[i].transform.name == "PcOpenCardsArea"){
            MyOpenCardsArea=dropzones[i];
        }
    }
    GameObject closeCardsList=GameObject.Find("PcCloseCards");
    for(int i=0; i<3 ; i++){ // player's close cards
            GameObject closeCardsI=closeCardsList.transform.GetChild(i).gameObject;
            My3CloseCards.Insert(0,closeCardsI);
    }
    
    gameHandler.CloseDeck.pullCardsToUser(this,6,true);
    gameHandler.CloseDeck.pullCardsToUser(this,3,false);
    
    for(int i=0; i<3; i++){
        GameObject card1 = getHighestCard();
        Transform closeCard= GameObject.Find("PcCloseCard"+i).transform;
        card1.transform.SetParent(closeCard);
        MyCards.Remove(card1);
        My3OpenCards.Add(card1);
        
        card1.transform.localPosition = Vector3.zero;
        card1.transform.localRotation = Quaternion.identity;
        RectTransform trans = card1.GetComponent<RectTransform>();

        trans.sizeDelta= new Vector2(70, 100); // custom size
        LayoutElement lay_el = card1.GetComponent<LayoutElement>();
        lay_el.minWidth=70;
        lay_el.minHeight=100;
        lay_el.preferredWidth=70;
        lay_el.preferredHeight=100;
        lay_el.flexibleHeight=0;
        lay_el.flexibleWidth=0;
        lay_el.layoutPriority=1;

        //// set image
        Image image = card1.GetComponent<Image>();
        var sprite1 =Resources.Load <Sprite>(card1.name ); // set cards pic
        image.sprite =sprite1;

    }
}

public IEnumerator StopUserPlayer(){
    // stop the user player and plays again
    if(gameHandler.gameStatus=="started"){
        gameHandler.HoldGameStatus="started";
        Popup popup = UIController.Instance.CreatePopup();
        popup.InitNoButtons(gameHandler.gameObject.transform,
            "Pc player stopped you\n(8 card), now it's his turn again..."
            );
        yield return new WaitForSeconds(4);
        popup.No_Selected();
        
        if(this.MyCards.Count==0 && this.My3CloseCards.Count==0){
            StartCoroutine( playerWonCorrutine(this));
            yield return null;
        }
        if(gameHandler.gameStatus=="started"){
            MyTurn();
        }
    }
}

    public bool hasLegalMove(Player p){
        //checks if the PcPlayer has legal move to play
        for(int i=0; i<p.MyCards.Count  ; ++i){
            if(gameHandler.TempOpenDeck.isLegal(gameHandler.TempOpenDeck.realLastValue, p.MyCards[i].GetComponent<CardObject>().value)){
                return true;
            }
        }
        if(MyCards.Count==0){
            if(My3CloseCards.Count > 0){
                return true;
            }
            for(int i=0; i<p.My3OpenCards.Count; ++i){
                if(gameHandler.TempOpenDeck.isLegal(gameHandler.TempOpenDeck.realLastValue, p.My3OpenCards[i].GetComponent<CardObject>().value)){
                    return true;
                }
            }
        }
        
        return false;

    }

public void MyTurn(){
    // plays the PcPlayer turn

    if(gameHandler.gameStatus=="started"){
        int bestVal=15;
        bool has3=false;
        bool has2=false;
        bool has10=false;
        if(!this.hasLegalMove(this)){
            getAllCardsFromDeck(this);
            StartCoroutine(waitAndPlayAgain(gameHandler.player));
            return;
        }

        if(MyCards.Count==0){
            int index=Random.Range(0,My3CloseCards.Count);
            this.closeCardClicked( gameHandler.PcPlayer,My3CloseCards[index]);            
            return;

        }

        else{   // I have cards to play with
            
            List<CardObject> localCardsToApply=new List<CardObject>() {MyCards[0].GetComponent<CardObject>()};

                for (int i=0 ;i<MyCards.Count  ; ++i){
                    int currVal=MyCards[i].GetComponent<CardObject>().value;

                    if(currVal==3){
                        has3=true;
                    }
                    else if(currVal==2){
                        has2=true;
                    }
                    else if(currVal==10){
                        has10=true;
                    }
                    
                    else if(currVal > 3 && (i==0 || currVal < bestVal) && gameHandler.TempOpenDeck.isLegal(gameHandler.TempOpenDeck.realLastValue,currVal) ){
                        localCardsToApply.Clear();
                        localCardsToApply.Add(MyCards[i].GetComponent<CardObject>());
                        bestVal = localCardsToApply[0].value;
                    }
                    else if(currVal == bestVal){
                            localCardsToApply.Add(MyCards[i].GetComponent<CardObject>());
                         }

                }
            
            /// best card (not 3 or 2) is not legal
            if(!gameHandler.TempOpenDeck.isLegal(gameHandler.TempOpenDeck.realLastValue , bestVal) || bestVal ==15){

                if(has3){
                    localCardsToApply.Clear();
                    for (int i=0 ;i<MyCards.Count  ; ++i){
                        CardObject currCard=MyCards[i].GetComponent<CardObject>();
                        int currVal=currCard.value;
                        if(currVal==3){
                            localCardsToApply.Add(currCard);
                            if(MyCards.Count>=3){
                                break;
                            }
                        }
                    }
                }

                else if(has2){
                    localCardsToApply.Clear();
                    for (int i=0 ;i<MyCards.Count  ; ++i){
                        CardObject currCard=MyCards[i].GetComponent<CardObject>();
                        int currVal=currCard.value;
                        if(currVal==2){
                            localCardsToApply.Add(currCard);
                            if(MyCards.Count>=3){
                                break;
                            }
                        }
                    }
                }
                else if(has10){
                    localCardsToApply.Clear();
                    for (int i=0 ;i<MyCards.Count  ; ++i){
                        CardObject currCard=MyCards[i].GetComponent<CardObject>();
                        int currVal=currCard.value;
                        if(currVal==10){
                            localCardsToApply.Add(currCard);
                            if(MyCards.Count>=3){
                                break;
                            }
                        }
                    }
                }
                else{
                    //I have nothing to do
                    getAllCardsFromDeck(this);
                }
            }
            
            if(gameHandler.TempOpenDeck.isLegal(gameHandler.TempOpenDeck.realLastValue , localCardsToApply[0].value)){
                
                int value= localCardsToApply[0].value;
                string shape= localCardsToApply[0].shape;
                int count= localCardsToApply.Count;

                for(int i=localCardsToApply.Count -1; i>=0;--i){
                    CardObject card= localCardsToApply[i];
                    //// set image
                    Image image = card.GetComponent<Image>();
                    var sprite1 =Resources.Load <Sprite>(card.name ); // set cards pic
                    image.sprite =sprite1;
                    gameHandler.TempOpenDeck.addToList(card);

                    //change parent to open deck
                    card.transform.SetParent(gameHandler.TempOpenDeck.transform);
                    card.parentToReturnTo=(gameHandler.TempOpenDeck.transform);
                            
                    //change card angel
                    card.transform.localPosition = Vector3.zero;
                    card.transform.localRotation = Quaternion.identity;

                    // set the (maybe) new curr value
                    gameHandler.TempOpenDeck.currListVal= card.value;
                    this.MyCards.Remove(card.gameObject);
                }


                bool deckCleaned = gameHandler.TempOpenDeck.PutNewCard(value,count);

                if(this.MyCards.Count<3){
                    gameHandler.CloseDeck.pullCardsToUser(this,3-this.MyCards.Count,true);
                    // this.MyCards.Count=3;
                }
                if(this.MyCards.Count==0){
                    if(this.My3OpenCards.Count ==3){
                        for(int i=0; i<3 ; i++){
                            Transform My_open_trans = this.MyOpenCardsArea.transform;
                            GameObject go=this.My3OpenCards[0];
                            CardObject card = go.transform.GetComponent<CardObject>();
                            //// set image
                            Image image = go.GetComponent<Image>();
                            var sprite1 =Resources.Load <Sprite>("red_back" ); // set cards pic
                            image.sprite =sprite1;
                            card.isShared=false;

                            this.My3OpenCards[0].transform.SetParent(My_open_trans);
                            this.MyCards.Add(go);
                            this.My3OpenCards.Remove(go);
                        }
                    }
                    if(this.My3CloseCards.Count==0 && this.MyCards.Count==0){ // finished shared cards and my cards
                        StartCoroutine( this.playerWonCorrutine(this));

                    }

                }
                if(value==8){
                    StartCoroutine(StopUserPlayer());
                }
                else if(deckCleaned){
                    StartCoroutine(holdAndResume(gameHandler.PcPlayer, value , shape));
                }
            }
        }

    }
}

}
