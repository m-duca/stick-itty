using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadMovement : MonoBehaviour
{
    #region Variáveis
    [Header("Configurações:")]
    [SerializeField, Range(0f, 10f)] private float stepsDistance;

    [Header("Referências:")]
    [SerializeField] private GameObject playerMiddlePrefab;

    // Componentes:
    private Rigidbody2D _rb;

    // Inputs:
    private Vector2 _moveInput;

    private bool _canMove = true;

    private bool _canAdd = true;
    #endregion

    #region Funções Unity
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetMoveInput();
    }

    private void FixedUpdate()
    {
        if (_moveInput != Vector2.zero && _canMove) 
        {
            ApplyMove();
            AddMiddle();
        }
    }
    #endregion

    #region Funções Próprias
    private void GetMoveInput()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        if (x != 0)
            _moveInput = new Vector2(x, 0);
        else if (y != 0)
            _moveInput = new Vector2(0, y);
        else
            _moveInput = new Vector2(0, 0);
    }

    private void ApplyMove() 
    {
        transform.position += (Vector3) _moveInput * stepsDistance;
    }

    private void AddMiddle()
    {
        var pos = (Vector2) transform.position - _moveInput * stepsDistance;
        Instantiate(playerMiddlePrefab, (Vector3) pos, Quaternion.identity);
    }
    #endregion
}
