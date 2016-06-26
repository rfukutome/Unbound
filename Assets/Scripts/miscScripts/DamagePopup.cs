using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///////////////////////////////////////////////////////////////////////////
//   Class:      DamagePopup
//   Purpose:    This class deals with the simple function of setting the text
//   of the damage popup to the incoming damage.
//   
//   Notes: Attach onto damagePopUp prefab.
//   Contributors: RSF
///////////////////////////////////////////////////////////////////////////

public class DamagePopup : MonoBehaviour {
    [SerializeField]
    private Text damageText;

    public void SetText(string argText)
    {
        damageText.text = argText;
        Destroy(gameObject, 0.6f);
    }
}
