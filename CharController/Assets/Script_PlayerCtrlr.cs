using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerCtrlr : MonoBehaviour
{
    private Animator PlayerAnimator;
    private CharacterController CharCtrlr;
    public float VelWalk;
    public float VelRun;
    private float VelocityCtrlr;
    public float MoveForward;
    public float InputForward;
    public float InputRotate;
    private Vector3 PlayerMovement;

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

    }

    void InputMovements() {
        InputForward = Input.GetAxis("Vertical");
        InputRotate = Input.GetAxis("Horizontal");
        
    }

    void AnimationsCtrlrKeys() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            PlayerAnimator.SetFloat("AnimatorVel", InputForward);
            VelocityCtrlr = VelRun;
        } else {
            PlayerAnimator.SetFloat("AnimatorVel", InputForward / 2.0f);
            VelocityCtrlr = VelWalk;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            PlayerAnimator.SetTrigger("Press");
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            PlayerAnimator.SetTrigger("Bye");
        }

    }
}
