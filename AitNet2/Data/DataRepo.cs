using AitNet2;
using AitNet2.Models;
using Newtonsoft.Json;

namespace AitNet2.Data
{
	public class DataRepo
    {
        public List<Person> PersonList { get; set; }
        public List<Contact> ContactList { get; set; }
        public List<BoyAndGirl> BoyAndGirlList { get; set; }

        public DataRepo()
        {
            PersonList = new List<Person>();
            ContactList = new List<Contact>();
            BoyAndGirlList = new List<BoyAndGirl>();
        }
        public void LoadList()
        {
            string json = File.ReadAllText("Data/People.json");
            PersonList = JsonConvert.DeserializeObject<List<Person>>(json);
            //List<Person> PersonList = JsonConvert.DeserializeObject<List<Person>>(json);
            //PersonList = PersonList.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
        }
        public void LoadContactList()
        {
            string json = File.ReadAllText("Data/Contacts90.json");
            ContactList = JsonConvert.DeserializeObject<List<Contact>>(json);
            //PersonList = PersonList.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
        }
        public static string GetAddressPassKey()
        {
            string json = File.ReadAllText("Data/PassKeys.json");
            List<string> keys = JsonConvert.DeserializeObject<List<string>>(json);
            DateTime today = Globals.GetUKDateTime(); 
            int day = today.Day - 1; // Zero based list
            return keys[day];
        }

        public void LoadBoyAndGirlList()
        {
            string json = File.ReadAllText("Data/BoysAndGirls.json");
            BoyAndGirlList = JsonConvert.DeserializeObject<List<BoyAndGirl>>(json);
        }
    }
}
