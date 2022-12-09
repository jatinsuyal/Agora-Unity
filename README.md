# Agora-Unity

Install Agora SDK v3.7.0.3.
 Here, I will explain you about how to use Video Call in Unity using agora and taking the data from the data bucket.

Add Your AppID
    Before you can build and run the project, you will need to add your AppID to the configuration. 
    Go to your developer account’s project console, create a new AppId or copy the Appd from an 
    existing project. Perform the following steps:
        1. Open the Assets/Demo/SceneHome scene, 
        2. Select the GameController from the Unity Editor’s Hierarchy panel. 
        3. The GameController game object has a property App ID, this is where you will add your Agora App ID.

You have to add your API key in the AgoraInfo addressables which we get previously.

At this step, you should be able to run the QuickTest scene within the Unity Editor. Input your desired 
channel name to the input field and click Join. You will be able to connect to the channel which has the same appID and channelID.

You can also customise the data like InstructorID, ChannelName ,Video height , width , Bitrate(through which the data will be processed),Audio Indication intervals, FrameRate of the video, Orientation , degradation prefrance, Video Quality, Audio Profile, Audio Scenario, Channel_Profile and you can even add some new ideas.

