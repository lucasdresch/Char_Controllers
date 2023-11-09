using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerCtrlr : MonoBehaviour
{
    public Animator PlayerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) {
            PlayerAnimator.SetFloat("AnimatorVel", Input.GetAxis("Vertical"));
        } else {
            PlayerAnimator.SetFloat("AnimatorVel", Input.GetAxis("Vertical") / 2.0f);
        }
    }
}
