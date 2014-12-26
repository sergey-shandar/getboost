using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace builder.Codeplex
{
    public interface IDoc
    {
        void Write(TextWriter writer);
    }

    public sealed class H1 : IDoc
    {
        private string Value;

        public H1(string value)
        {
            Value = value;
        }

        public void Write(TextWriter writer)
        {
            writer.Write("! ");
            writer.WriteLine(Value);
        }
    }

    public sealed class A: IDoc
    {
        private string Text;
        private string Url;

        public A(string text, string url)
        {
            Text = text;
            Url = url;
        }

        public void Write(TextWriter writer)
        {
            writer.Write("[url:");
            writer.Write(Text);
            writer.Write("|");
            writer.Write(Url);
            writer.Write("]");
        }
    }

    public abstract class DocList: IDoc
    {
        protected List<IDoc> list = new List<IDoc>();

        public virtual void Write(TextWriter writer)
        {
            foreach (var doc in list)
            {
                doc.Write(writer);
            }
        }
    }

    public sealed class Text: IDoc
    {
        readonly string Value;
 
        public Text(string value)
        {
            Value = value;
        }

        public void Write(TextWriter writer)
        {
            if (Value.Contains('_'))
            {
                writer.Write("{\"");
                writer.Write(Value);
                writer.Write("\"}");
            }
            else
            {
                writer.Write(Value);
            }
        }
    }

    public sealed class List: DocList
    {
        public List this[Text text]
        {
            get 
            {
                list.Add(text);
                return this;
            }
        }

        public List this[A a]
        {
            get
            {
                list.Add(a);
                return this;
            }
        }

        public override void Write(TextWriter writer)
        {
            writer.Write("* ");
            base.Write(writer);
            writer.WriteLine();
        }
    }

    class T
    {
        public static H1 H1(string value)
        {
            return new H1(value);
        }

        public static A A(string text, string url)
        {
            return new A(text, url);
        }

        public static List List
        {
            get { return new List(); }
        }

        public static Text Text(string value)
        {
            return new Text(value);
        }
    }

    sealed class Doc: DocList
    {
        public Doc this[H1 h1]
        {
            get
            {
                list.Add(h1);
                return this;
            }
        }

        public Doc this[List list]
        {
            get
            {
                this.list.Add(list);
                return this;
            }
        }
    }
}
