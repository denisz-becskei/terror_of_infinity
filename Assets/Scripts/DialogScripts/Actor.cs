using Unity.VisualScripting;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public static string CHARACTER_MAIN_NAME;

    public static void FixName()
    {
        CHARACTER_MAIN_NAME = CHARACTER_MAIN_NAME.TrimStart("I'm ").TrimEnd(".");
    }
}
