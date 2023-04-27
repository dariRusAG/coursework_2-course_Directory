using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Directory;
using System.IO;

namespace Directory
{
    public partial class Form1 : Form
    {
        internal DataGridView currentGrid;
        CategoryHashTable tab1 = new CategoryHashTable(); // Даша
        UsersHashTable tab2 = new UsersHashTable(); // Полина

        General gen = new General(); // 
        BinaryTreeCategory binTreeCategory = new BinaryTreeCategory(); // бинарное дерево Даша
        BinaryTree binTreeUsers = new BinaryTree(); // бинарное дерево Полина


        public Form1()
        {
            InitializeComponent();
        }

        // Дашина часть

        private void UpdateInformationDataViewCategory_combobox()
        {
            comboBox1_Category.Items.Clear();
            comboBox2_Category.Items.Clear();

            comboBox2_General.Items.Clear();
            comboBox3_General.Items.Clear();

            foreach (var elem in tab1.Element)
            {
                foreach (var value in elem.Value)
                {
                    int n = 0;
                    foreach (string i in comboBox1_Category.Items)
                        if (i.CompareTo(value.Key) == 0) n++;
                    if (n == 0)
                    {
                        comboBox1_Category.Items.Add(value.Key);
                        comboBox2_General.Items.Add(value.Key);
                    }

                    int nn = 0;
                    foreach (string i in comboBox2_Category.Items)
                        if (i.CompareTo(value.Value) == 0) nn++;
                    if (nn == 0)
                    {
                        comboBox2_Category.Items.Add(value.Value);
                        comboBox3_General.Items.Add(value.Value);
                    }
                }
            }
        }

        // Отобразить элементов в окне при загрузке
        private void UpdateInformationDataViewCategory()
        {
            int k = 0;

            foreach (var elem in tab1.Element)
            {
                foreach (var value in elem.Value)
                {
                    dataGridView1_Category.Rows.Add(tab1);
                    dataGridView1_Category.Rows[k].Cells[0].Value = elem.Key;
                    dataGridView1_Category.Rows[k].Cells[1].Value = value.Key;
                    dataGridView1_Category.Rows[k].Cells[2].Value = value.Value;
                    k++;

                    int n = 0;
                    foreach (string i in comboBox1_Category.Items)
                        if (i.CompareTo(value.Value) == 0) n++;
                    if (n == 0)
                    {
                        comboBox1_Category.Items.Add(value.Value);
                        comboBox3_General.Items.Add(value.Value);
                    }

                    int nn = 0;
                    foreach (string i in comboBox2_Category.Items)
                        if (i.CompareTo(value.Key) == 0) nn++;
                    if (nn == 0)
                    {
                        comboBox2_Category.Items.Add(value.Key);
                        comboBox2_General.Items.Add(value.Key);
                    }
                }
            }
        }

        // Загрузка данных из файла
        private void LoadInformationCategory()
        {
            string line;
            try
            {
                int count = 0;
                StreamReader file = new StreamReader("CATEGORY.txt");
                int n = System.IO.File.ReadAllLines("CATEGORY.txt").Length; // количество строк в файле

                for (int i = 0; i < n; i++)
                {
                    line = file.ReadLine();
                    string[] read = line.Split('_');
                    if (tab1.Proverka(read[0], read[1]) == 1) count++;
                    else tab1.Insert(read[0], read[1]);
                }

                UpdateInformationDataViewCategory();

                if (count > 0)
                {
                    toolStripStatusLabel1.Visible = true;
                    toolStripStatusLabel1.Text = "Проверьте корректность введенных данных в файле. Количество недобавленных строк - " + count;
                }

                else
                {
                    toolStripStatusLabel1.Visible = true;
                    toolStripStatusLabel1.Text = "Данные добавлены.";
                }
            }

            catch (Exception)
            {
                toolStripStatusLabel1.Visible = true;
                toolStripStatusLabel1.Text = "Ошибка при чтении файла.";
            }
        }

