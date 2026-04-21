using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClicked : MonoBehaviour
{
    private Button Onbutton;
    private Button Offbutton;
    public GameObject Keybynd;
    public GameObject ON;
    public GameObject OFF;

    void Start(){
        Onbutton = ON.GetComponent<Button>();
        Offbutton = OFF.GetComponent<Button>();
        KeybyndDisplayOn();
    }
    
    public void KeybyndDisplayOn(){
        ColorBlock Oncb = Onbutton.colors;
        ColorBlock Offcb = Offbutton.colors;
        Oncb.normalColor = Color.white;
        Offcb.normalColor = Color.gray;
        Onbutton.colors = Oncb;
        Offbutton.colors = Offcb;
        Keybynd.SetActive(true);
    }

    public void KeybyndDisplayOff(){
        ColorBlock Oncb = Onbutton.colors;
        ColorBlock Offcb = Offbutton.colors;
        Oncb.normalColor = Color.gray;
        Offcb.normalColor = Color.white;
        Onbutton.colors = Oncb;
        Offbutton.colors = Offcb;
        Keybynd.SetActive(false);
    }

}
