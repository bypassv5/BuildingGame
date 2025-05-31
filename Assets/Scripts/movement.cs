using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class movement : MonoBehaviour
{
    public CharacterController controller;

    private float walkspeed = 5f;
    private float move_input;
    private float turn_input;
    private float vertical_velocity;
    private float gravity = 6.81f;
    private float jumpheight = 0.75f;
    private float vertical_movement()
    {
        if (controller.isGrounded)
        {
            vertical_velocity = -1f;
            if (Input.GetButtonDown("Jump"))
            {
                vertical_velocity = Mathf.Sqrt(jumpheight * gravity);
            }
        }
        else
        {
            vertical_velocity -= gravity * Time.deltaTime;
        }
        return vertical_velocity;
    }
    private void ground_movement()
    {

        Vector3 move = transform.right * turn_input + transform.forward * move_input;

        move.y = vertical_movement();
        move *= walkspeed;
        controller.Move(move * Time.deltaTime);

    }
    private void input_management()
    {
        move_input = Input.GetAxis("Vertical");
        turn_input = Input.GetAxis("Horizontal");

    }


    // Update is called once per frame
    void Update()
    {
        input_management();
        ground_movement();
    }
}
