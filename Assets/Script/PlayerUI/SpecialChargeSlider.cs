using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialChargeSlider : MonoBehaviour
{
    private Slider slider;
    private PlayerMovement playermovement;
    CanvasGroup canvas;

    void OnEnable(){
        slider = GetComponent<Slider>();
        canvas = slider.GetComponent<CanvasGroup>();
        playermovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = playermovement.SheathingThreshold;
        slider.minValue = 0;
        canvas.alpha = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = playermovement.ElapsedTime;           
    }

    public void SetSpecialSlider(){
        canvas.alpha = 1;
        slider.maxValue = playermovement.SheathingThreshold;
        slider.minValue = 0;        
    }
    public void SetNormalSlider(){
        canvas.alpha = 1;
        slider.maxValue = playermovement.ChargeTime;
        slider.minValue = 0;
    }
    public void SetAlpha0(){
        canvas.alpha = 0;
    }
}