        // Нажатие на кнопку "ДОБАВИТЬ"
        private void button1_Category_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Visible = false;
            int k = tab1.Proverka(comboBox1_Category.Text, comboBox2_Category.Text);
            if (k == 1)
            {
                toolStripStatusLabel1.Visible = true;
                toolStripStatusLabel1.Text = "Проверьте корректность строки 'Категория' или 'Название'";
            }
            else
            {
                if (k == 3)
                {
                    toolStripStatusLabel1.Visible = true;
                    toolStripStatusLabel1.Text = "Такой ключ уже существует";
                }
                else
                {
                    tab1.Insert(comboBox1_Category.Text, comboBox2_Category.Text);
                    dataGridView1_Category.Rows.Add(tab1.GetHash(comboBox1_Category.Text, comboBox2_Category.Text), comboBox1_Category.Text, comboBox2_Category.Text);

                    toolStripStatusLabel1.Visible = true;
                    toolStripStatusLabel1.Text = "Данные " + comboBox1_Category.Text + " " + comboBox2_Category.Text + " добавлены.";
                }
            }
            UpdateInformationDataViewCategory_combobox();
        }

        // Нажатие на кнопку "НАЙТИ"
        private void button3_Category_Click(object sender, EventArgs e)
        {
           
            toolStripStatusLabel1.Visible = false;

            if (string.IsNullOrEmpty(comboBox1_Category.Text) || string.IsNullOrEmpty(comboBox2_Category.Text))
            {

                toolStripStatusLabel1.Visible = true;
                toolStripStatusLabel1.Text = "Проверьте корректность строки 'Название' или 'Категория'";
            }
            else
            {
                int q = tab1.Search(comboBox1_Category.Text, comboBox2_Category.Text);

                if (q == 0)
                {
                    toolStripStatusLabel1.Visible = true;
                    toolStripStatusLabel1.Text = "Элемент не найден";
                }

                int k = 0;

                foreach (var elem in tab1.Element)
                {
                    foreach (var value in elem.Value)
                    {
                        if (dataGridView1_Category.Rows[k].Cells[1].Value.ToString() == (comboBox2_Category.Text) &&
                            dataGridView1_Category.Rows[k].Cells[2].Value.ToString() == (comboBox1_Category.Text))
                        {
                            dataGridView1_Category.Rows[k].Cells[0].Selected = true;
                            dataGridView1_Category.Rows[k].Cells[1].Selected = true;
                            dataGridView1_Category.Rows[k].Cells[2].Selected = true;

                            toolStripStatusLabel1.Visible = true;
                            toolStripStatusLabel1.Text = "Количество сравнений - " + q;
                        }
                        else k++;
                    }
                }
            }
        }

        //Полинина часть 

        private void UpdateInformationDataViewUsers_combobox()
        {
            comboBox1_Users.Items.Clear();
            comboBox2_Users.Items.Clear();
            comboBox3_Users.Items.Clear();

            comboBox1_General.Items.Clear();
            comboBox5_General.Items.Clear();

            foreach (var elem in tab2.Element)
            {
                foreach (var value in elem.Value)
                {
                    int n = 0;
                    foreach (string i in comboBox1_Users.Items)
                        if (i.CompareTo(value.Key) == 0) n++;
                    if (n == 0)
                    {
                        comboBox1_Users.Items.Add(value.Key);
                        comboBox1_General.Items.Add(value.Key);
                    }

                    int nn = 0;
                    foreach (string i in comboBox3_Users.Items)
                        if (i.CompareTo(value.Key) == 0) nn++;
                    if (nn == 0) comboBox3_Users.Items.Add(value.Key);

                    int nnn = 0;
                    foreach (string i in comboBox2_Users.Items)
                        if (i.CompareTo(value.Value) == 0) nnn++;
                    if (nnn == 0)
                    {
                        comboBox2_Users.Items.Add(value.Value);
                        comboBox5_General.Items.Add(value.Value);
                    }
                }
            }
        }

        private void UpdateInformationDataViewUsers()
        {

            dataGridView1_Users.Rows.Clear();

            int k = 0;

            foreach (var elem in tab2.Element)
            {
                foreach (var value in elem.Value)
                {
                    dataGridView1_Users.Rows.Add(tab1);
                    dataGridView1_Users.Rows[k].Cells[0].Value = elem.Key;
                    dataGridView1_Users.Rows[k].Cells[1].Value = value.Key;
                    dataGridView1_Users.Rows[k].Cells[2].Value = value.Value;
                    k++;
                    int n = 0;
                    foreach (string i in comboBox1_Users.Items)
                        if (i.CompareTo(value.Key) == 0) n++;
                    if (n == 0)
                    {
                        comboBox1_Users.Items.Add(value.Key);
                        comboBox1_General.Items.Add(value.Key);
                    }

                    int nn = 0;
                    foreach (string i in comboBox3_Users.Items)
                        if (i.CompareTo(value.Key) == 0) nn++;
                    if (nn == 0) comboBox3_Users.Items.Add(value.Key);

                    int nnn = 0;
                    foreach (string i in comboBox2_Users.Items)
                        if (i.CompareTo(value.Value) == 0) nnn++;
                    if (nnn == 0)
                    {
                        comboBox2_Users.Items.Add(value.Value);
                        comboBox5_General.Items.Add(value.Value);
                    }
                }
            }
        }

