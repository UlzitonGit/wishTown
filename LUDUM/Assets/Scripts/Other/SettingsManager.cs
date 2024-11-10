using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private const string MouseSensitivityKey = "MouseSensitivity";
    private const string VolumeKey = "Volume";

    public static void SaveSettings(float mouseSensitivity, float volume)
    {
        PlayerPrefs.SetFloat(MouseSensitivityKey, mouseSensitivity);
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }

    public static float LoadMouseSensitivity(float defaultSensitivity)
    {
        return PlayerPrefs.GetFloat(MouseSensitivityKey, defaultSensitivity);
    }

    public static float LoadVolume(float defaultVolume)
    {
        return PlayerPrefs.GetFloat(VolumeKey, defaultVolume);
    }
}