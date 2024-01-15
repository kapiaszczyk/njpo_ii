using System.Collections;
using System.Text.RegularExpressions;

class CatalogGeneric<T>
{
    private CaesarCipher cipher = new();

    public CatalogGeneric()
    {
        catalog = [];
    }

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
                Console.WriteLine(item.ToString());
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

public class Employee(string name, string surname, string position, string email)
{

    // Instance variables 
    private string name = name;
    private string surname = surname;
    private string position = position;
    private string email = email;
    private int id;


    // getters
    public String GetName()
    {
        return this.name;
    }

    public String GetSurname()
    {
        return this.surname;
    }

    public String GetPosition()
    {
        return this.position;
    }

    public String GetEmail()
    {
        return this.email;
    }

    public int GetId()
    {
        return this.id;
    }

    // setters

    public void SetName(String name)
    {
        this.name = name;
    }

    public void SetSurname(String surname)
    {
        this.surname = surname;
    }

    public void SetPosition(String position)
    {
        this.position = position;
    }

    public void SetEmail(String email)
    {
        this.email = email;
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public bool ValidateData(string name, string surname, string position, string email, int id)
    {
        EmployeeValidator validator = new();
        return EmployeeValidator.ValidateAll(name, surname, position, email, id);
    }

    // Show()
    public String Show()
    {
        return string.Format("Name: {0}, Surname: {1}, Position: {2}, Email: {3},  Id: {4}", this.name, this.surname, this.position, this.email, this.id);
    }
    // IsMatch()
    public bool IsMatch(String name, String surname, String position, String email)
    {
        return (
            name.Equals(this.name) &&
            surname.Equals(this.surname) &&
            position.Equals(this.position) &&
            email.Equals(this.email)
            );
    }

    // ToString()
    public override string ToString()
    {
        return string.Format("Name: {0}, Surname: {1}, Position: {2}, Email: {3}", name, surname, position, email);
    }
}

class EmployeeValidator
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
