using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Directory;
using System.IO;

namespace Directory
{
    public class Elem
    {
        public string Val { get; set; } // Хранимые данные.
        public string Key { get; set; }

        // Создание нового экземпляря хранимых данных Elem.
        // key - ключ, val - значение
        public Elem(string val, string key)
        {
            // проверяем не пустые ли входные данные
            if (string.IsNullOrEmpty(val))
                throw new ArgumentNullException(nameof(val));
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            Val = val; // название
            Key = key; // категория
        }
    }

    public class Node<T>
    {
        public Node(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
        public Node<T> Next { get; set; }
    }

    public class CircularLinkedList<T> : IEnumerable<T>  // кольцевой связный список
    {
        Node<T> head; // головной/первый элемент
        Node<T> tail; // последний/хвостовой элемент
        int count;  // количество элементов в списке

        // добавление элемента
        public void Add(T data)
        {
            Node<T> node = new Node<T>(data);
            // если список пуст
            if (head == null)
            {
                head = node;
                tail = node;
                tail.Next = head;
            }
            else
            {
                node.Next = head;
                tail.Next = node;
                tail = node;
            }
            count++;
        }
        public bool Remove(T data)
        {
            Node<T> current = head;
            Node<T> previous = null;

            if (IsEmpty) return false;

            do
            {
                if (current.Data.Equals(data))
                {
                    // Если узел в середине или в конце
                    if (previous != null)
                    {
                        // убираем узел current, теперь previous ссылается не на current, а на current.Next
                        previous.Next = current.Next;

                        // Если узел последний,
                        // изменяем переменную tail
                        if (current == tail)
                            tail = previous;
                    }
                    else // если удаляется первый элемент
                    {

                        // если в списке всего один элемент
                        if (count == 1)
                        {
                            head = tail = null;
                        }
                        else
                        {
                            head = current.Next;
                            tail.Next = current.Next;
                        }
                    }
                    count--;
                    return true;
                }

                previous = current;
                current = current.Next;
            } while (current != head);

            return false;
        }

        public int Count { get { return count; } }
        public bool IsEmpty { get { return count == 0; } }

        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public string ElementsInfo()
        {
            string result = string.Empty;
            Node<T> current = head;
            while (current != null)
            {
                result += $"{current.Data}\n";
                current = current.Next;
            }

            return result;
        }

