using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WaterLightAnimation : MonoBehaviour
{
    [Header("Configurações:")]
    [SerializeField] private Light2D _light;

    private void SetLightSprite(Sprite newSprite) => _light.lightCookieSprite = newSprite;
}
