using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;

namespace OmnIDE.Bridges
{
    public class PythonBridge : IDisposable
    {
        private static bool _initialized = false;
        private static readonly object _initLock = new object();

        public PythonBridge(string pythonPath)
        {
            InitializePython(pythonPath);
        }


        private void InitializePython(string pythonPath)
        {
            if (!_initialized)
            {
                lock (_initLock)
                {
                    if (!_initialized)
                    {
                        try
                        {
                            // Set Python DLL path before initializing
                            Runtime.PythonDLL = pythonPath;
                            PythonEngine.Initialize();
                            _initialized = true;
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException("Failed to initialize Python runtime.", ex);
                        }
                    }
                }
            }
        }

        private void AddPythonPath()
        {
            string pythonScriptsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python");
            string pythonLibPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "python-embed");

            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(pythonScriptsPath);
                sys.path.append(pythonLibPath);
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
