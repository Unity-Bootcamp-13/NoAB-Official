using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public enum VolumeType
{
    MASTER,
    BGM,
    SFX,
    VOICE
}

public class SetVolume : MonoBehaviour
{
    [Header("설정")]
    public AudioMixer mixer;
    public VolumeType volumeType;

    [Header("믹서 그룹 이름")]
    public string masterMixerGroup = "MasterVolume";
    public string bgmMixerGroup = "BgmVolume";
    public string sfxMixerGroup = "SfxVolume";
    public string voiceMixerGroup = "VoiceVolume";

    private Slider slider;
    private string playerPrefsKey;
    private string mixerGroupName;

    private void Start()
    {
        slider = GetComponent<Slider>();

        SetupVolumeType();

        float savedVolume = PlayerPrefs.GetFloat(playerPrefsKey, 1.0f);
        slider.value = savedVolume;
        ApplyVolumeToMixer(savedVolume);
    }
    private void SetupVolumeType()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                playerPrefsKey = "MasterVolume";
                mixerGroupName = masterMixerGroup;
                break;
            case VolumeType.BGM:
                playerPrefsKey = "BgmVolume";
                mixerGroupName = bgmMixerGroup;
                break;
            case VolumeType.SFX:
                playerPrefsKey = "SfxVolume";
                mixerGroupName = sfxMixerGroup;
                break;
            case VolumeType.VOICE:
                playerPrefsKey = "VoiceVolume";
                mixerGroupName = voiceMixerGroup;
                break;
        }
    }

    public void SetLevel(float sliderValue)
    {
        PlayerPrefs.SetFloat(playerPrefsKey, sliderValue);
        PlayerPrefs.Save();

        ApplyVolumeToMixer(sliderValue);
    }

    private void ApplyVolumeToMixer(float volume)
    {
        if (mixer == null) return;

        float dbValue = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f; ;
        mixer.SetFloat(mixerGroupName, dbValue);
    }

}