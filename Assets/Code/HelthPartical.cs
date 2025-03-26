using UnityEngine;

public class HelthPartical : MonoBehaviour
{
    public PlayerHelth playerHelth;
    public float rayDistance = 5f;
    public float baseSpeed = 1.5f;
    public float maxSpeed = 4.0f;
    public bool showDebugLogs = true;

    private Transform playerTransform;
    private bool playerDetected = false;

    void Start()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        playerHelth = GetComponent<PlayerHelth>() ??
            GameObject.FindObjectOfType<PlayerHelth>();

        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerHelth == null && showDebugLogs)
        {
            Debug.LogWarning("HealthParticle: PlayerHelth component not found!");
        }

        if (playerTransform == null && showDebugLogs)
        {
            Debug.LogWarning("HealthParticle: Player not found!");
        }
    }

    bool CheckForPlayerRaycast()
    {
        if (playerTransform == null) return false;

        // Calculate direction to player
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Cast a single ray towards the player
        RaycastHit2D rayToPlayer = Physics2D.Raycast(
            transform.position,
            directionToPlayer,
            rayDistance
        );

        // Check if ray hits the player
        return rayToPlayer.collider != null &&
               rayToPlayer.collider.CompareTag("Player");
    }

    void Update()
    {
        // Detect player using single ray or distance check
        playerDetected = CheckForPlayerRaycast() || CheckPlayerInRange();

        if (playerDetected && playerTransform != null)
        {
            MoveToPlayer();
        }
    }

    bool CheckPlayerInRange()
    {
        if (playerTransform == null) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        return distanceToPlayer <= rayDistance;
    }

    private void MoveToPlayer()
    {
        if (playerTransform == null) return;

        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        float speedFactor = Mathf.Clamp(1.0f - (distanceToPlayer / rayDistance), 0.1f, 1.0f);
        float currentSpeed = Mathf.Lerp(baseSpeed, maxSpeed, speedFactor);

        if (showDebugLogs && Time.frameCount % 60 == 0)
        {
            Debug.Log($"Moving to player: Distance = {distanceToPlayer}, Speed = {currentSpeed}");
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            playerTransform.position,
            currentSpeed * Time.deltaTime
        );
    }

    void OnDrawGizmos()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        playerDetected = (playerTransform != null) &&
                         Vector2.Distance(transform.position, playerTransform.position) <= rayDistance;

        // Draw a single ray towards the player
        Gizmos.color = playerDetected ? Color.green : Color.red;

        if (playerTransform != null)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            Gizmos.DrawRay(transform.position, directionToPlayer * rayDistance);
        }

        // Draw detection range sphere
        Gizmos.DrawWireSphere(transform.position, rayDistance);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHelth.HelthSystem(0, 10);
            Destroy(gameObject);
        }
    }
}