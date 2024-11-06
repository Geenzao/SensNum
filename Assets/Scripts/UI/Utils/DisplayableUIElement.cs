using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayableUIElement : MonoBehaviour
{
    //protected Animator _animator;
    protected bool _animShouldAppear = false;

    protected virtual void TriggerVisibility(bool visible)
    {
        _animShouldAppear = visible;
        //_animator.SetBool("ShouldAppear", _animShouldAppear);
        if (visible)
            gameObject.SetActive(true);
        else
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        if (GetComponent<Image>())
            GetComponent<Image>().enabled = visible;
    }
}
