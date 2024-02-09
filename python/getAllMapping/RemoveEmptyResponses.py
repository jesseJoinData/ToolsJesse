import os
import json

folder_path = 'C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\getAllMapping\\Responses'

def remove_empty_json_files(folder_path):
    for filename in os.listdir(folder_path):
        if filename.endswith('.json'):
            file_path = os.path.join(folder_path, filename)
            # Attempt to determine if the file is empty or invalid and should be deleted
            try:
                with open(file_path, 'r') as file:
                    data = json.load(file)
                # The file has been closed at this point
                if not data:  # If the file is empty, data will be False
                    os.remove(file_path)
                    print(f'Removed empty file: {filename}')
            except json.JSONDecodeError:
                # Handles cases where the file might be empty or badly formatted
                os.remove(file_path)
                print(f'Removed invalid or empty JSON file: {filename}')
            except OSError as e:
                print(f'Error removing {filename}: {e}')
            except Exception as e:
                print(f'Unexpected error with {filename}: {e}')

remove_empty_json_files(folder_path)
