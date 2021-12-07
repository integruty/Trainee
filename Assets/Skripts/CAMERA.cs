using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CAMERA : MonoBehaviour
{	public GameObject player,hinge;
    float RF=1f;
    InputPlayer playerInput;
    public float y, z=0;
    Vector2 CurRotInput;
    bool isMovin;
    public float SRot, SAng;
    public float maxRotate =60;
    public float minRotate = -60;
    public float maxAngle= 30;
    public float minAngle = 0;
    // Start is called before the first frame update
    void Awake()
    {   
        SRot = hinge.transform.rotation.y*180;
        SAng = hinge.transform.rotation.x*180;
        z = SRot;
        y = SAng;
        playerInput = new InputPlayer();
        
        playerInput.Controls.Cameras.performed += CamControl;
        player =  GameObject.FindGameObjectWithTag("PlayerHead");
    }
    void CamControl(InputAction.CallbackContext context) 
    {  CurRotInput = context.ReadValue<Vector2>();
        y += -CurRotInput.y * 15;
        if (y>maxAngle+SAng||y<minAngle+SAng)
            y += CurRotInput.y * 15;
        Debug.Log("y=" + y);
        z += CurRotInput.x * 30;
        if (z >maxRotate+SRot || z < minRotate+SRot)
            z += -CurRotInput.x * 30;
        Debug.Log("z=" + z);
        isMovin = CurRotInput.y != 0 || CurRotInput.x != 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (isMovin) 
        {
            Quaternion posToLookAt;
            posToLookAt = Quaternion.Euler(y,z,0f) ;
            posToLookAt.Normalize();
            //posToLookAt.x=CurMove.x;

            //posToLookAt.y=0f;

            //posToLookAt.z=CurMove.z;

            Quaternion curRot = hinge.transform.rotation;

            if (isMovin)

            {               
                hinge.transform.rotation = Quaternion.Slerp(curRot, posToLookAt, RF*Time.deltaTime);
            }
           
        }
	//transform.LookAt(player.transform);
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
