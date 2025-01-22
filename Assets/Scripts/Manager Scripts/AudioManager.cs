using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    [SerializeField] private int BgmIndex;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        if (bgm.Length <= 0)
            return;

        InvokeRepeating(nameof(PlayMusicIfNeeded), 0, 2);

    }

    public void PlaySFX(int sfxToPlay, bool randomPicth = true)
    {
        if (sfxToPlay >= sfx.Length)
            return;
        if (randomPicth)
            sfx[sfxToPlay].pitch = Random.Range(.9f, 1.1f);

        sfx[sfxToPlay].Play();
    }

    public void PlayMusicIfNeeded()
    {
        if (bgm[BgmIndex].isPlaying == false)
            PlayRandomBGM();
    }

    public void PlayRandomBGM()
    {
        BgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(BgmIndex);
    }

    public void PlayBGM(int bgmToPlay)
    {
        if (bgm.Length <= 0)
        {
            Debug.LogWarning("No Music in AudioManager");
            return;
        }


        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }

        BgmIndex = bgmToPlay;
        bgm[bgmToPlay].Play();
    }

    public void StopSFX(int sfxToStop) => sfx[sfxToStop].Stop();
}