using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct SoundData
{
    public AudioClip clipBackground;
    public AudioClip[] clipNotePlayerGood;
    public AudioClip[] clipNotePlayerBad;
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    [SerializeField] private SoundData struct60BPM;
    [SerializeField] private SoundData struct90BPM;
    [SerializeField] private SoundData struct120BPM;
    [SerializeField] private SoundData struct180BPM;
    [SerializeField] private SoundData struct240BPM;
    
    [Header("Musical Background")]
    [SerializeField] private AudioSource audioSourceMusicalBackground;
    
    [Header("Note Player")]
    [SerializeField] private AudioSource[] audioSourceNotePlayer;

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
            60 => instance.struct60BPM.clipBackground,
            90 => instance.struct90BPM.clipBackground,
            120 => instance.struct120BPM.clipBackground,
            180 => instance.struct180BPM.clipBackground,
            240 => instance.struct240BPM.clipBackground,
            _ => instance.audioSourceMusicalBackground.clip
        };
        instance.audioSourceMusicalBackground.Play();
    }
    
    public static void PlayRandomGoodNotePlayer(int BPM)
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
        source.clip = BPM switch
        {
            60 => instance.struct60BPM.clipNotePlayerGood[
                Random.Range(0, instance.struct60BPM.clipNotePlayerGood.Length)],
            90 => instance.struct90BPM.clipNotePlayerGood[
                Random.Range(0, instance.struct90BPM.clipNotePlayerGood.Length)],
            120 => instance.struct120BPM.clipNotePlayerGood[
                Random.Range(0, instance.struct120BPM.clipNotePlayerGood.Length)],
            180 => instance.struct180BPM.clipNotePlayerGood[
                Random.Range(0, instance.struct180BPM.clipNotePlayerGood.Length)],
            240 => instance.struct240BPM.clipNotePlayerGood[
                Random.Range(0, instance.struct240BPM.clipNotePlayerGood.Length)],
            _ => source.clip
        };
    }

    public static void PlayRandomBadNotePlayer(int BPM)
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
        source.clip = BPM switch
        {
            60 => instance.struct60BPM.clipNotePlayerBad[
                Random.Range(0, instance.struct60BPM.clipNotePlayerBad.Length)],
            90 => instance.struct90BPM.clipNotePlayerBad[
                Random.Range(0, instance.struct90BPM.clipNotePlayerBad.Length)],
            120 => instance.struct120BPM.clipNotePlayerBad[
                Random.Range(0, instance.struct120BPM.clipNotePlayerBad.Length)],
            180 => instance.struct180BPM.clipNotePlayerBad[
                Random.Range(0, instance.struct180BPM.clipNotePlayerBad.Length)],
            240 => instance.struct240BPM.clipNotePlayerBad[
                Random.Range(0, instance.struct240BPM.clipNotePlayerBad.Length)],
            _ => source.clip
        };
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