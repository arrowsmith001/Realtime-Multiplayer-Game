using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
            Vector3 mouseScreen = Input.mousePosition;
            Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);
            mouse.z = 0;
            transform.position = mouse;
    }
}
