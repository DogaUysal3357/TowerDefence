using UnityEngine;
using System.Collections;

public class UpgradeTower : MonoBehaviour {

    public int upgradeCostGrow = 50;

    private BuildingManager buildingManager;
    private GameObject tower;
    private int upgradeCost;
    private int upgraded;
    private int towerType;
    private TowerAI towerAIScript;


    void Start()
    {
        tower = this.gameObject;
        towerAIScript = tower.GetComponent<TowerAI>();
        buildingManager = BuildingManager.instance;
        upgraded = 0;
        towerType = tower.GetComponent<TowerAI>().towerType;
        upgradeCost = upgradeCostGrow; 
    }

    void Update()
    {
        controlStats();
    }

    public void Upgrade()
    {
        if (EnoughMoney(upgradeCost))
        {
            upgraded++;
            GeneralUpgrade();
            if (upgraded == 5)
            {
                towerAIScript.UpgradeTransform();
                FirstCoreUpgrade();
            }
            if (upgraded == 10)
            {
                towerAIScript.UpgradeTransform();
                SecondCoreUpgrade();
            }
        }
        controlStats();
    }

    private void GeneralUpgrade()
    {
        buildingManager.purchase(upgradeCost);
        upgradeCost += upgradeCostGrow;

        int baseRNG = Random.Range(1, 11);
        int luckRNG = Random.Range(1, 11);
        // TODO: RNG'ler farklı mı diye kontrol et

        // RPG
        if (towerType == 0)
        {
            if(baseRNG <= 2)
            {
                if(luckRNG >= 8)
                {
                    towerAIScript.range += 0.5f;
                }
                towerAIScript.range += 0.1f;
            }else if(baseRNG <= 4)
            {
                if (luckRNG >= 8)
                {
                    towerAIScript.fireCD -= 0.15f;
                }
                towerAIScript.fireCD -= 0.05f;
            }else if(baseRNG <= 8)
            {
                if (luckRNG >= 8)
                {
                    towerAIScript.damage += 15;
                }
                towerAIScript.damage += 5;
            }else if (baseRNG <= 10)
            {
                if (luckRNG >= 8)
                {
                    towerAIScript.damageRadious += 0.75f;
                }
                towerAIScript.damageRadious += 0.25f;
            }
        }


        // IceTower
        if(towerType == 1)
        {
            if(baseRNG <= 2)
            {
                if(luckRNG >= 8)
                {
                    towerAIScript.range += 0.5f;
                }
                towerAIScript.range += 0.1f;
            }
            if( ( baseRNG > 2 && baseRNG <= 4 ) || baseRNG > 8)
            {
                if(luckRNG >= 8)
                {
                    towerAIScript.fireCD -= 0.15f;
                }
                towerAIScript.fireCD -= 0.05f;
            }
            if(baseRNG > 4 ||baseRNG <= 6)
            {
                if(luckRNG >= 8)
                {
                    towerAIScript.damage += 10;
                }
                towerAIScript.damage += 5;
            }
            if(baseRNG > 6 || baseRNG <= 8)
            {
                if(luckRNG >= 8)
                {
                    towerAIScript.damageRadious += 0.25f;
                }
                towerAIScript.damageRadious += 0.15f;
            }
            if(baseRNG >= 8 && luckRNG >= 8)
            {
                tower.GetComponent<SlowEffectTurret>().slowAmount += 5;
            }
            if(baseRNG == 10 && luckRNG >= 8)
            {
                tower.GetComponent<SlowEffectTurret>().slowDuration += 1;
            }

        }


        // FlameTower
        if(towerType == 2)
        {
            if(baseRNG <= 2)
            {
                if(luckRNG >= 8)
                {
                    towerAIScript.range += 0.1f;
                }
                towerAIScript.range += 0.05f;
            }
            else if (baseRNG <= 4)
            {
                if (luckRNG >= 8)
                {
                    towerAIScript.fireCD -= 0.2f;
                }
                towerAIScript.fireCD -= 0.05f;
            }
            else if (baseRNG <= 6)
            {
                if (luckRNG >= 8)
                {
                    towerAIScript.damage += 10;
                }
                towerAIScript.damage += 5;
            }
            if (baseRNG >= 6 && luckRNG <= 6)
            {
                towerAIScript.damageRadious += 0.5f;
            }
            if(baseRNG <= 8)
            {
                if(luckRNG >= 8)
                {
                    tower.GetComponent<IgniteEffectTurret>().igniteDamage += 5;
                }
                tower.GetComponent<IgniteEffectTurret>().igniteDamage += 2;
            }
            if(baseRNG >= 8 && luckRNG >= 8)
            {
                tower.GetComponent<IgniteEffectTurret>().igniteDuration += 1;
            } 
        }


        // EyeTower
        if(towerType == 3)
        {
            if(baseRNG <= 2)
            {
                if(luckRNG >= 8)
                {
                    towerAIScript.range += 0.2f;
                }
                towerAIScript.range += 0.1f;
            }
            else if (baseRNG <= 4 )
            {
                if(luckRNG >= 8)
                {
                    towerAIScript.fireCD -= 0.1f;
                }
                towerAIScript.fireCD -= 0.05f;
            }else if( baseRNG <= 6)
            {
                if(luckRNG >= 8)
                {
                    towerAIScript.damage += 10;
                }
                towerAIScript.damage += 5;
            }
            else if ( baseRNG <= 8 && luckRNG >= 8)
            {
                towerAIScript.damageRadious += 0.1f;
            } 
            if(baseRNG > 6)
            {
                if(luckRNG >= 8)
                {
                    tower.GetComponent<CurseEffectTurret>().cursePercent += 5;
                }
                tower.GetComponent<CurseEffectTurret>().cursePercent += 2;
            }
            if(baseRNG >=8 && luckRNG >= 8)
            {
                tower.GetComponent<CurseEffectTurret>().curseDuration += 1;
            }
        }


        //MGTower
        if(towerType == 4)
        {
            if(baseRNG <= 2)
            {
                if(luckRNG >= 8)
                {
                    towerAIScript.range += 0.5f;
                }
                towerAIScript.range += 0.15f;
            }
            else if(baseRNG <= 4)
            {
                if(luckRNG >= 8)
                {
                    towerAIScript.fireCD -= 0.1f;
                }
                towerAIScript.fireCD -= 0.05f;
            }
            else {
                if(luckRNG >= 6)
                {
                    towerAIScript.damage += 15;
                }
                towerAIScript.damage += 5;
            }
        }

        controlStats();
    }

