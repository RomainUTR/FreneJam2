using Sirenix.OdinInspector;
using UnityEngine;

public class OrbitalPlayer : MonoBehaviour
{
    [SerializeField, Required] private Transform sunCenter;
    [SerializeField] private float initialGravity = 5f;
    [SerializeField] private float pushForce = 15f;
    [SerializeField] private float rotationSpeed = 50f;

    [SerializeField] private float difficulty = 0.5f;

    private Rigidbody2D rb;
    private float currentGravity;
    private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentGravity = initialGravity;
    }

    private void Update()
    {
        if (isDead) return;

        currentGravity += difficulty * Time.deltaTime;

        if(Input.GetKey(KeyCode.Space))
        {
            ApplyPush();
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        ApplyGravity();
        ApplyOrbitRotation();
    }

    void ApplyGravity()
    {
        if (sunCenter == null) return;

        Vector2 directionToSun = (sunCenter.position - transform.position).normalized;
        rb.AddForce(directionToSun * currentGravity);

        float angle = Mathf.Atan2(directionToSun.y, directionToSun.x) * Mathf.Rad2Deg;
        rb.rotation = angle + 90f;
    }

    void ApplyPush()
    {
        Vector2 pushDir = (transform.position - sunCenter.position).normalized;
        rb.AddForce(pushDir * pushForce * Time.deltaTime * 60f);
    }

    void ApplyOrbitRotation()
    {
        Vector2 directionToSun = (sunCenter.position - transform.position).normalized;
        Vector2 tangent = new Vector2(-directionToSun.y, directionToSun.x);

        rb.AddForce(tangent * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sun"))
        {
            isDead = true;
            Debug.Log("Game Over! Score : " + Time.timeSinceLevelLoad);
        }
    }
}
