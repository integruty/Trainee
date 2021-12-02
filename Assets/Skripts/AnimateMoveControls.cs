using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class AnimateMoveControls : MonoBehaviour
{


	public float speed = 1f;

	float RF = .9f;

	public float JumpForce = 5;

	public float runMult = 1.5f;

	public float JumpH = 2;

	public float JumpT = 1;

	public Transform target;

	public float CurYVel;

	public GameObject[] cameras;

	float groundGrav = -.5f;

	float Grav = -4.9f;

	CharacterController charControl;

	InputPlayer playerInput;

	Animator animator;

	int curCam=0;

	int isWalkHash, isRunHash, isJumpHash;

	public Vector2 CurMoveInput;

	Vector3 CurMove, CurRun, forward, right;
	public Vector3 RealMove,MoveVec;
	bool isMovementPressed;

	bool isRunPressed;

	public bool isJumpPressed = false;

	bool walking;

	bool running;

	public bool jumping;

	public GameObject ground;

	void Awake()

	{
		VarSetup();
		
		isWalkHash = Animator.StringToHash("walk");

		isRunHash = Animator.StringToHash("run");

		isJumpHash = Animator.StringToHash("jump");

		charControl = GetComponent<CharacterController>();

		animator = GetComponent<Animator>();

		playerInput = new InputPlayer();

		playerInput.Controls.Move.started += onInputMove;

		playerInput.Controls.Move.canceled += onInputMove;

		playerInput.Controls.Move.performed += onInputMove;

		playerInput.Controls.Run.started += onRun;
		playerInput.Controls.Run.canceled += onRun;

		playerInput.Controls.Jump.started += onJump;
		playerInput.Controls.Jump.canceled += onJump;

		playerInput.Controls.Cameras.started += CamHang;
	}

	void VarSetup()
	{

		cameras = GameObject.FindGameObjectsWithTag("MainCamera");
		Grav = (-2 * JumpH / Mathf.Pow(JumpT / 2, 2));
		groundGrav = (-2 * JumpH / Mathf.Pow(JumpT / 2, 2)) / 50;
		JumpForce = (4 * JumpH) / JumpT;
		target = cameras[curCam].transform;
		for (int i = 0; i < cameras.Length; i++)
		{
			if (i != curCam)
				cameras[i].SetActive(false);
			else
				cameras[i].SetActive(true);
		}
	}

	void HangJump()
	{
		if (isJumpPressed & !jumping & Grounded())
		{
			CurYVel = JumpForce;
			jumping = true;


		}
		else
		if (!isJumpPressed & jumping & Grounded())
		{
			jumping = false;
		}
	}

	void HangGrav()
	{
		if (!jumping & Grounded())
		{

			CurYVel = groundGrav;
		} else if (jumping || !Grounded())
		{
			CurYVel += Grav * Time.deltaTime;
		}
	}

	void onJump(InputAction.CallbackContext context)
	{
		isJumpPressed = context.ReadValueAsButton();
	}

	void onRun(InputAction.CallbackContext context)

	{

		isRunPressed = context.ReadValueAsButton();

	}
	void onInputMove(InputAction.CallbackContext context)
	{

		CurMoveInput = context.ReadValue<Vector2>();

		CurMove.x = CurMoveInput.x * Time.deltaTime * speed;

		CurMove.z = CurMoveInput.y * Time.deltaTime * speed;

		CurRun.x = CurMoveInput.x * Time.deltaTime * speed * runMult;

		CurRun.z = CurMoveInput.y * Time.deltaTime * speed * runMult;
		isMovementPressed = CurMoveInput.x != 0 || CurMoveInput.y != 0;

	}
	bool Grounded()
	{
		return Physics.CheckSphere(ground.transform.position, .1f);
	}
	void CamHang(InputAction.CallbackContext context)
	{
				curCam++;
			if (curCam >= cameras.Length)
				curCam=0;
			target = cameras[curCam].transform;
			for (int i=0;i< cameras.Length; i++) 
			{if (i != curCam)
					cameras[i].SetActive(false);
				else
					cameras[i].SetActive(true);
			}
		
	}
	void AnimHang()
	{
		walking=animator.GetBool(isWalkHash);

		running=animator.GetBool(isRunHash);

		jumping=animator.GetBool(isJumpHash);

		if(isMovementPressed&!walking)

		{

			animator.SetBool(isWalkHash,true);

		}

		if(!isMovementPressed&walking)

		{

			animator.SetBool(isWalkHash,false);

		}

		if((isMovementPressed&isRunPressed)&!running)

		{

			animator.SetBool(isRunHash,true);

		}else

		if((!isMovementPressed||!isRunPressed)&running)

		{

			animator.SetBool(isRunHash,false);

		}

		if(isJumpPressed&!jumping)

		{

			animator.SetBool(isJumpHash,true);

		}else

		if(!isJumpPressed&ground)

		{
			
			animator.SetBool(isJumpHash,false);

		}
		
	}
	void RotHang()
	{
		Vector3 posToLookAt;
		posToLookAt=CurMoveInput.y*forward+CurMoveInput.x*right;
		//posToLookAt.x=CurMove.x;

		//posToLookAt.y=0f;

		//posToLookAt.z=CurMove.z;
		
		Quaternion curRot=transform.rotation;

		if(isMovementPressed)

		{
		Quaternion targetRot=Quaternion.LookRotation(posToLookAt);
		transform.rotation=Quaternion.Slerp(curRot,targetRot,RF); 
		}
	}
	void RelMove()
	{
		forward = target.forward;
		right = target.right;
		
		forward.y = 0f;
		right.y = 0f;
		
		forward.Normalize ();
		right.Normalize ();
		MoveVec = CurMoveInput.y * forward + CurMoveInput.x * right + Vector3.up;
		RealMove =(CurMoveInput.y*forward+CurMoveInput.x*right+Vector3.up*CurYVel)*Time.deltaTime*speed;
		CurRun=(CurMoveInput.y * runMult * forward+CurMoveInput.x*right * runMult + Vector3.up*CurYVel)*Time.deltaTime*speed;
	}
    // Update is called once per frame
    void Update()
    {	
		RotHang();
		
		HangJump();
	
		HangGrav();
		
		RelMove();

		

		AnimHang();
		
		if (isRunPressed)
	{			
		charControl.Move(CurRun);
	} else {
		charControl.Move(RealMove); 
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
