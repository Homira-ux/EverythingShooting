using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
//---Enemy---//
    public IEnumerator TutorialEnemyMovement(float move, float attackspeed, float bulletspeed, float attack, Transform Enemy, GameObject bulletprefab, Transform PlayerBody, Transform bulletposition, int RandomThrehold){
        float framesPerSecond = 60f;
        float waitTime = 1f / framesPerSecond;
        float movement = move / (2.0f * framesPerSecond);
        int randomattack = Random.Range(0,101);

        float duration = bulletspeed + 1.0f;

        Vector3 defaultbulletposition = bulletposition.localPosition;

        if(randomattack > RandomThrehold){
            Vector3 bulletposition1 = new Vector3(bulletposition.localPosition.x, -bulletposition.localPosition.y, bulletposition.localPosition.z);
            Vector3 bulletposition2 = new Vector3(bulletposition.localPosition.y, bulletposition.localPosition.x, bulletposition.localPosition.z);
            Vector3 bulletposition3 = new Vector3(-bulletposition.localPosition.y, -bulletposition.localPosition.x, bulletposition.localPosition.z);
            StartCoroutine(Enemyshot(duration, bulletspeed, bulletprefab, PlayerBody, bulletposition));
            bulletposition.localPosition = bulletposition1;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Enemyshot(duration, bulletspeed, bulletprefab, PlayerBody, bulletposition));
            bulletposition.localPosition = bulletposition2;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Enemyshot(duration, bulletspeed, bulletprefab, PlayerBody, bulletposition));
            bulletposition.localPosition = bulletposition3;
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(Enemyshot(duration, bulletspeed, bulletprefab, PlayerBody, bulletposition));
        }
        else{
            yield return StartCoroutine(Enemyshot(duration, bulletspeed, bulletprefab, PlayerBody, bulletposition));
        }
        
        bulletposition.localPosition = defaultbulletposition;
                
        yield break;
    }


    public IEnumerator Enemyshot(float duration, float bulletspeed, GameObject prefab, Transform playerBody, Transform bulletposition){
        GameObject bullet = Instantiate(prefab, bulletposition.position, bulletposition.rotation);
        Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
        rbBullet.isKinematic = true;

        yield return new WaitForSeconds(2.0f);

        float elapsedTime = 0f;
        float framesPerSecond = 60f;
        float waitTime = 1f / framesPerSecond;

        Vector3 bulletDirection = (playerBody.position - bullet.transform.position).normalized;

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
//-----------//

//---Player---//
    public IEnumerator HitEffect(GameObject bullet, GameObject HitParticlePrefab){
        GameObject HitParticle = Instantiate(HitParticlePrefab, bullet.transform.position, bullet.transform.rotation * HitParticlePrefab.transform.rotation);
        yield return new WaitForSeconds(0.30f);
        Destroy(HitParticle);
        yield break;
    }
}
