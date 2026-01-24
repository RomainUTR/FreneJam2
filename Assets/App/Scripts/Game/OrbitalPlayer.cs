using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitalPlayer : MonoBehaviour
{
    [SerializeField, Required] private Transform sunCenter;
    [SerializeField] private float initialGravity = 5f;
    public float pushForce = 15f;
    [SerializeField] private float rotationSpeed = 50f;

    [SerializeField] private float difficulty = 0.5f;

    private Rigidbody2D rb;
    [ReadOnly] public float currentGravity;
    public bool isDead = false;

    private InputSystem_Actions ctx;
    public PlayerFeedback feedback;

    private void OnEnable()
    {
        ctx = new InputSystem_Actions();
        ctx.Enable();
    }

    private void OnDisable()
    {
        ctx.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentGravity = initialGravity;

        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);
        if (!string.IsNullOrEmpty(rebinds))
        {
            ctx.LoadBindingOverridesFromJson(rebinds);
        }
        Debug.Log(ctx.ToString());
    }

    private void Update()
    {
        if (isDead) return;

        currentGravity += difficulty * Time.deltaTime;

        bool isPressing = ctx.Gameplay.Thrust.IsPressed();

        if (feedback != null)
        {
            feedback.UpdateFeedback(isPressing);
        }

        if (isPressing)
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
}
