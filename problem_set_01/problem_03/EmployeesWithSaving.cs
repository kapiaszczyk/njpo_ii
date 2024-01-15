using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

class CatalogGeneric<T>
{
    private List<T> catalog = [];
    private readonly IReader<T> reader;
    private readonly IWriter<T> writer;

    public CatalogGeneric(IWriter<T> writer, IReader<T> reader)
    {
        this.writer = writer;
        this.reader = reader;
    }

    public void Add(T item) { catalog.Add(item); }
    public void AddMany(List<T> items) { catalog.AddRange(items); }
    public void Remove(T item) { catalog.Remove(item); }

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

    public T? Search(T catalogItem) { return (catalog.Count != 0 && catalog.Contains(catalogItem)) ? catalogItem : default(T); }
    public void Clear() { catalog.Clear(); }
    public bool Validate(T item) { return catalog.Contains(item); }
    public void Write() { writer.Write(catalog); }
    public void Read() { catalog = reader.Read(); }

}

public class Employee
{

    public string Name { get; set; }
    public string Surname { get; set; }
    public string Position { get; set; }
    public string Email { get; set; }
    public int Id { get; set; }

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

    // Overriding Equals(), otherwise Contains() won't work, GetHashCode() is also required
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        else
        {
            Employee employee = (Employee)obj;
            return (
                this.Name == employee.Name &&
                this.Surname == employee.Surname &&
                this.Position == employee.Position &&
                this.Email == employee.Email &&
                this.Id == employee.Id
            );
        }
    }

    public override int GetHashCode() { return Id.GetHashCode(); }

    public bool ValidateData() { return EmployeeValidator.ValidateAll(this.Name, this.Surname, this.Position, this.Email, this.Id); }

    public String Show() { return string.Format("Name: {0}, Surname: {1}, Position: {2}, Email: {3},  Id: {4}", this.Name, this.Surname, this.Position, this.Email, this.Id); }

    public bool IsMatch(Employee employee) { return employee.Equals(this); }

    public override string ToString() { return string.Format("Name: {0}, Surname: {1}, Position: {2}, Email: {3}, Id: {4}", this.Name, this.Surname, this.Position, this.Email, this.Id); }

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

    private static bool ValidateName(String name)
    {
        return !String.IsNullOrEmpty(name) && name.Length > MIN_NAME_LENGTH && name.Length < MAX_NAME_LENGTH && Regex.IsMatch(name, NAME_REGEX);
    }

    private static bool ValidateEmail(String email)
    {
        return Regex.IsMatch(email, EMAIL_REGEX);
    }

    private static bool ValidatePosition(String position)
    {
        return !String.IsNullOrEmpty(position) && position.Length > MIN_POSITION_LENGTH && position.Length < MAX_POSITION_LENGTH && Regex.IsMatch(position, NAME_REGEX);
    }

    private static bool ValidateId(int id) { return id > MIN_ID; }

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
    public List<T> Read()
    {
        if (File.Exists("employee_catalog.json"))
        {
            string jsonString = File.ReadAllText("employee_catalog.json");

            List<T>? deserializedList = JsonSerializer.Deserialize<List<T>>(jsonString);
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
    public List<T> Read()
    {
        if (File.Exists("employee_catalog.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (FileStream fileStream = File.OpenRead("employee_catalog.xml"))
            {
                List<T>? deserializedList = serializer.Deserialize(fileStream) as List<T>;
                return deserializedList ?? [];
            }
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
    public void Write(List<T> list)
    {
        Console.WriteLine("Writing to JSON");
        string jsonString = JsonSerializer.Serialize(list);

        //print jsonString for debugging
        Console.WriteLine(jsonString);

        File.WriteAllText("employee_catalog.json", jsonString);
    }
}

public class XMLWriter<T> : IWriter<T>
{
    public void Write(List<T> list)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
        using (FileStream fileStream = File.Create("employee_catalog.xml"))
        {
            serializer.Serialize(fileStream, list);
        }
    }
}

class Program
{
    public bool TestJSON()
    {
        CatalogGeneric<Employee> catalog = new CatalogGeneric<Employee>(new XMLWriter<Employee>(), new XMLReader<Employee>());

        catalog.AddMany(new List<Employee>() {
            new Employee("Geralt", "Wiedzmin", "Wiedzmin", "geralt@gmail.com", 001),
            new Employee("Triss", "Merigold", "Czarodziejka", "triss@gmail.com", 002),
            new Employee("Sigismund", "Dijkstra", "Szpieg", "sigi@gmail.com", 003)
        });
        catalog.Write();
        catalog.Clear();
        catalog.Read();

        return catalog.Validate(new Employee("Triss", "Merigold", "Czarodziejka", "triss@gmail.com", 002));

    }

    public bool TestXML()
    {
        CatalogGeneric<Employee> catalog = new CatalogGeneric<Employee>(new XMLWriter<Employee>(), new XMLReader<Employee>());

        catalog.AddMany(new List<Employee>() {
            new Employee("Geralt", "Wiedzmin", "Wiedzmin", "geralt@gmail.com", 001),
            new Employee("Triss", "Merigold", "Czarodziejka", "triss@gmail.com", 002),
            new Employee("Sigismund", "Dijkstra", "Szpieg", "sigi@gmail.com", 003)
        });
        catalog.Write();
        catalog.Clear();
        catalog.Read();

        return catalog.Validate(new Employee("Triss", "Merigold", "Czarodziejka", "triss@gmail.com", 002));

    }

    static void Main(string[] args)
    {
        Program program = new Program();

        Console.WriteLine("JSON: " + program.TestJSON());
        Console.WriteLine("XML: " + program.TestXML());
    }

}