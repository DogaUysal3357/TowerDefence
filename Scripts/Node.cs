using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

    public Color hoverColor; //00CAFF00 
    public Color selectedColor; //EA0A0A00

    // Y = 5 en iyi sonuc
    public Vector3 offset;

    private Renderer rend;
    private Color originalColor;

    private GameObject turret;
    private TowerAI turretScrpt;
    private int timesRaised =  0;


	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }
	
    void Update()
    {
        if(turret != null)
        {
             if (turret == BuildingManager.instance.GetCurrentPanelInfoTower())
            {
                rend.material.color = selectedColor;
            }else
            {
                rend.material.color = originalColor;
            }
        }
    }


    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if ( turret != null)
        {
            return;
        }

        if (BuildingManager.instance.getShopActivity())
        {
            GameObject turretToBuild = BuildingManager.instance.GetTurretToBuild();
            turretScrpt = turretToBuild.GetComponent<TowerAI>();
            if ( BuildingManager.instance.getMoney() >= turretScrpt.cost)
            {
                turret = (GameObject)Instantiate(turretToBuild, this.transform.position + offset, this.transform.rotation);
                turret.GetComponent<TowerAI>().setTowerNode(this.gameObject);
                BuildingManager.instance.purchase(turretScrpt.cost);
            }
        }
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (BuildingManager.instance.getShopActivity())
        {
            rend.material.color = hoverColor;
        }
    }

    void OnMouseExit()
    {
        rend.material.color = originalColor;
    }

    public void DestroySelectedTower()
    {
        if (turret != null)
        {
            BuildingManager.instance.EarnGold((turretScrpt.cost / 2));
            Destroy(turret);
        }
    }

    public int getTimesRaised()
    {
        return timesRaised;
    }

    public void raiseTimesRaised()
    {
        timesRaised++;
    }

}
