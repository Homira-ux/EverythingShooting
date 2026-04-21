using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponCoroutine : MonoBehaviour
{
    public abstract IEnumerator shot(float duration, float bulletspeed, GameObject prefab, Transform bulletposition, float attack, int ChargeFlag);
    public abstract IEnumerator specialshot(Transform Player, float duration, float bulletspeed, GameObject prefab, Transform bulletposition, float attack, Transform Enemyposition);
}
