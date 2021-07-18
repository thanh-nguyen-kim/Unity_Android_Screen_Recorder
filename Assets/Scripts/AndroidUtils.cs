using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// Protection level: dangerous permissions 2015/11/25
// http://developer.android.com/intl/ja/reference/android/Manifest.permission.html
public enum AndroidPermission
{
    ACCESS_COARSE_LOCATION,
    ACCESS_FINE_LOCATION,
    ADD_VOICEMAIL,
    BODY_SENSORS,
    CALL_PHONE,
    CAMERA,
    GET_ACCOUNTS,
    PROCESS_OUTGOING_CALLS,
    READ_CALENDAR,
    READ_CALL_LOG,
    READ_CONTACTS,
    READ_EXTERNAL_STORAGE,
    READ_PHONE_STATE,
    READ_SMS,
    RECEIVE_MMS,
    RECEIVE_SMS,
    RECEIVE_WAP_PUSH,
    RECORD_AUDIO,
    SEND_SMS,
    USE_SIP,
    WRITE_CALENDAR,
    WRITE_CALL_LOG,
    WRITE_CONTACTS,
    WRITE_EXTERNAL_STORAGE
}
public enum VideoEncoder
{
    DEFAULT,
    H263,
    H264,
    HEVC,
    MPEG_4_SP,
    VP8
}
public class AndroidUtils : MonoBehaviour
{
    private const float SCREEN_WIDTH = 720f;
    private const string VIDEO_NAME = "Record", GALLERY_PATH = "/../../../../DCIM/VideoRecorders";
    public UnityAction onStartRecord, onStopRecord;
    public static UnityAction onAllowCallback, onDenyCallback, onDenyAndNeverAskAgainCallback;
#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject androidRecorder;
#endif
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            androidRecorder = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			androidRecorder.Call("setUpSaveFolder","Tee");//custom your save folder to Movies/Tee, by defaut it will use Movies/AndroidUtils
            int width = (int)(Screen.width > SCREEN_WIDTH ? SCREEN_WIDTH : Screen.width);
            int height = Screen.width > SCREEN_WIDTH ? (int)(Screen.height * SCREEN_WIDTH / Screen.width) : Screen.height;
            int bitrate = (int)(1f * width * height / 100 * 240 * 7);
            int fps = 30;
            bool audioEnable=true;
            androidRecorder.Call("setupVideo", width, height,bitrate, fps,audioEnable,VideoEncoder.H264.ToString());//this line manual sets the video record setting. You can use the defaut setting by comment this code block
        }
#endif
    }
    #region Android Recorder
    public void StartRecording()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!AndroidUtils.IsPermitted(AndroidPermission.RECORD_AUDIO))//RECORD_AUDIO is declared inside plugin manifest but we need to request it manualy
        {
            AndroidUtils.RequestPermission(AndroidPermission.RECORD_AUDIO);
            onAllowCallback = () =>
            {
                androidRecorder.Call("startRecording");
            };
            onDenyCallback = () => { ShowToast("Need RECORD_AUDIO permission to record voice");};
            onDenyAndNeverAskAgainCallback = () => { ShowToast("Need RECORD_AUDIO permission to record voice");};
        }
        else
            androidRecorder.Call("startRecording");
#endif
    }
    public void StopRecording()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	androidRecorder.Call("stopRecording");
#endif
    }
    public void VideoRecorderCallback(string message)//this function will be call when record status change
    {
        switch (message)
        {
            case "init_record_error":
                break;
            case "start_record":
                if (onStartRecord != null) onStartRecord.Invoke();
                break;
            case "stop_record":
                if (onStopRecord != null) onStopRecord.Invoke();
                break;
        }
    }
    #endregion
    #region Android Permissions
    private void OnAllow()//this function will be called when the permission has been approved
    {
        if (onAllowCallback != null)
            onAllowCallback();
        ResetAllCallBacks();
    }
    private void OnDeny()//this function will be called when the permission has been denied
    {
        if (onDenyCallback != null)
            onDenyCallback();
        ResetAllCallBacks();
    }
    private void OnDenyAndNeverAskAgain()//this function will be called when the permission has been denied and user tick to checkbox never ask again
    {
        if (onDenyAndNeverAskAgainCallback != null)
            onDenyAndNeverAskAgainCallback();
        ResetAllCallBacks();
    }
    private void ResetAllCallBacks()
    {
        onAllowCallback = null;
        onDenyCallback = null;
        onDenyAndNeverAskAgainCallback = null;
    }
    public static bool IsPermitted(AndroidPermission permission)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (var androidUtils = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            return androidUtils.GetStatic<AndroidJavaObject>("currentActivity").Call<bool>("hasPermission", GetPermissionStrr(permission));
        }
#endif
        return true;
    }
    public static void RequestPermission(AndroidPermission permission, UnityAction onAllow = null, UnityAction onDeny = null, UnityAction onDenyAndNeverAskAgain = null)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        onAllowCallback = onAllow;
        onDenyCallback = onDeny;
        onDenyAndNeverAskAgainCallback = onDenyAndNeverAskAgain;
        using (var androidUtils = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            androidUtils.GetStatic<AndroidJavaObject>("currentActivity").Call("requestPermission", GetPermissionStrr(permission));
        }
#endif
    }
    private static string GetPermissionStrr(AndroidPermission permission)
    {
        return "android.permission." + permission.ToString();
    }
    #endregion
    public static void ShowToast(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            new AndroidJavaClass("android.widget.Toast").CallStatic<AndroidJavaObject>("makeText", currentActivity.Call<AndroidJavaObject>("getApplicationContext"), new AndroidJavaObject("java.lang.String", message), 0).Call("show");
        }));
#endif
    }
    public static void ShareAndroid(string body, string subject, string url, string filePath, string mimeType, bool chooser, string chooserText)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent"))
        using (AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent"))
        {
            using (intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND")))
            { }
            using (intentObject.Call<AndroidJavaObject>("setType", mimeType))
            { }
            using (intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject))
            { }
            using (intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body))
            { }
            if (!string.IsNullOrEmpty(url))
            {
                using (AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri"))
                using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", url))
                using (intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject))
                { }
            }
            else if (filePath != null)
            {
                using (AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri"))
                using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + filePath))
                using (intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject))
                { }
            }
            using (AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                if (chooser)
                {
                    AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, chooserText);
                    currentActivity.Call("startActivity", jChooser);
                }
                else
                    currentActivity.Call("startActivity", intentObject);
            }
        }
#endif
    }
}
