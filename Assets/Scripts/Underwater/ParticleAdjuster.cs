using UnityEngine;

public class ParticleAdjuster : MonoBehaviour
{
    public Transform obj; // Reference to the object
    public ParticleSystem particleS; // Reference to the particle system

    private SpriteRenderer spriteRenderer;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.ShapeModule shapeModule;

    // Variables to store the original values from the particle system
    private ParticleSystem.MinMaxCurve originalStartSize;
    private ParticleSystem.MinMaxCurve originalRateOverTime;

    private void Start()
    {
        // Validate input references
        if (obj == null)
        {
            Debug.LogError("Object reference (obj) is missing in ParticleAdjuster.");
            return;
        }

        if (particleS == null)
        {
            Debug.LogError("ParticleSystem reference is missing in ParticleAdjuster.");
            return;
        }

        // Get modules from the particle system
        mainModule = particleS.main;
        shapeModule = particleS.shape;

        // Store original values
        originalStartSize = mainModule.startSize;

        // Try to find a SpriteRenderer on the object or its children
        spriteRenderer = obj.GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("No SpriteRenderer found on the object or its children. Particle size will only consider scale.");
        }
    }

    private void FixedUpdate()
    {
        if (obj == null || particleS == null) return;

        // Get the size of the object to adjust particle size
        float size = CalculateSize();

        // Adjust particle size
        mainModule.startSize = ScaleMinMaxCurve(originalStartSize, size);

        // Update the particle system shape to match the sprite's size
        shapeModule.shapeType = ParticleSystemShapeType.Sprite;
        shapeModule.sprite = spriteRenderer.sprite;
    }

    private float CalculateSize()
    {
        // If a SpriteRenderer exists, combine its bounds with the scale
        if (spriteRenderer != null)
        {
            Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

            // Consider the object's scale
            Vector2 scaledSize = new(
                spriteSize.x * Mathf.Abs(obj.localScale.x),
                spriteSize.y * Mathf.Abs(obj.localScale.y)
            );

            return (scaledSize.x + scaledSize.y) / 2f; // Average size
        }

        // Fallback to scale-only size calculation if no SpriteRenderer
        return (obj.localScale.x + obj.localScale.y + obj.localScale.z) / 3f;
    }

    private ParticleSystem.MinMaxCurve ScaleMinMaxCurve(ParticleSystem.MinMaxCurve originalCurve, float multiplier)
    {
        ParticleSystem.MinMaxCurve scaledCurve = originalCurve;

        // Scale constant values
        if (originalCurve.mode == ParticleSystemCurveMode.Constant)
        {
            scaledCurve.constant = originalCurve.constant * multiplier;
        }
        // Scale "Random Between Two Constants"
        else if (originalCurve.mode == ParticleSystemCurveMode.TwoConstants)
        {
            scaledCurve.constantMin = originalCurve.constantMin * multiplier;
            scaledCurve.constantMax = originalCurve.constantMax * multiplier;

            if (scaledCurve.constantMax > 0.4f)
            {
                scaledCurve.constantMax = 0.4f;
            }
        }
        // Scale curves (if needed)
        else if (originalCurve.mode == ParticleSystemCurveMode.Curve || originalCurve.mode == ParticleSystemCurveMode.TwoCurves)
        {
            Debug.LogWarning("Scaling curves in MinMaxCurve is not currently supported. Add implementation if needed.");
        }

        return scaledCurve;
    }
}