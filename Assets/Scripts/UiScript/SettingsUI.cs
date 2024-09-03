using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SettingsUI : UiBase
{
    [SerializeField] private string uid = UiConstant.SETTING_UI;
    public Toggle soundToggle;
    public Slider volumeSlider;

    public override void SetData(UiBaseData data)
    {

    }

    public void AssignEvents()
    {
        soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }

    private void OnSoundToggleChanged(bool isOn)
    {
        Debug.Log($"Sound toggled: {isOn}");
        // Add your action here
    }

    private void OnVolumeSliderChanged(float volume)
    {
        Debug.Log($"Volume changed: {volume}");
        // Add your action here
    }
    public override string GetUiId()
    {
        return uid;
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }
}
[System.Serializable]
public class SettingsData
{
    public bool isSoundOn;
    public float volume;
}



