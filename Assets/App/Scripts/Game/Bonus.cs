using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

public enum BonusType
{
    MorePush,
    LessGravity
}

public class Bonus : MonoBehaviour
{
    [EnumToggleButtons, ReadOnly] public BonusType bonusType;

    [SerializeField] private float pushBoostAmount = 2f;
    [SerializeField] private float gravityReductionAmount = 0.5f;

    [SerializeField, Required] private Sprite pushSprite;
    [SerializeField, Required] private Sprite gravitySprite;

    void Start()
    {
        var values = System.Enum.GetValues(typeof(BonusType));
        bonusType = (BonusType)values.GetValue(Random.Range(0, values.Length));
        UpdateVisuals();

        transform.DOScale(1.2f, 0.6f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(gameObject)
            .SetEase(Ease.InOutSine);

        transform.DORotate(new Vector3(0, 0, 360), 4f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetLink(gameObject)
            .SetEase(Ease.Linear);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OrbitalPlayer player = collision.GetComponent<OrbitalPlayer>();
            if (player != null) 
            {
                ApplyBonus(player);
            }
            transform.DOKill();
            Destroy(this.gameObject);
        }
    }

    void UpdateVisuals()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            switch (bonusType)
            {
                case BonusType.MorePush:
                    sr.sprite = pushSprite;
                    break;
                case BonusType.LessGravity:
                    sr.sprite = gravitySprite;
                    break;
            }
        }
    }

    void ApplyBonus(OrbitalPlayer player)
    {
        switch (bonusType)
        {
            case BonusType.MorePush:
                player.pushForce += pushBoostAmount;
                Debug.Log("UPGRADE: Réacteurs boostés !");
                break;

            case BonusType.LessGravity:
                player.currentGravity = Mathf.Max(0.5f, player.currentGravity * gravityReductionAmount);
                Debug.Log("UPGRADE: Le vaisseau est plus léger !");
                break;
        }
    }
}
