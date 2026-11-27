
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VideoSceneManager : MonoBehaviour
{
    [Header("Video")]
    public VideoPlayer videoPlayer;

    [Header("UI")]
    public Slider videoProgressBar;
    public Button playPauseButton;
    public Button backButton;

    [Header("Icons")]
    public Image playPauseIcon;
    public Sprite playSprite;
    public Sprite pauseSprite;

    private bool isDragging = false;

    void Start()
    {
        videoPlayer.Play();

        videoProgressBar.minValue = 0;
        videoProgressBar.maxValue = 1;

        videoProgressBar.onValueChanged.AddListener(SeekVideo);

        playPauseButton.onClick.AddListener(TogglePlayPause);
        backButton.onClick.AddListener(GoBack);

        UpdatePlayPauseIcon();
    }

    void Update()
    {
        if (videoPlayer.isPrepared && !isDragging)
        {
            if (videoPlayer.length > 0)
            {
                videoProgressBar.SetValueWithoutNotify(
                    (float)(videoPlayer.time / videoPlayer.length)
                );
            }
        }

        UpdatePlayPauseIcon();
    }

    public void SeekVideo(float value)
    {
        if (videoPlayer.isPrepared && videoPlayer.length > 0)
        {
            videoPlayer.time = value * videoPlayer.length;
        }
    }

    public void TogglePlayPause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            videoPlayer.Play();
        }

        UpdatePlayPauseIcon();
    }

    void UpdatePlayPauseIcon()
    {
        if (playPauseIcon == null || videoPlayer == null)
            return;

        playPauseIcon.sprite = videoPlayer.isPlaying
            ? pauseSprite
            : playSprite;
    }

    public void GoBack()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
