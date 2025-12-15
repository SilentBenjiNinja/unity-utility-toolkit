using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace bnj.utility_toolkit.Editor
{
    // TODO: why does it not spit out DLL?
    public static class BuildUtilityToolkitDLL
    {
        //[MenuItem("BNJ/Build Utility Toolkit DLL")]
        static void BuildDLL()
        {
            var packagePath = "Packages/com.bnj.utility-toolkit/Runtime";
            var outputPath = "Packages/com.bnj.utility-toolkit/Plugins";
            var dllName = "bnj.utility-toolkit.dll";
            
            // Create Plugins folder if it doesn't exist
            Directory.CreateDirectory(outputPath);
            
            // Get all .cs files in Runtime folder
            var scriptFiles = Directory.GetFiles(packagePath, "*.cs", SearchOption.AllDirectories);
            
            // Compile to DLL
            var assemblyBuilder = new AssemblyBuilder(
                Path.Combine(outputPath, dllName), 
                scriptFiles
            );
            
            // Add references (UnityEngine, etc.)
            assemblyBuilder.additionalReferences = new[]
            {
                "Library/ScriptAssemblies/UnityEngine.CoreModule.dll",
                "Library/ScriptAssemblies/UnityEngine.dll"
            };
            
            // Define symbols
            assemblyBuilder.buildTarget = EditorUserBuildSettings.activeBuildTarget;
            assemblyBuilder.buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            
            // Build
            assemblyBuilder.buildStarted += path => Debug.Log($"Building DLL: {path}");
            assemblyBuilder.buildFinished += (path, messages) =>
            {
                Debug.Log($"DLL built: {path}");
                AssetDatabase.Refresh();
            };
            
            if (!assemblyBuilder.Build())
            {
                Debug.LogError("Failed to build DLL");
            }
        }
    }
}