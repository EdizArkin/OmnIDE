using System;
using System.IO;
using OmnIDE.Bridges;
using Python.Runtime;

namespace OmnIDEApi.Test
{
    public class PythonBridgeZipTest
    {
        public bool TestExtractZip()
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

                string zipFolderPath = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "TestFiles"));
                string extractToPath = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "CompileTestFiles"));

                Console.WriteLine($"Using Python DLL from: {pythonPath}");
                Console.WriteLine($"Zip folder path: {zipFolderPath}");
                Console.WriteLine($"Extract path: {extractToPath}");

                Directory.CreateDirectory(zipFolderPath);
                Directory.CreateDirectory(extractToPath);

                bool result = bridge.CallDataProcessorExtractZip(zipFolderPath, extractToPath);
                if (result)
                {
                    Console.WriteLine("Zip extracted successfully.");
                }
                else
                {
                    Console.WriteLine("Zip could not be extracted.");
                }
                return result;
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
