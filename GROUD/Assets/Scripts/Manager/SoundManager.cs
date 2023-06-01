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
    [SerializeField] private SoundData struct80BPM;
    [SerializeField] private SoundData struct90BPM;
    [SerializeField] private SoundData struct100BPM;
    [SerializeField] private SoundData struct120BPM;
    [SerializeField] private SoundData struct140BPM;
    [SerializeField] private SoundData struct160BPM;
    [SerializeField] private SoundData struct180BPM;
    [SerializeField] private SoundData struct200BPM;

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
            60 => instance.struct60BPM.clipBackground,
            80 => instance.struct80BPM.clipBackground,
            90 => instance.struct90BPM.clipBackground,
            100 => instance.struct100BPM.clipBackground,
            120 => instance.struct120BPM.clipBackground,
            140 => instance.struct140BPM.clipBackground,
            160 => instance.struct160BPM.clipBackground,
            180 => instance.struct180BPM.clipBackground,
            200 => instance.struct200BPM.clipBackground,
            _ => null
        };
        if (instance.audioSourceMusicalBackground.clip)
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
            80 => instance.struct80BPM.clipNotePlayerGood[
                Random.Range(0, instance.struct60BPM.clipNotePlayerGood.Length)],
            90 => instance.struct90BPM.clipNotePlayerGood[
                Random.Range(0, instance.struct60BPM.clipNotePlayerGood.Length)],
            100 => instance.struct100BPM.clipNotePlayerGood[
                Random.Range(0, instance.struct60BPM.clipNotePlayerGood.Length)],
            120 => instance.struct120BPM.clipNotePlayerGood[
                Random.Range(0, instance.struct60BPM.clipNotePlayerGood.Length)],
            140 => instance.struct140BPM.clipNotePlayerGood[
                Random.Range(0, instance.struct60BPM.clipNotePlayerGood.Length)],
            160 => instance.struct60BPM.clipNotePlayerGood[
                Random.Range(0, instance.struct160BPM.clipNotePlayerGood.Length)],
            180 => instance.struct180BPM.clipNotePlayerGood[
                Random.Range(0, instance.struct60BPM.clipNotePlayerGood.Length)],
            200 => instance.struct200BPM.clipNotePlayerGood[
                Random.Range(0, instance.struct60BPM.clipNotePlayerGood.Length)],
            _ => null
        };
        if (source.clip)
            source.Play();
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
            80 => instance.struct80BPM.clipNotePlayerBad[
                Random.Range(0, instance.struct60BPM.clipNotePlayerBad.Length)],
            90 => instance.struct90BPM.clipNotePlayerBad[
                Random.Range(0, instance.struct60BPM.clipNotePlayerBad.Length)],
            100 => instance.struct100BPM.clipNotePlayerBad[
                Random.Range(0, instance.struct60BPM.clipNotePlayerBad.Length)],
            120 => instance.struct120BPM.clipNotePlayerBad[
                Random.Range(0, instance.struct60BPM.clipNotePlayerBad.Length)],
            140 => instance.struct140BPM.clipNotePlayerBad[
                Random.Range(0, instance.struct60BPM.clipNotePlayerBad.Length)],
            160 => instance.struct60BPM.clipNotePlayerBad[
                Random.Range(0, instance.struct160BPM.clipNotePlayerBad.Length)],
            180 => instance.struct180BPM.clipNotePlayerBad[
                Random.Range(0, instance.struct60BPM.clipNotePlayerBad.Length)],
            200 => instance.struct200BPM.clipNotePlayerBad[
                Random.Range(0, instance.struct60BPM.clipNotePlayerBad.Length)],
            _ => null
        };
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