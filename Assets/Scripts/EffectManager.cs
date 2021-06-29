using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectManager : MonoBehaviour
{

    [SerializeField]
    KeywordTrigger kt;
    [SerializeField]
    UIManager uiManager;
    //[SerializeField]
    //GameObject celebrationPrefab;
    [SerializeField]
    AudioClip[] celebrationAudioClips;

    //[SerializeField]
    //GameObject laughterPrefab;
    //[SerializeField]
    //AudioClip[] laughterAudioClips;

    //[SerializeField]
    //GameObject tikTokPrefab;
    //[SerializeField]
    //AudioClip[] tikTokAudioClips;
    [SerializeField]
    GameObject arCamera;

    private ColorAdjustments colorAdjust;
    private Vignette vignette;
    private float startTime;
    float direction = 1;



    public enum EffectType { Celebration, Laughter, TikTok}

    public EffectType effectType = EffectType.Celebration;


    private void Start()
    {
        kt.KeywordTriggered.AddListener(PlayEffect);
        startTime = Time.time;
    }

    public void PlayEffect()
    {
        switch (effectType)
        {
            case EffectType.Celebration:
                PlayCelebration();
                break;
            case EffectType.Laughter:
                PlayLaughter();
                break;
            case EffectType.TikTok:
                PlayTikTok();
                break;
            default:
                break;
        }

    }

    private void PlayCelebration()
    {
        GameObject[] celebrationPrefabs = GameObject.FindGameObjectsWithTag("celebration");
        foreach(GameObject prefab in celebrationPrefabs)
        {
            ParticleSystem[] celebrationParticleSystems = prefab.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in celebrationParticleSystems)
            {
                ps.Play();
            }
        }
       

        foreach(AudioClip audioClip in celebrationAudioClips)
        {
            StartCoroutine(PlayAudio(audioClip));
        }
        StartEffects();
        

    }

    private void PlayLaughter()
    {

    }

    private void PlayTikTok()
    {

    }
    private void StartEffects()
    {
        StartCoroutine(ChangeHue());
        StartCoroutine(ChangeVignette());
    }

    private void EndEffects() {
        uiManager.EnableReset();
    }


    private IEnumerator PlayAudio(AudioClip audio)
    {
        GameObject audioObject = new GameObject();
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.volume = 1f;
        audioSource.clip = audio;
        audioSource.Play();
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        //Debug.Log("Audio finished playing");
        Destroy(audioObject);
    }


    private IEnumerator ChangeHue()
    {
        startTime = Time.time;
        float duration = Time.time + 6f;
        Volume volume = arCamera.GetComponent<Volume>();
        ColorAdjustments tmp;
        if (volume.profile.TryGet<ColorAdjustments>(out tmp))
        {
            colorAdjust = tmp;
        }
        while (Time.time < duration)
        {
            colorAdjust.hueShift.Override(Mathf.PingPong((startTime - Time.time) * 100, 180));
            yield return null;
        }
        colorAdjust.hueShift.Override(0f);
        yield return null;

    }

    private IEnumerator ChangeVignette()
    {
        startTime = Time.time;
        float duration = Time.time + 6f;
        Volume volume = arCamera.GetComponent<Volume>();
        Vignette tmp;
        if (volume.profile.TryGet<Vignette>(out tmp))
        {
            vignette = tmp;
        }
        while (Time.time < duration)
        {
            vignette.intensity.Override(Mathf.Lerp(0f, .542f, Time.time * .2f));
            //Debug.Log("vignette value: " + vignette.intensity.value);
            yield return null;
        }
        vignette.intensity.Override(0f);
        EndEffects();
    }
}
