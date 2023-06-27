using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SaveableData : MonoBehaviour
{
    public string uniqueName;

    // Start is called before the first frame update
    public virtual void Start()
    {
        loadFromFile();
    }

    public void saveToFile()
    {
        Debug.Log("ToJSON");
        string s = JsonUtility.ToJson(this);
        FileManager.WriteToFile(uniqueName + ".json" , s);
        Debug.Log(s);
    }

    public void loadFromFile()
    {
        Debug.Log("FromJSON");
        string s = "";
        FileManager.LoadFromFile(uniqueName + ".json", out s);
        JsonUtility.FromJsonOverwrite(s, this);
        Debug.Log(s);
    }

}

