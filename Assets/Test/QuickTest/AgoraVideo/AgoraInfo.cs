using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;

[CreateAssetMenu(fileName = "AgoraInfo", menuName = "ScriptableObjects/AgoraInfo", order = 1)]
public class AgoraInfo : ScriptableObject
{
    [SerializeField] private string appId = "adsadasasdasda";
    [SerializeField] private uint instructorId = 969926744;
    [SerializeField] private bool isInstructor = false;
    [SerializeField] private string channelName = "LiveStream";
    [SerializeField] private int videoWidth = 300;
    [SerializeField] private int videoHeight = 200;
    [SerializeField] private int bitrate = 800;
    [SerializeField] private int audioIndicationInterval = 210;
    [SerializeField] private int audioIndicationSmooth = 6;
    [SerializeField] private FRAME_RATE frameRate = FRAME_RATE.FRAME_RATE_FPS_15;
    [SerializeField] private ORIENTATION_MODE orientation = ORIENTATION_MODE.ORIENTATION_MODE_ADAPTIVE;
    [SerializeField] private DEGRADATION_PREFERENCE degradationPrefrance = DEGRADATION_PREFERENCE.MAINTAIN_QUALITY;
    [SerializeField] private VIDEO_QUALITY videoQuality = VIDEO_QUALITY.VIDEO_240;
    [SerializeField] private AUDIO_PROFILE_TYPE audioProfile = AUDIO_PROFILE_TYPE.AUDIO_PROFILE_MUSIC_STANDARD_STEREO;
    [SerializeField] private AUDIO_SCENARIO_TYPE audioScenario = AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_CHATROOM_GAMING;
    [SerializeField] private CHANNEL_PROFILE channelProfile = CHANNEL_PROFILE.CHANNEL_PROFILE_GAME;
    [SerializeField] private uint myUID;
 //   [SerializeField] private bool isTV;

    public string AppId { get => appId; set => appId = value; }
    public uint InstructorId { get => instructorId; private set => instructorId = value; }
    public int VideoWidth { get => videoWidth; private set => videoWidth = value; }
    public string ChannelName { get => channelName; private set => channelName = value; }
    public int VideoHeight { get => videoHeight; private set => videoHeight = value; }
    public int Bitrate { get => bitrate; private set => bitrate = value; }
    public int AudioIndicationInterval { get => audioIndicationInterval; private set => audioIndicationInterval = value; }
    public int AudioIndicationSmooth { get => audioIndicationSmooth; private set => audioIndicationSmooth = value; }
    public FRAME_RATE FrameRate { get => frameRate; private set => frameRate = value; }
    public ORIENTATION_MODE Orientation { get => orientation; private set => orientation = value; }
    public DEGRADATION_PREFERENCE DegradationPreference { get => degradationPrefrance; private set => degradationPrefrance = value; }
    public VIDEO_QUALITY VideoQuality { get => videoQuality; private set => videoQuality = value; }
    public AUDIO_PROFILE_TYPE AudioProfile { get => audioProfile; private set => audioProfile = value; }
    public CHANNEL_PROFILE ChannelProfile { get => channelProfile; private set => channelProfile = value; }
    public AUDIO_SCENARIO_TYPE AudioScenario { get => audioScenario; private set => audioScenario = value; }
    public bool IsInstructor { get => isInstructor; set => isInstructor = value; }
    public uint MyUID { get => myUID; set => myUID = value; }

    public VideoEncoderConfiguration VideoConfig(VIDEO_QUALITY videoQuality)
    {
        VideoEncoderConfiguration config = new VideoEncoderConfiguration();

        switch (videoQuality)
        {
            case VIDEO_QUALITY.VIDEO_240:
                config.dimensions.width = 320;
                config.dimensions.height = 240;
                break;
            case VIDEO_QUALITY.VIDEO_320:
                config.dimensions.width = 480;
                config.dimensions.height = 320;
                break;
            case VIDEO_QUALITY.VIDEO_480:
                config.dimensions.width = 858;
                config.dimensions.height = 480;
                break;
            case VIDEO_QUALITY.VIDEO_720:
                config.dimensions.width = 1280;
                config.dimensions.height = 720;
                break;
            case VIDEO_QUALITY.VIDEO_1080:
                config.dimensions.width = 1920;
                config.dimensions.height = 1080;
                break;
            default:
                config.dimensions.width = 300;
                config.dimensions.height = 200;
                break;
        }

        config.bitrate = Bitrate;
        config.frameRate = FrameRate;
        config.orientationMode = Orientation;
        config.degradationPreference = DegradationPreference;

        return config;
    }

    public void SetChannelName(string name)
    {
        ChannelName = name;
    }    
}
public enum VIDEO_QUALITY
{
    VIDEO_240,
    VIDEO_320,
    VIDEO_480,
    VIDEO_720,
    VIDEO_1080
}