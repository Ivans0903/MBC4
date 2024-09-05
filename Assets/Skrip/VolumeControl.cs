using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // Reference ke UI Slider
    public AudioSource audioSource; // Reference ke Audio Source

    void Start()
    {
        // Mengatur nilai default slider berdasarkan volume AudioSource
        volumeSlider.value = audioSource.volume;

        // Tambahkan listener untuk menangani perubahan nilai slider
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
