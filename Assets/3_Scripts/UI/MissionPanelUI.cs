using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanelUI : MonoBehaviour
{
    [SerializeField] private Image check;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject fillBarContainer;
    [SerializeField] private Image fillBar;
    [SerializeField] private TextMeshProUGUI fillBarText;
    [SerializeField] private GameObject claimButton;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private Sprite[] rewardIconSprites;
    [SerializeField] private TextMeshProUGUI rewardQuantityText;

    private int missionIndex;
    private MissionSystem missionSystem;
    private Action OnCompleted;
    public void PrepareForMission(int missionIndex, MissionSystem missionSystem, Action OnCompleted)
    {
        this.missionIndex = missionIndex;
        this.missionSystem = missionSystem;
        this.OnCompleted = OnCompleted;

        UpdateData();
    }

    public void UpdateData()
    {
        var mission = missionSystem.GetMission(missionIndex);
        var missionRewardClaimed = missionSystem.GetSelectedMissionRewardClaimed(missionIndex);
        var progress = missionSystem.GetSelectedMissionProgress(missionIndex);
        var reward = missionSystem.GetReward(missionIndex);

        check.enabled = missionRewardClaimed;
        descriptionText.text = mission.GetDescription();
        fillBarContainer.SetActive(!missionRewardClaimed && progress < mission.GetGoal());
        fillBar.fillAmount = progress / (float)mission.GetGoal();
        fillBarText.text = progress + "/" + mission.GetGoal();
        claimButton.SetActive(!missionRewardClaimed && progress >= mission.GetGoal());
        rewardIcon.sprite = rewardIconSprites[(int)reward.GetRewardType()];
        rewardQuantityText.text = reward.GetQuantity().ToString();
    }

    public void ClaimButton()
    {
        missionSystem.ClaimMissionReward(missionIndex);
        OnCompleted?.Invoke();
    }
}
