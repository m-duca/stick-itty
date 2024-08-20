using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    private GameObject _pausePanel;

    private void Update()
    {

    }

    public void Pause()
    {
        _pausePanel.SetActive(true);
    }

    public void Continue()
    {
        _pausePanel.SetActive(false);
    }
}
