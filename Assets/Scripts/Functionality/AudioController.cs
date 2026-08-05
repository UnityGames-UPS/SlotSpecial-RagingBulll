using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource bg_adudio;
    [SerializeField] internal AudioSource audioPlayer_wl;
    [SerializeField] internal AudioSource audioPlayer_button;
    [SerializeField] internal AudioSource audioSpin_button;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioClip[] Bonusclips;
    [SerializeField] private AudioSource bg_audioBonus;
    [SerializeField] private AudioSource audioPlayer_Bonus;
    [SerializeField] private AudioClip[] BgClips;


    private void Start()
    {
        // if (bg_adudio) bg_adudio.Play();
        PlayBGAudio(false);
        audioPlayer_button.clip = clips[6];
        audioSpin_button.clip = clips[5];
    }

    private readonly Dictionary<AudioSource, bool> preFocusMuteState = new Dictionary<AudioSource, bool>();
    private bool isForceMuted = false;

    private IEnumerable<AudioSource> AllManagedSources()
    {
        yield return bg_adudio;
        yield return bg_audioBonus;
        yield return audioPlayer_button;
        yield return audioPlayer_wl;
        yield return audioSpin_button;
        yield return audioPlayer_Bonus;
    }

    // Focus-driven — called from both UIManager.OnFocusChanged (JS path) and
    // SlotBehaviour.OnApplicationFocus (native path). Guarded so a duplicate call
    // for the same direction can't clobber the captured "restore to" state.
    internal void SetMuteAll(bool forceMute)
    {
        if (forceMute == isForceMuted) return;
        isForceMuted = forceMute;

        foreach (var source in AllManagedSources())
        {
            if (source == null) continue;
            if (forceMute)
            {
                preFocusMuteState[source] = source.mute;
                source.mute = true;
            }
            else
            {
                source.mute = preFocusMuteState.TryGetValue(source, out bool prevMuted) ? prevMuted : source.mute;
            }
        }
    }

    internal void SwitchBGSound(bool isbonus)
    {
        if (isbonus)
        {
            if (bg_audioBonus) bg_audioBonus.enabled = true;
            if (bg_adudio) bg_adudio.enabled = false;
        }
        else
        {
            if (bg_audioBonus) bg_audioBonus.enabled = false;
            if (bg_adudio) bg_adudio.enabled = true;
        }
    }

    internal void PlayWLAudio(string type)
    {
        Debug.Log("678" + type);
        audioPlayer_wl.loop = false;
        int index = 0;
        switch (type)
        {
            case "bigwin":
                index = 0;
                break;
            case "win":
                index = 1;
                break;
            case "lose":
                index = 2;
                break;
            case "spinStop":
                index = 3;
                break;
            case "megaWin":
                index = 4;
                break;
            case "bullBonus":
                index = 8;
                Debug.Log("bull audio type: " + type);
                break;
            case "coin":
                index = 7;
                break;
            case "baseWin":
                index = 9;
                break;
            case "bull":
                index = 10;
                Debug.Log("bull audio type: " + type);
                break;
            default:
                index = 0; // default clip
                break;

        }
        StopWLAaudio();
        audioPlayer_wl.clip = clips[index];
        audioPlayer_wl.Play();
    }

    internal void PlayButtonAudio()
    {
        audioPlayer_button.Play();
    }

    internal void PlaySpinButtonAudio()
    {
        audioSpin_button.Play();
    }

    internal void StopWLAaudio()
    {
        audioPlayer_wl.Stop();
        audioPlayer_wl.loop = false;
    }

    internal void PlayBGAudio(bool FreespinBG)
    {
        if (bg_adudio) bg_adudio.clip = FreespinBG ? BgClips[1] : BgClips[0];
        bg_adudio.Play();
    }
    internal void ToggleMute(bool toggle, string type)
    {
        AudioSource[] group = type == "music"
            ? new[] { bg_adudio, bg_audioBonus }
            : new[] { audioPlayer_button, audioPlayer_wl, audioSpin_button, audioPlayer_Bonus };

        foreach (var source in group)
        {
            if (source == null) continue;
            // While force-muted (backgrounded), record the user's choice so it's
            // applied on focus regain instead of being clobbered by SetMuteAll(false).
            if (isForceMuted)
                preFocusMuteState[source] = toggle;
            else
                source.mute = toggle;
        }
    }

}
