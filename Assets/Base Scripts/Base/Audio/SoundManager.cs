using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Sound
{
    POPUP_CLOSE,
    POPUP_OPEN,
    CLICK,
    SCREAM,
    WIN,
    CHICKEN_DYING
}

public class SoundManager : MonoSingletonGlobal<SoundManager>
{
    [System.Serializable]
    public class SoundTable
    {
        public Sound sound;
        public AudioClip clip;
    }

    private Coroutine corChangeVolume;
    [SerializeField] AudioSource audioSourceNormal;
    [SerializeField] AudioSource audioSourceSpecial;

    [SerializeField] private SoundTable[] sounds;
    private Dictionary<Sound, AudioClip> soundDics = new Dictionary<Sound, AudioClip>();
    private Queue<Audio3D> queue3d = new Queue<Audio3D>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var _s in sounds)
        {
            soundDics.Add(_s.sound, _s.clip);
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => RuntimeStorageData.SOUND != null);

        audioSourceNormal = GetComponent<AudioSource>();
        audioSourceNormal.mute = !RuntimeStorageData.SOUND.isSound;
        audioSourceSpecial.mute = !RuntimeStorageData.SOUND.isSound;
        StartCoroutine(TurnOnSoundAwake());

        queue3d.Clear();
    }

    public void PlayOnShot(Sound sound, float volume = 1f)
    {
        AudioClip clip = ConvertToClip(sound);
        audioSourceNormal.PlayOneShot(clip);
    }

    public void PlayOnShot(AudioClip sound, float volume = 1f)
    {
        audioSourceNormal.PlayOneShot(sound);
    }

    public IEnumerator PlayOnShotCustom(Sound sound, float volume = 1f, float after = 0, int numberPlay = 1)
    {
        yield return WaitForSecondCache.GetWFSCache(after);
        for (int i = 0; i < numberPlay; i++)
        {
            AudioClip clip = ConvertToClip(sound);
            audioSourceNormal.PlayOneShot(clip);

            yield return WaitForSecondCache.GetWFSCache(clip.length);
        }
    }

    public void PlayLoopInfinity(Sound sound)
    {
        AudioClip clip = ConvertToClip(sound);
        audioSourceNormal.clip = clip;
        audioSourceNormal.Play();
    }

    public void Stop()
    {
        audioSourceNormal.clip = null;
        audioSourceNormal.Stop();
    }

    public void PlaySoundAsync(Sound sound)
    {
        if (!isPlayingAsync)
        {
            AudioClip clip = ConvertToClip(sound);
            float length = GetSoundLength(sound);
            StartCoroutine(PlaySoundWithUpdate(clip, length));
        }
    }

    public void PlaySoundAsyncWithDelay(Sound sound, float delay)
    {
        if (!isPlayingAsync)
        {
            AudioClip clip = ConvertToClip(sound);
            float length = GetSoundLength(sound);
            StartCoroutine(PlaySoundWithUpdate(clip, length + delay));
        }
    }    

    private bool isPlayingAsync = false;
    IEnumerator PlaySoundWithUpdate(AudioClip clip, float length, float volume = 1f)
    {
        isPlayingAsync = true;
        audioSourceNormal.PlayOneShot(clip);
        yield return WaitForSecondCache.GetWFSCache(length);
        isPlayingAsync = false;
    }    

    public void PlaySoundWithCounter(Sound sound, int counter)
    {
        AudioClip clip = ConvertToClip(sound);
        float length = GetSoundLength(sound);
        StartCoroutine(PlaySoundWithDelay(clip, length, counter));
    }

    IEnumerator PlaySoundWithDelay(AudioClip clip, float sLength, int counter)
    {
        int t = 0;
        while (t < counter)
        {
            audioSourceNormal.PlayOneShot(clip);
            t += 1;
            yield return WaitForSecondCache.GetWFSCache(sLength);
        }
    }

    AudioClip ConvertToClip(Sound sound)
    {
        if (soundDics.ContainsKey(sound))
            return soundDics[sound];
        return null;
    }


    /// <summary>
    /// V==1  mở âm thanh
    /// </summary>
    /// <param name="v"></param>
    public void Turn(bool isEnable)
    {
        audioSourceSpecial.mute = !isEnable;
        audioSourceNormal.mute = !isEnable;
        RuntimeStorageData.SOUND.isSound = isEnable;
    }

    public float GetSoundLength(Sound sound)
    {
        AudioClip clip = ConvertToClip(sound);
        return clip.length;
    }

    //public void ChangeVolume(float target)
    //{
    //    if (corChangeVolume != null)
    //    {
    //        StopCoroutine(corChangeVolume);
    //    }
    //    corChangeVolume = StartCoroutine(RunChangeVolume(target));
    //}

    //IEnumerator RunChangeVolume(float target)
    //{
    //    float speed = target - audioSourceNormal.volume;
    //    speed = speed / 20;
    //    if (speed > 0)
    //    {
    //        while (audioSourceNormal.volume < target)
    //        {
    //            audioSourceNormal.volume += speed;
    //            yield return null;
    //        }
    //    }
    //    else if (speed < 0)
    //    {
    //        while (audioSourceNormal.volume > target)
    //        {
    //            audioSourceNormal.volume += speed;
    //            yield return null;
    //        }
    //    }
    //    audioSourceNormal.volume = target;
    //}

    public IEnumerator PlayOnShotSpecial(Sound sound, float volume = 1f, float after = 0, int numberPlay = 1)
    {
        yield return WaitForSecondCache.GetWFSCache(after);
        for (int i = 0; i < numberPlay; i++)
        {
            AudioClip clip = ConvertToClip(sound);
            audioSourceSpecial.PlayOneShot(clip);
            yield return WaitForSecondCache.GetWFSCache(clip.length);
        }
    }

    public void PlayLoopSpecial(Sound sound)
    {
        AudioClip clip = ConvertToClip(sound);
        audioSourceSpecial.clip = clip;
        audioSourceSpecial.Play();
    }

    public void StopSpecial()
    {
        audioSourceNormal.clip = null;
        audioSourceNormal.Stop();
    }

    private IEnumerator TurnOnSoundAwake()
    {
        audioSourceNormal.enabled = false;
        audioSourceSpecial.enabled = false;
        yield return WaitForSecondCache.GetWFSCache(2f);
        audioSourceNormal.enabled = true;
        audioSourceSpecial.enabled = true;
    }

    public bool isEnable
    {
        get { return RuntimeStorageData.SOUND.isSound; }
        set
        {
            Turn(value);
        }
    }
    //public float DefaultVolume
    //{
    //    get { return audioSourceNormal.volume; }
    //    set
    //    {
    //        ChangeVolume(value);
    //    }
    //}

    public void PlaySound(Sound id, float volumeMultiply = 1)
    {
        PlayOnShot(id, volumeMultiply);
    }

    public void PlaySoundAtLocation(Sound id, Vector3 worldPosition, float volumeMultiply = 1)
    {
        if (queue3d.Count == 0)
        {
            var _obj = new GameObject("AudioSource", typeof(Audio3D), typeof(AudioSource));
            _obj.transform.parent = transform;
            queue3d.Enqueue(_obj.GetComponent<Audio3D>());
        }

        var clip = ConvertToClip(id);
        var audio3D = queue3d.Dequeue();
        audio3D.SpawnAudio3D(clip, worldPosition, volumeMultiply,
            () =>
            {
                queue3d.Enqueue(audio3D);
            });
    }

    // test spawn sound 3d
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Services.Find(out ISoundManager service);
    //        service.PlaySoundAtLocation(Sound.BUTTON_CLICK, Vector3.one);
    //    }
    //}
}
