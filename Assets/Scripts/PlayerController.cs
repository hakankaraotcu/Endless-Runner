using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerController : MonoBehaviour
{
    public bool jump = false;
    public bool slide = false;

    public Side side = Side.Mid;
    
    private float newXPos = 0f;
    
    [HideInInspector]
    public bool swipeLeft, swipeRight, swipeUp, swipeDown;
    [HideInInspector]
    public bool goLeft, goRight, goUp, goDown;
    [HideInInspector]
    public bool continueToMove;

    public float xValue;
    public float speedDodge;
    public float jumpPower = 7f;
    private float x;
    private float y;
    public float forwardSpeed = 7f;
    public float maxSpeed = 15f;
    private float colHeight;
    private float colCenterY;

    private float x1, x2, y1, y2; 

    public Animator anim;
    private CharacterController controller;

    private float leftBorder, rightBorder;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        colHeight = controller.height;
        colCenterY = controller.center.y;
        transform.position = Vector3.zero;
    }
    
    void Update()
    {   
        if (Input.GetMouseButtonDown(0))
        {
            x1 = Input.mousePosition.x;
            y1 = Input.mousePosition.y;
            continueToMove = true;
        }

        if (Input.GetMouseButton(0) && continueToMove)
        {
            x2 = Input.mousePosition.x;
            y2 = Input.mousePosition.y;
        
            goRight = x2 - x1 > Screen.width / 5;
            goLeft = x2 - x1 < 0 && Mathf.Abs(x2 - x1) > Screen.width / 5;
            goUp = y2 - y1 > Screen.height / 5;
            goDown = y2 - y1 < 0 && Mathf.Abs(y2 - y1) > Screen.height / 5;

            if (goLeft)
            {
                if (side == Side.Mid)
                {
                    newXPos = -xValue;
                    side = Side.Left;
                    leftBorder = -2;
                    rightBorder = 0;
                }
                else if (side == Side.Right)
                {
                    newXPos = 0;
                    side = Side.Mid;
                    leftBorder = 0;
                    rightBorder = -2;
                }
                continueToMove = false;
            }

            else if (goRight)
            {
                if (side == Side.Mid)
                {
                    newXPos = xValue;
                    side = Side.Right;
                    leftBorder = 0;
                    rightBorder = 2;
                }
                else if (side == Side.Left)
                {
                    newXPos = 0;
                    side = Side.Mid;
                    leftBorder = -2;
                    rightBorder = 0;
                }
                continueToMove = false;
            }

            if(goUp)
            {
                JumpMouse();
            }
        }



        swipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        swipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        swipeUp = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        swipeDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);

        // Move horizontally
        if (swipeLeft)
        {
            if(side == Side.Mid)
            {
                newXPos = -xValue;
                side = Side.Left;
            }
            else if(side == Side.Right)
            {
                newXPos = 0;
                side = Side.Mid;
            }
        }
        else if (swipeRight)
        {
            if (side == Side.Mid)
            {
                newXPos = xValue;
                side = Side.Right;
            }
            else if (side == Side.Left)
            {
                newXPos = 0;
                side = Side.Mid;
            }
        }

        // Increase speed
        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;
        }

        // Move forward
        x = Mathf.Clamp(Mathf.Lerp(x, newXPos, Time.deltaTime * speedDodge), leftBorder, rightBorder);
        Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, forwardSpeed * Time.deltaTime);
        controller.Move(moveVector);

        // Jumping
        //Jump();
        JumpMouse();

        // Sliding
        if(swipeDown && !slide)
        {
            //StartCoroutine(Slide());
            StartCoroutine(SlideMouse());
        }
    }
    
    private void Jump()
    {
        if (controller.isGrounded)
        {
            if (swipeUp)
            {
                y = jumpPower;
                jump = true;
                anim.SetBool("isJump", jump);
            }
        }
        else
        {
            y -= jumpPower * 2 * Time.deltaTime;
            jump = false;
            anim.SetBool("isJump", jump);
        }
    }

    private IEnumerator Slide()
    {
        slide = true;
        anim.SetBool("isSlide", slide);
        controller.center = new Vector3(0, colCenterY / 2f, 0);
        controller.height = colHeight / 2f;

        yield return new WaitForSeconds(1.3f);

        controller.center = new Vector3(0, colCenterY, 0);
        controller.height = colHeight;
        slide = false;
        anim.SetBool("isSlide", slide);
    }
    private void JumpMouse()
    {
        if (controller.isGrounded)
        {
            if (goUp)
            {
                y = jumpPower;
                jump = true;
                anim.SetBool("isJump", jump);
            }
        }
        else
        {
            y -= jumpPower * 2 * Time.deltaTime;
            jump = false;
            anim.SetBool("isJump", jump);
        }
    }

    private IEnumerator SlideMouse()
    {
        slide = true;
        anim.SetBool("isSlide", slide);
        controller.center = new Vector3(0, colCenterY / 2f, 0);
        controller.height = colHeight / 2f;

        yield return new WaitForSeconds(1.3f);

        controller.center = new Vector3(0, colCenterY, 0);
        controller.height = colHeight;
        slide = false;
        anim.SetBool("isSlide", slide);
    }
}
