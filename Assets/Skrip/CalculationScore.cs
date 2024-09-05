using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CalculationScore : MonoBehaviour
{

    [Header("Current Value")]
    public float CurrentValue;

    [Header("Event Settings")]
    public UnityEvent StartEvents;
    public UnityEvent UpdateEvents;
    public UnityEvent AfterTransferEvents;

    float TotalScore;
    bool StartTransfer = false;


    // Start is called before the first frame update
    void Start()
    {
        StartEvents?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEvents?.Invoke();

        if (StartTransfer)
        {
            CurrentValue += 1f;
            if (CurrentValue > TotalScore)
            {
                CurrentValue = TotalScore;
                StartTransfer = false;
                AfterTransferEvents?.Invoke();
            }
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat(transform.gameObject.name, CurrentValue);
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(transform.name))
        {
            CurrentValue = PlayerPrefs.GetFloat(transform.name);
        }
    }

    public void SetCurrentValue(float aValue)
    {
        CurrentValue = aValue;
        if (CurrentValue <= 0) CurrentValue = 0;
    }

    public void AddToCurrentValue(float aValue)
    {
        CurrentValue += aValue;
    }

    public void AddToCurrentValue(CalculationScore aValue)
    {
        CurrentValue += aValue.CurrentValue;
    }

    public void SubtractFromCurrentValue(float aValue)
    {
        CurrentValue -= aValue;
        if (CurrentValue <= 0) CurrentValue = 0;
    }

    public void SubtractFromCurrentValue(CalculationScore aValue)
    {
        CurrentValue -= aValue.CurrentValue;
        if (CurrentValue <= 0) CurrentValue = 0;
    }

    public void WriteTo(Slider aValue)
    {
        aValue.value = CurrentValue;
    }

    public void WriteTo(InputField aValue)
    {
        aValue.text = CurrentValue.ToString();
    }

    public void WriteTo(Text aValue)
    {
        aValue.text = CurrentValue.ToString();
    }

    public void WriteTo(TextMesh aValue)
    {
        aValue.text = CurrentValue.ToString();
    }

    public void TransferToCurrentValue(float aValue)
    {
        TotalScore = CurrentValue + aValue;
        StartTransfer = true;
        Invoke("ThreeSecond", 3);
    }
    public void ReadFrom(Slider aValue)
    {
        CurrentValue = aValue.value;
    }
    public void ReadFrom(InputField aValue)
    {
        CurrentValue = float.Parse(aValue.text);
    }

    public void ReadFrom(Text aValue)
    {
        CurrentValue = float.Parse(aValue.text);
    }

    public void ReadFrom(TextMesh aValue)
    {
        CurrentValue = float.Parse(aValue.text);
    }

    void ThreeSecond()
    {
        CurrentValue = TotalScore;
    }
}
