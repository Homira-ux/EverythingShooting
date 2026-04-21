using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{   
    //呼び出されたEnemyのタイプからステータスを読み込む
    public int HP;
    public int Attack;
    public float AttackSpeed;
    public float bulletSpeed;

    private float currentAttackSpeed;

    public float Angry;
    public int RandomThrehold;
    private bool isAngry;

    public CoroutineManager coroutine;

    private Transform playerBody;
    public Transform EnemyObject;
    public Transform EnemyDirection;
    public GameObject DamageTextPrefab;

    public float duration = 2.0f;
    public float throughTime;

    public GameObject EnemyBullet;
    private PlayerMovement playerstatus;
    public GameManager gamemanager;
    private Animator EnemyAnimator;
    private Rigidbody rb;

    private float AttackRandom = 3.0f;

    public bool WeakFlag = false;

    public void SetInitialStatus(int hp, int attack, float attackspeed, float bulletspeed){
        HP = hp;
        Attack = attack;
        AttackSpeed = attackspeed;
        bulletSpeed = bulletspeed;  
    }

    void OnEnable(){
        playerBody = GameObject.FindWithTag("Player").transform;
        playerstatus = playerBody.GetComponent<PlayerMovement>();

        EnemyAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Start(){
        Angry = HP / 2.0f;
        RandomThrehold = 60;
        isAngry = false;

        currentAttackSpeed = AttackSpeed;
    }   

    void Update(){
        if(gamemanager.isEnemylives){
            if(Time.time >= (AttackSpeed + throughTime)){
                float starttime = Time.time;
                StartCoroutine(coroutine.TutorialEnemyMovement(5.0f, AttackSpeed, bulletSpeed, Attack, EnemyObject, EnemyBullet, playerBody, EnemyDirection, RandomThrehold));
                throughTime = Time.time;
                AttackSpeed = currentAttackSpeed +  Random.Range(-AttackRandom, AttackRandom);
            }

            transform.LookAt(playerBody);

            if(!isAngry){
                if(HP < Angry){
                    isAngry = true;
                    currentAttackSpeed /= 1.5f;
                    AttackRandom /= 1.5f;
                    RandomThrehold = 30;
                    EnemyAnimator.SetTrigger("Angry");
                }
            }
        }
    }

    public void TakeDamage(float damage, Vector3 hitPosition){
        if(HP > 0){
            HP -= (int)damage;
            gamemanager.Score += (int)(damage / 10.0f);
            if(HP <= 0 && gamemanager.isEnemylives){
                gamemanager.isEnemylives = false;
                Die();
            }

            ShowDamageText(damage, hitPosition);
        }
    }

    public void TakeWeakDamage(float damage, Vector3 hitPosition){
        damage *= 2f;
        HP -= (int)damage;
        if(HP <= 0 && gamemanager.isEnemylives){
            gamemanager.isEnemylives = false;
            Die();
        }

        ShowDamageText((int)damage, hitPosition);
    }

    private void ShowDamageText(float damage, Vector3 hitPosition){
        Vector3 transposition = new Vector3 (0, 5f, 0);
        GameObject damageText = Instantiate(DamageTextPrefab, hitPosition + transposition, Quaternion.identity);
        damageText.transform.SetParent(GameObject.Find("Damage").transform, false);
        WeaponDamage weapondamage = damageText.GetComponent<WeaponDamage>();
        if(weapondamage != null){
            weapondamage.SetDamage((int)damage);
        }
    }

    public void Die(){
        StopAllCoroutines();
        EnemyAnimator.SetTrigger("Die");
        SetFreezePosition();
        SetFreezeRotation();
        gamemanager.EnemyDead();
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.CompareTag("playerbullet")){
            other.collider.enabled = false;
            playerstatus.WeaponHitEffect(other);
            Destroy(other.gameObject);           
            float damage = playerstatus.Attack;
            if(WeakFlag){
                TakeWeakDamage(damage, other.gameObject.transform.position);
                WeakFlag = false;
            }
            else{
                TakeDamage(damage, other.gameObject.transform.position);
            }
        }
    }

    public void SetFreezePosition()
    {
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None; // すべての制約を解除
            rb.constraints |= RigidbodyConstraints.FreezePositionX;
            rb.constraints |= RigidbodyConstraints.FreezePositionY;
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        }
    }

    public void SetFreezeRotation()
    {
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None; // すべての制約を解除
            rb.constraints |= RigidbodyConstraints.FreezeRotationX;
            rb.constraints |= RigidbodyConstraints.FreezeRotationY;
            rb.constraints |= RigidbodyConstraints.FreezeRotationZ;
        }
    }
}
