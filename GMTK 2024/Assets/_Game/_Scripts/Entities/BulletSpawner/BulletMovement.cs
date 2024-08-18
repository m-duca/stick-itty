using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    #region Variáveis
    [Header("Configurações:")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeTime;

    // Componentes:
    private Rigidbody2D _rb;

    public Vector2 MoveDir;
    #endregion

    #region Funções Unity
    private void Start() => _rb = GetComponent<Rigidbody2D>();        

    private void FixedUpdate() => ApplyMovement();
    #endregion

    #region Funções Próprias
    private void ApplyMovement() => _rb.velocity = MoveDir * moveSpeed;
    #endregion
}
