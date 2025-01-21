using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using Unity.Services.Vivox;

public class VivoxManager : MonoBehaviour
{
    private const int k_DefaultMaxStringLength = 15;
    
    private const string DefaultChannelName = "GameChannel";

    private int m_PermissionAskedCount = 0;

    // 3D Positional Audio Properties
    private Channel3DProperties channel3DProperties = new Channel3DProperties(
        audibleDistance: 32,  // Maximum distance at which a player can hear others
        conversationalDistance: 5,  // Ideal distance for normal volume
        audioFadeIntensityByDistanceaudio: 1,  // Volume drop-off over distance
        audioFadeModel: AudioFadeModel.LinearByDistance);

    private IEnumerator Start()
    {
        // Ensure microphone permissions
        if (!IsMicPermissionGranted())
        {
            AskForPermissions();
            yield return new WaitUntil(() => IsMicPermissionGranted() || IsPermissionsDenied());
        }

        // Initialize Vivox and join the voice channel
        yield return InitializeVivox();
    }

    private IEnumerator InitializeVivox()
    {
        string displayName = GenerateDisplayName();
        var initializeTask = VivoxVoiceManager.Instance.InitializeAsync(displayName);
        yield return new WaitUntil(() => initializeTask.IsCompleted);

        if (initializeTask.IsFaulted)
        {
            Debug.LogError("Failed to initialize Vivox: " + initializeTask.Exception);
            yield break;
        }

        var joinChannelTask = VivoxService.Instance.JoinPositionalChannelAsync(DefaultChannelName, ChatCapability.AudioOnly, channel3DProperties);
        yield return new WaitUntil(() => joinChannelTask.IsCompleted);

        if (joinChannelTask.IsFaulted)
        {
            Debug.LogError("Failed to join Vivox channel: " + joinChannelTask.Exception);
        }
        else
        {
            Debug.Log($"Connected to Vivox channel '{DefaultChannelName}' as '{displayName}'.");
        }
    }

    private string GenerateDisplayName()
    {
        // Generate a unique player name (or use a predefined one)
        string systemName = string.IsNullOrWhiteSpace(SystemInfo.deviceName) ? Environment.MachineName : SystemInfo.deviceName;
        return systemName.Substring(0, Math.Min(k_DefaultMaxStringLength, systemName.Length));
    }

    private bool IsMicPermissionGranted()
    {
        bool isGranted = Permission.HasUserAuthorizedPermission(Permission.Microphone);

#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (IsAndroid12AndUp())
        {
            // For Android 12 and above, check for BLUETOOTH_CONNECT permission
            isGranted &= Permission.HasUserAuthorizedPermission(GetBluetoothConnectPermissionCode());
        }
#endif
        return isGranted;
    }

    private void AskForPermissions()
    {
        string permissionCode = Permission.Microphone;

#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (m_PermissionAskedCount == 1 && IsAndroid12AndUp())
        {
            permissionCode = GetBluetoothConnectPermissionCode();
        }
#endif
        m_PermissionAskedCount++;
        Permission.RequestUserPermission(permissionCode);
    }

    private bool IsPermissionsDenied()
    {
#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
        if (IsAndroid12AndUp())
        {
            return m_PermissionAskedCount == 2;
        }
#endif
        return m_PermissionAskedCount == 1;
    }

#if (UNITY_ANDROID && !UNITY_EDITOR) || __ANDROID__
    private bool IsAndroid12AndUp()
    {
        const int android12VersionCode = 31; // API level for Android 12
        AndroidJavaClass buildVersionClass = new AndroidJavaClass("android.os.Build$VERSION");
        int buildSdkVersion = buildVersionClass.GetStatic<int>("SDK_INT");

        return buildSdkVersion >= android12VersionCode;
    }

    private string GetBluetoothConnectPermissionCode()
    {
        if (IsAndroid12AndUp())
        {
            AndroidJavaClass manifestPermissionClass = new AndroidJavaClass("android.Manifest$permission");
            return manifestPermissionClass.GetStatic<string>("BLUETOOTH_CONNECT");
        }
        return "";
    }
#endif
}
