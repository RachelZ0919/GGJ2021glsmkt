using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;

    public Dictionary<string, AudioClip> audios;
    private AudioSource audioSource;

    private void Start()
    {
        audios = new Dictionary<string, AudioClip>();
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < audioClips.Count; i++)
        {
            if(audioClips[i] == null)
            {
                Debug.LogError($"Audio[{i}] has not assigned yet.");
            }
            else
            {
                audios.Add(audioClips[i].name, audioClips[i]);
            }
        }
        audioClips.Clear();
    }


    public void PlayAudio(string name)
    {
        if (audios.ContainsKey(name))
        {
            audioSource.PlayOneShot(audios[name]);
        }
        else
        {
            Debug.LogError("Audio " + name + " is not in the preset dictionary!");
        }
    }

    public void PlayAudio(AudioClip clip)
    {
        if(clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {

        }
    }
}
