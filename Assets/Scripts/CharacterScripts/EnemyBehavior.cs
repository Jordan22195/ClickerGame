using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class EnemyBehavior : CharacterBehavior {

    DateTime despawnTime;

    public GameEvent enemyCollisionEvent;
    public GameEvent enemyDieEvent;
    public GameEvent enemyDestroyEvent;

    private bool combatStarted = false;

    void OnMouseDown()
    {
        takeDamage((int)CombatManager.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.CLICK_DAMAGE) );
        Debug.Log("eb click");
    }

    void boop()
    {
        Debug.Log("boop");
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
        Debug.Log("die");
        enemyDieEvent.TriggerEvent();

        StartCoroutine(destroySelfCoroutine());
    }

    IEnumerator destroySelfCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        enemyDestroyEvent.TriggerEvent();
        Destroy(this.gameObject);

    }

    public override void performAction()
    {

    }

    public override void attack()
    {

    }

    public override void Update()
    {
     

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "char1" && stats.currentHP > 0 && !combatStarted)
        {
            combatStarted = true;
            enemyCollisionEvent.TriggerEvent();
            CombatManager.registerEnemy(this);
        }
    }





}
