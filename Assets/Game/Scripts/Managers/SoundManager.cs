using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    public class SFXData
    {
        public string name;
        public AudioClip clip;
    }

    [SerializeField] private List<SFXData> sfxList;
    [SerializeField] private AudioSource sfxSource;

    private Dictionary<string, AudioClip> sfxDictionary;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Convert list → dictionary for fast lookup
        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (var sfx in sfxList)
        {
            sfxDictionary[sfx.name] = sfx.clip;
        }
    }

    public void PlaySFX(string name)
    {
        if (sfxDictionary.TryGetValue(name, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SFX not found: " + name);
        }
    }
}