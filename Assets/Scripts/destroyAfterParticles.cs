using UnityEngine;

public class FoodParticleScript : MonoBehaviour
{
    private ParticleSystem foodParticleSystem;

    void Start()
    {
        // Get the ParticleSystem component
        foodParticleSystem = GetComponent<ParticleSystem>();

        // Destroy the GameObject after the particle system's duration
        Destroy(gameObject, foodParticleSystem.main.duration);
    }
}
