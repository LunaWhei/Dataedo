using System;
using System.Collections.Generic;
using System.IO;
using ConsoleApp.Models;

namespace ConsoleApp
{
    public class DataReader
    {
        public void ImportAndPrintData(string fileToImport, bool printData = true)
        {
            List<ImportedObject> importedObjectList = new List<ImportedObject>();

            var streamReader = new StreamReader(fileToImport);

            List<string> importedLines = new List<string>();
            while (!streamReader.EndOfStream)
            {
                importedLines.Add(streamReader.ReadLine());
            }

            foreach (var item in importedLines)
            {
                try
                {
                    var values = item.Split(';');
                    ImportedObject importedObject = new ImportedObject();
                    importedObject.type = values[0];
                    importedObject.name = values[1];
                    importedObject.schema = values[2];
                    importedObject.parentName = values[3];
                    importedObject.parentType = values[4];
                    importedObject.dataType = values[5];
                    importedObject.isNullable = values[6];
                    importedObjectList.Add(importedObject);
                }
                catch
                {
                    Console.WriteLine("Some data is missing - We should implement Exception later on");
                }
            }
            // clear and correct imported data
            foreach (var importedObject in importedObjectList)
            {
                try
                {
                    importedObject.type = importedObject.type.Trim().Replace(" ", "").Replace(Environment.NewLine, "").ToUpper();
                    importedObject.name = importedObject.name.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                    importedObject.schema = importedObject.schema.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                    importedObject.parentName = importedObject.parentName.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                    importedObject.parentType = importedObject.parentType.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                }
                catch
                {
                    Console.WriteLine("Incorrect data in List - We should implement Exception later on");
                }
            }
            //assign number of children
            foreach (ImportedObject item in importedObjectList)
            {
                foreach (var impObj in importedObjectList)
                {
                    if (impObj.parentType == item.type)
                    {
                        if (impObj.parentName == item.name)
                        {
                            item.numberOfChildren += 1;
                        }
                    }
                }
            }
            foreach (var database in importedObjectList)
            {
                if (database.type == "DATABASE")
                {
                    Console.WriteLine($"Database '{database.name}' ({database.numberOfChildren} tables)");

                    // print all database's tables
                    foreach (var table in importedObjectList)
                    {
                        if (table.parentType.ToUpper() == database.type)
                        {
                            if (table.parentName == database.name)
                            {
                                Console.WriteLine($"\tTable '{table.schema}.{table.name}' ({table.numberOfChildren} columns)");

                                // print all table's columns
                                foreach (var column in importedObjectList)
                                {
                                    if (column.parentType.ToUpper() == table.type)
                                    {
                                        if (column.parentName == table.name)
                                        {
                                            Console.WriteLine($"\t\tColumn '{column.name}' with {column.dataType} data type {(column.isNullable == "1" ? "accepts nulls" : "with no nulls")}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.ReadLine();
        }
    }


}
