using Assets.Scritps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float myYRotation = this.gameObject.transform.rotation.y;
        float cameraYRotation = Globals.cameraObject.transform.rotation.y;
        if (this.gameObject.transform.rotation.y != Globals.cameraObject.transform.rotation.y)
        {
            float delta = cameraYRotation - myYRotation;
            this.gameObject.transform.rotation = new Quaternion(0, Camera.main.transform.rotation.y, 0, 0);
        }
    }
}
