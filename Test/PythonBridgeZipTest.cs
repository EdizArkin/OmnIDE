using System;
using OmnIDE.Bridges;

namespace OmnIDE.Test
{
    public class PythonBridgeZipTest
    {
        public bool TestExtractZip()
        {
            try
            {
                var bridge = new PythonBridge();

                string zipFolderPath = @"C:\Users\Ediz Arkın Kobak\OneDrive\Masaüstü\zipFiles";
                string extractToPath = @"C:\Users\Ediz Arkın Kobak\OneDrive\Masaüstü\ExtractedZips";

                bool result = bridge.CallDataProcessorExtractZip(zipFolderPath, extractToPath);

                Console.WriteLine($"ExtractZip çalıştı mı? Sonuç: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test sırasında hata oluştu: {ex.Message}");
                return false;
            }
        }
    }
}
