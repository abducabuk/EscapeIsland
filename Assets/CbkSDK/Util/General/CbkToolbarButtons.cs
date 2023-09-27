#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using CbkSDK.Core.ServiceLocator;
using CbkSDK.Level.Command;
using ThirdParty.Tools.ExternalToolbar;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CbkSDK.Util.General
{
    public static class ToolbarStyles
    {
        public static GUIStyle SmallCommandButtonStyle() =>new GUIStyle("Command")
        {
            fixedWidth = 20,
            fontSize = 9,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageLeft,
            fontStyle = FontStyle.Bold
        };
        public static GUIStyle MediumCommandButtonStyle() => new GUIStyle("Command")
        {
            fixedWidth = 35,
            fontSize = 9,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageLeft,
            fontStyle = FontStyle.Bold
        };
        public static GUIStyle LargeCommandButtonStyle() => new GUIStyle("Command")
        {
            fixedWidth = 55,
            fontSize = 9,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageLeft,
            fontStyle = FontStyle.Bold
        };
        public static GUIStyle XLargeCommandButtonStyle() => new GUIStyle("Command")
        {
            fixedWidth = 80,
            fontSize = 8,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageLeft,
            fontStyle = FontStyle.Bold
        };
    }

    [InitializeOnLoad]
    public class CbkToolbarButtons : IPreprocessBuildWithReport
    {

        static CbkToolbarButtons()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUILeft);
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUIRight);
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

        }

        static List<SceneAsset> scenes = new List<SceneAsset>();

        static void OnToolbarGUILeft()
        {

            GUILayout.BeginHorizontal();

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (GUILayout.Button(new GUIContent(i.ToString()), ToolbarStyles.SmallCommandButtonStyle()))
                {
                    SceneHelper.OpenScene(EditorBuildSettings.scenes[i].path);
                }
            }
            GUILayout.EndHorizontal();
        }

        static void OnToolbarGUIRight()
        {

            GUILayout.BeginHorizontal();
            
            AddEditModeButtonOnToolbar("SaveAssets", Color.magenta, ToolbarStyles.LargeCommandButtonStyle(), 
                (buttonName,buttonColor,buttonGuiStyle) =>
                {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                });


            if (ServiceLocator.Instance)
            {
                AddPlayModeButtonOnToolbar("NxtLvl",Color.cyan,ToolbarStyles.MediumCommandButtonStyle(), 
                    (buttonName,buttonColor,buttonGuiStyle) =>
                    {
                        //DO SOME STUFF
                        new NextLevelCommand().Execute();
                    });
                AddPlayModeButtonOnToolbar("PreLvl",Color.yellow,ToolbarStyles.MediumCommandButtonStyle(), 
                    (buttonName,buttonColor,buttonGuiStyle) =>
                    {
                        //DO SOME STUFF
                        new PreviousLevelCommand().Execute();
                    });
                AddPlayModeButtonOnToolbar("Success",Color.green, ToolbarStyles.MediumCommandButtonStyle(), 
                    (buttonName,buttonColor,buttonGuiStyle) =>
                    {
                        //DO SOME STUFF
                        new SuccessLevelCommand().Execute();
                    });
                AddPlayModeButtonOnToolbar("Fail",Color.red,ToolbarStyles.MediumCommandButtonStyle(), 
                    (buttonName,buttonColor,buttonGuiStyle) =>
                    {
                        //DO SOME STUFF
                        new FailLevelCommand().Execute();
                    });
            }
            GUILayout.EndHorizontal();
        }

        private static void AddPlayModeButtonOnToolbar(string buttonName, Color color, GUIStyle guiStyle, Action<string,Color,GUIStyle> onClick)
        {
            if (Application.isPlaying)
            {
                AddButtonOnToolbar(buttonName,color,guiStyle,onClick);
            }
        }
        
        private static void AddEditModeButtonOnToolbar(string buttonName, Color color, GUIStyle guiStyle, Action<string,Color,GUIStyle> onClick)
        {
            if (!Application.isPlaying)
            {
                AddButtonOnToolbar(buttonName,color,guiStyle,onClick);
            }
        }

        private static void AddButtonOnToolbar(string buttonName, Color color, GUIStyle guiStyle, Action<string,Color,GUIStyle> onClick)
        {
            guiStyle.normal.textColor = color;
            var guiContent = new GUIContent(buttonName);
            if (GUILayout.Button(guiContent, guiStyle))
            {
                onClick?.Invoke(buttonName,color,guiStyle);
            }
        }

        public int callbackOrder { get; }
        
        private static void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    //DO Some stuff when enter editmode !
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    //DO Some stuff when exit editmode before press play !
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    //DO Some stuff when enter play mode after press play !
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    //DO Some stuff when exit play mode after press stop!
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
            }
        }
        public void OnPreprocessBuild(BuildReport report)
        {
            //DO Some stuff before BUILDING!!!!!
        }
    }
    
    static class SceneHelper
    {
        static string sceneToOpen;

        public static void OpenScene(string scene)
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(scene);
        }
    }
}
#endif
