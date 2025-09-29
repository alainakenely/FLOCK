using UnityEngine;

public class FlyBehavior : MonoBehaviour
{
    [SerializeField] private float _velocity = 1.5f;
    
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
}
