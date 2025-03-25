using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovmentSys : MonoBehaviour
{
    public GameObject homePoint;
    public GameObject playerPoint;
    [Header("Boss Settings")]
    public float phaseOneMovmentspeed;
    public float phaseTwoMovmentspeed;
    public float phaseThreeMovmentspeed;
    [Space]
    public float phaseOneWaitTime;
    public float phaseTwoWaitTime;
    public float phaseThreeWaitTime;

    private bool moveHome;
    private bool moveToPlayerpointX;

    void Start()
    {
        moveHome = true;
        moveToPlayerpointX = false;
    }

    void Update()
    {
       // mcdonaldsmeanforkidsorsomthingiforgot();
    }

   /* void mcdonaldsmeanforkidsorsomthingiforgot()
    {
        if (moveHome)
            transform.position = new Vector2(Mathf.MoveTowards(transform.position.x, homePoint.transform.position.x, phaseOneMovmentspeed * Time.deltaTime),
                                             Mathf.MoveTowards(transform.position.y, homePoint.transform.position.y, phaseOneMovmentspeed * Time.deltaTime));

        if (Vector2.Distance(transform.position, homePoint.transform.position) < 0.1f)
            StartCoroutine(HomeTime());


        if (moveToPlayerpointX)
        {
            transform.position = new Vector2(Mathf.Lerp(transform.position.x, playerPoint.transform.position.x, phaseOneMovmentspeed * Time.deltaTime), transform.position.y);

        }
    }*/

    IEnumerator HomeTime()
    {
        Debug.Log("Boss is moving home");
        yield return new WaitForSeconds(3f);
        moveHome = false;
        moveToPlayerpointX = true;
    }

    IEnumerator AttackCounter()
    {
        yield return new WaitForSeconds(phaseOneWaitTime);

    }

    void Attack()
    {
       // transform.position = new Vector2.Lerp(transform.position, Vector2.down, phaseOneMovmentspeed * Time.deltaTime);
    }

}