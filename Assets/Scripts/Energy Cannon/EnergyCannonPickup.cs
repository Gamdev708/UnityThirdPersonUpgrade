using UnityEngine;

public class EnergyCannonPickup : MonoBehaviour
{
    public Player player;
    public Transform cannon;

    public AudioClip pickupClip;

    private void OnTriggerEnter(Collider other)
    {
        player.GetCannon(cannon);

        AudioSource.PlayClipAtPoint(pickupClip, transform.position);

        gameObject.SetActive(false);
    }
}
