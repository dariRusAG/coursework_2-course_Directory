using System.Windows.Forms;

namespace Directory
{
    public class KnotCt
    {
        public string Key;
        public CircularDoublyLinkedList<DataGridViewRow> DataList = new CircularDoublyLinkedList<DataGridViewRow>();

        public KnotCt()
        {
            Key = "";
            DataList = null;
        }

        public KnotCt(string key, CircularDoublyLinkedList<DataGridViewRow> Dlist)
        {
            Key = key;
            DataList = Dlist;
        }
    }

    public class BinaryTreeCategory
    {
        private KnotCt data;
        private BinaryTreeCategory left, right;
        public BinaryTreeCategory()
        {
            KnotCt R = new KnotCt();
            this.data = R;
            this.left = null;
            this.right = null;

        }
        public BinaryTreeCategory(string val, DataGridViewRow str)
        {
            KnotCt R = new KnotCt();
            CircularDoublyLinkedList<DataGridViewRow> DList = new CircularDoublyLinkedList<DataGridViewRow>();
            this.data = R;
            data.DataList = DList;
            this.data.Key = val;
            this.data.DataList.Add(str);
            this.left = null;
            this.right = null;

        }
        public void Add(string val, DataGridViewRow str)
        {
            if (data.Key == "")
            {
                CircularDoublyLinkedList<DataGridViewRow> DList = new CircularDoublyLinkedList<DataGridViewRow>();
                data.Key = val;
                data.DataList = DList;
                data.DataList.Add(str);
            }
            else
            {
                if (val.CompareTo(data.Key) < 0)
                {
                    if (left == null) left = new BinaryTreeCategory(val, str);
                    else left.Add(val, str);
                }

                else if (val.CompareTo(data.Key) > 0)
                {
                    if (right == null) right = new BinaryTreeCategory(val, str);
                    else right.Add(val, str);
                }
                else if (val.CompareTo(data.Key) == 0) data.DataList.Add(str);
            }
        }

        // Выводит значения полей узлов дерева на экран с учётом связей
        // Формальные параметры: пусто
        // Входные данные: дерево
        // Выходные данные: значения узлов дерева в порядке прямого обхода
        public string Info()
        {
            string result = string.Empty;

            if (this.data.Key == "") result = "Дерево пусто.\n";
            else result += Info(this);

            return result;
        }

        // Выводит значения полей узлов поддерева на экран с учётом связей
        // Формальные параметры: узел-корень поддерева
        // Входные данные: дерево
        // Выходные данные: значения узлов поддерева в порядке прямого обхода КЛП
        private string Info(BinaryTreeCategory current)
        {
            string result = string.Empty;

            if (current != null)
            {
                
                    result += $"Ключ: {current.data.Key}\n";
                    foreach (DataGridViewRow el in current.data.DataList)
                        result += $"Индексы:\n{el}\n";

                    result += Info(current.left);
                    result += Info(current.right);
                
            }
            return result;
        }

        public CircularDoublyLinkedList<DataGridViewRow> Poisk(string val)
        {
            bool isFound = false;
            BinaryTreeCategory cur = new BinaryTreeCategory();
            cur = this;

            while (!isFound)
            {
                if (cur == null)
                {
                    break; //Дерево пустое

                }

                if (val.CompareTo(cur.data.Key) < 0)
                {
                    if (left == null)
                    {
                        return null;
                    }
                    else cur = cur.left;
                }

                else if (val.CompareTo(cur.data.Key) > 0)
                {
                    if (right == null)
                    {
                        return null;
                    }
                    else cur = cur.right;
                }

                else isFound = true;
            }

            if (isFound)
            {
                return cur.data.DataList;
            }
            else return null;
        }


    public int Poisk2(string val)
        {
            int count = 0;
            bool isFound = false;
            BinaryTreeCategory cur = new BinaryTreeCategory();
            cur = this;

            while (!isFound)
            {
                if (cur == null)
                {
                    break; //Дерево пустое
                }

                if (val.CompareTo(cur.data.Key) < 0)
                {
                    count++;
                    if (left == null)
                    {
                        return -1;
                    }
                    else cur = cur.left;
                }

                else if (val.CompareTo(cur.data.Key) > 0)
                {
                    count++;
                    if (right == null)
                    {
                        return -1;
                    }
                    else cur = cur.right;
                }

                else isFound = true;
            }

            if (isFound)
            {

                return count;
            }
            else return -1;
        }


        public KnotCt MinSprPoisk()
        {
            BinaryTreeCategory cur = new BinaryTreeCategory();
            cur = this;
            while (cur.left != null) cur = cur.left;
            return (cur.data);
        }

