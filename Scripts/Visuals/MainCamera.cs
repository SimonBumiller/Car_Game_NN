using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform target;

    public Vector3 Offset;

    public Camera Camera;
    public Light Light;

    private void LateUpdate()
    {
        if (target != null)
        {
            Camera.transform.position = target.transform.position + Offset;
            Light.transform.position = target.transform.position + Offset;
        }
        
        Camera.transform.LookAt(target);
        Light.transform.LookAt(target);
    }
}