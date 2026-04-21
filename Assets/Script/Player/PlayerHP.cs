using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    private TMP_Text playerhp;
    private PlayerMovement playermovement;
    public int currentHP;

    // Start is called before the first frame update
    void OnEnable(){
        playermovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }
    void Start()
    {
        playerhp = GetComponent<TMP_Text>();
        currentHP = playermovement.HP;
        playerhp.text = $"{playermovement.HP.ToString()} / {playermovement.HP.ToString()}";
    }

    public void setHP(int damage){
        playerhp.text = $"{playermovement.HP.ToString()} / {currentHP.ToString()}";
    }
}
