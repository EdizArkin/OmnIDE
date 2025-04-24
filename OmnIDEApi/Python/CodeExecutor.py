import subprocess
import os
import glob
import sys
import io

class CodeExecutor:
    def __init__(self, code: str = ""):
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

    def compile_and_run(self, file_path):
        _, ext = os.path.splitext(file_path)

        try:
            if ext == ".c":
                output_binary = os.path.splitext(file_path)[0]
                subprocess.run(["gcc", file_path, "-o", output_binary], check=True)
                result = subprocess.run([f"./{output_binary}"], check=True, stdout=subprocess.PIPE, text=True)
                return result.stdout.strip()
            elif ext == ".cpp":
                output_binary = os.path.splitext(file_path)[0]
                subprocess.run(["g++", file_path, "-o", output_binary], check=True)
                result = subprocess.run([f"./{output_binary}"], check=True, stdout=subprocess.PIPE, text=True)
                return result.stdout.strip()
            elif ext == ".java":
                class_name = os.path.splitext(os.path.basename(file_path))[0]
                subprocess.run(["javac", file_path], check=True)
                result = subprocess.run(["java", class_name], check=True, stdout=subprocess.PIPE, text=True)
                return result.stdout.strip()
            elif ext == ".cs":
                result = subprocess.run(["dotnet", "run", "--source", file_path], check=True, stdout=subprocess.PIPE, text=True)
                return result.stdout.strip()
            elif ext == ".py":
                result = subprocess.run(["python3", file_path], check=True, stdout=subprocess.PIPE, text=True)
                return result.stdout.strip()
            elif ext == ".go":
                result = subprocess.run(["go", "run", file_path], check=True, stdout=subprocess.PIPE, text=True)
                return result.stdout.strip()
            elif ext == ".js":
                result = subprocess.run(["node", file_path], check=True, stdout=subprocess.PIPE, text=True)
                return result.stdout.strip()
            else:
                return f"Unsupported file type: {ext}"
        except subprocess.CalledProcessError as e:
            return f"Error for {file_path}: {e}"

    def process_Compile_directory(self, directory):
        try:
            os.chdir(directory)
        except FileNotFoundError:
            return f"Error: Directory {directory} not found."

        files = glob.glob("*.c") + glob.glob("*.cpp") + glob.glob("*.java") + glob.glob("*.cs") + glob.glob("*.py") + glob.glob("*.js") + glob.glob("*.go")
        results = {}

        if not files:
            print(f"No supported files found in directory: {directory}")

        for file in files:
            print(f"Processing file: {file}")
            results[file] = self.compile_and_run(file)
            print(f"Result for {file}: {results[file]}")

        return results