using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public Animator anim;
    private const String BOOL_IDLE = "idle";
    private const String BOOL_FORWARD = "forward";
    private const String BOOL_STAND = "stand";
    
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode jump = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Transform rotTarget;
    public Transform arrowExit;

    public float moveSpeed = 3;
    public float jumpForce = 4;
    
    // Update is called once per frame
    void Update()
    {

    }

    public float adjust = 180f;

    public Transform head;
    public Transform twistTarget;

    private void LateUpdate() {

        // Get speed
        Vector3 vel = GetComponent<Rigidbody2D>().velocity;

        // Set H speed (move)
        vel.x = Input.GetAxis("Horizontal") * moveSpeed;

        // Player front or back facing
        if(vel.x > 0){
            Vector3 qEuler = player.transform.rotation.eulerAngles;
            Quaternion q = Quaternion.Euler(qEuler.x, 180, qEuler.z);
            player.transform.rotation = q;
        }else if(vel.x < 0){
            Vector3 qEuler = player.transform.rotation.eulerAngles;
            Quaternion q = Quaternion.Euler(qEuler.x, 0, qEuler.z);
            player.transform.rotation = q;
        }
        
        // Set V speed (jump)
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            vel.y = jumpForce;
        }

        // Set speed
        GetComponent<Rigidbody2D>().velocity = vel;

        bool isTurning = false;
        if(Input.GetMouseButton(0)){
            isTurning = true;
            //anim.enabled = false;

            Quaternion q;

            // Turn rotation
            Vector3 mouseScreen = Input.mousePosition;
            Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);
            
            float z = Mathf.Atan2(mouse.y - rotTarget.position.y, mouse.x - rotTarget.position.x) * Mathf.Rad2Deg;
            q = Quaternion.Euler(0, 0, z) ;
            rotTarget.rotation = q;

            //transform.Lo

            // Twist
            if(Math.Abs(Vector3.Angle(head.up, Vector3.up)) > 90){
            
                Vector3 qEuler = twistTarget.rotation.eulerAngles;
                    q = Quaternion.Euler(qEuler.x, 180, qEuler.z);
                    twistTarget.rotation = q;
            }
            else
            {
                
                Vector3 qEuler = twistTarget.rotation.eulerAngles;
                q = Quaternion.Euler(qEuler.x, 0, qEuler.z);
                twistTarget.rotation = q;
            }            
            

        }


        // Animate based on speed
        if(Math.Abs(vel.x) > 0){
            Animate(anim, BOOL_FORWARD);
        }else
        {
            if(!isTurning) Animate(anim, BOOL_IDLE);
            else Animate(anim, BOOL_STAND);
        }


    }


    void Animate(Animator anim, String animation)
    {
        DisableOtherAnimations(anim, animation);
        anim.SetBool(animation, true);

        
    }

    void DisableOtherAnimations(Animator anim, String animation)
    {
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if(param.name != animation)
            {
                anim.SetBool(param.name, false);
            }
        }
    }
}
