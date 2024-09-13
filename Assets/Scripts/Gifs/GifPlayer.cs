using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GifPlayer : MonoBehaviour
{
    public Gif_SO_Manager gif_SO_Manager;
    [OdinSerialize] public Gif_SO currentGif;
    [OdinSerialize] public Gif_SO queuedGif;
    [SerializeField]
    [ReadOnly]
    private bool queuedGifHasPlayTime = false;
    [SerializeField]
    [ReadOnly]
    private float queuedPlayTime = 0;
    public SpriteRenderer spriteRenderer;
    [SerializeField] private bool gifSpriteExpanded = false;
    private Coroutine gifCoroutine;

    public UnityEvent playGifEvent;
    public UnityEvent stopGifEvent;


    void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void PlayGif(string gifName)
    {
        if (SetGif(gifName, false, -1))
        {
            PlayGif();
        }
    }

    public void PlayGif(string gifName, float time)
    {
        if (SetGif(gifName, true, time))
        {
            PlayGifForTime(time);
        }
    }

    private bool SetGif(string gifName, bool hasPlayTime, float playTime)
    {
        if (gifSpriteExpanded && currentGif != null && currentGif.gifName != gifName)
        {
            SetQueuedGif(gifName);
            queuedGifHasPlayTime = hasPlayTime;
            queuedPlayTime = playTime;
            StopGif();
            return false;
        }
        else if (!gifSpriteExpanded)
        {
            currentGif = gif_SO_Manager.GetGif_SO(gifName);
            return true;
        }
        return false;
    }
    private void SetQueuedGif(string gifName)
    {
        if (queuedGif == null)
        {
            queuedGif = gif_SO_Manager.GetGif_SO(gifName);
        }
        else if (queuedGif != null && queuedGif.gifName != gifName)
        {
            queuedGif = gif_SO_Manager.GetGif_SO(gifName);
        }
    }
    private void PlayGif()
    {
        playGifEvent.Invoke();
        if (gifCoroutine != null)
        {
            StopCoroutine(gifCoroutine);
            gifCoroutine = null;
        }

        gifCoroutine = StartCoroutine(PlayGifCoroutine());
    }
    private void PlayGifForTime(float time)
    {
        if (gifCoroutine != null)
        {
            StopCoroutine(gifCoroutine);
            gifCoroutine = null;
            stopGifEvent.Invoke();
        }
        gifCoroutine = StartCoroutine(PlayGifForTimeCoroutine(time));
    }

    public void PlayQueuedGif()
    {
        //Debug.Log("Playing Queued Gif");
        if (queuedGif != null)
        {
            currentGif = queuedGif;
            if(queuedGifHasPlayTime)
            {
                PlayGifForTime(queuedPlayTime);
                queuedGifHasPlayTime = false;
                queuedPlayTime = -1;
            }
            else
            {
                PlayGif();
            }
            queuedGif = null;
        }
    }

    public string GetGifName()
    {
        if (currentGif == null)
        {
            return "No Gif Set";
        }
        return currentGif.gifName;
    }

    public bool IsGifAlreadyPlayingOrQueued(string gifName)
    {
        bool passed = true;
        if (currentGif != null && currentGif.gifName == gifName)
        {
            passed = false;
        }
        if (queuedGif != null && queuedGif.gifName == gifName)
        {
            passed = false;
        }
        return passed;
    }

    public string GetQueuedGifName()
    {
        if (queuedGif == null)
        {
            return "No Gif Queued";
        }
        return queuedGif.gifName;
    }


    public void StopGif()
    {
        if (gifSpriteExpanded)
        {
            stopGifEvent.Invoke();
        }
        if (gifCoroutine != null)
        {
            StopCoroutine(gifCoroutine);
        }
        currentGif = null;
    }
    public void SetExpandedState(bool state)
    {
        gifSpriteExpanded = state;
    }

    private IEnumerator PlayGifCoroutine()
    {
        int index = 0;
        while (true)
        {
            if (currentGif == null) break;
            //Debug.Log("Playing Gif on index: " + index);
            spriteRenderer.sprite = currentGif.gifSprites[index];
            if (index < currentGif.gifSprites.Count - 1)
            {
                index++;
            }
            else
            {
                if (currentGif.loop)
                {
                    index = 0;
                }
                else
                {
                    StopGif();
                    //yield break;
                }
            }
            yield return new WaitForSeconds(currentGif.frameRate);
        }
    }

    //function to play gif for a set amount of time
    //private Coroutine playGifForTimeCoroutine;
    private IEnumerator PlayGifForTimeCoroutine(float time)
    {
        PlayGif();
        yield return new WaitForSeconds(time);
        StopGif();
    }
}
