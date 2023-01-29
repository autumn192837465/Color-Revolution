using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEditor;
using UnityEngine;

public static class Example
{
// Add a menu item to create custom GameObjects.
    // Priority 10 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarchy context menus.
    [MenuItem("GameObject/MMFPlayer", false, 10)]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("Feedbacks");
        go.AddComponent<MMF_Player>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Feedbacks");
        Selection.activeObject = go;
    }
}