import sys
import io

class CodeExecutor:
    def __init__(self, code: str):
        self.code = code

    def execute(self):
        output = io.StringIO()
        sys.stdout = output
        sys.stderr = output
        try:
            exec(self.code)
        except Exception as e:
            return f"An error occurred: {e}"
        finally:
            sys.stdout = sys.__stdout__
            sys.stderr = sys.__stderr__
        return output.getvalue()