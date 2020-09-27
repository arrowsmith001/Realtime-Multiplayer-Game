using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    bool inMotion = true;

    // Update is called once per frame
    void Update()
    {
        if(inMotion){

                Vector3 direction = GetComponent<Rigidbody2D>().velocity.normalized;

                if(direction != Vector3.zero){

                    // rotate that vector by 90 degrees around the Z axis
                    Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * direction;
                    
                    // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
                    // (resulting in the X axis facing the target)
                    Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

                    // changed this from a lerp to a RotateTowards because you were supplying a "speed" not an interpolation value
                    transform.rotation = targetRotation;
                }
            
        }

    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.GetComponent<Controller>() != null 
            && other.gameObject.GetComponent<Controller>() == this.controller) return;

        inMotion = false;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
    }

    Controller controller;

    internal void PassReference(Controller controller)
    {
        this.controller = controller;
    }
}
