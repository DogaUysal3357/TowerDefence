using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeTurretInfoPanel : MonoBehaviour {

    public Text infoPanel;
    public GameObject currTurret;

	// Update is called once per frame
	void Update () {
        currTurret = BuildingManager.instance.GetCurrentPanelInfoTower();
        infoPanel.text = "Cost : " + currTurret.GetComponent<TowerAI>().GetComponent<UpgradeTower>().getUpgradeCost().ToString();
	}
}
