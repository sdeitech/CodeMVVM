using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ViewModel
{
    public class FileManagement
    {
        public static string CreateXML(object obj)
        {
            var Settinglocation = @"C:\Demo\";
            var filename = @"Settings.xml";


            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            string root = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = string.Format(Settinglocation + filename);
            if (!Directory.Exists(Settinglocation))
            {
                Directory.CreateDirectory(Settinglocation);
            }

            // Creates a stream whose backing store is memory. 
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, obj);
                xmlStream.Position = 0;
                //Loads the XML document from the specified string.
                xmlDoc.Load(xmlStream);
                xmlDoc.Save(fileName);

                return xmlDoc.InnerXml;
            }
        }

        
    }
}
