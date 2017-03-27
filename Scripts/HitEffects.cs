using UnityEngine;
using System.Collections;

public class HitEffects : MonoBehaviour {

    [Header("Type By Color")]
    public int enemyType = 0;
    // 0 > White/Red/Green
    // 1 > Black
    // 2 > Yellow
    [Header("StatusEffects")]
    
    public GameObject igniteEffect;
    public GameObject curseEffect;
    public GameObject slowEffect;

    private bool isSlowed = false;
    private bool isIgnited = false;
    private bool isCursed = false;
    private bool isVanished = false;

    private float vanishCD = 3f;
    private float vanishCDLeft = 0f;
    private float vanishDuration = 2f;
    private float vanishDurationLeft = 0f;

    private float slowDurationLeft = 0;
    private float baseSpeed = 0;

    private float igniteDamage = 0;
    private float igniteDurationLeft = 0;
    private float igniteTickCheck = 0;

    private float cursePercent = 0;
    private float curseDurationLeft = 0;

    private GameObject EnemyGO = null;
    private Enemy EnemyBaseScript = null;

	// Use this for initialization
	void Start () {
        EnemyGO = this.gameObject;
        EnemyBaseScript = EnemyGO.GetComponent<Enemy>();
        baseSpeed = EnemyBaseScript.getSpeed();
        vanishCDLeft = vanishCD;
        vanishDurationLeft = vanishDuration;
    }
	
	// Update is called once per frame
	void Update () {
        if (isVanished)
        {
            vanishDurationLeft -= Time.deltaTime;
            if(vanishDurationLeft <= 0)
            {
                RemoveVanish();
            }
        }
        else
        {
            if (isSlowed)
            {
                if (slowDurationLeft <= 0)
                {
                    RemoveSlow();
                }
                slowDurationLeft -= Time.deltaTime;
            }
            if (isIgnited)
            {
                if (SecondPassed(Time.deltaTime))
                {
                    TakeDamage(igniteDamage);
                    igniteTickCheck = 0;
                }
                if (igniteDurationLeft <= 0)
                {
                    RemoveIgnite();
                }
                igniteDurationLeft -= Time.deltaTime;
            }
            if (isCursed)
            {
                if (curseDurationLeft <= 0)
                {
                    RemoveCurse();
                }
                curseDurationLeft -= Time.deltaTime;
            }

            if(enemyType == 1)
            {
                vanishCDLeft -= Time.deltaTime;
            }
        }
	}

    private bool SecondPassed(float time)
    {
        return ((igniteTickCheck += time) >= 1);
    }


    private bool NeededTimePassed(float wantedTime, float time)
    {
        return ((igniteTickCheck += time) >= wantedTime);
    }


    public void TakeDamage(float damage)
    {
        if (!isVanished)
        {
            damage = DamageAfterCurse(damage);
            if (EnemyBaseScript != null)
            {
                EnemyBaseScript.loseHp(damage);

                if (EnemyBaseScript.getHealth() <= 0)
                {
                    if (enemyType == 2)
                    {
                        GetComponent<EnemyHost>().SpawnWeaklings();
                        EnemyBaseScript.Die();
                    }
                    else
                    {
                        EnemyBaseScript.Die();
                    }
                }
            }
        }
        if(enemyType == 1 && vanishCDLeft<= 0)
        {
            ApplyVanish();
            vanishCDLeft = vanishCD;
        }
    }


    private void ApplyVanish()
    {
        isVanished = true;
        Color vanishedColor = GetComponent<Renderer>().material.color;
        vanishedColor.a = 0f;
        GetComponent<Renderer>().material.color = vanishedColor;
        this.gameObject.tag = "Vanished";
    }

    private void RemoveVanish()
    {
        isVanished = false;
        Color vanishedColor = GetComponent<Renderer>().material.color;
        vanishedColor.a = 255f;
        GetComponent<Renderer>().material.color = vanishedColor;
        this.gameObject.tag = "Enemy";
        vanishDurationLeft = vanishDuration;
    }

    //TODO: Daha düşük bir slow gelirse neler yapılabilir ? 
    public void ApplySlow(float slowAmount, float slowDuration)
    {
        isSlowed = true;
        slowEffect.SetActive(true);
        EnemyBaseScript.setSpeed(baseSpeed * (100-slowAmount) / 100);
        slowDurationLeft = slowDuration;
    }

    public void RemoveSlow()
    {
        isSlowed = false;
        slowEffect.SetActive(false);
        EnemyBaseScript.setSpeed(baseSpeed);
    }

    public void ApplyIgnite(float igniteDuration, float igniteDamage)
    {
        isIgnited = true;
        igniteEffect.SetActive(true);
        this.igniteDamage = igniteDamage;
        igniteDurationLeft = igniteDuration;
        igniteTickCheck = 0;
    }

    public void RemoveIgnite()
    {
        isIgnited = false;
        igniteEffect.SetActive(false);
        igniteTickCheck = 0;
    }

    public void ApplyCurse(float cursePercent, float curseDuration)
    {
        isCursed = true;
        curseEffect.SetActive(true);
        this.cursePercent = cursePercent;
        curseDurationLeft = curseDuration;

    }

    public void RemoveCurse()
    {
        isCursed = false;
        curseEffect.SetActive(false);
        cursePercent = 0;
    }

    private float DamageAfterCurse(float damage)
    {
        if (cursePercent == 0)
        {
            return damage;
        }
        else
        {
            return (damage + damage * cursePercent / 100);
        }
    }


}
