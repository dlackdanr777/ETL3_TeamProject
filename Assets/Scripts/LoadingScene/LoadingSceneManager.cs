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


/// <summary>�ε� ���� �����ϴ� �Ŵ���</summary>
public class LoadingSceneManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image _loadingBarImage;


    [Header("Values")]
    [Tooltip("�ε��ð��� �ּ� �� ������ ����")]
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

        //�� �ε��� �ȳ����� ��� �ݺ�
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
