using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 歓声用のサウンドマネージャ
/// </summary>
public class ApplauseManager : SoundManager
{
    public static ApplauseManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        CraeteAudioMap();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 指定したSEを再生する。
    /// </summary>
    /// <param name="type">再生するSEのタイプ</param>
    public void PlaySe(SeType type, float volume)
    {
        // Debug.Log(string.Format("PlaySe() called. type={0}", type));

        string strType = type.ToString();
        PlaySe(strType, volume);
    }

    /// <summary>
    /// 指定したSEを再生する
    /// </summary>
    /// <param name="strType">再生するSEのタイプ名</param>
    public void PlaySe(string strType, float volume)
    {
        // Debug.Log(string.Format("PlaySe() called. strType={0}", strType));
        if (audioMap.ContainsKey(strType))
        {
            AudioClip clip = audioMap[strType];
            if (clip != null)
            {
                PlayOneShot(clip);
                SetVolume(volume);
            }
            else
            {
                Debug.Log(string.Format("PlaySe() '{0}' audioClip is null.", strType));
            }
        }
        else
        {
            Debug.Log(string.Format("PlaySe() '{0}' not found.", strType));
        }
    }

    /// <summary>
    /// 再生中のSeを停止する
    /// </summary>
    public void StopSe()
    {
        stopAudioClip();
    }

    /// <summary>
    /// 再生中のSEをフェードアウトさせ、その後停止する。
    /// </summary>
    /// <param name="duration">フェード時間（秒）</param>
    public void FadeStopSe(float duration)
    {
        fadeStopAudioClip(duration);
    }

    /// <summary>
    /// 再生中のSEのボリュームを変更する。
    /// </summary>
    /// <param name="volume">ボリューム</param>
    private void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
