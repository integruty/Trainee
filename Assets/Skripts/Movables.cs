using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movables : MonoBehaviour
{
    GameObject player;
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
                Debug.Log(Picked);
            }
        
            else if (NearPlayer & Picked) 
            { 
                Picked = false; 
                Debug.Log(Picked); 
            }
    }
    void OnTriggerEnter(Collider trig)
    {
        if (trig.tag == "Player")
            Debug.Log("PlayerNear");
        NearPlayer = true;
    }
    void OnTriggerExit(Collider trig)
    {
        if (trig.tag == "Player")
            Debug.Log("PlayerLeaved");
        NearPlayer = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Picked) 
        {
            Vector3 forward=playerPos.forward;
            forward.Normalize();
            block.position = playerPos.position;
            block.Translate(player.GetComponent<AnimateMoveControls>().RealMove+ player.GetComponent<AnimateMoveControls>().MoveVec*2, playerPos);
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
