using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theButtonScript : MonoBehaviour
{

    public GameEvent theButtonClickEvent;

    public Sprite ButtonDown;
    public Sprite ButtonUp;
    public Sprite ButtonDone;

    bool buttonDown = false;


    float triggersPerSecond = 5;
    float triggerTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonDown)
        {
            if(Time.fixedTime > triggerTime)
            {
                theButtonClickEvent.TriggerEvent();
                triggerTime = Time.fixedTime + (1 / triggersPerSecond);
            }
        }

    }


    private void OnMouseUp()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = ButtonUp;
        buttonDown = false;
        triggerTime = 0f;
    }
    private void OnMouseDown()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = ButtonDown;
        buttonDown = true;
    }
}
