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
    [SerializeField] private GameObject playerMiddlePrefab;

    // Componentes:
    private Rigidbody2D _rb;
    private LineRenderer _line;

    // Inputs:
    private Vector2 _moveInput;
    private Vector2 _lastMoveInput;
    private bool _canMove = true;

    private Vector3[] _linePoints = new Vector3[5];

    private int _lastTargetDistance = 0;
    #endregion

    #region Funções Unity
    private void Start()
    {
        ResetLineMiddlePoints();

        _rb = GetComponent<Rigidbody2D>();
        _line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLinePoints();

        GetMoveInput();

        HasReachMaxDistance();
    }

    private void FixedUpdate()
    {
        if (_moveInput != Vector2.zero && _canMove) 
            ApplyMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Knot")) 
        {
            var knot = Instantiate(playerMiddlePrefab, collision.gameObject.transform.position, Quaternion.identity);
            
            for (int i = 0; i < _linePoints.Length; i++) 
            {
                if (i != 0 && i != 4) 
                {
                    if (i != _lastTargetDistance)
                    {
                        _linePoints[i] = knot.transform.position;
                        _lastTargetDistance = i;
                        break;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
            Die();

        if (collision.gameObject.layer == 7)
            ResetStretch();
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

    private void ApplyMove() => _rb.AddForce((Vector3) _moveInput * moveForce, ForceMode2D.Impulse);

    private void HasReachMaxDistance() 
    {
        if (Vector3.Distance(gameObject.transform.position, _linePoints[_lastTargetDistance]) >= maxDistance) 
        {
            ResetStretch();
        }
    }

    private void ResetCanMove() => _canMove = true;

    private void UpdateLinePoints() 
    {
        for (int i = 0; i < _line.positionCount; i++)
        {
            if (i == 0)
            {
                _line.SetPosition(i, gameObject.transform.position);
            }
            else if (i == _line.positionCount - 1)
            {
                _line.SetPosition(i, playerButt.transform.position);
            }
            else
            {
                _line.SetPosition(i, _linePoints[i]);
            }
        }
    }

    private void ResetLineMiddlePoints() 
    {
        for (int i = 0; i < _linePoints.Length; i++)
        {
            if (i != 0 && i != 4)
                _linePoints[i] = gameObject.transform.position;
        }

        _lastTargetDistance = 4;
    }

    private void Die() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    private void ResetStretch()
    {
        _rb.velocity = Vector2.zero;
        gameObject.transform.position = playerButt.transform.position + Vector3.up * 1f;

        ResetLineMiddlePoints();

        _canMove = false;
        Invoke("ResetCanMove", resetCanMoveInterval);

        var playerMiddlePoints = GameObject.FindGameObjectsWithTag("Player Middle");

        foreach (GameObject point in playerMiddlePoints)
            Destroy(point);
    }
    #endregion
}
