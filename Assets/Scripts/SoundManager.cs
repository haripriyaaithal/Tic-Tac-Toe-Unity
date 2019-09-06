using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

    [Header("Audio Clips")]
    [SerializeField] private AudioClip playerTouch;
    [SerializeField] private AudioClip aiSound;
    [SerializeField] private AudioClip playerWon;
    [SerializeField] private AudioClip aiWon;
    [SerializeField] private AudioClip gameDraw;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPlayerOneSound() {
        audioSource.PlayOneShot(playerTouch);
    }

    public void PlayPlayerTwoSound() {
        audioSource.PlayOneShot(aiSound);
    }

    public void PlayPlayerWon() {
        audioSource.PlayOneShot(playerWon);
    }

    public void PlayPlayerTwoWon() {
        audioSource.PlayOneShot(aiWon);
    }

    public void PlayGameDrawSound() {
        audioSource.PlayOneShot(gameDraw);
    }

    public void UpdateMusicVolume(float volume) {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSFXVolume(float volume) {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        audioMixer.SetFloat("SFXVolume", volume);
    }
}
