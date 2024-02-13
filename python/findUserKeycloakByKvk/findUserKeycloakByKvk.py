import pandas as pd
import os

# Load CSV files
csv_file_a = 'C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\findUserKeycloakByKvk\\CSVfileA.csv'  # Update with the actual path
csv_file_b = 'C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\findUserKeycloakByKvk\\CSVfileB.csv'  # Update with the actual path
output_csv_file_c = 'C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\findUserKeycloakByKvk\\CSVoutputfileC.csv'  # Output path
not_found_csv_file = 'C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\findUserKeycloakByKvk\\notfound.csv'
# Ensure the directory for the output file exists
for file_path in [output_csv_file_c, not_found_csv_file]:
    output_dir = os.path.dirname(file_path)
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)

# Assuming we're checking if values in 'column_in_a' of df_a are in 'column_in_b' of df_b
column_in_a = 'kvk'  # Update with the actual column name
column_in_b = 'company_value'  # Update with the actual column name

dtype_dict = {column_in_a: str, column_in_b: str}

df_a = pd.read_csv(csv_file_a, dtype=dtype_dict)
df_b = pd.read_csv(csv_file_b, dtype=dtype_dict)

# Find matching values
filtered_df_b = df_b[df_b[column_in_b].isin(df_a[column_in_a].astype(str))]

# Save the filtered df_b to CSV file C
filtered_df_b.to_csv(output_csv_file_c, index=False)
# Identify values in df_a not found in df_b and save to a separate file
not_found_df_a = df_a[~df_a[column_in_a].astype(str).isin(df_b[column_in_b])]
not_found_df_a.to_csv(not_found_csv_file, index=False)

print(f"Rows from B where {column_in_b} matches values in {column_in_a} of A saved to {output_csv_file_c}")
print(f"Values from A not found in B saved to {not_found_csv_file}")
