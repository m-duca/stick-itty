using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    #region Variáveis
    [Header("Configurações:")]
    [SerializeField] private float bulletRotationZ;
    [SerializeField] private Vector2 bulletMoveDir;
    [SerializeField] private float spawnInterval;

    [Header("Referências:")]
    [SerializeField] private BulletMovement bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    #endregion

    #region Funções Unity
    private void Start() => StartCoroutine(SpawnBullet());
    #endregion

    #region Funções Próprias
    private IEnumerator SpawnBullet() 
    {
        var bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.Euler(0f, 0f, bulletRotationZ));
        bullet.MoveDir = bulletMoveDir.normalized;

        yield return new WaitForSeconds(spawnInterval);

        StartCoroutine(SpawnBullet());
    }
    #endregion
}