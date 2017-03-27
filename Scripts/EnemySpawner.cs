using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

   
    [Header("General Setup")]
    public float spawnDelay = 1f; // spawnlanan moblar arası sn
    public int numberOfPaths = 1; // kaç farklı yol oldugu
    public Transform startTransform; // yaratıkların spawnlacagı noktanın transformu
    public int baseWaveSpawnRNGMin = 1; // Temel Spawn RNG min degeri
    public int baseWaveSpawnRNGMax = 5; // Tmemel Spawn RNG max degeri
    public int diffLength = 5; // Kaç wave'de bir oyunun zorlaşacağı
    public int diffGrowth = 2; // zorlaşma katsayısı

    [Header("Tier Setup")]
    // If tierStartIndex is 0, one straight chaos gameplay
    public int[] tierEndIndex; // Seçili zorluk seviyesinin kaçıncı düşmana kadar alacağını gösterir. düşman listesi -> enemyList
    public int[] tierWaveStart; // Zorluk seviyelerinin kaçıncı wave'de başladığının arrayı

    [Header("Spawnable Mob List")]
    public GameObject[] enemyList; // spawnlanabilir düşman listesi
    

    private float waveNum = 0;
    private int currTier = -1; //Şu anda kaçıncı zorluk seviyesinde olunduğu
    private GameMaster gameMaster;
    private bool allSpawned = false;
    private bool allDestroyed = false;
    private GameObject[] spawnedMobs;


    // Spawn CD hazırlanır ve RNG doğru çalışması için ayarlanır
    void Start () {
        gameMaster = GameMaster.instance;
        baseWaveSpawnRNGMax++;
        spawnedMobs = null;
    }

    void Update () {
        // Wave spawnlanırken, waveNumarası arttırılır. WaveSetup'ı yapılıp, spawnlanmaya başlanır.
        
        if(waveNum < gameMaster.getWaveNum())
        {
            waveNum++;
            
            if (waveNum != gameMaster.getMaxWave())
            {
                checkWaves();
                StartCoroutine(SpawnWave());
            }
            else
            {
                gameMaster.setLastWave(true);

                checkWaves();
                StartCoroutine(SpawnWave());

                waveNum = Mathf.Infinity;
            }
        }
        if(gameMaster.getLastWave() && allSpawned)
        {
            checkWaveEnding();
        }
    }

    // WaveSetup'ı yapılır.
    // Her wave'de zorluk seviyesi değişiyor mu diye kontrol yapılır. Eğer değişiyorsa, zorluk seviyesi ( currTier ) 1 arttırılır.
    // Ardından wave'de spawnlanacak yarıtık sayısı arttırılacak mı diye kontrol edilip gerekirse artış yapılır.
    /* enemyList'te spawnlanacak mob listesi var.
     * tierWaveStart'ta kaçıncı wave'lerde currTier atlanacak onun bilgisi saklı
     * tierEndIndex'te de zorluk seviyelerinde, enemyList'ten kaçıncı index'e kadar spawn izni var o saklı.
     * 
     * Mesela tierEndIndex = { 3,5,7}
     *        tierWaveStart = { 1,5,10} olsun. Dipnot yazdığım sistemde tierWaveStart'ın ilk elemanı her zaman olmalı ve 1 olmalı.
     * 
     *  1. wave'de tierWaveStart'a bakılıyor, zorluk seviyesi artıcak. currTier -1'den 0 oluyor. Böyle olunca tierEndIndex[currTier] bakılıyor, oda üç.
     *  Yani enemyList'teki 0-1-2-3 indexlerindeki mobların spawnına izin var 
     */
    private void checkWaves()
    {
        for (int i = 0; i < tierWaveStart.Length; i++)
        {
            if (tierWaveStart[i] == gameMaster.getWaveNum() )
            {
                currTier++;
                break;
            }
        }

        if(gameMaster.getWaveNum() % diffLength == 0)
        {
            int increaseRNG = Random.Range(0, 3);
            if(increaseRNG == 0 )
            {
                baseWaveSpawnRNGMax += diffGrowth;
            }
            else if( increaseRNG == 1)
            {
                baseWaveSpawnRNGMin += diffGrowth;
            }
            else if ( increaseRNG == 2)
            {
                baseWaveSpawnRNGMax += diffGrowth;
                baseWaveSpawnRNGMin += diffGrowth;     
            }
           
        }
    }

    // Gidilecek yol sayısı alınarak spawnlanacak moblara bir path seçilir.
    // NOT: Bunun çalışabilmesi için. Oyun içindeki tüm waypointlerden oluşan yolların,
    // "Path 1" "Path 2" gibi parentlar altında toplanması gerekir.
    private string TakeAPath()
    {
        string path = "Path";
        //  int pathNum;
        //  pathNum = Random.Range(1, numberOfPaths+1);
        //   return (path + pathNum);
        return (path + numberOfPaths);
    }

    // Verilen mob spawnlanarak hangi yolu takip edeceğinin bilgisi yüklenir.
    private GameObject SpawnMob ( GameObject mobToSpawn)
    {
        GameObject mob;

        mob = (GameObject)Instantiate(mobToSpawn, startTransform.position, startTransform.rotation);
        mob.GetComponent<Enemy>().setPath(TakeAPath());

        return mob;
    }

    // Temel Spawn RNG'leri arasından rastgele bir rakam seçilerek wave uzunluğu belirlenir.
    // Ardından o anki zorluk seviyesine göre bir mob index'i seçilir ve spawn edilir.

    IEnumerator SpawnWave()
    {
        int enemyIndex = 0; //enemyList'ten hangi mob'un spawnlanacağının index'i
        int waveLength = 0; //Spawnlanacak wave'de kaç yaratık bulunacağı
       
        waveLength = Random.Range(baseWaveSpawnRNGMin, baseWaveSpawnRNGMax);

        spawnedMobs = new GameObject[waveLength];

        for (int i = 0; i < waveLength; i++)
        {
            if (tierEndIndex.Length != 0)
            {
                enemyIndex = Random.Range(0, tierEndIndex[currTier]);
            }
            else
            {
                enemyIndex = enemyList.Length;
            }

            if (!gameMaster.getLastWave())
            {
                SpawnMob(enemyList[enemyIndex]);
            }else
            {
                spawnedMobs[i] = SpawnMob(enemyList[enemyIndex]);
            }
            yield return new WaitForSeconds(spawnDelay);
        }

        if (gameMaster.getLastWave())
        {
            allSpawned = true;
        }
    }
    

    private void checkWaveEnding()
    {
        allDestroyed = true;
        for (int i = 0; i < spawnedMobs.Length; i++)
        {
            if(spawnedMobs[i] != null)
            {
                allDestroyed = false;
            }
        }

        if (allDestroyed)
        {
            gameMaster.setWaveEnding(true);
        }        
    }


}
