using UnityEngine;
using System.Collections;

public class EnemyHost : MonoBehaviour {

    public int numOfSpawns = 0;
    public GameObject mobToSpawn = null;

    public void SpawnWeaklings()
    {
        for (int i = 0; i < numOfSpawns; i++)
        {
            GameObject spawned = (GameObject) Instantiate(mobToSpawn, this.gameObject.transform.position, this.gameObject.transform.rotation);
            spawned.GetComponent<Enemy>().setPath(this.gameObject.GetComponent<Enemy>().path);
            spawned.GetComponent<Enemy>().setWayPointIndex( this.gameObject.GetComponent<Enemy>().getWayPointIndex() -1 );
        }
    }
}
