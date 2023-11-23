using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

/// <summary>
/// LoadScene制御クラス
/// </summary>
public class LoadSceneController : MonoBehaviour
{
    /// <summary>
    /// ロード中テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI loadingText;

    /// <summary>
    /// ロード中の文字列
    /// </summary>
    private static readonly string baseText = "Now Loading";
    /// <summary>
    /// ロード完了の文字列
    /// </summary>
    private static readonly string completeText = "COMPLETE!";


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        Debug.Log(string.Format("{0} version:{1}", Application.productName, Application.version));
    }

    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        if (AtsumaruAPI.Instance.IsValid())
        {
            if (AtsumaruAPI.Instance.LoadServerData())
            {
                SetText(0);
                loadingText.gameObject.SetActive(true);
                StartCoroutine(LoadingCoroutine());
            }
            else
            {
                SceneManager.LoadScene("TitleScene");
            }
        }
        else
        {
            if (LocalStorageAPI.Instance.LoadLocalData())
            {
                SetText(0);
                loadingText.gameObject.SetActive(true);
                StartCoroutine(LoadingCoroutine());
            }
            else
            {
                SceneManager.LoadScene("TitleScene");
            }
        }
    }

    /// <summary>
    /// ServerData をロードする
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator LoadingCoroutine()
    {
        bool isEnd = false;
        float timer = 0.0f;
        float interval = 0.25f;
        float loadTimeoutTimer = 0.0f;
        float loadTimeout = 60.0f;
        int dotCount = 0;
        int maxDotCount = 3;
        float displayCompleteTime = 0.5f;
        float displayMinimumTimer = 0.0f;
        float minimumTime = 0.5f;

        while (!isEnd)
        {
            yield return null;

            displayMinimumTimer += Time.deltaTime;
            if (CheckLoadComplete() && displayMinimumTimer >= minimumTime)
            {
                isEnd = true;
            }

            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0.0f;
                dotCount++;
                dotCount %= (maxDotCount + 1);
                SetText(dotCount);
            }

            loadTimeoutTimer += Time.deltaTime;
            if (loadTimeoutTimer >= loadTimeout)
            {
                Debug.Log("LoadData timeout.");
                isEnd = true;
            }
        }

        if (AtsumaruAPI.Instance.IsValid())
        {
            if (AtsumaruAPI.Instance.IsServerDataLoaded())
            {
                string json = AtsumaruAPI.Instance.GetServerDataJson();
                // Debug.Log(json);
                AtsumaruAPI.ServerDataItems serverDataItems = JsonUtility.FromJson<AtsumaruAPI.ServerDataItems>(json);
                if (serverDataItems != null && serverDataItems.data != null)
                {
                    foreach (var item in serverDataItems.data)
                    {
                        if (item.key == MyRecords.key)
                        {
                            // Debug.Log(item.value);
                            MyRecords myRecords = JsonUtility.FromJson<MyRecords>(item.value);
                            RecordManager.Instance.Restore(myRecords);
                        }
                    }
                }
            }

            SetCompleteText();
            yield return new WaitForSeconds(displayCompleteTime);
        }
        else
        {
            if (LocalStorageAPI.Instance.IsLocalDataLoaded())
            {
                string json = LocalStorageAPI.Instance.GetLocalDataJson();
                // Debug.Log(json);
                AtsumaruAPI.ServerDataItems serverDataItems = JsonUtility.FromJson<AtsumaruAPI.ServerDataItems>(json);
                if (serverDataItems != null && serverDataItems.data != null)
                {
                    foreach (var item in serverDataItems.data)
                    {
                        if (item.key == MyRecords.key)
                        {
                            // Debug.Log(item.value);
                            MyRecords myRecords = JsonUtility.FromJson<MyRecords>(item.value);
                            RecordManager.Instance.Restore(myRecords);
                        }
                    }
                }
            }
            SetCompleteText();
            yield return new WaitForSeconds(displayCompleteTime);
        }

        SceneManager.LoadScene("TitleScene");
    }

    /// <summary>
    /// ロード中かどうかを判定する。
    /// </summary>
    /// <returns>ロード中の場合はtrue、そうでない場合はfalseを返す</returns>
    private bool CheckLoadComplete()
    {
        if (AtsumaruAPI.Instance.IsValid())
        {
            if (!AtsumaruAPI.Instance.IsServerDataLoaded())
            {
                return false;
            }
        }
        else
        {
            if (!LocalStorageAPI.Instance.IsLocalDataLoaded())
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Loding + . を表示する
    /// </summary>
    /// <param name="dotCount"></param>
    private void SetText(int dotCount)
    {
        string text = baseText;
        for (int i = 0; i < dotCount; i++)
        {
            text += ".";
        }
        loadingText.SetText(text);
    }

    /// <summary>
    /// Complete を表示する
    /// </summary>
    private void SetCompleteText()
    {
        loadingText.SetText(completeText);
    }
}
