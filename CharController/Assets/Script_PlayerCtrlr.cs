using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerCtrlr : MonoBehaviour
{
    private Animator PlayerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {



        AnimationsCtrlrKeys();
    }

    void AnimationsCtrlrKeys() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            PlayerAnimator.SetFloat("AnimatorVel", Input.GetAxis("Vertical"));
        } else {
            PlayerAnimator.SetFloat("AnimatorVel", Input.GetAxis("Vertical") / 2.0f);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            PlayerAnimator.SetTrigger("Press");
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            PlayerAnimator.SetTrigger("Bye");
        }

    }
}
