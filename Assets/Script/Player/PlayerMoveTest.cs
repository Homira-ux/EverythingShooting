using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    public CoroutineManager coroutine;
    private WeaponCoroutine weaponshot;
    public PlayerHP playerhp;
    public GameObject UIObject;
    private Animator PlayerUI;

    public Transform bulletDirection;

    private float holdTime = 0.0f;
    public float holdThreshold = 0.1f;
    public float ChargeTime = 5f;
    private bool ChargeState = false;
    private int Chargeflag = 0;
    private bool shot = false;
    private bool isFullcharge = false;
    public bool isSheathing = false;
    public float SheathingThrehold = 5.0f;

    public float SheathingTime = 0.0f;

    public int HP;
    public float Attack;
    public float currentAttack;
    public float bulletSpeed;
    public float attackSpeed;
    public float MoveSpeed;
    private float currentMoveSpeed;
    public GameObject Weapon;
    public GameObject Bullet;
    public GameObject HitParticle;

    private Animator Weaponanimator;
    private Animator PlayerAnimator;
    public AudioClip SEClip;
    private AudioSource audioSource;

    public float throughTime = 0;
    public float duration = 1.25f;
    public bool SpecialFinish = false;
    public float AttackBoostTime = 30f;
    public float Boosting = 0f;

    bool isFirstAttack;

    public Transform playerBody;

    public float defaultspeed = 12f;
    public float gravity = -9.81f;

    private EnemyMovement enemystatus;
    private PlayerController playercontroller;
    public GameManager gamemanager;
    public PlayerMouseLook PlayerMouse;
    private MonoBehaviour PlayerMouseLookScript;

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

        GameObject Weaponprefab = Instantiate(Weapon, transform.position, transform.rotation);
        Weaponprefab.transform.SetParent(playerBody);
        weaponshot = Weaponprefab.GetComponent<WeaponCoroutine>();

        PlayerAnimator = GetComponent<Animator>();

        isFirstAttack = true;

        Weaponanimator = Weaponprefab.GetComponent<Animator>();
        audioSource = Weaponprefab.GetComponent<AudioSource>();
        enemystatus = GameObject.Find("Enemy").GetComponent<EnemyMovement>();
        playercontroller = GetComponent<PlayerController>();
        
        SEClip = audioSource.clip;

        currentAttack = Attack;
        currentMoveSpeed = MoveSpeed;

        PlayerUI = UIObject.GetComponent<Animator>();
        PlayerMouseLookScript = PlayerMouse.GetComponent<MonoBehaviour>();
    }

    void Update()
    {
        //”[“UŒ‚
        if(!isSheathing &&  Input.GetMouseButtonDown(1)){
            PlayerAnimator.SetTrigger("Special");
            Weaponanimator.SetTrigger("Special");

            SpecialFinish = false;
            isSheathing = true;
            MoveSpeed = currentMoveSpeed / 2.0f;
            PlayerMouse.isSpecialCharge = true;
        }
        if(isSheathing && Input.GetMouseButton(1)){
            SheathingTime += Time.deltaTime;

            if(SheathingTime >= SheathingThrehold){
                if(Input.GetMouseButtonDown(0)){
                    Attack = currentAttack * 4.0f;
                    StartCoroutine(weaponshot.specialshot(playerBody, duration, bulletSpeed, Bullet, bulletDirection, Attack, enemystatus.transform));

                    throughTime = Time.time;
                    isSheathing = false;
                    MoveSpeed = currentMoveSpeed;
                    SheathingTime = 0.0f;
                    Boosting = Time.time;

                    if(!SpecialFinish){
                        Weaponanimator.SetTrigger("SpecialFinish");
                        PlayerAnimator.SetTrigger("SpecialFinish");
                        SpecialFinish = true;
                    }
                }
            }
        }


        if(Input.GetMouseButtonUp(1)){
            isSheathing = false;
            MoveSpeed = currentMoveSpeed;
            SheathingTime = 0.0f;

            if(!SpecialFinish){
                Weaponanimator.SetTrigger("SpecialFinish");
                PlayerAnimator.SetTrigger("NoSpecial");
                SpecialFinish = true;
            }
        }

        if(Boosting + AttackBoostTime < Time.time){
            Attack = currentAttack;
        }

        //”²“UŒ‚
        if(!isSheathing){
            PlayerMouse.isSpecialCharge = false;
            if(Input.GetMouseButtonDown(0)){
                if(isFirstAttack ||Time.time >= (attackSpeed + throughTime)){
                    isFirstAttack = false;
                    shot = true;
                    holdTime = 0.0f;
                    ChargeState = false;
                }
            }

            if(Input.GetMouseButton(0) && shot){
                holdTime += Time.deltaTime;
                if(holdTime > holdThreshold && ChargeState == false){
                    playercontroller.isFreezed = true;
                    ChargeState = true;
                    Weaponanimator.SetTrigger("Charge");
                }

                if(isFullcharge == false){
                    if(Chargeflag == 0 && holdTime >= (ChargeTime / 3f)){
                        audioSource.pitch = 0.8f;
                        Chargeflag = 1;
                    }
                    if(Chargeflag == 1 && holdTime >= (ChargeTime * 2f / 3f)){
                        audioSource.pitch = 1.2f;
                        Chargeflag = 2;
                    }
                    if((Chargeflag == 2 && holdTime >= ChargeTime) && !isFullcharge){
                        isFullcharge = true;
                        audioSource.pitch = 1.8f;
                        audioSource.PlayOneShot(SEClip);
                        Weaponanimator.SetTrigger("FullCharge");
                        Chargeflag = 3;
                    }
                }
            }

            
            if(shot && Input.GetMouseButtonUp(0)){
                shot = false;
                throughTime = Time.time;
                playercontroller.isFreezed = false;

                if(holdTime < holdThreshold){
                    Attack = currentAttack;
                    Weaponanimator.SetTrigger("Shot");
                }
                else if(holdTime < ChargeTime){
                    Attack = currentAttack * (1 + holdTime);
                    Weaponanimator.SetTrigger("ChargeShot");
                }
                else{
                    Attack = currentAttack * 6.0f;
                    Weaponanimator.SetTrigger("ChargeShot");
                }
                StartCoroutine(weaponshot.shot(duration, bulletSpeed, Bullet, bulletDirection, Attack, Chargeflag));
                Chargeflag = 0;
                isFullcharge = false;
            }
        }
    }

    public void TakeDamage(int damage){
        HP -= damage;
        playerhp.setHP(damage);
        PlayerUI.SetTrigger("Damage");
        if(HP <= 0){
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
