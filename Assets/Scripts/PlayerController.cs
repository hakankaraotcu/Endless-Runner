using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public enum HitX { Left, Mid, Right, None}
public enum HitY { Up, Mid, Down, Low, None }
public enum HitZ { Forward, Mid, Backward, None }


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

    // Stumble
    public HitX hitX = HitX.None;
    public HitY hitY = HitY.None;
    public HitZ hitZ = HitZ.None;

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
        //forwardSpeed = 0f;
        maxSpeed = 15f;
        anim = GetComponentInChildren<Animator>();
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
        /*if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;
        }*/

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

        // Active skill
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

    public void OnCharacterColliderHit(Collider col)
    {
        hitX = GetHitX(col);
        hitY = GetHitY(col);
        hitZ = GetHitZ(col);
        
        if(hitZ == HitZ.Forward && hitX == HitX.Mid)
        {
            if(hitY == HitY.Mid)
            {
                // Stumble Low
            }
            else if(hitY == HitY.Down)
            {
                // Death Lower
            }
            else if(hitY == HitY.Mid)
            {
                if(col.tag == "MovingObject")
                {
                    // Death Moving Object
                }
                else if(col.tag != "Ramp")
                {
                    // Death Bounce
                }
            }
            else if(hitY == HitY.Up && !isSliding)
            {
                // Death Upper
            }
        }
        else if(hitZ == HitZ.Mid)
        {
            if(hitX == HitX.Right)
            {
                // Stumble Side Right
            }
            else if(hitX == HitX.Left)
            {
                // Stumble Side Left
            }
        }
        else
        {
            if(hitX == HitX.Right)
            {
                // Stumble Corner Right
            }
            else if(hitX == HitX.Left)
            {
                // Stumble Corner Left
            }
        }
    }

    public HitX GetHitX(Collider col)
    {
        Bounds char_bounds = controller.bounds;
        Bounds col_bounds = col.bounds;
        float min_x = Mathf.Max(col_bounds.min.x, char_bounds.min.x);
        float max_x = Mathf.Min(col_bounds.max.x, char_bounds.max.x);
        float average = (min_x + max_x) / 2f - col_bounds.min.x;

        HitX hit;

        if(average > col_bounds.size.x - 0.33f)
        {
            hit = HitX.Right;
        }
        else if(average < 0.33f)
        {
            hit = HitX.Left;
        }
        else
        {
            hit = HitX.Mid;
        }
        return hit;
    }

    public HitY GetHitY(Collider col)
    {
        Bounds char_bounds = controller.bounds;
        Bounds col_bounds = col.bounds;
        float min_y = Mathf.Max(col_bounds.min.y, char_bounds.min.y);
        float max_y = Mathf.Min(col_bounds.max.y, char_bounds.max.y);
        float average = ((min_y + max_y) / 2f - char_bounds.min.y) / char_bounds.size.y;

        HitY hit;

        if (average < 0.17f)
        {
            hit = HitY.Low;
        }

        else if (average < 0.33f)
        {
            hit = HitY.Down;
        }
        else if (average < 0.66f)
        {
            hit = HitY.Mid;
        }
        else
        {
            hit = HitY.Up;
        }
        return hit;
    }

    public HitZ GetHitZ(Collider col)
    {
        Bounds char_bounds = controller.bounds;
        Bounds col_bounds = col.bounds;
        float min_z = Mathf.Max(col_bounds.min.z, char_bounds.min.z);
        float max_z = Mathf.Min(col_bounds.max.z, char_bounds.max.z);
        float average = ((min_z + max_z) / 2f - char_bounds.min.z) / char_bounds.size.z;

        HitZ hit;

        if (average < 0.33f)
        {
            hit = HitZ.Backward;
        }
        else if (average < 0.66f)
        {
            hit = HitZ.Mid;
        }
        else
        {
            hit = HitZ.Forward;
        }
        return hit;
    }
}