using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossMovment : MonoBehaviour
{
    public GameObject levelCornerL;
    public GameObject levelCornerR;
    public GameObject levelHome;
    private string bossStatus;
    private int bossAttacking;
    private bool CanAttack;
    public bool canDebug;
    public float dashSpeed = 20f; // Speed of the dash
    private Rigidbody2D rb;
    private bool facingRight = true;
    public float dashDuration = 0.75f; // How long the dash lasts
    private bool isDashing = false;

    void Start()
    {
        CanAttack = true;
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Debuger(true, "Rigidbody2D initialized");
        }
        else
        {
            Debuger(true, "ERROR: No Rigidbody2D found!");
        }
    }

    void Update()
    {
        if (isDashing)
        {
            CheckBoundaries();
        }

        if ((bossStatus == null && CanAttack) || (bossStatus == "Idle" && CanAttack))
            StartCoroutine(IdleState());

        if (bossStatus != null && CanAttack && bossStatus != "Idle")
            StartCoroutine(Attacking());
    }

    void CheckBoundaries()
    {
        if (transform.position.x <= levelCornerL.transform.position.x ||
            transform.position.x >= levelCornerR.transform.position.x)
        {
            StopDash();
        }
    }

    void StopDash()
    {
        isDashing = false;
        rb.velocity = Vector2.zero;
        Debuger(canDebug, "Dash stopped at boundary");
    }

    void BossStatSystem(float StateSelector)
    {
        if (StateSelector == 0)
        {
            bossStatus = "Idle";
            bossAttacking = 0;
        }
        else
        {
            StateSelector = Random.Range(0, 3);
            switch (StateSelector)
            {
                case 0:
                    bossStatus = "Dash Attacking";
                    bossAttacking = 1;
                    StartCoroutine(DashAttack());
                    break;
                case 1:
                    bossStatus = "Projectile Attacking";
                    bossAttacking = 2;
                    break;
                case 2:
                    bossStatus = "Jumping Attacking";
                    bossAttacking = 3;
                    break;
            }
        }
        Debuger(canDebug, "Boss Status: " + bossStatus);
    }

    IEnumerator IdleState()
    {
        CanAttack = false;
        yield return new WaitForSeconds(2);
        BossStatSystem(1);
        CanAttack = true;
    }

    IEnumerator Attacking()
    {
        CanAttack = false;
        yield return new WaitForSeconds(2);
        BossStatSystem(0);
        CanAttack = true;
    }

    IEnumerator DashAttack()
    {
        Debuger(canDebug, "Starting Dash Attack");
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 3; i++)
        {
            facingRight = !facingRight;
            isDashing = true;
            Vector2 targetPos = facingRight ?
                new Vector2(levelCornerR.transform.position.x - 0.5f, levelCornerR.transform.position.y) :
                new Vector2(levelCornerL.transform.position.x + 0.5f, levelCornerL.transform.position.y);

            Vector2 velocity = Vector2.zero;
            float dashTime = 0;

            while (dashTime < 3f && Vector2.Distance(rb.position, targetPos) > 0.5f)
            {
                dashTime += Time.fixedDeltaTime;
                rb.velocity = Vector2.SmoothDamp(rb.position, targetPos, ref velocity, dashSpeed * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }
            StopDash();
            Debuger(canDebug, facingRight ? "Right dash complete" : "Left dash complete");
            yield return new WaitForSeconds(1);
        }
        Debuger(canDebug, "Dash Attack Complete");
        StartCoroutine(GoHome());
    }

    IEnumerator GoHome()
    {
        Vector2 velocity = Vector2.zero;

        while ((Vector2)transform.position != (Vector2)levelHome.transform.position)
        {
            rb.position = Vector2.SmoothDamp(rb.position, levelHome.transform.position, ref velocity, dashSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    void Debuger(bool canDebug, string text)
    {
        if (canDebug)
            Debug.Log(text);
    }
}
