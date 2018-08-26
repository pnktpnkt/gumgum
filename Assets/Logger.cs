using System;
using System.IO;

public class MyLogger
{
    private string filePath;

	public MyLogger(int index)
	{
        filePath = $@"C:\Desktop\data{index}.txt";
	}

    public void log(string text)
    {
        File.AppendAllText(filePath, text+Environment.NewLine);
    }
}
