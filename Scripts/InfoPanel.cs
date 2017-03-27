using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {



    private GameObject tower;
    private TowerAI towerScript;
    private Text[] texts;

    // Use this for initialization
    void Start () {
	 texts = GetComponentsInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        tower = BuildingManager.instance.GetCurrentPanelInfoTower();
        towerScript = tower.GetComponent<TowerAI>();

        updatePanel();
    }



    private void updatePanel() {
        texts[0].text = towerScript.turretName + "(" + tower.GetComponent<UpgradeTower>().getUpgraded().ToString() + ")";
        texts[1].text = "Damage  : " + towerScript.damage.ToString();
        texts[2].text = "Range     : " + towerScript.range.ToString();
        texts[3].text = "AttackCD : " + towerScript.fireCD.ToString("F2");


        if (towerScript.towerType == 0)
        {
            texts[4].text = "Special    : Area damage for " + towerScript.damageRadious.ToString() + " radious";
        }
        if (towerScript.towerType == 1)
        {
            texts[4].text = "Special    : %" + tower.GetComponent<SlowEffectTurret>().getSlowAmount().ToString() + " slow for " + tower.GetComponent<SlowEffectTurret>().getSlowDuration().ToString() + " secs.";
        }
        if (towerScript.towerType == 2)
        {
            texts[4].text = "Special    : " + tower.GetComponent<IgniteEffectTurret>().getIgniteDamage().ToString() + " ignite damage for " + tower.GetComponent<IgniteEffectTurret>().getIgniteDuration().ToString() + " secs.";
        }
        if (towerScript.towerType == 3)
        {
            texts[4].text = "Special    : %" + tower.GetComponent<CurseEffectTurret>().getCursePercent().ToString() + " extra damage debuff for " + tower.GetComponent<CurseEffectTurret>().getCurseDuration().ToString() + " secs.";
        }
        if (towerScript.towerType == 4)
        {
            texts[4].text = " ";
        }
    }


}
