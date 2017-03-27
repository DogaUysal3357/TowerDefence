using UnityEngine;
using System.Collections;

public class EnemyHealer : MonoBehaviour {

    public float healingRange = 5f;
    public int healingRate = 10;
    public float healingCD = 3f;

    private string enemyTag = "Enemy";
    private float healingCDLeft;
    private GameObject thisGO;

    void Start()
    {
        healingCDLeft = healingCD;
        thisGO = this.gameObject;
    }

    void Update()
    {
        healingCDLeft -= Time.deltaTime;

        if (healingCDLeft <= 0)
        {
            Heal();
            healingCDLeft = healingCD;
        }
    }

    private void Heal()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float distanceToTarget = 0;

        foreach (GameObject e in enemies)
        {
            distanceToTarget = Vector3.Distance(thisGO.transform.position, e.transform.position);
            if(distanceToTarget <= healingRange)
            {
                e.GetComponent<Enemy>().Heal(healingRate);
            }
        }
    }
}
