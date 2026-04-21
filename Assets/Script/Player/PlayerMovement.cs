using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //コルーチン処理
    public CoroutineManager coroutine;
    private WeaponCoroutine weaponshot;

    //UI関連
    public PlayerHP playerhp;
    public GameObject UIObject;
    private Animator PlayerUI;

    //弾発射場所
    public Transform bulletDirection;

    //要変更クールタイム関連
    public bool isCool = true;     //クールタイム状態変数
    public bool isNormalAttack = false;  //通常攻撃処理状態変数
    public bool isSpecialAttack = false;  //特殊攻撃処理状態変数
    public float holdThreshold = 0.1f;  //通常攻撃チャージ閾値
    public float ChargeTime = 5f;   //通常攻撃チャージ完了時間
    private bool ChargeState = false;   //通常攻撃チャージ状態変数
    private int Chargeflag = 0;     //チャージ時間判定
    public float ElapsedTime = 0.0f;    //特殊攻撃チャージ時間
    private bool isChargeAnimation = false; //チャージアニメーション実行状態
    private bool isFullChargeAnimation = false;
    private bool isSheated = false;
    public float SheathingThreshold = 5.0f;

    //PlayerStatus
    public int HP;
    public float Attack;
    public float currentAttack;
    public float bulletSpeed;
    public float attackSpeed;
    public float MoveSpeed;
    private float currentMoveSpeed;
    private GameObject Weapon;
    private GameObject Bullet;
    private GameObject HitParticle;

    //Animator関連
    private Animator Weaponanimator;
    private Animator PlayerAnimator;
    public AudioClip SEClip;
    private AudioSource audioSource;
    
    //要変更クールタイム関連
    public float throughTime = 0;
    public float duration = 1.25f;
    public bool SpecialFinish = false;
    public float AttackBoostTime = 30f;
    public float Boosting = 0f;
    
    //PlayerObject
    private Transform playerBody;

    //スクリプト読み込み
    private EnemyMovement enemystatus;
    private PlayerController playercontroller;
    public GameManager gamemanager;
    public PlayerMouseLook PlayerMouse;
    private MonoBehaviour PlayerMouseLookScript;
    public SpecialChargeSlider slider;

    //非同期のキャンセルクラス
    //納刀攻撃
    private CancellationTokenSource Sheathingcts;
    //通常攻撃
    private CancellationTokenSource Normalcts;
    //通常チャージ攻撃
    private CancellationTokenSource NormalFullcts;

    //プレイヤーステータス初期化
    public void SetInitialStatus(int hp, float attack, float attackspeed, float bulletspeed, float movespeed, GameObject weapon, GameObject bullet, GameObject hiteffect){
        HP = hp;
        Attack = attack;
        bulletSpeed = bulletspeed;
        attackSpeed = attackspeed;
        MoveSpeed = movespeed;
        Weapon = weapon;
        Bullet = bullet;
        HitParticle = hiteffect;
    }

    void Start(){
        if(Weapon == null){
            Debug.Log("Weapon is Null");
        }

        playerBody = this.transform;

        //コンポーネント読み込み
        GameObject Weaponprefab = Instantiate(Weapon, transform.position, transform.rotation);
        Weaponprefab.transform.SetParent(playerBody);
        weaponshot = Weaponprefab.GetComponent<WeaponCoroutine>();

        PlayerAnimator = GetComponent<Animator>();
        Weaponanimator = Weaponprefab.GetComponent<Animator>();

        //コンポーネント読み込み        
        audioSource = Weaponprefab.GetComponent<AudioSource>();
        enemystatus = GameObject.FindWithTag("Enemy").GetComponent<EnemyMovement>();
        playercontroller = GetComponent<PlayerController>();
        
        PlayerUI = UIObject.GetComponent<Animator>();
        PlayerMouseLookScript = PlayerMouse.GetComponent<MonoBehaviour>();
        
        SEClip = audioSource.clip;

        //可変ステータスの初期値
        currentAttack = Attack;
        currentMoveSpeed = MoveSpeed;

        //攻撃処理状態変数初期化
        isCool = true;
        isNormalAttack = false;
        isSpecialAttack = false;

        slider.SetAlpha0();
    }

    void Update()
    {
        if(isCool){
            //-----特殊攻撃
            if(!isNormalAttack){
                //特殊チャージ状態に入る
                if(Input.GetMouseButtonDown(1)){
                    //Animatorの初期化
                    ResetAllTriggers(PlayerAnimator);
                    ResetAllTriggers(Weaponanimator);

                    //！！！特殊攻撃処理開始！！！
                    isSpecialAttack = true;

                    //移動速度の低下
                    MoveSpeed = currentMoveSpeed / 3.0f;
                    //視点処理-敵オブジェクトに固定
                    PlayerMouse.isSpecialCharge = true;

                    //スライダーのセット
                    slider.SetSpecialSlider();

                    //アニメーション処理
                    PlayerAnimator.SetTrigger("Special");
                    Weaponanimator.SetTrigger("Special");

                    //特殊チャージ状態をリセット
                    isSheated = false;
                    ElapsedTime = 0.0f;

                    //チャージ処理
                    SheathingCharge();
                }

                //特殊チャージ解除
                if(Input.GetMouseButtonUp(1)){
                    //特殊チャージ状態によるステータス変化のリセット
                    MoveSpeed = currentMoveSpeed;
                    PlayerMouse.isSpecialCharge = false;
                
                    //特殊チャージ完了状態
                    if(isSheated){ 
                        Attack = currentAttack * 4.0f;
                        //特殊攻撃
                        StartCoroutine(weaponshot.specialshot(playerBody, duration, bulletSpeed, Bullet, bulletDirection, Attack, enemystatus.transform));

                        //特殊攻撃アニメーション
                        Weaponanimator.SetTrigger("SpecialFinish");
                        PlayerAnimator.SetTrigger("SpecialFinish");
                    }
                    else{
                        //非同期キャンセル処理
                        CancelCharging(Sheathingcts);

                        //特殊チャージ解除アニメーション
                        Weaponanimator.SetTrigger("NoSpecial");
                        PlayerAnimator.SetTrigger("NoSpecial");
                    }

                    //クールタイム
                    Cooling();
                    
                    //スライダ～リセット
                    slider.SetAlpha0();
                    ElapsedTime = 0.0f;

                    //特殊チャージ状態をリセット
                    isSheated = false;

                    //！！！特殊攻撃処理終了！！！
                    isSpecialAttack = false;                    

                    //オーバーヘッドの低減
                    return;
                }
            }
            //-----

            //-----通常攻撃
            if(!isSpecialAttack){
                //通常処理
                if(Input.GetMouseButtonDown(0)){
                    //Animatorの初期化
                    ResetAllTriggers(PlayerAnimator);
                    ResetAllTriggers(Weaponanimator);

                    //！！！通常攻撃処理開始！！！
                    isNormalAttack = true;
                    

                    //チャージ状態のリセット
                    ChargeState = false;
                    isChargeAnimation = false;
                    isFullChargeAnimation = false;

                    NormalCharge();
                }

                //攻撃処理
                if(isNormalAttack){
                    //チャージを閾値で判定
                    if(ChargeState){
                        if(!isChargeAnimation){
                            //スライダーセット
                            slider.SetNormalSlider();
                            
                            //プレイヤーを不動化
                            playercontroller.isFreezed = true;
                            Chargeflag = 1;

                            //チャージアニメーション
                            Weaponanimator.SetTrigger("Charge");
                            isChargeAnimation = true;
                        }
                        
                        if(Chargeflag == 2 && !isFullChargeAnimation){
                            Weaponanimator.SetTrigger("FullCharge");
                            isFullChargeAnimation = true;
                        }
                    }
                    else{
                        //チャージ状態をなしに
                        Chargeflag = 0;
                    }
                  
                    //通常解除
                    if(Input.GetMouseButtonUp(0)){
                        //プレイヤーを可動化
                        playercontroller.isFreezed = false;
                        //チャージ状態変数のリセット
                        ChargeState = false;

                        //チャージ未完了の場合、チャージ計測をキャンセル
                        if(Chargeflag != 2){
                            CancelCharging(Normalcts);
                        }

                        //チャージ時間に応じた攻撃力変化
                        switch(Chargeflag){
                            case 0 : Attack = currentAttack;                        //通常攻撃アニメーション
                                     Weaponanimator.SetTrigger("Shot");
                                     break;
                                    
                            case 1 : Attack = currentAttack * (1 + ElapsedTime);
                                    //チャージ攻撃アニメーション
                                     Weaponanimator.SetTrigger("ChargeShot");
                                     break;

                            case 2 : Attack = currentAttack * 6.0f;
                                     //チャージ攻撃アニメーション
                                     Weaponanimator.SetTrigger("ChargeShot");
                                     break;
                        }

                        //通常攻撃処理
                        StartCoroutine(weaponshot.shot(duration, bulletSpeed, Bullet, bulletDirection, Attack, Chargeflag));
                        
                        //スライダーリセット
                        slider.SetAlpha0();
                        ElapsedTime = 0.0f;

                        Chargeflag = 0;
                        
                        //！！！通常攻撃処理終了！！！
                        isNormalAttack = false;

                        //クールタイム
                        Cooling();

                        //オーバーヘッドの低減
                        return;
                    }
                }


            }
            
        }
    }

