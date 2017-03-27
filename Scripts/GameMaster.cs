using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

    public static GameMaster instance;

    [Header("General Info Panel")]
    public Text moneyText;
    public Text livesText;
    public Text waveNumText;
    public Text levelTimeText;
    public GameObject turretInfoPanel;
    public GameObject lastWavePanel;
    public GameObject endGamePanel;
    public GameObject[] shop;
    public GameObject pausePanel;
    public int enemyHealthDoubler = 2;
    private int statMultiper = 1;

    [Header(" Game/Wave Setup")]
    public int lives = 20;
    public int maxWave = -1; // -1 ise sonsuza kadar 
    public float waveCD = 10f; // 2 wave arası CD
    public bool isPaused = false;
    public int waveCDGrowth = 0;
    public int waveCDGrowthCD = 0;
    public int waveHeal; // only works if maxWave is set to -1

    private float waveCDLeft = 0; // Wave spawnlama sayacı
    private bool waveEnding = false;
    private int waveNum = 0;
    private bool lastWave = false;
    private float levelTime;
    private bool healed = true;
 

    void Start()
    {
        levelTime = 0;
        waveCDLeft = waveCD;
    }
    void Awake ()
    {
        if (instance != null)
        {
            Debug.Log("More than one BuildManager is in scene!");
            return;
        }
        instance = this;
    }
    
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shop[0].activeSelf)
            {
                shop[0].SetActive(false);
                shop[1].SetActive(true);
                BuildingManager.instance.changeShopActity(false);
            }else if (turretInfoPanel.activeSelf)
            {
                BuildingManager.instance.setCurrentSelectedTowerToNull();
                turretInfoPanel.SetActive(false);
            }else if (pausePanel.activeSelf)
            {
                UnpauseGame();
            }else if ( !(pausePanel.activeSelf))
            {
                PauseGame();
            }
        }

        if(!(waveEnding || lives <= 0))
        {
            levelTime += Time.deltaTime;
        }
        UpdateTexts();
        waveCDLeft -= Time.deltaTime;
        // Wave spawnlanırken, waveNumarası arttırılır. WaveSetup'ı yapılıp, spawnlanmaya başlanır.
        UpdateWaves();
    }

    /*
    private void EnemyHealthAndMoneySetter()
    {
        int waveNum = GameMaster.instance.getWaveNum();
        int waveMultiper = GameMaster.instance.enemyHealthDoubler;

        if (waveNum % waveMultiper == 0)
        {
            statMultiper++;
        }
    }
    */


    private void UpdateWaves()
    {
        if(maxWave == -1)
        {
            if(waveNum % waveHeal == 1)
            {
                healed = true;
            }
            if( waveNum != 0 && (waveNum % waveHeal == 0 ) )
            {
                if (healed)
                {
                    GainLife(10);
                    healed = false;
                }
            }
        }
        if (waveCDLeft <= 0)
        {
            if (waveNum != maxWave)
            {
                waveNum++;
                waveCDLeft = waveCD;
                if(waveCDGrowth != 0 && (waveNum % waveCDGrowthCD == 0))
                {
                    waveCD += waveCDGrowth;
                    waveCDLeft = waveCD;
                }
            }
        }
        if (waveEnding)
        {
            EndGame();
        }
    }

    public void GainLife(int h)
    {
        lives += h;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void UpdateTexts()
    {
        livesText.text = lives.ToString();
        moneyText.text = (BuildingManager.instance.getMoney()).ToString();
        waveNumText.text = waveNum.ToString();
        levelTimeText.text = "Level Time : " + levelTime.ToString("F2") + " sec";

        if (lastWave)
        {
            if (lastWavePanel != null)
            {
                lastWavePanel.SetActive(true);
                Destroy(lastWavePanel, 2f);
            }
        }
    }


    public GameObject GetTurretInfoPanel()
    {
        return turretInfoPanel;
    }

   
    public void loseLife(int damage)
    {
        lives -= damage;
        if(lives <= 0 )
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        endGamePanel.SetActive(true);
        turretInfoPanel.SetActive(false);
        for (int i = 0; i < shop.Length; i++)
        {
            if (shop[i] != null)
                shop[i].SetActive(false);
        }

        if (lives>0)
        {
            //TODO: belki lv menude sonraki lv unlocklansın ? 
        }else
        {
           
        }
    }


    // Geters - Seters

    public int getWaveNum()
    {
        return waveNum;
    }
    
    public int getMaxWave()
    {
        return maxWave;
    }

    public void setWaveEnding(bool _b)
    {
        waveEnding = _b;
    }

    public bool getWaveEnding()
    {
        return waveEnding;
    }

    public int getLives()
    {
        return lives;
    }

    public void setLastWave(bool _b)
    {
        lastWave = _b;
    }

    public bool getLastWave()
    {
        return lastWave;
    }
}
