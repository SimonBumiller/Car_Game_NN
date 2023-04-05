using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;

    private void LateUpdate()
    {
        if (target != null)
        {
            var newPos = target.position + offset;
            newPos.z = transform.position.z;

            transform.position = newPos;
        }
    }
}