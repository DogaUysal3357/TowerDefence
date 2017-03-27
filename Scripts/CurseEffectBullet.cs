using UnityEngine;
using System.Collections;

public class CurseEffectBullet : MonoBehaviour {

    private GameObject bulletGO;
    private GameObject targetGO;
    private GameObject towerThatShotGO;
    private Bullet bulletScript;

    private float cursePercent;
    private float curseDuration;
    private float distanceToTarget;

    void Start()
    {
        bulletGO = this.gameObject;
        bulletScript = bulletGO.GetComponent<Bullet>();

        targetGO = bulletScript.getTarget();
        towerThatShotGO = bulletScript.getTowerThatShot();
        cursePercent = towerThatShotGO.GetComponent<CurseEffectTurret>().getCursePercent();
        curseDuration = towerThatShotGO.GetComponent<CurseEffectTurret>().getCurseDuration();
    }

    void Update()
    {
        distanceToTarget = bulletScript.getDistanceToTarget();

        if (distanceToTarget <= 1)
        {
            targetGO.GetComponent<HitEffects>().ApplyCurse(cursePercent, curseDuration);
        }
    }
}
