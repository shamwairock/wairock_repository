using System;
using System.Collections.Generic;
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
            var htmlFile = @"K:\Development\github\wairock_repository\XsltTester\Index.html";

            XmlDocument xmlFile = new XmlDocument();
            xmlFile.Load(@"XMLFile1.xml");
            //var xsltString = 
            XmlDocument xsltFile = new XmlDocument();
            xsltFile.Load(@"XSLTFile1.xslt");

            var htmlFileOutput = Transform(xmlFile.InnerXml, xsltFile.InnerXml);

            System.IO.File.WriteAllText(htmlFile, htmlFileOutput);

            System.Diagnostics.Process.Start(htmlFile);

        }

        private static string Transform(string inputXml, string xsltString)
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
