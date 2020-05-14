using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceCompleteScreen : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneFader.instance.QueueBaseScene("Overview");
        SceneFader.instance.FadeOut();
    }
}
