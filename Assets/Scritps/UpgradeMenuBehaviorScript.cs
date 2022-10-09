using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UpgradeMenuBehaviorScript : MonoBehaviour
{

    public List<GameObject> upgrades;

    public double getUpgradeLinearIncrease(UpgradeButtonBehaviorScript.EnumBonusType upgradeType)
    {
        double increase = 0;
        foreach (GameObject u in upgrades)
        {
            UpgradeButtonBehaviorScript upgrade = u.GetComponent<UpgradeButtonBehaviorScript>();
            if (upgrade.bonusType == upgradeType && upgrade.bonusScaleType == UpgradeButtonBehaviorScript.EnumScaleType.LINEAR)
            {
                increase += u.GetComponent<UpgradeButtonBehaviorScript>().getBonus();
            }

        }
        return increase;
    }

    public double getUpgradeMultiplier(UpgradeButtonBehaviorScript.EnumBonusType upgradeType)
    {
        double mult = 1;
        foreach (GameObject u in upgrades)
        {
            UpgradeButtonBehaviorScript upgrade = u.GetComponent<UpgradeButtonBehaviorScript>();
            if (upgrade.bonusType == upgradeType && upgrade.bonusScaleType == UpgradeButtonBehaviorScript.EnumScaleType.EXPONENTIAL)
            {
                mult *= (1 + (u.GetComponent<UpgradeButtonBehaviorScript>().getBonus()/100));
            }
            
        }
        return mult;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        CombatManager.characterStatUpgrades = this;
    }

    // Update is called once per frame
    void Update()
    {
     }

}