        // Загрузка данных из файла
        private void LoadInformationUsers()
        {
            string line;
            try
            {
                int count = 0;
                StreamReader file = new StreamReader("USERS.txt");
                int n = System.IO.File.ReadAllLines("USERS.txt").Length; // количество строк в файле

                for (int i = 0; i < n; i++)
                {
                    line = file.ReadLine();
                    string[] read = line.Split('_');
                    
                    if (tab2.UsersProverka(read[1], read[0]) == 1) count++;
                    else tab2.UsersInsert(read[1], read[0]);
                }

                UpdateInformationDataViewUsers();

                if (count > 0)
                {
                    toolStripStatusLabel2.Visible = true;
                    toolStripStatusLabel2.Text = "Проверьте корректность введенных данных в файле. Количество недобавленных строк - " + count;
                }

                else
                {
                    toolStripStatusLabel2.Visible = true;
                    toolStripStatusLabel2.Text = "Данные добавлены.";
                }
            }

            catch (Exception)
            {
                toolStripStatusLabel2.Visible = true;
                toolStripStatusLabel2.Text = "Ошибка при чтении файла.";
            }
        }

        // Нажатие на кнопку "ДОБАВИТЬ"
        private void button_Users1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Visible = false;
            int k = tab2.UsersProverka(comboBox2_Users.Text, comboBox1_Users.Text);
            if (k == 1)
            {
                toolStripStatusLabel2.Visible = true;
                toolStripStatusLabel2.Text = "Проверьте корректность строки 'Логин' или 'Мерки пользователя'";
            }
            else
            {
                if (k == 3)
                {
                    toolStripStatusLabel2.Visible = true;
                    toolStripStatusLabel2.Text = "Такой ключ уже существует";
                }
                else
                {
                    if (k == 4)
                    {
                        toolStripStatusLabel2.Visible = true;
                        toolStripStatusLabel2.Text = "Проверьте корректность строки 'Логин' или 'Мерки пользователя'";
                    }
                    else
                    {
                        dataGridView1_Users.Rows.Add(tab2.UsersGetHash(comboBox1_Users.Text), comboBox1_Users.Text, comboBox2_Users.Text);
                        tab2.UsersInsert(comboBox2_Users.Text, comboBox1_Users.Text);
                        toolStripStatusLabel2.Visible = true;
                        toolStripStatusLabel2.Text = "Данные " + comboBox2_Users.Text + " " + comboBox1_Users.Text + " добавлены.";
                    }
                }
            }
            UpdateInformationDataViewUsers_combobox();
        }


        // Нажатие на кнопку "НАЙТИ"
        private void button_Users3_Click(object sender, EventArgs e)
        {
           
            toolStripStatusLabel2.Visible = false;

            if (string.IsNullOrEmpty(comboBox3_Users.Text))
            {
                toolStripStatusLabel2.Visible = true;
                toolStripStatusLabel2.Text = "Проверьте корректность строки 'Логин' ";
            }
            else
            {
                int q = tab2.UsersSearch("0990 0990 0990 0990 0990 0990 0990 0990 0990 0990 0990", comboBox3_Users.Text);

                if (q == 0)
                {
                    toolStripStatusLabel2.Visible = true;
                    toolStripStatusLabel2.Text = "Элемент не найден";
                }

                int k = 0;

                foreach (var elem in tab2.Element)
                {
                    foreach (var value in elem.Value)
                    {
                        if (dataGridView1_Users.Rows[k].Cells[1].Value.ToString() == comboBox3_Users.Text)
                        {
                            dataGridView1_Users.Rows[k].Cells[0].Selected = true;
                            dataGridView1_Users.Rows[k].Cells[1].Selected = true;
                            dataGridView1_Users.Rows[k].Cells[2].Selected = true;

                            toolStripStatusLabel2.Visible = true;
                            toolStripStatusLabel2.Text = "Количество сравнений - " + q;
                        }
                        else k++;
                    }
                }
            }
        }

        // Вкладка ИЗБРАННОЕ

        //Проверка корректности введенных данных
        public int Proverka(string user, string category, string name, string measure, string imeasure)
        {
            int n = 0;
            //Дашина проверка
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] < 'А' || name[i] > 'Я') n++;
                if (name[i] == ' ' || name[i] == '-') n--;
                if (name[i] == 'Ё') n--;
            }

            for (int i = 0; i < category.Length; i++)
            {
                if (category[i] < 'А' || category[i] > 'Я') n++;
                if (category[i] == 'Ё') n--;
            }

            //Полинина проверка
            int n2 = 0;
            int probel = 0;

            for (int i = 0; i < imeasure.Length; i++)
            {
                if ((imeasure[i] < '0' || imeasure[i] > '9') && !(imeasure[i] == ' '))
                    n2++;
            }

            for (int i = 0; i < imeasure.Length; i++)
            {
                if (probel <= 10)
                {
                    if (n2 == 0)
                    {
                        if (imeasure[i] == ' ')
                            probel++;
                    }
                    else break;
                }
                else return 0;
            }
            if ((probel < 10) || (probel > 10) || (imeasure[imeasure.Length - 1] == ' '))
            {
                return 0;
            }

            for (int i = 0; i < user.Length; i++)
            {
                if ((user[i] < 'A' || user[i] > 'Z') && (user[i] < 'a' || user[i] > 'z') && (user[i] < '0' || user[i] > '9'))
                    n2++;
            }

            if (string.IsNullOrEmpty(user) ||
                 string.IsNullOrEmpty(category) ||
                 string.IsNullOrEmpty(name) ||
                 string.IsNullOrEmpty(measure) ||
                 string.IsNullOrEmpty(imeasure) ||
                 n > 0 || n < 0 || n2 > 0) return 0;

            else return 1;
        }

        // Проверка на наличие в личных справочниках информации, которую добавляют в общий
        public int DataIntegrity(CategoryHashTable hash1, string name, string category, UsersHashTable hash2, string user, string imeasure)
        {
            int ct, us, ms;
            ct = CategoryProverka(hash1, name, category);
            us = UsersProverka(hash2, user, imeasure);
            ms = 1;

            if (ct == 0 && us == 0 && ms == 0) return 0;

            if (ct != 0 && us == 0 && ms == 0) return 2;
            if (ct == 0 && us != 0 && ms == 0) return 3;
            if (ct == 0 && us == 0 && ms != 0) return 4;

            if (ct != 0 && us != 0 && ms == 0) return 5;
            if (ct == 0 && us != 0 && ms != 0) return 6;
            if (ct != 0 && us == 0 && ms != 0) return 7;

            else return 1;
        }

        // Проверка на наличие в справочникe 'Категории' информации, которую добавляют в общий
        public int CategoryProverka(CategoryHashTable hash, string name, string category)
        {
            if (hash.Search(name, category) == 0) return 0;
            return 1;
        }

        // Проверка на наличие в справочникe 'Категории' информации, которую добавляют в общий
        public int UsersProverka(UsersHashTable hash, string user, string imeasure)
        {
            if (hash.UsersSearch(imeasure, user) == 0) return 0;
            return 1;
        }

        // Нажатие на кнопку "ДОБАВИТЬ"
        private void buttonGeneral_1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabelGeneral.Visible = false;

            int k = Proverka(comboBox1_General.Text, comboBox2_General.Text, comboBox3_General.Text, comboBox4_General.Text, comboBox5_General.Text);
            if (k == 0)
            {
                toolStripStatusLabelGeneral.Visible = true;
                toolStripStatusLabelGeneral.Text = "Проверьте корректность введенных строк";
            }

            else
            {
                int q = DataIntegrity(tab1, comboBox3_General.Text, comboBox2_General.Text, tab2, comboBox1_General.Text, comboBox5_General.Text);

                if (q == 0)
                {
                    toolStripStatusLabelGeneral.Visible = true;
                    toolStripStatusLabelGeneral.Text = "Данные не сходятся со всеми справочниками.";
                    return;
                }
                else if (q == 2)
                {
                    toolStripStatusLabelGeneral.Visible = true;
                    toolStripStatusLabelGeneral.Text = "Данные не сходятся со справочниками 'Пользователи' и 'Мерки'.";
                    return;
                }
                else if (q == 3)
                {
                    toolStripStatusLabelGeneral.Visible = true;
                    toolStripStatusLabelGeneral.Text = "Данные не сходятся со справочниками 'Категории' и 'Мерки'.";
                    return;
                }
                else if (q == 4)
                {
                    toolStripStatusLabelGeneral.Visible = true;
                    toolStripStatusLabelGeneral.Text = "Данные не сходятся со справочниками 'Категории' и 'Пользователи'.";
                    return;
                }
                else if (q == 5)
                {
                    toolStripStatusLabelGeneral.Visible = true;
                    toolStripStatusLabelGeneral.Text = "Данные не сходятся со справочником 'Мерки'.";
                    return;
                }
                else if (q == 6)
                {
                    toolStripStatusLabelGeneral.Visible = true;
                    toolStripStatusLabelGeneral.Text = "Данные не сходятся со справочником 'Категории'.";
                    return;
                }
                else if (q == 7)
                {
                    toolStripStatusLabelGeneral.Visible = true;
                    toolStripStatusLabelGeneral.Text = "Данные не сходятся со справочником 'Пользователи'.";
                    return;
                }

                else
                {
                    for (int i = 0; i < gen.GeneralList.Count(); i++)
                    {
                        if (gen.GeneralList[i].User == comboBox1_General.Text &&
                             gen.GeneralList[i].Category == comboBox2_General.Text &&
                              gen.GeneralList[i].Name == comboBox3_General.Text &&
                               gen.GeneralList[i].Measure == comboBox4_General.Text &&
                                gen.GeneralList[i].IndividualMeasure == comboBox5_General.Text)

                        {
                            toolStripStatusLabelGeneral.Visible = true;
                            toolStripStatusLabelGeneral.Text = "Такая запись уже существует";
                            return;
                        }
                    }
                    gen.Insert(comboBox1_General.Text, comboBox2_General.Text, comboBox3_General.Text, comboBox4_General.Text, comboBox5_General.Text);
                }

                int k1 = dataGridView1_General.Rows.Add(dataGridView1_General.Rows.Count - 1, comboBox1_General.Text, comboBox2_General.Text, comboBox3_General.Text, comboBox4_General.Text, comboBox5_General.Text);
                binTreeUsers.Add(comboBox1_General.Text, dataGridView1_General.Rows[k1]);
                string val = comboBox3_General.Text.ToString() + comboBox2_General.Text.ToString();
                binTreeCategory.Add(val, dataGridView1_General.Rows[k1]);

                toolStripStatusLabelGeneral.Visible = true;
                toolStripStatusLabelGeneral.Text = "Запись добавлена";
            }
        }

        // Нажатие на кнопку "УДАЛИТЬ"
        private void buttonGeneral_2_Click(object sender, EventArgs e)
        {

            DataGridViewRow currentRow = null;
            bool result;
            string key = "";

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    currentGrid = dataGridView1_Category;
                    currentRow = currentGrid.CurrentRow;
                    if (currentRow.Cells[1].Value == null)
                    {
                        break;
                    }
                    if (currentRow.Cells[0].Value != null)
                        key = currentRow.Cells[2].Value.ToString() + currentRow.Cells[1].Value.ToString();
                    break;

                case 1:
                    currentGrid = dataGridView1_Users;
                    currentRow = currentGrid.CurrentRow;
                    if (currentRow.Cells[0].Value == null)
                    {
                        break;
                    }
                    if (currentRow.Cells[0].Value != null)
                        key = currentRow.Cells[1].Value.ToString();
                    break;

                case 3:
                    currentGrid = dataGridView1_General;
                    currentRow = currentGrid.CurrentRow;
                    if (currentRow.Cells[0].Value != null) key = currentRow.Cells[1].Value.ToString();
                    break;

                default: break;
            }

            if (currentRow.Cells[0].Value != null)
            {
                if (currentGrid == dataGridView1_Users)
                {
                    if (binTreeUsers.Poisk(key) != null)
                    {
                        result = false;
                        if (result == false)
                        {
                            CircularLinkedList<DataGridViewRow> deletedIndexes = binTreeUsers.Delete2(key);
                            foreach (DataGridViewRow row in deletedIndexes)
                            {
                                binTreeCategory.Delete(row.Cells[3].Value.ToString() + row.Cells[2].Value.ToString(),row);

                                dataGridView1_General.Rows.RemoveAt(row.Index);

                            }
                        }
                    }

                    string login = currentRow.Cells[1].Value.ToString();
                    tab2.UsersDelete("0990 0990 0990 0990 0990 0990 0990 0990 0990 0990 0990", login);

                    if (gen.GeneralList.Count != 0)
                        gen.DeleteForUsers(currentRow.Cells[1].Value.ToString(), currentRow.Cells[2].Value.ToString());

                    UpdateInformationDataViewUsers_combobox();
                }

                if (currentGrid == dataGridView1_Category)
                {
                    if (binTreeCategory.Poisk(key) != null)
                    {
                        result = false;
                        if (result == false)
                        {
                            CircularDoublyLinkedList<DataGridViewRow> deletedIndexes = binTreeCategory.Delete2(key);
                            foreach (DataGridViewRow row in deletedIndexes)
                            {
                                binTreeUsers.Delete(row.Cells[1].Value.ToString(), row);

                                dataGridView1_General.Rows.RemoveAt(row.Index);
                            }
                        }
                    }

                    if (gen.GeneralList.Count != 0)

                        gen.DeleteForCategory(currentRow.Cells[1].Value.ToString(), currentRow.Cells[2].Value.ToString());

                    tab1.Delete(currentRow.Cells[2].Value.ToString(), currentRow.Cells[1].Value.ToString());

                    UpdateInformationDataViewCategory_combobox();
                }

                currentGrid.Rows.RemoveAt(currentRow.Index);

                if (currentGrid == dataGridView1_General)
                {

                    gen.Delete(currentRow.Cells[1].Value.ToString(), currentRow.Cells[2].Value.ToString(), currentRow.Cells[3].Value.ToString(),
                        currentRow.Cells[4].Value.ToString(), currentRow.Cells[5].Value.ToString());

                    string val = currentRow.Cells[3].Value.ToString() + currentRow.Cells[2].Value.ToString();
                    binTreeCategory.Delete(val, currentRow);
                    binTreeUsers.Delete(key, currentRow);
                }
            }
            else MessageBox.Show("Требуется выбрать строку", "Не выбрана строка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Загрузка данных для общего справочника из файла 
        public string LoadInformationGeneral(CategoryHashTable hash1, UsersHashTable hash2)
        {
            string line;
            try
            {
                int n = System.IO.File.ReadAllLines("FAVORITES.txt").Length;
                StreamReader file = new StreamReader("FAVORITES.txt");

                for (int i = 0; i < n; i++)
                {
                    line = file.ReadLine();
                    string[] Ready = line.Split('_');

                    int q = DataIntegrity(hash1, Ready[2], Ready[1], hash2, Ready[0], Ready[4]);

                    if (q == 0) return "Данные не сходятся со всеми справочниками. Ничего не добавлено.";
                    if (q == 2) return "Данные не сходятся со справочниками 'Пользователи' и 'Мерки'. Добавлены не все данные.";
                    if (q == 3) return "Данные не сходятся со справочниками 'Категории' и 'Мерки'. Добавлены не все данные.";
                    if (q == 4) return "Данные не сходятся со справочниками 'Категории' и 'Пользователи'. Добавлены не все данные.";
                    if (q == 5) return "Данные не сходятся со справочником 'Мерки'. Добавлены не все данные.";
                    if (q == 6) return "Данные не сходятся со справочником 'Категории'. Добавлены не все данные.";
                    if (q == 7) return "Данные не сходятся со справочником 'Пользователи'. Добавлены не все данные.";
                    else 
                        gen.Insert(Ready[0], Ready[1], Ready[2], "/////", Ready[4]);
     
                    int k1 = dataGridView1_General.Rows.Add(dataGridView1_General.Rows.Count - 1, Ready[0], Ready[1], Ready[2], "/////", Ready[4]);
                    binTreeUsers.Add(Ready[0], dataGridView1_General.Rows[k1]);
                    string val = Ready[2] + Ready[1];
                    binTreeCategory.Add(val, dataGridView1_General.Rows[k1]);
                }
                return "Проверка данных прошла успешно. Записи все добавлены.";
            }
            catch (Exception)
            {
                return "Ошибка при чтении файла.";
            }
        }

        // Загрузка данных из файлов (для всех вкладок)
        private void General_Load(object sender, EventArgs e)
        {
            LoadInformationCategory();
            LoadInformationUsers();
            
            toolStripStatusLabelGeneral.Visible = true;
            toolStripStatusLabelGeneral.Text = LoadInformationGeneral(tab1, tab2);
        }


        private void radioButton1_Category_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox.Text = binTreeCategory.Info();
        }

        private void radioButton2_Users_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox.Text = binTreeUsers.Info();
        }

        private void buttonGeneral_Category_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = $"Индексы:\n";
            CircularDoublyLinkedList<DataGridViewRow> cur = new CircularDoublyLinkedList<DataGridViewRow>();
            int cur2;

            cur = binTreeCategory.Poisk(comboBox3_General.Text + comboBox2_General.Text);
            cur2 = binTreeCategory.Poisk2(comboBox3_General.Text + comboBox2_General.Text);
            if (cur == null) richTextBox1.Text = "Изделие не было найдено";
            else
            {
                foreach (DataGridViewRow el in cur)
                {
                    richTextBox1.Text += $"{el}\n";
                    el.Cells[0].Selected = true;
                    el.Cells[1].Selected = true;
                    el.Cells[2].Selected = true;
                    el.Cells[3].Selected = true;
                    el.Cells[4].Selected = true;
                    el.Cells[5].Selected = true;
                }
                richTextBox1.Text += $"Количество сравнений - {cur2}\n";
            }
        }

        private void buttonGeneral_Users_Click(object sender, EventArgs e)
        {


            richTextBox1.Text = $"Индексы:\n";
            CircularLinkedList<DataGridViewRow> cur = new CircularLinkedList<DataGridViewRow>();
            int cur2;

            cur = binTreeUsers.Poisk(comboBox1_General.Text);
            cur2 = binTreeUsers.Poisk2(comboBox1_General.Text);
            if (cur == null) richTextBox1.Text = "Пользователь не был найден";
            else
            {
                foreach (DataGridViewRow el in cur)
                {
                    richTextBox1.Text += $"{el}\n";
                    el.Cells[0].Selected = true;
                    el.Cells[1].Selected = true;
                    el.Cells[2].Selected = true;
                    el.Cells[3].Selected = true;
                    el.Cells[4].Selected = true;
                    el.Cells[5].Selected = true;
                }
                richTextBox1.Text += $"Количество сравнений - {cur2}\n";


            }

        }
        private void General_Save(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogSave = MessageBox.Show("Сохранить изменения?", "Данные сохранены.", MessageBoxButtons.YesNo);
            if (dialogSave == DialogResult.Yes)
            {
                string file = "FAVORITES2.txt";
                using (StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine("Справочник Категории");
                    for (int i = 0; i < dataGridView1_Category.Rows.Count; i++)
                    {
                        sw.WriteLine($"{dataGridView1_Category.Rows[i].Cells[1].Value}" + "_" + $"{dataGridView1_Category.Rows[i].Cells[2].Value}");
                    }
                    sw.WriteLine("Справочник Пользователи");
                    for (int i = 0; i < dataGridView1_Users.Rows.Count; i++)
                    {
                        sw.WriteLine($"{dataGridView1_Users.Rows[i].Cells[1].Value}" + "_" + $"{dataGridView1_Users.Rows[i].Cells[2].Value}");
                    }
                    sw.WriteLine("Справочник Избранное");
                    for (int i = 0; i < dataGridView1_General.Rows.Count; i++)
                    {
                        sw.WriteLine($"{dataGridView1_General.Rows[i].Cells[1].Value}" + "_"
                        + $"{dataGridView1_General.Rows[i].Cells[2].Value}" + "_"
                        + $"{dataGridView1_General.Rows[i].Cells[3].Value}" + "_"
                        + $"{dataGridView1_General.Rows[i].Cells[4].Value}" + "_"
                        + $"{dataGridView1_General.Rows[i].Cells[5].Value}");
                    }
                }
            }
        }
    }
}

