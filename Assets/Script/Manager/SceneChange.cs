using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void NormalPlay(){
        SceneManager.LoadScene("PlayNormal");
    }
    
    public void returnTitle(){
        SceneManager.LoadScene("Title");
    }

}
