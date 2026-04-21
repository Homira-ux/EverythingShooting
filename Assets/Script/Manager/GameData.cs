using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int Levels;
    public int TotalScore;
    public int BasicAttack;
    
    public float MouseSensitivity;
    public bool KeybyndDisplay;

    public string SelectWeapon;
    public string Weapon;

    public GameData(){
        Levels = 0;
        TotalScore = 0;
        BasicAttack = 0;

        MouseSensitivity = 100f;
        KeybyndDisplay = true;

        SelectWeapon = "SwordMaster";
        Weapon = "Katana";
    }
}
