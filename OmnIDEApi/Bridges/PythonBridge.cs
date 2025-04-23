using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;

namespace OmnIDE.Bridges
{
    public class PythonBridge : IDisposable
    {
        private static bool _initialized = false;

        public PythonBridge()
        {
            InitializePython();
        }

        private void InitializePython()
        {
            if (!_initialized)
            {
                // Set Python Home to your Python installation
                Runtime.PythonDLL = @"C:\Python39\python39.dll"; // Adjust path to your Python installation
                PythonEngine.Initialize();
                _initialized = true;
            }
        }

        private void AddPythonPath()
        {
            string pythonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python");

            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(pythonPath);
            }
        }

        public bool CallDataProcessorExtractZip(string sourcePath, string destinationPath)
        {
            using (Py.GIL())
            {
                AddPythonPath();

                dynamic pyBridge = Py.Import("python_bridge");
                dynamic bridgeInstance = pyBridge.PythonBridge();

                dynamic result = bridgeInstance.call_data_processor_ExtractZip(sourcePath, destinationPath);

                return (bool)result;
            }
        }

        public Dictionary<string, object> CallCodeExecutor(List<string> files, Dictionary<string, object> metadata)
        {
            using (Py.GIL())
            {
                AddPythonPath();

                dynamic pyBridge = Py.Import("python_bridge");
                dynamic bridgeInstance = pyBridge.PythonBridge();

                dynamic result = bridgeInstance.call_code_executor(files.ToPython(), metadata.ToPython());

                return result.As<Dictionary<string, object>>();
            }
        }

        public Dictionary<string, object> CallReportGenerator(Dictionary<string, object> results)
        {
            using (Py.GIL())
            {
                AddPythonPath();

                dynamic pyBridge = Py.Import("python_bridge");
                dynamic bridgeInstance = pyBridge.PythonBridge();

                dynamic report = bridgeInstance.call_report_generator(results.ToPython());

                return report.As<Dictionary<string, object>>();
            }
        }

        public void Dispose()
        {
            if (_initialized)
            {
                PythonEngine.Shutdown();
                _initialized = false;
            }
        }
    }
}
