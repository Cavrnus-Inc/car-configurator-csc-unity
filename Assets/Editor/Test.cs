using CavrnusSdk.Setup;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    [MenuItem("Cavrnus/TEST", false, 0)]
    public static void SetupSceneForCavrnus()
    {
        if (GameObject.Find("Cavrnus Spatial Connector") != null) {
            Debug.LogWarning(
                "A Cavrnus Spatial Connector object already exists in your scene.  If you wish to replace it please delete it first.");
            return;
        }

        string cscPackagePath = "Packages/com.cavrnus.csc/CavrnusSdk/Runtime/Prefabs/Cavrnus Spatial Connector.prefab";
        string cscAssetsPath = "Assets/com.cavrnus.csc/CavrnusSdk/Runtime/Prefabs/Cavrnus Spatial Connector.prefab";

        var corePrefab = AssetDatabase.LoadAssetAtPath<CavrnusSpatialConnector>(cscPackagePath);

        //For development project
        if (corePrefab == null) corePrefab = AssetDatabase.LoadAssetAtPath<CavrnusSpatialConnector>(cscAssetsPath);

        if (corePrefab == null) {
            Debug.LogError(
                $"Cavrnus Spatial Connector prefab was not found at its expected location ({cscPackagePath}).  Please update or reinstall the Plugin to fix!");
            return;
        }
    }
}
