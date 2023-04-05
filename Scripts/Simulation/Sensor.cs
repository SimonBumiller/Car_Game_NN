///Code taken from Samuel Arzt - Applying EANNs
/// <link> https://github.com/ArztSamuel/Applying_EANNs </link>

#region Includes

using UnityEngine;

#endregion


public class Sensor : MonoBehaviour
{
    // Max and min readings
    private const float MAX_DIST = 10F;
    private const float MIN_DIST = 0.01f;
    [SerializeField] private LayerMask LayerToSense;

    [SerializeField] private SpriteRenderer Cross;
    [SerializeField] private GameObject LineRenderer;

    public bool ShowCross { private get; set; }

    public bool ShowLine { private get; set; }

    public float Output { get; private set; }

    private void Start()
    {
        Cross.gameObject.SetActive(ShowCross);
        LineRenderer.SetActive(ShowLine);
    }

    private void FixedUpdate()
    {
        Vector2 direction = Cross.transform.position - transform.position;
        direction.Normalize();

        var hit = Physics2D.Raycast(transform.position, direction, MAX_DIST, LayerToSense);

        //Check distance
        if (hit.collider == null)
            hit.distance = MAX_DIST;
        else if (hit.distance < MIN_DIST)
            hit.distance = MIN_DIST;
        Output = hit.distance;

        Cross.transform.position = (Vector2)transform.position + direction * hit.distance;

        Cross.gameObject.SetActive(ShowCross);
        LineRenderer.gameObject.SetActive(ShowLine);
    }

    public void SetPosition(Vector2 position)
    {
        Cross.transform.position = position;
    }
}