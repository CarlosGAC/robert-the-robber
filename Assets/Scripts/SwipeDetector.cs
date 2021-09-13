using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwipeDetector : MonoBehaviour
{
    public enum SwipeDirection { UP, DOWN, LEFT, RIGHT, TAP };

    [Header("Position Vectors")]
    [SerializeField] private Vector3 firstpos;   //Starting position of the touch
    [SerializeField] private Vector3 lastpos;   //Ending Positition of the touch

    [Header("Distance for a drag to be considered a swipe")]
    [SerializeField] private float dragDistance;  //Minimum distance for a drag to be considered a swipe

    [Header("Percentage of the screen to drag for it to be considered a swipe")]
    [Range(5, 13)]
    [SerializeField]
    private float percentage = 9.65f;

    [SerializeField]
    public SwipeDirection swipeDirection;

    public UnityEvent<SwipeDirection> OnSwipeDetected;
    void Start()
    {
        dragDistance = Screen.height * percentage / 100; //dragDistance is the x% of the screen to drag for it to be considered a swipe
    }

    void Update()
    {
        SwipeDetection();
    }//update ends

    public void SwipeDetection()
    {
        if (Input.touchCount == 1) // User first press on the screen
        {
            Touch touch = Input.GetTouch(0); // Gets the touch
            if (touch.phase == TouchPhase.Began) //Stores the initial position of the touch
            {
                firstpos = touch.position;
                lastpos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // Updates de lp vector to the position of the touch
            {
                lastpos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //checks if the swipe has ended (if the finger was lifted)
            {
                lastpos = touch.position;  //Last position of the touch

                Debug.Log("Aqui andamos ptos");

                SwipeDirection swipeDirection = DetermineSwipeDirection(firstpos, lastpos);
                OnSwipeDetected.Invoke(swipeDirection);
            }
        }
    }//end

    private SwipeDirection DetermineSwipeDirection(Vector3 firstPos, Vector3 lastPos)
    {
        //checks if the swipe distance is greater than the threshold so as to not register accidental swipes
        if (Mathf.Abs(lastpos.x - firstpos.x) > dragDistance || Mathf.Abs(lastpos.y - firstpos.y) > dragDistance)
        {//if it is a Swipe....
         //Checks whether it was a horizontal or vertical swipe
            if (Mathf.Abs(lastpos.x - firstpos.x) > Mathf.Abs(lastpos.y - firstpos.y))
            {   //if horizontal movement is greater than vertical movement.....
                if ((lastpos.x > firstpos.x))  //checks whether its a left or right swipe
                {   //Swipe to the Right
                    Debug.Log("Right Swipe");
                    return SwipeDirection.RIGHT;
                }
                else
                {   //Swipe to the left
                    Debug.Log("Left Swipe");
                    return SwipeDirection.LEFT;
                }
            }
            else
            {   //in case vertical movement is greater than horizontal movement
                if (lastpos.y > firstpos.y)  //checks wheter its a upwards or downwards swipe
                {   //Upwards Swipe
                    Debug.Log("Up Swipe");
                    return SwipeDirection.UP;
                }
                else
                {   //Downwards Swipe
                    Debug.Log("Down Swipe");
                    return SwipeDirection.DOWN;
                }
            }
        }
        else
        {   // the drag distance was lower than the threshold, therefore is considered a tap on the screen.
            Debug.Log("Tap");
            return SwipeDirection.TAP;
        }
    }
}