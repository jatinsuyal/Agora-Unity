using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    [CreateAssetMenu(fileName = "Player Info", menuName = "ScriptableObjects/Player/PlayerInfo", order = 1)]
    public class PlayerInfo : ScriptableObject
    {
        public delegate void OnChangeBoolean(bool result);
        public static event OnChangeBoolean OnChangeHaveControl;

        [SerializeField] private string nickName = "TestUser";
        [SerializeField] private string roomName = string.Empty;
        [SerializeField] private string instructorRoomName = "LiveStream";
        [SerializeField] private bool isInstructor = false;
        [SerializeField] private bool haveControl = false;
        [SerializeField] private bool inSpace = false;
        [SerializeField] private bool isMultiplayer = false;
        [SerializeField] private bool isLive = false;
        [SerializeField] private bool hasGivenControl = false;
        [SerializeField] private bool isMasterClient = false;
        [SerializeField] private bool isSolvingBarrel = false;
        [SerializeField] private bool isRaycasting = true;
        
        [Header("Tick this is debugging. Untick if production build")]
        [SerializeField] private bool isDebug = false;

        public string NickName { get => nickName; private set => nickName = value; }
        public string RoomName { get => roomName; private set => roomName = value; }
        public bool IsInstructor { get => isInstructor; private set => isInstructor = value; }
        public bool HaveControl { get => haveControl; private set => haveControl = value; }
        public bool InSpace { get => inSpace; private set => inSpace = value; }
        public bool IsMultiplayer { get => isMultiplayer; private set => isMultiplayer = value; }
        public bool IsLive { get => isLive; private set => isLive = value; }
        public bool HasGivenControl { get => hasGivenControl; private set => hasGivenControl = value; }
        public bool IsDebug { get => isDebug; private set => isDebug = value; }
        public bool IsMasterClient { get => isMasterClient; private set => isMasterClient = value; }
        public string InstructorRoomName { get => instructorRoomName; private set => instructorRoomName = value; }
        public bool IsSolvingBarrel { get => isSolvingBarrel; private set => isSolvingBarrel = value; }
        public bool IsRaycasting { get => isRaycasting; private set => isRaycasting = value; }

        public void SetNickName(string name)
        {
            NickName = name;
        }

        public void SetRoomName(string roomName)
        {
            RoomName = roomName;
        }

        public void SetInstructor(bool value)
        {
            IsInstructor = value;
        }

        public void SetControl(bool value)
        {
            HaveControl = value;
            OnChangeHaveControl?.Invoke(value);
        }

        public void SetInSpace(bool value)
        {
            InSpace = value;
        }

        public void SetMultiplayer(bool value)
        {
            IsMultiplayer = value;
        }

        public void SetIsLive(bool value)
        {
            IsLive = value;
        }

        public void SetHasGivenControl(bool value)
        {
            HasGivenControl = value;
        }

        public void SetIsDebug(bool value)
        {
            isDebug = value;
        }

        public void SetIsMasterClient(bool value)
        {
            IsMasterClient = value;
        }

        public void SetIsSolvingBarrel(bool value)
        {
            IsSolvingBarrel = value;
        }
    
        public void SetIsRaycasting(bool value)
        {
            IsRaycasting = value;
        }
    }
}