using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    TMP_Text Score;
    // Start is called before the first frame update
    void Start()
    {
        Score = GetComponent<TMP_Text>();
    }

    public void Display(int score){
        Score.text = "SCORE : " + score.ToString();
    }
}
