using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendlyCharacterBehavior : CharacterBehavior {


    public int attackPower = 0;
    public float attackSpeed = 0.5f; // attacks per second
    public int clickDamage = 1;
    public double moveSpeed = 1; // meters per second. todo: make this number make sense
    public int passiveDamage = 0; //dps?

    // Use this for initialization
    public override void Start () {
        loadChar();
        attackPower = 1;
        attackSpeed = 0.5f; // attacks per second
        clickDamage = 1;
        moveSpeed = 1; // meters per second. todo: make this number make sense
        passiveDamage = 0; //dps?
        //CharacterType = chartype.FRIENDLY;
        base.Start();
        
    }
	
	// Update is called once per frame
	public override void Update () {
        //statusText.GetComponent<Text>().text = "HP: " + me.HP.ToString();
        base.Update();
        attackSpeed = 0.5f;
        Debug.Log(attackSpeed);
    }

    public override void OnEnable()
    {
        CharacterType = chartype.FRIENDLY;
        base.OnEnable();
    }

    public override void performAction()
    {

    }

    public override void attack()
    {
        CharacterBehavior target = CombatManager.GetWeakestEnemy(this);
        if (target != null)
        {
            int damage = (int)CombatManager.getUpgradedStat(UpgradeButtonBehaviorScript.EnumBonusType.ATTACK_POWER);
            CombatManager.updateDPS(damage);
            target.takeDamage(damage);
        }
    }

    public void incHP()
    {
        stats.currentHP--;
    }

    public void loadChar()
    {

        if (this.gameObject.name == "char1" && party.char1 != null)
        {
            this.stats = party.char1;
        }
        else if (this.gameObject.name == "char2" && party.char2 != null)
        {
            this.stats = party.char1;
        }
        else if (this.gameObject.name == "char3" && party.char3 != null)
        {
            this.stats = party.char1;
        }
        else if (this.gameObject.name == "char4" && party.char4 != null)
        {
            this.stats = party.char1;
        }
    }

    public void saveChar()
    {
        if (this.gameObject.name == "char1")
        {
            if (party.char1 == null)
            {
                party.char1 = new CharacterStats();
            }
            party.char1 = stats;
        }
        else if (this.gameObject.name == "char2")
        {
            if (party.char2 == null)
            {
                party.char2 = new CharacterStats();
            }
            party.char2 = stats;
        }
        else if (this.gameObject.name == "char3")
        {
            if (party.char3 == null)
            {
                party.char3 = new CharacterStats();
            }
            party.char3 = stats;
        }
        else if (this.gameObject.name == "char4")
        {
            if (party.char4 == null)
            {
                party.char4 = new CharacterStats();
            }
            party.char4 = stats;
        }
    }

    public override void OnDestroy()
    {
        //saveChar();
        base.OnDestroy();
    }

    public override void die()
    {
        base.die();
        return;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    }

}
