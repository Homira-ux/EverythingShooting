using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    private Animator SettingAnimation;
    private Animator SettingDisplayAnimation;
    public GameObject SettingDisplay;
    public GameManager gamemanager;

    public bool isPushed = false;

    void OnEnable(){
        SettingAnimation = GetComponent<Animator>();
        SettingDisplayAnimation = SettingDisplay.GetComponent<Animator>();
        SettingAnimation.updateMode = AnimatorUpdateMode.UnscaledTime;
        SettingDisplayAnimation.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void SettingClicked(){
        if(isPushed){
            Time.timeScale = 1f;
            GameDataManager.Instance.SaveGameData(); 
            isPushed = false;
            gamemanager.GamePaused = false;
            SettingAnimation.SetTrigger("CloseSetting");
            SettingDisplayAnimation.SetTrigger("CloseSetting");
        }
        else{
            Time.timeScale = 0f;
            isPushed = true;
            gamemanager.GamePaused = true;
            SettingAnimation.SetTrigger("OpenSetting");
            SettingDisplayAnimation.SetTrigger("OpenSetting");
        }
    }   

}
