using TMPro;
using UnityEngine;

public class MissionsUI : Singleton<MissionsUI>
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MissionPanelUI[] missionPanelUIs;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI coinsAmount, gemsAmount;

    private bool isOpen = false;

    private void Awake()
    {
        animator.speed = 1.0f / Time.timeScale;
        isOpen = false;
        animator.SetBool("isOpen", isOpen);

        for (var i = 0; i < gameManager.missionSystem.GetTotalMissions() && i < missionPanelUIs.Length; i++)
        {
            missionPanelUIs[i].PrepareForMission(i, gameManager.missionSystem, RefreshPanels);
        }

        RefreshCoins();
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);

        if(isOpen)
        {
            RefreshPanels();
        }
    }

    private void RefreshPanels()
    {
        foreach (var mission in missionPanelUIs)
        {
            mission.UpdateData();
        }
    }

    public void RefreshCoins()
    {
        coinsAmount.text = SaveData.SoftCoin.ToString();
        gemsAmount.text = SaveData.HardCoin.ToString();
    }
}
