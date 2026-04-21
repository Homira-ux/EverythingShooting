using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingContents : MonoBehaviour
{
    private Slider slider;
    private PlayerMouseLook Player;
    // Start is called before the first frame update
    void OnEnable(){
        Player = GameObject.FindWithTag("MainCamera").GetComponent<PlayerMouseLook>();
    }
    
    void Start()
    {
        slider = GetComponent<Slider>();    
        slider.maxValue = 120f;
        slider.minValue = 80f; 
        slider.value = Player.mouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        Player.mouseSensitivity = slider.value;
        GameDataManager.Instance.gameData.MouseSensitivity = Player.mouseSensitivity;
    }
}
