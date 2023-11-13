using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Cutscene : MonoBehaviour
{
    private VideoPlayer video;

    [SerializeField] private GameObject startMenu;

    private void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.loopPointReached += OnEndCutscene;
    }

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            OnEndCutscene(video);
        }
    }

    private void OnEndCutscene(UnityEngine.Video.VideoPlayer vp)
    {
        startMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
