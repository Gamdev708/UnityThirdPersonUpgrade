using UnityEngine;

public class EnergyCannonPickup : MonoBehaviour
{
    public Player player;
    public Transform cannon;

    private void OnTriggerEnter(Collider other)
    {
        player.GetCannon(cannon);

        gameObject.SetActive(false);
    }
}
