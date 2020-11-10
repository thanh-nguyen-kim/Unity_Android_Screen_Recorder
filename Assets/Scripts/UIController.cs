using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIController : MonoBehaviour
{
    private AndroidUtils androidUtils;
    [SerializeField] private GameObject startRecordbtn, stopRecordBtn;
    private void Start()
    {
        androidUtils = FindObjectOfType<AndroidUtils>();
        androidUtils.onStartRecord += OnStartRecord;//listen to start record event
        androidUtils.onStopRecord += OnStopRecord;//listen to stop record event
    }
    public void OnClickStartRecord()
    {
        OnStartRecord();
        androidUtils.StartRecording();
    }
    public void OnClickStopRecord()
    {
        OnStopRecord();
        androidUtils.StopRecording();
    }
    public void OnStartRecord()
    {
        startRecordbtn.SetActive(false);
        stopRecordBtn.SetActive(true);
    }
    public void OnStopRecord()
    {
        startRecordbtn.SetActive(true);
        stopRecordBtn.SetActive(false);
    }
}
