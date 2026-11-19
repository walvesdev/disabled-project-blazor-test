using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

using Sindvamb.Utils.Extension;

namespace Sindvamb.Utils.Extension
{
    /// <summary>
    ///     Métodos de extensão para todos os objetos.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Usado para simplificar e embelezar a conversão de um objeto para um tipo.
        /// </summary>
        /// <typeparam name="T">Tipo a ser convertido</typeparam>
        /// <param name="obj">Objeto a ser lançado</param>
        /// <returns>Objeto convertido</returns>
        public static T As<T>(this object obj) where T : class => (T)obj;

        /// <summary>
        ///     Converte determinado objeto em um tipo de valor usando o método
        ///     <see cref="M:System.Convert.ChangeType(System.Object,System.Type)" />.
        /// </summary>
        /// <param name="obj">Objeto a ser convertido</param>
        /// <typeparam name="T">Tipo do objeto alvo</typeparam>
        /// <returns>Objeto convertido</returns>
        public static T To<T>(this object obj) where T : struct
        {
            if (typeof(T) == typeof(Guid))
                return (T)TypeDescriptor.GetConverter(typeof(T))
                    .ConvertFromInvariantString(obj.ToString() ?? string.Empty);

            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Verifica se um item está em uma lista.
        /// </summary>
        /// <param name="item">Item a verificar</param>
        /// <param name="list">Lista de itens</param>
        /// <typeparam name="T">Tipo dos itens</typeparam>
        public static bool IsIn<T>(this T item, params T[] list) => list.Contains(item);

        /// <summary>
        ///     Verifica se um item está no enumerável fornecido.
        /// </summary>
        /// <param name="item">Item a verificar</param>
        /// <param name="items">Itens</param>
        /// <typeparam name="T">Tipo dos itens</typeparam>
        public static bool IsIn<T>(this T item, IEnumerable<T> items) => items.Contains(item);

        /// <summary>
        ///     Pode ser usado para executar condicionalmente uma função
        ///     em um objeto e returna o objeto modificado ou original.
        ///     É útil para chamadas encadeadas.
        /// </summary>
        /// <param name="obj">Um objeto</param>
        /// <param name="condition">Uma condição</param>
        /// <param name="func">Uma função que é executada somente se a condição for <code>true</code></param>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <returns>
        ///     returna o objeto modificado (pelo <paramref name="func" /> se o <paramref name="condition" /> for <code>true</code>
        ///     )
        ///     ou o objeto original se o <paramref name="condition" /> for <code>false</code>
        /// </returns>
        public static T If<T>(this T obj, bool condition, Func<T, T> func)
        {
            if (condition) return func(obj);

            return obj;
        }

        /// <summary>
        ///     Pode ser usado para realizar uma ação condicionalmente
        ///     em um objeto e retorna o objeto original.
        ///     É útil para chamadas encadeadas no objeto.
        /// </summary>
        /// <param name="obj">Um objeto</param>
        /// <param name="condition">Uma condição</param>
        /// <param name="action">Uma ação que é executada somente se a condição for <code>true</code></param>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <returns>
        ///     Retorna o objeto original.
        /// </returns>
        public static T If<T>(this T obj, bool condition, Action<T> action)
        {
            if (condition) action(obj);

            return obj;
        }

        public static bool IsNotNull<T>(this T item) => item != null;

        public static bool IsNull<T>(this T item) => item == null;

        public static string ToJson(object data) => data == null ? null : JsonConvert.SerializeObject(data);

        public static void BindObjectValue<T>(this T target, T source) where T : class, new()
        {
            var instanceTarget = Activator.CreateInstance(typeof(T)) as T;
            var instanceSource = Activator.CreateInstance(typeof(T)) as T;

            instanceTarget = target;
            instanceSource = source;

            foreach (var propertyTarget in target.GetType().GetProperties())
                foreach (var propertySource in source.GetType().GetProperties())
                {
                    if (propertyTarget.Name != propertySource.Name) continue;

                    var value = propertySource.GetValue(instanceSource, null);
                    propertyTarget.SetValue(instanceTarget, value, null);
                }

            instanceTarget = null;
            instanceSource = null;
        }

        public static void BindObjectValue<T, TProperty>(this T target, T source,
            Expression<Func<T, TProperty>> ignoreProperty = null) where T : class, new()
        {
            var instanceTarget = Activator.CreateInstance(typeof(T)) as T;
            var instanceSource = Activator.CreateInstance(typeof(T)) as T;

            instanceTarget = target;
            instanceSource = source;

            if (ignoreProperty != null)
            {
                var name = GetPropertyName(ignoreProperty);

                foreach (var propertyTarget in target.GetType().GetProperties())
                    foreach (var propertySource in source.GetType().GetProperties())
                        if (propertyTarget.Name == propertySource.Name && propertyTarget.Name != name)
                        {
                            var value = propertySource.GetValue(instanceSource, null);
                            propertyTarget.SetValue(instanceTarget, value, null);
                        }
            }

            instanceTarget = null;
            instanceSource = null;
        }

        public static void BindObjectValue<T>(this T target, T source, string[] propertiesName = null)
            where T : class, new()
        {
            var instanceTarget = Activator.CreateInstance(typeof(T)) as T;
            var instanceSource = Activator.CreateInstance(typeof(T)) as T;

            instanceTarget = target;
            instanceSource = source;

            foreach (var propertyTarget in target.GetType().GetProperties())
                foreach (var propertySource in source.GetType().GetProperties())
                {
                    if (propertiesName != null && (propertyTarget.Name != propertySource.Name ||
                                                   CheckIfExists(propertiesName, propertyTarget.Name)))
                        continue;

                    var value = propertySource.GetValue(instanceSource, null);
                    propertyTarget.SetValue(instanceTarget, value, null);
                }

            instanceTarget = null;
            instanceSource = null;
        }

        private static bool CheckIfExists(IEnumerable<string> propertiesName, string propName)
        {
            var exists = false;

            foreach (var name in propertiesName) exists = name == propName;

            return exists;
        }

        private static string GetPropertyName<TSource, TField>(Expression<Func<TSource, TField>> field)
        {
            if (Equals(field, null)) throw new Exception("Field is required");

            MemberExpression expr = null;

            switch (field.Body)
            {
                case MemberExpression body:
                    expr = body;

                    break;

                case UnaryExpression expression:
                    expr = (MemberExpression)expression.Operand;

                    break;

                default:
                    {
                        const string Format = "Expression '{0}' not supported.";
                        var message = string.Format(Format, field);

                        throw new Exception($"{message} - {nameof(field)}");
                    }
            }

            return expr.Member.Name;
        }

        public static object GetPropertyValue(this object src, string propName) => src.GetType().GetProperty(propName)?.GetValue(src, null);

        public static string? Serialize(this object data, Formatting formatting = Formatting.Indented) =>
            data.IsNotNull()
                ? JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                    Formatting = formatting,
                    NullValueHandling = NullValueHandling.Ignore
                })
                : default;

        public static T? Deserialize<T>(this string data, Formatting formatting = Formatting.Indented) =>
            // var regex_newline = new Regex("(\r\n|\r|\n)");
            // data = regex_newline.Replace(data, "");
            data.IsNotNull()
                ? JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                    Formatting = formatting,
                    NullValueHandling = NullValueHandling.Ignore,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                })
                : default;

        public static TTarget Assign<TSource, TTarget>(this TTarget target, TSource source)
            where TSource : class
            where TTarget : class
        {
            var obj = Serialize(source);

            return target = Deserialize<TTarget>(obj);
        }

        public static TTarget MapTo<TTarget>(this object source)
            where TTarget : class, new()
        {
            try
            {
                var target = new TTarget();
                string obj = null;

                if (!source.Is<string>())
                {
                    obj = Serialize(source);
                    target = Deserialize<TTarget>(obj);
                }
                else
                {
                    target = Deserialize<TTarget>(source.ToString());
                }

                return (TTarget)target;
            }
            catch
            {
                return default;
            }
        }

        public static object GetId(this object src) => src.GetType().GetProperty("Id")?.GetValue(src, null);

        public static bool CompareId<T>(this T src, T obj)
            where T : class, new()
        {
            if (src == null || obj == null) return false;

            var idSrc = src.GetType().GetProperty("Id")?.GetValue(src, null);
            var idObj = src.GetType().GetProperty("Id")?.GetValue(obj, null);

            if (idSrc == null || idObj == null) return false;

            if (idSrc.GetType() != idObj.GetType()) return false;

            return idSrc == idObj;
        }

        public static bool Is<T>(this object data) => data?.GetType() == typeof(T);

        /// <summary>
        ///     Determina se uma instância deste tipo pode ser atribuída a
        ///     uma instância do <typeparamref name="TTarget"></typeparamref>.
        ///     Usa internamente <see cref="M:System.Type.IsAssignableFrom(System.Type)" />.
        /// </summary>
        /// <typeparam name="TTarget">Tipo de destino</typeparam>
        /// (como reverso).
        public static bool IsAssignableTo<TTarget>(this object type)
        {


            return type.GetType().IsAssignableTo(typeof(TTarget));
        }

        public static bool IsAssignableFrom<TTarget>(this object type)
        {


            return type.GetType().IsAssignableFrom(typeof(TTarget));
        }

        public static TItem Clone<TItem>(this TItem item)
        {
            var ret = item;
            if (item != null)
            {
                var type = item.GetType();
                if (typeof(ICloneable).IsAssignableFrom(type))
                {
                    var clv = type.GetMethod("Clone")?.Invoke(type, null);
                    if (clv != null) ret = (TItem)clv;
                }

                if (type.IsClass)
                {
                    ret = Activator.CreateInstance<TItem>();
                    var valType = ret?.GetType();
                    if (valType != null)
                    {
                        type.GetFields().ToList().ForEach(f =>
                        {
                            var v = f.GetValue(item);
                            valType.GetField(f.Name)?.SetValue(ret, v);
                        });
                        type.GetProperties().ToList().ForEach(p =>
                        {
                            if (p.CanWrite)
                            {
                                var v = p.GetValue(item);
                                valType.GetProperty(p.Name)?.SetValue(ret, v);
                            }
                        });
                    }
                }
            }

            return ret;
        }


        public static void Reset<TItem>(this TItem source) where TItem : class, new()
        {
            var v = new TItem();
            foreach (var pi in source.GetType().GetProperties()) pi.SetValue(source, v.GetType().GetProperty(pi.Name).GetValue(v));
        }

        private static readonly JsonSerializerSettings setting2 = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };


        public static string ObjectToJsonIndented(this object obj) => JsonConvert.SerializeObject(obj, setting2);


        public static string ObjectToJson(this object obj) => JsonConvert.SerializeObject(obj);


        public static object JsonToObject(this string jsonString, object obj) => JsonConvert.DeserializeObject(jsonString, obj.GetType());

        public static string ToString2<T>(this List<T> l)
        {
            var retVal = string.Empty;
            foreach (var item in l)
                retVal += string.Format("{0}{1}", string.IsNullOrEmpty(retVal) ? "" : ", ",
                    item);
            return string.IsNullOrEmpty(retVal) ? "{}" : "{ " + retVal + " }";
        }

        public static int ToInt(this object obj)
        {
            try
            {
                return obj.IsNum() ? Convert.ToInt32(obj) : 0;
            }
            catch (Exception)
            {
                return obj.IsNum() ? (int)Convert.ToDouble(obj) : 0;
            }
        }

        public static string Format2Num(this object obj, int PadLeft = 8, int num = 2) => obj.IsNum() ? Convert.ToDecimal(obj).ToString($"n{num}").PadLeft(PadLeft, ' ') : "".PadLeft(PadLeft, ' ');

        public static decimal ToDecimal(this object obj) => obj.IsNum() ? Convert.ToDecimal(obj) : 0;

        public static string FormatNum2(this object obj, int num = 2) => obj.ToDecimal().ToString($"f{num}");

        public static decimal ToDecimal(this object obj, int num) => decimal.Round(obj.ToDecimal(), num);

        public static double ToDbl(this object obj) => obj.IsNum() ? Convert.ToDouble(obj) : 0;

        public static bool IsNum(this object t) => t == null ? false : double.TryParse(t.ToString(), out _);

        public static bool IsNumber(this Type t)
        {
            var targetType = Nullable.GetUnderlyingType(t) ?? t;
            var check = targetType == typeof(byte) ||
                        targetType == typeof(sbyte) ||
                        targetType == typeof(int) ||
                        targetType == typeof(long) ||
                        targetType == typeof(short) ||
                        targetType == typeof(float) ||
                        targetType == typeof(double) ||
                        targetType == typeof(decimal);
            return check;
        }


        public static bool IsDateTime(this Type t)
        {
            var targetType = Nullable.GetUnderlyingType(t) ?? t;
            var check = targetType == typeof(DateTime) ||
                        targetType == typeof(DateTimeOffset);
            return check;
        }

        public static Dictionary<string, decimal?> GetDisplayNameDic<T>(T t)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var dic = new Dictionary<string, decimal?>();
            foreach (var p in properties)
            {
                var name = p.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                var value = t.GetType().GetProperty(p.Name).GetValue(t, null);
                dic.Add(name, Convert.ToDecimal(value));
            }

            return dic;
        }


        public static string GetDisplayNameII<TItem>(this TItem source, string pname, string defaultValue = "")
        {
            var type = typeof(TItem);
            var properties = type.GetProperties();
            foreach (var p in properties.Where(a => a.Name == pname))
            {
                var pAttribute = p.GetCustomAttribute<DisplayNameAttribute>();
                var name = p.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? defaultValue;
                return name;
            }

            return defaultValue;
        }

        public static bool ContainProperty(this object instance, string propertyName)
        {
            if (instance != null && !string.IsNullOrEmpty(propertyName))
            {
                var _findedPropertyInfo = instance.GetType().GetProperty(propertyName);
                return _findedPropertyInfo != null;
            }

            return false;
        }

