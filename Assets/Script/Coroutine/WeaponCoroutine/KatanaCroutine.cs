using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaCroutine : WeaponCoroutine
{
    public override IEnumerator shot(float duration, float bulletspeed, GameObject prefab, Transform bulletposition, float attack, int ChargeFlag){
        yield return new WaitForSeconds(0.1f);
        GameObject bullet = Instantiate(prefab, bulletposition.position, bulletposition.rotation*prefab.transform.rotation);
        bullet.SetActive(false);
        Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
        rbBullet.isKinematic = true;

        float elapsedTime = 0f;
        float framesPerSecond = 60f;
        float waitTime = 1f / framesPerSecond;

        if(ChargeFlag == 0){
            bullet.transform.Find("KatanaBulletFire").gameObject.SetActive(false);
        }
        else{
            if(ChargeFlag == 2){
                bullet.transform.localScale = new Vector3(prefab.transform.localScale.x, prefab.transform.localScale.y * 1.5f, prefab.transform.localScale.z);
            }
            bullet.transform.Rotate(0,0,-60);
            yield return new WaitForSeconds(0.25f);
        }

        bullet.SetActive(true);

        Vector3 bulletDirection = bulletposition.forward;

        while(elapsedTime < duration){
            elapsedTime += waitTime;
            if(bullet != null){
                bullet.transform.position += bulletDirection * bulletspeed;
            }
            else{
                break;
            }
            yield return new WaitForSeconds(waitTime);
        }
        Destroy(bullet);
        yield break;
    }

    public override IEnumerator specialshot(Transform Player, float duration, float bulletspeed, GameObject prefab, Transform bulletposition, float attack, Transform Enemyposition){
        yield return new WaitForSeconds(0.5f);
        Player.LookAt(Enemyposition);
        int AttackNumber = 10;

        for(int count = 0; count < AttackNumber; count++){
            GameObject bullet = Instantiate(prefab, Enemyposition.position, Quaternion.Euler((360/AttackNumber)*count,(360/AttackNumber)*count,(360/AttackNumber)*count));
            yield return new WaitForSeconds(0.05f);
        }

        yield break;
    }
    
}
