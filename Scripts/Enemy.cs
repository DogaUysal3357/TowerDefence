using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Attributes")]
    public float health = 25f;
    public float speed = 5f;
    public int gold = 10;

    [Header("Others")]
    public float rotationSpeed = 50f;
    public int coreDamage = 1;    
    public string path;

    private Transform target;
    private int waypointIndex = 0;
    private float maxHp;
    GameObject pathGO;
    private int statMultiper = 1;

    //CAUTION!
    void Awake()
    {
        pathGO = GameObject.Find(path);
        maxHp = health;
    }

    void Start()
    {
        pathGO = GameObject.Find(path);
        maxHp = health;
    }    

    void Update()
    {
        if ( target == null)
        {
            GetNextWaypoint();
            if(target == null)
            {
                ReachedGoal();
                return;
            }
        }

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        if (Vector3.Distance(transform.position , target.position) <= 0.5f)
        {
            GetNextWaypoint();
        }
        
    }


    void GetNextWaypoint()
    {
        if(waypointIndex < pathGO.transform.childCount)
        {
            target = pathGO.transform.GetChild(waypointIndex);
            waypointIndex++;
        }
        else
        {
            ReachedGoal();
        }
        
    }

    void ReachedGoal()
    {
        GameMaster.instance.loseLife(coreDamage);
        BuildingManager.instance.EarnGold(gold);
        Destroy(gameObject);
    }

    public void Die()
    {
        Destroy(gameObject);
        BuildingManager.instance.EarnGold(gold);
    }
    
    public void setPath(string _p)
    {
        path = _p;
    }

    public void loseHp(float damage)
    {
        health -= damage;
    }

    public void Heal(float _h)
    {
        health += maxHp*(_h/100);
        if (health > maxHp)
        {
            health = maxHp;
        }
    }

    //Geters - Seters
    
    public int getWayPointIndex()
    {
        return waypointIndex;
    }

    public void setWayPointIndex(int _wp)
    {
        waypointIndex = _wp;
    }

    public float getHealth()
    {
        return health;
    }

    public float getMaxHP()
    {
        return maxHp;
    }

    public float getSpeed()
    {
        return speed;
    }

    public int getCoreDamage()
    {
        return coreDamage;
    }
    
    public void setSpeed(float _s)
    {
        speed = _s;
    }

}
 