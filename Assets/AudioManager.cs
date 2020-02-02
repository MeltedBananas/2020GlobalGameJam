using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundsBank { TalkSpeech }
    
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private AudioClip _talkSpeechSound = null;

    public void PlaySound(SoundsBank soundId, bool isLooping = true)
    {
        switch (soundId)
        {
            case SoundsBank.TalkSpeech:
                _audioSource.clip = _talkSpeechSound;
                break;
        }
        
        _audioSource.Play();
        _audioSource.loop = isLooping;
    }

    public void StopSound()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}
