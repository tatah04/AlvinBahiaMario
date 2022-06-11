using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SoundManager soundManagerInstance;
    public List<AudioClip> audioClips = new List<AudioClip>();
    public AudioSource sfxSource;
    public AudioSource bgmSource;
    [SerializeField] Image imageBgm;
    [SerializeField] Image imageSfx;
    [SerializeField] Sprite on;
    [SerializeField] Sprite off;
    private void Awake()
    {
        soundManagerInstance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySfx(int soundInt)
    {
        soundManagerInstance.sfxSource.Stop();
        soundManagerInstance.sfxSource.clip = soundManagerInstance.audioClips[soundInt];
        soundManagerInstance.sfxSource.Play();
    }
    public static void PlayBgm(int soundInt)
    {
        soundManagerInstance.bgmSource.Stop();
        soundManagerInstance.bgmSource.clip = soundManagerInstance.audioClips[soundInt];
        soundManagerInstance.bgmSource.Play();
    }

    public void toggleSoundSource(string sourceStr)
    {
        if (sourceStr=="bgm")
        {
            bgmSource.mute = !bgmSource.mute;
            if (bgmSource.mute)
            {
                imageBgm.sprite = off;
            }
            else
            {
                imageBgm.sprite = on;
            }
        }
        if (sourceStr=="sfx")
        {
            sfxSource.mute = !sfxSource.mute;
            if (sfxSource.mute)
            {
                imageSfx.sprite = off;
            }
            else
            {
                imageSfx.sprite = on;
            }
        }
      
    }

}
