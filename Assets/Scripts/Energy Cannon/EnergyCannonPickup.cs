using UnityEngine;

public class EnergyCannonPickup : MonoBehaviour
{
    public Player player;
    public Transform cannon;

    public AudioSource pickupAudio;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Pickup triggered");
        player.AttachCannon(cannon);

        AudioSource.PlayClipAtPoint(pickupAudio.clip, transform.position);

        gameObject.SetActive(false);
    }
}
