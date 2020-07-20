﻿using UnityEditor;
using UnityEngine;
using System.IO;

namespace UnityVolumeRendering
{
    public class VolumeRendererEditorFunctions
    {
        [MenuItem("Volume Rendering/Load raw dataset")]
        static void ShowDatasetImporter()
        {
            string file = EditorUtility.OpenFilePanel("Select a dataset to load", "DataFiles", "");
            if (File.Exists(file))
            {
                EditorDatasetImporter.ImportDataset(file);
            }
            else
            {
                Debug.LogError("File doesn't exist: " + file);
            }
        }

        [MenuItem("Volume Rendering/Load DICOM")]
        static void ShowDICOMImporter()
        {
            string dir = EditorUtility.OpenFolderPanel("Select a folder to load", "", "");
            if (Directory.Exists(dir))
            {
                DICOMImporter importer = new DICOMImporter(dir, true);
                VolumeDataset dataset = importer.Import();
                if(dataset != null)
                    VolumeObjectFactory.CreateObject(dataset);
            }
            else
            {
                Debug.LogError("Directory doesn't exist: " + dir);
            }
        }

        [MenuItem("Volume Rendering/Enable preintegrated texture")]
        public static void EnablePreintTF()
        {
            VolumeRenderedObject volObj = GameObject.FindObjectOfType<VolumeRenderedObject>();
            Texture2D preintTex = volObj.transferFunction.CalculatePreintegratedTexture();
            volObj.meshRenderer.sharedMaterial.SetTexture("_TFPreintTex", preintTex);
            volObj.meshRenderer.sharedMaterial.EnableKeyword("PREINTEGRATED_TF");
        }

        [MenuItem("Volume Rendering/Disable preintegrated texture")]
        public static void DisablePreintTF()
        {
            VolumeRenderedObject volObj = GameObject.FindObjectOfType<VolumeRenderedObject>();
            volObj.meshRenderer.sharedMaterial.DisableKeyword("PREINTEGRATED_TF");
        }
    }
}