        public CircularDoublyLinkedList<DataGridViewRow> Delete2(string val)
        {
            CircularDoublyLinkedList<DataGridViewRow> Z = Poisk(val);
            KnotCt cur = new KnotCt();
            cur.Key = val;
            Delete_Uzel(cur);
            return Z;
        }

        public void Delete(string val, DataGridViewRow stroka)
        {
            if (data.Key == "") return; //Элемента нет в дереве

            //элемент находится в левой поддереве?
            if (val.CompareTo(data.Key) < 0)
            {
                if ((left != null) && (val.CompareTo(left.data.Key) != 0)) left.Delete(val, stroka);
                else
                {
                    if ((left != null) && (val.CompareTo(left.data.Key) == 0))
                    {
                        //потомков нет
                        if ((left.left == null) && (left.right == null))
                        {
                            if ((left.data.DataList.Contains(stroka)) && (stroka != null))
                            {
                                left.data.DataList.Remove(stroka);
                                if (left.data.DataList.IsEmpty) left = null;
                            }
                            else if (stroka == null) left = null;
                        }
                        //есть только левый
                        else if ((left.left != null) && (left.right == null))
                        {
                            if ((left.data.DataList.Contains(stroka)) && (stroka != null))
                            {
                                left.data.DataList.Remove(stroka);
                                if (left.data.DataList.IsEmpty) left = left.left;
                            }
                            else if (stroka == null) left = left.left;
                        }

                        else //есть только правый
                        {
                            if ((left.left == null) && (left.right != null))
                            {
                                if ((left.data.DataList.Contains(stroka)) && (stroka != null))
                                {
                                    left.data.DataList.Remove(stroka);
                                    if (left.data.DataList.IsEmpty) left = left.right;
                                }
                                else if (stroka == null) left = left.right;
                            }
                            else //есть оба
                            {
                                if ((left.data.DataList.Contains(stroka)) && (stroka != null))
                                {
                                    left.data.DataList.Remove(stroka);
                                    if (left.data.DataList.IsEmpty)
                                    {
                                        DataGridViewRow p = null;
                                        KnotCt k = left.left.MinSprPoisk();
                                        left.Delete(k.Key, p);
                                        left.data = k;
                                    }
                                }
                                else
                                {
                                    if (stroka == null)
                                    {
                                        DataGridViewRow p = null;
                                        KnotCt k = left.left.MinSprPoisk();
                                        left.Delete(k.Key, p);
                                        left.data = k;
                                    }
                                }
                            }
                        }
                    }
                    else return; //Элемента нет в дереве
                }
            }
            //элемент находится в правом поддереве
            else
            {
                if (val.CompareTo(data.Key) > 0)
                {
                    if ((right != null) && (val.CompareTo(right.data.Key) != 0)) right.Delete(val, stroka);
                    else
                    {
                        if ((right != null) && (val.CompareTo(right.data.Key) == 0))
                        {
                            //потомков нет
                            if ((right.left == null) && (right.right == null))
                            {

                                if ((right.data.DataList.Contains(stroka)) && (stroka != null))
                                {
                                    right.data.DataList.Remove(stroka);
                                    if (right.data.DataList.IsEmpty) right = null;
                                }
                                else if (stroka == null)right = null;
                            }
                            //есть только левый
                            else if ((right.left != null) && (right.right == null))
                            {
                                if ((right.data.DataList.Contains(stroka)) && (stroka != null))
                                {
                                    right.data.DataList.Remove(stroka);
                                    if (right.data.DataList.IsEmpty) right = right.left;
                                }
                                else if (stroka == null) right = right.left;
                            }
                            //есть только правый
                            else
                            {
                                if ((right.left == null) && (right.right != null))
                                {
                                    if ((right.data.DataList.Contains(stroka)) && (stroka != null))
                                    {
                                        right.data.DataList.Remove(stroka);
                                        if (right.data.DataList.IsEmpty) right = right.right;
                                    }
                                    else if (stroka == null) right = right.right;
                                }
                                else //есть оба
                                {
                                    if ((right.data.DataList.Contains(stroka)) && (stroka != null))
                                    {
                                        right.data.DataList.Remove(stroka);
                                        if (right.data.DataList.IsEmpty)
                                        {
                                            DataGridViewRow p = null;
                                            KnotCt k = right.left.MinSprPoisk();
                                            right.Delete(k.Key, p);
                                            right.data = k;
                                        }
                                    }
                                    else
                                    {
                                        if (stroka == null)
                                        {
                                            DataGridViewRow p = null;
                                            KnotCt k = right.left.MinSprPoisk();
                                            right.Delete(k.Key, p);
                                            right.data = k;
                                        }
                                    }
                                }
                            }
                        }
                        else return; //Элемента нет в дереве
                    }
                }
                else
                {
                    if ((val.CompareTo(data.Key) == 0) && (left != null))
                    {
                        if ((data.DataList.Contains(stroka)) && (stroka != null))
                        {
                            data.DataList.Remove(stroka);
                            if (data.DataList == null)
                            {
                                DataGridViewRow p = null;
                                KnotCt k = left.MinSprPoisk();
                                Delete(k.Key, p);
                                data = k;
                            }
                        }
                        else
                        {
                            if (stroka == null)
                            {
                                DataGridViewRow p = null;
                                KnotCt k = left.MinSprPoisk();
                                Delete(k.Key, p);
                                data = k;
                            }
                        }
                    }
                    else
                    {
                        if ((val.CompareTo(data.Key) == 0) && (left == null) && (right != null))
                        {
                            if ((data.DataList.Contains(stroka)) && (stroka != null))
                            {
                                data.DataList.Remove(stroka);
                                if (data.DataList == null)
                                {
                                    data = right.data;
                                    left = right.left;
                                    right = right.right;
                                }
                            }
                            else
                            {
                                if (stroka == null)
                                {
                                    data = right.data;
                                    left = right.left;
                                    right = right.right;
                                }
                            }
                        }
                        else
                        {
                            if ((val.CompareTo(data.Key) == 0) && (left == null) && (right == null))
                            {
                                if ((data.DataList.Contains(stroka)) && (stroka != null))
                                {
                                    data.DataList.Remove(stroka);
                                    if (data.DataList.IsEmpty) data.Key = "";
                                }
                                else if (stroka == null) data.Key = "";
                            }
                        }
                    }
                }
            }
        }

