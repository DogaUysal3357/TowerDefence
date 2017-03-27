using UnityEngine;
using System.Collections;

public class IgniteEffectBullet : MonoBehaviour {

    private GameObject bulletGO;
    private GameObject targetGO;
    private GameObject towerThatShotGO;
    private Bullet bulletScript;

    private float igniteDamage;
    private float igniteDuration;
    private float distanceToTarget;

    void Start()
    {
        bulletGO = this.gameObject;
        bulletScript = bulletGO.GetComponent<Bullet>();

        targetGO = bulletScript.getTarget();
        towerThatShotGO = bulletScript.getTowerThatShot();
        igniteDamage = towerThatShotGO.GetComponent<IgniteEffectTurret>().getIgniteDamage();
        igniteDuration = towerThatShotGO.GetComponent<IgniteEffectTurret>().getIgniteDuration();
    }

    void Update()
    {
        distanceToTarget = bulletScript.getDistanceToTarget();

        if (distanceToTarget <= 1)
        {
            targetGO.GetComponent<HitEffects>().ApplyIgnite(igniteDamage, igniteDuration);
        }
    }

}
