using UnityEngine;

public class FallDeath : MonoBehaviour
{
    public GameObject grid;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            grid.transform.position = player.transform.position;
            timer = 5;
            grid.SetActive(true);

            player.Destroyed();
        }
    }

    float timer;

    private void FixedUpdate()
    {
        if (grid.activeInHierarchy)
        {
            timer -= Time.fixedDeltaTime;

            if (timer < 0)
            {
                grid.SetActive(false);
            }
        }
    }
}
