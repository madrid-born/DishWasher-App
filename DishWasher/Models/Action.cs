using System.Text.Json;

namespace DishWasher.Models ;

    public class Action
    {
        public string GroupName { get; set; }
        public string Cleaner { get; set; }
        public List<string> SelectedUsers { get; set; }
        public DateTime Time { get; set; }
        
        public string SelectedUsersText { get; set; }
        public string TimeText { get; set; }
        
        public Action(string groupName, string cleaner, List<string> selectedUsers, DateTime time)
        {
            GroupName = groupName;
            Cleaner = cleaner;
            SelectedUsers = selectedUsers;
            Time = time;
            SelectedUsersText = ListToString(SelectedUsers);
            TimeText = Time.ToString();
        }

        private string ListToString(List<string> selectedUsers)
        {
            string result = "";
            foreach (var user in selectedUsers)
            {
                result += $"{user},";
            }
            result.Remove(result.Length - 1);
            return result;
        }
    }