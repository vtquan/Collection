using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace MyBookLibrary.Collections
{
    public interface IOrderedDictionary : IDictionary
    {
        object this[int index] { get; set; }

        new IDictionaryEnumerator GetEnumerator();

        void Insert(int index, object key, object value);

        void RemoveAt(int index);
    }

    public interface IOrderedDictionary<TKey, TValue> : IOrderedDictionary,
                                                        IDictionary<TKey, TValue>
    {
        new TValue this[int index] { get; set; }

        new int Add(TKey key, TValue value);

        void Insert(int index, TKey key, TValue value);
    }

    public class OrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue>
    {
        private static readonly string KeyTypeName;

        private static readonly string _valueTypeName;

        private static readonly bool _valueTypeIsReferenceType;
        private readonly IEqualityComparer<TKey> _comparer;
        private readonly Int32 _initialCapacity;

        private Dictionary<TKey, TValue> _dictionary;

        private List<KeyValuePair<TKey, TValue>> _list;

        private Object _syncRoot;

        static OrderedDictionary()
        {
            KeyTypeName = typeof (TKey).FullName;
            _valueTypeName = typeof (TValue).FullName;
            _valueTypeIsReferenceType =
                !typeof (ValueType).GetTypeInfo().IsAssignableFrom(typeof (TValue).GetTypeInfo());
        }

        public OrderedDictionary()
            : this(0, null)
        {
        }

        public OrderedDictionary(Int32 capacity)
            : this(capacity, null)
        {
        }

        public OrderedDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer)
        {
        }

        public OrderedDictionary(Int32 capacity, IEqualityComparer<TKey> comparer)
        {
            if (0 > capacity)
            {
                throw new ArgumentOutOfRangeException("capacity", "'capacity' must be non-negative");
            }
            _initialCapacity = capacity;
            _comparer = comparer;
        }

        private Dictionary<TKey, TValue> Dictionary
        {
            get { return _dictionary ?? (_dictionary = new Dictionary<TKey, TValue>(_initialCapacity, _comparer)); }
        }

        private List<KeyValuePair<TKey, TValue>> List
        {
            get { return _list ?? (_list = new List<KeyValuePair<TKey, TValue>>(_initialCapacity)); }
        }

        public Int32 Count
        {
            get { return List.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public TValue this[int index]
        {
            get
            {
                KeyValuePair<TKey, TValue> item = List[index];
                return item.Value;
            }
            set
            {
                if (index >= Count || index < 0)
                {
                    throw new ArgumentOutOfRangeException("index",
                                                          "'index' must be non-negative and less than the size of the collection");
                }
                KeyValuePair<TKey, TValue> item = List[index];
                TKey key = item.Key;
                List[index] = new KeyValuePair<TKey, TValue>(key, value);
                Dictionary[key] = value;
            }
        }

        public TValue this[TKey key]
        {
            get { return Dictionary[key]; }
            set
            {
                if (!Dictionary.ContainsKey(key))
                {
                    Add(key, value);
                    return;
                }
                Dictionary[key] = value;
                List[IndexOfKey(key)] = new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        public ICollection<TKey> Keys
        {
            get { return Dictionary.Keys; }
        }

        Object IOrderedDictionary.this[int index]
        {
            get { return this[index]; }
            set { this[index] = ConvertToValueType(value); }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        Object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange(ref _syncRoot, new Object(), null);
                }
                return _syncRoot;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get { return false; }
        }

        Object IDictionary.this[Object key]
        {
            get { return this[ConvertToKeyType(key)]; }
            set { this[ConvertToKeyType(key)] = ConvertToValueType(value); }
        }

        ICollection IDictionary.Keys
        {
            get { return (ICollection) Keys; }
        }

        ICollection IDictionary.Values
        {
            get { return (ICollection) Values; }
        }

        public ICollection<TValue> Values
        {
            get { return Dictionary.Values; }
        }

        public Int32 Add(TKey key, TValue value)
        {
            Dictionary.Add(key, value);
            List.Add(new KeyValuePair<TKey, TValue>(key, value));
            return Count - 1;
        }

        public void Clear()
        {
            Dictionary.Clear();
            List.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            return Dictionary.ContainsKey(key);
        }

        public void Insert(Int32 index, TKey key, TValue value)
        {
            if (index > Count || index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            Dictionary.Add(key, value);
            List.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
        }

        IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        void IOrderedDictionary.Insert(Int32 index, Object key, Object value)
        {
            Insert(index, ConvertToKeyType(key), ConvertToValueType(value));
        }

        public bool Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            Int32 num = IndexOfKey(key);
            if (num < 0 || !Dictionary.Remove(key))
            {
                return false;
            }
            List.RemoveAt(num);
            return true;
        }

        public void RemoveAt(Int32 index)
        {
            if (index >= Count || index < 0)
            {
                throw new ArgumentOutOfRangeException("index",
                                                      "'index' must be non-negative and less than the size of the collection");
            }
            KeyValuePair<TKey, TValue> item = List[index];
            TKey key = item.Key;
            List.RemoveAt(index);
            Dictionary.Remove(key);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>) Dictionary).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, Int32 arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>) Dictionary).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            Add(key, value);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        void ICollection.CopyTo(Array array, Int32 index)
        {
            ((ICollection) List).CopyTo(array, index);
        }

        void IDictionary.Add(Object key, Object value)
        {
            Add(ConvertToKeyType(key), ConvertToValueType(value));
        }

        bool IDictionary.Contains(Object key)
        {
            return ContainsKey(ConvertToKeyType(key));
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        void IDictionary.Remove(Object key)
        {
            Remove(ConvertToKeyType(key));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return Dictionary.TryGetValue(key, out value);
        }

        private static TKey ConvertToKeyType(Object keyObject)
        {
            if (keyObject == null)
            {
                throw new ArgumentNullException("key");
            }
            if (!(keyObject is TKey))
            {
                throw new ArgumentException(string.Concat("'key' must be of type ", KeyTypeName), "key");
            }
            return (TKey) keyObject;
        }

        private static TValue ConvertToValueType(Object value)
        {
            if (value == null)
            {
                if (!_valueTypeIsReferenceType)
                {
                    throw new ArgumentNullException("value");
                }
                TValue tValue = default(TValue);
                return tValue;
            }
            if (!(value is TValue))
            {
                throw new ArgumentException(string.Concat("'value' must be of type ", _valueTypeName), "value");
            }
            return (TValue) value;
        }

        public int IndexOfKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            for (Int32 i = 0; i < List.Count; i++)
            {
                KeyValuePair<TKey, TValue> item = List[i];
                TKey tKey = item.Key;
                if (_comparer == null)
                {
                    if (tKey.Equals(key))
                    {
                        return i;
                    }
                }
                else
                {
                    if (_comparer.Equals(tKey, key))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}