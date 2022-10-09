using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendlyCharacterBehavior : CharacterBehavior {


    // Use this for initialization
    public override void Start () {
        loadChar();
        //CharacterType = chartype.FRIENDLY;
        base.Start();
    }
	
	// Update is called once per frame
	public override void Update () {

        //statusText.GetComponent<Text>().text = "HP: " + me.HP.ToString();
        base.Update();
    }

    public override void OnEnable()
    {
        CharacterType = chartype.FRIENDLY;
        base.OnEnable();
    }

    public override void performAction()
    {

    }
    public void incHP()
    {
        stats.currentHP--;
        Debug.Log(this.gameObject.name + " " + stats.currentHP.ToString());
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
        CombatManager.DeRegisterCharacter(this);
        this.gameObject.transform.rotation =
            Quaternion.Lerp(this.gameObject.transform.rotation,
            Quaternion.Euler(
                transform.eulerAngles + new Vector3(0, 0, 90f)), 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("friendly collision");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("friendly trigger");
    }

}
