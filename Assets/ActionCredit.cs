using UnityEngine;

public class ActionCredit : MonoBehaviour
{
    void Update()
    {
        if (UIManager.CurrentMenuState == UIManager.MenuState.None)
        {
            UIManager.Instance.UpdateMenuState(UIManager.MenuState.Credits);
            GameObject.Destroy(this);
        }
    }
}