//クールタイム処理
    private async void Cooling(){
        isCool = false;
        await Task.Delay((int)(attackSpeed * 1000));
        isCool = true;
    }

//特殊チャージ処理
    private async void SheathingCharge(){
        Sheathingcts = new CancellationTokenSource();

        isSheated = await ChargeJudge(Sheathingcts.Token, SheathingThreshold);
    }

//非同期キャンセル処理
    private void CancelCharging(CancellationTokenSource cts)
    {
        // キャンセルを通知
        if (cts != null)
        {
            cts.Cancel();
        }
    }

//通常攻撃
    private async void NormalCharge(){
        Normalcts = new CancellationTokenSource();

        ChargeState = await ChargeJudge(Normalcts.Token, holdThreshold);
        if(ChargeState){
            try{
                await Charge(Normalcts.Token, ChargeTime);
                Chargeflag = 2;
            }
            catch(TaskCanceledException){
                return;
            }
        }
    }

//チャージ時間計測
    private async Task Charge(CancellationToken token, float ChargeTimeThrehold){
        ElapsedTime = 0.0f;

        while(ChargeTimeThrehold > ElapsedTime){
            // キャンセルされていないか確認
            if(token.IsCancellationRequested){
                ElapsedTime = 0.0f;
                return;
            }
            await Task.Yield();
            ElapsedTime += Time.deltaTime;
        }
    }

