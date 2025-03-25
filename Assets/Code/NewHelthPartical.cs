using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHelthPartical : MonoBehaviour
{
    RaycastHit2D ray;
    public PlayerHelth playerHelth;

    [SerializeField] GameObject player;
    public float rayDistance = 5f;
    [Space] // Movement variables
    public float baseSpeed = 1.5f;
    public float maxSpeed = 4.0f;


    // Start is called before the first frame update
    void Start()
    {
        playerHelth = GetComponent<PlayerHelth>();

        if (playerHelth == null)
        {
            playerHelth = GameObject.FindObjectOfType<PlayerHelth>();
            Debug.LogWarning("HealthParticle: PlayerHelth component not found! Make sure your player has the 'PlayerHelth' script.");
        }
    }

    void raycastSys()
    {
        ray = Physics2D.Raycast(transform.position, player.gameObject.transform.position, rayDistance);
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            // Draw ray from current position towards player
            Vector2 direction = (player.transform.position - transform.position).normalized * rayDistance;
            Gizmos.color = Color.red; // Optional: set a color for visibility
            Gizmos.DrawRay(transform.position, direction);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
