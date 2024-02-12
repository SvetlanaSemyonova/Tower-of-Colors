using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboUI : MonoBehaviour
{
    [SerializeField] private List<Animation> encouragementList;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private Animator comboAnimation;
    [SerializeField] private int minCount = 5;
    [SerializeField] private int countForEncouragement = 10;
    [SerializeField] private float finishComboTime = 1;
    [SerializeField] private RectTransform comboRect;
    [SerializeField] private float moveTime;

    private int currentCombo = 0;
    private bool showingCombo = false;

    private Camera mainCamera;
    private Vector3 targetPos;
    private Coroutine resetComboRoutine;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (showingCombo)
            comboRect.position = Vector3.Lerp(comboRect.position, targetPos, moveTime * Time.deltaTime);
    }

    private void OnTileDestroyed(TowerTile obj)
    {
        CountCombo(obj.transform.position);
    }

    public void CountCombo(Vector3 worldPos)
    {
        if (isActiveAndEnabled) {
            if (++currentCombo > minCount) {
                targetPos = mainCamera.WorldToScreenPoint(worldPos);
                comboText.text = $"x{currentCombo}";
                if (!showingCombo) {
                    showingCombo = true;
                    comboRect.position = targetPos;
                    comboAnimation.SetBool("show", true);
                }
                comboAnimation.SetTrigger("bounce");
            }
            if (resetComboRoutine != null)
                StopCoroutine(resetComboRoutine);
            resetComboRoutine = StartCoroutine(FinishCombo());
        }
    }

    private void ShowEncouragement()
    {
        var randomAnim = encouragementList[Random.Range(0, encouragementList.Count)];
        randomAnim.transform.position = targetPos + Vector3.up * 50;
        randomAnim.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(8, 20) * Mathf.Sign(Random.value - 0.5f));
        randomAnim.Play();
    }

    private IEnumerator FinishCombo()
    {
        yield return new WaitForSeconds(finishComboTime * Time.timeScale);
       
        if (currentCombo >= countForEncouragement)
        {
            ShowEncouragement();
        }
        
        showingCombo = false;
        comboAnimation.SetBool("show", false);
        GameManager.Instance.missionSystem.ReportData(Mission.DataType.Combo, currentCombo);
        currentCombo = 0;
        resetComboRoutine = null;
    }
}
