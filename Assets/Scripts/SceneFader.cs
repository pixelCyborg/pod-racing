using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;
    private static Image rend;
    private float fadeTime = 0.5f;

    private List<QueuedScene> queuedScenes = new List<QueuedScene>();

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        rend = GetComponentInChildren<Image>();
        rend.DOFade(0.0f, 0.0f);
        rend.enabled = false;
    }

    public void QueueBaseScene(string scene)
    {
        QueueScene(scene, LoadSceneMode.Single);
    }

    public void QueueAdditiveScene(string scene)
    {
        QueueScene(scene, LoadSceneMode.Additive);
    }

    public void QueueScene(string scene, LoadSceneMode mode)
    {
        queuedScenes.Add(new QueuedScene(scene, mode));
    }

    public void FadeOut()
    {
        FadeOut(() =>
        {
            for(int i = 0; i < queuedScenes.Count; i++)
            {
                if(queuedScenes[i].mode == LoadSceneMode.Single)
                    SceneManager.LoadScene(queuedScenes[i].scene, queuedScenes[i].mode);
            }

            for (int i = 0; i < queuedScenes.Count; i++)
            {
                if(queuedScenes[i].mode == LoadSceneMode.Additive)
                    SceneManager.LoadScene(queuedScenes[i].scene, queuedScenes[i].mode);
            }
            queuedScenes = new List<QueuedScene>();
        });
    }

    private void FadeOut(System.Action callback)
    {
        rend.DOFade(0.0f, 0.0f);
        rend.enabled = true;
        rend.DOFade(1.0f, fadeTime).OnComplete(() =>
        {
            callback();
            FadeIn();
        });
    }

    private void FadeIn()
    {
        rend.DOFade(1.0f, 0.0f);
        rend.enabled = true;
        rend.DOFade(0.0f, fadeTime).OnComplete(() =>
        {
            rend.enabled = false;
        });
    }

    IEnumerator LoadScenes()
    {
        yield return null;
    }

    // Update is called once per frame
    private struct QueuedScene{
        public string scene;
        public LoadSceneMode mode;

        public QueuedScene(string _scene, LoadSceneMode _mode)
        {
            scene = _scene;
            mode = _mode;
        }
    }
}
