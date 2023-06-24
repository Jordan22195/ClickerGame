﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{

    DateTime despawnTime;

    public GameEvent enemyCollisionEvent;
    public GameEvent enemyDieEvent;
    public GameEvent enemyDestroyEvent;

    public GameObject HPBar;
    public GameObject RedBar;
    public GameObject floatingCombatTextPrefab;
    public GameObject sprite;

    private bool combatStarted = false;


    public enum chartype { FRIENDLY, ENEMEY }

    public chartype CharacterType = chartype.ENEMEY;

    private GameObject SceneManagerRef;
    public GameObject statusText;
    private CombatManager manager;




    private CharacterStats _stats = new CharacterStats();

    public virtual CharacterStats stats
    {
        get
        {
            return _stats;
        }
        set
        {
            _stats = value;
        }
    }


    void OnMouseDown()
    {
        takeDamage((int)CombatManager.managerRef.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.CLICK_DAMAGE) );
    }

    public  void Start()
    {
        stats.currentHP = stats.maxHP;

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
        CombatManager.managerRef.applyXP(XPvalue);
        sprite.transform.Rotate(0, 0, 270f);
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


    public void takeDamage(int damage)
    {
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
            CombatManager.managerRef.registerEnemy(this);

            HPBar.SetActive(true);
            RedBar.SetActive(true);

        }
    }



    public virtual void attack()
    {

    }

    public void setStatusText(string status)
    {
        statusText.GetComponent<Text>().text = status;
    }




    public void applyXP(int xpValue)
    {
        stats.xp += xpValue;
    }



}
