///Code taken from Samuel Arzt - Applying EANNs
/// <link> https://github.com/ArztSamuel/Applying_EANNs </link>

#region Includes

using UnityEngine;

#endregion


public class Sensor : MonoBehaviour
{
    // Max and min readings
    private const float MAX_DIST = 1000F;
    private const float MIN_DIST = 0.000001f;
    [SerializeField] private LayerMask LayerToSense;

    [SerializeField] private GameObject LineRenderer;


    public Vector3 RayDirection;
    
    public bool ShowLine { private get; set; }

    public float Output { get; private set; }

    private void Start()
    {
        LineRenderer.SetActive(ShowLine);
    }

    private void FixedUpdate()
    {
        //Debug.Log("Will fire a raycast into direction " + RayDirection.ToString());
        var result = Physics.Raycast(new Ray(transform.position, RayDirection), out var hit, 999999F, LayerToSense.value);
        if (result)
        {
            Debug.DrawLine(transform.position, hit.point);
            if (hit.collider == null)
                hit.distance = MAX_DIST;
            else if (hit.distance < MIN_DIST)
                hit.distance = MIN_DIST;
            Output = hit.distance;
            
            LineRenderer.gameObject.SetActive(ShowLine);
        }
    }
}