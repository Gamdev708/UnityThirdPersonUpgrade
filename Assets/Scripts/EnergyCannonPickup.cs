using UnityEngine;

public class EnergyCannonPickup : MonoBehaviour
{
    public Player player;
    public Transform cannon;

    public AudioSource pickupAudio;

    private void OnTriggerEnter(Collider other)
    {
        player.AttachCannon(cannon);

        pickupAudio.Play();

        gameObject.SetActive(false);
    }
}
