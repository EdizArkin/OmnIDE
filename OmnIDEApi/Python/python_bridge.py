from DataProcessor import DataProcessor
from CodeExecutor import CodeExecutor
from ReportGenerator import ReportGenerator

class PythonBridge:
    def __init__(self):
        self.data_processor = DataProcessor()
        self.code_executor = CodeExecutor()
        self.report_generator = ReportGenerator()

    def call_data_processor(self, files):
        return self.data_processor.process_zip(files)

    def call_code_executor(self, files, metadata):
        return self.code_executor.run_code(files, metadata)

    def call_report_generator(self, results):
        return self.report_generator.generate_report(results)
