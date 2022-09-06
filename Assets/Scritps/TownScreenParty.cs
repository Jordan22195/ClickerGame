using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownScreenParty : MonoBehaviour {

    public GameObject char1;
    private static bool gameInitialized = false;
	// Use this for initialization
	void Start () {
        if(!gameInitialized)
        {
            initGame();
        }
        char1.name = "char1";
        party.char1.currentHP = party.char1.maxHP;
        char1.GetComponent<CharacterUIScript>().loadChar(party.char1);

    }
	
	// Update is called once per frame
	void Update () {

        //char1Text.GetComponent<Text>().text = party.char1.name + "\r\n" + "Level : " + party.char1.level.ToString();
		
	}

    private void initGame()
    {
        party.char1.name = "Billy";
        party.char1.maxHP = 15;
        gameInitialized = true;
    }
}
