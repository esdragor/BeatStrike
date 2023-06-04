using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public struct SoundData
{
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    [SerializeField] private AudioClip struct60BPM;
    [SerializeField] private AudioClip struct80BPM;
    [SerializeField] private AudioClip struct90BPM;
    [SerializeField] private AudioClip struct100BPM;
    [SerializeField] private AudioClip struct120BPM;
    [SerializeField] private AudioClip struct140BPM;
    [SerializeField] private AudioClip struct160BPM;
    [SerializeField] private AudioClip struct180BPM;
    [SerializeField] private AudioClip struct200BPM;
    public AudioClip[] clipNotePlayerDodge;
    public AudioClip[] clipNotePlayerAttack;

    [Header("Musical Background")] [SerializeField]
    private AudioSource audioSourceMusicalBackground;

    [Header("Note Player")] [SerializeField]
    private AudioSource[] audioSourceNotePlayer;

    private void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);
        instance = this;
    }

    public static void PlayRandomBackground(int BPM)
    {
        instance.audioSourceMusicalBackground.clip = BPM switch
        {
            60 => instance.struct60BPM,
            80 => instance.struct80BPM,
            90 => instance.struct90BPM,
            100 => instance.struct100BPM,
            120 => instance.struct120BPM,
            140 => instance.struct140BPM,
            160 => instance.struct160BPM,
            180 => instance.struct180BPM,
            200 => instance.struct200BPM,
            _ => null
        };
        if (instance.audioSourceMusicalBackground.clip)
            instance.audioSourceMusicalBackground.Play();
    }
    
    public static void PauseUnpauseBackground(bool pause)
    {
        if (pause)
            instance.audioSourceMusicalBackground.Pause();
        else
            instance.audioSourceMusicalBackground.UnPause();
    }

    public static void PlayRandomDodgeNotePlayer()
    {
        int sizeAudioSource = instance.audioSourceNotePlayer.Length;
        AudioSource source = null;
        for (int i = 0; i < sizeAudioSource; i++)
        {
            if (!instance.audioSourceNotePlayer[i] || instance.audioSourceNotePlayer[i].isPlaying) continue;
            source = instance.audioSourceNotePlayer[i];
            break;
        }

        if (source == null) return;
        source.clip = instance.clipNotePlayerDodge[
            Random.Range(0, instance.clipNotePlayerDodge.Length)];
        if (source.clip)
            source.Play();
    }

    public static void PlayRandomAttackNotePlayer()
    {
        int sizeAudioSource = instance.audioSourceNotePlayer.Length;
        AudioSource source = null;
        for (int i = 0; i < sizeAudioSource; i++)
        {
            if (!instance.audioSourceNotePlayer[i] || instance.audioSourceNotePlayer[i].isPlaying) continue;
            source = instance.audioSourceNotePlayer[i];
            break;
        }

        if (source == null) return;
        source.clip = instance.clipNotePlayerAttack[
                Random.Range(0, instance.clipNotePlayerAttack.Length)];
        if (source.clip)
            source.Play();
    }

    private static bool isMute;

    public static bool ToggleMute()
    {
        isMute = !isMute;

        instance.audioSourceMusicalBackground.mute = isMute;
        foreach (var s in instance.audioSourceNotePlayer)
        {
            s.mute = isMute;
        }

        return isMute;
    }
}