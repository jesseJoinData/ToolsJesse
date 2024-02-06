import pandas as pd
import requests
import json

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
    
# Function to get data from API
def get_company_mappings(KVK, token):
    headers = {
        "Authorization": f"Bearer {token}",
        "Content-Type": "application/json"
    }
    # Modify the URL to use the provided KVK value
    url = f'https://production.join-data.net/api/company-mapping-service/v1/company-mapping?owner-scheme=nl.kvk&owner-value=27378529&target-scheme=nl.kvk&target-value={KVK}'
    response = requests.get(url, headers=headers)
    if response.status_code == 200:
        data = response.json()
        if isinstance(data, list):  # Check if the response is a list
            return data
        else:
            return []  # Return an empty list if the response is not a list
    else:
        return []  # Return an empty list in case of an unsuccessful response

# Function to update the last column of the DataFrame with JSON values
def update_csv_with_json_values(row):
    token = get_access_token()  # You should implement this function to get the access token
    
    # Ensure 'company key value' is 8 characters long or add leading zeros if needed
    company_key_value = str(row['company key value']).zfill(8)
    
    company_mapping = get_company_mappings(company_key_value, token)
    
    # Check if company_mapping is not empty
    if company_mapping:
        # Extract the "value" field from items with scheme "nl.ubn" in the response
        values = [item.get('value', '') for item in company_mapping if item.get('scheme') == 'nl.ubn']
        
        # Combine the values into a comma-separated string
        return ', '.join(values)
    else:
        return ''

# Replace 'your_csv_file.csv' with the actual path to your CSV file
input_csv_file = 'ToolsJesse\\checkubnscript\\Machtigingen Agrifirm.csv'
output_csv_file = 'ToolsJesse\\checkubnscript\\Machtigingen Agrifirm Updated.csv'

df = pd.read_csv(input_csv_file)

# Apply the function to each row of the DataFrame and create a new column 'json_response'
df['json_response'] = df.apply(update_csv_with_json_values, axis=1)

# Save the updated DataFrame to a new CSV file
df.to_csv(output_csv_file, index=False)

print(f"New CSV file '{output_csv_file}' created with JSON values in the last column for 'nl.ubn' scheme and 8-character 'company key value' with leading zeros added if needed.")