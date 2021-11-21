using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Worker.Order.Read.Entity;
using Worker.Order.Read.Repository.Interfaces;
using Worker.Order.Read.Service.Interfaces;

namespace Worker.Order.Read.Service
{
    public class FileService : IFileService
    {
        private string CurrentFile { get; set; }

        private readonly string FilePath;

        private readonly string FileReadPath;

        private readonly string FileErrorPath;

        private readonly ILogsRepository _logsRepository;

        public FileService(ILogsRepository logsRepository)
        {
            FilePath = @"C:\XMLFiles\NEW\";

            FileReadPath = @"C:\XMLFiles\PROCESSED\";

            FileErrorPath = @"C:\XMLFiles\ERROR\";

            _logsRepository = logsRepository;
        }

        public int CheckFile()
        {
            try
            {
                foreach (string file in Directory.GetFiles(FilePath, "*.xml"))
                {
                    if (File.Exists(file))
                    {
                        var logRead = _logsRepository.LogRead(file);

                        return logRead;
                    }
                }

                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Entity.Order ReadFile(int logRead)
        {
            try
            {
                var order = new Entity.Order();

                foreach (string file in Directory.GetFiles(FilePath, "*.xml"))
                {
                    CurrentFile = file;

                    Address addressShipping = null;
                    Address addressBilling = null;
                    var items = new List<Item>();

                    string name = string.Empty, street = string.Empty, city = string.Empty, state = string.Empty, country = string.Empty;
                    int zip = 0;

                    string productName = string.Empty, comment = string.Empty;
                    int quantity = 0;
                    double price = 0;

                    using (XmlReader xml = XmlReader.Create(file))
                    {
                        while (xml.Read())
                        {
                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "OrderNumber")
                                order.OrderNumber = int.Parse(xml.ReadElementContentAsString());

                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "OrderDate")
                                order.OrderDate = DateTime.Parse(xml.ReadElementContentAsString());

                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "DeliveryNotes")
                                order.DeliveryNotes = xml.ReadElementContentAsString();

                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "Name")
                                name = xml.ReadElementContentAsString();

                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "Street")
                                street = xml.ReadElementContentAsString();

                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "City")
                                city = xml.ReadElementContentAsString();

                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "State")
                                state = xml.ReadElementContentAsString();

                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "Zip")
                                zip = int.Parse(xml.ReadElementContentAsString());

                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "Country")
                                country = xml.ReadElementContentAsString();

                            if (name != string.Empty && street != string.Empty && city != string.Empty && state != string.Empty && country != string.Empty && zip != 0)
                            {
                                if (addressShipping == null)
                                {
                                    addressShipping = new Address
                                    {
                                        Name = name,
                                        Street = street,
                                        City = city,
                                        State = state,
                                        Country = country,
                                        Zip = zip
                                    };

                                    name = string.Empty;
                                    street = string.Empty;
                                    city = string.Empty;
                                    state = string.Empty;
                                    country = string.Empty;
                                    zip = 0;
                                }
                                else
                                {
                                    addressBilling = new Address
                                    {
                                        Name = name,
                                        Street = street,
                                        City = city,
                                        State = state,
                                        Country = country,
                                        Zip = zip
                                    };

                                    name = string.Empty;
                                    street = string.Empty;
                                    city = string.Empty;
                                    state = string.Empty;
                                    country = string.Empty;
                                    zip = 0;
                                }
                            }

                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "ProductName")
                                productName = xml.ReadElementContentAsString();

                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "Quantity")
                                quantity = int.Parse(xml.ReadElementContentAsString());

                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "USPrice")
                                price = double.Parse(xml.ReadElementContentAsString());

                            if (xml.NodeType == XmlNodeType.Element && xml.Name == "Comment")
                                comment = xml.ReadElementContentAsString();

                            if (productName != string.Empty && comment != string.Empty && quantity != 0 && price != 0)
                            {
                                var item = new Item
                                {
                                    ProductName = productName,
                                    Quantity = quantity,
                                    Price = price,
                                    Comment = comment
                                };

                                items.Add(item);

                                productName = string.Empty;
                                comment = string.Empty;
                                quantity = 0;
                                price = 0;
                            }
                        }

                        order.Shipping = addressShipping;
                        order.Billing = addressBilling;
                        order.Items = items;

                        //CurrentFile = file;
                    }
                }

                return order;
            }
            catch (Exception e)
            {
                MoveFile(true);

                _logsRepository.LogRead(e.Message.Replace("'", ""), logRead);

                throw;
            }
        }

        public void MoveFile(bool isError)
        {
            try
            {
                if (!isError)
                {
                    string fileNameOrigin = CurrentFile.Split("\\").GetValue(3).ToString();

                    var fileName = fileNameOrigin + "." + Convert.ToString(DateTime.Now.ToString("yyyy''MM''dd'T'HH''mm''ss"));

                    File.Move(CurrentFile, Path.Combine(FileReadPath, fileName));
                }
                else
                {
                    string fileNameOrigin = CurrentFile.Split("\\").GetValue(3).ToString();

                    var fileName = fileNameOrigin + "." + Convert.ToString(DateTime.Now.ToString("yyyy''MM''dd'T'HH''mm''ss"));

                    File.Move(CurrentFile, Path.Combine(FileErrorPath, fileName));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
