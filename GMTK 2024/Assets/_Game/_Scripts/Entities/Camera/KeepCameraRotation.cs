using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepCameraRotation : MonoBehaviour
{
    private void Update() => transform.rotation = Quaternion.Euler(0f, 0f, 0f);
}
