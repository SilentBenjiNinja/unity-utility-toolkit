using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace bnj.utility_toolkit.Editor
{
    public static class BuildUtilityToolkitDLL
    {
        const string PackagePath = "Packages/com.bnj.utility-toolkit/Runtime";
        const string OutputPath = "Packages/com.bnj.utility-toolkit/Plugins";
        const string DLLName = "bnj.utility-toolkit.dll";

        [MenuItem("BNJ/Build Utility Toolkit DLL")]
        static void BuildDLL()
        {
            Directory.CreateDirectory(OutputPath);

            // Get all .cs files EXCEPT .asmdef files
            var scriptFiles = Directory.GetFiles(PackagePath, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.EndsWith(".asmdef") && !f.EndsWith(".asmdef.disabled"))
                .ToArray();

            if (scriptFiles.Length == 0)
            {
                Debug.LogError("No C# files found");
                return;
            }

            Debug.Log($"Found {scriptFiles.Length} files to compile");

            var cscPath = GetCompilerPath();
            if (string.IsNullOrEmpty(cscPath) || !File.Exists(cscPath))
            {
                Debug.LogError($"C# compiler not found at: {cscPath}");
                return;
            }

            // Get Unity references from the actual Unity installation
            var references = GetUnityReferences();

            if (references.Length == 0)
            {
                Debug.LogError("No Unity references found");
                return;
            }

            Debug.Log($"Found {references.Length} references");
            foreach (var reference in references)
            {
                Debug.Log($"  - {Path.GetFileName(reference)}");
            }

            var refArgs = string.Join(" ", references.Select(r => $"-r:\"{r}\""));
            var sources = string.Join(" ", scriptFiles.Select(f => $"\"{f}\""));
            var output = Path.Combine(OutputPath, DLLName);

            // Use -nostdlib and specify all references explicitly
            var args = $"-target:library -nostdlib+ -out:\"{output}\" {refArgs} {sources} -langversion:9.0 -optimize+";

            ExecuteCompiler(cscPath, args);
        }

        static string[] GetUnityReferences()
        {
            var references = new System.Collections.Generic.List<string>();

            // Try multiple possible paths for managed assemblies
            var monoPath = Path.Combine(EditorApplication.applicationContentsPath, "MonoBleedingEdge");

            var possibleManagedPaths = new[]
            {
                Path.Combine(monoPath, "lib", "mono", "unityjit-win32"),
                Path.Combine(monoPath, "lib", "mono", "unityjit"),
                Path.Combine(monoPath, "lib", "mono", "4.5"),
                Path.Combine(EditorApplication.applicationContentsPath, "Managed"),
                Path.Combine(EditorApplication.applicationContentsPath, "Data", "Managed")
            };

            string managedPath = null;
            foreach (var path in possibleManagedPaths)
            {
                if (Directory.Exists(path))
                {
                    // Check if it actually contains mscorlib.dll
                    if (File.Exists(Path.Combine(path, "mscorlib.dll")))
                    {
                        managedPath = path;
                        Debug.Log($"Found managed path: {managedPath}");
                        break;
                    }
                }
            }

            if (managedPath == null)
            {
                Debug.LogError($"Could not find managed assemblies in any expected location.");
                Debug.LogError($"Tried paths:");
                foreach (var path in possibleManagedPaths)
                {
                    Debug.LogError($"  - {path}");
                }
                return new string[0];
            }

            // Core .NET assemblies (required with -nostdlib)
            var coreAssemblies = new[]
            {
                "mscorlib.dll",
                "System.dll",
                "System.Core.dll"
            };

            // Try to find core assemblies in Managed folder
            foreach (var assembly in coreAssemblies)
            {
                var path = Path.Combine(managedPath, assembly);
                if (File.Exists(path))
                {
                    references.Add(path);
                }
                else
                {
                    Debug.LogError($"REQUIRED assembly not found: {assembly} at {path}");
                    return new string[0];
                }
            }

            // Try facades folder for additional references
            var facadesPath = Path.Combine(managedPath, "Facades");
            if (Directory.Exists(facadesPath))
            {
                var facadeAssemblies = new[]
                {
                    "System.Runtime.dll",
                    "System.Collections.dll",
                    "System.Linq.dll"
                };

                foreach (var facade in facadeAssemblies)
                {
                    var path = Path.Combine(facadesPath, facade);
                    if (File.Exists(path))
                    {
                        references.Add(path);
                    }
                }
            }

            // Now find Unity assemblies
            var unityManagedPaths = new[]
            {
                Path.Combine(EditorApplication.applicationContentsPath, "Managed"),
                Path.Combine(EditorApplication.applicationContentsPath, "Data", "Managed")
            };

            string unityManagedPath = null;
            foreach (var path in unityManagedPaths)
            {
                if (Directory.Exists(path) && File.Exists(Path.Combine(path, "UnityEngine.dll")))
                {
                    unityManagedPath = path;
                    Debug.Log($"Found Unity managed path: {unityManagedPath}");
                    break;
                }
            }

            if (unityManagedPath != null)
            {
                var unityAssemblies = new[]
                {
                    "UnityEngine.dll",
                    "UnityEngine.CoreModule.dll"
                };

                foreach (var assembly in unityAssemblies)
                {
                    // Try root managed path first
                    var path = Path.Combine(unityManagedPath, assembly);
                    if (File.Exists(path))
                    {
                        references.Add(path);
                        continue;
                    }

                    // Try UnityEngine subfolder
                    var unityModulesPath = Path.Combine(unityManagedPath, "UnityEngine");
                    path = Path.Combine(unityModulesPath, assembly);
                    if (File.Exists(path))
                    {
                        references.Add(path);
                    }
                    else
                    {
                        Debug.LogWarning($"Unity assembly not found: {assembly}");
                    }
                }
            }
            else
            {
                Debug.LogWarning("Could not find Unity managed assemblies");
            }

            return references.ToArray();
        }

        static string GetCompilerPath()
        {
            var monoPath = Path.Combine(EditorApplication.applicationContentsPath, "MonoBleedingEdge");

#if UNITY_EDITOR_WIN
            // Try roslyn compiler FIRST (it's more self-contained)
            var roslynPath = Path.Combine(EditorApplication.applicationContentsPath, "Tools", "Roslyn", "csc.exe");
            if (File.Exists(roslynPath))
            {
                Debug.Log($"Using Roslyn compiler: {roslynPath}");
                return roslynPath;
            }

            var path = Path.Combine(monoPath, "lib", "mono", "4.5", "csc.exe");
            if (File.Exists(path)) return path;

            // Try alternate location for newer Unity versions
            path = Path.Combine(monoPath, "bin", "csc.exe");
            if (File.Exists(path)) return path;
#elif UNITY_EDITOR_OSX
            var path = Path.Combine(monoPath, "bin", "mcs");
            if (File.Exists(path)) return path;
#else
            var path = Path.Combine(monoPath, "bin", "mcs");
            if (File.Exists(path)) return path;
#endif

            return null;
        }

        static void ExecuteCompiler(string compiler, string args)
        {
            Debug.Log($"Executing: {compiler}");
            Debug.Log($"Arguments: {args}");

            // Add Mono's bin directory to PATH so compiler can find its dependencies
            var monoPath = Path.Combine(EditorApplication.applicationContentsPath, "MonoBleedingEdge");
            var monoBinPath = Path.Combine(monoPath, "bin");
            var monoLibPath = Path.Combine(monoPath, "lib");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = compiler,
                    Arguments = args,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    EnvironmentVariables =
                    {
                        ["PATH"] = $"{monoBinPath};{monoLibPath};{System.Environment.GetEnvironmentVariable("PATH")}"
                    }
                }
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var errors = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(output))
            {
                Debug.Log($"Compiler output:\n{output}");
            }

            if (process.ExitCode == 0)
            {
                Debug.Log($"[SUCCESS] DLL built successfully: {Path.Combine(OutputPath, DLLName)}");
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError($"[FAILED] Build failed (Exit code: {process.ExitCode}):\n{errors}");
            }
        }
    }
}