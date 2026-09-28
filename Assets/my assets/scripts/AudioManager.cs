using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Settings")]
    public List<TimedAudio> audioTracks = new List<TimedAudio>();
    public float crossfadeDuration = 2f; // Seconds for smooth fade

    private AudioSource sourceA;
    private AudioSource sourceB;
    private AudioSource activeSource;

    private float timer = 0f;
    private int nextTrackIndex = 0;
    private bool isFading = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            sourceA = gameObject.AddComponent<AudioSource>();
            sourceB = gameObject.AddComponent<AudioSource>();
            activeSource = sourceA;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Play next track at the correct time
        if (!isFading && nextTrackIndex < audioTracks.Count && timer >= audioTracks[nextTrackIndex].startTime)
        {
            StartCoroutine(CrossfadeTo(audioTracks[nextTrackIndex].clip));
            nextTrackIndex++;
        }
    }

    private IEnumerator CrossfadeTo(AudioClip newClip)
    {
        isFading = true;

        AudioSource newSource = (activeSource == sourceA) ? sourceB : sourceA;
        newSource.clip = newClip;
        newSource.volume = 0f;
        newSource.Play();

        float elapsed = 0f;
        float startVol = activeSource.volume;

        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / crossfadeDuration;

            activeSource.volume = Mathf.Lerp(startVol, 0f, t);
            newSource.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        activeSource.Stop();
        newSource.volume = 1f;
        activeSource = newSource;

        isFading = false;
    }

    // Reset manager (optional, if restarting game)
    public void ResetAudio()
    {
        timer = 0f;
        nextTrackIndex = 0;
        activeSource.Stop();
    }
}
