
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace W
{

    public static class Creator
    {
        /// <summary>
        /// 序列化设置
        /// </summary>
        private static JsonSerializerSettings Settings { get; } = new JsonSerializerSettings {
            MaxDepth = 24,
            // ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,

            MissingMemberHandling = MissingMemberHandling.Ignore,

            MetadataPropertyHandling = MetadataPropertyHandling.Default, // 序列化自动生成 attribute property getter setter
            ReferenceLoopHandling = ReferenceLoopHandling.Error, // 循环依赖

            Formatting = Formatting.None, // 不缩进
            DefaultValueHandling = DefaultValueHandling.Ignore, // 不序列化默认值
            TypeNameHandling = TypeNameHandling.Auto, // 只有 object 标出类型
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple, // 短 assembly name
        };

        public static byte[] passwordBytes = new byte[] { 1, 9, 9, 7, 1, 1, 1, 1 };

        public static byte[] CompressAndEncrypt(byte[] bytes) {
            // return AES.Encrypt(Compression.Compress(bytes), passwordBytes);
            return Compression.Compress(bytes);
        }
        public static byte[] DecryptAndDepress(byte[] bytes) {
            // return Compression.Decompress(AES.Decrypt(bytes, passwordBytes));
            return Compression.Decompress(bytes);
        }

        public static string Read(string path, System.Text.Encoding encoding, bool useCompressionAndEncryption) {
            if (useCompressionAndEncryption) {
                byte[] raw = File.ReadAllBytes(path);
                byte[] bytes = Creator.DecryptAndDepress(raw);
                return encoding.GetString(bytes);
            } else {
                return File.ReadAllText(path, encoding);
            }
        }
        public static void Write(string path, string json, System.Text.Encoding encoding, bool useCompressionAndEncryption) {
            if (useCompressionAndEncryption) {
                byte[] bytes = encoding.GetBytes(json);
                byte[] raw = Creator.CompressAndEncrypt(bytes);
                File.WriteAllBytes(path, raw);
            } else {
                File.WriteAllText(path, json, encoding);
            }
        }
        public static void WriteSafer(string[] pathes, string[] jsons, System.Text.Encoding encoding, bool useCompressionAndEncryption) {
            A.Assert(pathes.Length == jsons.Length);
            if (useCompressionAndEncryption) {
                byte[][] raws = new byte[pathes.Length][];
                for (int i = 0; i < pathes.Length; i++) {
                    byte[] bytes = encoding.GetBytes(jsons[i]);
                    byte[] raw = Creator.CompressAndEncrypt(bytes);
                    raws[i] = raw;
                }
                for (int i = 0; i < pathes.Length; i++) {
                    File.WriteAllBytes(pathes[i], raws[i]);
                }
            } else {
                for (int i = 0; i < pathes.Length; i++) {
                    File.WriteAllText(pathes[i], jsons[i], encoding);
                }
            }
        }


        /// <summary>
        /// 反序列化 json
        /// </summary>
        public static object DeserializeFromJson(Type type, string json) {
            A.Assert(Ty.Is(type, typeof(ICreatable)));
            ICreatable creatable = JsonConvert.DeserializeObject(json, type, Settings) as ICreatable;
            A.Assert(creatable != null);
            return creatable;
        }
        public static T DeserializeFromJson<T>(string json) where T : ICreatable {
            T creatable = JsonConvert.DeserializeObject<T>(json, Settings);
            A.Assert(creatable != null);
            return creatable;
        }

        /// <summary>
        /// 序列化 json
        /// </summary>
        public static string SerializeToJson<T>(T obj) {
            return JsonConvert.SerializeObject(obj, Settings);
        }

        /// <summary>
        /// 根据 System.Type 动态创建物品
        /// </summary>
        public static object __CreateData(Type type) {
            ICreatable creatable = Activator.CreateInstance(type) as ICreatable;
            creatable.OnCreate();
            return creatable;
        }
        public static T __CreateData<T>() where T : class, ICreatable {
            T result = Activator.CreateInstance<T>();
            result.OnCreate();
            return result;

        }
    }


    /// <summary>
    /// 为接口绑定一个实现类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class implAttribute : Attribute
    {

        public readonly Type BindType;
        public readonly long Count = 1;

        private readonly Type itemType = typeof(Item);
        public implAttribute(Type type) {
            A.Assert(Ty.Is(type, itemType), () => $"implAttribute 参数异常 {type.Name} 不是 Item");
            BindType = type;
        }

        public implAttribute(Type type, long count = 1) {
            A.Assert(Ty.Is(type, itemType), () => $"implAttribute 参数异常 {type.Name} 不是 Item");
            BindType = type;
            Count = count;
        }

        public implAttribute(long count = 1) {
            Count = count;
        }


        public static Item Of(Type type) {
            A.Assert(!Ty.Is(type, typeof(Item)), () => $"how can this be a interface {(type == null ? "null" : type.Name)}");

            implAttribute i = Attr.Get<implAttribute>(type);

            Item item;
            if (i != null) {
                if (i.BindType != null) {
                    item = Creator.__CreateData(i.BindType) as Item;
                } else {
                    item = Creator.__CreateData<Item>();
                }
                if (i.Count != 0) item.Quantity = i.Count;
            } else {
                item = Creator.__CreateData<Item>();
                item.Quantity = 1;
            }

            item.type = type;

            return item;
        }
    }
}
