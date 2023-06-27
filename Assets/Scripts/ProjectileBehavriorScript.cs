using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavriorScript : MonoBehaviour
{

    public Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, 100f);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
