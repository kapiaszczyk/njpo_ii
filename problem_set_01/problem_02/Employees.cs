using System.Collections;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;

class CatalogGeneric<T>()
{

    private List<T> catalog = [];

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


}

public class Employee
{

    // Instance variables 
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Position { get; set; }
    public string Email { get; set; }
    public int Id { get; set; }


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

class Program
{
    static void Main(string[] args)
    {
    }

}