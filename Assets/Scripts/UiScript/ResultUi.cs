using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUi : UiBase
{
    [SerializeField] private string uid = UiConstant.RESULT_UI;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI coinAmount;
    [SerializeField] private Button continueButton;
    Action onContinue;

    private void Start()
    {
        continueButton.onClick.AddListener(() => onContinue?.Invoke());
    }
    public override void SetData(UiBaseData data)
    {
        if (data is ResultUiData _resultUiData)
        {
            ResultUiData resultUiData = _resultUiData;
            SetCoinAmount(resultUiData.CoinAmount);
            SetResultText(resultUiData.ResultType);

        }

    }

    private void SetResultText(ResultType resultType)
    {
        switch (resultType)
        {
            case ResultType.Win:
                resultText.text = "You Win!";
                break;
            case ResultType.Lose:
                resultText.text = "You Lose!";
                break;
        }
    }

    public void SetCoinAmount(long amount)
    {
        coinAmount.text = amount.ToString();
    }

    public void AssignEvents(Action _onContinue)
    {
        onContinue = _onContinue;
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
public class ResultUiData : UiBaseData
{
    public long CoinAmount;
    public ResultType ResultType;
}
public enum ResultType
{
    Win,
    Lose
}
