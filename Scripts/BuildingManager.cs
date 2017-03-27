using UnityEngine;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager instance;

    public GameObject MGTurret;
    public int money = 20;
    public int raiseMoney = 100;
    private int raiseLimit = 4;

    private GameObject turretToBuild;
    private bool isShopActive = false;
    private GameObject currentSelectedPanelInfoTower = null;
    private GameObject currentSelectedTowerNode = null;

    // Use this for initialization
    void Start()
    {
        turretToBuild = MGTurret;
    }
    
    void Awake()
    {
        if(instance != null)
        {
            Debug.Log("More than one BuildManager is in scene!");
            return;
        }

        instance = this;
    }

    public void changeShopActity(bool status)
    {
        isShopActive = status;
    }

    public void purchase(int cost)
    {
        money -= cost;
    }

    public void EarnGold(int gold)
    {
        money += gold;
    }

    public void DestroySelectedTower()
    {
        GameMaster.instance.GetTurretInfoPanel().SetActive(false);
        EarnGold((currentSelectedPanelInfoTower.GetComponent<TowerAI>().cost / 2));
        Destroy(currentSelectedPanelInfoTower.gameObject);
        setCurrentSelectedTowerToNull();
    }

    public void RaisePlatform()
    {
        if (getMoney() >= raiseMoney)
        {
            if(getCurrentTowerNode().GetComponent<Node>().getTimesRaised() <= raiseLimit)
            {
                getCurrentTowerNode().GetComponent<Node>().raiseTimesRaised();
                purchase(raiseMoney);
                Vector3 dir = new Vector3(0, 2, 0);
                currentSelectedPanelInfoTower.transform.position += dir;
                currentSelectedTowerNode.transform.position += dir;  
            }
        }
    }

    public void UpgradeCurrentSelectedTower()
    {
        GetCurrentPanelInfoTower().GetComponent<UpgradeTower>().Upgrade();
    }


    public void changeTurretAggro(int aggro)
    {
        currentSelectedPanelInfoTower.GetComponent<TowerAI>().setAggro(aggro);
    }
    
    
    // Geters - Seters 

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public int getMoney()
    {
        return money;
    }

    public void setTurret(GameObject turret)
    {
        turretToBuild = turret;
    }

    public bool getShopActivity()
    {
        return isShopActive;
    }

    public void setCurrentPanelInfoTower( GameObject tower)
    {
        currentSelectedPanelInfoTower = tower;
    }

    public GameObject GetCurrentPanelInfoTower()
    {
        return currentSelectedPanelInfoTower;
    }

    public void setCurrentSelectedTowerToNull()
    {
        currentSelectedPanelInfoTower = null;
    }

    public void setCurrentTowerNode( GameObject node)
    {
        currentSelectedTowerNode = node;
    }

    public GameObject getCurrentTowerNode()
    {
        return currentSelectedTowerNode;
    }

    
}
