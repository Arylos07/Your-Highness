using UnityEditor;
using UnityEngine;

public class StartVisualStudio : MonoBehaviour
{
    //this function is a little broken now. The product name is "Your Highness", while the solution is "Your-Highness.sln". I need to find a better way for this tool to work.
    //[MenuItem("Tools/Open Visual Studio", false, 0)]
    public static void OpenVisualStudio()
    {
        //if (VSOpen) return;

        string path = Application.dataPath;
        path = path.Replace("/Assets", string.Empty);
        path += "/" + Application.productName + ".sln";
        //Debug.Log(path);
        System.Diagnostics.Process.Start(path);
    }

    //public static bool VSOpen => System.Diagnostics.Process.GetProcessesByName("devenv").Length > 0;
}