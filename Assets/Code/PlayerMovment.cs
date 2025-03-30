using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public GameObject Hitregg;
    Rigidbody2D rb;

    [Header("Player Movement")]
    public float speed = 10.0f;
    public float movementDrag = 0.5f;
    [Space]
    public float jumpForce = 900.0f;
    public float jumpDrag = 0.5f;
    [Space]
    public float dashForce = 20.0f;
    public float dashingCooldown = 0.5f;
    [Space]
    public float attackCooldown = 0.5f;
    public float attackSpeed = 5.0f;
    public float attackDamige = 5.0f;
    [Space]
    public float raycastDistance = 0.1f;
    public float Gravity = 9.8f;
    [Space]
    public Vector2 raycastOffset;
    
    private float inputX;
    private bool inputY;
    private bool dashInput;
    private bool mouseInput;

    private bool canAttack;
    private bool isGrounded;
    private bool canDash = true;
    private float currentSpeed;
    private Vector2 raycastOrigin;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canAttack = true;
        canDash = true;
    }
    // Update is called once per frame
    void Update()
    {
        MovementSystem();
        flip();

        if (mouseInput && !isGrounded && inputY)
            attack();

        animator.SetBool("Jump", !isGrounded);
        animator.SetFloat("Speed", rb.velocity.x);
        animator.SetBool("Attack", mouseInput);
    }
    private void FixedUpdate()
    {
        if (!isGrounded)
            rb.AddForce(-transform.up * Gravity * Time.deltaTime * jumpDrag, ForceMode2D.Force);
        else
            rb.AddForce(-transform.up * Gravity * Time.deltaTime, ForceMode2D.Force);
        rb.gravityScale = Gravity / 2   ;
    }
    void InputManager() 
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetKeyDown(KeyCode.Space);
        mouseInput = Input.GetMouseButtonDown(0);
        dashInput = Input.GetKeyDown(KeyCode.LeftShift);
    }
    void MovementSystem()
    {
        InputManager();

        if (mouseInput)
            StartCoroutine(Attacking());
        if (dashInput && canDash)
            dashMichanic();

            rb.velocity = new Vector2(inputX * speed * movementDrag, rb.velocity.y);

        raycastOrigin = new Vector2(transform.position.x + raycastOffset.x, transform.position.y + raycastOffset.y);
        isGrounded = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistance, LayerMask.GetMask("Ground"));
        if (isGrounded && inputY && !mouseInput)
            JumpingMechanic();
    }
    void JumpingMechanic()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    void flip()
    {
        if (inputX > 0)
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        else if (inputX < 0)
            transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);
    }
    void attack()
    {
        AttackingCollodwn();
    }

    void dashMichanic() // fix dash direction
    {
        if (inputX > 0)
            rb.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
        else if (inputX < 0)
            rb.AddForce(-transform.right * dashForce, ForceMode2D.Impulse);
        canDash = false;
        StartCoroutine(DashCooldown());
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 raycastStart = Application.isPlaying ? raycastOrigin : new Vector2(transform.position.x + raycastOffset.x, transform.position.y + raycastOffset.y);
        Gizmos.DrawLine(raycastStart, new Vector2(raycastStart.x, raycastStart.y - raycastDistance));
    }
    IEnumerator Attacking()
    {
        currentSpeed = 2 * inputX;
        transform.Translate(new Vector3(currentSpeed * Time.deltaTime, 0, 0));
        yield return new WaitForSeconds(1f);
    }
    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    IEnumerator AttackingCollodwn()
    {
        canAttack = false;
        yield return new WaitForSeconds(dashingCooldown);
        canAttack = true;
    }
}