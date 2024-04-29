<<<<<<< HEAD:Assets/UI/CrewManagementScreen.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewManagementScreen : MonoBehaviour
{
static Dictionary<string, CrewCard> crew = new Dictionary<string, CrewCard>();
  static CrewCard GetMember(string name) {
     if (crew.ContainsKey("name")){
       return crew[name];}
     else return null;
  }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewManagementScreen : MonoBehaviour
{
  //Script that manages the crew interfaces in the manage screen
static Dictionary<string, CrewCard> crew = new Dictionary<string, CrewCard>();
  static CrewCard GetMember(string name) {
     if (crew.ContainsKey("name")){
       return crew[name];}
     else return null;
  }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
>>>>>>> 1d2f3e3bfc47ccf7de0cbf438d6fcc3b50b5f0b6:CrewManagementScreen.cs
