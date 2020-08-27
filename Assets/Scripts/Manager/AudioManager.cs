using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager _instance;

    private AudioSource bg, effect;

    private void Awake()
    {
        _instance = this;

        bg = transform.Find("bg").GetComponent<AudioSource>();
        effect = transform.Find("effect").GetComponent<AudioSource>();

        bg.volume = PlayerPrefs.GetFloat(Const.Music, 0.5f);
        effect.volume = PlayerPrefs.GetFloat(Const.Sound, 0.5f);
    }

    public void PlayerMusic(AudioClip audioClip)
    {
        bg.clip = audioClip;
        bg.loop = true;
        bg.Play();
    }

    public void PlaySound(AudioClip audioClip)
    {
        effect.PlayOneShot(audioClip);
    }

    public void OnMusicVolumeChange(float val)
    {
        bg.volume = val;
    }

    public void OnEffectVolumeChange(float val)
    {
        effect.volume = val;
    }
}
