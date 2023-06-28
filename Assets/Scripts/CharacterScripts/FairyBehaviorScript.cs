using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scritps;

public class FairyBehaviorScript : MonoBehaviour
{
    float YDir = 0;
    float despawnTime;
    public  int value;
    // Start is called before the first frame update
    void Start()
    {
        despawnTime = Time.fixedTime + 30f;   
    }

    // Update is called once per frame
    void Update()
    {
        float high = YDir + 0.2f;
        float low = YDir - 0.2f;
        if (high > 1.0f) high = 1.0f;
        if (low < -1.0f) low = -1.0f;
        if(this.gameObject.transform.position.y > 500)
        {
            high = 0f;
        }
        if(this.gameObject.transform.position.y < -500)
        {
            low = 0f;
        }


       YDir = Random.Range(low, high);
        Vector3 movement = new Vector3(1f, YDir, 0f);
        this.gameObject.transform.position +=
            movement *
            Time.deltaTime *
            1.0f *
            Globals.pixelsPerMeter;

        if (Time.fixedTime > despawnTime)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnMouseDown()
    {
        CombatManager.managerRef.gold += value;
        Destroy(this.gameObject);
    }

}
