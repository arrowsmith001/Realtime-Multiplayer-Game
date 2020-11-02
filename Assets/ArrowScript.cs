using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public bool debug = false;
    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        // PV = GetComponent<PhotonView>();

        // if(debug) return;

        // if(!PV.IsMine) {
        //     Destroy(GetComponent<Rigidbody2D>());
        //     motionControlled = false;
        // }
    }

    bool motionControlled = true;

    // Update is called once per frame
    void Update()
    {
        if(motionControlled){

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
        if(other.gameObject.GetComponent<PlayerController>() != null 
            && other.gameObject.GetComponent<PlayerController>() == this.controller) return;

        if(other.gameObject.GetComponent<ArrowScript>() != null) return;

        motionControlled = false;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
    }

    PlayerController controller;

    internal void PassReference(PlayerController controller)
    {
        this.controller = controller;
    }

    internal void MakeActive()
    {
            this.gameObject.AddComponent<Rigidbody2D>();
            motionControlled = true;
    }
}
