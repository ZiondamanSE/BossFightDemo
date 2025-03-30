using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovment : MonoBehaviour
{
    public BossHelth bossHelth;
    public GameObject player;

    public float speed = 10.0f;

    private bool Pahse1 = true;
    private bool Pahse2 = false;
    private bool canAttack = false;


    // Start is called before the first frame update
    void Start()
    {
        bossHelth = GetComponent<BossHelth>();
        if (bossHelth == null)
        {
            Debug.LogError("BossHelth is null");
        }
        if (player == null)
        {
            Debug.LogError("Player is null");
        }

        InvokeRepeating("ChekingAttackState", 5, 4);
    }

    // Update is called once per frame
    void Update()
    {
        pahseSequens();
        MovmentSpeed();
    }

    void pahseSequens()
    {
        if(bossHelth.cunrentHelth <= 50)
        {
            Pahse1 = false;
        }
        if (bossHelth.cunrentHelth <= 30)
        {
            Pahse2 = true;
            speed = speed * 2;
        }
        if (bossHelth.cunrentHelth <= 10)
        {
            Pahse2 = false;
            speed = speed * 3;
        }
    }

    void MovmentSpeed()
    {   
        float currentSpeed;
        if (bossHelth.rage != 0)
            currentSpeed = speed * Time.deltaTime * bossHelth.rage;
        else
            currentSpeed = speed * Time.deltaTime;
    }

    void ChekingAttackState()
    {
        if (randomiser() == 0)
        {
            StartCoroutine(JumpAttack());
        }
        else if (randomiser() == 1)
        {
            StartCoroutine(DashAttack());
            
        }
        else if (randomiser() == 2)
        {
            StartCoroutine(ThouAttack());
        }
    }

    private float randomiser()
    {
        float random = Random.Range(0, 3);
        return random;
    }

    IEnumerator JumpAttack()
    {
        Debug.Log("Jump Attacking");
        canAttack = false;
        yield return new WaitForSeconds(1.0f); // attack with cooldown
        canAttack = true;
    }
    IEnumerator DashAttack()
    {
        Debug.Log("Dash Attack");
        canAttack = false;
        yield return new WaitForSeconds(1.0f); // attack with cooldown
        canAttack = true;
    }
    IEnumerator ThouAttack()
    {
        Debug.Log("ThrouAttack");
        canAttack = false;
        yield return new WaitForSeconds(1.0f); // attack with cooldown
        canAttack = true;
    }
}
