using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deletedEvidenceCards : MonoBehaviour
{
    [SerializeField]
    List<Sprite> EvidenceCards = new List<Sprite>();
    [SerializeField]
    Image EvidenceCardsImage;

    [SerializeField]
    InteractionsMurderer murderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetEvidenceCardsImage();
    }

    public void SetEvidenceCardsImage()
    {
        if (murderer.CleanupAmount >= EvidenceCards.Count)
        {
            EvidenceCardsImage.sprite = EvidenceCards[EvidenceCards.Count - 1];
        }
        else
        {
            EvidenceCardsImage.sprite = EvidenceCards[murderer.CleanupAmount];
        }
    }
}
