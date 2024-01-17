using Microsoft.VisualBasic.FileIO;

namespace CheckEabMessageFlow1_0.Services;
public class CSVFileService
{
    private string filePath = "C:\\Users\\jesse\\OneDrive\\Documents\\ToolsJesse\\CheckEabMessageFlow\\companies\\companies-2023-11-28-10-34-13.csv";
    public List<Companies> LoadCompaniesFromFile()
    {
        List<Companies> companiesList = new List<Companies>();

        if (File.Exists(filePath))
        {

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                // Skip header if your CSV file has one
                if (!parser.EndOfData)
                {
                    parser.ReadFields();
                }
                int i = 0;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields()!;
                    if (fields.Length >= 2)
                    {
                        Companies company = new Companies
                        {
                            ID = i++,
                            CompanyName = fields[0],
                            KvkNumber = fields[1],
                            ClientID = fields[2],
                            ClientSecret = fields[3]
                        };

                        companiesList.Add(company);
                    }
                }
            }
        }
        else
        {
            MessageBox.Show($"File for companies not found {filePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return companiesList;
    }
}