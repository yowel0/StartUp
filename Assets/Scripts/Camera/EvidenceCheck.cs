using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EvidenceCheck : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> evidence;
    [SerializeField]
    public List<GameObject> foundEvidence;
    [SerializeField]
    public Camera Cam;

    [SerializeField]
    private float distanceToEvidence;

    

    [SerializeField]
    private GameObject Phone;
    [SerializeField]
    private GameObject Journal;

    private bool phoneIsOut;
    private bool JournalActive;
    bool once = true;



    [SerializeField, HideInInspector]
    public Sprite photoSprite;

    [SerializeField]
    private Image photoDisplay;
    
    public List<Texture2D> texture2Ds = new List<Texture2D>();
    public List<Sprite> takenPhotos = new List<Sprite>();
    private void Start()
    {

    }
    private void Update()
    {
        if (once)
        {
            int layer = LayerMask.NameToLayer("Evidence");
            GameObject[] task = FindObjectsOfType<GameObject>();
            for(int i = 0; i < task.Length; i++)
            {
                if (task[i].layer == layer) { evidence.Add(task[i]); }
            }
            once = false;
        }
        CheckClientTaskServerRpc();
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (phoneIsOut)
            {
                phoneIsOut = false;
            }
            else if (!phoneIsOut && !JournalActive)
            {
                phoneIsOut = true;
            }

            if(!phoneIsOut && JournalActive)
            {
                JournalActive = false;
                phoneIsOut = true;
                
            }
        }

        if (phoneIsOut)
        {
            Phone.SetActive(true);
        }
        else
        {
            Phone.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (JournalActive)
            {
                JournalActive = false;
            }
            else if(!JournalActive && !phoneIsOut)
            {
                JournalActive = true;
            }

            if(phoneIsOut && !JournalActive)
            {
                phoneIsOut = false;
                JournalActive = true;
            }
        }

        if (JournalActive)
        {
            Journal.SetActive(true);
        }
        else
        {
            Journal.SetActive(false);
        }





        foreach (var target in evidence)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (IsVisible(Cam, target))
            {
                if (phoneIsOut && Input.GetMouseButtonDown(0) && distance <= distanceToEvidence)
                {
                    Cam.cullingMask = ~(1 << 1);
                    StartCoroutine(CapturePhoto());
                    foundEvidence.Add(target);
                    evidence.Remove(target);
                    CheckClientTaskServerRpc();
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
            GameManager.Instance.CheckClientsTaskRpc(this.gameObject.GetInstanceID());
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

    public IEnumerator CapturePhoto()
    {
        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        Texture2D screenCapture;
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
        texture2Ds.Add(screenCapture);
        Cam.cullingMask |= (1 << 1);
    }

    public void ShowPhoto()
    {
        Texture2D screenCapture = texture2Ds[texture2Ds.Count - 1];
        Sprite newPhotoSprite = Sprite.Create(screenCapture, new Rect(0, 0, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        //photoDisplay.sprite = photoSprite;
        takenPhotos.Add(newPhotoSprite);
    }
}
