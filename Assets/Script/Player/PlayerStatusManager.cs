using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    public int HP;
    public float Attack;
    public float AttackSpeed;
    public float BulletSpeed;
    public float MoveSpeed;
    public GameObject Weapon;
    public GameObject Bullet;
    public GameObject HitEffect;

    public PlayerStatus(int hp, float attack, float attackspeed, float bulletspeed, float movespeed, GameObject weapon, GameObject bullet, GameObject hiteffect){
        HP = hp;
        Attack = attack;
        AttackSpeed = attackspeed;
        BulletSpeed = bulletspeed;
        MoveSpeed = movespeed;
        Weapon = weapon;
        Bullet = bullet;
        HitEffect = hiteffect;
    }   
}

public class PlayerStatusManager : MonoBehaviour
{
    public GameObject Katana;
    public GameObject KatanaHitEffect;
    public GameObject KatanaBullet;
    public Dictionary<string, PlayerStatus> PlayerStatusDictionary;

    public void InitPlayerDictionary(){
        PlayerStatusDictionary = new Dictionary<string, PlayerStatus>
        {
            {"SwordMaster", new PlayerStatus(100, 1200f, 0.75f, 1.0f, 5f, Katana, KatanaBullet, KatanaHitEffect)}
        };
    }

    public PlayerStatus GetPlayerStatus(string playertype){
        bool statusGet = PlayerStatusDictionary.TryGetValue(playertype, out PlayerStatus status);
        if(statusGet){
            return status;
        }
        else{
            Debug.LogError("Player Type Not Found");
            return null;
        }
    }
    public void InitializePlayer(GameObject Player, string playerType){
        PlayerStatus player = GetPlayerStatus(playerType);

        if(player != null){
            PlayerMovement playerComponent = Player.GetComponent<PlayerMovement>();

            if(playerComponent != null){
                playerComponent.SetInitialStatus(player.HP, player.Attack + GameDataManager.Instance.gameData.BasicAttack, player.AttackSpeed, player.BulletSpeed, player.MoveSpeed, player.Weapon, player.Bullet, player.HitEffect);
            }
        }
    }

}


