using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDroppingSFX : MonoBehaviour
{
    #region Funções Unity
    private void Start() => StartCoroutine(PlaySFX());
    #endregion

    #region Funções Próprias
    private IEnumerator PlaySFX() 
    {
        AudioManager.Instance.PlaySFX("water dropping");
        yield return new WaitForSeconds(22.5f);
        StartCoroutine(PlaySFX());
    }
    #endregion
}
