using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LoadingType
{
    Single,
    Multi
}


/// <summary>로딩 씬을 관리하는 매니저</summary>
public class LoadingSceneManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image _loadingBarImage;


    [Header("Values")]
    [Tooltip("로딩시간이 최소 몇 초일지 설정")]
    [SerializeField] private float _minChangeTime;

    private static string _nextScene;


    private void Start()
    {
        _loadingBarImage.fillAmount = 0;
        StartCoroutine(LoadScene());
    }


    public static void LoadScene(string sceneName)
    {
        _nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    
    private IEnumerator LoadScene()
    {
        yield return YieldCache.WaitForSeconds(0.02f);
        AsyncOperation op = SceneManager.LoadSceneAsync(_nextScene);

        op.allowSceneActivation = false;
        float timer = 0;
        float timer2 = 0;

        //씬 로딩이 안끝났을 경우 반복
        while(!op.isDone)
        {
            yield return YieldCache.WaitForSeconds(0.02f);
            timer += 0.02f;

            if (0.9f <= op.progress && _minChangeTime * 0.9f <= timer)
            {
                timer2 += 0.02f;
                _loadingBarImage.fillAmount = 0.9f + (timer2 / (_minChangeTime * 0.1f)) * 0.1f;
                if (_minChangeTime * 0.1f <= timer2)
                {
                    _loadingBarImage.fillAmount = 1;
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
            else
            {
                _loadingBarImage.fillAmount = timer / _minChangeTime;
            }
        }
    }
}
