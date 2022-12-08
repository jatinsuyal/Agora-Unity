using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using System;
//using Utilities;

public class VideoConnect : MonoBehaviour
{
    [SerializeField] private VideoSurface smallVideo;
    [SerializeField] private VideoSurface smallVideo2;
    [SerializeField] private VideoSurface fullVideo;
    private AgoraInfo agoraInfo;

    private IRtcEngine rtcEngine;

    public void Init()
    {
        agoraInfo = FindObjectOfType<DataBucket>().agoraInfo;

        rtcEngine = GetComponent<AVManager>().GetCurrentEngine();

        if (agoraInfo.IsInstructor)
        {
            EnableLocalVideo();
            smallVideo?.SetEnable(true);
            smallVideo2?.SetEnable(true);
            fullVideo?.SetEnable(true);
        }
        else
        {
            smallVideo?.SetForUser(agoraInfo.InstructorId);
            smallVideo2?.SetForUser(agoraInfo.InstructorId);
            fullVideo?.SetForUser(agoraInfo.InstructorId);
            DisableLocalVideo();
        }
    }

    public void EnableLocalVideo()
    {
        if (agoraInfo.IsInstructor)
        {
           // LogToFile.Log("Enabled local video.");
        }
        rtcEngine.EnableLocalVideo(true);
    }

    public void DisableLocalVideo()
    {
        if (agoraInfo.IsInstructor)
        {
          //  LogToFile.Log("Disabled local video.");
        }
        rtcEngine.EnableLocalVideo(false);
    }
    
    public void ChangeVideoConfig(VIDEO_QUALITY _quality)
    {
        rtcEngine.SetVideoEncoderConfiguration(agoraInfo.VideoConfig(_quality));
    }
}
