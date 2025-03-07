using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class Warrior : PlayerCtrl
{
    private Rig aimRig;
    private int comboStep = 0;
    private float lastClickTime = 0f;
    private float comboDelay = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void Update()
    {

        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            HandleCombo();
        }

        if (Time.time - lastClickTime > comboDelay)
        {
            ResetCombo();
        }

        if (Input.GetMouseButton(1))
        {
            animator.SetBool("Shield", true);
        }
        else
        {
            animator.SetBool("Shield", false);
        }
    }
    // Update is called once per frame
    protected override void Awake()
    {
        base.Awake();
    }
    private void HandleCombo()
    {
        lastClickTime = Time.time;

        if (comboStep == 0)
        {
            animator.SetBool("Combo1", true);
            comboStep = 1;
        }
        else if (comboStep == 1 && animator.GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack01"))
        {
            animator.SetBool("Combo2", true);
            comboStep = 2;
            animator.SetBool("Combo1", false);
        }
        else if (comboStep == 2 && animator.GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack03"))
        {
            animator.SetBool("Combo3", true);
            comboStep = 3;
            animator.SetBool("Combo2", false);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("RightHand@Attack04"))
        {
            comboStep = 0;
            animator.SetBool("Combo3", false);
        }
    }

    public void ResetCombo()
    {
        comboStep = 0;
        animator.SetBool("Combo1", false);
        animator.SetBool("Combo2", false);
        animator.SetBool("Combo3", false);
    }
}
