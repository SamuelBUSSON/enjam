using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VolumeBar : MonoBehaviour
{
    public int valToChangeImage;

    private Vector2 basePositon;
    private VolumeUI volumeUi;
    private Image image;
    private Transform player;

    private bool complete = false;

    private void Start()
    {
        image = GetComponent<Image>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        volumeUi = GetComponentInParent<VolumeUI>();

        basePositon = new Vector2(0,0);
        basePositon = GetComponent<RectTransform>().anchoredPosition;
    }

    private void Update()
    {
        if(valToChangeImage <= player.GetComponentInChildren<BoomBox>().GetPercentVolume()*100)
        {
            image.sprite = volumeUi.imageOn;

            if((valToChangeImage/10)%2 == 0)
            {
                if (volumeUi.goTimerPair && !volumeUi.goTimerImpair)
                {
                    if (!complete)
                    {
                        complete = true;
                        GetComponent<RectTransform>().DOJumpAnchorPos(GetComponent<RectTransform>().anchoredPosition, 6.0f, 1, 0.6f).OnComplete(() => complete = false);
                    }
                }
            }
            else
            {
                if (volumeUi.goTimerImpair && volumeUi.goTimerPair)
                {
                    if (!complete)
                    {
                        complete = true;
                        GetComponent<RectTransform>().DOJumpAnchorPos(GetComponent<RectTransform>().anchoredPosition, 6.0f, 1, 0.6f).OnComplete(() => complete = false);
                    }
                }
            }

           

        }
        else
        {
            image.sprite = volumeUi.imageOff;
        }
    }

}
