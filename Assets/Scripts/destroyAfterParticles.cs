using UnityEngine;

public class destroyAfterParticles : MonoBehaviour
{
    private ParticleSystem particleS;

    void Start()
    {
        // Get the ParticleSystem component
        particleS = GetComponent<ParticleSystem>();

        // Destroy the GameObject after the particle system's duration
        Destroy(gameObject, particleS.main.duration);
    }
}
