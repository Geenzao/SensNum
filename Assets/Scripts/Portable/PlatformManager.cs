using UnityEngine;


//Ce scripte a pour but de gerer quand on est sur un portable ou sur un ordinateur


public class PlatformManager : Singleton<PlatformManager>
{
    private bool isMobile = false;

    private bool isAlwedyCalculate = false;
    public void DetectPlatform()
    {
        isAlwedyCalculate = true;
        // Détection standard sur Android/iOS
        if (Application.isMobilePlatform)
        {
            isMobile = true;
        }
        // Cas WebGL : On utilise SystemInfo.deviceType
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                isMobile = true;
            }
        }

        /*Debug.Log(isMobile ? "Mobile détecté" : "Ordinateur détecté");*/
    }


    public bool fctisMobile()
    {
        if (isAlwedyCalculate == false)
            DetectPlatform();
        //ensuite dans tous les cas on envoie la réponse
        return isMobile;
        //return true;
    }
}
