using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace munimji.core.lib.coding {
    public static class ReflectUtils {
        public static readonly Object Obj = new object();

        public static XmlReader GetXmlResource(Assembly assembly, string resourceQualifiedName) {
            var stream = assembly.GetManifestResourceStream(resourceQualifiedName);
            if (stream == null) {
                throw new NullReferenceException(String.Format("Resource:'{0}' not found in Assembly: {1}",
                                                               resourceQualifiedName, assembly.FullName));
            }
            return XmlReader.Create(stream);
        }

        public static bool InheritsFrom<T>(this Type type) where T : class {
            return type.InheritsFrom(typeof (T));
        }

        public static bool InheritsFrom(this Type type, Type otherType) {
            return type.IsSubclassOf(otherType);
        }

        public static void Set<TObj, TOut>(this object obj, Expression<Func<TObj, TOut>> expression, TOut value) {
            Set(obj, (PropertyInfo) Meta(expression), value);
        }

        public static void Set<T>(this object obj, string properyName, T value) {
            var pi = obj.GetType().GetProperty(properyName);
            Set(obj, pi, value);
        }

        private static void Set<T>(this object obj, PropertyInfo propertyInfo, T value) {
            propertyInfo.SetValue(obj, value, null);
        }


        public static MemberInfo Meta<TObj, TOut>(Expression<Func<TObj, TOut>> expression) {
            MemberExpression me;

            if (expression.Body is UnaryExpression) {
                var ue = (UnaryExpression) expression.Body;
                me = (MemberExpression) ue.Operand;
            }
            else if (expression.Body is MemberExpression) {
                me = (MemberExpression) expression.Body;
            }
            else {
                throw new NotSupportedException("This expression is not supported");
            }
            return me.Member;
        }
        public static MemberInfo Meta<TObj>(Expression<Func<TObj, object>> expression)
        {
            MemberExpression me;

            if (expression.Body is UnaryExpression)
            {
                var ue = (UnaryExpression)expression.Body;
                me = (MemberExpression)ue.Operand;
            }
            else if (expression.Body is MemberExpression)
            {
                me = (MemberExpression)expression.Body;
            }
            else
            {
                throw new NotSupportedException("This expression is not supported");
            }
            return me.Member;
        }

        /// <summary>
        ///   Reflection operation for Static members
        ///   http://weblogs.asp.net/bleroy/archive/2009/09/17/fun-with-c-4-0-s-dynamic.aspx
        /// </summary>
        public class Dynamic : DynamicObject {
            private readonly Type type;

            private const BindingFlags Flags =
                BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public;

            public Dynamic(Type type) {
                this.type = type;
            }

            // Handle static properties
            public override bool TryGetMember(GetMemberBinder binder, out object result) {
                var prop = type.GetProperty(binder.Name, Flags);
                if (prop == null) {
                    result = null;
                    return false;
                }

                result = prop.GetValue(null, null);
                return true;
            }

            // Handle static methods
            public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) {
                var method = type.GetMethod(binder.Name, Flags);
                if (method == null) {
                    result = null;
                    return false;
                }

                result = method.Invoke(null, args);
                return true;
            }
        }
    }
}