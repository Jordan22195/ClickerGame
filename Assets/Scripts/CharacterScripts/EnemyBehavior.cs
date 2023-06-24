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

    public GameObject HPBar;
    public GameObject RedBar;
    public GameObject floatingCombatTextPrefab;
    public GameObject sprite;

    private bool combatStarted = false;

    void OnMouseDown()
    {
        takeDamage((int)CombatManager.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.CLICK_DAMAGE) );
        Debug.Log("eb click");
    }

    public  override void Start()
    {
        base.Start();
        HPBar.SetActive(false);
        RedBar.SetActive(false);
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


    public void die()
    {
        CombatManager.applyXP(XPvalue);
        sprite.transform.Rotate(0, 0, 270f);
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


    //called with TakeDamageEvent
    public void updateHPBar()
    {
        float p = (float)stats.currentHP / (float)stats.maxHP;
        Vector3 barscale = RedBar.transform.localScale;
        Vector3 barposition = RedBar.transform.localPosition;
        float barwidth = RedBar.GetComponent<SpriteRenderer>().size.x * barscale.x;
        barscale.x = barscale.x * p;
        barposition.x = RedBar.transform.localPosition.x - ((1 - p) * barwidth / 2);
        HPBar.transform.localScale = barscale;
        HPBar.transform.localPosition = barposition;
    }

    public override void Update()
    {
     

    }

    public void takeDamage(int damage)
    {
        Debug.Log("take damage");
        //damage -= stats.defense;
        //if (damage <= 0) damage = 1;
        stats.currentHP -= damage;
        updateHPBar();
        if (stats.currentHP < 0) stats.currentHP = 0;
        
        //setStatusText("-" + damage.ToString());

        GameObject clone = Instantiate(floatingCombatTextPrefab,
            this.gameObject.transform.position,
            Quaternion.identity);
        clone.GetComponent<floatingCombatTextBehavior>().setFloatingCombatText("-" + damage);

        if (stats.currentHP <= 0)
        {
            die();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "char1" && stats.currentHP > 0 && !combatStarted)
        {
            combatStarted = true;
            enemyCollisionEvent.TriggerEvent();
            CombatManager.registerEnemy(this);

            HPBar.SetActive(true);
            RedBar.SetActive(true);

        }
    }





}
