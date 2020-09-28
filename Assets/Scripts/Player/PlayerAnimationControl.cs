using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class PlayerAnimationControl : MonoBehaviour
{

    public Animator anim;
    public NetworkManager NetManager;
    public AxisControl AxisManager;

    public Joystick joystickControls;

    public string WhatAnim;
    public string OldAnim;

    public float PacketCooldown;

    // Start is called before the first frame update
    void Start()
    {
        AxisManager = GameObject.Find("AxisManager").GetComponent<AxisControl>();
        anim = transform.GetComponent<Animator>();
        NetManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnimationJoystick();
        //.......

        PacketCooldown -= Time.deltaTime;
        if (PacketCooldown <= 0)
        {
            if (OldAnim != WhatAnim)
            {                
                try
                {
                    SendAnimation(WhatAnim);
                    OldAnim = WhatAnim;
                    PacketCooldown = 0.15f;
                }
                catch
                {

                }
            }
        }

    }
    public void SendAnimation(string Direction)
    {
        if (NetManager.Mode)
        {
            NetManager.SendPacket("anim" + Direction);
        }
    }

    private void SwitchAnimationJoystick()
    {
        if (joystickControls.Vertical > joystickControls.DeadZone)
        {
            SwitchAnimation("Up");
        }
        else if (joystickControls.Vertical < -joystickControls.DeadZone)
        {
            SwitchAnimation("Down");
        }
        else if (joystickControls.Horizontal > joystickControls.DeadZone)
        {
            SwitchAnimation("Right");
        }
        else if (joystickControls.Horizontal < -joystickControls.DeadZone)
        {
            SwitchAnimation("Left");
        }
        else
        {
            SwitchAnimation("Idle");
        }
    }

    public void SwitchAnimation(string animation)
    {
        switch (animation)
        {
            case "Idle":
                anim.SetBool("MovingRight", false);
                anim.SetBool("MovingLeft", false);
                anim.SetBool("MovingDown", false);
                anim.SetBool("MovingUp", false);
                WhatAnim = "Idle";
                break;
            case "Up":
                anim.SetBool("MovingUp", true);
                WhatAnim = ("Up");
                anim.SetBool("MovingLeft", false);
                anim.SetBool("MovingDown", false);
                anim.SetBool("MovingRight", false);
                break;
            case "Down":
                anim.SetBool("MovingDown", true);
                WhatAnim = ("Down");
                anim.SetBool("MovingLeft", false);
                anim.SetBool("MovingUp", false);
                anim.SetBool("MovingRight", false);
                break;

            case "Right":
                anim.SetBool("MovingRight", true);
                WhatAnim = ("Right");
                anim.SetBool("MovingLeft", false);
                anim.SetBool("MovingDown", false);
                anim.SetBool("MovingUp", false);
                break;

            case "Left":
                anim.SetBool("MovingLeft", true);
                WhatAnim = ("Left");
                anim.SetBool("MovingUp", false);
                anim.SetBool("MovingDown", false);
                anim.SetBool("MovingRight", false);
                break;
        }
    }
}
