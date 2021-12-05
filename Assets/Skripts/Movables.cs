using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movables : MonoBehaviour
{
    GameObject player;
    public Rigidbody Rb;
    
    Vector3 forward;
    public Transform playerPos;
    public Transform block;
    public bool NearPlayer;
    public bool Picked;
    InputPlayer playerInput;
    // Start is called before the first frame update
    void Awake() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos =player.transform;
        playerInput = new InputPlayer();

        playerInput.Controls.Pick.started += Pick;

    }
   
    void Pick (InputAction.CallbackContext context) 
    {
        Debug.Log("Picked");
        if (NearPlayer&!Picked) 
            {
                Picked = true;
            Rb.useGravity = false;
               
            }
        
            else if (Picked) 
            { 
                Picked = false;
            Rb.useGravity = true;
             
            }
    }
    void OnTriggerEnter(Collider trig)
    {
        if (trig.tag == "Player")
            
        NearPlayer = true;
    }
    void OnTriggerExit(Collider trig)
    {
        if (trig.tag == "Player")
            
        NearPlayer = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Picked) 
        {            
            block.position = playerPos.position;
            block.rotation = playerPos.rotation;
            block.Translate(0f, .75f, 1.2f);
        }
        
    }
    void OnEnable()
    {
        playerInput.Controls.Enable();
    }
    void OnDisable()
    {
        playerInput.Controls.Disable();
    }
}
