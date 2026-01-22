using UnityEngine;
using RomainUTR.SLToolbox.Runtime;

public class AsteroidsBehaviour : MonoBehaviour
{
    public float gravityStrength = 30f;

    public Sprite[] sprites;

    private Transform sunCenter;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public void InitializeAsteroid(Transform sun, Vector2 initialVelocity)
    {
        sunCenter = sun;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites.GetRandom();
        rb.linearVelocity = initialVelocity;
        rb.angularVelocity = Random.Range(-100f, 100f);
    }

    private void FixedUpdate()
    {
        if (sunCenter == null) return;

        Vector2 directionToSun = (sunCenter.position - transform.position).normalized;
        rb.AddForce(directionToSun * gravityStrength * Time.fixedDeltaTime);

        if (rb.linearVelocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x);
            transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        }
    }
}
