using System;
using System.Xml;

namespace qBtAssist
{
    class XML
    {

        private static XmlDocument xmlFile = new XmlDocument();

        /// <summary>
        /// Loads XML file.
        /// </summary>
        /// <param name="string">Path and name of the xml file.</param>
        public static void LoadXML(string file)
        {
            try
            {
                xmlFile.Load(file);
            }
            catch (Exception)
            {
                Console.WriteLine("Error loading XML file.");
            }
        }

        /// <summary>
        /// Parses XML file.
        /// </summary>
        /// <returns>String array made of the contents of the XML file.</returns>
        public static string[] ParseXML()
        {
            try
            {
                XmlElement root = xmlFile.DocumentElement;
                XmlNodeList nodes = root.GetElementsByTagName("Process");

                string[] xmlData = new string[5 + nodes.Count];
                xmlData[0] = xmlFile.SelectSingleNode("//Login/Host/text()").Value;
                xmlData[1] = xmlFile.SelectSingleNode("//Login/Username/text()").Value;
                xmlData[2] = xmlFile.SelectSingleNode("//Login/Password/text()").Value;
                xmlData[3] = xmlFile.SelectSingleNode("//Setup/UploadLimit/text()").Value;
                xmlData[4] = xmlFile.SelectSingleNode("//Setup/DownloadLimit/text()").Value;

                for (int i = 5; i < (nodes.Count + 5); i++)
                {
                    xmlData[i] = nodes[i-5].InnerXml;
                }
                return xmlData;
            }
            catch (Exception)
            {
                Console.WriteLine("Error parsing XML file.");
                return null;
            }

        }


    }
}