#nullable enable
        public static object? GetIdentityKey<TItem>(this TItem instance, string? propertyName)
        {
            if (instance != null && !string.IsNullOrEmpty(propertyName))
            {
                var propertyInfo = instance.GetType().GetProperty(propertyName);
                if (propertyInfo == null) return null;

                var value = propertyInfo.GetValue(instance, null);
                return value;
            }

            return null;
        }
#nullable restore

        public static object FieldSetValue<TItem>(this TItem instance, string propertyName, object value)
        {
            if (instance != null && !string.IsNullOrEmpty(propertyName))
            {
                var propertyInfo = instance.GetType().GetProperty(propertyName);
                propertyInfo.SetValue(instance, value);
                return value;
            }

            return null;
        }

        public static object GetField<TItem>(this TItem instance, string propertyName)
        {
            if (instance != null && !string.IsNullOrEmpty(propertyName))
            {
                var propertyInfo = instance.GetType().GetProperty(propertyName);
                return propertyInfo;
            }

            return null;
        }


        public static bool IsEmail(this string _input) => Regex.IsMatch(_input, @"^\\w+@\\w+\\.\\w+$");


        public static string Quot(this string _input, string _quot) => _quot + _input + _quot;

        public static string AddNewLineText(this string _input, string _text, bool addNewLine = true, string _quot = "", [CallerMemberName] string propertyName = "")
        {
            _input += $"{(addNewLine ? Environment.NewLine : "")}{_text}{_quot}";
            return _input;
        }


        public static Dictionary<string, string> ObjectToMap(this object obj)
        {
            var map = new Dictionary<string, string>(); //

            var t = obj.GetType();

            var pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var p in pi)
            {
                var m = p.GetGetMethod();
                if (m != null && m.IsPublic)
                    if (m.Invoke(obj, new object[] { }) != null)
                        map.Add(p.Name, m.Invoke(obj, new object[] { }).ToString());
            }

            return map;
        }

        public static Dictionary<string, object> EntityToMap(this object obj) => JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(obj));

        [DataContract]
        public class proc1
        {
            [DataMember] public string parameter1 { get; set; } = Guid.NewGuid().ToString();

            public Dictionary<string, object> ToDict(proc1 proc1) => JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(proc1));
        }
    }

    public static class ObjectUtils
    {
        public static void AddNewLine(ref string _input, string _text, bool addNewLine = true)
        {
            if (!string.IsNullOrEmpty(_text)) _input += $"{(addNewLine ? Environment.NewLine : "")}{_text}";
        }

        public static void AddNewBlankLine(ref string _input) => _input += Environment.NewLine;
    }

    public static class StringExt
    {
        public static bool IsNumeric(this string text) => double.TryParse(text, out _);

        public static decimal ToDecimal(this string t, decimal defaultValue = 0m)
        {
            try
            {
                var x = t.IsNumeric() ? Convert.ToDecimal(t) : defaultValue;
                return x;
            }
            catch
            {
            }

            return defaultValue;
        }

        public static double ToDouble(this string t, double defaultValue = 0d)
        {
            try
            {
                var x = t.IsNumeric() ? Convert.ToDouble(t) : defaultValue;
                return x;
            }
            catch
            {
            }

            return defaultValue;
        }
    }
}
