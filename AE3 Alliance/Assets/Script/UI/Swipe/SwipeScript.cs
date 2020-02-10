using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwipeScript : MonoBehaviour {

    private Vector3 FirstTouchPos;   //First touch position
    private Vector3 LastTouchPos;   //Last touch position
    public int ScreenPercentageForSwipe;
    private float dragDistance;  //minimum distance for a swipe to be registered
    public GameObject Player;

    Sets Set;

    void Start()
    {
        Set = gameObject.GetComponent<Sets>();
        dragDistance = Screen.height * ScreenPercentageForSwipe / 100; //dragDistance is N% height of the screen        
    }

    void Update()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                FirstTouchPos = touch.position;
                LastTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                LastTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                LastTouchPos = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than N% of the screen height
                //It's a drag
                if (Mathf.Abs(LastTouchPos.x - FirstTouchPos.x) > dragDistance || Mathf.Abs(LastTouchPos.y - FirstTouchPos.y) > dragDistance)
                {
                    //check if the drag is vertical or horizontal
                    if (Player.GetComponent<Stats>().Combat)
                        if (Mathf.Abs(LastTouchPos.x - FirstTouchPos.x) > Mathf.Abs(LastTouchPos.y - FirstTouchPos.y))
                    {

                        //If the horizontal movement is greater than the vertical movement...

                        if ((LastTouchPos.x > FirstTouchPos.x))  //If the movement was to the right)
                        {
                            //Right swipe
                            
                            Set.ActivateSet1();

                        }
                        else
                        {
                            //Left swipe
                            Set.ActivateSet2();

                        }
                    }
                    else
                    {
                        //the vertical movement is greater than the horizontal movement
                        if (LastTouchPos.y > FirstTouchPos.y)  //If the movement was up
                        {
                            //Up swipe
                            Set.ActivateSet3();

                        }
                        else
                        {
                            //Down swipe
                            Set.ActivateSet4();

                        }
                    }
                }
                else
                {
                    //It's a tap as the drag distance is less than N% of the screen height          

                    
                }
            }
        }
    }
}
