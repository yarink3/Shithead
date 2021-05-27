using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public static UIController Instance;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null){
            GameObject.Destroy(this.gameObject);
            return;
        }
        Instance=this;
    }

    public Popup CreatePopup(){
        GameObject PopupGo =Instantiate(Resources.Load("UI/Popup") as GameObject);
        return PopupGo.GetComponent<Popup>();

    }
}
