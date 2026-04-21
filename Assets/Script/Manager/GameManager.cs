using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Tutorial;
    public GameObject Player;
    public PlayerStatusManager playerstatusmanager;
    public EnemyStatusManager enemystatusmanager;
    
    private PlayerController playercontroller;
    
    public MonoBehaviour PlayerMovement;
    public MonoBehaviour EnemyMovement;
    
    public GameObject CountDownObject;
    public TMP_Text CountDownText;

    public SettingMenu setting;

    private Rigidbody Playerrb;

    public GameObject UIObject;
    private Animator PlayerUI;

    public GameObject ReplayButton;
    public GameObject ReturnTitleButton;
    public ScoreDisplay Scoredisplay;

    public bool isPlayerlives = false;
    public bool isEnemylives = false;
    public bool GamePaused = false;

    public bool SettingState = false;

    private bool isGameFinished = false;

    public int Score;
    public int LowestScore = 200;

    // Start is called before the first frame update
    void Awake(){
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);

        GameDataManager.Instance.LoadGameData();
 
        enemystatusmanager.InitEnemyDictionary();
        playerstatusmanager.InitPlayerDictionary();
        enemystatusmanager.InitializeEnemy(Tutorial, "TutorialEnemy");
        playerstatusmanager.InitializePlayer(Player, GameDataManager.Instance.gameData.SelectWeapon);

        PlayerMovement = Player.GetComponent<MonoBehaviour>();
        EnemyMovement = Tutorial.GetComponent<MonoBehaviour>();

        playercontroller = Player.GetComponent<PlayerController>();

        Playerrb = Player.GetComponent<Rigidbody>();
        PlayerUI = UIObject.GetComponent<Animator>();
    }

    void OnEnable()
    {
        GamePaused = false;

        ReplayButton.SetActive(false);
        ReturnTitleButton.SetActive(false);

        Score = 0;
    }

    void Start(){
        StartCoroutine(CountDown(5.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if(!SettingState && Input.GetKey(KeyCode.Escape)){
            SettingState = true;
            setting.SettingClicked();
            WaitForSetting(500);
        }

        if(isGameFinished){
            isGameFinished = false;
            GameDataManager.Instance.gameData.TotalScore += Score;
            GameDataManager.Instance.SaveGameData();    
        }
    }

    public void PlayerDead(){
        PlayerMovement.enabled = false;
        Playerrb.isKinematic = true;
        
        Score = (int)Time.time * LowestScore / 100;
        
        EnemyMovement.enabled = false;
        PlayerUI.SetTrigger("Die");
        Scoredisplay.Display(Score);
        
        ReplayButton.SetActive(true);
        ReturnTitleButton.SetActive(true);
        
        GamePaused = true;
        isGameFinished = true;
    }

    public void EnemyDead(){
        EnemyMovement.enabled = false;
        PlayerMovement.enabled = false;
                
        Playerrb.isKinematic = true;
        PlayerUI.SetTrigger("Win");
        Scoredisplay.Display(Score);
        
        ReplayButton.SetActive(true);
        ReturnTitleButton.SetActive(true);
        
        GamePaused = true;
        isGameFinished = true;
    }

    private async void WaitForSetting(int millistime){
        await Task.Delay(millistime);
        SettingState = false;
    }

    private IEnumerator CountDown(float time){
        while(time > 0.0f){
            CountDownText.text = ((int)time).ToString();
            yield return new WaitForSeconds(1.0f);
            time -= 1.0f;
        }

        CountDownText.text = "Game Start!!";

        isPlayerlives = true;
        isEnemylives = true;

        yield return new WaitForSeconds(1.0f);

        CountDownObject.SetActive(false);

        yield break;
    }
}
