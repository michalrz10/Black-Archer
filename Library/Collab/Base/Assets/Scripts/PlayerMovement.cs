using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public GameControls controls;
    public Animator animator;
    public CinemachineVirtualCamera cam;
    public GameObject followTransform;
    public float speed = 6f;
    public float rotationPower = 1f;
    public float turnSmoothTime = 0.1f;
    public float jumpVelocity = 10f;
    private CharacterController controller;

    Quaternion nextRotation;
    Vector2 movementInput;
    Vector3 moveDir;

    private bool _aimFlag;
    public int aimSpeed;


    public void Awake()
    {
        controls = new GameControls();
        controls.Player.Enable();
        controls.Player.Movement.started += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Player.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Player.Movement.canceled += ctx => Move(new Vector2(0,0));
        controls.Player.Movement.canceled += ctx => AnimationStopMovement();
        controls.Player.Jump.performed += _ => Jump();
        controls.Player.Shoot.performed += _ => Shoot();
        controls.Player.Aim.started += _ => Aim();
        controls.Player.Aim.canceled += _ => AimDown();
        jumpVelocity = jumpVelocity / 100f;

    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        controller.detectCollisions = true;
        movementInput = new Vector2(0, 0);
        moveDir = Vector3.zero;
    }

    private void AnimationStopMovement()
    {
        animator.SetBool("Run", false);
    }

    private void Move(Vector2 vector)
    {
        Debug.Log(vector);
        movementInput = vector;
        animator.SetBool("Run", true);
    }

    void Jump()
    {
        animator.SetBool("Jump", true);
        if (controller.isGrounded)
        { 
            
            Debug.Log("PIKA BU!");
            moveDir.y = jumpVelocity;
        }
    }

    void Shoot()
    {
        //Debug.Log("Shoot!@");
        if(_aimFlag)
            animator.SetBool("Shoot", true);
    }

    void Aim()
    {
        _aimFlag = true;
        animator.SetBool("Aim", true);

    }
    void AimDown()
    {
        _aimFlag = false;
        animator.SetBool("Aim", false);
        GetComponentInChildren<Bow>().ConcelAiming();
    }
    
    void aimAnimation()
    {
        //Debug.Log(_aimFlag);
        if(_aimFlag == false && cam.m_Lens.FieldOfView < 60)
        {
            float smoothSpeed = Mathf.Sin(Mathf.Abs(((cam.m_Lens.FieldOfView - 60)) / 30) * Mathf.PI);
            cam.m_Lens.FieldOfView += smoothSpeed * aimSpeed;
        }
        if(_aimFlag == true && cam.m_Lens.FieldOfView > 30)
        {
            float smoothSpeed = Mathf.Sin(Mathf.Abs((1 -(cam.m_Lens.FieldOfView - 60)) / 30) * Mathf.PI);
            
            cam.m_Lens.FieldOfView -= smoothSpeed * aimSpeed;
        }
    }
    void Update()
    {
        
        if(controller.isGrounded)
        {
            animator.SetBool("Jump", false);
        }

        aimAnimation();
        Vector2 mouse = controls.Player.MousePosition.ReadValue<Vector2>();
        followTransform.transform.rotation *= Quaternion.AngleAxis(mouse.x * rotationPower, Vector3.up);
        followTransform.transform.rotation *= Quaternion.AngleAxis(-mouse.y * rotationPower, Vector3.right);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        if (angle > 180 && angle < 320)
        {
            angles.x = 320;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }


        followTransform.transform.localEulerAngles = angles;
        nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime);

        float moveSpeed = speed / 100f;
        Vector3 position = (transform.forward * movementInput.y * moveSpeed) + (transform.right * movementInput.x * moveSpeed) + (transform.up * moveDir.y);
        moveDir = position;
        moveDir.y -= 0.002f;

        if (movementInput.x == 0 && movementInput.y == 0)
        {
            
            if (_aimFlag)
            {
                transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
                followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            }
            
            controller.Move(transform.up * moveDir.y);
            ///return;
        }
        else { 
            transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
            followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            controller.Move(moveDir);
        }


        
        

    }
}
