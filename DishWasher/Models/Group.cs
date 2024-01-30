using System.Text.Json;

namespace DishWasher.Models ;

    public class Group
    {
        public static string Name { get; set; }
        public static List<string> Members { get; set; }

        public Group(string name)
        {
            Name = name;
            Members = Groups.GetGroupMembers(name);
        }
    }