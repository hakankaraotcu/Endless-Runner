using System.Collections;
using UnityEngine;

[System.Serializable]

public class PlayerTouchController : MonoBehaviour
{
    private bool isJumping;
    private bool isSliding;

    public Side side;

    private float newXPos;

    private bool swipeLeft, swipeRight, swipeUp, swipeDown;
    private float x;
    private float y;
    private float colHeight;
    private float colCenterY;
    private bool continueToMove;
    public float xValue;
    public float speedDodge;
    public float jumpPower;
    public float forwardSpeed;
    public float maxSpeed;

    private float x1, x2, y1, y2;

    public Animator anim;
    private CharacterController controller;


    private void Start()
    {
        isJumping = false;
        isSliding = false;
        side = Side.Mid;
        newXPos = 0f;
        jumpPower = 7f;
        forwardSpeed = 7f;
        maxSpeed = 15f;
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        colHeight = controller.height;
        colCenterY = controller.center.y;
        transform.position = Vector3.zero;
    }

    private void Update()
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

            swipeRight = x2 - x1 > Screen.width / 5;
            swipeLeft = x2 - x1 < 0 && Mathf.Abs(x2 - x1) > Screen.width / 5;
            swipeUp = y2 - y1 > Screen.height / 5;
            swipeDown = y2 - y1 < 0 && Mathf.Abs(y2 - y1) > Screen.height / 5;

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
                continueToMove = false;
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
                continueToMove = false;
            }

            if (swipeUp)
            {
                continueToMove = false;
            }

            if (swipeDown && !isSliding)
            {
                StartCoroutine(Slide());
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

        Jump();

    }

    private void Jump()
    {
        if (controller.isGrounded && swipeUp)
        {
            swipeUp = false;
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
}
