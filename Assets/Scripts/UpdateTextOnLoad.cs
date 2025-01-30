using UnityEngine;

public class UpdateTextOnLoad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LanguageManager.InvokeOnLanguageChange();
        Destroy(gameObject);
    }
}
