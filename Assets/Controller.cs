using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public bool debug = false;

    public GameObject projectile;

    private PhotonView PV;
    private void Awake() {
        PV = GetComponent<PhotonView>();    
    }

    private void Start() {
        if(debug) return;
        if(!PV.IsMine) {
            Destroy(GetComponent<Rigidbody2D>());
            if(PV.Owner.CustomProperties.ContainsKey("color")) 
                ChangeColor(MenuController.inst.GetColor((int) PV.Owner.CustomProperties["color"]));    
        }
    }


    public float moveSpeed = 3;
    public float jumpForce = 4;

    public Transform rotTarget;
    public Transform arrowExit;

    public void ChangeColor(Color color)
    {
        //this.head.gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    public Transform headRaisedPosition;
    public Transform headCrouchedPosition;

    void Move(){

        if(Input.GetMouseButton(0)){
            // Turn rotation
            Vector3 mouseScreen = Input.mousePosition;
            Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);
            rotTarget.rotation = (Quaternion.Euler(0, 0, Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg));
        }
        else
        {
            rotTarget.rotation = Quaternion.Euler(0,0,0);
        }


            // Get speed
            Vector3 vel = GetComponent<Rigidbody2D>().velocity;

            // Set H speed (move)
            vel.x = Input.GetAxis("Horizontal") * moveSpeed;
            
            // Set V speed (jump)
            if(Input.GetKeyDown(KeyCode.UpArrow)){
                vel.y = jumpForce;
            }

            // Set speed
            GetComponent<Rigidbody2D>().velocity = vel;

            // Duck
            if(Input.GetKey(KeyCode.DownArrow)){
              //  head.transform.position = Vector3.Lerp(headCrouchedPosition.position, headRaisedPosition.position, Time.deltaTime);
            }else
            {
              //  head.transform.position = Vector3.Lerp(headRaisedPosition.position, headCrouchedPosition.position, Time.deltaTime);
            }
    }

    public Transform projectileExit;

    public float shootForce = 20;

    private void Shoot()
    {

        GameObject proj;

        if(!debug) proj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Arrow"), projectileExit.position, arrowExit.rotation);
        else {
            proj = Instantiate(Resources.Load<GameObject>(Path.Combine("Prefabs", "Arrow")), projectileExit.position, arrowExit.rotation);
            // proj.GetComponent<ArrowScript>().MakeActive();
        }

        proj.GetComponent<Rigidbody2D>().AddForce(arrowExit.transform.right * shootForce, ForceMode2D.Impulse);
        //proj.GetComponent<ArrowScript>().PassReference(this);
        proj.GetComponent<ArrowScript>().debug = debug;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(debug || PV.IsMine){

            Move();

            if(Input.GetMouseButtonDown(0)) Shoot();

        }


    }


    public enum DataUpdate{
        NewPositionX, NewPositionY, NewRotation
    }


}

