using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Movement : MonoBehaviour
{
    public float MoveSpeed = 0;



    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(CrossPlatformInputManager.GetAxisRaw("Horizontal") * MoveSpeed, CrossPlatformInputManager.GetAxisRaw("Vertical") * MoveSpeed);

        GetComponent<Animator>().SetFloat("direction", GetComponent<Rigidbody2D>().velocity.y);

        if (GetComponent<Rigidbody2D>().velocity.x < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else if(GetComponent<Rigidbody2D>().velocity.x > 0)
            GetComponent<SpriteRenderer>().flipX = false;

    }
}
