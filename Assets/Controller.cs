using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public GameObject projectile;

    private PhotonView PV;
    private void Awake() {
        PV = GetComponent<PhotonView>();    
    }

    private void Start() {
        if(!PV.IsMine) {
            Destroy(GetComponent<Rigidbody2D>());
            if(PV.Owner.CustomProperties.ContainsKey("color")) 
                ChangeColor(MenuController.inst.GetColor((int) PV.Owner.CustomProperties["color"]));    
        }
    }

    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode jump = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    

    public float moveSpeed = 3;
    public float jumpForce = 4;

    public Transform head;

    public void ChangeColor(Color color)
    {
        this.head.gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    public Transform headRaisedPosition;
    public Transform headCrouchedPosition;

    void Move(){
        // Turn head
            Vector3 mouseScreen = Input.mousePosition;
            Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);
            head.rotation = (Quaternion.Euler(0, 0, Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg));

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
                head.transform.position = Vector3.Lerp(headCrouchedPosition.position, headRaisedPosition.position, Time.deltaTime);
            }else
            {
                head.transform.position = Vector3.Lerp(headRaisedPosition.position, headCrouchedPosition.position, Time.deltaTime);
            }
    }

    public Transform projectileExit;

    public float shootForce = 20;

    private void Shoot()
    {
        GameObject proj = Instantiate(projectile, projectileExit.position, head.rotation);
        proj.GetComponent<ArrowScript>().PassReference(this);
        proj.GetComponent<Rigidbody2D>().AddForce(head.transform.right * shootForce, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if(PV.IsMine){

            Move();

            if(Input.GetMouseButtonDown(0)) Shoot();

        }


    }


    public enum DataUpdate{
        NewPositionX, NewPositionY, NewRotation
    }


}

