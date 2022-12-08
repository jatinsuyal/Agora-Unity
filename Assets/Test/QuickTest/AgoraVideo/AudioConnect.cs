using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
//using Utilities;

public class AudioConnect : MonoBehaviour
{
    private/*public*/ AgoraInfo agoraInfo;
    private IRtcEngine rtcEngine;

    public void Init()
    {
        agoraInfo = FindObjectOfType<DataBucket>().agoraInfo;

        rtcEngine = GetComponent<AVManager>().GetCurrentEngine();

        if (agoraInfo.IsInstructor)
        {
            DisableLocalAudio();

            //LogToFile.Log("Disabled local audio first time.");

            //StartCoroutine(CheckAudioRecordingDevice());
        }
        else
        {
            DisableLocalAudio();
        }
    }

    public void EnableUserAudio(uint _uid)
    {
        rtcEngine.AdjustUserPlaybackSignalVolume(_uid, 100);//Was 100 before
    }

    public void DisableUserAudio(uint _uid)
    {
        rtcEngine.AdjustUserPlaybackSignalVolume(_uid, 0);
    }

    public void EnableLocalAudio()
    {
        if (agoraInfo.IsInstructor)
        {
          //  LogToFile.Log("Enabled local audio.");
        }

        rtcEngine.EnableLocalAudio(true);
        rtcEngine.MuteLocalAudioStream(false);
    }

    public void DisableLocalAudio()
    {
        if (agoraInfo.IsInstructor)
        {
         //   LogToFile.Log("Disabled local audio.");
        }

        rtcEngine.EnableLocalAudio(false);
        rtcEngine.MuteLocalAudioStream(true);
    }

    private IEnumerator CheckAudioRecordingDevice()
    {
        WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(5f);

        while (true)
        {
            IAudioRecordingDeviceManager aM = rtcEngine.GetAudioRecordingDeviceManager();
            aM.CreateAAudioRecordingDeviceManager();

            int volume = aM.GetAudioRecordingDeviceVolume();
            //CDL.LogDebug($"Mic recording volume: {volume}.");
         //   LogToFile.Log($"Mic recording volume: {volume}.");

            aM.ReleaseAAudioRecordingDeviceManager();

            yield return waitTime;
        }
    }
}
