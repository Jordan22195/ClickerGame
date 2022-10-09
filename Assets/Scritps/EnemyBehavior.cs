using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : CharacterBehavior {

    void OnMouseDown()
    {
        takeDamage((int)CombatManager.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.CLICK_DAMAGE) );
        Debug.Log("eb click");
    }

    public int XPvalue
    {
        get
        {
            return stats.level;
        }
    }

    public override void OnEnable()
    {
        setStatusText("lvl " + stats.level.ToString());
        base.OnEnable();
    }

    public override void die()
    {
        CombatManager.applyXP(XPvalue);
        base.die();
    }

    public override void performAction()
    {

    }

    public override void attack()
    {

    }

    public override void Update()
    {
        if (CombatManager.currentDungeonState == CombatManager.DungeonState.RUNNING)
        {
            Vector3 movement = new Vector3(-1f, 0f, 0f);
            this.gameObject.transform.position += movement * Time.deltaTime * (float)CombatManager.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.MOVEMENT_SPEED) * DungeonUIBehaviorScript.pixelsPerMeter;
        }


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CombatManager.currentDungeonState = CombatManager.DungeonState.COMBAT;
    }





}
