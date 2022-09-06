using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CharacterBehavior : MonoBehaviour {

    public enum chartype { FRIENDLY, ENEMEY}

    public chartype CharacterType = chartype.ENEMEY;

    private GameObject SceneManagerRef;
    public GameObject statusText;
    private CombatManager manager;
    public GameObject HPBar;
    public GameObject RedBar;
    public GameObject floatingCombatTextPrefab;



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

    public UnityEvent TakeDamageEvent;

    // Use this for initialization
    public virtual void Start() {
        stats.currentHP = stats.maxHP;
        this.gameObject.GetComponent<CharacterUIScript>().initUI();
    }

    // Update is called once per frame
    public virtual void Update() {
       // setStatusText(currentHP.ToString() + " / " + stats.maxHP.ToString());
    }

    public virtual void OnEnable()
    {
        CombatManager.RegisterCharacter(this);

    }

    public virtual void attack()
    {
        CharacterBehavior target = CombatManager.GetWeakestEnemy(this);
        if (target != null)
        {
            target.takeDamage(CombatManager.currentChar.getDamage());
        }
    }

    public virtual void performAction()
    {
        //manager.getEnemyList();
        //raise attack event to do gui stuff and animations
        statusText.GetComponent<Text>().text = "attack";
        attack();

    }

    public virtual void die()
    {
        CombatManager.DeRegisterCharacter(this);
        //this.gameObject.transform.rotation = 
        //    Quaternion.Lerp(this.gameObject.transform.rotation, 
        //    Quaternion.Euler(
        //        transform.eulerAngles + new Vector3(0, 0, 270f)), 1f);
        if(CharacterType == chartype.FRIENDLY)
        {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().transform.Rotate(0, 0, 90f);

        }
        if (CharacterType == chartype.ENEMEY)
        {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().transform.Rotate(0, 0, 270f);
        }
    }

    public void setStatusText(string status)
    {
        statusText.GetComponent<Text>().text = status;
    }



    private void OnDisable()
    {
        CombatManager.DeRegisterCharacter(this);
    }

    public void takeDamage(int damage)
    {
        //damage -= stats.defense;
        if (damage <= 0) damage = 1;
        stats.currentHP -= damage;
        if (stats.currentHP < 0) stats.currentHP = 0;
        TakeDamageEvent.Invoke();
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

    public virtual void OnDestroy()
    {
        Debug.Log(gameObject.name + " destroyed");
    }

    public void applyXP(int xpValue)
    {
        stats.xp += xpValue;
    }


}
