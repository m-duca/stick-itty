using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class FadeVolume : MonoBehaviour
{
    #region Variáveis Globais
    private float _masterVolume;
    #endregion

    #region Funções Unity
    private void Start()
    {
        AudioListener.volume = 0f;
        _masterVolume = PlayerPrefs.GetFloat("masterVolume");
    }

    private void Update() => ApplyFade();
    #endregion

    #region Funções Próprias
    private void ApplyFade()
    {
        if (AudioListener.volume < _masterVolume)
        {
            AudioListener.volume += 0.25f * Time.deltaTime;
        }
        else
        {
            AudioListener.volume = _masterVolume;
            this.enabled = false;
        }
    }
    #endregion
}
