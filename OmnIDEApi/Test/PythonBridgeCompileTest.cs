using System;
using System.Collections.Generic;
using System.IO;
using OmnIDE.Bridges;
using Python.Runtime;

namespace OmnIDEApi.Test
{
    public class PythonBridgeCompileTest
    {
        public bool TestCompileDirectory()
        {
            try
            {
                // Get the project root directory
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string pythonPath = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "Python", "python-embed", "python39.dll"));

                if (!File.Exists(pythonPath))
                {
                    Console.WriteLine($"Python DLL not found at: {pythonPath}");
                    Console.WriteLine("Please ensure Python 3.9 embedded distribution is installed in the correct location.");
                    return false;
                }

                // Check if Python is already initialized
                if (PythonEngine.IsInitialized)
                {
                    Console.WriteLine("PythonEngine is already initialized. Using the existing Python runtime.");
                }
                else
                {
                    // Set Python DLL path
                    Runtime.PythonDLL = pythonPath;
                }

                // Create bridge after setting DLL path
                var bridge = new PythonBridge(pythonPath);

                string compileDirectory = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "CompileTestFiles"));

                Console.WriteLine($"Using Python DLL from: {pythonPath}");
                Console.WriteLine($"Compile directory path: {compileDirectory}");

                Directory.CreateDirectory(compileDirectory);

                Console.WriteLine("Listing files in compile directory:");
                foreach (var file in Directory.GetFiles(compileDirectory))
                {
                    Console.WriteLine($"Found file: {file}");
                }

                try
                {
                    Dictionary<string, string> results = bridge.CallCodeExecutorCompileDirectory(compileDirectory);

                    if (results.Count == 0)
                    {
                        Console.WriteLine("No results returned from CallCodeExecutorCompileDirectory.");
                    }

                    foreach (var result in results)
                    {
                        Console.WriteLine($"File: {result.Key}, Result: {result.Value}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during CallCodeExecutorCompileDirectory execution: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");

                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                        Console.WriteLine($"Inner stack trace: {ex.InnerException.StackTrace}");
                    }

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test sırasında hata oluştu: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }
    }
}
