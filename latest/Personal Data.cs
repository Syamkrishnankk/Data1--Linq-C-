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
var groupPeople = people.GroupBy(s => s.Name);
foreach(var data in groupPeople){
  // Console.WriteLine($"-> {data.Key}");
  foreach(var d in data){
    Console.WriteLine($"-> {d.Name}");
  }
}
Console.WriteLine("");

// 5. Find the average age of all people in the dataset.
Console.WriteLine("Find the average age of all people in the dataset.");
var avgAge = people.Average(s => s.Age);
Console.WriteLine($"-> {String.Format("{0:0}",avgAge)}");
Console.WriteLine("");

// 6. Find the first person who has "Swimming" as a hobby.
Console.WriteLine("Find the first person who has Swimming as a hobby");
var firstPerson = people.Select(s => s.Hobbies);
foreach(var data in firstPerson){
  foreach(var d in data){
    Console.WriteLine(d);
  }
  
}


    }
}
