using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Mono.CecilX;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditorInternal;
using Debug = UnityEngine.Debug;
using UnityAssembly = UnityEditor.Compilation.Assembly;

namespace UniMob.Editor.Weaver
{
    public static class CompilationHook
    {
        private const string UniMobWeavedFlagName = "UniMobWeaved";

        private const string UniMobRuntimeAssemblyName = "UniMob";
        private const string UniMobEditorAssemblyName = "UniMob.Editor";

        [InitializeOnLoadMethod]
        private static void OnInitializeOnLoad()
        {
            CompilationPipeline.assemblyCompilationFinished += OnCompilationFinished;

            if (!SessionState.GetBool(UniMobWeavedFlagName, false))
            {
                SessionState.SetBool(UniMobWeavedFlagName, true);
                WeaveExistingAssemblies(CompilationPipeline.GetAssemblies());
            }
        }

        private static void WeaveExistingAssemblies(UnityAssembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                if (File.Exists(assembly.outputPath))
                {
                    OnCompilationFinished(assembly.outputPath, new CompilerMessage[0]);
                }
            }

            InternalEditorUtility.RequestScriptReload();
        }

        private static UnityAssembly FindUniMobRuntime(UnityAssembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                if (assembly.name == UniMobRuntimeAssemblyName)
                {
                    return assembly;
                }
            }

            return null;
        }

        private static void OnCompilationFinished(string assemblyPath, CompilerMessage[] messages)
        {
            if (Array.FindIndex(messages, m => m.type == CompilerMessageType.Error) != -1)
            {
                return;
            }

            var assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
            if (assemblyName == UniMobRuntimeAssemblyName)
            {
                return;
            }
 
            if (assemblyName == UniMobEditorAssemblyName)
            {
                // should we reload scripts when Weaver changed?
                // it allow package update but assemblies will reload twice
                // on project startup
                //SessionState.SetBool(UniMobWeavedFlagName, false);
                return;
            }
 
            if (assemblyPath.Contains("-Editor") || assemblyPath.Contains(".Editor"))
            {
                return;
            }

            var assemblies = CompilationPipeline.GetAssemblies();

            var uniMobRuntime = FindUniMobRuntime(assemblies);
            if (uniMobRuntime == null)
            {
                Debug.LogError("Failed to find UniMob runtime assembly");
                return;
            }

            if (!File.Exists(uniMobRuntime.outputPath))
            {
                return;
            }

            var dependencyPaths = new HashSet<string>
            {
                Path.GetDirectoryName(assemblyPath)
            };

            var shouldWeave = false;

            foreach (var assembly in assemblies)
            {
                if (assembly.outputPath != assemblyPath) continue;

                foreach (var referencePath in assembly.compiledAssemblyReferences)
                {
                    dependencyPaths.Add(Path.GetDirectoryName(referencePath));
                }

                foreach (var reference in assembly.assemblyReferences)
                {
                    if (reference.outputPath == uniMobRuntime.outputPath)
                    {
                        shouldWeave = true;
                    }
                }

                break;
            }

            if (!shouldWeave)
            {
                return;
            }

            var sw = Stopwatch.StartNew();
            Weave(assemblyPath, dependencyPaths);
            sw.Stop();

            Debug.Log($"Weaved {assemblyPath} in {sw.ElapsedMilliseconds}ms");
        }

        public static void Weave(string assemblyPath, HashSet<string> dependencies)
        {
            using (var resolver = new DefaultAssemblyResolver())
            using (var assembly = AssemblyDefinition.ReadAssembly(assemblyPath,
                new ReaderParameters
                {
                    ReadWrite = true, ReadSymbols = true, AssemblyResolver = resolver
                }))
            {
                resolver.AddSearchDirectory(Path.GetDirectoryName(assemblyPath));
                resolver.AddSearchDirectory(Helpers.UnityEngineDllDirectoryName());
                resolver.AddSearchDirectory(Helpers.GetEngineCoreModuleDirectoryName());

                if (dependencies != null)
                {
                    foreach (var path in dependencies)
                    {
                        resolver.AddSearchDirectory(path);
                    }
                }

                var dirty = new AtomWeaver().Weave(assembly);
                if (dirty)
                {
                    assembly.Write(new WriterParameters {WriteSymbols = true});
                }
            }
        }
    }
}