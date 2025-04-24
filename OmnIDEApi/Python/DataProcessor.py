import zipfile
import pandas as pd
from pathlib import Path

class DataProcessor:

    def extractZip(self, raw_folder_path, raw_extract_to):
        """Extracts all .zip files in the given folder to the specified directory.
        Returns True if at least one file is extracted successfully, otherwise False."""
        
        try:
            folder_path = Path(raw_folder_path).expanduser().resolve()
            extract_to = Path(raw_extract_to).expanduser().resolve()
            extract_to.mkdir(parents=True, exist_ok=True)

            extracted_any = False

            for zip_file in folder_path.glob("*.zip"):
                with zipfile.ZipFile(zip_file, 'r') as zipObj:
                    target_folder = extract_to / zip_file.stem
                    target_folder.mkdir(parents=True, exist_ok=True)
                    zipObj.extractall(target_folder)
                    print(f"Extracted {zip_file} to {target_folder}")
                    extracted_any = True

            return extracted_any
        
        except Exception as e:
            print(f"Hata oluştu: {e}")
            return False



