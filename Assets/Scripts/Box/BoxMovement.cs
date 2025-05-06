using UnityEngine;
using System.Collections;

public class BoxMovement : MonoBehaviour
{
    
    public bool beingPushed;
    float xPos;

    public Vector3 lastPos;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3f; // Ensure gravity is applied
        xPos = transform.position.x;
        lastPos = transform.position;
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (beingPushed == false)
        {
            // Freeze only rotation, not position
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            // Unfreeze all constraints except rotation
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }

      
    }
    
    

}