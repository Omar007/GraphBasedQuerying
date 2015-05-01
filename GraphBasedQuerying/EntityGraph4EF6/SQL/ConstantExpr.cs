using System.Text;

namespace EntityGraph4EF6.SQL
{
    internal class ConstantExpr : UnaryExpr
    {
        public object Value { get; private set; }

        public ConstantExpr(object value)
        {
            //Encapsulate string in quotes!
            if (value is string)
            {
                value = "\'" + (string)value + "\'";
            }
            Value = value;
        }

        public override void AppendToStringBuilder(StringBuilder sb)
        {
            sb.Append(Value.ToString());
        }
    }
}
