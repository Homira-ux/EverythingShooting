using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobChange : MonoBehaviour
{
    public GameObject AwakeDisplay;
    public GameObject JobChangeDisplay;
    public GameObject WeaponChangeDisplay;

    public void JobChangeClicked(){
        AwakeDisplay.SetActive(true);
        JobChangeDisplay.SetActive(true);
    }

    public void JobChangeApply(){
        GameDataManager.Instance.SaveGameData();
        AwakeDisplay.SetActive(false);
    }

}
