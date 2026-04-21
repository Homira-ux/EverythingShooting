using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStatus{
    public int HP;
    public int Attack;
    public float AttackSpeed;
    public float BulletSpeed;

    public EnemyStatus(int hp, int attack, float attackspeed, float bulletspeed){
        HP = hp;
        Attack = attack;
        AttackSpeed = attackspeed;
        BulletSpeed = bulletspeed;
    }
}

public class EnemyStatusManager : MonoBehaviour
{
    public Dictionary<string, EnemyStatus> EnemyStatusDictionary;
    
    // Start is called before the first frame update
    public void InitEnemyDictionary()
    {
        EnemyStatusDictionary = new Dictionary<string, EnemyStatus>
        {
            {"TutorialEnemy", new EnemyStatus(100000, 25, 10.0f, 0.4f)}
        };        
    }

    //敵ステータス取得メソッド
    public EnemyStatus GetEnemyStatus(string enemyType){
        if(EnemyStatusDictionary.TryGetValue(enemyType, out EnemyStatus status)){
            return status;
        }
        else{
            Debug.LogError("Enemy Type Not Found");
            return null;
        }
    }

    //敵ステータス初期化
    public void InitializeEnemy(GameObject Enemy, string enemyType){
        EnemyStatus enemy = GetEnemyStatus(enemyType);

        if(enemy != null){
            EnemyMovement enemyComponent = Enemy.GetComponent<EnemyMovement>();
            
            if(enemyComponent != null){
                enemyComponent.SetInitialStatus(enemy.HP, enemy.Attack, enemy.AttackSpeed, enemy.BulletSpeed);
            }
        }
    }

}
