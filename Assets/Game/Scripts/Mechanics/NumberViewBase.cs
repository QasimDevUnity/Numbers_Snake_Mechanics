using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class NumberViewBase : MonoBehaviour
{
    [SerializeField] private Transform digitsParent;
    [Tooltip("For Player Specific FeedBack"),SerializeField] private CharacterType characterType;
    public UnityEvent actionToInvokeOnIncrement;
     float digitSpacing = 0.1f;

    private List<PooledObject> activeDigits = new();



    
    
    
    
    

    public void SetValue(int value) // no storage for tracking 
    {
        digitSpacing = GameManager.Instance.gameConfig.digitSpacing;
        ClearDigits();

        string numberString = Mathf.Abs(value).ToString();

        float totalWidth = (numberString.Length - 1) * digitSpacing;
        float startOffset = -totalWidth / 2f;

        for (int i = 0; i < numberString.Length; i++)
        {
            char digitChar = numberString[i];
            string poolKey = digitChar.ToString();

            PooledObject digitObj = GameManager.Instance.poolManager.GetFromPool(poolKey);

            digitObj.transform.SetParent(digitsParent);
            digitObj.transform.localPosition = new Vector3(startOffset + i * digitSpacing, 0, 0);
            //print(digitSpacing+"Digit Sp[acing Defined");

            // Optional Y rotation
           
            digitObj.transform.localRotation = Quaternion.Euler(0f, 180, 0f);
            /*if (customizeYVal)
            {
                
            }
            else
            {
                digitObj.transform.localRotation = Quaternion.identity;
            }*/

            digitObj.transform.localScale = Vector3.one;

            activeDigits.Add(digitObj);
            
            if (characterType == CharacterType.Player)
            {
                AnimateDigitPopup(digitObj.transform, i * 0.05f);
            }
            actionToInvokeOnIncrement?.Invoke();
        }
        
    }

    
    
    private void AnimateDigitPopup(Transform digitTransform, float delay)
    {
   
        HapticManager.CustomVibrate(40, 70);        digitTransform.localScale = Vector3.zero;
        digitTransform.DOScale(Vector3.one * 1.35f, 0.25f)
            .SetDelay(delay)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                digitTransform.DOScale(Vector3.one, 0.25f);
            });
    }

    private void ClearDigits()
    {
        for (int i = 0; i < activeDigits.Count; i++)
        {
            activeDigits[i].ReturnToPool();
        }

        activeDigits.Clear();
        transform.localScale = Vector3.one;
    }
}


[Serializable]
public enum CharacterType
{
   Other,Player
}