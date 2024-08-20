using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using UnityEditor.SceneManagement;
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
    private PortalManager _portalManager;
    private StageManager _stageManager;

    // Inputs:
    private Vector2 _moveInput;
    private Vector2 _lastMoveInput;
    private bool _canMove = true;

    private Vector3[] _linePoints;

    private int _colliderStartPoint;

    private int _lastTargetDistance = 0;

    private bool _changedPos = false;

    // Audio:
    private static int _frogSfxIndex = 1;
    private static int _knotSfxIndex = 1;
    private static int _checkpointSfxIndex = 1;
    private static int _buttonSfxIndex = 1;

    // Knots:
    private BoxCollider2D[] _knotsColliders;

    private Vector2 _headSpawnPosDir = Vector2.up;
    #endregion

    #region Funções Unity
    private void Start()
    {
        _linePoints = new Vector3[line.positionCount];

        _knotsColliders = new BoxCollider2D[line.positionCount - 2];

        ResetLineMiddlePoints();

        _rb = GetComponent<Rigidbody2D>();
        _menuManager = FindObjectOfType<MenuManager>();
        _portalManager = FindObjectOfType<PortalManager>();
        _stageManager = FindObjectOfType<StageManager>();
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
            if (_lastTargetDistance < _linePoints.Length - 2)
            {
                var knot = Instantiate(playerMiddlePrefab, collision.ClosestPoint(gameObject.transform.position), Quaternion.identity);

                if (_colliderStartPoint != _linePoints.Length - 2)
                    _colliderStartPoint++;

                StoreKnotCollider(collision.gameObject.GetComponent<BoxCollider2D>());

                SetNewDistance(knot);

                if (AudioManager.Instance != null) 
                {
                    AudioManager.Instance.PlaySFX("knot " + _knotSfxIndex);
                    if (_knotSfxIndex == 1)
                        _knotSfxIndex = 2;
                    else
                        _knotSfxIndex = 1;
                }
            }
        }
        else if (collision.gameObject.CompareTag("NewBase") && !_changedPos)
        {
            var newButtPos = collision.gameObject.transform.Find("New Butt Pos").transform.position;
            if (playerButt.transform.position != newButtPos)
            {
                _colliderStartPoint = 0;
                SetButtPos(newButtPos);
                SetButtRotation(collision.gameObject.GetComponent<BaseButtRotation>().NewRotationZ);
                playerParent.transform.parent = collision.transform;
                _changedPos = true;

                _headSpawnPosDir = collision.gameObject.GetComponent<BaseHeadPos>().OffsetDir;

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX("frog " + _frogSfxIndex);
                    if (_frogSfxIndex == 1)
                        _frogSfxIndex = 2;
                    else
                        _frogSfxIndex = 1;

                    AudioManager.Instance.PlaySFX("checkpoint " + _checkpointSfxIndex);
                    if (_checkpointSfxIndex == 1)
                        _checkpointSfxIndex = 2;
                    else
                        _checkpointSfxIndex = 1;
                }
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


            // Portal
            case 9:
                if (_portalManager != null)
                {
                    SceneManager.LoadScene(_portalManager.nextStage);
                    CompleteStage(_stageManager.currentStageNumber);
                }
                break;

            // SelecionarFases
            case 15:
                if (_menuManager != null)
                {
                    ButtonSfx();
                    ResetStretch();
                    _menuManager.OpenStageSelection();
                }
                break;

            // Sair
            case 16:
                if (_menuManager != null)
                {
                    ButtonSfx();
                    ResetStretch();
                    _menuManager.QuitGame();
                }
                break;

            // Voltar
            case 17:
                if (_menuManager != null)
                {
                    ButtonSfx();
                    ResetStretch();
                    _menuManager.ReturnToMenu();
                }
                break;

            // Fase1
            case 18:
                if (_menuManager != null)
                {
                    ButtonSfx();
                    _menuManager.OpenStage("Level 1");
                }
                break;

            // Fase2
            case 19:
                if (_menuManager != null)
                {
                    ButtonSfx();
                    _menuManager.OpenStage("Level 2");
                }
                break;

            // Fase3
            case 20:
                if (_menuManager != null)
                {
                    ButtonSfx();
                    _menuManager.OpenStage("Level 3");
                }
                break;

            /*// Fase4
            case 21:
                if (_menuManager != null)
                {
                    ButtonSfx();
                    _menuManager.OpenStage("SampleScene");
                }
                break;

            // Fase5
            case 22:
                if (_menuManager != null)
                {
                    ButtonSfx();
                    _menuManager.OpenStage("TESTEFINAL");
                }
                break;*/
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
            else if (i > _lastTargetDistance)
            {
                var headPos = gameObject.transform.position;
                line.SetPosition(i, new Vector3(headPos.x, headPos.y, 0f));
                _linePoints[i] = headPos;
            }
        }
    }

    private void ResetLineMiddlePoints() 
    {
        for (int i = 0; i < _linePoints.Length; i++)
        {
            if (i != 0 && i != _linePoints.Length - 1)
                _linePoints[i] = playerButt.transform.position;
        }

        _lastTargetDistance = 0;
    }

    public void ResetStretch()
    {
        screenShakeScript.ApplyScreenShake();

        _rb.velocity = Vector2.zero;
        gameObject.transform.position = playerButt.transform.position + (Vector3)_headSpawnPosDir * 1f;

        ResetLineMiddlePoints();
        ClearKnotsColliders();
        _lastTargetDistance = 0;

        _canMove = false;
        Invoke("ResetCanMove", resetCanMoveInterval);

        var playerMiddlePoints = GameObject.FindGameObjectsWithTag("Player Middle");

        foreach (GameObject point in playerMiddlePoints)
            Destroy(point);
    }

    private void UpdateLineCollider()
    {
        var startPoint = _linePoints[_colliderStartPoint];
        var endPoint = _linePoints[_linePoints.Length - 1];

        var points = new Vector2[] { startPoint, endPoint };

        lineCollider.points = points;
    }

    private void OpenGate() => _valveScript.connectedGate.SetActive(false);
    
    private void SetNewDistance(GameObject knot) 
    {
        _lastTargetDistance++;
        _linePoints[_lastTargetDistance] = knot.transform.position;

        /*
        for (int i = 0; i < _linePoints.Length; i++)
        {
            if (i != 0 && i != _linePoints.Length - 1)
            {
                if (i == _lastTargetDistance)
                    _linePoints[i] = knot.transform.position;
            }
        }
        */
    }

    private void SetButtPos(Vector3 basePos) 
    {
        playerButt.transform.position = basePos;
        ResetStretch();
    }

    private void SetButtRotation(float z) => playerButt.transform.rotation = Quaternion.Euler(0f, 0f, z);

    private void ResetChangedPos() => _changedPos = false;

    private void StoreKnotCollider(BoxCollider2D collider) 
    {
        for (int i = 0; i < _knotsColliders.Length; i++) 
        {
            if (_knotsColliders[i] == null) 
            {
                _knotsColliders[i] = collider;
                _knotsColliders[i].enabled = false;
            }
        }
    }

    private void ClearKnotsColliders()
    {
        for (int i = 0; i < _knotsColliders.Length; i++)
        {
            if (_knotsColliders[i] != null)
            {
                _knotsColliders[i].enabled = true;
                _knotsColliders[i] = null;
            }
        }
    }

    public void CompleteStage(int stageNumber)
    {
        Debug.Log("Completou a fase " + stageNumber);

        // Pega o progresso salvo
        int unlockedStages = PlayerPrefs.GetInt("UnlockedStages", 1);

        // Se a próxima fase não estiver desbloqueada ainda, desbloqueia
        if (unlockedStages == stageNumber)
        {
            PlayerPrefs.SetInt("UnlockedStages", unlockedStages + 1);
            PlayerPrefs.Save();
        }
    }

    private void ButtonSfx() 
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("button " + _buttonSfxIndex);
            if (_buttonSfxIndex == 1)
                _buttonSfxIndex = 2;
            else
                _buttonSfxIndex = 1;
        }
    }
    #endregion
}
