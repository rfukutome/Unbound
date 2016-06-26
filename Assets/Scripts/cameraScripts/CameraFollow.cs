using UnityEngine;
using System.Collections;

/////////////////////////////////////////////////////////////////////////
//   Class:      CameraFollow
//   Purpose:    To keep the focus of the camera on the target, within 
//   a given area. Smoothing can be specifed, along with the focus area.
//   
//   Notes: Attach onto a camera, and drag a reference to the player. 
//   Contributors: RSF
/////////////////////////////////////////////////////////////////////////    


public class CameraFollow : MonoBehaviour {
    public PlayerMovement target;

    //Offset variables for the Camerafollow box area
    public float verticalOffset;
    public float lookAheadDstX;

    //How quickly the camera will follow the player once the
    //player leaves the camera region.
    public float lookSmoothTimeX;
    public float verticalSmoothTime;
    public Vector2 focusAreaSize;

    FocusArea focusArea;

    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirX;
    float smoothLookVelocityX;
    float smoothVelocityY;

    bool lookAheadStopped;

    void Start()
    {
        focusArea = new FocusArea(target.collider.bounds, focusAreaSize);
    }

    void LateUpdate()
    {
        focusArea.Update(target.collider.bounds);

        Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

        if(focusArea.velocity.x != 0)
        {
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
            if(Mathf.Sign(target.playerInput.x) == Mathf.Sign(focusArea.velocity.x) && target.playerInput.x != 0)
            {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirX * lookAheadDstX;
            }
            else
            {
                if (!lookAheadStopped)
                {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4f;
                }
            }

            focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
            
        }

        targetLookAheadX = lookAheadDirX * lookAheadDstX;
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

        focusPosition += Vector2.right * currentLookAheadX;
        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }
    struct FocusArea
    {
        public Vector2 center;
        public Vector2 velocity;
        float left, right;
        float top, bottom;
        
        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds argTargetBounds)
        {
            float shiftX = 0;
            if(argTargetBounds.min.x < left)
            {
                shiftX = argTargetBounds.min.x - left;
            }else if(argTargetBounds.max.x > right)
            {
                shiftX = argTargetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (argTargetBounds.min.y < bottom)
            {
                shiftY = argTargetBounds.min.y - bottom;
            }
            else if (argTargetBounds.max.y > top)
            {
                shiftY = argTargetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right)/ 2, (top+bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
