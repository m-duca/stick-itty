using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private bool canFollow = true;

    [SerializeField] private Vector3 offset;

    [Header("Min/Max Positions:")]
    [SerializeField] private float xMin;
    [SerializeField] private float xMax;
    [SerializeField] private float yMin;
    [SerializeField] private float yMax;

    // References
    private Transform _playerTransf;
    //private float _speed;

    private void Start()
    {
        _playerTransf = FindObjectOfType<PlayerHeadMovement>().transform;
    }

    private void LateUpdate()
    {
        if (canFollow)
        {
            float xClamp = Mathf.Clamp(_playerTransf.position.x, xMin, xMax);
            float yClamp = Mathf.Clamp(_playerTransf.position.y, yMin, yMax);

            Vector3 targetpos = _playerTransf.transform.position + offset;
            Vector3 clampedpos = new Vector3(Mathf.Clamp(targetpos.x, xMin, xMax), Mathf.Clamp(targetpos.y, yMin, yMax), 0);

            SetNewPosition(clampedpos);
        }
    }

    public void SetNewPosition(Vector2 pos)
    {
        Vector3 newPos = (Vector3)pos + new Vector3(0, 0, -10f);
        transform.position = newPos;
    }
}
