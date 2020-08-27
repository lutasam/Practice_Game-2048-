using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Set : View
{

    public Slider slider_sound;
    public Slider slider_music;


    //音效
    public void SoundSet(float f)
    {
        AudioManager._instance.OnEffectVolumeChange(f);
        PlayerPrefs.SetFloat(Const.Sound, f);
    }

    //音乐
    public void MusicSet(float f)
    {
        AudioManager._instance.OnMusicVolumeChange(f);
        PlayerPrefs.SetFloat(Const.Music, f);
    }

    public override void Show()
    {
        base.Show();
        slider_sound.value = PlayerPrefs.GetFloat(Const.Sound, 0);
        slider_music.value = PlayerPrefs.GetFloat(Const.Music, 0);
    }
}
