using System.Text.Json;

namespace DishWasher.Models ;

    // this class will help the app to run faster and make the code cleaner
    public static class Groups
    {
        //return a list with all the names of all the groups you have
        public static List<string> GetGroupsNames()
        {
            //read from dataBase
            string jsonString = Preferences.Get("GroupsNames", "");
            List<string> groupsNames = jsonString is "" ? new List<string>() : JsonSerializer.Deserialize<List<string>>(jsonString);
            return groupsNames;
        }

        //
        // return a list with "group" object of all the groups you have
        public static List<Group> GetGroupsList()
        {
            List<Group> groupsArray = new List<Group>();
            foreach (var group in GetGroupsNames())
            {
                //making a new object
                groupsArray.Add(new Group(group));
            }
            return groupsArray;
        }
        
        // return names of members of a group
        public static List<string> GetGroupMembers(string name)
        {
            //read from dataBase
            string jsonString = Preferences.Get($"{name}.members", "");
            List<string> members = jsonString is "" ? new List<string>() : JsonSerializer.Deserialize<List<string>>(jsonString);
            return members;
        }
        
        // returns the table of washed dish
        public static List<List<int>> GetTable(string groupName)
        {
            //read from dataBase
            string jsonString = Preferences.Get($"{groupName}.table", "");
            List<List<int>> table = jsonString is "" ? new List<List<int>>() : JsonSerializer.Deserialize<List<List<int>>>(jsonString);
            return table;
        }
        
        // returns the the whole detailed actions list
        public static List<Action> GetActions(string groupName)
        {
            string jsonString = Preferences.Get($"{groupName}.Actions", "");
            List<Action> actions = jsonString is "" ? new List<Action>() : JsonSerializer.Deserialize<List<Action>>(jsonString);
            return actions;
        }
        
        public static void SetGroupsList(string newGroupName)
        {
            List<string> newGroupsList = GetGroupsNames();
            newGroupsList.Add(newGroupName);
            string jsonString = JsonSerializer.Serialize(newGroupsList);
            Preferences.Set("GroupsNames", jsonString);
        }

        public static void SetGroupMembers(string groupName, List<string> names)
        {
            string jsonString = JsonSerializer.Serialize(names);
            Preferences.Set($"{groupName}.members", jsonString);
            Preferences.Set($"{groupName}.table", CleanTable(names));
        }

        private static string CleanTable(List<string> names)
        {
            List<List<int>> table = new List<List<int>>();
            List<int> row = new List<int>();
            foreach (var _ in names)
            {
                row.Add(0);
            }
            foreach (var _ in names)
            {
                table.Add(row);
            }
            string jsonString = JsonSerializer.Serialize(table);
            return jsonString;
        }

        public static void SetTableChange(string groupName, string cleaner, List<string> selectedUsers, bool add)
        {
            List<List<int>> table = GetTable(groupName);
            List<string> members = GetGroupMembers(groupName);
            int cleanerIndex = members.IndexOf(cleaner);
            List<int> newRow = new List<int>();
            for (int i = 0; i < table[cleanerIndex].Count; i += 1)
            {
                if (selectedUsers.Contains(members[i]))
                {
                   if (add)
                       newRow.Add(table[cleanerIndex][i] + 1);
                   else
                       newRow.Add(table[cleanerIndex][i] - 1); 
                }else
                    newRow.Add(table[cleanerIndex][i]);
            }
            table[cleanerIndex] = newRow;
            string jsonString = JsonSerializer.Serialize(table);
            Preferences.Set($"{groupName}.table", jsonString);
        }

        public static void AddAction(string groupName, string cleaner, List<string> selectedUsers, DateTime time)
        {
            Action currentWash = new Action(groupName, cleaner, selectedUsers, time);
            string jsonString = Preferences.Get($"{groupName}.Actions", "");
            List<Action> actions = jsonString is "" ? new List<Action>() : JsonSerializer.Deserialize<List<Action>>(jsonString);
            actions.Add(currentWash);
            jsonString = JsonSerializer.Serialize(actions);
            Preferences.Set($"{groupName}.Actions", jsonString);
        }

        public static void DeleteGroup(string groupName)
        {
            Preferences.Set($"{groupName}.members", "");
            Preferences.Set($"{groupName}.table", "");
            Preferences.Set($"{groupName}.Actions", "");
            List<string> groups = GetGroupsNames();
            groups.Remove(groupName);
            string jsonString = JsonSerializer.Serialize(groups);
            Preferences.Set("GroupsNames", jsonString);
        }
        
        public static void DeleteAction(Action action)
        {
            string groupName = action.GroupName;
            SetTableChange(groupName, action.Cleaner, action.SelectedUsers, false);
            List<Action> actions= GetActions(groupName);
            actions.RemoveAll(evt => evt.Time == action.Time);
            string jsonString = JsonSerializer.Serialize(actions);
            Preferences.Set($"{groupName}.Actions", jsonString);
        }
    }