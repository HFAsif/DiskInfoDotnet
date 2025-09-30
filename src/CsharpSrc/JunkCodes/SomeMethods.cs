namespace JunkCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class SomeMethods
{
    public static T CastTo<T>(object obj) => (T)obj;
    public static bool IsObjectOfType(object obj, Type type, StringComparison comparisonType, bool exactMatch)
    {
        if (exactMatch)
        {
            return string.Equals(obj.GetType().FullName, type.FullName, comparisonType);
        }
        else
        {
            return obj.GetType().FullName != null && obj.GetType().FullName.IndexOf(type.FullName, comparisonType) >= 0;
        }
    }

    public static bool IsObjectOfType(object obj, Type type, StringComparison comparisonType)
    {
        return string.Equals(obj.GetType().FullName, type.FullName, comparisonType);
    }

    public static bool IsObjectOfType(object obj, string typeFullName, StringComparison comparisonType)
    {
        return string.Equals(obj.GetType().FullName, typeFullName, comparisonType);
    }

    public static bool IsObjectOfType(object obj, string typeFullName, StringComparison comparisonType, bool exactMatch)
    {
        if (exactMatch)
        {
            return string.Equals(obj.GetType().FullName, typeFullName, comparisonType);
        }
        else
        {
            return obj.GetType().FullName != null && obj.GetType().FullName.IndexOf(typeFullName, comparisonType) >= 0;
        }
    }

    public static bool IsObjectOfType(object obj, string typeFullName, bool exactMatch)
    {
        if (exactMatch)
        {
            return obj.GetType().FullName == typeFullName;
        }
        else
        {
            return obj.GetType().FullName != null && obj.GetType().FullName.Contains(typeFullName);
        }
    }

    public static bool IsObjectOfType(object obj, Type type, bool exactMatch)
    {
        if (exactMatch)
        {
            return obj.GetType() == type;
        }
        else
        {
            return type.IsInstanceOfType(obj);
        }
    }

    public static bool IsObjectOfType(object obj, string typeFullName)
    {
        return obj.GetType().FullName == typeFullName;
    }

    public static bool IsObjectOfType(object obj, Type type)
    {
        return type.IsInstanceOfType(obj);
    }

    public static bool IsObjectOfType<T>(object obj)
    {
        return obj is T;
    }
    public static T? TryCastObjectNullable<T>(object obj) where T : struct
    {
        if (obj is T t)
        {
            return t;
        }
        else
        {
            return null;
        }
    }

    public static bool TryCastObjectNullable<T>(object obj, out T? result) where T : struct
    {
        if (obj is T t)
        {
            result = t;
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }

    public static T TryCastObject<T>(object obj) where T : class
    {
        if (obj is T t)
        {
            return t;
        }
        else
        {
            return null;
        }
    }

    public static bool TryCastObject<T>(object obj, out T result)
    {
        if (obj is T t)
        {
            result = t;
            return true;
        }
        else
        {
            result = default!;
            return false;
        }
    }

    public static void CastObject<T>(object obj, out T result)
    {
        if (obj is T t)
        {
            result = t;
        }
        else
        {
            throw new InvalidCastException($"Cannot cast object of type {obj.GetType()} to type {typeof(T)}");
        }
    }

    public static T CastObject<T>(object obj)
    {
        if (obj is T t)
        {
            return t;
        }
        else
        {
            throw new InvalidCastException($"Cannot cast object of type {obj.GetType()} to type {typeof(T)}");
        }
    }
    /// <summary>
    public static List<T> CastListObjects<T>(IEnumerable<object> objects)
    {
        List<T> result = new List<T>();
        foreach (var obj in objects)
        {
            if (obj is T t)
            {
                result.Add(t);
            }
            else
            {
                throw new InvalidCastException($"Cannot cast object of type {obj.GetType()} to type {typeof(T)}");
            }
        }
        return result;
    }
}
