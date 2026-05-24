using UnityEngine;

public class EnergyShot : MonoBehaviour
{
    public PlayerParameters parameters;

    public MeshRenderer meshRenderer;

    float timer;

    Vector3 startPosition;
    Color baseEmissionColor;

    private void Start()
    {
        baseEmissionColor = meshRenderer.material.GetColor("_EmissionColor");
    }

    public void Shoot(Vector3 startPosition, Quaternion rotation)
    {
        timer = 0;
        this.startPosition = startPosition;
        transform.rotation = rotation;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float phase = timer / parameters.shotLifetime;

        if (phase > 1)
        {
            gameObject.SetActive(false);
            return;
        }

        transform.position = startPosition + transform.forward * parameters.shotDistance * phase;

        transform.localScale = Vector3.one * parameters.shotScale.Evaluate(phase);

        meshRenderer.material.SetColor("_EmissionColor", baseEmissionColor * parameters.shotAlpha.Evaluate(phase));
    }
}
