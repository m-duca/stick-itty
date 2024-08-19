using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private string musicName;

    private void Start() => AudioManager.Instance.PlayMusic(musicName);
}