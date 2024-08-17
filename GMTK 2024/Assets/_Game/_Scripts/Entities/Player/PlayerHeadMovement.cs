using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadMovement : MonoBehaviour
{
    #region Variáveis
    [Header("Configurações:")]
    [SerializeField] private float stepsDistance;
    [SerializeField] private float maxDistance;

    [Header("Referências:")]
    [SerializeField] private GameObject playerButt;

    // Componentes:
    private Rigidbody2D _rb;
    private LineRenderer _line;

    // Inputs:
    private Vector2 _moveInput;
    private Vector2 _lastMoveInput;
    #endregion

    #region Funções Unity
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _line.SetPosition(0, gameObject.transform.position);
        _line.SetPosition(1, playerButt.transform.position);
        GetMoveInput();

        HasReachMaxDistance();
    }

    private void FixedUpdate()
    {
        if (_moveInput != Vector2.zero) 
            ApplyMove();
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
        _lastMoveInput = _moveInput;
    }

    private void HasReachMaxDistance() 
    {
        if (Vector3.Distance(gameObject.transform.position, playerButt.transform.position) >= maxDistance)
            gameObject.transform.position = playerButt.transform.position + Vector3.up * 1f;
    }
    #endregion
}