        public bool Contains(T data)
        {
            Node<T> current = head;
            if (current == null) return false;
            do
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            while (current != head);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            Node<T> current = head;
            do
            {
                if (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
            while (current != head);
        }
    }

    public class UsersHashTable
    {
        public readonly byte Max_Size = 10; // Максимальная длина ключевого поля.
        public Dictionary<int, CircularLinkedList<Element>> _element = null;  // Хранимые данные.

        // Хранимые данные в хеш-таблице в виде пар Хеш-Значение.
        public IReadOnlyCollection<KeyValuePair<int, CircularLinkedList<Element>>> Element => _element?.ToList()?.AsReadOnly();

        // Инициализация.
        public UsersHashTable()
        {
            _element = new Dictionary<int, CircularLinkedList<Element>>(Max_Size);
        }

        // Хеш функция - модульная
        public int UsersGetHash(string key)
        {
            int result = 0;
            foreach (char symbol in key) // сумма символов UNICODE
                result += symbol;

            var hash = result % 10;
            return hash;
        }

        // Проверка входных данных.
        public int UsersProverka(string value, string key)
        {
            int n = 0;
            int probel = 0;

            for (int i = 0; i < value.Length; i++)
            {
                if ((value[i] < '0' || value[i] > '9') && !(value[i] == ' '))
                    n++;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (probel <= 10)
                {

                    if (n == 0)
                    {
                        if (value[i] == ' ')
                            probel++;

                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    return 4;
                }
            }
            if ((probel < 10) || (probel > 10) || (value[value.Length - 1] == ' '))
            {
                return 4;
            }


            for (int i = 0; i < key.Length; i++)
            {
                if ((key[i] < 'A' || key[i] > 'Z') && (key[i] < 'a' || key[i] > 'z') && (key[i] < '0' || key[i] > '9'))
                    n++;
            }

            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(key) || n > 0)
                return 1;

            var elem = new Element(value, key);
            var hash = UsersGetHash(elem.Key);

            //// Если таблица не содержит такого хеша, то возвращаем null.
            //if (!_element.ContainsKey(hash))
            //    return 2;

            //// Получаем список с таким же хешем ключа.
            //// Если он не пустой, значит значения с таким хешем уже существуют, добавляем элемент
            //// Иначе создаем новый

            CircularLinkedList<Element> NewHashTableElement = null;
            if (_element.ContainsKey(hash))
            {
                // Получаем элемент хеш-таблицы.
                NewHashTableElement = _element[hash];

                // Проверяем наличие внутри списка значения с полученным ключом (название).
                // Если такой элемент найден, то сообщаем об ошибке.
                var oldElementWithKey = NewHashTableElement.SingleOrDefault(i => i.Key == elem.Key);
                if (oldElementWithKey != null) return 3;
            }
            return 0;
        }

        // Добавление данных в хеш-таблицу.
        public void UsersInsert(string value, string key)
        {
            var elem = new Element(value, key);
            var hash = UsersGetHash(elem.Key);

            // Получаем список с таким же хешем ключа.
            // Если он не пустой, значит значения с таким хешем уже существуют, добавляем элемент
            // Иначе создаем новый

            CircularLinkedList<Element> hashTableElement = null;

            if (_element.ContainsKey(hash))
            {
                hashTableElement = _element[hash];
                var oldElementWithKey = hashTableElement.SingleOrDefault(i => i.Key == elem.Key);
                if (oldElementWithKey == null)
                    _element[hash].Add(elem);
            }
            else
            {
                // Создаем новый список.
                hashTableElement = new CircularLinkedList<Element> { elem };
                _element.Add(hash, hashTableElement);
            }
        }

        // Удалить данные из хеш-таблицы по ключу.
        public void UsersDelete(string value, string key)
        {
            var hash = UsersGetHash(key);
            var hashTableElement = _element[hash];

            // Получаем элемент списка по ключу.
            foreach (var val in hashTableElement)
            {
                if (val.Key == key)
                {
                    var elem = val;
                    hashTableElement.Remove(elem);
                    break;
                }
            }
        }

        // Поиск значения по ключу
        public int UsersSearch(string value, string key)
        {
            int n = 0; // количество сравнений
            var hash = UsersGetHash(key);

            // Если таблица не содержит такого хеша, то завершаем выполнения метода вернув null.
            if (!_element.ContainsKey(hash))
                return 0;

            var hashTableElement = _element[hash];

            if (hashTableElement != null) // Если хеш найден, то ищем значение в списке по ключу.
                foreach (var val in hashTableElement)
                {
                    n++;
                    if (val.Key == key)
                        return n;
                }
            return 0;
        }

        //public int LoadHashTable()
        //{
        //    string line;
        //    try
        //    {
        //        StreamReader file = new StreamReader("USERS.txt");
        //        int n = System.IO.File.ReadAllLines("USERS.txt").Length; // количество строк в файле

        //        for (int i = 0; i < n; i++)
        //        {
        //            line = file.ReadLine();
        //            string[] read = line.Split('_');
        //            UsersInsert(read[1], read[0]);
        //        }
        //        return 1;
        //    }

        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}

        public void SaveUsersHashTable(UsersHashTable hashTable)
        {
            string file = "USERS2.txt";
            try
            {
                using (StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.Default))
                {
                    foreach (var i in hashTable.Element)
                        foreach (var value in i.Value)
                            sw.WriteLine(value.Key + "-" + value.Value);
                }
            }
            catch (Exception) { }
        }
    }
}
