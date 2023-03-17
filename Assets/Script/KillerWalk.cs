using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerWalk : MonoBehaviour
{
    public Rigidbody2D rb;
    public float velocity;
    public float startVelocity;
    private PlayerControls player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startVelocity = velocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(velocity, rb.velocity.y);
    }
}
