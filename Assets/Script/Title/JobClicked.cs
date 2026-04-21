using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobClicked : MonoBehaviour
{
    private Button button;
    private ColorBlock cb;

    void Start(){
        button = GetComponent<Button>();
    } 

    public void JobSelected(string job){
        GameDataManager.Instance.gameData.SelectWeapon = job;
    }

    public void OtherJobSelected(){
        button.OnDeselect(null);
    }
}
