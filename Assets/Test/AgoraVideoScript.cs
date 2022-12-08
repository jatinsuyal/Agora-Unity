using agora_gaming_rtc;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
[RequireComponent(typeof(DeviceChange))]
public class AgoraVideoScript : MonoBehaviour
{
    [HideInInspector]
    // public IRtcEngine mRtcEngine;
    public string appId;
    //private string token = "";
    private string channel = "Test";
    private ArrayList permissionList = new ArrayList();
    private const float Offset = 100;
    public Resolution CurrentResolution;
    public Vector2 newResolution;
    public bool instructor;
    public Toggle toggle_Instructor, toggle_DisableVideo, toggle_DisableAudio;

    public VideoSurface videoSurface;
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        permissionList.Add(Permission.Microphone);
        permissionList.Add(Permission.Camera);
    }
    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        foreach (string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermission(permission);
            }
        }
#endif
    }

    void Start()
    {
        toggle_Instructor.isOn = false;
        toggle_Instructor.onValueChanged.AddListener(delegate
        {
            IsInstructor(toggle_Instructor);
        });
        //OnJoin();
    }
    private void IsInstructor(Toggle toggle)
    {
        instructor = toggle.isOn;
    }
    public void ToggleAudio()
    {
        if (toggle_DisableAudio.isOn)
        {
            rtcEngine.EnableLocalAudio(false);
        }
        else
        {
            rtcEngine.EnableLocalAudio(true);
        }
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
    /*    private void ToggleAudio()
        {
            if (toggle.isOn)
            {
                mRtcEngine.DisableAudio();
            }
            else if(toggle.isOn==false)
            {
                mRtcEngine.EnableAudio();
            }
        }*/
    public void OnJoin()
    {

        rtcEngine = IRtcEngine.GetEngine(appId);
        JoinChannelUsingName();
        if (rtcEngine != null) { return; }
        if (instructor)
        {
            rtcEngine.EnableLocalVideo(true);
            rtcEngine.OnVideoSizeChanged = OnVideoSizeChangedHandler;
            DisableStudentStuff();
        }
        else
        {
            //  mRtcEngine.EnableAudio();
            rtcEngine.EnableLocalVideo(false);
            rtcEngine.EnableLocalAudio(false);
            // mRtcEngine.DisableVideo();
            DisableInstructorStuff();
        }


        rtcEngine.OnUserJoined = onUserJoined;
        rtcEngine.OnUserOffline = onUserOffline;

        rtcEngine.EnableVideo();
        rtcEngine.EnableVideoObserver();

        // mRtcEngine.JoinChannelByKey(channelKey: token, channelName: channel);
       /* GetComponent<DeviceChange>().Init();*/

    }

    private void DisableInstructorStuff()
    {
        // toggle_DisableAudio.gameObject.SetActive(false);
        // toggle_DisableVideo.gameObject.SetActive(false);
        // toggle_Instructor.gameObject.SetActive(false);
        //Disable join btn,2 dropdowns
    }
    private void DisableStudentStuff()
    {
        // toggle_DisableAudio.gameObject.SetActive(false);
        // toggle_DisableVideo.gameObject.SetActive(false);
        // toggle_Instructor.gameObject.SetActive(false);
        //Disable join btn ,Studentbtn & 2 dropdowns
    }
    public uint InstructorId = 100;
    private void onUserJoined(uint uid, int elapsed)
    {
        /*        GameObject go = GameObject.Find(uid.ToString());
                if (!ReferenceEquals(go, null))
                {
                    return;
                }
        */

        if (uid == InstructorId)
        {
            if (!ReferenceEquals(videoSurface, null))
            {
                videoSurface.SetForUser(uid);
                videoSurface.SetEnable(true);
                videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
            }
        }
    }
    [HideInInspector]
    public IRtcEngine rtcEngine;
    private void JoinChannelUsingName()
    {
        if (instructor)
        {
            rtcEngine.JoinChannel(channel, null, InstructorId);
        }
        else
        {
            rtcEngine.JoinChannel(channel);
        }

    }
    void OnVideoSizeChangedHandler(uint uid, int width, int height, int rotation)
    {
        if (uid == 0)
        {
            return;
        }
        print("UserID : " + uid + " " + height + " = " + width);
        if (GameObject.Find("Canvas/AllVideoSurfaces" + uid.ToString()) != null)
        {
            AdjustResolution(GameObject.Find("Canvas/AllVideoSurfaces" + uid.ToString()));
        }
        print(GameObject.Find("Canvas/AllVideoSurfaces" + uid.ToString()).name);
    }
    void Update()
    {
        CheckPermissions();
    }
    public void AdjustResolution(GameObject go, int mul = 1)
    {
        if (/*(Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight) &&*/ Screen.height > Screen.width)
        {
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(newResolution.x * mul, newResolution.y * mul);
        }
        else
        {
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(newResolution.y * mul, newResolution.x * mul);
        }
    }
    private void onUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        // remove video stream
        Debug.Log("onUserOffline: uid = " + uid + " reason = " + reason);
        // this is called in main thread
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            Destroy(go);
        }
    }
    public void Leave()
    {
        if (rtcEngine == null)
        {
            return;
        }

        rtcEngine.LeaveChannel();
        rtcEngine.DisableVideoObserver();
    }
    void OnApplicationQuit()
    {
        if (rtcEngine != null)
        {
            IRtcEngine.Destroy();
            rtcEngine = null;
        }
    }


    /*--------------------------------BTN Section------------------------------------*/
    public void BtnDown()
    {
        rtcEngine.EnableLocalAudio(true);
        print("called Down");
    }
    public void BtnUp()
    {
        rtcEngine.EnableLocalAudio(false);
        print("called Up");

    }
}