using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseDeckObject : MonoBehaviour
{
   
    public List<GameObject> ListOfCards ; 
    public GameHandler gameHandler ;
    public AudioSource PullCard ;
    public AudioSource StartGame ;
    [SerializeField] Text LeftCardsText;


    // Start is called before the first frame update
    protected CloseDeckObject(){
        
    }

    public void pullCardsToUser(Player user, int numOfCards , bool open){
        
        // Debug.Log(user.username);
     //// set user cards

     for(int i=0; i < numOfCards && ListOfCards.Count>0; i++ ){
        PullCard.Play();
        int index=Random.Range(0,ListOfCards.Count);
    
        Transform card1 =gameObject.transform.GetChild(index);
        GameObject go=card1.gameObject;

        LayoutElement lay_el = go.AddComponent<LayoutElement>();
        lay_el.minWidth=70;
        lay_el.minHeight=100;
        lay_el.preferredWidth=70;
        lay_el.preferredHeight=100;
        lay_el.flexibleHeight=0;
        lay_el.flexibleWidth=0;
        lay_el.layoutPriority=1;
        if(user.username=="Pc"){
            card1.GetComponent<CardObject>().isShared=true;
            if(open){
            //// set image
            Image image = card1.GetComponent<Image>();
            var sprite1 =Resources.Load <Sprite>("red_back" ); // set cards pic
            image.sprite =sprite1;
            }
        }
        if(open){

            card1.SetParent(user.MyOpenCardsArea.transform);
            user.MyCards.Add(go);
             
        }
        else{
            
            GameObject closeCardsObject=  GameObject.Find(user.username+"CloseCards");
            CardObject card2=go.GetComponent<CardObject>() ;

            Transform closeCard = closeCardsObject.transform.GetChild(i);
            CardObject card3=closeCard.gameObject.GetComponent<CardObject>() ;

            card3.value=card2.value;
            
            card3.shape=card2.shape;    
            card2.transform.SetParent(closeCard) ;       
        } 
        ListOfCards.Remove(go);
     }

     if(open && user.username=="Player"){
        user.SortMycards();
     }

     if(this.ListOfCards.Count==0){

         //// set image
            Image image = this.gameObject.GetComponent<Image>();
            var sprite1 =Resources.Load <Sprite>("empty_card" ); // set cards pic
            image.sprite =sprite1;
     }
    }
    
    public string get_val_string_for_pic(string val_str){
        switch (val_str)
            {
            case "11":
                return "J";                        
                
            case "12":
                return "Q";                        
                
            case "13":
                return "K";                        
                
            case "14":
                return "A";                        
                
            default:
                return val_str;
            }  
    }

    public void setup()
    {
        
    // Debug.Log("inside start of close deck");
    this.ListOfCards=new List<GameObject>();
    this.gameHandler = GameObject.FindObjectOfType<GameHandler>();
  
    string [] Shapes = {"H","C","S","D"};

    for(int shapeId = 0; shapeId<4 ; shapeId++){ //TODO change to 4
        for(int val=2; val<=14 ; val++){
        string val_str=get_val_string_for_pic(val.ToString());

        GameObject imgObject = new GameObject(val_str +Shapes[shapeId]);

        RectTransform trans = imgObject.AddComponent<RectTransform>();
        trans.transform.SetParent(this.transform); // setting parent
        trans.localScale = Vector3.one;
        trans.anchoredPosition = new Vector2(0f, 0f); // setting position, will be on center
        trans.sizeDelta= new Vector2(0, 0); // custom size

        //// set image
        Image image = imgObject.AddComponent<Image>();
        Texture2D tex = Resources.Load<Texture2D>(val_str +Shapes[shapeId]);
        var sprite1 =Resources.Load <Sprite>(val_str +Shapes[shapeId] ); // set cards pic
        image.sprite =sprite1;
   
        //// set card
        CardObject card=imgObject.AddComponent<CardObject>() ;
        card.value=val;
        card.shape=Shapes[shapeId];

        CanvasGroup cg= imgObject.AddComponent<CanvasGroup>();

        ListOfCards.Add(imgObject);
    
        }
    }

   
    }

    void Update(){
        LeftCardsText.text=ListOfCards.Count+" Cards";
    }
}

