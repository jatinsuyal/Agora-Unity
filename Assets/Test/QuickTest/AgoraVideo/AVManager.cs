using System.Collections;
using UnityEngine;
using agora_gaming_rtc;
using UnityEngine.UI;
using UnityEngine.Android;
public class AVManager : MonoBehaviour
{
    public AgoraInfo agoraInfo;
    private IRtcEngine rtcEngine;
    private uint myUID;

    private AudioConnect audioConnect;
    private VideoConnect videoConnect;

    public float inSpeakingVolume = 0.0f;
    public float speakingVolume = 0.0f;

    public Button joinCall = default;

    public bool isDebug = true;
    private bool isFirstTime = true;

    private void Start()
    {
        Init();
    }

    private ArrayList permissionList = new ArrayList();
    private void Awake()
    {
      //  Screen.orientation = ScreenOrientation.LandscapeLeft;
        permissionList.Add(Permission.Microphone);
        permissionList.Add(Permission.Camera);
    }
    private void CheckPermissions()
    {
//#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        foreach (string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermission(permission);
            }
        }
//#endif
    }
    private void Update()
    {
        CheckPermissions();
    }
    private void Init()
    {
        audioConnect = GetComponent<AudioConnect>();
        videoConnect = GetComponent<VideoConnect>();

        CheckAppId(agoraInfo.AppId);

        rtcEngine = IRtcEngine.GetEngine(agoraInfo.AppId);

        joinCall.onClick.AddListener(Join);
    }

    public void Join()
    {
        if (rtcEngine == null)
        {
            return;
        }

       // CDL.LogDebug($"Joining channel = {agoraInfo.ChannelName}");

        DisableVideoIfNotInstructor();

        SetupRtcEngine();

        SetupCallbacks();

        JoinChannelUsingName();
    }

    private void SetupRtcEngine()
    {
        rtcEngine.EnableVideo();
        rtcEngine.EnableVideoObserver();

        rtcEngine.StartPreview();

        rtcEngine.SetAudioProfile(agoraInfo.AudioProfile, agoraInfo.AudioScenario);
        rtcEngine.SetChannelProfile(agoraInfo.ChannelProfile);

        SetVideoConfiguration();

        rtcEngine.SetDefaultAudioRouteToSpeakerphone(true);

        if (agoraInfo.IsInstructor)
        {
            rtcEngine.AdjustPlaybackSignalVolume(100);
        }
        rtcEngine.EnableAudioVolumeIndication(agoraInfo.AudioIndicationInterval, agoraInfo.AudioIndicationSmooth, true);
    }

    private void SetVideoConfiguration()
    {
        rtcEngine.SetVideoEncoderConfiguration(agoraInfo.VideoConfig(agoraInfo.VideoQuality));
    }

    private void SetupCallbacks()
    {
        rtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccess;
        rtcEngine.OnUserJoined = OnUserJoined;
        rtcEngine.OnUserOffline = OnUserOffline;
        rtcEngine.OnConnectionLost = OnConnectionLost;
        rtcEngine.OnActiveSpeaker = OnActiveSpeaker;
        rtcEngine.OnVolumeIndication = OnVolumeIndication;
        rtcEngine.OnWarning = HandleWarning;
        rtcEngine.OnError = HandleError;
    }

    private void JoinChannelUsingName()
    {
        if (agoraInfo.IsInstructor)
        {
            rtcEngine.JoinChannel(agoraInfo.ChannelName, null, agoraInfo.InstructorId);
        }
        else
        {
            rtcEngine.JoinChannel(agoraInfo.ChannelName);
        }
        
    }

    public void OnClick_LeaveAndUnload()
    {
        Leave();
        UnloadEngine();
    }

    public void UnloadEngine()
    {
        //CDL.LogDebug("Unloading engine.");
       
        if(rtcEngine != null)
        {
            rtcEngine.StopPreview();
            rtcEngine.DisableVideoObserver();
            rtcEngine.DisableVideo();
            rtcEngine.DisableAudio();

            IRtcEngine.Destroy();
            rtcEngine = null;
        }
    }

    #region Callbacks
    private void OnJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        if (isFirstTime)
        {
            audioConnect.Init();
            videoConnect.Init();
            isFirstTime = false;
        }
        if(!agoraInfo.IsInstructor)
        {
            audioConnect.EnableUserAudio(agoraInfo.InstructorId);
        }
        GetComponent<DeviceChange>().Init(agoraInfo.IsInstructor);

        //CDL.LogDebug($"Join channel {name} success with uid : {uid}.");
        
        //if (data.playerInfo.IsInstructor)
        //{
        //    LogToFile.Log($"Join channel {name} success with uid : {uid}.");
        //}
        
        myUID = uid;
        agoraInfo.MyUID = uid;
    }

    private void OnUserJoined(uint _uid, int elapsed)
    {
        //CDL.LogDebug($"User joined with uid : {_uid}.", Color.yellow);

        if(_uid != agoraInfo.InstructorId)
        {
            // TODO : if lined 87 works then remove this whole if block otherwise add code
        }
    }

    private void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        //CDL.LogDebug($"User with uid : {uid} is offline reason : {reason}.");
    }

    private void OnConnectionLost()
    {
        //CDL.LogDebug("Connection lost");

        if (agoraInfo.IsInstructor)
        {
            //  LogToFile.Log("Connection lost");
            Debug.Log("Connection Lost");
        }
        // TODO : Join or do something according to some condition
    }

    private void OnActiveSpeaker(uint uid)
    {
        //CDL.LogDebug($"User with uid : {uid} is speaking.");
        Debug.Log($"User with uid : {uid} is speaking.");
    }

    private void OnVolumeIndication(AudioVolumeInfo[] speakers, int speakerNumber, int totalVolume)
    {
        if (speakers.Length == 0)
        {
            speakingVolume = 0;
            //CDL.LogDebug($"Volume : {speakingVolume}.");
            Debug.Log($"Volume : {speakingVolume}.");
        }
        else
        {
            for (int i = 0; i < speakers.Length; i++)
            {
               // if (ChildAudio.isOn)
                {
                    if (speakers[i].vad == 1)
                    {
                        speakingVolume = (float)speakers[i].volume;
                    }
                }

                if (speakers[i].uid == agoraInfo.InstructorId)
                {
                    inSpeakingVolume = (float)speakers[i].volume;
                }
            }
        }
    }

    private bool disableEnable = false;
    private void HandleWarning(int warn, string msg)
    {
        //CDL.LogDebug($"Warning code: {warn} msg: {IRtcEngine.GetErrorDescription(warn)}.");

        if (agoraInfo.IsInstructor)
        {
            if(warn == 1020 || warn == 1052)
            {
                if (!disableEnable)
                {
                    disableEnable = true;
                    StartCoroutine(DisableEnableAudio());
                }
            }

            Debug.Log($"Warning code: {warn} msg: {IRtcEngine.GetErrorDescription(warn)}.");
        }
    }

    private IEnumerator DisableEnableAudio()
    {
        Debug.Log("Disabling audio using function.");
        audioConnect.DisableLocalAudio();

        yield return new WaitForSecondsRealtime(0.5f);

        Debug.Log("Enabling audio using function.");
        audioConnect.EnableLocalAudio();

        disableEnable = false;
    }
    private int LastError { get; set; }
    private void HandleError(int error, string msg)
    {
        if(error == LastError)
        {
            return;
        }
        msg = string.Format("Error code:{0} msg:{1}", error, IRtcEngine.GetErrorDescription(error));

        switch (error)
        {
            case 101:
                msg += "\nPlease make sure your AppId is valid and it does not require a certificate for this demo.";
                break;
        }

        //CDL.LogError(msg);

        LastError = error;
    }
    #endregion

    private void DisableVideoIfNotInstructor()
    {
        if (agoraInfo.IsInstructor)
        {
            rtcEngine.EnableLocalVideo(true);
        }
        else
        {
            rtcEngine.EnableLocalVideo(false);
        }
    }

    public IRtcEngine GetCurrentEngine()
    {
        return rtcEngine;
    }

    private void CheckAppId(string appId)
    {
        Debug.Assert(appId.Length > 10, "Please fill in your AppId first on Game Controller object.");
        GameObject go = GameObject.Find("AppIDText");
        if (go != null)
        {
            Text appIDText = go.GetComponent<Text>();
            if (appIDText != null)
            {
                if (string.IsNullOrEmpty(appId))
                {
                    appIDText.text = "AppID: " + "UNDEFINED!";
                }
                else
                {
                    appIDText.text = "AppID: " + appId.Substring(0, 4) + "********" + appId.Substring(appId.Length - 4, 4);
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (isDebug)
        {
            Leave();
            UnloadEngine();
        }
    }

    private void OnDestroy()
    {
        Leave();
        UnloadEngine();
    }

    private void Leave()
    {
        if (rtcEngine == null)
        {
            return;
        }

        rtcEngine.LeaveChannel();
        rtcEngine.DisableVideoObserver();
    }
    public Toggle toggle_DisableAudio, toggle_DisableVideo;
    public void ToggleAudio()
    {
        if (toggle_DisableAudio.isOn)
        {
            //rtcEngine.EnableLocalAudio(false);
            audioConnect.DisableLocalAudio();
        }
        else
        {
            //rtcEngine.EnableLocalAudio(true);
            audioConnect.EnableLocalAudio();

        }

    }
    public Button btn;
    public void BtnUp()
    {
        audioConnect.DisableLocalAudio();
    }
    public void BtnDwn()
    {
        audioConnect.EnableLocalAudio();
    }
    public void ToggleVideo()
    {
        if (toggle_DisableVideo.isOn)
        {
            // mRtcEngine.EnableLocalVideo(false);
            rtcEngine.DisableVideo();
            rtcEngine.DisableVideoObserver();
        }
        else
        {
            rtcEngine.EnableVideo();
            rtcEngine.EnableVideoObserver();
            rtcEngine.EnableLocalVideo(true);
        }
    }

}
