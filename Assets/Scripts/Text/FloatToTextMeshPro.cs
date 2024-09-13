using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatToTextMeshPro : MonoBehaviour
{
    public DataInt_SO moneySO;
    public TextMeshProUGUI moneyText;

    void Start()
    {
        UpdateText();
    }
    
    public void UpdateText()
    {
        moneyText.text = $"${moneySO.data.ToString()}";
    }
}
