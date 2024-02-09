# https://production.join-data.net/api/company-mapping-service/v1/company-mapping?owner-scheme=nl.kvk&owner-value={0}&target-scheme=nl.kvk&target-value={1}
import pandas as pd
import requests
import os
import json

# Load your dataset
df = pd.read_csv('C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\getAllMapping\\allCompanies06022024.csv', encoding='ISO-8859-1')

df = df[pd.notna(df['Nationaal identificatienummer'])]
# Specify the folder where you want to save the responses
folder_path = 'C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\getAllMapping\\Responses'
os.makedirs(folder_path, exist_ok=True)

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
    
def get_data_from_api(KVK, naam, token):
    headers = {
    "Authorization": f"Bearer {token}",
    "Content-Type": "application/json"  # Add if required by the API
    }
    url = f'https://production.join-data.net/api/company-mapping-service/v1/company-mapping?owner-scheme=nl.kvk&owner-value={KVK}'
    response = requests.get(url, headers=headers)
    if response.status_code == 200:
        response_data = response.json()
        # Sanitize the naam value to ensure it's a valid filename
        safe_naam = "".join([c for c in naam if c.isalnum() or c in [" ", "_", "-"]]).rstrip()
        file_path = os.path.join(folder_path, f'{safe_naam}.json')
        with open(file_path, 'w') as f:
            json.dump(response_data, f)
    else:
        print(f"Failed to retrieve data for KVK: {KVK} with naam: {naam}")

# Get the access token
token = get_access_token()
if token:
    # Iterate through each row in the DataFrame
    for index, row in df.iterrows():
        if index == 100:
            # refresh token
            token = get_access_token()
        
        KVK = row['Nationaal identificatienummer']  # Replace 'YourKVKColumnName' with the actual column name for KVK numbers
        naam = row['Naam']  # Assuming 'naam' is the column name for the names you want to use as filenames
        if pd.notna(KVK) and KVK:  # This checks both for NaN/None and empty strings
            get_data_from_api(KVK, naam, token)
            print(f"getting {naam}.")
        else:
            print(f"Skipping row {index} due to empty KVK value.")
else:
    print("Cannot proceed without access token.")

