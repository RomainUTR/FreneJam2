using UnityEngine;

public class SolarTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player touch the sun");
    }
}
