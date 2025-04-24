class ReportGenerator:
    def __init__(self, report):
        self.report = report

    def generate_report(self):
        # Placeholder for report generation logic
        return f"Report: {self.report}"
    
    def save_report(self, filename):
        with open(filename, 'w') as file:
            file.write(self.generate_report())
        print(f"Report saved to {filename}")