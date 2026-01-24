using UnityEngine;
using Sirenix.OdinInspector;

public class SunTrigger : MonoBehaviour
{
    [SerializeField, Required] private GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision with" +  collision.gameObject.name);
        if (collision.gameObject.CompareTag("Asteroids"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            OrbitalPlayer player = collision.GetComponent<OrbitalPlayer>();
            player.isDead = true;
            player.GetComponent<BoxCollider2D>().enabled = false;
            gameManager.TriggerGameOver();
        }
    }
}
