using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerCtrlr : MonoBehaviour
{
    private Animator PlayerAnimator;
    private CharacterController CharCtrlr;
    public float VelWalk;
    public float VelRun;
    public float VelRot;
    public float Jump;
    public float Bounce;

    private float VelocityCtrlr;
    private float InputForward;
    private float InputRotate;
    private Vector3 PlayerMovement, PlayerRotation, VectorJump;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        CharCtrlr = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputMovements();
        AnimationsCtrlrKeys();
        PlayerMovement = InputForward * transform.TransformDirection(Vector3.forward) * VelocityCtrlr;
        CharCtrlr.Move(PlayerMovement * Time.deltaTime);
        transform.Rotate(new Vector3(0, InputRotate * VelRot * Time.deltaTime, 0));

        CharCtrlr.SimpleMove(Physics.gravity);
        InputJump();
    }
    
    void InputJump() {
        if (Input.GetButton("Jump")) {
            if (CharCtrlr.isGrounded) {
                VectorJump.y = Jump;
            }
        }
        CharCtrlr.Move(VectorJump * Time.deltaTime);
        VectorJump.y -= Bounce * Time.deltaTime;

    }

    void InputMovements() {
        InputForward = Input.GetAxis("Vertical");
        InputRotate = Input.GetAxis("Horizontal");
        PlayerRotation.y = InputRotate;
    }

    void AnimationsCtrlrKeys() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            PlayerAnimator.SetFloat("AnimatorVel", InputForward);
            VelocityCtrlr = VelRun;
        } else {
            PlayerAnimator.SetFloat("AnimatorVel", InputForward / 2.0f);
            VelocityCtrlr = VelWalk;
        }
        if (Input.GetButton("Jump")) {
            PlayerAnimator.SetInteger("Jump", 2);
        } else {
            PlayerAnimator.SetInteger("Jump", 1);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            PlayerAnimator.SetTrigger("Press");
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            PlayerAnimator.SetTrigger("Bye");
        }

    }
}
