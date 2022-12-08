using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using UnityEngine.UI;
using TMPro;
using System;

public class DeviceChange : MonoBehaviour
{
    private IRtcEngine rtcEngine = null;

    private List<string> micNames = new List<string>();
    private List<string> micIds = new List<string>();
    private List<string> cameraNames = new List<string>();
    private List<string> cameraIds = new List<string>();

    private int currentCameraIndex = 0;
    private int currentMicIndex = 0;

    //[SerializeField] private Button btnCameraChange;

    [SerializeField] private TMP_Dropdown drpDwnCameraDevice;
    [SerializeField] private TMP_Dropdown drpDwnMicDevice;

    public void Init(bool isInstructor)
    {
        rtcEngine = GetComponent<AVManager>().GetCurrentEngine();
        //btnCameraChange.onClick.AddListener(OnButtonClick_ChangeCamera);

        if (isInstructor)
        {
            ClearDropDowns();

            SetupCameraData();
            SetupMicData();

            SetUpDropDowns();
        }
    }

    private void SetUpDropDowns()
    {
        drpDwnCameraDevice.AddOptions(cameraNames);
        drpDwnMicDevice.AddOptions(micNames);

        drpDwnCameraDevice.onValueChanged.AddListener(ChangeCamera);
        drpDwnMicDevice.onValueChanged.AddListener(ChangeMic);
    }

/*    private void OnButtonClick_ChangeCamera()
    {
        int cameraCount = cameraNames.Count;

        currentCameraIndex++;

        if(currentCameraIndex == cameraCount)
        {
            currentCameraIndex = 0;
        }

        ChangeCamera(currentCameraIndex);
    }

    private void OnButtonClick_ChangeMic()
    {
        int micCount = micNames.Count;

        currentMicIndex++;

        if(currentMicIndex == micCount)
        {
            currentMicIndex = 0;
        }

        ChangeMic(currentMicIndex);
    }*/

    private void SetupMicData()
    {
        IAudioRecordingDeviceManager aM = rtcEngine.GetAudioRecordingDeviceManager();
        aM.CreateAAudioRecordingDeviceManager();

        micIds.Clear();
        micNames.Clear();

        string micName = string.Empty;
        string micId = string.Empty;

        int micCount = aM.GetAudioRecordingDeviceCount();

        for(int i = 0; i < micCount; i++)
        {
            int result = aM.GetAudioRecordingDevice(i, ref micName, ref micId);

            if(result == 0)
            {
                micNames.Add(micName);
                micIds.Add(micId);
            }
        }

        aM.ReleaseAAudioRecordingDeviceManager();
        // Clear and add options in drop down
    }

    private void SetupCameraData()
    {
        IVideoDeviceManager vd = rtcEngine.GetVideoDeviceManager();

        vd.CreateAVideoDeviceManager();

        cameraIds.Clear();
        cameraNames.Clear();

        string camName = string.Empty;
        string camId = string.Empty;

        int count = vd.GetVideoDeviceCount();

        for(int i = 0; i < count; i++)
        {
            int result = vd.GetVideoDevice(i, ref camName, ref camId);

            if(result == 0)
            {
                cameraNames.Add(camName);
                cameraIds.Add(camId);
            }
        }

        vd.ReleaseAVideoDeviceManager();
        // Clear and add options in dropdown;
    }

    private void ChangeCamera(int arg0)
    {
        string cameraId = string.Empty;
        string cameraName = string.Empty;

        IVideoDeviceManager vd = rtcEngine.GetVideoDeviceManager();

        vd.CreateAVideoDeviceManager();

        vd.GetVideoDevice(0, ref cameraName, ref cameraId);
        vd.SetVideoDevice(cameraIds[arg0]);

        vd.ReleaseAVideoDeviceManager();
    }

    private void ChangeMic(int arg0)
    {
        string micName = string.Empty;
        string micId = string.Empty;

        IAudioPlaybackDeviceManager aM = rtcEngine.GetAudioPlaybackDeviceManager();

        aM.CreateAAudioPlaybackDeviceManager();

        aM.GetAudioPlaybackDevice(0, ref micName, ref micId);
        aM.SetAudioPlaybackDevice(micIds[arg0]);

        aM.ReleaseAAudioPlaybackDeviceManager();
    }

    private void ClearDropDowns()
    {
        drpDwnCameraDevice.ClearOptions();
        drpDwnMicDevice.ClearOptions();
    }
}
