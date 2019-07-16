using UnityEngine;
using UnityEngine.UI;

public class PlayerPreferences : MonoBehaviour {

    [Header("Option Menu")]
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider SFXVolume;

    void Start() {
        SetValuesForSliders();
    }

    private void SetValuesForSliders() {
        float musicVolumeValue = PlayerPrefs.GetFloat("MusicVolume", -15f);
        float sfxVolumeValue = PlayerPrefs.GetFloat("SFXVolume", 0f);

        musicVolume.value = musicVolumeValue;
        SFXVolume.value = sfxVolumeValue;
    }
}
