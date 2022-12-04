using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public bool isJumping;
    public bool isSliding;

    public Side side;
    private float newXPos;
    [HideInInspector]
    public bool swipeLeft, swipeRight, swipeUp, swipeDown;
    public float xValue;
    public float speedDodge;
    public float jumpPower;
    public bool skill = false;
    private float x;
    private float y;
    public float forwardSpeed;
    public float maxSpeed;
    private float colHeight;
    private float colCenterY;

    // Power time
    public bool timer = false;

    public Animator anim;
    private CharacterController controller;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        isJumping = false;
        isSliding = false;
        side = Side.Mid;
        newXPos = 0f;
        jumpPower = 7f;
        forwardSpeed = 7f;
        maxSpeed = 15f;
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        colHeight = controller.height;
        colCenterY = controller.center.y;
        transform.position = Vector3.zero;
    }

    public static PlayerController GetInstance()
    {
        return instance;
    }

    private void Update()
    {
        swipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        swipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        swipeUp = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        swipeDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);

        // Move horizontally
        if (swipeLeft)
        {
            if (side == Side.Mid)
            {
                newXPos = -xValue;
                side = Side.Left;
            }
            else if (side == Side.Right)
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
        Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, forwardSpeed * Time.deltaTime);
        x = Mathf.Lerp(x, newXPos, Time.deltaTime * speedDodge);
        controller.Move(moveVector);

        // Jumping
        Jump();

        // Sliding
        if (swipeDown && !isSliding)
        {
            StartCoroutine(Slide());
        }

        Skill();

        // Reset power
        if (timer)
        {
            PlayerManager.GetInstance().powerCount -= Time.deltaTime;
            PowerBar.GetInstance().SetPower(PlayerManager.GetInstance().powerCount);
            if (PlayerManager.GetInstance().powerCount <= 0)
            {
                ResetPower();
            }
        }
    }

    private void Jump()
    {
        if (controller.isGrounded && swipeUp)
        {
            y = jumpPower;
            isJumping = true;
            anim.SetBool("isJump", isJumping);
        }

        else
        {
            y -= jumpPower * 2 * Time.deltaTime;
            isJumping = false;
            anim.SetBool("isJump", isJumping);
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        anim.SetBool("isSlide", isSliding);
        controller.center = new Vector3(0, colCenterY / 2f, 0);
        controller.height = colHeight / 2f;

        yield return new WaitForSeconds(1.3f);

        controller.center = new Vector3(0, colCenterY, 0);
        controller.height = colHeight;
        isSliding = false;
        anim.SetBool("isSlide", isSliding);
    }

    private void Skill()
    {
        if ((PowerBar.GetInstance().slider.value == PowerBar.GetInstance().slider.maxValue) && Input.GetKeyDown(KeyCode.F))
        {
            skill = true;
            timer = true;
        }
    }

    public void ResetPower()
    {
        skill = false;
        timer = false;
        PlayerManager.GetInstance().stopIncrease = false;
        PowerBar.GetInstance().SetPower(0);
        PlayerManager.GetInstance().powerCount = 0;
    }
}