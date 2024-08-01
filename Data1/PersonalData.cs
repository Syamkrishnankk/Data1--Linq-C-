using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public class PersonalData
{
    public string? Name { get; set; } 
    public int? Age { get; set; }
    public string? Email { get; set; }
    public List<string>? Hobbies { get; set; }
}

class Data
{
    static void Main(string[] args)
    {
        string jsonData = @"[
    {
        'name': 'John Doe',
        'age': 28,
        'email': 'john.doe@example.com',
        'hobbies': [
            'Reading',
            'Swimming',
            'Coding'
        ]
    },
    {
        'name': 'Jane Smith',
        'age': 34,
        'email': 'jane.smith@example.com',
        'hobbies': [
            'Painting',
            'Running',
            'Cycling'
        ]
    },
    {
        'name': 'Sam Brown',
        'age': 22,
        'email': 'sam.brown@example.com',
        'hobbies': [
            'Gaming',
            'Music',
            'Cooking'
        ]
    },
    {
        'name': 'Emily White',
        'age': 45,
        'email': 'emily.white@example.com',
        'hobbies': [
            'Gardening',
            'Photography',
            'Traveling'
        ]
    },
    {
        'name': 'Michael Green',
        'age': 29,
        'email': 'michael.green@example.com',
        'hobbies': [
            'Hiking',
            'Writing',
            'Swimming'
        ]
    }
]";


var people = JsonConvert.DeserializeObject<List<PersonalData>>(jsonData);


// 1. Find all people who are older than 30.
Console.WriteLine("People who are older than 30");
var age = people.Where(p => p.Age > 30).Select(s => s.Name);
foreach(var a in age){
  Console.WriteLine($"-> {a}");
}
Console.WriteLine("");

// 2. Extract only the names and email addresses of all people.
Console.WriteLine("Extract only the names and email addresses of all people.");
var nameAndEmail = people.Select(s => new{
  name = s.Name,
  email = s.Email
});
foreach(var data in nameAndEmail){
  Console.WriteLine($"-> {data.name}, {data.email}");
}
Console.WriteLine("");

// 3. Sort the list of people by age in ascending order.
Console.WriteLine("Sort the list of people by age in ascending order.");
var sortPeople = people.OrderBy(e => e.Age);
foreach(var data in sortPeople){
  Console.WriteLine($"-> {data.Name}");
}
Console.WriteLine("");

// 4. Group people by the first letter of their name.
Console.WriteLine("Group people by the first letter of their name.");
var groupPeople = people.GroupBy(s => s.Name.FirstOrDefault());
foreach(var data in groupPeople){
  Console.WriteLine($"-> {data.Key}");
  foreach(var d in data){
    Console.WriteLine($"---> {d.Name}");
  }
}
Console.WriteLine("");

// 5. Find the average age of all people in the dataset.
Console.WriteLine("Find the average age of all people in the dataset.");
var avgAge = people.Average(s => s.Age);
Console.WriteLine($"-> {String.Format("{0:0}",avgAge)}");
Console.WriteLine("");

// 6. Find the first person who has "Swimming" as a hobby.
Console.WriteLine("Find the first person who has 'Swimming' as a hobby");
var firstPerson = people.FirstOrDefault(s => s.Hobbies.Contains("Swimming"));
Console.WriteLine($"-> {firstPerson.Name}");
Console.WriteLine("");

// 7. List the names of people who are older than 25 and have "Cycling" as a hobby.
Console.WriteLine("List the names of people who are older than 25 and have 'Cycling' as a hobby.");
var namePeople = people.Where(s => s.Age > 25 && s.Hobbies.Contains("Cycling")).Select(s => s.Name);
foreach(var data in namePeople){
  Console.WriteLine($"-> {data}");
}
Console.WriteLine("");

// 8. Count the number of people who have more than two hobbies.
Console.WriteLine("Count the number of people who have more than two hobbies.");
var count = people.Where(s => s.Hobbies.Count() > 2).Select(s => s.Name).Count();
Console.WriteLine($"-> {count}");
Console.WriteLine("");

// 9. Check if there is any person whose name starts with 'E'.
Console.WriteLine("Check if there is any person whose name starts with 'E'.");
var starte = people.Any(s => s.Name.StartsWith("E"));
Console.WriteLine($"-> {starte}");
Console.WriteLine("");

// 10. List all unique hobbies in the dataset.
Console.WriteLine("List all unique hobbies in the dataset.");
var uniqueHobbies = people.SelectMany(s => s.Hobbies).Distinct();
foreach(var data in uniqueHobbies){
  Console.WriteLine($"-> {data}");
} 
Console.WriteLine("");

    }
}
