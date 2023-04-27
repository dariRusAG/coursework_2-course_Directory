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
    // Элемент данных хеш-таблицы.
    public class Element
    {                                                   
        public string Value { get; set; } // Хранимые данные.
        public string Key { get; set; }

        // Создание нового экземпляря хранимых данных Element.
        // key - ключ, value - значение
        public Element(string value, string key)
        {
            // Проверяем входные данные на корректность.
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            Value = value; // название
            Key = key; // категория
        }
        
    }

    // Список 
    public class DoublyNode<T>
    {

        public DoublyNode(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
        public DoublyNode<T> Previous { get; set; }
        public DoublyNode<T> Next { get; set; }
    }
    public class CircularDoublyLinkedList<T> : IEnumerable<T>  // Кольцевой двусвязный список
    {
        DoublyNode<T> head; // Головной/первый элемент
        int count;  // Количество элементов в списке

        // Добавление элемента
        public void Add(T data)
        {
            DoublyNode<T> node = new DoublyNode<T>(data);

            if (head == null)
            {
                head = node;
                head.Next = node;
                head.Previous = node;
            }
            else
            {
                node.Previous = head.Previous;
                node.Next = head;
                head.Previous.Next = node;
                head.Previous = node;
            }
            count++;
        }

        // Удаление элемента
        public bool Remove(T data)
        {
            DoublyNode<T> current = head;
            DoublyNode<T> removedElement = null;

            if (count == 0) return false;

            // Поиск удаляемого узла
            do
            {
                if (current.Data.Equals(data))
                {
                    removedElement = current;
                    break;
                }
                current = current.Next;
            }
            while (current != head);

            if (removedElement != null)
            {
                // Если удаляется единственный элемент списка
                if (count == 1) head = null;
                else
                {
                    // Если удаляется первый элемент
                    if (removedElement == head) head = head.Next;

                    removedElement.Previous.Next = removedElement.Next;
                    removedElement.Next.Previous = removedElement.Previous;
                }
                count--;
                return true;
            }
            return false;
        }
        public int Count { get { return count; } }
        public bool IsEmpty { get { return count == 0; } }

        public void Clear()
        {
            head = null;
            count = 0;
        }

        public bool Contains(T data)
        {
            DoublyNode<T> current = head;
            if (current == null) return false;
            do
            {
                if (current.Data.Equals(data)) return true;
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
            DoublyNode<T> current = head;
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

    // Хеш-таблица.
    public class CategoryHashTable
    {
        public readonly byte Max_Size = 30; // Максимальная длина ключевого поля.
        public Dictionary<int, CircularDoublyLinkedList<Element>> _element = null;  // Хранимые данные.

        // Хранимые данные в хеш-таблице в виде пар Хеш-Значение.
        public IReadOnlyCollection<KeyValuePair<int, CircularDoublyLinkedList<Element>>> Element => _element?.ToList()?.AsReadOnly();

        // Инициализация.
        public CategoryHashTable()
        {
            _element = new Dictionary<int, CircularDoublyLinkedList<Element>>(Max_Size);
        }

        // Проверка входных данных.
        public int Proverka(string value, string key)
        {
            int n = 0;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] < 'А' || value[i] > 'Я') n++;
                if (value[i] == ' ') n--;
                if (value[i] == 'Ё') n--;
            }

            for (int i = 0; i < key.Length; i++)
            {
                if (key[i] < 'А' || key[i] > 'Я') n++;
                if (key[i] == 'Ё') n--;
            }

            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(key) || n > 0) return 1;

            var elem = new Element(value, key);
            var hash = GetHash(elem.Key, elem.Value);

            //// Если таблица не содержит такого хеша, то возвращаем null.
            //if (!_element.ContainsKey(hash)) return 2;

            //// Получаем список с таким же хешем ключа.
            //// Если он не пустой, значит значения с таким хешем уже существуют, добавляем элемент
            //// Иначе создаем новый

            CircularDoublyLinkedList<Element> hashTableElement = null;
            if (_element.ContainsKey(hash))
            {
                // Получаем элемент хеш-таблицы.
                hashTableElement = _element[hash];

                // Проверяем наличие внутри списка значения с полученным ключом (название).
                // Если такой элемент найден, то сообщаем об ошибке.
                var oldElementWithKey = hashTableElement.SingleOrDefault(i => i.Value == elem.Value);
                if (oldElementWithKey != null) return 3;
            }
            return 0;
        }

        // Добавление данных в хеш-таблицу.
        public void Insert(string value, string key)
        {
            var elem = new Element(value, key);
            var hash = GetHash(elem.Key, elem.Value);

            // Получаем список с таким же хешем ключа.
            // Если он не пустой, значит значения с таким хешем уже существуют, добавляем элемент
            // Иначе создаем новый

            CircularDoublyLinkedList<Element> hashTableElement = null;
            if (_element.ContainsKey(hash))
            {
                hashTableElement = _element[hash];
                var oldElementWithKey = hashTableElement.SingleOrDefault(i => i.Value == elem.Value);
                if (oldElementWithKey == null) _element[hash].Add(elem);
            }
            else
            {
                // Создаем новый список.
                hashTableElement = new CircularDoublyLinkedList<Element> { elem };
                _element.Add(hash, hashTableElement);
            }
        }

        // Удалить данные из хеш-таблицы по ключу.
        public void Delete(string value, string key)
        {
            var hash = GetHash(value, key);
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
        public int Search(string value, string key)
        {
            int n = 0; // количество сравнений
            var hash = GetHash(value, key);

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

        // Хеш функция - свертка
        // Складывает цифры длины строки
        // Возвращает остаток от деления на размер хеш-таблицы
        public int GetHash(string value, string key)
        {
            int result = 0;
            string ValKey = value.ToString() + key.ToString();

            foreach (char symbol in ValKey) // сумма символов UNICODE
                result += symbol;

            int cel = 0;

            while (result != 0)
            {
                cel = cel + result % 10;
                result /= 10;
            }
            return cel % 10;
        }


        public void SaveCategoryHashTable(CategoryHashTable hashTable)
        {
            string file = "CATEGORY2.txt";
            try
            {
                using (StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.Default))
                {
                    foreach (var i in hashTable.Element)
                        foreach (var value in i.Value)
                            sw.WriteLine(value.Key + "_" + value.Value);
                }
            }
            catch (Exception) { }
        }
    }
}