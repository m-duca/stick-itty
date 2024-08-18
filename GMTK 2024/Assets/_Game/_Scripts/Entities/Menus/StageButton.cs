using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    [SerializeField] private string selectedStage;
    public void PlayStage()
    {
        SceneManager.LoadScene(selectedStage);
    }
}
