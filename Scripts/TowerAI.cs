using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class TowerAI : MonoBehaviour {

    [Header("Attributes")]
    public float range = 5f;
    public float fireCD = 0.2f;
    public int damage = 2;
    public float damageRadious = 0f;
    public int cost = 0;
    public int towerType = 0;
    // 0 -> RPG
    // 1 -> Icey
    // 2 -> Flame
    // 3 -> Eye
    // 4 -> MG


    // TODO: Info Panel'i Update'de güncellensin
    [Header("Setup")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public string enemyTag =  "Enemy";
    
    public string turretName;

    [Header("UpgradeModels")]
    public GameObject standartTransform;
    public GameObject firstUpgradeTransform;
    public GameObject secondUpgradeTransform;
    public GameObject partToRotate;



    private int transformChangeCount = 0;
    private float turnSpeed = 10f;
    private int aggroType = 0;
    private float fireCDLeft = 0;
    private GameObject target;
    private GameObject infoUI;
    private BuildingManager buildingManager;
    private GameObject towerNode;

    // Use this for initialization
    void Start () {
        buildingManager = BuildingManager.instance;
        infoUI = GameMaster.instance.GetTurretInfoPanel();
        InvokeRepeating("SelectTargetViaAggro", 0f, 0.1f);
	}

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            return;
        }

        Vector3 dir = target.transform.position - partToRotate.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.transform.rotation = Quaternion.Euler(0, rotation.y, 0);
        
        if (fireCDLeft <= 0)
        {
            Shoot();
            fireCDLeft = fireCD;
        }

        fireCDLeft -= Time.deltaTime;
    }

    public void UpgradeTransform()
    {
        if(transformChangeCount == 0)
        {  
            standartTransform.SetActive(false);
            firstUpgradeTransform.SetActive(true);
        }
        if(transformChangeCount == 1)
        {
            firstUpgradeTransform.SetActive(false);
            secondUpgradeTransform.SetActive(true);
        }
        transformChangeCount++;
    }



    private void SelectTargetViaAggro()
    {
        float currSpeed;
        float currHp;
        float shortestDist = Mathf.Infinity;
        float maxHp = Mathf.Infinity;
        float minHp = 0;
        float minSpeed = 0;
        float maxSpeed = Mathf.Infinity;

        float distanceToEnemy;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        GameObject targetEnemy = null;


        if (target != null)
        {
            distanceToEnemy = Vector3.Distance(this.transform.position, target.transform.position);
            if(distanceToEnemy > range)
            {
                target = null;
            }
        }


        switch (aggroType)
        {
            case 0: // Range içine ilk girenler için Aggro
                foreach (GameObject enemy in enemies)
                {
                    distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);
                    if (distanceToEnemy < shortestDist)
                    {
                        targetEnemy = enemy;
                        shortestDist = distanceToEnemy;
                    }

                    if (targetEnemy != null && shortestDist <= range)
                    {
                        target = targetEnemy;
                    }
                }
                break;

            case 1: // En hizli olanlar icin aggro
                currSpeed = minSpeed;
                foreach (GameObject enemy in enemies)
                {
                    distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);
                    if (distanceToEnemy <= range)
                    {
                        if (enemy.GetComponent<Enemy>().getSpeed() > currSpeed)
                        {
                            currSpeed = enemy.GetComponent<Enemy>().getSpeed();
                            targetEnemy = enemy;
                        }
                    }
                }

                if (targetEnemy != null)
                {
                    target = targetEnemy;
                }
                break;

            case 2: // En yavas olanlar icin aggro
                currSpeed = maxSpeed;
                foreach (GameObject enemy in enemies)
                {
                    distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);
                    if (distanceToEnemy <= range)
                    {
                        if (enemy.GetComponent<Enemy>().getSpeed() < currSpeed)
                        {
                            currSpeed = enemy.GetComponent<Enemy>().getSpeed();
                            targetEnemy = enemy;
                        }
                    }
                }

                if (targetEnemy != null)
                {
                    target = targetEnemy;
                }
                break;

            case 3: // Hpsi en fazla olanlar icin aggro
                currHp = minHp;
                foreach (GameObject enemy in enemies)
                {
                    distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);
                    if (distanceToEnemy <= range)
                    {
                        if (enemy.GetComponent<Enemy>().getHealth() > currHp)
                        {
                            currHp = enemy.GetComponent<Enemy>().getHealth();
                            targetEnemy = enemy;
                        }
                    }
                }

                if (targetEnemy != null)
                {
                    target = targetEnemy;
                }
                break;

            case 4: // Hpsi en az olanlar icin aggro
                currHp = maxHp;
                foreach (GameObject enemy in enemies)
                {
                    distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);
                    if (distanceToEnemy <= range)
                    {
                        if (enemy.GetComponent<Enemy>().getHealth() < currHp)
                        {
                            currHp = enemy.GetComponent<Enemy>().getHealth();
                            targetEnemy = enemy;
                        }
                    }
                }

                if (targetEnemy != null)
                {
                    target = targetEnemy;
                }
                break;
        }


       


    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null)
        {
            bullet.setTowerThatShot(this.gameObject);
            bullet.SeekTarget(target);
            bullet.setDamage(damage);
            bullet.setDamageRad(damageRadious);
            bullet.setEnemyTag(enemyTag);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, range);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        buildingManager.setCurrentPanelInfoTower(this.gameObject);
        buildingManager.setCurrentTowerNode(getTowerNode());
        infoUI.SetActive(true);        
    }

    // Geter - Seter

    public void setTowerNode(GameObject node)
    {
        towerNode = node;
    }

    public GameObject getTowerNode()
    {
        return towerNode;
    }

    public void setAggro(int _a)
    {
        aggroType = _a;
    }

    public int getAggro()
    {
       return aggroType;
    }


}
