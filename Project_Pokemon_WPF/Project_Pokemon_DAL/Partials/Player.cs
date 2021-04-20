using Project_Pokemon_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Pokemon_DAL
{
    public partial class Player : Basisklasse
    {
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Name" && string.IsNullOrWhiteSpace(Name))
                {
                    return "Your username can't be nothing!";

                }
                else if (columnName == "Name" && Name.Length < 2)
                {
                    return "Your username can't be less than 2 characters";
                }
                else if (columnName == "Name" && Name.Length > 15)
                {
                    return "Your username can't be more than 15 characters";
                }
                return "";
            }
        }

        public override string ToString()
        {
            if(Id == 0)
            {
                return Name;
            }
            else
            {
                return $"{Name.PadRight(34)} {("Playtime: " + PlayedTime.ToString(@"hh\:mm\:ss"))}";
            }

        }
    }
}
