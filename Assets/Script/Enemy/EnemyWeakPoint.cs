using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeakPoint : MonoBehaviour
{
    private EnemyMovement enemymovement;
    // Start is called before the first frame update
    void Start()
    {
        Transform parent = transform.parent;
        enemymovement = parent.GetComponent<EnemyMovement>();
    }

    void OnTriggerEnter(Collider other){
        enemymovement.WeakFlag = true;
    }
}
