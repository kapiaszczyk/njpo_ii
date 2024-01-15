using System.Collections;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using System.Text;

class CatalogGeneric<T>(IWriter<T> writer, IReader<T> reader)
{

    private List<T> catalog = [];
    private readonly IReader<T> reader = reader;
    private readonly IWriter<T> writer = writer;

    // adding
    public void Add(T item)
    {
        catalog.Add(item);
    }

    // removing
    public void Remove(T item)
    {
        catalog.Remove(item);
    }

    // printing
    public void Print()
    {
        if (catalog.Count == 0)
        {
            Console.WriteLine("Catalog is empty");
        }
        else
        {
            foreach (T item in catalog)
            {
                Console.WriteLine(item?.ToString());
            }
        }
    }

    // searching
    public T? Search(T catalogItem)
    {
        if (catalog.Count != 0 && catalog.Contains(catalogItem))
        {
            return catalogItem;
        }
        else
        {
            return default(T);
        }
    }

    // validating
    public bool Validate(T item)
    {
        return catalog.Contains(item);
    }

    // writing
    public void Write()
    {
        writer.Write(catalog);
    }

    // reading
    public void Read()
    {
        catalog = reader.Read();
    }


}

public class Employee
{

    // Instance variables 
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Position { get; set; }
    public string Email { get; set; }
    public int Id { get; set; }

    // Parameterless constructor for XML serialization
    public Employee()
    {
    }

    public Employee(string name, string surname, string position, string email, int id)
    {
        Name = name;
        Surname = surname;
        Position = position;
        Email = email;
        Id = id;
    }


    // Validate data
    public bool ValidateData()
    {
        EmployeeValidator validator = new();
        return EmployeeValidator.ValidateAll(this.Name, this.Surname, this.Position, this.Email, this.Id);
    }

    // Show()
    public String Show()
    {
        return string.Format("Name: {0}, Surname: {1}, Position: {2}, Email: {3},  Id: {4}", this.Name, this.Surname, this.Position, this.Email, this.Id);
    }

    // IsMatch()
    public bool IsMatch(Employee employee)
    {
        return (
            this.Name == employee.Name &&
            this.Surname == employee.Surname &&
            this.Position == employee.Position &&
            this.Email == employee.Email &&
            this.Id == employee.Id
        );
    }

    // ToString()
    public override string ToString()
    {
        return string.Format("Name: {0}, Surname: {1}, Position: {2}, Email: {3}, Id: {4}", this.Name, this.Surname, this.Position, this.Email, this.Id);
    }
}

partial class EmployeeValidator
{

    const int MIN_NAME_LENGTH = 2;
    const int MAX_NAME_LENGTH = 20;
    const int MIN_POSITION_LENGTH = 2;
    const int MAX_POSITION_LENGTH = 20;
    const int MIN_ID = 0;
    const String NAME_REGEX = @"^[a-zA-Z]+$";
    const String EMAIL_REGEX = "^[A-Za-z0-9.]+@(.+)$";  // Regex pattern : any number of letters, numbers and dots, then @, 
                                                        // then any number of letters, numbers and dots and $ asserting the end of the string

    // validate name and surname
    private static bool ValidateName(String name)
    {
        return !String.IsNullOrEmpty(name) && name.Length > MIN_NAME_LENGTH && name.Length < MAX_NAME_LENGTH && Regex.IsMatch(name, NAME_REGEX);
    }

    // validate email address against regex
    private static bool ValidateEmail(String email)
    {
        return Regex.IsMatch(email, EMAIL_REGEX);
    }

    private static bool ValidatePosition(String position)
    {
        return !String.IsNullOrEmpty(position) && position.Length > MIN_POSITION_LENGTH && position.Length < MAX_POSITION_LENGTH && Regex.IsMatch(position, NAME_REGEX);
    }

    private static bool ValidateId(int id)
    {
        return id > MIN_ID;
    }

    public static bool ValidateAll(String name, String surname, String position, String email, int id)
    {
        return (
            ValidateName(name) &&
            ValidateName(surname) &&
            ValidatePosition(position) &&
            ValidateEmail(email) &&
            ValidateId(id)
            );
    }
}

public interface IReader<T>
{
    List<T> Read();
}

public class JSONReader<T> : IReader<T>
{

    private CaesarCipher cipher = new();

    public List<T> Read()
    {
        if (File.Exists("employee_catalog.json"))
        {
            string encryptedJsonString = File.ReadAllText("employee_catalog.json");
            string decryptedJsonString = cipher.Decrypt(encryptedJsonString);
            List<T>? deserializedList = JsonSerializer.Deserialize<List<T>>(decryptedJsonString);
            // if deserializedList is null, return empty list
            return deserializedList ?? [];
        }
        else
        {
            return [];
        }

    }
}

public class XMLReader<T> : IReader<T>
{

    private CaesarCipher cipher = new();

