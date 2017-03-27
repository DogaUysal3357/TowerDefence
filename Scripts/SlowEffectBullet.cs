using UnityEngine;
using System.Collections;

public class SlowEffectBullet : MonoBehaviour {

    private GameObject bulletGO;
    private GameObject targetGO;
    private GameObject towerThatShotGO;
    private Bullet bulletScript;

    private float slowAmount;
    private float slowDuration;
    private float distanceToTarget;

    void Start()
    {
        bulletGO = this.gameObject;
        bulletScript = bulletGO.GetComponent<Bullet>();

        targetGO = bulletScript.getTarget();
        towerThatShotGO = bulletScript.getTowerThatShot();
        slowAmount = towerThatShotGO.GetComponent<SlowEffectTurret>().getSlowAmount();
        slowDuration = towerThatShotGO.GetComponent<SlowEffectTurret>().getSlowDuration();    
    }

    void Update()
    {
        distanceToTarget = bulletScript.getDistanceToTarget();

        if(distanceToTarget <= 1)
        {
            targetGO.GetComponent<HitEffects>().ApplySlow(slowAmount, slowDuration);
        }
    }
}
