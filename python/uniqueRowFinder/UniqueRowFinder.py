import pandas as pd

# Load the CSV files into Pandas DataFrames with the specified encoding if needed
df_a = pd.read_csv('C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\uniqueRowFinder\\file_a.csv', encoding='ISO-8859-1')
df_b = pd.read_csv('C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\uniqueRowFinder\\file_b.csv', encoding='ISO-8859-1')

# Perform an outer join and use the indicator=True to add a column _merge that indicates whether the row is from both or just one DataFrame
merged_df = pd.merge(df_a, df_b, how='outer', indicator=True)

# Filter rows that are only found in df_a (left_only)
unique_to_a = merged_df[merged_df['_merge'] == 'left_only'].drop('_merge', axis=1)

# Save the unique rows to a new CSV file
unique_to_a.to_csv('C:\\Users\\jesse\\Documents\\Tools\\ToolsJesse\\python\\uniqueRowFinder\\file_c.csv', index=False, encoding='ISO-8859-1')
