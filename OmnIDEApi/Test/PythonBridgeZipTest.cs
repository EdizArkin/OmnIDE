using System;
using System.IO;
using OmnIDE.Bridges;

namespace OmnIDEApi.Test
{
    public class PythonBridgeZipTest
    {
        public bool TestExtractZip()
        {
            try
            {
                // Check Python installation first
                string pythonPath = Path.Combine("..", "python-embed", "python39.dll");
                if (!File.Exists(pythonPath))
                {
                    Console.WriteLine($"Python DLL not found at: {pythonPath}");
                    Console.WriteLine("Please ensure Python 3.9 embedded distribution is installed in the correct location.");
                    return false;
                }

                var bridge = new PythonBridge();

                string baseDir = AppDomain.CurrentDomain.BaseDirectory 
                    ?? throw new InvalidOperationException("Base directory cannot be null");

                string zipFolderPath = Path.GetFullPath(Path.Combine(baseDir, "..", "Zip"));
                string extractToPath = Path.GetFullPath(Path.Combine(baseDir, "..", "TargetFolder"));

                Console.WriteLine($"Using Python DLL from: {pythonPath}");
                Console.WriteLine($"Zip folder path: {zipFolderPath}");
                Console.WriteLine($"Extract path: {extractToPath}");

                Directory.CreateDirectory(zipFolderPath);
                Directory.CreateDirectory(extractToPath);

                bool result = bridge.CallDataProcessorExtractZip(zipFolderPath, extractToPath);
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
