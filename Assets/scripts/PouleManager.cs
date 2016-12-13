using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouleManager : MonoBehaviour {

    public enum Direction { left, right };

    public float jumpCount = 1f;
    public float baseJumpCount = 1f;
    public float jumpForce = 5f;

    public float baseSpeed = .5f;
    public float maxSpeed = 5f;//Replace with your max speed

    public int colliderCount = 0;

    public bool isEatingSomeSeeds = false;
    public bool seekingAStack = true;
    public bool canMove = false;
    public bool onGround = false;
    public bool restored = false;

    public int collisionNotHandled = 0;
    
    public GameObject target;
    private Rigidbody2D rb;
    private Animator anim;
    public Direction dir, oldDir; 

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dir = Direction.left;
        oldDir = dir;
	}

    void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < .5f)
            {
                canMove = false;
                rb.drag = 20f;
            }
            else
            {
                oldDir = dir;
                dir = (transform.position.x - target.transform.position.x > 0f) ? Direction.left : Direction.right;
                canMove = true;
                rb.drag = 0f;
            }
            
        }
        else
        {
            canMove = false;
        }
        if (dir != oldDir)
        {
            rb.velocity = Vector2.down;
            if(target)
            {
                if (Mathf.Abs(target.transform.position.x - transform.position.x) > .5f)
                {
                    if (dir == Direction.left)
                    {
                        transform.localRotation = Quaternion.Euler(0, 0, 0);
                        rb.AddForce(Vector2.left, ForceMode2D.Impulse);
                    }
                    else
                    {
                        transform.localRotation = Quaternion.Euler(0, 180, 0);
                        rb.AddForce(Vector2.right, ForceMode2D.Impulse);
                    }
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update () {

        

        if (GameObject.FindGameObjectsWithTag("graines").Length == 0)
        {
            isEatingSomeSeeds = false;
        }
        if (canMove)
        {
            if (isEatingSomeSeeds)
            {
                rb.velocity.Scale(Vector3.right);
            }
            else
            {
                if (dir == Direction.left)
                {
                    rb.AddForce(Vector2.left * baseSpeed * Time.deltaTime, ForceMode2D.Impulse);
                    transform.parent = null;
                }
                else
                {
                    rb.AddForce(Vector2.right * baseSpeed * Time.deltaTime, ForceMode2D.Impulse);
                    transform.parent = null;
                }
            }
        }
        if (Input.GetButtonDown("Jump") && jumpCount != 0)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            transform.parent = null;
            jumpCount -= 1;
        }
        if(target)
        {
            if (Mathf.Abs(target.transform.position.x - transform.position.x) > .1f)
            {
                transform.localRotation = (dir == Direction.left) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
            }
        }
        anim.SetBool("isEating", isEatingSomeSeeds);
        anim.SetFloat("speed", rb.velocity.magnitude);
        anim.SetBool("onGround", onGround);
    }

    public void setTarget(GameObject target_c)
    {
        target = target_c;
    }

    public bool isSeekingAStack()
    {
        return seekingAStack;
    }

    public void restore()
    {
        if(!restored)
        {
            collisionNotHandled = colliderCount;
            colliderCount = 0;
            seekingAStack = false;
            onGround = false;
            canMove = false;
            isEatingSomeSeeds = false;
            restored = true;
        }
        
    }

    public bool isAlreadyEating()
    {
        return isEatingSomeSeeds;
    }
    
    void OnCollisionEnter2D(Collision2D hit)
    {
        if (!GameObject.Find("Scene Manager").GetComponent<SceneManager>().isloading())
        {
            if (hit.collider.gameObject.tag == "Floor")
            {
                jumpCount = baseJumpCount;
                ++colliderCount;
                onGround = true;
                rb.AddForce(Vector2.up * rb.velocity.magnitude / 2, ForceMode2D.Impulse);
            }
        }
    }

    void OnCollisionExit2D(Collision2D hit)
    {
        if (!GameObject.Find("Scene Manager").GetComponent<SceneManager>().isloading())
        {
            if (hit.collider.gameObject.tag == "Floor" && restored)
            {
                --collisionNotHandled;
                while (collisionNotHandled < 0)
                {
                    ++collisionNotHandled;
                    --colliderCount;
                }
                if (colliderCount == 0)
                    onGround = false;
            }
            else
            if (hit.collider.gameObject.tag == "Floor")
            {
                --colliderCount;

                if (colliderCount == 0)
                    onGround = false;
            }
        }
    }
}
