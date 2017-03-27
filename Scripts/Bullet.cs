using UnityEngine;

public class Bullet : MonoBehaviour {

    [Header("Attributes")]
    public float speed = 15f;

    [Header("Setup Map")]
    public Transform target;
    public GameObject impactEffect;
    private GameObject targetGO;
    private GameObject towerThatShot;

    private float damageRad = 0f;
    private float damage;
    private string enemyTag;
    private float turnSpeed = 10f;
    private float distanceToTarget;

	// Update is called once per frame
	void Update () {
        if ( target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - this.transform.position;
        this.transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        this.transform.rotation = Quaternion.Euler(0, rotation.y, 0);

        distanceToTarget = Vector3.Distance(this.transform.position, target.position);

        if (distanceToTarget <= 0.5f)
        {
          BulletHit();
        }
        
    }

    public void SeekTarget( GameObject _target)
    {
        targetGO = _target;
        target = targetGO.transform;
    }

    void BulletHit()
    {
        GameObject hitEff = (GameObject) Instantiate(impactEffect, this.transform.position, this.transform.rotation);
        Destroy(hitEff, 1f);

        if(damageRad == 0)
        {
        targetGO.GetComponent<HitEffects>().TakeDamage(damage);
        }
        else
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);

                if (enemy != null && distanceToEnemy <= damageRad)
                {
                    enemy.GetComponent<HitEffects>().TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }


    // Geters-Seters

    public void setTowerThatShot( GameObject _t)
    {
        towerThatShot = _t;
    }

    public GameObject getTowerThatShot()
    {
        return towerThatShot;
    }

    public void setDamage(float _damage)
    {
        damage = _damage;
    }

    public void setDamageRad(float _r)
    {
        damageRad = _r;
    }

    public void setEnemyTag(string _s)
    {
        enemyTag = _s;
    }

    public float getDistanceToTarget()
    {
        return distanceToTarget;
    }

    public float getDamageRad()
    {
        return damageRad;
    }

    public GameObject getTarget()
    {
        return targetGO;
    }
}
