using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////
//   Class:      Player
//   Purpose:    This class holds all the properties of the Player. The Player
//   script handles many player states. This includes jumping, animations, 
//   wall climbing interactions.
//
//   Notes: Attach onto the player
//   Contributors: RSF
///////////////////////////////////////////////////////////////////////////

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour {
    [Header("Jump Information")]
    [SerializeField]
    float maxJumpHeight = 4;
    [SerializeField]
    float minJumpHeight = 1;
    [SerializeField]
    float timeToJumpApex = .5f;

    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    [Header("Wall Jump Info")]
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    //The speed at which you slide down the wall
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    [Header("Movement")]
    [SerializeField]
    float moveSpeed = 6;

    //Animator
    private ArmControl arm;
    private WeaponBasics weap;
    private Animator anim;

    bool facingRight;

    public bool isBusy;
    //Gravity and jumpvelocity are determined by jumpheight and timeToJumpApex
    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;

    Vector3 velocity;
    float velocityXSmoothing;

    PlayerMovement controller;

    void Start()
    {
        facingRight = true;
        isBusy = false;
        controller = GetComponent<PlayerMovement>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        
        //Get components of Player;
        anim = GetComponentInChildren<Animator>();    //gets the animator component in same obj
        arm = GetComponentInChildren<ArmControl>();
        weap = GetComponentInChildren<WeaponBasics>();
    }

    void Update()
    {
        if (isBusy)
        {
            anim.SetFloat("Speed", 0);
            //If the player is in the air, let the animator know
            anim.SetBool("Jump", false);
            return;
        }
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        //WALL SLIDING LOGIC
        bool wallSliding = false;
        if((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;
            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }
            //Sticking to the wall;
            if (timeToWallUnstick > 0)
            {
                velocity.x = 0;
                velocityXSmoothing = 0;

                if(input.x != wallDirX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }

        //JUMPING LOGIC
        if (Input.GetButtonDown("Jump"))
        {
            //Wall jumping logic
            if (wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if(input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            //This is a regular jump
            if (controller.collisions.below)
            {
                velocity.y = maxJumpVelocity;
            }   
        }
        //JUMPING QUICK RELEASE
        if (Input.GetButtonUp("Jump"))
        {
            if(velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);
        //ARM ROTATION

        //If the arm rotation is facing the right side
        if (arm.rotZ < 90 && arm.rotZ > -90)
        {
            if (!facingRight)
            {
                FlipSprite();
            }
        }
        //Arm rotation is facing to the left side
        else
        {
            if (facingRight)
            {
                FlipSprite();
            }
        }
        //ANIMATOR STATES//
        //Let the animator know that the player is moving horizontally
        anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        //If the player is in the air, let the animator know
        anim.SetBool("Jump", (controller.collisions.below) ? false : true);

        //If there is a collision above or below, stop the player
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);

    }

    //===============================================================================
    //Function: FlipSprite()
    //Purpose: When the player's orientation (right or left) changes Flip is used to
    //  change the direction they are facing along with the arm, and gun flipping.
    //===============================================================================
    void FlipSprite()
    {
        facingRight = !facingRight;
        arm.MirrorArm();
        weap.setFacingRight(facingRight);
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
