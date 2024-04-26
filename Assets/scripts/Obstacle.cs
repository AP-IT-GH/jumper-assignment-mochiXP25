using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float MoveSpeed = 3.5f;
    private void Update()
    {
        this.transform.Translate(Vector3.back * MoveSpeed * Time.deltaTime); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wallend" )
        {
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "agent")
        {

            Destroy(this.gameObject);
            Destroy(GameObject.FindWithTag("destroy"));

        }
    }
}
