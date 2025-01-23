using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class EvidenceCheck : MonoBehaviour
{
    public static EvidenceCheck instance { get; private set; }
    [SerializeField]
    public Camera Cam;

    [SerializeField]
    private float distanceToEvidence;



    [SerializeField]
    private GameObject Journal;
    [SerializeField]
    private GameObject phone;

    private bool phoneIsOut;
    private bool JournalActive;
    bool once = true;



    [SerializeField, HideInInspector]
    public Sprite photoSprite;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (phoneIsOut)
            {
                phone.SetActive(false);
                phoneIsOut = false;
            }
            else if(!phoneIsOut && JournalActive)
            {
                Journal.SetActive(false);
                phone.SetActive(true);
                phoneIsOut = true;
                JournalActive = false;

            }
            else
            {
                phone.SetActive(true);
                phoneIsOut = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (JournalActive)
            {
                Journal.SetActive(false);
                JournalActive = false;
            }
            else if (phoneIsOut && !JournalActive)
            {
                phone.SetActive(false);
                Journal.SetActive(true);
                phoneIsOut= false;
                JournalActive = true;
            }
            else
            {
                Journal.SetActive(true);
                JournalActive= true;
            }
        }
        if (phoneIsOut && Input.GetMouseButtonDown(0))
        {
            TakePicture();
        }

    }

    void TakePicture()
    {
        foreach (TaskManager.Task task in TaskManager.instance.taskList) //fixxx dit na bumbo clatttt xDDDD
        {
            if (!task.found)
            {
                float distance = Vector3.Distance(transform.position, task.evidenceObject.transform.position);
                if (IsVisible(Cam, task.evidenceObject))
                {
                    if (distance <= distanceToEvidence)
                    {
                        Cam.cullingMask = ~(1 << 1);
                        StartCoroutine(CapturePhoto(task));
                        //task.Find();
                        //CheckClientTaskServerRpc();
                    }
                }
            }
        }
    }

    [Rpc(SendTo.Server)]
    void CheckClientTaskServerRpc()
    {
        if (GameManager.Instance != null)
        {
            print("checkie");
            gameObject.name = gameObject.GetInstanceID().ToString();
            //GameManager.Instance.CheckClientsTaskRpc(this.gameObject.GetInstanceID());
        }
    }
    public bool IsVisible(Camera c, GameObject target)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        var point = target.transform.position;

        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                return false;
            }
        }
        return true;
    }

    public IEnumerator CapturePhoto(TaskManager.Task _task)
    {
        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        Texture2D screenCapture;
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
        Cam.cullingMask |= (1 << 1);
        Sprite newPhotoSprite = Sprite.Create(screenCapture, new Rect(0, 0, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        _task.Find(newPhotoSprite);
    }

    //public Sprite ShowRecentPhoto()
    //{
    //    //Texture2D screenCapture = texture2Ds[texture2Ds.Count - 1];
    //    //Sprite newPhotoSprite = Sprite.Create(screenCapture, new Rect(0, 0, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
    //    //takenPhotos.Add(newPhotoSprite);
    //    return newPhotoSprite;
    //}
}
