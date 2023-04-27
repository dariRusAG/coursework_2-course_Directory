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
    public class Elem_Of_Measure
    {
        //данные справочника
        public string Value
        {
            get;//аксессор, который используется для чтения значения из внутреннего поля класса;
            set;//аксессор, используемый для записи значения во внутреннее поле класса.
        }
        public string Key
        {
            get;//аксессор, который используется для чтения значения из внутреннего поля класса;
            set;//аксессор, используемый для записи значения во внутреннее поле класса.
        }

        //создаем новый экземпляр хранимых данных
        public Elem_Of_Measure(string val, string key) //ПОЧЕМУ КЛЮЧ STRING, А НЕ INT??
        {
            {           //Проверка на пустоту
                if (string.IsNullOrEmpty(val))
                    throw new ArgumentNullException(nameof(val));
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException(nameof(key));

                Value = val;
                Key = key;

            }
        }
        public class Node<T>
        {
            public T Data
            {
                get;
                set;
            }
            public Node<T> Next
            {
                get;
                set;
            }
            public Node(T data)
            {
                Data = data;
            }
        }
        public class Singly_Linked_List<T> : IEnumerable<T> //односвязный список
        {
            Node<T> Head; //головной элемент
            Node<T> tail;
            int count; //количество элементов списка

            //метод добавления
            public void Add(T data)
            {
                Node<T> node = new Node<T>(data);
                //при пустом списке
                if (Head == null)
                {
                    Head = node;
                }
                else
                {
                    tail.Next = node;
                }
                tail = node;
                count++;
            }
            //удаление
            public bool Remove(T data)
            {
                Node<T> current = Head;
                Node<T> previous = null;

                while (current != null)
                {
                    if (current.Data.Equals(data))
                    {
                        if (previous != null)//если узел в середине или в конце
                        {
                            // убираем узел current, теперь previous ссылается не на current, а на current.Next
                            previous.Next = current.Next;
                            // Если current.Next не установлен, значит узел последний,
                            // изменяем переменную tail
                            if (current.Next == null)
                            {
                                tail = previous;
                            }
                        }
                        else
                        {
                            // если удаляется первый элемент
                            // переустанавливаем значение head
                            Head = Head.Next;
                            // если после удаления список пуст, сбрасываем tail
                            if (Head == null)
                            {
                                tail = null;
                            }
                        }
                        count--;
                        return true;
                    }
                    previous = current;
                    current = current.Next;
                }

                return false;
            }
            public int Count
            {
                get { return count; }
            }
            public bool isEmpty
            {
                get { return count == 0; }
            }

            //очистка списка
            public void Clear()
            {
                Head = null;
                tail = null;
                count = 0;
            }
            //проверка на содержание элементов
            public bool Contains(T data)
            {
                Node<T> current = Head;
                while (current != null)
                {
                    if (current.Data.Equals(data))
                    {
                        return true;
                    }
                    current = current.Next;
                }
                return false;
            }

            //добавление в начало
            public void push(T data)
            {
                Node<T> node = new Node<T>(data);
                node.Next = Head;
                Head = node;
                if (count == 0)
                {
                    tail = Head;
                }
                count++;

            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this).GetEnumerator();
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                Node<T> current = Head;
                while (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
        }
    }
    public class Measures_Hash_Table
    {
        public readonly byte Max_size = 10; //максимальная длина поля
        public Dictionary<int, CircularLinkedList<Element>> _element = null;//хранимые данные

        //Хранимые данные в хеш-тфблицу в виде пар хеш--значение
        public IReadOnlyCollection<KeyValuePair<int, CircularLinkedList<Element>>> Element => _element?.ToList()?.AsReadOnly();

        //инициализация
        public Measures_Hash_Table()
        {
            _element = new Dictionary<int, CircularLinkedList<Element>>(Max_size);
        }


        //мультипликативная хеш-функция
        public int Measure_Hash_Function(string key)
        {
            int N = 16;
            double E = 0.618033;
            int H;
            H = N * (int)(key.Length * E % 1);
            return H;
        }

        //проверка на вводимые символы
        public int Check(string value, string key)
        {
            int n = 0;
            int probel = 0;

            for (int i = 0; i < value.Length; i++)
            {
                if ((value[i] > '0' || value[i] < '9') && !(value[i] == ' '))
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
                if ((key[i] > 'A' || key[i] < 'Z') && (key[i] > 'a' || key[i] < 'z') && (key[i] > '0' || key[i] < '9'))
                    n++;
            }

            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(key) || n > 0)
                return 1;

            var elem = new Element(value, key);
            var hash = Measure_Hash_Function(elem.Key);

            // Если таблица не содержит такого хеша, то возвращаем null.
            if (!_element.ContainsKey(hash))
                return 2;

            // Получаем список с таким же хешем ключа.
            // Если он не пустой, значит значения с таким хешем уже существуют, добавляем элемент
            // Иначе создаем новый

            CircularLinkedList<Element> NewHashTableElement = null;
            if (_element.ContainsKey(hash))
            {
                // Получаем элемент хеш-таблицы.
                NewHashTableElement = _element[hash];

                // Проверяем наличие внутри списка значения с полученным ключом (название).
                // Если такой элемент найден, то сообщаем об ошибке.
                var oldElementWithKey = NewHashTableElement.SingleOrDefault(i => i.Value == elem.Value);
                if (oldElementWithKey != null) return 3;
            }
            return 0;
        }
        //вставка элемента в таблицу
        public void Measure_Insert(string value, string key)
        {
            var elem = new Element(value, key);
            var hash = Measure_Hash_Function(elem.Key);

            // Получаем список с таким же хешем ключа.
            // Если он не пустой, значит значения с таким хешем уже существуют, добавляем элемент
            // Иначе создаем новый

            CircularLinkedList<Element> hashTableElement = null;

            if (_element.ContainsKey(hash))
            {
                hashTableElement = _element[hash];
                var oldElementWithKey = hashTableElement.SingleOrDefault(i => i.Value == elem.Value);
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
        public void Measer_Delete(string value, string key)
        {
            var hash = Measure_Hash_Function(key);
            var hashTableElement = _element[hash];

            // Получаем элемент списка по ключу.
            foreach (var val in hashTableElement)
            {
                if (val.Value == value)
                {
                    var elem = val;
                    hashTableElement.Remove(elem);
                    break;
                }
            }
        }

        // Поиск значения по ключу
        public int Measer_Search(string value, string key)
        {
            int n = 0; // количество сравнений
            var hash = Measure_Hash_Function(key);

            // Если таблица не содержит такого хеша, то завершаем выполнения метода вернув null.
            if (!_element.ContainsKey(hash)) return 0;

            var hashTableElement = _element[hash];

            if (hashTableElement != null) // Если хеш найден, то ищем значение в списке по ключу.
                foreach (var val in hashTableElement)
                {
                    n++;
                    if (val.Value == value) return n;
                }
            return 0;
        }

        public int LoadHashTable()
        {
            string line;
            try
            {
                StreamReader file = new StreamReader("MEASURES.txt");
                int n = System.IO.File.ReadAllLines("MEASURES.txt").Length; // количество строк в файле

                for (int i = 0; i < n; i++)
                {
                    line = file.ReadLine();
                    string[] read = line.Split('_');
                    Measure_Insert(read[0], read[1]);
                }
                return 1;
            }

            catch (Exception)
            {
                return 0;
            }
        }
    }
}

