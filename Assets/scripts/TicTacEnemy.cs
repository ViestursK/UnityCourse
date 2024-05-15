using UnityEngine;

public class TicTacEnemy : MonoBehaviour
{
    public int health = 3;
    public float detectionRange = 15f;
    public float jumpForce = 5f;
    public float jumpInterval = 2f;
    public float deathDuration = 10f;

    private Transform player;
    private Rigidbody rb;
    private bool isJumping;

    private enum State { Alive, Dying }
    private State currentState = State.Alive;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isJumping = false;
        InvokeRepeating("JumpAround", jumpInterval, jumpInterval);
    }

    void Update()
    {
        if (player != null && currentState == State.Alive)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    // Handle damage taken by the player
    public void TakeDamage(int damage, Vector3 hitPoint)
    {
        if (currentState != State.Alive) return;

        health -= damage;
        ApplyForce(hitPoint);

        if (health <= 0)
        {
            Die();
        }
    }

    // Apply force to simulate impact
    public void ApplyForce(Vector3 hitPoint)
    {
        if (rb != null)
        {
            Vector3 forceDirection = transform.position - hitPoint;
            rb.AddForce(forceDirection.normalized * 10f, ForceMode.Impulse);
        }
    }

    // Handle enemy death
    void Die()
    {
        currentState = State.Dying;
        rb.isKinematic = false;

        rb.AddTorque(UnityEngine.Random.onUnitSphere * 10f, ForceMode.Impulse);

        Destroy(gameObject, deathDuration);
    }

    // Make the enemy jump around
    void JumpAround()
    {
        if (currentState != State.Alive || isJumping) return;

        isJumping = true;

        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 jumpDirection = (direction + Vector3.up).normalized;
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        }

        Invoke("ResetJump", 1f);
    }

    void ResetJump()
    {
        if (currentState != State.Alive) return;

        isJumping = false;
    }
}
