using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    [SerializeField] private ScreenShake screenShakeScript;
    [SerializeField] private GameObject playerButt;
    [SerializeField] private GameObject playerMiddlePrefab;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform playerParent;
    [SerializeField] private EdgeCollider2D lineCollider;

    // Componentes:
    private Rigidbody2D _rb;
    private ValveScript _valveScript;
    private MenuManager _menuManager;

    // Inputs:
    private Vector2 _moveInput;
    private Vector2 _lastMoveInput;
    private bool _canMove = true;

    private Vector3[] _linePoints = new Vector3[5];

    private int _colliderStartPoint;

    private int _lastTargetDistance = 0;

    private bool _changedPos = false;
    #endregion

    #region Funções Unity
    private void Start()
    {
        ResetLineMiddlePoints();

        _rb = GetComponent<Rigidbody2D>();
        _menuManager = FindObjectOfType<MenuManager>();
    }

    private void Update()
    {
        UpdateLinePoints();

        UpdateLineCollider();

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

            if (_colliderStartPoint != _linePoints.Length - 2)
                _colliderStartPoint++;

            SetNewDistance(knot);
        }
        else if (collision.gameObject.CompareTag("NewBase") && !_changedPos)
        {
            var newButtPos = collision.gameObject.transform.Find("New Butt Pos").transform.position;
            if (playerButt.transform.position != newButtPos)
            {
                _colliderStartPoint = 0;
                SetButtPos(newButtPos);
                playerParent.transform.parent = collision.transform;
                _changedPos = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NewBase"))
        {
            Invoke("ResetChangedPos", 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.layer) 
        {
            // Bala
            case 6:
                ResetStretch();
                break;

            // Espinho
            case 7:
                ResetStretch();
                break;

            // Valvula
            case 8:
                _valveScript = collision.gameObject.GetComponent<ValveScript>();
                OpenGate();
                break;

            // SelecionarFases
            case 15:
                if (_menuManager != null)
                {
                    ResetStretch();
                    _menuManager.OpenStageSelection();
                }
                break;

            // Sair
            case 16:
                if (_menuManager != null)
                {
                    ResetStretch();
                    _menuManager.QuitGame();
                }
                break;

            // Voltar
            case 17:
                if (_menuManager != null)
                {
                    _menuManager.ReturnToMenu();
                }
                break;

            // Fase1
            case 18:
                if (_menuManager != null)
                {
                    
                }
                break;

            // Fase2
            case 19:
                if (_menuManager != null)
                {
                    
                }
                break;

            // Fase3
            case 20:
                if (_menuManager != null)
                {
                    
                }
                break;

            // Fase4
            case 21:
                if (_menuManager != null)
                {
                    
                }
                break;

            // Fase5
            case 22:
                if (_menuManager != null)
                {
                    
                }
                break;
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

    private void ApplyMove() => _rb.AddForce((Vector3) _moveInput * moveForce, ForceMode2D.Impulse);

    private void HasReachMaxDistance() 
    {
        if (Vector3.Distance(gameObject.transform.position, _linePoints[_lastTargetDistance]) >= maxDistance) 
            ResetStretch();
    }

    private void ResetCanMove() => _canMove = true;

    private void UpdateLinePoints() 
    {
        for (int i = 0; i < line.positionCount; i++)
        {
            if (i == 0)
            {
                var buttPos = playerButt.transform.position;
                line.SetPosition(i, new Vector3(buttPos.x, buttPos.y, 0f));
                _linePoints[i] = buttPos;
            }
            else if (i == line.positionCount - 1)
            {
                var headPos = gameObject.transform.position;
                line.SetPosition(i, new Vector3(headPos.x, headPos.y, 0f));
                _linePoints[i] = headPos;
            }
            else
            {
                line.SetPosition(i, new Vector3(_linePoints[_lastTargetDistance].x, _linePoints[_lastTargetDistance].y, 0f));
            }
        }
    }

    private void ResetLineMiddlePoints() 
    {
        for (int i = 0; i < _linePoints.Length; i++)
        {
            if (i != 0 && i != 4)
                _linePoints[i] = playerButt.transform.position;
        }

        _lastTargetDistance = 4;
    }

    private void ResetStretch()
    {
        screenShakeScript.ApplyScreenShake();

        _rb.velocity = Vector2.zero;
        gameObject.transform.position = playerButt.transform.position + Vector3.up * 1f;

        ResetLineMiddlePoints();

        _canMove = false;
        Invoke("ResetCanMove", resetCanMoveInterval);

        var playerMiddlePoints = GameObject.FindGameObjectsWithTag("Player Middle");

        foreach (GameObject point in playerMiddlePoints)
            Destroy(point);
    }

    private void UpdateLineCollider()
    {
        var startPoint = _linePoints[_colliderStartPoint];
        var endPoint = _linePoints[4];

        var points = new Vector2[] { startPoint, endPoint };

        lineCollider.points = points;
    }

    private void OpenGate()
    {
        _valveScript.connectedGate.SetActive(false);
    }
    
    private void SetNewDistance(GameObject knot) 
    {
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

    private void SetButtPos(Vector3 basePos) 
    {
        playerButt.transform.position = basePos;
        ResetStretch();
    }

    private void ResetChangedPos() => _changedPos = false;
    #endregion
}
