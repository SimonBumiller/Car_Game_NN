using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace Io
{
    public class IterationManager
    {
        private readonly string IterationDir = Path.Combine(FileManager.STATS_DIR, "iterations");

        public static readonly IterationManager Instance = new();

        public void SaveIteration(IterationInfo iteration)
        {
            var json = JsonUtility.ToJson(iteration);
            var date = DateTime.Now.ToString("dd.MM.yyyy-hh-mm-ss");
            
            var filename = "Iteration_" + date + ".json";

            var rootDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var path = Path.Combine(rootDir, IterationDir);
            var filePath = Path.Combine(path, filename);
            
            FileManager.Instance.CreateFile(filename, path);
            FileManager.Instance.WriteToFile(json, filePath);
        }
    }
}