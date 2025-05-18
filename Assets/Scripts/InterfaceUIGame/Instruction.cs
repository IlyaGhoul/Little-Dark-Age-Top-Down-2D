using UnityEngine;
using System.Diagnostics;
using System;

public class Instruction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private string _filePath = @"D:\StudioGame\W\�����������.docx";

    [SerializeField] private GameObject _gameObject;

    public void OpenWordFile()
    {
        // ���������, ���������� �� ����
        if (System.IO.File.Exists(_filePath))
        {
            // ��������� ���� � Word
            Process.Start(new ProcessStartInfo(_filePath) { UseShellExecute = true });
        }
        else
        {
            Console.WriteLine("���� �� ������!");
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