    public List<T> Read()
    {
        if (File.Exists("employee_catalog.xml"))
        {
            XmlSerializer serializer = new(typeof(List<T>));

            using FileStream fileStream = File.OpenRead("employee_catalog.xml");
            using StreamReader reader = new(fileStream);
            string encryptedXmlString = reader.ReadToEnd();
            string decryptedXmlString = cipher.Decrypt(encryptedXmlString);

            StringReader stringReader = new(decryptedXmlString);
            List<T>? deserializedList = (List<T>?)serializer.Deserialize(stringReader);

            // if deserializedList is null, return an empty list
            return deserializedList ?? [];
        }
        else
        {
            return [];
        }
    }
}

public interface IWriter<T>
{
    void Write(List<T> list);
}

public class JSONWriter<T> : IWriter<T>
{

    private CaesarCipher cipher = new();

    public void Write(List<T> list)
    {
        Console.WriteLine("Writing to JSON");
        string jsonString = JsonSerializer.Serialize(list);

        // encrypt jsonString
        string encryptedJsonString = cipher.Encrypt(jsonString);

        //print jsonString for debugging
        Console.WriteLine(jsonString);

        File.WriteAllText("employee_catalog.json", encryptedJsonString);
    }
}

public class XMLWriter<T> : IWriter<T>
{

    private CaesarCipher cipher = new();

    public void Write(List<T> list)
    {
        XmlSerializer serializer = new(typeof(List<T>));
        using StringWriter stringWriter = new();
        serializer.Serialize(stringWriter, list);
        string encryptedXmlString = cipher.Encrypt(stringWriter.ToString());
        File.WriteAllText("employee_catalog.xml", encryptedXmlString);
    }
}


class CaesarCipher
{

    // Storing the shift value in app.config
    private int shift;

    // Encrypting a string
    public string Encrypt(string input)
    {
        // generate shift value
        this.shift = GenerateShift();

        StringBuilder encrypted = new();
        foreach (char c in input)
        {
            encrypted.Append((char)(c + shift));
        }
        return encrypted.ToString();
    }


    // Decrypting a string
    public string Decrypt(string input)
    {
        // read shift value from app.config
        this.shift = GetShift();

        StringBuilder decrypted = new();
        foreach (char c in input)
        {
            decrypted.Append((char)(c - shift));
        }
        return decrypted.ToString();
    }

    public static int GenerateShift()
    {
        Random random = new();
        int shift = random.Next(2, 10);

        // save to app.config
        ConfigurationManager.AppSettings["shift"] = shift.ToString();

        return shift;

    }

    public static int GetShift()
    {
        return int.TryParse(ConfigurationManager.AppSettings["shift"], out int parsedShift)
            ? parsedShift
            : throw new ArgumentException("Invalid shift value in configuration");
    }

}


class Program
{

    public void TestJSON()
    {
        // create catalog
        CatalogGeneric<Employee> catalog = new CatalogGeneric<Employee>(new JSONWriter<Employee>(), new JSONReader<Employee>());

        // create employees
        Employee employee1 = new Employee("John", "Doe", "Manager", "some@mail.com", 123);
        Employee employee2 = new Employee("Jane", "Doe", "Manager", "someother@mail.com", 456);
        Employee employee3 = new Employee("John", "Smith", "Manager", "someother@mail.com", 789);

        // add employees to catalog
        catalog.Add(employee1);
        catalog.Add(employee2);
        catalog.Add(employee3);

        // print catalog
        catalog.Print();

        // save catalog
        catalog.Write();

        // clear catalog
        catalog = new CatalogGeneric<Employee>(new JSONWriter<Employee>(), new JSONReader<Employee>());

        // read catalog
        catalog.Read();

        // print catalog
        catalog.Print();

    }

    public void TestXML()
    {
        // create catalog
        CatalogGeneric<Employee> catalog = new CatalogGeneric<Employee>(new XMLWriter<Employee>(), new XMLReader<Employee>());

        // create employees
        Employee employee1 = new Employee("John", "Doe", "Manager", "some@mail.com", 123);
        Employee employee2 = new Employee("Jane", "Doe", "Manager", "someother@mail.com", 456);
        Employee employee3 = new Employee("John", "Smith", "Manager", "someother@mail.com", 789);

        // add employees to catalog
        catalog.Add(employee1);
        catalog.Add(employee2);
        catalog.Add(employee3);

        // print catalog
        Console.WriteLine("Printing catalog before saving");
        catalog.Print();

        // save catalog
        catalog.Write();

        // clear catalog
        catalog = new CatalogGeneric<Employee>(new XMLWriter<Employee>(), new XMLReader<Employee>());

        // Printing empty catalog
        Console.WriteLine("Printing empty catalog");
        catalog.Print();

        // read catalog
        catalog.Read();

        // print catalog
        Console.WriteLine("Printing catalog after reading");
        catalog.Print();

    }

    static void Main(string[] args)
    {
        Program program = new Program();
        program.TestJSON();
        program.TestXML();
    }


}