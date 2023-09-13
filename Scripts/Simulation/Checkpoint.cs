///Code taken from Samuel Arzt - Applying EANNs
/// <link> https://github.com/ArztSamuel/Applying_EANNs </link>

using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] public float Radius = 3.5F;

    /// <summary>
    ///     The amount that is received when this checkpoint is triggered.
    /// </summary>
    public float Reward;

    /// <summary>
    ///     The total reward that has been received when this checkpoint is triggered.
    /// </summary>
    public float TotalReward;

    /// <summary>
    ///     The distance to the most recent checkpoint.
    /// </summary>
    public float DistanceToLast;

    /// <summary>
    ///     The total distance that has been accumulated when this checkpoint is triggered.
    /// </summary>
    public float TotalDistance;

    private SpriteRenderer Renderer;

    public Vector3 Position
    {
        get => transform.position;
        private set => transform.position = value;
    }

    private void Awake()
    {
        transform.localScale = new Vector3(2 * Radius, transform.localScale.y, 2 * Radius);
    }

    public float GetPartOfReward(float currentDistance)
    {
        var completePerc = (DistanceToLast - currentDistance) / DistanceToLast;

        if (completePerc < 0)
            return 0;
        return completePerc * Reward;
    }
}