using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject AnyKeyScene;
    [SerializeField] private GameObject AnyKeySceneNext;
    [SerializeField] private string LoadScene = "GameScene";

    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown qualityDropdown;

    [SerializeField] private Resolution[] resolutions;
    [SerializeField] private Dropdown resolution;

    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider SFXSlider;

    public void ChangeVolumeMusic(float volume)
    {
        mixer.SetFloat("musicVolume", volume);
    }

    public void ChangeVolumeSFX(float volume)
    {
        mixer.SetFloat("SFXVolume", volume);
    }

    public void Start()
    {
        Debug.Log("Starting Game Main Menu");

        resolutions = Screen.resolutions;
        resolution.ClearOptions();
        List<string> options = new List<string>();


        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)//go through every resolution
        {
            //build a string for displaying the resolution
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                //Found current resolution, save it
                currentResolutionIndex = i;
            }
        }

        //set up the dropdown
        resolution.AddOptions(options);
        resolution.value = currentResolutionIndex;
        resolution.RefreshShownValue();

        if (!PlayerPrefs.HasKey("fullscreen"))
        {
            PlayerPrefs.SetInt("fullscreen", 0);
            Screen.fullScreen = false;

        }
        else
        {
            if (PlayerPrefs.GetInt("fullscreen") == 0)
            {
                Screen.fullScreen = false;
            }
            else
            {
                Screen.fullScreen = true;
            }
        }

        if (!PlayerPrefs.HasKey("quality"))
        {
            PlayerPrefs.SetInt("quality", 3);
            QualitySettings.SetQualityLevel(3);
        }
        else
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality"));
        }

        PlayerPrefs.Save();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");


#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, false);
    }

    public void SetFullScreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SavePlayerPrefs()
    {

        if (fullscreenToggle.isOn)
        {
            PlayerPrefs.SetInt("fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("fullscreen", 0);
        }

        float musicVolume;
        if (mixer.GetFloat("musicVolume", out musicVolume))
        {
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
        }
        float SFXVolume;
        if (mixer.GetFloat("SFXVolume", out SFXVolume))
        {
            PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
        }

        PlayerPrefs.SetInt("quality", QualitySettings.GetQualityLevel());
        //PlayerPrefs.SetInt("quality", qualityDropdown.value);

        PlayerPrefs.Save();
    }

    public void LoadPlayerPrefs()
    {
        if (PlayerPrefs.GetInt("fullscreen") == 0)
        {
            fullscreenToggle.isOn = false;
        }
        else
        {
            fullscreenToggle.isOn = true;
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("musicVolume");
            musicSlider.value = musicVolume;
            mixer.SetFloat("musicVolume", musicVolume);
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float SFXVolume = PlayerPrefs.GetFloat("SFXVolume");
            SFXSlider.value = SFXVolume;
            mixer.SetFloat("SFXVolume", SFXVolume);
        }

        qualityDropdown.value = PlayerPrefs.GetInt("quality");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(LoadScene);
    }

    public void MenuLoaded()
    {
        AnyKeyScene.SetActive(false);
        AnyKeySceneNext.SetActive(true);
    }

    void Update()
    {
        if (AnyKeyScene.activeSelf == true)
        {
            if (Input.anyKey)
            {
                MenuLoaded();
            }
        }
    }


}
