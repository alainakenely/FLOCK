using UnityEngine;

public class FollowParallax : MonoBehaviour
{
    public Transform target; // assign your parallax object
    void LateUpdate()
    {
        transform.position = target.position;
    }
}