using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Rainmeter;

namespace PluginExportVars
{
    internal class Measure
    {
        private API _api;  // Store the API instance

        public string SourceFile { get; set; }
        public string DestinationFile { get; set; }
        public string OnCompleteAction { get; set; }

        public Measure()
        {
            SourceFile = string.Empty;
            DestinationFile = string.Empty;
            OnCompleteAction = string.Empty;
        }

        public void Reload(API api, ref double maxValue)
        {
            _api = api;  // Initialize the API instance
            SourceFile = _api.ReadString("SourceFile", "");
            DestinationFile = _api.ReadString("DestinationFile", "");
            OnCompleteAction = _api.ReadString("OnCompleteAction", "");
        }

        public void Execute()
        {
            if (!File.Exists(SourceFile) || !File.Exists(DestinationFile))
            {
                _api.Log(API.LogType.Error, "ExportVars.dll: Source or Destination file not found.");
                return;
            }

            try
            {
                // Read and parse the source file variables
                var sourceLines = File.ReadAllLines(SourceFile);
                var sourceVariables = sourceLines
                    .Where(line => line.Contains('='))
                    .ToDictionary(
                        line => line.Split('=')[0].Trim(),
                        line => line.Split('=')[1].Trim()
                    );

                // Read the destination file and find the existing variables
                var destLines = File.ReadAllLines(DestinationFile).ToList();
                var destVariables = destLines
                    .Where(line => line.Contains('='))
                    .ToDictionary(
                        line => line.Split('=')[0].Trim(),
                        line => line.Split('=')[1].Trim()
                    );

                // Replace existing variables in the destination file with source variables
                for (int i = 0; i < destLines.Count; i++)
                {
                    string line = destLines[i];
                    if (line.Contains('='))
                    {
                        var key = line.Split('=')[0].Trim();
                        if (sourceVariables.ContainsKey(key))
                        {
                            destLines[i] = $"{key}={sourceVariables[key]}";
                        }
                    }
                }

                // Add missing variables from source to destination file
                foreach (var sourceVar in sourceVariables)
                {
                    if (!destVariables.ContainsKey(sourceVar.Key))
                    {
                        destLines.Add($"{sourceVar.Key}={sourceVar.Value}");
                    }
                }

                // Write the updated lines back to the destination file
                File.WriteAllLines(DestinationFile, destLines);

                // Execute OnCompleteAction if set
                if (!string.IsNullOrEmpty(OnCompleteAction))
                {
                    _api.Execute(OnCompleteAction);
                }

                _api.Log(API.LogType.Debug, "ExportVars.dll: Variables successfully exported.");
            }
            catch (Exception ex)
            {
                _api.Log(API.LogType.Error, $"ExportVars.dll: Error exporting variables - {ex.Message}");
            }
        }
    }

    public static class Plugin
    {
        [DllExport]
        public static void Initialize(ref IntPtr data, IntPtr rm)
        {
            data = GCHandle.ToIntPtr(GCHandle.Alloc(new Measure()));
        }

        [DllExport]
        public static void Finalize(IntPtr data)
        {
            GCHandle.FromIntPtr(data).Free();
        }
         
        [DllExport]
        public static void Reload(IntPtr data, IntPtr rm, ref double maxValue)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            measure.Reload(new API(rm), ref maxValue);
        }

        [DllExport]
        public static double Update(IntPtr data)
        {
            return 0.0;
        }

        [DllExport]
        public static void ExecuteBang(IntPtr data, [MarshalAs(UnmanagedType.LPWStr)] string args)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            measure.Execute();
        }
    }
}
