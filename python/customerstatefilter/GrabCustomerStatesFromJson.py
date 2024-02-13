import json
import csv

# Step 1: Read the CSV file with a semicolon delimiter and get the required customer IDs
customer_ids_batch1 = set()
customer_ids_batch2 = set()
customer_ids_batch3 = set()
with open('C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\customerstatefilter\\Batches2.csv', 'r') as csvfile:
    csvreader = csv.reader(csvfile, delimiter=',')
    for row in csvreader:
        customer_ids_batch1.add(row[0])  # Assuming the IDs are in the first column
        customer_ids_batch2.add(row[1])
        customer_ids_batch3.add(row[2])

# Step 2: Read the JSON file
with open('C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\customerstatefilter\\response.json', 'r') as jsonfile:
    data = json.load(jsonfile)

# Step 3: Filter the data
filtered_data_batch1 = []
filtered_data_batch2 = []
filtered_data_batch3 = []

for state in data["customerStates"]:
    # Check if providingCompanyCustomerId does not start with '000' and is in customer_ids
    if (state["providingCompanyCustomerId"] in customer_ids_batch1):
        filtered_data_batch1.append(state)
    elif (state["providingCompanyCustomerId"] in customer_ids_batch2):
        filtered_data_batch2.append(state)
    elif (state["providingCompanyCustomerId"] in customer_ids_batch3):
        filtered_data_batch3.append(state)

# Step 4: Write the filtered data to a new CSV file using a semicolon delimiter, excluding 'stateDetails'
if filtered_data_batch1:
    with open('C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\customerstatefilter\\filtered_data_batch1.csv', 'w', newline='') as csvfile:
        # Exclude 'stateDetails' from field names
        fieldnames = [key for key in filtered_data_batch1[0].keys() if key != 'stateDetails']
        writer = csv.DictWriter(csvfile, fieldnames=fieldnames, delimiter=',')

        writer.writeheader()
        for item in filtered_data_batch1:
            # Remove 'stateDetails' from each item before writing
            item.pop('stateDetails', None)
            writer.writerow(item)
else:
    print("No data found for batch1")

if filtered_data_batch2:
    with open('C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\customerstatefilter\\filtered_data_batch2.csv', 'w', newline='') as csvfile:
        # Exclude 'stateDetails' from field names
        fieldnames = [key for key in filtered_data_batch2[0].keys() if key != 'stateDetails']
        writer = csv.DictWriter(csvfile, fieldnames=fieldnames, delimiter=',')

        writer.writeheader()
        for item in filtered_data_batch2:
            # Remove 'stateDetails' from each item before writing
            item.pop('stateDetails', None)
            writer.writerow(item)
else:
    print("No data found for batch2")

if filtered_data_batch3:
    with open('C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\customerstatefilter\\filtered_data_batch3.csv', 'w', newline='') as csvfile:
        # Exclude 'stateDetails' from field names
        fieldnames = [key for key in filtered_data_batch3[0].keys() if key != 'stateDetails']
        writer = csv.DictWriter(csvfile, fieldnames=fieldnames, delimiter=',')

        writer.writeheader()
        for item in filtered_data_batch3:
            # Remove 'stateDetails' from each item before writing
            item.pop('stateDetails', None)
            writer.writerow(item)
else:
    print("No data found for batch3")

filtered_batch1_ids = set()
filtered_batch2_ids = set()
filtered_batch3_ids = set()

with open('C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\customerstatefilter\\filtered_data_batch1.csv', 'r') as csvfile:
    csvreader = csv.reader(csvfile, delimiter=',')
    for row in csvreader:
        filtered_batch1_ids.add(row[2])

with open('C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\customerstatefilter\\filtered_data_batch2.csv', 'r') as csvfile:
    csvreader = csv.reader(csvfile, delimiter=',')
    for row in csvreader:
        filtered_batch2_ids.add(row[2])

with open('C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\customerstatefilter\\filtered_data_batch3.csv', 'r') as csvfile:
    csvreader = csv.reader(csvfile, delimiter=',')
    for row in csvreader:
        filtered_batch3_ids.add(row[2])

not_in_companyMappingBatch1 = customer_ids_batch1 - filtered_batch1_ids
not_in_companyMappingBatch2 = customer_ids_batch2 - filtered_batch2_ids
not_in_companyMappingBatch3 = customer_ids_batch3 - filtered_batch3_ids

list_not_in_companyMappingBatch1 = list(not_in_companyMappingBatch1)
list_not_in_companyMappingBatch2 = list(not_in_companyMappingBatch2)
list_not_in_companyMappingBatch3 = list(not_in_companyMappingBatch3)

max_length = max(len(list_not_in_companyMappingBatch1), len(list_not_in_companyMappingBatch2), len(list_not_in_companyMappingBatch3))

# Normalize the length of the lists by filling with None for missing elements
list_not_in_companyMappingBatch1 += [None] * (max_length - len(list_not_in_companyMappingBatch1))
list_not_in_companyMappingBatch2 += [None] * (max_length - len(list_not_in_companyMappingBatch2))
list_not_in_companyMappingBatch3 += [None] * (max_length - len(list_not_in_companyMappingBatch3))

with open('C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\customerstatefilter\\not_in_companyMapping.csv', mode='w', newline='') as file:
        writer = csv.writer(file)
        # Writing a header (optional)
        writer.writerow(['Batch1', 'Batch2', 'Batch3'])
        # Writing the elements of each set in different columns
        for a, b, c in zip(list_not_in_companyMappingBatch1, list_not_in_companyMappingBatch2, list_not_in_companyMappingBatch3):
            writer.writerow([a, b, c])

        print(f'Sets saved to C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\customerstatefilter\\not_in_companyMapping.csv')