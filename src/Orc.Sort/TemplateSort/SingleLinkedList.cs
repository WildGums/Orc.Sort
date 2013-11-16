namespace Orc.Sort.TemplateSort
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a generic single linked list.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    public class SingleLinkedList<T> : ICollection<T>
    {
        int count;
        Node first;
        Node last;

        /// <summary>
        /// Initializes a new instance of the SingleLinkedList class.
        /// </summary>
        public SingleLinkedList()
        {
            count = 0;
            first = null;
            last = null;
        }

        /// <summary>
        /// Add an object to the end of the list.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (first == null)
            {
                first = last = new Node(item);
            }
            else
            {
                last.Next = new Node(item);
                last = last.Next;
            }
            count++;
        }

        /// <summary>
        /// Concatinates two lists, effectively moving all items from the specified list to the current one. The other list will be empty after concatinating.
        /// </summary>
        /// <param name="other">The list to concatinate with this list.</param>
        public void Concat(SingleLinkedList<T> other)
        {
            if (other.first == null)
                return;

            if (first == null)
            {
                first = other.first;
                last = other.last;
                count = other.count;
            }
            else
            {
                last.Next = other.first;
                last = other.last;
                count += other.count;
            }

            other.first = null;
            other.last = null;
            other.count = 0;
        }

        /// <summary>
        /// Clears the list from all items.
        /// </summary>
        public void Clear()
        {
            count = 0;
            first = null;
            last = null;
        }

        /// <summary>
        /// Gets whether the specified item exists in the list.
        /// </summary>
        /// <param name="item">The item to look for.</param>
        /// <returns>Returns whether the item existed.</returns>
        public bool Contains(T item)
        {
            Node current = first;
            while (current != null)
            {
                if (current.Element.Equals(item))
                    return true;
                current = current.Next;
            }
            return false;
        }

        /// <summary>
        /// Copies the content of this list to the target array starting at the specified position.
        /// </summary>
        /// <param name="array">The array which to copy to.</param>
        /// <param name="arrayIndex">A zero-based index in the target array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (T item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        /// <summary>
        /// Gets the number of items in the list.
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// Gets whether this collection is readonly.
        /// </summary>
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified item from the list if it exists.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>Returns whether or not the item what successfully removed.</returns>
        public bool Remove(T item)
        {
            if (first == null)
                return false;

            if (first.Element.Equals(item))
            {
                if (first == last)
                {
                    count = 0;
                    first = null;
                    last = null;
                }
                else
                {
                    first = first.Next;
                    count--;
                }
                return true;
            }

            Node current = first;
            while (current.Next != null)
            {
                if (current.Next.Element.Equals(item))
                {
                    if (current.Next == last)
                        last = current;
                    current.Next = current.Next.Next;
                    count--;
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        /// <summary>
        /// Gets an enumerator used to iterate through the list.
        /// </summary>
        /// <returns>Returns an iterator for the list.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new SingleLinkedListIterator(this);
        }

        /// <summary>
        /// Gets an enumerator used to iterate through the list.
        /// </summary>
        /// <returns>Returns an iterator for the list.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new SingleLinkedListIterator(this);
        }

        /// <summary>
        /// Supports iteration over a SingleLinkedList. 
        /// </summary>
        public class SingleLinkedListIterator : IEnumerator<T>
        {
            SingleLinkedList<T> list;
            Node current;
            bool started;

            public SingleLinkedListIterator(SingleLinkedList<T> list)
            {
                this.list = list;
                this.current = null;
                this.started = false;
            }

            public T Current
            {
                get { return current.Element; }
            }

            public bool MoveNext()
            {
                if (started)
                {
                    if (current == null)
                        return false;
                    current = current.Next;
                }
                else
                {
                    current = list.first;
                    started = true;
                }
                return current != null;
            }

            public void Reset()
            {
                this.current = null;
                this.started = false;
            }

            void IDisposable.Dispose()
            {
            }

            object System.Collections.IEnumerator.Current
            {
                get { return current.Element; }
            }
        }

        private class Node
        {
            public T Element;
            public Node Next;

            public Node(T element)
            {
                this.Element = element;
                this.Next = null;
            }

            public Node(T element, Node next)
            {
                this.Element = element;
                this.Next = next;
            }
        }
    }
}
