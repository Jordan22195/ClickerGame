using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class UpgradeMenuBehaviorScript : MonoBehaviour
{

    public List<UpgradeButtonBehaviorScript> upgrades;

    public double getUpgradeLinearIncrease(UpgradeButtonBehaviorScript.EnumBonusType upgradeType)
    {
        double increase = 0;
        foreach (UpgradeButtonBehaviorScript u in upgrades)
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
        foreach (UpgradeButtonBehaviorScript u in upgrades)
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
    }

    // Update is called once per frame
    void Update()
    {
     }

    public void ToJSON()
    {
        foreach (UpgradeButtonBehaviorScript u in upgrades)
        {
            u.ToJSON();
        }
            Debug.Log("ToJSON");
        string s = JsonUtility.ToJson(this);
        Debug.Log(s);
        //return s;
    }

}
