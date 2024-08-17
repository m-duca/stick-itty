using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHeadMovement : MonoBehaviour
{
    #region Variáveis
    [Header("Configurações:")]
    [SerializeField] private float moveForce;
    [SerializeField] private float maxDistance;
    [SerializeField] private float resetCanMoveInterval;

    [Header("Referências:")]
    [SerializeField] private GameObject playerButt;

    // Componentes:
    private Rigidbody2D _rb;
    private LineRenderer _line;

    // Inputs:
    private Vector2 _moveInput;
    private Vector2 _lastMoveInput;
    private bool _canMove = true;
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
        if (_moveInput != Vector2.zero && _canMove) 
            ApplyMove();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Die();
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
        _rb.AddForce((Vector3) _moveInput * moveForce, ForceMode2D.Impulse);
    }

    private void HasReachMaxDistance() 
    {
        if (Vector3.Distance(gameObject.transform.position, playerButt.transform.position) >= maxDistance) 
        {
            _rb.velocity = Vector2.zero;
            gameObject.transform.position = playerButt.transform.position + Vector3.up * 1f;
            _canMove = false;
            Invoke("ResetCanMove", resetCanMoveInterval);
        }
    }

    private void ResetCanMove() => _canMove = true;
    
    private void Die()
    {
        SceneManager.LoadScene("SampleScene");
    }
    #endregion
}
