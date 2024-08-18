using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [Header("Configurações:")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeTime;

    // Componentes:
    private Rigidbody2D _rb;

    public Vector2 MoveDir;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();        
    }

    private void FixedUpdate() => ApplyMovement();

    private void ApplyMovement() => _rb.velocity = MoveDir * moveSpeed;
}
