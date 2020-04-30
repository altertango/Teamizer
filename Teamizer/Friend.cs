using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamizer {
    class Friend {
        public Dictionary<string, int> teaRatings =new Dictionary<string,int>();
        public static List<string> fList = new List<string>();
        public Friend(string name) {
            Name = name;
            fList.Add(Name);
        }
        public string Name { get; set; }
        


        public void AddTea(string line) {
            string[] lineSplit = line.Split(',');
            if (lineSplit[0] == Name)
            {
                int rate;
                bool suc=Int32.TryParse(lineSplit[2], out rate);
                if (suc)
                {
                    if (teaRatings.ContainsKey(lineSplit[1]))
                    {
                        teaRatings[lineSplit[1]] = rate;
                    }
                    else
                    {
                        teaRatings.Add(lineSplit[1], rate);
                    }
                    
                }
                
            }
        }
        public static void RemoveF(string f) {
            fList.Remove(f);
        }
        public override string ToString() {
            string msg = $"List of Tea Ratings for {Name}:\n";
            foreach (KeyValuePair<String,int> entry in teaRatings)
            {
                msg += entry.Key + ": " + entry.Value + " point(s)\n";
            }
            return msg;
        }

    }
}
