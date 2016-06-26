using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////
//   Class:      PlayerMovement
//   Purpose:    This class handles all of the players movement in world-
//   space. This class inherits from the RayCastController, which handles
//   all the player movements using Raycasts. This class acts as a higher
//   level controller, that takes player inputs for movement. It then 
//   pulls from the raycastcontroller class. Collisions are all handled in
//   this class. 
//
//   Notes: Attach onto the player
//   Contributors: RSF
///////////////////////////////////////////////////////////////////////////

public class PlayerMovement : RaycastController {

    public int testfaceDir;
    [Header("Wall Angles")]
    float maxClimbAngle = 80f;
    float maxDescendAngle = 75f;

    public CollisionInfo collisions;
    public Vector2 playerInput;
    
    public override void Start()
    {
        base.Start();

        //default to looking right;
        collisions.faceDir = 1;
    }

    public void Move(Vector3 argVelocity, bool argStandingOnPlatform)
    {
        Move(argVelocity, Vector2.zero, argStandingOnPlatform);
    }

    public void Move(Vector3 argVelocity, Vector2 argInput, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = argVelocity;
        playerInput = argInput;

        if(argVelocity.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(argVelocity.x);
            testfaceDir = collisions.faceDir;
        }
        if(argVelocity.y < 0)
        {
            DescendSlope(ref argVelocity);
        }
        //if(argVelocity.x != 0)
        //{
        HorizontalCollisions(ref argVelocity);
        //}
        if(argVelocity.y != 0)
        {
            VerticalCollisions(ref argVelocity);
        }
        
        transform.Translate(argVelocity);

        if (standingOnPlatform)
        {
            collisions.below = true;
        }
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if(Mathf.Abs(velocity.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);


            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            //If the raycast hits something;
            if (hit)
            {
                if(hit.distance == 0)
                {
                    continue;
                }
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(i ==0 && slopeAngle <= maxClimbAngle)
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if(slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);

                    velocity.x += distanceToSlopeStart * directionX;
                }

                if(!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }


    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
             
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
            //If the raycast hits something;
            if (hit)
            {
                if(hit.collider.tag == "throughPlatform")
                {
                    if(directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if (collisions.fallingThroughPlatform)
                    {
                        continue;
                    }
                    if (playerInput.y == -1)
                    {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", .5f);
                        continue;
                    }
                }
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / (Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x));
                }
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up*velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 argVelocity, float argSlopeAngle)
    {
        float moveDistance = Mathf.Abs(argVelocity.x);
        float climbVelocityY = Mathf.Sin(argSlopeAngle * Mathf.Deg2Rad) * moveDistance;

        if(argVelocity.y <= climbVelocityY)
        {
            argVelocity.y = climbVelocityY;
            argVelocity.x = Mathf.Cos(argSlopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(argVelocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = argSlopeAngle;
        }
    }

    void DescendSlope(ref Vector3 argVelocity)
    {
        float directionX = Mathf.Sign(argVelocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(argVelocity.x)){
                        float moveDistance = Mathf.Abs(argVelocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        argVelocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(argVelocity.x);
                        argVelocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }

    public struct CollisionInfo
    {
        //true if colliding
        public bool above, below;
        //true if colliding
        public bool left, right;
        public bool climbingSlope, descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;

        //-1 for left and 1 for right;
        public int faceDir;
        public bool fallingThroughPlatform;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0f;
        }
    }

}