//チャージ時間計測-成功時状態変数の変更、失敗時非同期キャンセル処理
    private async Task<bool> ChargeJudge(CancellationToken token, float ChargeTimeThrehold){
        ElapsedTime = 0.0f;

        while(ChargeTimeThrehold > ElapsedTime){
            // キャンセルされていないか確認
            if(token.IsCancellationRequested){
                ElapsedTime = 0.0f;
                return false;
            }
            await Task.Yield();
            ElapsedTime += Time.deltaTime;
        }

        return true;
    }

//連打によるトリガーの持ち越し防止のトリガーリセット
    public void ResetAllTriggers(Animator animator){
        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }
    }

    public void TakeDamage(int damage){
        HP -= damage;
        playerhp.setHP(damage);
        PlayerUI.SetTrigger("Damage");
        if(HP <= 0 && gamemanager.isPlayerlives){
            gamemanager.isPlayerlives = false;
            Die();
        }
    }

    public void Die(){
        gamemanager.PlayerDead();
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.CompareTag("Enemybullet")){
            int damage = enemystatus.Attack;
            TakeDamage(damage);
            Destroy(other.gameObject);
        }
    }

    public void WeaponHitEffect(Collision other){
        StartCoroutine(coroutine.HitEffect(other.gameObject, HitParticle));
    }
}
