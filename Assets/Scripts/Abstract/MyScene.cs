using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class MyScene : MonoBehaviour
{
    [SerializeField]
    protected float fadeInTime;

    [SerializeField]
    protected float fadeOutTime;

    public IEnumerator SceneLoad(float sec, int sceneValue)
    {
        yield return new WaitForSeconds(sec);

        SceneManager.LoadScene(sceneValue);
    }
}
