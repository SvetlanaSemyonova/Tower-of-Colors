using UnityEngine;
using UnityEngine.UI;

public class CustomToggle : MonoBehaviour
{
    [SerializeField] private GameObject enabledObject;
    [SerializeField] private GameObject disabledObject;
    [SerializeField] private Toggle.ToggleEvent toggleEvent = new Toggle.ToggleEvent();

    private bool isEnabled { get; set; }
    private Button button;


    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        isEnabled = false;
    }

    private void OnClick()
    {
        SetEnabled(!isEnabled);
    }

    public void SetEnabled(bool value)
    {
        isEnabled = value;
        enabledObject.SetActive(isEnabled);
        disabledObject.SetActive(!isEnabled);
        toggleEvent?.Invoke(isEnabled);
    }
}