        public void Delete_Uzel(KnotCt val)
        {

            if (data.Key == "") return; //Элемента нет в дереве


            //элемент находится в левой поддереве?
            if (val.Key.CompareTo(data.Key) < 0)
            {

                if ((left != null) && (val.Key.CompareTo(left.data.Key) != 0))
                {
                    left.Delete_Uzel(val);
                }
                else
                {
                    if ((left != null) && (val.Key.CompareTo(left.data.Key) == 0))
                    {
                        //потомков нет
                        if ((left.left == null) && (left.right == null))
                        {

                            left = null;

                        }
                        else
                            //есть только левый
                            if ((left.left != null) && (left.right == null))
                        {
                            left = left.left;
                        }
                        else
                        {//есть только правый
                            if ((left.left == null) && (left.right != null))
                            {
                                left = left.right;
                            }
                            else //есть оба
                            {
                                KnotCt k = left.left.MinSprPoisk();
                                left.Delete_Uzel(k);
                                left.data = k;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            //элемент находится в правом поддереве
            else
            {
                if (val.Key.CompareTo(data.Key) > 0)
                {
                    if ((right != null) && (val.Key.CompareTo(right.data.Key) != 0))
                    {
                        right.Delete_Uzel(val);
                    }
                    else
                    {
                        if ((right != null) && (val.Key.CompareTo(right.data.Key) == 0))
                        {
                            //потомков нет
                            if ((right.left == null) && (right.right == null))
                            {

                                right = null;

                            }
                            else
                                //есть только левый
                                if ((right.left != null) && (right.right == null))
                            {
                                right = right.left;
                            }
                            else
                            {//есть только правый
                                if ((right.left == null) && (right.right != null))
                                {
                                    right = right.right;
                                }
                                else //есть оба
                                {
                                    KnotCt k = right.left.MinSprPoisk();
                                    right.Delete_Uzel(k);
                                    right.data = k;


                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    }

                }
                else
                {
                    if ((val.Key.CompareTo(data.Key) == 0) && (left != null))
                    {
                        KnotCt k = left.MinSprPoisk();
                        Delete_Uzel(k);
                        data = k;
                    }
                    else
                    {
                        if ((val.Key.CompareTo(data.Key) == 0) && (left == null) && (right != null))
                        {
                            data = right.data;
                            data.Key = right.data.Key;
                            left = right.left;
                            right = right.right;


                        }
                        else
                        {
                            if ((val.Key.CompareTo(data.Key) == 0) && (left == null) && (right == null))
                            {
                                data.Key = "";
                            }
                        }
                    }
                }

            }
        }
    }
}