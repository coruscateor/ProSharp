using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace ProSharp
{
    
    public sealed class ProObject : DynamicObject, IDictionary<string, object>, IEnumerable<KeyValuePair<string, object>>
    {

        private ProObject myPrototype;

        private Dictionary<string, object> myProperties = new Dictionary<string, object>();

        public ProObject()
        {
        }

        public ProObject(ProObject ThePrototype)
        {

            myPrototype = ThePrototype;

        }

        public ProObject(IDictionary<string, object> TheItems)
        {

            foreach(var Item in TheItems)
            {

                if(!myProperties.ContainsKey(Item.Key))
                    myProperties.Add(Item.Key, Item.Value);

            }

        }

        public ProObject(Dictionary<string, object> TheItems)
        {

            foreach(var Item in TheItems)
            {

                myProperties.Add(Item.Key, Item.Value);

            }

        }

        public ProObject(ConcurrentDictionary<string, object> TheItems)
        {

            foreach(var Item in TheItems)
            {

                myProperties.Add(Item.Key, Item.Value);

            }

        }

        public ProObject Prototype
        {

            get
            {

                return myPrototype;

            }

        }

        public bool TryGetProtoType(out ProObject TheProtoType)
        {

            TheProtoType = myPrototype;

            return TheProtoType != null;

        }

        public bool TryGetProtoType(Action<ProObject> TheProtoTypeAction)
        {

            if(myPrototype != null)
            {

                TheProtoTypeAction(myPrototype);

                return true;

            }

            return false;

        }

        public bool Contains(string BinderName)
        {

            if(!myProperties.ContainsKey(BinderName))
            {

                if(myPrototype != null)
                    return myPrototype.Contains(BinderName);

            }

            return false;

        }

        private bool IsPrototype(string BinderName)
        {

            return "Prototype" == BinderName;

        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {

            return TrySetObjectMemeber(binder.Name, value);

        }

        public bool TrySetObjectMemeber(string BinderName, object value)
        {

            myProperties[BinderName] = value;

            return true;

        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {

            object CurrentResult;

            if(TryGetObjectMember(binder.Name, out CurrentResult))
            {

                result = CurrentResult;

            }

            result = null;

            return false;

        }

        public bool TryGetObjectMember(string BinderName, out object result)
        {

            if(myProperties.ContainsKey(BinderName))
            {

                result = myProperties[BinderName];

                return true;

            }
            else if(myPrototype != null)
            {

                ProObject CurrentPrototype = myPrototype;

                while(CurrentPrototype != null)
                {

                    object TGMResult;

                    if(CurrentPrototype.TryGetObjectMember(BinderName, out TGMResult))
                    {

                        myProperties.Add(BinderName, TGMResult);

                        result = TGMResult;

                        return true;

                    }

                    CurrentPrototype = CurrentPrototype.Prototype;

                }

            }

            if(IsPrototype(BinderName))
            {

                if(myPrototype != null)
                {

                    result = myPrototype;

                    return true;

                }

            }

            result = null;

            return false;

        }

        private bool TrySetStringIndex(object[] indexes, object value)
        {

            object Item = indexes[0];

            string IndexMemberName;

            if(Item == null)
            {

                IndexMemberName = "";

            }
            else
            {

                IndexMemberName = Item.ToString();

            }

            return TrySetObjectMemeber(IndexMemberName, value);

        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {

            if(indexes.Length != 1)
                throw new Exception("Indexes must have one value");

            return TrySetStringIndex(indexes, value);

        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {

            object Item = indexes[0];

            string IndexMemberName;

            if(Item == null)
            {

                IndexMemberName = "";

            }
            else
            {

                IndexMemberName = Item.ToString();

            }

            object CurrentResult;

            if(TryGetObjectMember(IndexMemberName, out CurrentResult))
            {

                result = CurrentResult;

                return true;

            }

            result = null;

            return false;

        }

        public ProObject GetFlattend()
        {

            ProObject NewObject = new ProObject();

            foreach(var Item in myProperties)
            {

                NewObject.Add(Item.Key, Item.Value);

            }

            ProObject CurrentPrototype = myPrototype;

            while(CurrentPrototype != null)
            {

                foreach(var Item in CurrentPrototype)
                {

                    string CurrentKey = Item.Key;

                    if(!NewObject.Contains(CurrentKey))
                        NewObject.Add(CurrentKey, Item.Value);

                }

                CurrentPrototype = CurrentPrototype.Prototype;

            }

            return NewObject;

        }

        public ProObject GetDerived()
        {

            return new ProObject(this);

        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {

            return myProperties.GetEnumerator();

        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {

            return GetEnumerator();

        }

        public void Add(string key, object value)
        {
            
            myProperties.Add(key, value);

        }

        public bool ContainsKey(string key)
        {
            
            return myProperties.ContainsKey(key);

        }

        public ICollection<string> Keys
        {

            get
            {

                return myProperties.Keys;

            }

        }

        public bool Remove(string key)
        {
            
            return myProperties.Remove(key);

        }

        public bool TryGetValue(string key, out object value)
        {

            object CurrentValue;

            if(myProperties.TryGetValue(key, out CurrentValue))
            {

                value = CurrentValue;

                return true;

            }

            value = null;

            return false;

        }

        public ICollection<object> Values
        {

            get
            {

                return myProperties.Values;

            }

        }

        public object this[string key]
        {

            get
            {

                return myProperties[key];

            }
            set
            {

                myProperties[key] = value;

            }

        }

        public void Add(KeyValuePair<string, object> item)
        {

            myProperties.Add(item.Key, item.Value);

        }

        public void Clear()
        {

            myProperties.Clear();

        }

        public bool Contains(KeyValuePair<string, object> item)
        {

            return myProperties.Contains(item);

        }
        
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {

            throw new NotSupportedException();

        }

        public int Count
        {

            get
            {

                return myProperties.Count;

            }

        }

        public bool IsReadOnly
        {

            get
            {

                return false;

            }

        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            
            if(myProperties.Contains(item))
            {

                return myProperties.Remove(item.Key);

            }

            return false;
            
        }

    }

}
