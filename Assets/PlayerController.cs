using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerController : MonoBehaviour
{
    public const bool debug = false;
    public float shootForce = 5;
    public GameObject player;
    public Animator anim;
    private const String BOOL_IDLE = "idle";
    private const String BOOL_FORWARD = "forward";
    private const String BOOL_STAND = "stand";
    private const String BOOL_JUMP = "jump";
    private const String BOOL_SHOOT = "shoot";
    private const String BOOL_AIM = "aim";
    private const int LAYER_BASE = 0;
    private const int LAYER_UPPER = 1;
    private List<String> baseAnims = new List<String>{BOOL_IDLE, BOOL_FORWARD, BOOL_JUMP, BOOL_STAND};
    
    
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode jump = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Transform[] rotTargets;
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

        MoveAndRotate();

    }

    void MoveAndRotate(){

        // Get speed
        Vector3 vel = GetComponent<Rigidbody2D>().velocity;
        float velx = vel.x;

        // Set H speed (move)
        vel.x = Input.GetAxis("Horizontal") * moveSpeed;

        // Set V speed (jump)
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            vel.y = jumpForce;
            Animate(anim, BOOL_JUMP);
        }

        // Set speed
        GetComponent<Rigidbody2D>().velocity = vel;
        float y = 0, z = 0;
        bool isTurning = false;

        if(Input.GetMouseButton(0)){
            isTurning = true;

            ChangeLayerWeight(anim, LAYER_UPPER, 1);

            // Turn rotation
            Vector3 mouseScreen = Input.mousePosition;
            Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);
            float mouseX = mouse.x - arrowExit.position.x;
            float mouseY = mouse.y - arrowExit.position.y;

            z = Mathf.Atan2(mouseY, mouseX) * Mathf.Rad2Deg;
            
            // Accounts for twist
            y = mouseX > 0 ? 0 : 180;
            if(mouseX < 0) z = 180 - z; 
        
            Quaternion q = Quaternion.Euler(0, y, z) ;
            RotateAllTargets(q);

            if(Input.GetMouseButtonDown(1) && !isShooting){
                StartCoroutine(Shoot());
            }
 
        }else
        {
            ChangeLayerWeight(anim, LAYER_UPPER, 0);
        }

        // Player front or back facing
        if(velx > 0){
            Vector3 qEuler = player.transform.rotation.eulerAngles;
            Quaternion q = Quaternion.Euler(qEuler.x, 180, qEuler.z);
            player.transform.rotation = q;
        }else if(velx < 0){
            Vector3 qEuler = player.transform.rotation.eulerAngles;
            Quaternion q = Quaternion.Euler(qEuler.x, 0, qEuler.z);
            player.transform.rotation = q;
        }

        // Animate based on speed
        if(Math.Abs(velx) > 0){
            Animate(anim, BOOL_FORWARD);
        }else
        {
            if(!isTurning) Animate(anim, BOOL_IDLE);
            else Animate(anim, BOOL_STAND);
        }

    }

    void RotateAllTargets(Quaternion q){
        foreach(Transform t in rotTargets){
            t.rotation = q;
        }
    }

    public float shootCooldownMs = 500;
    private bool isShooting = false;    
    IEnumerator Shoot(){

        isShooting = true;
        anim.SetBool(BOOL_AIM, false);
        anim.SetBool(BOOL_SHOOT, true);
        
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);

        GameObject proj = Instantiate(Resources.Load<GameObject>(Path.Combine("Prefabs", "Arrow")), arrowExit.position, Quaternion.Euler(-mouse + arrowExit.position));
        proj.GetComponent<Rigidbody2D>().AddForce(-(mouse - arrowExit.position) * shootForce, ForceMode2D.Impulse);
        proj.GetComponent<ArrowScript>().PassReference(this);
        proj.GetComponent<ArrowScript>().debug = debug;

        yield return new WaitForSeconds(shootCooldownMs/1000);

        anim.SetBool(BOOL_AIM, true);
        anim.SetBool(BOOL_SHOOT, false);
        isShooting = false;
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
            if(param.name != animation && baseAnims.Contains(param.name))
            {
                anim.SetBool(param.name, false);
            }
        }
    }

    void ChangeLayerWeight(Animator anim, int layer, int to){
        anim.SetLayerWeight(layer, to);
    }
}
