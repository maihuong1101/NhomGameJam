using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class forcebar : MonoBehaviour
{
    [SerializeField] private Image forceBar;
    public void SetForce(float force)
    {
        forceBar.fillAmount = force;
    }
}
