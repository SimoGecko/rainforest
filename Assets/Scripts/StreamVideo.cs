using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour {
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
        StartCoroutine(PlayVideo());	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator PlayVideo() {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared) {
            yield return new WaitForSeconds(1);
            break;
        }
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
        audioSource.Play();
    }
}
