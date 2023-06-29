using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class UpgradeButtonBehaviorScript : SaveableData
{
    float[] tierBase = new float[] { 10, 20, 50, 200 };

    public string Name;
    public int tier = 1;

    public EnumBonusType bonusType;
    public float BonusPercentPerLevel = 10;
    public int maxLevel = 5;
    public int level = 0;
    public TextMeshProUGUI toolTipText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI totalBonusText;
    public GameObject icon;
    public Sprite ButtonDown;
    public Sprite ButtonUp;
    public Sprite ButtonDone;
    public enum EnumBonusType { MOVEMENT_SPEED, ATTACK_SPEED, ATTACK_POWER, CLICK_DAMAGE, PASSIVE_DAMAGE}




    string bonusTypeEnumToString(EnumBonusType b)
    {
        switch(b)
        { 
        case EnumBonusType.MOVEMENT_SPEED:
            return "Movement Speed";
        case EnumBonusType.ATTACK_SPEED:
            return "Attack Speed";
        case EnumBonusType.ATTACK_POWER:
            return "Attack Damage";
        case EnumBonusType.CLICK_DAMAGE:
            return "Click Damage";
         case EnumBonusType.PASSIVE_DAMAGE:
            return "Passive Damage";
        }
        return "";
    }


    public override void  Start()
    {
        base.Start();
        if (level < maxLevel)
            this.gameObject.GetComponent<SpriteRenderer>().sprite = ButtonUp;
        else
            this.gameObject.GetComponent<SpriteRenderer>().sprite = ButtonDone;

    }

    // Update is called once per frame
    void Update()
    {
        nameText.text = Name;
        costText.text = "Cost: " + getUpgradeCost().ToString();
        levelText.text = "Lvl " + level.ToString() + "/" + maxLevel.ToString();

        int b = (int)(BonusPercentPerLevel);
        toolTipText.text = "Increase " + bonusTypeEnumToString(bonusType) + "  by " + b.ToString() + "%";

        totalBonusText.text = "+ " + getBonus().ToString() + "%";

    }

    public float getBonus()
    {
        return BonusPercentPerLevel * level;
    }

   public float getUpgradeCost()
    {
        float exp = 1.0f / 0.75f;
        float mult = 1f + (getBonus() / 100) + (BonusPercentPerLevel / 100f);
        float cost = Mathf.Pow(    (1f / 3f)  * tierBase[tier-1] * mult,  exp);
        return cost;
    }

    private void OnMouseUp()
    {
        if (level < maxLevel)
            this.gameObject.GetComponent<SpriteRenderer>().sprite = ButtonUp;
        else
            this.gameObject.GetComponent<SpriteRenderer>().sprite = ButtonDone;
    }
    private void OnMouseDown()
    {
        float cost = getUpgradeCost();
        if (CombatManager.managerRef.gold >= cost && level < maxLevel)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = ButtonDown;

            CombatManager.managerRef.gold -= cost;
            
            level++;
        }
    }
}
