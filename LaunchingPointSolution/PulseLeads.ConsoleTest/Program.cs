using System;
using System.Data;
using System.IO;
using System.Net;
using System.Xml;

namespace PulseLeads.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Clients\FocusIT\Github\Pulse\LaunchingPointSolution\PulseLeads.ConsoleTest\Sample1_V50.xml");

            //
            //doc.Load(@"C:\Clients\FocusIT\Github\Pulse\LaunchingPointSolution\PulseLeads.ConsoleTest\Sample1_v60.xml");
            string xmlcontents = doc.InnerXml;

            DataSet ds = new DataSet();
            XmlNodeReader xmlReader = new XmlNodeReader(doc);
            ds.ReadXml(xmlReader, XmlReadMode.Auto);

            PostXMLTransaction("http://localhost:57523/Zillow/ContactAPI.aspx", doc);
        }

        public static XmlDocument PostXMLTransaction(string url, XmlDocument xmldoc)
        {
            //Declare XMLResponse document
            XmlDocument xml_response = null;

            //Declare an HTTP-specific implementation of the WebRequest class.

            //Declare an HTTP-specific implementation of the WebResponse class
            HttpWebResponse http_web_response = null;

            //Declare a generic view of a sequence of bytes
            Stream request_stream = null;
            Stream response_stream = null;

            //Declare XMLReader
            XmlTextReader xml_reader;

            //Creates an HttpWebRequest for the specified URL.
            HttpWebRequest http_web_request = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                //---------- Start HttpRequest 

                //Set HttpWebRequest properties
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmldoc.InnerXml);
                http_web_request.Method = "POST";
                http_web_request.ContentLength = bytes.Length;
                http_web_request.ContentType = "text/xml; encoding='utf-8'";

                //Get Stream object 
                request_stream = http_web_request.GetRequestStream();

                //Writes a sequence of bytes to the current stream 
                request_stream.Write(bytes, 0, bytes.Length);

                //Close stream
                request_stream.Close();

                //---------- End HttpRequest

                //Sends the HttpWebRequest, and waits for a response.
                http_web_response = (HttpWebResponse)http_web_request.GetResponse();

                //---------- Start HttpResponse
                if (http_web_response.StatusCode == HttpStatusCode.OK)
                {
                    //Get response stream 
                    response_stream = http_web_response.GetResponseStream();

                    //Load response stream into XMLReader
                    TextReader tr = new StreamReader(response_stream);
                    string return_value = tr.ReadToEnd();
                    tr.Close();
                }

                //Close HttpWebResponse
                http_web_response.Close();
            }
            catch (WebException we)
            {
                //TODO: Add custom exception handling
                throw new Exception(we.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //Close connections
                request_stream.Close();
                response_stream.Close();
                http_web_response.Close();

                //Release objects
                xml_reader = null;
                request_stream = null;
                response_stream = null;
                http_web_response = null;
                http_web_request = null;
            }

            //Return
            return xml_response;
        }
    }
}
