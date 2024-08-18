using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    #region Variáveis
    [Header("Screen Shake Settings:")]
    [SerializeField] private float animationSpeed;

    private Animator camAnim;
    #endregion


    #region Funções Unity
    private void Awake()
    {
        camAnim = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        camAnim.speed = animationSpeed;
    }
    #endregion

    #region Funções Próprias
    public void ApplyScreenShake()
    {
        camAnim.SetTrigger("shake");
        camAnim.SetInteger("random", Random.Range(1, 3));
    }
    #endregion
}
