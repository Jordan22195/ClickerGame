using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpgradeButtonBehaviorScript : MonoBehaviour
{



    public string Name;
    public EnumBonusType bonusType;
    public EnumScaleType bonusScaleType;
    public float BonusPerLevel = 1;
    public int maxLevel = 5;
    public EnumScaleType costScaleType;
    public int cost = 100;
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
    public enum EnumScaleType { LINEAR, EXPONENTIAL};
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

    // Linear: static cost the price should increase per level. Exponential: Percent the cost should increase per level
    public double costScaleFactor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nameText.text = Name;
        costText.text = "Cost: " + cost.ToString();
        levelText.text = "Lvl " + level.ToString() + "/" + maxLevel.ToString();

        int b = (int)(BonusPerLevel);
        if(bonusScaleType == EnumScaleType.EXPONENTIAL)
            toolTipText.text = "Increase " + bonusTypeEnumToString(bonusType) + "  by " + b.ToString() + "%";
        else
            toolTipText.text = "Increase " + bonusTypeEnumToString(bonusType) + "  by " + b.ToString();



        if (bonusScaleType == EnumScaleType.EXPONENTIAL)
            totalBonusText.text = "+ " + getBonus().ToString() + "%";
        else
            totalBonusText.text = "+ " + getBonus().ToString();

    }

    public double getBonus()
    {
        return BonusPerLevel * level;
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
        if (CombatManager.managerRef.gold >= cost && level < maxLevel)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = ButtonDown;

            CombatManager.managerRef.gold -= cost;
            if (costScaleType == EnumScaleType.LINEAR)
            {
                cost += (int)costScaleFactor;
            }
            else
            {
                cost = (int)((double)cost * costScaleFactor);
            }
            
            level++;
        }
    }
}
