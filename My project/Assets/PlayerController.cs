using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 direction = Vector2.zero; // used to store users inputs to make player move
    private Rigidbody2D rb; // setting rigidbody variable

    public float max_speed = 5;
    private Vector2 velocity;
    public float acceleration = 1.5f;
    public float friction = 0.75f;
    public float gravity = 0.5f;
    public float max_fall_speed = 9.8f;

    public LayerMask mask;
    private RaycastHit2D left_ground_check;
    private RaycastHit2D right_ground_check;

    private bool jump_check;
    public float jump_force = 15f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // setting rb's meaning  
    }

    // Update is called once per frame
    void Update()
    {    
        if (Input.GetKey(KeyCode.D)) // horizontal movement
        {
            direction.x = 1;
        }

        else if (Input.GetKey(KeyCode.A)) // horizontal movement
        {
            direction.x = -1;
        }
        else // horizontal movement
        {
            direction.x = 0;
        }

        // make the direction vector have a length of one.
        direction = direction.normalized;

        if(Input.GetKey(KeyCode.Space))
        {
            jump_check = true;
        }
        else
        {
            jump_check = false;
        }
    }

    private void FixedUpdate()
    {
        velocity.x += acceleration * direction.x;
        velocity.x = Mathf.Clamp(velocity.x, -max_speed, max_speed);
        rb.MovePosition(rb.position + (velocity * Time.fixedDeltaTime));
        if(direction.x == 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, friction);
        }

        float ground_offset = 0f;
        left_ground_check = Physics2D.Raycast(rb.position + new Vector2(-0.5f, 0), Vector2.down, 1, mask);
        right_ground_check = Physics2D.Raycast(rb.position + new Vector2(0.5f, 0), Vector2.down, 1, mask);

        if (left_ground_check || right_ground_check)
        {
            velocity.y = 0;
            ground_offset = Mathf.Max(left_ground_check.distance, right_ground_check.distance) - 0.5f;
            if(jump_check)
            {
                velocity.y = jump_force;
            }
        }
        else
        {
            velocity.y -= gravity;
            velocity.y = Mathf.Max(velocity.y, -max_fall_speed);
        }
        rb.MovePosition(rb.position + (velocity * Time.fixedDeltaTime) - new Vector2(0, ground_offset));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(GetComponent<Rigidbody2D>().position + new Vector2(-0.5f, 0), Vector2.down);
        Gizmos.DrawRay(GetComponent<Rigidbody2D>().position + new Vector2(0.5f, 0), Vector2.down);
    }
}
