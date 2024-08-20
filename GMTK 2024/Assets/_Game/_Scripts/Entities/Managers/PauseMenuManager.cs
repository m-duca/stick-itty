using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using 

public class PauseMenuManager : MonoBehaviour
{
    [Header("ReferÍncias:")]
    [SerializeField] private GameObject _pausePanel;
    private bool _paused = false;

    public void Update()
    {
        if (_pausePanel != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_paused)
                {
                    Debug.Log("Pausou");
                    Pause();
                }
                else
                {
                    Debug.Log("Despausou");
                    Continue();
                }
            }
        }
    }

    public void Pause()
    {
        _pausePanel.SetActive(true);
        _paused = true;

        
    }

    public void Continue()
    {
        _pausePanel.SetActive(false);
        _paused = false;
    }
}
