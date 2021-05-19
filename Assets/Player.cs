using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEditor;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public string username;
    public List<GameObject> MyCards;
    public List<GameObject> My3OpenCards;
    public List<GameObject> My3CloseCards;
    public DropZone MyOpenCardsArea;
    public GameHandler gameHandler ;
    public Transform open_cards_transform ;

    public bool hasLegalMove(Player p){
        // Debug.Log("inside my turn function");
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

    public void getAllCardsFromDeck(Player p){

            // Debug.Log("getAllCardsFromDeck function");
           
			
			Transform real_deck_transform =GameObject.Find("OpenDeckItem").transform;
			Transform temp_deck_transform =GameObject.Find("TempDeck").transform;
			// Transform open_cards_transform =GameObject.Find("MyOpenCardsArea").transform;

			int children1 =real_deck_transform.childCount;
			int children2 =temp_deck_transform.childCount;

			for (int i = 0; real_deck_transform.childCount!=0; ++i){
				Transform child0Trans=real_deck_transform.GetChild(0).transform;
				// Debug.Log(child0Trans.gameObject.name);

                Image image= child0Trans.GetComponent<Image>();
				image.color = new Color32(255, 255, 255, 255);
                
                if(p.username=="Player"){
                    child0Trans.GetComponent<CardObject>().applied=false;
                    child0Trans.GetComponent<CardObject>().isShared=false;
                }
                else{
                    var sprite1 =Resources.Load <Sprite>("red_back" ); // set cards pic
                    image.sprite =sprite1;
                }
				
				p.MyCards.Add( child0Trans.gameObject);
				child0Trans.SetParent(p.open_cards_transform);

				
				int new_children =p.open_cards_transform.childCount;
				p.open_cards_transform.GetChild(new_children-1).transform.localPosition = Vector3.one;
				p.open_cards_transform.GetChild(new_children-1).transform.localRotation = Quaternion.identity;
				// change the card angle

				
			}

			for (int i = 0; temp_deck_transform.childCount!=0; ++i){
				Transform child0Trans=temp_deck_transform.GetChild(0).transform;
				// Debug.Log(child0Trans.name);
                Image image= child0Trans.GetComponent<Image>();
				image.color = new Color32(255, 255, 255, 255);

				if(p.username=="Player"){
                    child0Trans.GetComponent<CardObject>().applied=false;
                    child0Trans.GetComponent<CardObject>().isShared=false;
                }
                else{
                    var sprite1 =Resources.Load <Sprite>("red_back" ); // set cards pic
                    image.sprite =sprite1;
                }

				
				p.MyCards.Add( child0Trans.gameObject);
				child0Trans.SetParent(p.open_cards_transform);

				
				int new_children =p.open_cards_transform.childCount;
				p.open_cards_transform.GetChild(new_children-1).transform.localPosition = Vector3.one;
				p.open_cards_transform.GetChild(new_children-1).transform.localRotation = Quaternion.identity;
				// change the card angle

				
			}
			p.gameHandler.TempOpenDeck.realLastValue =2;
			p.gameHandler.TempOpenDeck.lastCardValue =2;

            if(p.username=="Player"){
			    this.SortMycards();
            }
            else{
                gameHandler.TempOpenDeck.setToZero();
            }



        
    }

    public bool hasBetterThan8(){
        int valCheck;
        bool all8 = true;

        for(int i=0; i<MyCards.Count;i++){
                valCheck=MyCards[i].GetComponent<CardObject>().value;
                
                if(valCheck>8 || valCheck ==2 || valCheck ==3){
                    return true;
                }
                if(valCheck!=8){
                    all8=false;
                } 

        }
        
        
        if(all8){
            for(int i=0; i<3; i++){
                valCheck=My3OpenCards[i].GetComponent<CardObject>().value;
                if(valCheck>8 || valCheck ==2 || valCheck ==3){
                    return true;
                }
            }
        }

        return false;




    }
    // protected abstract void MyTurn();
    public IEnumerator waitAndPlayAgain(Player p)
    {
        yield return new WaitForSeconds(1);

        if(p.MyCards.Count==0 && p.My3CloseCards.Count==0){
            // p.playerWon(p);
            StartCoroutine( p.playerWonCorrutine(p));

            yield return new WaitForSeconds(0);

        }
        
        if(p.username=="Pc"){
            gameHandler.PcPlayer.MyTurn();
        }
        
    }

    public IEnumerator holdAndResume(Player nextPlayer,string name)
    {
        if(gameHandler.gameStatus=="started"){
            Debug.Log("inside hold and resume");
            GameObject deck=GameObject.Find("OpenDeckItem");
            //// set image
            Image image = deck.GetComponent<Image>();
            Texture2D tex = Resources.Load<Texture2D>(name);
            var sprite1 =Resources.Load <Sprite>(name ); // set cards pic
            image.sprite =sprite1;

            yield return new WaitForSeconds(1);

            // image = deck.AddComponent<Image>();
            tex = Resources.Load<Texture2D>("empty_card");
            sprite1 =Resources.Load <Sprite>("empty_card" ); // set cards pic
            image.sprite =sprite1;
            // st.SetActive(false);


            if(this.MyCards.Count==0 && this.My3CloseCards.Count==0){
                
                // this.playerWon(this);
                StartCoroutine( this.playerWonCorrutine(this));

                Debug.Log("won from hold and resume");

                yield return new WaitForSeconds(0);

            }


            else if(nextPlayer.username=="Pc" ){
                gameHandler.PcPlayer.MyTurn();
            }
        }
    }

    public void closeCardClicked(Player currPlayer, CardObject OpenCardOnMe,GameObject closeCardObject){
        Debug.Log("close card clicked");
        CardObject card = closeCardObject.transform.GetComponent<CardObject>();
        bool deckCleaned=false;
        if (currPlayer.MyCards.Count == 0 && currPlayer.My3OpenCards.Count ==0 ){
            //// set image
            Image image = closeCardObject.transform.GetChild(0).transform.GetComponent<Image>();
            // Texture2D tex = Resources.Load<Texture2D>(closeCardObject.name);
           
            card.isShared=false;
            Transform temp_deck_transform =GameObject.Find("TempDeck").transform;

            if(!gameHandler.TempOpenDeck.isLegal(gameHandler.TempOpenDeck.realLastValue,card.value)){

                // gameHandler.ActivePanel({"Illeagle move","No luck this time, you will get all the cards.", "Ok"})
                if(currPlayer.username=="Player"){
                    Popup popup = UIController.Instance.CreatePopup();
                    popup.Init1Button(gameHandler.gameObject.transform,
                        "No luck this time, you will get all the cards.",
                        "OK"
                    );
                    var sprite1 =Resources.Load <Sprite>(closeCardObject.transform.GetChild(0).name); // set cards pic
                    image.sprite =sprite1;
                    // this.CloseCardsPanel.SetActive(true);
                    // EditorUtility.DisplayDialog ("Illeagle move","No luck this time, you will get all the cards.", "Ok");
                }
                else{
                    var sprite1 =Resources.Load <Sprite>("red_back"); // set cards pic
                    image.sprite =sprite1;
                }

                
                
                currPlayer.MyCards.Add(closeCardObject.transform.GetChild(0).gameObject);
                currPlayer.My3CloseCards.Remove(closeCardObject.transform.GetChild(0).gameObject);
                currPlayer.My3CloseCards.Remove(closeCardObject);
                closeCardObject.transform.GetChild(0).SetParent(open_cards_transform);
                
                getAllCardsFromDeck(currPlayer);
                if(currPlayer.username=="Player"){
                    StartCoroutine(waitAndPlayAgain(gameHandler.PcPlayer));

                }
                gameHandler.TempOpenDeck.realLastValue =2;
                gameHandler.TempOpenDeck.lastCardValue =2;
                
                // else{
                //      StartCoroutine(gameHandler.player.waitAndPlayAgain(gameHandler.player));

                // }
			
		

        
            }
            else{
                
                
                card.applied=true;
                card.isShared=false;
                closeCardObject.transform.GetChild(0).SetParent(temp_deck_transform);
                temp_deck_transform.GetChild(0).localPosition = Vector3.zero;
                temp_deck_transform.GetChild(0).localRotation = Quaternion.identity;
                deckCleaned = gameHandler.TempOpenDeck.PutNewCard(card.value,1);
                currPlayer.My3CloseCards.Remove(closeCardObject);
            
            if(currPlayer.username=="Pc" && card.value==8 && (currPlayer.MyCards.Count!=0 || currPlayer.My3CloseCards.Count!=0)){
                StartCoroutine(gameHandler.PcPlayer.StopUserPlayer());
            }

            }

            GameObject.Destroy(closeCardObject);
            
            
            if(card.value==10 || deckCleaned){
                if(currPlayer.username=="Player"){
                    StartCoroutine(holdAndResume(gameHandler.player, card.value+card.shape));
                }
                else{
                    StartCoroutine(holdAndResume(gameHandler.PcPlayer, card.value+card.shape));

                }
            }
            else if(currPlayer.username=="Player" && card.value!=8) {
                StartCoroutine(currPlayer.waitAndPlayAgain(gameHandler.PcPlayer));
            }



            if(gameHandler.gameStatus=="started" && currPlayer.MyCards.Count==0 && currPlayer.My3CloseCards.Count==0){
                Debug.Log("won from close card clicked");

                // currPlayer.playerWon(currPlayer);
                StartCoroutine( currPlayer.playerWonCorrutine(currPlayer));

                return;
            }

        }

        


        
    }

     public IEnumerator playerWonCorrutine(Player p)
    {
        if(p.username=="Player"){
            gameHandler.WinAudio.Play();
        }
        // else{
        //     gameHandler.LoseAudio.Play();

        // }
        yield return new WaitForSeconds(1);
         Debug.Log(p.username+" won!!!!");
        gameHandler.gameStatus="finished";
        

        if(p.username=="Player"){
            Popup popup = UIController.Instance.CreatePopup();
		    popup.Init2Buttons(gameHandler.gameObject.transform,
				"Nice job , you won!\nWould you like to play again?",
				"Yes",
				"No"

			 );
        }
        else{
            Popup popup = UIController.Instance.CreatePopup();
		    popup.Init2Buttons(gameHandler.gameObject.transform,
				"This time you lost, but it is not the end!\nWould you like to try again?",
				"Yes",
				"No"

			 );

        }

    }


    public void SortMycards(){
        MyCards.Sort(delegate(GameObject x, GameObject y) {
            return x.transform.GetComponent<CardObject>().value.CompareTo(y.transform.GetComponent<CardObject>().value);
        });
        for (int i = 0; i < MyCards.Count; i++)
        {
        MyCards[i].transform.SetSiblingIndex(i);
        }
    }




}
