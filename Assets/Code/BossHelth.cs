using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHelth : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerHelth playerHelth;
    public float maxHelth;
    public float cunrentHelth;
    [HideInInspector]public float rage;
    private float rageDubbelr;

    private void Start()
    {
        GetComponent<PlayerMovement>();
        if (playerMovement == null )
        {
            playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
            Debug.LogWarning("HealthParticle: PlayerHelth component not found! Make sure your player has the 'PlayerHelth' script.");
        }
        GetComponent<PlayerHelth>();
        if(playerHelth == null)
        {
            playerHelth = GameObject.FindObjectOfType<PlayerHelth>();
            Debug.LogWarning("HealthParticle: PlayerHelth component not found! Make sure your player has the 'PlayerHelth' script.");
        }

        cunrentHelth = maxHelth;
        rage = 0;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null)
        { return; }
        else if (collision.gameObject.name == "Attacking Box")
        { HeathSystem(true); }

    }

    void HeathSystem(bool didThiGetHit)
    {
        cunrentHelth = maxHelth;

        if (didThiGetHit) 
        { 
            cunrentHelth -= playerMovement.attackDamige; 
            RageSystem(true, 5); 
        }
    }

    public void RageSystem(bool Canrage, float rageAmout)
    {
        if (rageAmout < 0)
            rageDubbelr = 0;
        else
            rageDubbelr += 1;

        if(rageAmout != 100 && Canrage)
            rage += rageAmout * rageDubbelr;
    }
}
