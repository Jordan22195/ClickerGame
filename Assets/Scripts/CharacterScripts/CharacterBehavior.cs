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


    // Use this for initialization
    public virtual void Start() {
        stats.currentHP = stats.maxHP;
    }

    // Update is called once per frame
    public virtual void Update() {
       // setStatusText(currentHP.ToString() + " / " + stats.maxHP.ToString());
    }

    public virtual void attack()
    {

    }

    public void setStatusText(string status)
    {
        statusText.GetComponent<Text>().text = status;
    }



    private void OnDisable()
    {
        CombatManager.DeRegisterCharacter(this);
    }


    public void applyXP(int xpValue)
    {
        stats.xp += xpValue;
    }


}
