using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Directory
{
    class General
    {
        public class GeneralDirectory
        {
            public string User;
            public string Category;
            public string Name;
            public string Measure;
            public string IndividualMeasure;

            public GeneralDirectory(string user, string category, string name, string measure, string imeasure)
            {
                User = user;
                Category = category;
                Name = name;
                Measure = measure;
                IndividualMeasure = imeasure;
            }
        }

        public List<GeneralDirectory> GeneralList;

        public General()
        {
            GeneralList = new List<GeneralDirectory>();
        }
       
        // Добавление элементов в общий справочник
        public void Insert(string user, string category, string name, string measure, string imeasure)
        {
            GeneralDirectory directory = new GeneralDirectory(user, category, name, measure, imeasure);
            bool q = false;
            foreach (var k in GeneralList)
            {
                if (k.User == directory.User &&
                     k.Category == directory.Category &&
                     k.Name == directory.Name &&
                     k.Measure == directory.Measure &&
                     k.IndividualMeasure == directory.IndividualMeasure
                   ) q = true;
            }

            if (!q) GeneralList.Add(directory);
        }

        // Удаление элементов из общего справочника
        public void Delete(string user, string category, string name, string measure, string imeasure)
        {
            for (int i = 0; i < GeneralList.Count; i++)
            {
                if (
                    GeneralList[i].User == user &&
                    GeneralList[i].Category == category &&
                    GeneralList[i].Name == name &&
                    GeneralList[i].Measure == measure &&
                    GeneralList[i].IndividualMeasure == imeasure
                    )
                {
                    GeneralList.RemoveAt(i); // удалить элемент
                }
            }
        }

        
        public void DeleteForUsers(string user, string imeasure)
        {
            for (int i = 0; i < GeneralList.Count; i++)
            {
                if (
                    GeneralList[i].User == user &&
                    GeneralList[i].IndividualMeasure == imeasure
                    )
                {
                    GeneralList.RemoveAt(i); // удалить элемент
                }
            }
        }

        public void DeleteForCategory(string category, string name)
        {
            for (int i = 0; i < GeneralList.Count; i++)
            {
                if (
                  GeneralList[i].Category == category &&
                    GeneralList[i].Name == name 
                    )
                {
                    GeneralList.RemoveAt(i); // удалить элемент
                }
            }
        }

    }
}
