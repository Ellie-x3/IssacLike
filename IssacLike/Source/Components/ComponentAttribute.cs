using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssacLike.Source.Components {
    [AttributeUsage(AttributeTargets.Field)]
    internal class ComponentAttribute : Attribute {
        public Type ComponentType { get;}

        public ComponentAttribute(Type componentType) {
            ComponentType = componentType;
        }
    }
}
