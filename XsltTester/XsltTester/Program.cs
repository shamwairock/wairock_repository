using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace XsltTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var html = string.Empty;
            var inputXml = File.ReadAllText(@"XMLFile1.xml");
            var xsltString = File.ReadAllText(@"XSLTFile1.xslt");

            html = TransformXMLToHTML(inputXml, xsltString);

            File.WriteAllText(@"HTMLPage1.html", html);

            Process.Start(@"HTMLPage1.html");
            
            Console.ReadKey();
        }

        private static string TransformXMLToHTML(string inputXml, string xsltString)
        {
            XslCompiledTransform transform = new XslCompiledTransform();
            using (XmlReader reader = XmlReader.Create(new StringReader(xsltString)))
            {
                transform.Load(reader);
            }
            StringWriter results = new StringWriter();
            using (XmlReader reader = XmlReader.Create(new StringReader(inputXml)))
            {
                transform.Transform(reader, null, results);
            }
            return results.ToString();
        }
    }
}
