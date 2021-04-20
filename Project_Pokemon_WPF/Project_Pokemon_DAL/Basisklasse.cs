using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Pokemon_Models
{
    public abstract class Basisklasse: IDataErrorInfo
    {
        //indexer
        public abstract string this[string columnName] { get; }

        //checkt of object geldig is
        public bool IsGeldig()
        {
            return string.IsNullOrWhiteSpace(Error);

        }

        //foutmeldingen
        public string Error
        {
            get
            {
                string foutmeldingen = "";

                foreach (var item in this.GetType().GetProperties()) //reflection 
                {
                    if (item.CanRead)
                    {
                        string fout = this[item.Name];
                        if (!string.IsNullOrWhiteSpace(fout))
                        {
                            foutmeldingen += fout + Environment.NewLine;
                        }
                    }
                }
                return foutmeldingen;
            }
        }
    }
}
