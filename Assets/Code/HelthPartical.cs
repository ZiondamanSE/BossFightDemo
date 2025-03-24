
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelthPartical : MonoBehaviour
{
    public PlayerHelth playerHelth;

    RaycastHit2D ray1, ray2, ray3, ray4, ray5, ray6, ray7, ray8;
    public float rayAngle = 30;
    public float rayDistance = 5f; // Increased default detection range

    // Movement variables
    public float baseSpeed = 1.5f;
    public float maxSpeed = 4.0f;
    private Transform playerTransform;
    private bool playerDetected = false;

    // Debug variables
    public bool showDebugLogs = true;

    // Start is called before the first frame update
    void Start()
    {
        playerHelth = GetComponent<PlayerHelth>();

        if(playerHelth == null && showDebugLogs)
        {
            playerHelth = GameObject.FindObjectOfType<PlayerHelth>();
            Debug.LogWarning("HealthParticle: PlayerHelth component not found! Make sure your player has the 'PlayerHelth' script.");
        }

        // Find the player (typically tagged as "Player" in Unity)
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerTransform == null && showDebugLogs)
        {
            Debug.LogWarning("HealthParticle: Player not found! Make sure your player has the 'Player' tag.");
        }
    }

    void CastRays()
    {
        // Cast rays in all directions, symmetrically from the center
        ray1 = Physics2D.Raycast(transform.position, Vector2.up, rayDistance);
        ray2 = Physics2D.Raycast(transform.position, Vector2.down, rayDistance);
        ray3 = Physics2D.Raycast(transform.position, Vector2.right, rayDistance);
        ray4 = Physics2D.Raycast(transform.position, Vector2.left, rayDistance);

        // Angled rays at +rayAngle and -rayAngle (symmetrical)
        Vector2 angleDir1 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * rayAngle), Mathf.Sin(Mathf.Deg2Rad * rayAngle));
        Vector2 angleDir2 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * -rayAngle), Mathf.Sin(Mathf.Deg2Rad * -rayAngle));
        Vector2 angleDir3 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (180 - rayAngle)), Mathf.Sin(Mathf.Deg2Rad * (180 - rayAngle)));
        Vector2 angleDir4 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (180 + rayAngle)), Mathf.Sin(Mathf.Deg2Rad * (180 + rayAngle)));

        ray5 = Physics2D.Raycast(transform.position, angleDir1, rayDistance);
        ray6 = Physics2D.Raycast(transform.position, angleDir2, rayDistance);
        ray7 = Physics2D.Raycast(transform.position, angleDir3, rayDistance);
        ray8 = Physics2D.Raycast(transform.position, angleDir4, rayDistance);
    }

    bool CheckPlayerInRange()
    {
        // First make sure we have a player reference
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (playerTransform == null) return false;
        }

        // Simple distance check (alternative to raycasts)
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        return distanceToPlayer <= rayDistance;
    }

    bool CheckForPlayerRaycast()
    {
        CastRays();

        // Check all raycasts for player hits
        if ((ray1.collider != null && ray1.collider.CompareTag("Player")) ||
            (ray2.collider != null && ray2.collider.CompareTag("Player")) ||
            (ray3.collider != null && ray3.collider.CompareTag("Player")) ||
            (ray4.collider != null && ray4.collider.CompareTag("Player")) ||
            (ray5.collider != null && ray5.collider.CompareTag("Player")) ||
            (ray6.collider != null && ray6.collider.CompareTag("Player")) ||
            (ray7.collider != null && ray7.collider.CompareTag("Player")) ||
            (ray8.collider != null && ray8.collider.CompareTag("Player")))
        {
            if (showDebugLogs) Debug.Log("HealthParticle: Player detected by raycast!");
            return true;
        }

        return false;
    }

    void OnDrawGizmos()
    {
        // Get player and check detection during edit mode for visualization
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        // Check player in range for gizmo color
        playerDetected = (playerTransform != null) &&
                         Vector2.Distance(transform.position, playerTransform.position) <= rayDistance;

        // Set color based on whether player is detected
        Gizmos.color = playerDetected ? Color.green : Color.red;

        // Draw rays in all directions from center
        Gizmos.DrawRay(transform.position, Vector2.up * rayDistance);
        Gizmos.DrawRay(transform.position, Vector2.down * rayDistance);
        Gizmos.DrawRay(transform.position, Vector2.right * rayDistance);
        Gizmos.DrawRay(transform.position, Vector2.left * rayDistance);

        // Draw angled rays
        Vector2 angleDir1 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * rayAngle), Mathf.Sin(Mathf.Deg2Rad * rayAngle));
        Vector2 angleDir2 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * -rayAngle), Mathf.Sin(Mathf.Deg2Rad * -rayAngle));
        Vector2 angleDir3 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (180 - rayAngle)), Mathf.Sin(Mathf.Deg2Rad * (180 - rayAngle)));
        Vector2 angleDir4 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (180 + rayAngle)), Mathf.Sin(Mathf.Deg2Rad * (180 + rayAngle)));

        Gizmos.DrawRay(transform.position, angleDir1 * rayDistance);
        Gizmos.DrawRay(transform.position, angleDir2 * rayDistance);
        Gizmos.DrawRay(transform.position, angleDir3 * rayDistance);
        Gizmos.DrawRay(transform.position, angleDir4 * rayDistance);

        // Draw a circle showing detection range
        Gizmos.DrawWireSphere(transform.position, rayDistance);
    }

    // Update is called once per frame
    void Update()
    {
        // Try both detection methods and move if either works
        playerDetected = CheckPlayerInRange() || CheckForPlayerRaycast();

        // Move towards player if detected
        if (playerDetected && playerTransform != null)
        {
            MoveToPlayer();
        }
    }

    private void MoveToPlayer()
    {
        if (playerTransform == null)
            return;

        // Calculate direction to player
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Calculate distance to player
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Speed increases as particle gets closer to player (inverse of distance)
        float speedFactor = Mathf.Clamp(1.0f - (distanceToPlayer / rayDistance), 0.1f, 1.0f);
        float currentSpeed = Mathf.Lerp(baseSpeed, maxSpeed, speedFactor);

        if (showDebugLogs && Time.frameCount % 60 == 0) // Log only every 60 frames to reduce spam
        {
            Debug.Log($"Moving to player: Distance = {distanceToPlayer}, Speed = {currentSpeed}");
        }

        // Move toward player
        transform.position = Vector2.MoveTowards(
            transform.position,
            playerTransform.position,
            currentSpeed * Time.deltaTime
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHelth.HelthSystem(0, 10);
            Destroy(gameObject);
        }
    }

    // Add this to visualize the detection range in the game view
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere to show the detection range when selected
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rayDistance);
    }

}