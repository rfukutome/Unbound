using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour {
    [SerializeField]
    private Text damageText;

    public void SetText(string argText)
    {
        damageText.text = argText;
        Destroy(gameObject, 0.6f);
    }
}
