import pandas as pd
import requests

def get_access_token():
    url = "https://production.join-data.net/auth/realms/datahub/protocol/openid-connect/token"
    data = {
        "client_id": "",
        "client_secret": "",
        "grant_type": "client_credentials"
    }
    response = requests.post(url, data=data)
    if response.status_code == 200:
        return response.json().get("access_token")
    else:
        return None
    
# Function to get data from API
def get_data_from_api(UBN, token):
    headers = {
        "Authorization": f"Bearer {token}",
        "Content-Type": "application/json"  # Add if required by the API
    }
    url = f'https://production.join-data.net/api/company-mapping-service/v1/company-mapping?scheme=nl.ubn&value={UBN}'
    response = requests.get(url, headers=headers)
    if response.status_code == 200:
        data = response.json()
        if isinstance(data, list) and len(data) > 0:
            first_item = data[0]
            owner_company_scheme = first_item.get('ownerCompany', {}).get('scheme', 'N/A')
            owner_company_value = first_item.get('ownerCompany', {}).get('value', 'N/A')
            target_company_scheme = first_item.get('targetCompany', {}).get('scheme', 'N/A')
            target_company_value = first_item.get('targetCompany', {}).get('value', 'N/A')
            return owner_company_scheme, owner_company_value, target_company_scheme, target_company_value
        else:
            return 'N/A', 'N/A', 'N/A', 'N/A'
    else:
        print(f"Error fetching data for UBN {UBN}: {response.status_code} - {response.text}")
        return 'Error', 'Error', 'Error', 'Error'

token = get_access_token()
if token is None:
    raise Exception("Failed to get access token")

# Load CSV file
csv_file = "ToolsJesse\\checkubnscript\\kvkubn.csv"
df = pd.read_csv(csv_file)

# Add new columns for the API response
df['owner_company_scheme'] = None
df['owner_company_value'] = None
df['target_company_scheme'] = None
df['target_company_value'] = None

# Process each row
i = 0
for index, row in df.iterrows():
    ocs, ocv, tcs, tcv = get_data_from_api(row['UBN'], token)
    print(i)
    i = i + 1
    df.at[index, 'owner_company_scheme'] = ocs
    df.at[index, 'owner_company_value'] = ocv
    df.at[index, 'target_company_scheme'] = tcs
    df.at[index, 'target_company_value'] = tcv

# Save the updated dataframe to a new CSV file
df.to_csv('ToolsJesse\\checkubnscript\\updated_file.csv', index=False)
