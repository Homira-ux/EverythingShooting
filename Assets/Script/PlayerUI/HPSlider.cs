using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
    private Slider slider;
    private PlayerMovement playermovement;
    // Start is called before the first frame update

    void OnEnable(){
        playermovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }
    
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = playermovement.HP;
        slider.minValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = playermovement.HP;           
    }
}
