using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public struct SoundData
{
    public AudioClip clipBackground;
    public AudioClip[] clipNotePlayerDodge;
    public AudioClip[] clipNotePlayerAttack;
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
    
    public static void PauseUnpauseBackground(bool pause)
    {
        if (pause)
            instance.audioSourceMusicalBackground.Pause();
        else
            instance.audioSourceMusicalBackground.UnPause();
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
            60 => instance.struct60BPM.clipNotePlayerDodge[
                Random.Range(0, instance.struct60BPM.clipNotePlayerDodge.Length)],
            80 => instance.struct80BPM.clipNotePlayerDodge[
                Random.Range(0, instance.struct80BPM.clipNotePlayerDodge.Length)],
            90 => instance.struct90BPM.clipNotePlayerDodge[
                Random.Range(0, instance.struct90BPM.clipNotePlayerDodge.Length)],
            100 => instance.struct100BPM.clipNotePlayerDodge[
                Random.Range(0, instance.struct100BPM.clipNotePlayerDodge.Length)],
            120 => instance.struct120BPM.clipNotePlayerDodge[
                Random.Range(0, instance.struct120BPM.clipNotePlayerDodge.Length)],
            140 => instance.struct140BPM.clipNotePlayerDodge[
                Random.Range(0, instance.struct140BPM.clipNotePlayerDodge.Length)],
            160 => instance.struct60BPM.clipNotePlayerDodge[
                Random.Range(0, instance.struct160BPM.clipNotePlayerDodge.Length)],
            180 => instance.struct180BPM.clipNotePlayerDodge[
                Random.Range(0, instance.struct180BPM.clipNotePlayerDodge.Length)],
            200 => instance.struct200BPM.clipNotePlayerDodge[
                Random.Range(0, instance.struct200BPM.clipNotePlayerDodge.Length)],
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
            60 => instance.struct60BPM.clipNotePlayerAttack[
                Random.Range(0, instance.struct60BPM.clipNotePlayerAttack.Length)],
            80 => instance.struct80BPM.clipNotePlayerAttack[
                Random.Range(0, instance.struct80BPM.clipNotePlayerAttack.Length)],
            90 => instance.struct90BPM.clipNotePlayerAttack[
                Random.Range(0, instance.struct90BPM.clipNotePlayerAttack.Length)],
            100 => instance.struct100BPM.clipNotePlayerAttack[
                Random.Range(0, instance.struct100BPM.clipNotePlayerAttack.Length)],
            120 => instance.struct120BPM.clipNotePlayerAttack[
                Random.Range(0, instance.struct120BPM.clipNotePlayerAttack.Length)],
            140 => instance.struct140BPM.clipNotePlayerAttack[
                Random.Range(0, instance.struct140BPM.clipNotePlayerAttack.Length)],
            160 => instance.struct160BPM.clipNotePlayerAttack[
                Random.Range(0, instance.struct160BPM.clipNotePlayerAttack.Length)],
            180 => instance.struct180BPM.clipNotePlayerAttack[
                Random.Range(0, instance.struct180BPM.clipNotePlayerAttack.Length)],
            200 => instance.struct200BPM.clipNotePlayerAttack[
                Random.Range(0, instance.struct200BPM.clipNotePlayerAttack.Length)],
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