using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class townScene : MonoBehaviour {

    GameObject char1;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OpenDungeonScene()
    {
        SceneManager.LoadScene("dungeon1", LoadSceneMode.Single);
    }
}
