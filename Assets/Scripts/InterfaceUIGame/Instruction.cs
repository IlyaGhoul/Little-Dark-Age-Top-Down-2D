using UnityEngine;
using System.Diagnostics;
using System;

public class Instruction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private string _filePath = @"D:\StudioGame\W\Разработчик.docx";

    [SerializeField] private GameObject _gameObject;

    public void OpenWordFile()
    {
        // Проверяем, существует ли файл
        if (System.IO.File.Exists(_filePath))
        {
            // Открываем файл в Word
            Process.Start(new ProcessStartInfo(_filePath) { UseShellExecute = true });
        }
        else
        {
            Console.WriteLine("Файл не найден!");
        }
    }


    public void SetActiveTrue()
    {
        _gameObject.SetActive(true);
    }

    public void SetActiveFalse()
    {
        _gameObject.SetActive(false);
    }
}