    // 0 -> RPG
    // 1 -> Ice
    // 2 -> Flame
    // 3 -> Eye
    // 4 -> MG
    private void FirstCoreUpgrade()
    {
        if(towerType == 0)
        {
            towerAIScript.range += 6f;
            towerAIScript.fireCD -= 0.5f;
            towerAIScript.damage += 10;
            towerAIScript.damageRadious += 2f;
        }
        if(towerType == 1)
        {
            towerAIScript.range += 3;
            towerAIScript.fireCD -= 2;
            towerAIScript.damage += 15;
            towerAIScript.damageRadious += 2;
            tower.GetComponent<SlowEffectTurret>().slowAmount += 15;
            tower.GetComponent<SlowEffectTurret>().slowDuration += 2;
        }
        if(towerType == 2)
        {
            towerAIScript.range += 2;
            towerAIScript.damage += 20;
            tower.GetComponent<IgniteEffectTurret>().igniteDamage += 10;
            tower.GetComponent<IgniteEffectTurret>().igniteDuration += 1;
        }
        if(towerType == 3)
        {
            towerAIScript.range += 3;
            towerAIScript.fireCD -= 0.5f;
            tower.GetComponent<CurseEffectTurret>().cursePercent += 15;
            tower.GetComponent<CurseEffectTurret>().curseDuration += 1;
        }
        if(towerType == 4)
        {
            towerAIScript.range += 2f;
            towerAIScript.fireCD -= 0.2f;
            towerAIScript.damage += 20;
        }
    }

    private void SecondCoreUpgrade()
    {
        if (towerType == 0)
        {
            towerAIScript.range += 7f;
            towerAIScript.fireCD -= 1f;
            towerAIScript.damage += 50;
            towerAIScript.damageRadious += 3f;
        }
        if (towerType == 1)
        {
            towerAIScript.range += 5;
            towerAIScript.fireCD -= 2;
            towerAIScript.damage += 30;
            towerAIScript.damageRadious += 3;
            tower.GetComponent<SlowEffectTurret>().slowAmount += 25;
            tower.GetComponent<SlowEffectTurret>().slowDuration += 4;
        }
        if (towerType == 2)
        {
            towerAIScript.range += 2;
            towerAIScript.fireCD -= 1;
            towerAIScript.damage += 10;
            tower.GetComponent<IgniteEffectTurret>().igniteDamage += 20;
            tower.GetComponent<IgniteEffectTurret>().igniteDuration += 1;
        }
        if (towerType == 3)
        {
            towerAIScript.range += 5;
            towerAIScript.fireCD -= 0.5f;
            tower.GetComponent<CurseEffectTurret>().cursePercent += 25;
            tower.GetComponent<CurseEffectTurret>().curseDuration += 1;
        }
        if (towerType == 4)
        {
            towerAIScript.range += 3f;
            towerAIScript.fireCD -= 0.3f;
            towerAIScript.damage += 70;
        }
    }

    private bool EnoughMoney(int cost)
    {
        return (buildingManager.getMoney() >= cost);
    }

    private void controlStats()
    {
        if(towerAIScript.fireCD < 0.2f && towerType == 4)
        {
            towerAIScript.fireCD = 0.2f;
        }
        if(towerAIScript.fireCD < 0.5f && towerType !=4)
        {
            towerAIScript.fireCD = 0.5f;
        }
        if(towerAIScript.damageRadious > towerAIScript.range)
        {
            towerAIScript.damageRadious = towerAIScript.range;
        }
        if(towerType == 1)
        {
            if(tower.GetComponent<SlowEffectTurret>().slowAmount >= 100)
            {
                tower.GetComponent<SlowEffectTurret>().slowAmount = 100;
            }
        }
    }

    public int getUpgraded()
    {
        return upgraded;
    }

    public int getUpgradeCost()
    {
        return upgradeCost;
    }
}
