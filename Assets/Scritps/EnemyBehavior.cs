using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : CharacterBehavior {

    void OnMouseDown()
    {
        if (CombatManager.currentDungeonState == CombatManager.DungeonState.COMBAT)
        {
            takeDamage(CombatManager.currentClicker.getDamage());
        }
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

}
