import os
import json
import csv
import requests
import time

input_folder_path = 'C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\getAllMapping\\Responses'  # Update this to your folder path
output_folder_path = 'C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\getAllMapping\\NoSourceLocations'

def get_access_token():
    url = "https://production.join-data.net/auth/realms/datahub/protocol/openid-connect/token"
    data = {
        "client_id": "source-registry",
        "client_secret": "4029e2b9-d74b-4147-9086-df606c817fbf",
        "grant_type": "client_credentials"
    }
    response = requests.post(url, data=data)
    if response.status_code == 200:
        return response.json().get("access_token")
    else:
        return None
    
def make_api_request(owner_value, target_value, token):
    headers = {
        "Authorization": f"Bearer {token}",
        "Content-Type": "application/json",
        "Accept": "application/json"
    }
    url = f'https://production.join-data.net/api/source-registry/v2/locations/nl.kvk/{target_value}/sources?provider-company-value={owner_value}'
    response = requests.get(url, headers=headers)
    if response.status_code == 200:
        response_data = response.json()
        # Check if the response's "data" key is empty
        if not response_data.get('data'):
            return None, True  # Indicating the response is empty
        return response_data, False
    else:
        print(f"API request failed for Owner: {owner_value}, Target: {target_value} with status code {response.status_code}")
        return None, False

def save_to_csv_with_headers(csv_file_path, owner_value, target_value):
    # Check if the file exists to determine if we need to write headers
    file_exists = os.path.isfile(csv_file_path)
    
    with open(csv_file_path, 'a', newline='') as csvfile:
        fieldnames = ['OwnerValue', 'TargetValue']
        writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
        
        # Write header only if the file did not exist
        if not file_exists:
            writer.writeheader()
        
        writer.writerow({'OwnerValue': owner_value, 'TargetValue': target_value})

def process_json_files(input_folder_path, output_folder_path):
    for filename in os.listdir(input_folder_path):
        if filename.endswith('.json'):
            file_path = os.path.join(input_folder_path, filename)
            with open(file_path, 'r') as file:
                data = json.load(file)
                for item in data:
                    time.sleep(0.1)
                    owner_value = item['ownerCompany']['value']
                    target_value = item['targetCompany']['value']
                    print(f"Getting sources for target {target_value} and owner {owner_value}.")
                    token = get_access_token()
                    response, isEmpty = make_api_request(owner_value, target_value, token)
                    if isEmpty:
                        # Ensure the output folder exists
                        if not os.path.exists(output_folder_path):
                            os.makedirs(output_folder_path)
                        # Save to a different folder
                        csv_filename = os.path.splitext(filename)[0] + '.csv'
                        csv_file_path = os.path.join(output_folder_path, csv_filename)
                        save_to_csv_with_headers(csv_file_path, owner_value, target_value)
                        print(f"Saved targetCompanyValue {target_value} to {csv_filename} in output folder.")

# Example token for API authentication
token = get_access_token()  # Replace with your actual token

process_json_files(input_folder_path, output_folder_path)