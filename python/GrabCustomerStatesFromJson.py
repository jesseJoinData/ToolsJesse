import json
import csv

# Step 1: Read the CSV file with a semicolon delimiter and get the required customer IDs
# customer_ids = set()
# with open('your_file.csv', 'r') as csvfile:
#     csvreader = csv.reader(csvfile, delimiter=';')
#     for row in csvreader:
#         customer_ids.add(row[0])  # Assuming the IDs are in the first column

# Step 2: Read the JSON file
with open('new.json', 'r') as jsonfile:
    data = json.load(jsonfile)

# Step 3: Filter the data
filtered_data = []
for state in data["customerStates"]:
    # Check if providingCompanyCustomerId does not start with '000' and is in customer_ids
    if not(state["providingCompanyCustomerId"].startswith('000')) and not(len(state["providingCompanyCustomerId"]) < 6):
        filtered_data.append(state)

# Step 4: Write the filtered data to a new CSV file using a semicolon delimiter, excluding 'stateDetails'
if filtered_data:
    with open('filtered_data.csv', 'w', newline='') as csvfile:
        # Exclude 'stateDetails' from field names
        fieldnames = [key for key in filtered_data[0].keys() if key != 'stateDetails']
        writer = csv.DictWriter(csvfile, fieldnames=fieldnames, delimiter=',')

        writer.writeheader()
        for item in filtered_data:
            # Remove 'stateDetails' from each item before writing
            item.pop('stateDetails', None)
            writer.writerow(item)
else:
    print("No matching data found to write to CSV.")
