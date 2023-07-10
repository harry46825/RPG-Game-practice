using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    // Start is called before the first frame update
    float Health;

    void Start()
    {
        Health = GameObject.Find("Player").GetComponent<PlayerInformation>().PlayerHealth;
    }

    void OnCollisionEnter(Collision collisionInfo) 
    {
        Destroy(gameObject);

        if(collisionInfo.collider.tag == "Player")
        {
            Health -= 2;
        }
    }
}
