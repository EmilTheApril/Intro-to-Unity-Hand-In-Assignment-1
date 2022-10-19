using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    public float jumpForce;
    public bool isGrounded;
    public bool isWallRiding;
    public int jumps;
    private Rigidbody rb;
    private GameObject wall;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //WallRun();
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
        Jump();
    }

    public void WallRun()
    {
        if (isWallRiding)
        {
            isGrounded = true;
            transform.rotation = wall.transform.rotation;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }

    public void Move()
    {
        Vector3 moveInputForward = transform.forward * Input.GetAxis("Vertical");

        if (Input.GetAxisRaw("Vertical") == 0)
        {
            moveInputForward = transform.forward * 0;
        }

        rb.velocity = new Vector3(moveInputForward.x * speed * Time.deltaTime, rb.velocity.y, moveInputForward.z * speed * Time.deltaTime);
    }

    public void Rotate()   
    {
        Vector3 moveInputRotate = Vector3.zero;

        if (Input.GetAxisRaw("Vertical") >= 0)
        {
            moveInputRotate = new Vector3(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        } else
        {
            moveInputRotate = new Vector3(0, -Input.GetAxis("Horizontal") * rotateSpeed, 0);
        }

        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            moveInputRotate = Vector3.zero;
        }

        Quaternion deltaRotation = Quaternion.Euler(moveInputRotate * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    public void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (other.transform.CompareTag("Wall"))
        {
            isWallRiding = true;
            wall = other.gameObject;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("MoreJump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce * 1.5f, ForceMode.Impulse);
            Destroy(other.gameObject);
        }
    }

    public void OnCollisionExit(Collision other)
    {
        isWallRiding = false;
        wall = null;
    }
}
