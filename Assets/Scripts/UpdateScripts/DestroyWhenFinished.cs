using UnityEngine;

public class DestroyWhenFinished : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        if (!TryGetComponent(out ps))
        {
            Debug.LogError("No ParticleSystem found on the GameObject.");
        }
    }

    void Update()
    {
        if (ps != null && !ps.IsAlive(true))
        {
            Destroy(gameObject);
        }
    }
}
